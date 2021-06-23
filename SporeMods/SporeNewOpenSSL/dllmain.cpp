//
// SporeServer - https://github.com/Rosalie241/SporeServer
//  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License version 3.
//  You should have received a copy of the GNU Affero General Public License
//  along with this program. If not, see <https://www.gnu.org/licenses/>.
//
// dllmain.cpp : Defines the entry point for the DLL application.

//
// Includes
// 

#define _CRT_SECURE_NO_WARNINGS
#define _WINSOCK_DEPRECATED_NO_WARNINGS

// fix for openssl UI define
#define UI TMPUI
#include "stdafx.h"
#undef UI

// needed for connect
#include <WinSock2.h>

// needed for SSL_connect, SSL_read, SSL_write
#include <openssl/ssl.h>
#include <openssl/err.h>

// needed for mutex lock
#include <mutex>

// needed for certificate validation
#include <Windows.h>
#include <wincrypt.h>

// needed for settings
#include "..\SporeServerConfig\SporeServerConfig.hpp"

// needed for std::stoi
#include <string>

//
// Libraries
//

#pragma comment(lib, "libssl32MT.lib")
#pragma comment(lib, "libcrypto32MT.lib")
#pragma comment(lib, "crypt32.lib")
#pragma comment(lib, "ws2_32.lib")

//
// Global variables
//

static SSL*         OpenSSL_SSL = nullptr;
static SSL_CTX*     OpenSSL_CTX = nullptr;
static std::mutex   OpenSSL_MTX;
static int          OpenSSL_ThreadId = -1;
static bool         SporeServerPortOverride = false;
static uint16_t     SporeServerPort;
static bool         SporeServerSslVerification = true;

//
// Helper functions
//

static void DisplayError(const char* fmt, ...)
{
    char buf[200];

    va_list args;
    va_start(args, fmt);
    vsprintf(buf, fmt, args);
    va_end(args);

    MessageBoxA(NULL, buf, "SporeNewOpenSSL", MB_OK | MB_ICONERROR);
}

//
// Detour functions
//

static int (WINAPI* connect_real)(SOCKET, const sockaddr*, int) = connect;
static int WINAPI connect_detour(SOCKET s, const sockaddr* name, int namelen)
{
    bool https = ntohs(((sockaddr_in*)name)->sin_port) == 443;

    // override port when requested,
    // and when we're using port 443
    if (SporeServerPortOverride &&
        https)
    {
        ((sockaddr_in*)name)->sin_port = htons(SporeServerPort);
    }

    int ret = connect_real(s, name, namelen);

    // we don't need openssl things
    // when it can't connect or if it's HTTP
    if (ret != 0 || !https)
    {
        return ret;
    }

    // does the game use this function?
    if (SSL_set_fd(OpenSSL_SSL, s) != 1)
    {
        DisplayError("SSL_set_fd(OpenSSL_SSL) Failed");
        return -1;
    }

    return 0;
}

static int (WINAPI* closesocket_real)(SOCKET) = closesocket;
static int WINAPI closesocket_detour(SOCKET s)
{
    // unlock the mutex here,
    // the game always calls closesocket
    if (OpenSSL_ThreadId == GetCurrentThreadId())
    {
        OpenSSL_ThreadId = -1;
        OpenSSL_MTX.unlock();
    }

    return closesocket_real(s);
}

static_detour(SSLCtxNewDetour, void* (void*)) {
    void* detoured(void* meth)
    {
        OpenSSL_CTX = SSL_CTX_new(TLS_method());
        if (OpenSSL_CTX == nullptr)
        {
            return nullptr;
        }

        return original_function(meth);
    }
};

static_detour(SSLNewDetour, void* (void*)) {
    void* detoured(void* ssl_ctx)
    {
        // TODO, figure out what kinda locking the game uses
        OpenSSL_MTX.lock();
        OpenSSL_ThreadId = GetCurrentThreadId();

        OpenSSL_SSL = SSL_new(OpenSSL_CTX);
        if (OpenSSL_SSL == nullptr)
        {
            return nullptr;
        }

        return original_function(ssl_ctx);
    }
};

static_detour(SSLClearDetour, int(void*)) {
    int detoured(void* ssl)
    {
        if (SSL_clear(OpenSSL_SSL) != 1)
        {
            return 0;
        }

        return original_function(ssl);
    }
};

static_detour(SSLConnectDetour, int(void*)) {
    int detoured(void* ssl)
    {
        return SSL_connect(OpenSSL_SSL);
    }
};

static_detour(SSLReadDetour, int(void*, void*, int)) {
    int detoured(void* ssl, void* buffer, int num)
    {
        return SSL_read(OpenSSL_SSL, buffer, num);
    }
};


static_detour(SSLWriteDetour, int(void*, const void*, int)) {
    int detoured(void* ssl, const void* buffer, int num)
    {
        return SSL_write(OpenSSL_SSL, buffer, num);
    }
};


static_detour(GameValidateCertificate, int(int, char*)) {
    int detoured(int arg1, char* servername)
    {
        // return success when verification is disabled
        if (!SporeServerSslVerification)
        {
            return 0;
        }

        // openssl variables
        unsigned char* x509_cert_buf = nullptr;
        int x509_cert_len = 0;

        // win32 crypt variables
        PCCERT_CONTEXT cert_ctx = nullptr;
        PCCERT_CHAIN_CONTEXT chain_ctx = nullptr;
        CERT_CHAIN_POLICY_PARA cert_chain_policy = { 0 };
        CERT_CHAIN_POLICY_STATUS cert_chain_status = { 0 };
        CERT_CHAIN_PARA cert_chain_params = { 0 };
        SSL_EXTRA_CERT_CHAIN_POLICY_PARA cert_chain_ssl_policy = { 0 };

        BOOL ret = false;

        // retrieve current certificate
        X509* x509_cert = SSL_get_peer_certificate(OpenSSL_SSL);
        if (x509_cert == nullptr)
        {
            goto out;
        }

        // extract encoded x509
        x509_cert_len = i2d_X509(x509_cert, &x509_cert_buf);
        if (x509_cert_len < 0)
        {
            goto out;
        }

        // convert encoded x509 to PCCERT_CONTEXT
        cert_ctx = (PCCERT_CONTEXT)CertCreateContext(CERT_STORE_CERTIFICATE_CONTEXT,
            X509_ASN_ENCODING,
            x509_cert_buf,
            x509_cert_len,
            CERT_CREATE_CONTEXT_NOCOPY_FLAG,
            nullptr);
        if (cert_ctx == nullptr)
        {
            return 1;
        }

        // retrieve hash of PCCERT_CONTEXT
        BYTE win32_cert_hash[20];
        DWORD win32_cert_hash_len = 20;
        ret = CertGetCertificateContextProperty(cert_ctx, CERT_HASH_PROP_ID,
            win32_cert_hash, &win32_cert_hash_len);
        if (!ret)
        {
            goto out;
        }

        // sadly the official servers
        // don't have valid certificates
        // so return success when
        // we encounter one of these
        BYTE ignoredCertsHashes[][20] =
        {
            { // pollinator.spore.com
                0xc8, 0xb2, 0x8f, 0xb, 0xd6,
                0x63, 0x2, 0x1d, 0x9b, 0x9c,
                0x36, 0x98, 0x82, 0xc, 0x76,
                0x74, 0xc7, 0xa7, 0x15, 0xb3
            },
            { // community.spore.com
                0xe6, 0xc8, 0x63, 0xf8, 0x55,
                0xf1, 0xc3, 0x7b, 0x9b, 0x34,
                0x78, 0xe6, 0xed, 0xaa, 0x85,
                0x36, 0xb, 0x7, 0xf1, 0xc4
            }
        };

        // loop over each certificate
        // and check if the current hash matches
        // if it does, return success
        for (int i = 0; i < ARRAYSIZE(ignoredCertsHashes); i++)
        {
            if (memcmp(win32_cert_hash, ignoredCertsHashes[i], 20) == 0)
            {
                ret = true;
                goto out;
            }
        }

        // retrieve certificate chain
        LPSTR usage[] = { szOID_PKIX_KP_SERVER_AUTH };
        cert_chain_params.RequestedUsage.dwType = USAGE_MATCH_TYPE_AND;
        cert_chain_params.RequestedUsage.Usage.cUsageIdentifier = 1;
        cert_chain_params.RequestedUsage.Usage.rgpszUsageIdentifier = (LPSTR*)usage;
        ret = CertGetCertificateChain(nullptr, cert_ctx,
            nullptr, nullptr,
            &cert_chain_params, CERT_CHAIN_REVOCATION_CHECK_CHAIN,
            nullptr, &chain_ctx);
        if (!ret)
        {
            goto out;
        }

        // convert servername into a widechar
        wchar_t servername_w[MAX_PATH];
        mbstowcs(servername_w, servername, MAX_PATH);

        // verify certificate
        cert_chain_ssl_policy.dwAuthType = AUTHTYPE_SERVER;
        cert_chain_ssl_policy.pwszServerName = servername_w;
        cert_chain_policy.pvExtraPolicyPara = &cert_chain_ssl_policy;
        ret = CertVerifyCertificateChainPolicy(CERT_CHAIN_POLICY_SSL,
            chain_ctx,
            &cert_chain_policy,
            &cert_chain_status);
        // we're only truly successful when
        // there are no errors
        ret = ret && (cert_chain_status.dwError == 0);

    out:
        if (x509_cert != nullptr)
        {
            X509_free(x509_cert);
        }
        if (cert_ctx != nullptr)
        {
            CertFreeCertificateContext(cert_ctx);
        }
        if (chain_ctx != nullptr)
        {
            CertFreeCertificateChain(chain_ctx);
        }

        // 0 = success
        // 1 = failure
        return ret ? 0 : 1;
    }
};

static_detour(GameUseHttpsDetour, bool(unsigned int, unsigned int, char*)) {
    bool detoured(unsigned int arg1, unsigned int arg2, char* arg3)
    {
        return original_function(arg1, arg2, arg3);
    }
};

static_detour(GameUseHttpDetour, bool(unsigned int, unsigned int, char*)) {
    bool detoured(unsigned int arg1, unsigned int arg2, char* arg3)
    {
        return GameUseHttpsDetour::original_function(arg1, arg2, arg3);
    }
};

//
// Boilerplate
//

void Initialize()
{
    if (!SporeServerConfig::Initialize())
    {
        DisplayError("SporeServerConfig::Initialize() Failed!");
        return;
    }

    if (SporeServerConfig::GetValue("OverridePort", "0")
        == "1")
    {
        try
        {
            int value = std::stoi(SporeServerConfig::GetValue("Port", "443"));
            // make sure we can safely cast the value
            if (value >= 0 && value <= USHRT_MAX)
            {
                SporeServerPortOverride = true;
                SporeServerPort = (uint16_t)value;
            }
        }
        catch (std::exception)
        {
            // ignore exception
        }
    }

    if (SporeServerConfig::GetValue("SslVerification", "1")
        == "0")
    {
        SporeServerSslVerification = false;
    }

    // This method is executed when the game starts, before the user interface is shown
    // Here you can do things such as:
    //  - Add new cheats
    //  - Add new simulator classes
    //  - Add new game modes
    //  - Add new space tools
    //  - Change materials
}

void Dispose()
{
    // This method is called when the game is closing
}

void AttachDetours()
{
    DWORD_PTR base_addr = (DWORD_PTR)GetModuleHandle(NULL);

    // RVA latest = 0xD7CA80
    // RVA disc   = 0xD7F200
    SSLCtxNewDetour::attach(base_addr + ModAPI::ChooseAddress(0xD7F200, 0xD7CA80));

    // RVA latest = 0xD7C580
    // RVA disc   = 0xD7ED00
    SSLNewDetour::attach(base_addr + ModAPI::ChooseAddress(0xD7ED00, 0xD7C580));

    // RVA latest = 0xD7C460
    // RVA disc   = 0xD7EBE0
    SSLClearDetour::attach(base_addr + ModAPI::ChooseAddress(0xD7EBE0, 0xD7C460));

    // RVA latest = 0xD7CE50
    // RVA disc   = 0xD7F5D0
    SSLConnectDetour::attach(base_addr + ModAPI::ChooseAddress(0xD7F5D0, 0xD7CE50));

    // RVA latest = 0xD7B430
    // RVA disc   = 0xD7DBB0
    SSLReadDetour::attach(base_addr + ModAPI::ChooseAddress(0xD7DBB0, 0xD7B430));

    // RVA latest = 0xD7B4C0
    // RVA disc   = 0xD7DC40
    SSLWriteDetour::attach(base_addr + ModAPI::ChooseAddress(0xD7DC40, 0xD7B4C0));

    // RVA latest = 0x54EB60
    // RVA disc   = 0x54F080
    GameValidateCertificate::attach(base_addr + ModAPI::ChooseAddress(0x54F080, 0x54EB60));

    // RVA latest = 0x2216E0
    // RVA disc   = 0x221740
    GameUseHttpsDetour::attach(base_addr + ModAPI::ChooseAddress(0x221740, 0x2216E0));

    // RVA latest = 0x2217A0
    // RVA disc   = 0x221800
    GameUseHttpDetour::attach(base_addr + ModAPI::ChooseAddress(0x221800, 0x2217A0));

    DetourAttach(&(PVOID&)connect_real, connect_detour);
    DetourAttach(&(PVOID&)closesocket_real, closesocket_detour);
}


// Generally, you don't need to touch any code here
BOOL APIENTRY DllMain(HMODULE hModule,
    DWORD  ul_reason_for_call,
    LPVOID lpReserved
)
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
        ModAPI::AddPostInitFunction(Initialize);
        ModAPI::AddDisposeFunction(Dispose);

        PrepareDetours(hModule);
        AttachDetours();
        CommitDetours();

        break;

    case DLL_PROCESS_DETACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
        break;
    }
    return TRUE;
}


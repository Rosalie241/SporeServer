//
// SporeServer - https://github.com/Rosalie241/SporeServer
//  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License version 3.
//  You should have received a copy of the GNU Affero General Public License
//  along with this program. If not, see <https://www.gnu.org/licenses/>.
//

//
// Includes
// 

#include "stdafx.h"
#include "Network.hpp"

// needed for connect
#include <WinSock2.h>

// needed for mutex lock
#include <mutex>

// needed for certificate validation
#include <Windows.h>
#include <wincrypt.h>

// needed for settings
#include "Configuration.hpp"

// needed for std::stoi
#include <string>

//
// Global variables
//

static bool     SporeServerPortOverride = false;
static uint16_t SporeServerPort;
static bool     SporeServerSslVerification = true;

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

    return connect_real(s, name, namelen);
}

static_detour(SSL_CTX_set_verify, void(void*, int, void*))
{
    void detoured(void* ssl, int mode, void* callback)
    {
        // force SSL_VERIFY_NONE to disable verifying CA chain,
        // this isn't that insecure because we force a hash match
        // in NetSSLVerifyConnection anyways
        // TODO: figure out how Spore sets the CA certificates
        return original_function(ssl, 0x00, callback);
    }
};

static_detour(NetSSLVerifyConnection, int(void*, char*)) {
    int detoured(void* ssl, char* servername)
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
        // X509* x509_cert = SSL_get_peer_certificate(ssl);
        void* x509_cert = STATIC_CALL(Address(ModAPI::ChooseAddress(0x0117db60, 0x0117b3e0)), void*, void*, ssl);
        if (x509_cert == nullptr)
        {
            goto out;
        }

        // extract encoded x509
        // x509_cert_len = i2d_X509(x509_cert, &x509_cert_buf);
        x509_cert_len = STATIC_CALL(Address(ModAPI::ChooseAddress(0x0117f700, 0x0117cf80)), int, Args(void*, unsigned char**), Args(x509_cert, &x509_cert_buf));
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
                0x26, 0x95, 0x77, 0x65,
                0x5C, 0xDD, 0x70, 0x98,
                0x74, 0x29, 0x72, 0x47,
                0x99, 0xFB, 0xFF, 0x57,
                0x38, 0xC7, 0x88, 0x74
            },
            { // community.spore.com
                0xA9, 0x9B, 0xE0, 0xF2,
                0xED, 0xC0, 0x7D, 0xA0,
                0x7D, 0x9B, 0xC0, 0x84,
                0x20, 0xC6, 0x3D, 0xF0,
                0x3B, 0xD6, 0x9C, 0xC2
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
            // X509_free(x509_cert);
            STATIC_CALL(Address(ModAPI::ChooseAddress(0x0117f730, 0x0117cfb0)), void, void*, x509_cert);
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
// Exported Functions
//

void Network::Initialize(void)
{
    if (Configuration::GetBoolValue(Configuration::Key::OverridePort))
    {
        try
        {
            int value = std::stoi(Configuration::GetStringValue(Configuration::Key::Port));
            // make sure we can safely cast the value
            if (value >= 0 && value <= USHRT_MAX)
            {
                SporeServerPortOverride = true;
                SporeServerPort = (uint16_t)value;
            }
        }
        catch (...)
        {
            // ignore exception
        }
    }

    SporeServerSslVerification = Configuration::GetBoolValue(Configuration::Key::SSLVerification);
}

void Network::AttachDetours(void)
{
    SSL_CTX_set_verify::attach(Address(ModAPI::ChooseAddress(0x0117e2b0, 0x0117bb30)));
    NetSSLVerifyConnection::attach(Address(ModAPI::ChooseAddress(0x0094f080, 0x0094eb60)));
    GameUseHttpsDetour::attach(Address(ModAPI::ChooseAddress(0x00621740, 0x006216e0)));
    GameUseHttpDetour::attach(Address(ModAPI::ChooseAddress(0x00621800, 0x006217a0)));
    DetourAttach(&(PVOID&)connect_real, connect_detour);
}

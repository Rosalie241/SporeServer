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

//
// Libraries
//

#pragma comment(lib, "libssl32MT.lib")
#pragma comment(lib, "libcrypto32MT.lib")
#pragma comment(lib, "crypt32.lib")
#pragma comment(lib, "ws2_32.lib")

//
// configure options
//

// saves cookie from previous sessions
// and injects them when a header
// doesn't contain a cookie,
// this is required for SporeServer
#define SPORENEWOPENSSL_FIXHTTP

// forces HTTPS on all connections
#define SPORENEWOPENSSL_FORCEHTTPS

//
// Global variables
//

static SSL*			OpenSSL_SSL = nullptr;
static SSL_CTX*		OpenSSL_CTX = nullptr;
static std::mutex	OpenSSL_MTX;
static int			OpenSSL_ThreadId = -1;

//
// Helper functions
//

static void DisplayError(const char* fmt, ...)
{
	char buf[2000];

	va_list args;
	va_start(args, fmt);
	vsprintf(buf, fmt, args);
	va_end(args);

	MessageBoxA(NULL, buf, "SporeModernOpenssl", MB_OK | MB_ICONERROR);
}

//
// Detour functions
//

static int (WINAPI* connect_real)(SOCKET, const sockaddr*, int) = connect;
static int WINAPI connect_detour(SOCKET s, const sockaddr* name, int namelen)
{
	int ret = connect_real(s, name, namelen);

	// we don't need openssl things
	// when it can't connect or if it's HTTP
	if (ret != 0 ||
		ntohs(((sockaddr_in*)name)->sin_port) != 443)
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

static_detour(SSLNewDetour, void*(void*)) {
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
		return SSL_connect(OpenSSL_SSL);;
	}
};

static_detour(SSLReadDetour, int(void*, void*, int)) {
	int detoured(void* ssl, void* buffer, int num)
	{
		return SSL_read(OpenSSL_SSL, buffer, num);;
	}
};

static_detour(SSLWriteDetour, int(void*, const void*, int)) {
	int detoured(void* ssl, const void* buffer, int num)
	{
#ifdef SPORENEWOPENSSL_FIXHTTP
		static char* cookie_str = NULL;
		static bool sent_cookie = false;
		static bool saved_cookie = false;
		static bool in_header = false;

		const char* str_buf = (const char*)buffer;

		// begin of HTTP header
		if (strstr(str_buf, "GET") != NULL ||
			strstr(str_buf, "POST") != NULL)
		{
			in_header = true;
			sent_cookie = false;
		}
		// store cookie
		else if (strstr(str_buf, "Cookie:") != NULL)
		{
			// hopefully this won't fail, ever
			cookie_str = _strdup(str_buf);
			if (cookie_str == NULL)
			{
				DisplayError("strdup(buf) Failed: %li", GetLastError());
				return -1;
			}

			saved_cookie = true;
			sent_cookie = true;
		}
		// end of HTTP header
		else if (strcmp(str_buf, "\r\n") == 0
			&& in_header)
		{
			// inject cookie if no cookie has been sent
			if (saved_cookie
				&& !sent_cookie)
			{
				int ret;
				ret = SSL_write(OpenSSL_SSL, cookie_str, strlen(cookie_str));
				if (ret <= 0)
				{
					return ret;
				}
			}

			in_header = false;
		}

		return SSL_write(OpenSSL_SSL, buffer, num);
#else
		return SSL_write(OpenSSL_SSL, buffer, num);
#endif
	}
};

// this game function seems to validate "subjectaltname"
// from the certificate, this is an unneeded check 
// and only causes issues for i.e third-party servers
// so just always return a value the game expects
static_detour(GameFunctionDetour, int(int, char*)) {
	int detoured(int arg1, char* arg2)
	{
		return 0;
	}
};

#ifdef SPORENEWOPENSSL_FORCEHTTPS
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
#endif

//
// Boilerplate
//

void Initialize()
{
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
	uint32_t base_addr = (uint32_t)GetModuleHandle(NULL);

	// RVA = 0xD7CA80
	SSLCtxNewDetour::attach(base_addr + 0xD7CA80);

	// RVA = 0xD7C580
	SSLNewDetour::attach(base_addr + 0xD7C580);

	// RVA = 0xD7C460
	SSLClearDetour::attach(base_addr + 0xD7C460);

	// RVA = 0xD7CE50
	SSLConnectDetour::attach(base_addr + 0xD7CE50);

	// RVA = 0xD7B430
	SSLReadDetour::attach(base_addr + 0xD7B430);

	// RVA = 0xD7B4C0
	SSLWriteDetour::attach(base_addr + 0xD7B4C0);

	// RVA = 0x54EB60
	GameFunctionDetour::attach(base_addr + 0x54EB60);
	
#ifdef SPORENEWOPENSSL_FORCEHTTPS
	// RVA = 0x2216E0
	GameUseHttpsDetour::attach(base_addr + 0x2216E0);

	// RVA = 0x2217A0
	GameUseHttpDetour::attach(base_addr + 0x2217A0);
#endif
	
	DetourAttach(&(PVOID&)connect_real, connect_detour);
	DetourAttach(&(PVOID&)closesocket_real, closesocket_detour);
}


// Generally, you don't need to touch any code here
BOOL APIENTRY DllMain( HMODULE hModule,
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


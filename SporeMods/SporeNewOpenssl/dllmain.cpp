// dllmain.cpp : Defines the entry point for the DLL application.
#define _CRT_SECURE_NO_WARNINGS
// needed
#define UI TMPUI
#include "stdafx.h"
#undef UI

#define _WINSOCK_DEPRECATED_NO_WARNINGS
#include <WinSock2.h>


#include <openssl/ssl.h>
#include <openssl/err.h>

#pragma comment(lib, "wsock32.lib")

#pragma comment(lib, "libssl32MT.lib")
#pragma comment(lib, "libcrypto32MT.lib")
#pragma comment(lib, "crypt32.lib")
#pragma comment(lib, "ws2_32.lib")

static void DisplayOpensslError(const char* buf);
static void DisplayError(const char* fmt, ...);

/*
static bool Openssl_Init = false;
static SSL* Openssl_SSL = NULL;
static SSL_CTX* Openssl_CTX = NULL;
static int Openssl_Sock = 0;
static SOCKET Openssl_Currentsock = 0;
static SOCKET blah;

static int (WINAPI* connect_real)(SOCKET, const sockaddr*, int) = connect;
static int WINAPI connect_detour(SOCKET s, const sockaddr* name, int namelen)
{
	sockaddr_in* in_addr = (sockaddr_in*)name;
	char* ip = inet_ntoa(in_addr->sin_addr);
	USHORT port = ntohs(in_addr->sin_port);
	
	// we only support IPv4 HTTP (at port 80) right now
	if (port != 80 ||
		in_addr->sin_family != AF_INET ||
		!Openssl_Init)
	{
		goto fallback;
	}

	DisplayError("connect_detour(): %i", s);

	//DisplayError("connect_detour(%i)", s);

	blah = socket(AF_INET, SOCK_STREAM, 0);
	if (s < 0)
	{
		DisplayError("socket() Failed: %li", GetLastError());
	}

	DisplayError("connect_detour(): aaaaa %i", s);

	((sockaddr_in*)name)->sin_port = htons(443);
	if (connect_real(blah, name, namelen) != 0)
	{
		DisplayError("connect_real() Failed: %li", GetLastError());
		goto fallback;
	}

	Openssl_SSL = SSL_new(Openssl_CTX);
	if (Openssl_SSL == NULL)
	{
		DisplayOpensslError("SSL_new(Openssl_CTX)");
		goto fallback;
	}

	SSL_CTX_set_mode(Openssl_CTX, SSL_CTX_get_mode(Openssl_CTX) | SSL_MODE_ACCEPT_MOVING_WRITE_BUFFER);
	//SSL_set_mode(Openssl_SSL, SSL_get_mode(Openssl_SSL) | SSL_MODE_ACCEPT_MOVING_WRITE_BUFFER);

	//Openssl_Sock = SSL_get_fd(Openssl_SSL);
	if (SSL_set_fd(Openssl_SSL, blah) != 1)
	{
		DisplayOpensslError("SSL_set_fd(Openssl_SSL)");
		goto fallback;
	}

	int ret = 0;
	if (SSL_connect(Openssl_SSL) != 1)
	{
		//DisplayError("SSL_connect(Openssl_SSL) Failed: %i", ret);
		DisplayOpensslError("SSL_connect(Openssl_SSL)");
		goto fallback;
	}

	Openssl_Currentsock = s;
	return 0;

fallback:
	return connect_real(s, name, namelen);
}

static int (WINAPI* recv_real)(SOCKET, char*, int, int) = recv;
static int WINAPI recv_detour(SOCKET s, char* buf, int len, int flags)
{
	DisplayError("recv_detour()", buf);

	if (Openssl_Currentsock != s)
		return recv_real(s, buf, len, flags);

	int ret = SSL_read(Openssl_SSL, buf, len);
	
	//DisplayError("recv_detour(): %s", buf);


	if (ret <= 0)
		DisplayOpensslError("SSL_read(Openssl_SSL)");


	return ret;
	//DisplayError("recv_detour(%i)", s);
	//return recv_real(s, buf, len, flags);
}

static int (WINAPI* send_real)(SOCKET, const char*, int, int) = send;
static int WINAPI send_detour(SOCKET s, const char* buf, int len, int flags)
{
	DisplayError("send_detour(%s)", buf);

	if (Openssl_Currentsock != s)
		return send_real(s, buf, len, flags);

	int ret;
	
	//DisplayError("send_detour(%s)", buf);

	ret = SSL_write(Openssl_SSL, buf, len);
	if (ret <= 0)
	{
			int err = SSL_get_error(Openssl_SSL, ret);

			DisplayError("SSL_get_error(): %i", err);

			//if (err == SSL_ERROR_WANT_READ || err == SSL_ERROR_WANT_WRITE)
	}

	
	if (ret <= 0)
		DisplayOpensslError("SSL_write(Openssl_SSL)");
		
	//DisplayError("send_detour %i", ret);
	return ret;
	//DisplayError("send_detour(%i)", s);
	//return SSL_write(Openssl_SSL, buf, len);
}

static int (WINAPI* bind_real)(SOCKET, const sockaddr*, int) = bind;
static int WINAPI bind_detour(SOCKET s, const sockaddr* name, int namelen)
{
	DisplayError("bind_detour()");

	if (Openssl_Sock != s)
		return bind_real(s, name, namelen);

	//DisplayError("bind_detour(%i)", s);

	return bind_real(blah, name, namelen);
}

static int (WINAPI* listen_real)(SOCKET, int) = listen;
static int listen_detour(SOCKET s, int backlog)
{
	DisplayError("listen_detour()");

	if (Openssl_Sock != s)
		return listen_real(s, backlog);

	DisplayError("listen_detou 2r()");

	return listen_real(blah, backlog);
}

static int (WINAPI* recvfrom_real)(SOCKET, char*, int, int, sockaddr*, int*) = recvfrom;
static int WINAPI recvfrom_detour(SOCKET s, char* buf, int len, int flags, sockaddr* from, int* fromlen)
{
	DisplayError("recvfrom_detour()");

	return recvfrom_real(s, buf, len, flags, from, fromlen);
}

static int (WINAPI* closesocket_real)(SOCKET) = closesocket;
static int WINAPI closesocket_detour(SOCKET s)
{
	DisplayError("closesocket_detour()");

	return closesocket_real(s);
}

*/



static void DisplayOpensslError(const char* buf)
{
	DisplayError("%s Failed: %s", buf, ERR_error_string(ERR_get_error(), 0));
}


static void DisplayError(const char* fmt, ...)
{
	char buf[200];

	va_list args;
	va_start(args, fmt);
	vsprintf(buf, fmt, args);
	va_end(args);

	MessageBoxA(NULL, buf, "SporeModernOpenssl", MB_OK | MB_ICONERROR);
}

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

/*static bool Openssl_Init = false;
static SSL* Openssl_SSL = NULL;
static SSL_CTX* Openssl_CTX = NULL;
static int Openssl_Sock = 0;
static SOCKET Openssl_Currentsock = 0;
static SOCKET blah;

static void InitializeOpenssl()
{
	SSL_library_init();
	SSL_load_error_strings();

	const SSL_METHOD* meth = TLSv1_2_client_method();
	//SSLv23_client_method

	Openssl_CTX = SSL_CTX_new(meth);
	if (Openssl_CTX == NULL)
	{
		DisplayOpensslError("SSL_CTX_new(meth)");
		return;
	}

	SSL_CTX_set_mode(Openssl_CTX, SSL_MODE_ENABLE_PARTIAL_WRITE | SSL_MODE_ACCEPT_MOVING_WRITE_BUFFER);
	SSL_CTX_set_verify(Openssl_CTX, SSL_VERIFY_NONE, NULL);

	//SSL_CTX_set_mode(Openssl_CTX, SSL_MODE_ACCEPT_MOVING_WRITE_BUFFER | SSL_MODE_ENABLE_PARTIAL_WRITE);
	/*Openssl_SSL = SSL_new(Openssl_CTX);
	Openssl_Init = (Openssl_SSL != NULL);
	if (!Openssl_Init)
	{
		DisplayOpensslError("SSL_new(Openssl_CTX);");
		return;
	}*/

//}

static FILE* file = NULL;
static char buf[500];

/*
static int (WINAPI* connect_real)(SOCKET, const sockaddr*, int) = connect;
static int WINAPI connect_detour(SOCKET s, const sockaddr* name, int namelen)
{

	sockaddr_in* in_addr = (sockaddr_in*)name;
	USHORT port = ntohs(in_addr->sin_port);


	int ret = connect_real(s, name, namelen);

	DisplayError("connect_real(%i, %i)", ret, port);

	if (ret != 0)
		return ret;

	
	DisplayError("connect_detour(%i)", port);

	Openssl_SSL = SSL_new(Openssl_CTX);
	if (Openssl_SSL == NULL)
	{
		DisplayOpensslError("SSL_new(Openssl_CTX)");
		return -1;
	}

	//SSL_CTX_set_mode(Openssl_CTX, SSL_CTX_get_mode(Openssl_CTX) | SSL_MODE_ACCEPT_MOVING_WRITE_BUFFER);
	//SSL_set_mode(Openssl_SSL, SSL_get_mode(Openssl_SSL) | SSL_MODE_ACCEPT_MOVING_WRITE_BUFFER);

	//Openssl_Sock = SSL_get_fd(Openssl_SSL);
	if (SSL_set_fd(Openssl_SSL, s) != 1)
	{
		DisplayOpensslError("SSL_set_fd(Openssl_SSL)");
		return -1;
	}

	
	if ((ret = SSL_connect(Openssl_SSL)) != 1)
	{
		DisplayError("aaaaa %i", SSL_get_error(Openssl_SSL, ret));
		//DisplayError("SSL_connect(Openssl_SSL) Failed: %i", ret);
		DisplayOpensslError("SSL_connect(Openssl_SSL)");
		return -1;
	}

	//Openssl_Currentsock = s;
	return 0;
}*/

int (*SSL_read_real)(void*, void*, int) = (int(*)(void*, void*, int))0x00E3B430;
int SSL_read_detour(void* ssl, void* buffer, int num)
{
	int ret = SSL_read_real(ssl, buffer, num);

	//if (ret <= 0)
		//DisplayOpensslError("SSL_write");

	fwrite("SSL_read_detour(): ", 19, 1, file);
	fwrite(buffer, ret, 1, file);

//DisplayError("SSL_read_detour() buf: %s", buffer);

	return ret;
}

int (*SSL_write_real)(void*, const void*, int) = (int(*)(void*, const void*, int))0x00E3B4C0;
int SSL_write_detour(void* ssl, const void* buffer, int num)
{
	int ret = SSL_write_real(ssl , buffer, num);


	//DisplayError("SSL_write_detour() buf: %s", buffer);

	//if (ret <= 0)
		//DisplayOpensslError("SSL_write");
	fwrite("SSL_write_detour(): ", 20, 1, file);
	fwrite(buffer, ret, 1, file);

	//DisplayError("SSL_write_detour() buf: %s", buffer);

	return ret;
}

void AttachDetours()
{
	file = fopen("network.txt", "w");

	DisplayError("AttachDetours");
	if (DetourAttach(&(PVOID&)SSL_read_real, (PVOID)SSL_read_detour) != NO_ERROR)
	{
		DisplayError("DetourAttach(SSL_read_real) Failed: %li", GetLastError());
		return;
	}

	if (DetourAttach(&(PVOID&)SSL_write_real, (PVOID)SSL_write_detour) != NO_ERROR)
	{
		DisplayError("DetourAttach(SSL_write_real) Failed: %li", GetLastError());
		return;
	}

	/*if (DetourAttach(&(PVOID&)connect_real, (PVOID)connect_detour) != NO_ERROR)
	{
		DisplayError("DetourAttach(SSL_write_real) Failed: %li", GetLastError());
		return;
	}

	/*if (DetourAttach(&(PVOID&)connect_real, connect_detour) != NO_ERROR)
	{
		DisplayError("DetourAttach(connect_real) Failed: %li", GetLastError());
		return;
	}

	if (DetourAttach(&(PVOID&)recv_real, recv_detour) != NO_ERROR)
	{
		DisplayError("DetourAttach(recv_real) Failed: %li", GetLastError());
		return;
	}

	if (DetourAttach(&(PVOID&)send_real, send_detour) != NO_ERROR)
	{
		DisplayError("DetourAttach(send_real) Failed: %li", GetLastError());
		return;
	}

	if (DetourAttach(&(PVOID&)bind_real, bind_detour) != NO_ERROR)
	{
		DisplayError("DetourAttach(send_real) Failed: %li", GetLastError());
		return;
	}

	if (DetourAttach(&(PVOID&)listen_real, listen_detour) != NO_ERROR)
	{
		DisplayError("DetourAttach(listen_real) Failed: %li", GetLastError());
		return;
	}

	if (DetourAttach(&(PVOID&)recvfrom_real, recvfrom_detour) != NO_ERROR)
	{
		DisplayError("DetourAttach(recvfrom_real) Failed: %li", GetLastError());
		return;
	}

	if (DetourAttach(&(PVOID&)closesocket_real, closesocket_detour) != NO_ERROR)
	{
		DisplayError("DetourAttach(closesocket_real) Failed: %li", GetLastError());
		return;
	}*/

	// Call the attach() method on any detours you want to add
	// For example: cViewer_SetRenderType_detour::attach(GetAddress(cViewer, SetRenderType));
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
		// TODO
		//InitializeOpenssl();

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


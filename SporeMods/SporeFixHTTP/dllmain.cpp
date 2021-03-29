// dllmain.cpp : Defines the entry point for the DLL application.
#define _CRT_SECURE_NO_WARNINGS
#define _CRT_NONSTDC_NO_WARNINGS
#include "stdafx.h"


#include <Winsock2.h>
#pragma comment(lib, "wsock32.lib")

#include <iostream>

static void DisplayError(const char* fmt, ...)
{
	char buf[1000];

	va_list args;
	va_start(args, fmt);
	vsprintf(buf, fmt, args);
	va_end(args);

	MessageBoxA(NULL, buf, "SporeFixHTTP", MB_OK | MB_ICONERROR);
}

/*
	whenever a /community/ endpoint returns 401 (unauthorized),
	the game will send Spore-User and Spore-Password in a header without a HTTP request (no POST/GET/PATCH).
	To fix this issue (because I *do* want to follow the HTTP spec), 
	the following code will save the cookie from a previous HTTP request and 
	re-send that if the current HTTP header does *not* contain a cookie.
*/

static char* cookie_str = NULL;
static bool sent_cookie = false;
static bool saved_cookie = false;
static bool in_header = false;

static int (WINAPI* send_real)(SOCKET, const char*, int, int) = send;
static int WINAPI send_detour(SOCKET s, const char* buf, int len, int flags)
{
	/* begin of HTTP header */
	if (strstr(buf, "GET") != NULL)
	{
		in_header = true;
	}
	/* store cookie */
	else if (strstr(buf, "Cookie:") != NULL)
	{
		/* hopefully this won't fail, ever */
		cookie_str = strdup(buf);
		if (cookie_str == NULL)
		{
			DisplayError("strdup(buf) Failed: %li", GetLastError());
			return -1;
		}

		saved_cookie = true;
		sent_cookie = true;
	}
	/* end of HTTP header */
	else if (strcmp(buf, "\r\n") == 0
			&& in_header)
	{
		if (saved_cookie 
			&& !sent_cookie)
		{
			int ret;
			ret = send_real(s, cookie_str, strlen(cookie_str), flags);
			if (ret <= 0)
			{
				return ret;
			}
		}

		in_header = false;
		sent_cookie = false;
	}

	return send_real(s, buf, len, flags);
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
	/* cleanup */
	if (cookie_str != NULL)
	{
		free(cookie_str);
	}
	// This method is called when the game is closing
}

void AttachDetours()
{
	if (DetourAttach(&(PVOID&)send_real, send_detour) != NO_ERROR)
	{
		DisplayError("DetourAttach(send_real) Failed: %li", GetLastError());
		return;
	}
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


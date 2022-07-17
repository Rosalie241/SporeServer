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

#include "stdafx.h"

// needed for gethostbyname
#include <WinSock2.h>

// needed for settings
#include "..\SporeServerConfig\SporeServerConfig.hpp"

//
// Libraries
//

#pragma comment(lib, "wsock32.lib")

//
// Global variables
//

static bool        SporeServerHostOverride = false;
static std::string SporeServerHost;

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

    MessageBoxA(NULL, buf, "SporeRedirectTraffic", MB_OK | MB_ICONERROR);
}

//
// Detour functions
//

static hostent* (WINAPI* gethostbyname_real)(const char*) = gethostbyname;
static hostent* WINAPI gethostbyname_detour(const char* hostname)
{
    // only override when requested
    if (SporeServerHostOverride)
    {
        return gethostbyname_real(SporeServerHost.c_str());
    }

    return gethostbyname_real(hostname);
}

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

    if (SporeServerConfig::GetValue("OverrideHost", "0")
        == "1")
    {
        SporeServerHostOverride = true;
        SporeServerHost = SporeServerConfig::GetValue("Host", "localhost");
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
    DetourAttach(&(PVOID&)gethostbyname_real, gethostbyname_detour);

    // Call the attach() method on any detours you want to add
    // For example: cViewer_SetRenderType_detour::attach(GetAddress(cViewer, SetRenderType));
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


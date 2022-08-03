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

#include "stdafx.h"

#include "Configuration.hpp"
#include "OpenSSL.hpp"
#include "RedirectTraffic.hpp"

//
// Boilerplate
//

void Initialize()
{
    if (!Configuration::Initialize())
    {
        MessageBoxA(nullptr, "Configuration::Initialize() Failed!", "SporeServer", MB_OK | MB_ICONERROR);
        return;
    }
    OpenSSL::Initialize();
    RedirectTraffic::Initialize();
}

void Dispose()
{
}

void AttachDetours()
{
    OpenSSL::AttachDetours();
    RedirectTraffic::AttachDetours();
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


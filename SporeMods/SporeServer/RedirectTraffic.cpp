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
#include "RedirectTraffic.hpp"

// needed for gethostbyname
#include <WinSock2.h>

// needed for std::string
#include <string>

// needed for settings
#include "Configuration.hpp"

//
// Global variables
//

static bool        SporeServerHostOverride = false;
static std::string SporeServerHost;

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
// Exported Function
//

void RedirectTraffic::Initialize(void)
{
    if (Configuration::GetBoolValue(Configuration::Key::OverrideHost))
    {
        SporeServerHostOverride = true;
        SporeServerHost = Configuration::GetStringValue(Configuration::Key::Host);
    }
}

void RedirectTraffic::AttachDetours(void)
{
    DetourAttach(&(PVOID&)gethostbyname_real, gethostbyname_detour);
}

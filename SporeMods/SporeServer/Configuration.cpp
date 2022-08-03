//
// SporeServer - https://github.com/Rosalie241/SporeServer
//  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License version 3.
//  You should have received a copy of the GNU Affero General Public License
//  along with this program. If not, see <https://www.gnu.org/licenses/>.
//
#include "stdafx.h"
#include "Configuration.hpp"

#include <iostream>
#include <filesystem>

//
// Local Structs
//

struct configurationKeyOption
{
    std::string AppName;
    std::string Name;
    std::string DefaultValue;
};

//
// Local Variables
//

static std::filesystem::path l_ConfigFilePath;

//
// Local Functions
//

static configurationKeyOption configurationKeyToOption(Configuration::Key key)
{
    configurationKeyOption option;

    switch (key)
    {
        case Configuration::Key::OverrideHost:
            option = { "SporeServer", "OverrideHost", "0" };
            break;
        case Configuration::Key::Host:
            option = { "SporeServer", "Host", "localhost" };
            break;
        case Configuration::Key::OverridePort:
            option = { "SporeServer", "OverridePort", "0" };
            break;
        case Configuration::Key::Port:
            option = { "SporeServer", "Port", "5001" };
            break;
        case Configuration::Key::SSLVerification:
            option = { "SporeServer", "SSLVerification", "1" };
            break;
        default:
            // shouldn't happen
            throw std::exception();
    }

    return option;
}

//
// Exported Functions
//

bool Configuration::Initialize(void)
{
    wchar_t envBuffer[4096] = { 0 };
    configurationKeyOption option;

    if (GetEnvironmentVariableW(L"APPDATA", envBuffer, 4096) == 0)
    {
        return false;
    }

    l_ConfigFilePath += envBuffer;
    l_ConfigFilePath += "\\Spore\\Preferences\\SporeServer.ini";

    if (std::filesystem::is_regular_file(l_ConfigFilePath))
    { // file already exists
        return true;
    }

    for (int i = 0; i < (int)Key::Unknown; i++)
    {
        option = configurationKeyToOption((Key)i);

        if (!SetValue((Key)i, option.DefaultValue))
        {
            return false;
        }
    }

    return true;
}

std::string Configuration::GetStringValue(Configuration::Key key)
{
    char buf[MAX_PATH] = { 0 };

    configurationKeyOption option = configurationKeyToOption(key);

    GetPrivateProfileStringA(option.AppName.c_str(), option.Name.c_str(), option.DefaultValue.c_str(), buf, MAX_PATH, l_ConfigFilePath.string().c_str());

    return std::string(buf);
}

bool Configuration::GetBoolValue(Configuration::Key key)
{
    std::string value;

    value = GetStringValue(key);

    return value == "1" || value == "true";
}

bool Configuration::SetValue(Configuration::Key key, std::string value)
{
    configurationKeyOption option = configurationKeyToOption(key);

    return WritePrivateProfileStringA(option.AppName.c_str(), option.Name.c_str(), value.c_str(), l_ConfigFilePath.string().c_str());
}


//
// SporeServer - https://github.com/Rosalie241/SporeServer
//  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License version 3.
//  You should have received a copy of the GNU Affero General Public License
//  along with this program. If not, see <https://www.gnu.org/licenses/>.
//
// SporeServerConfig.cpp : Defines the functions for the static library.
//

#define _CRT_SECURE_NO_WARNINGS
#include <Windows.h>
#include "SporeServerConfig.hpp"

#define APP_NAME "SporeServer"

static std::string ConfigPath;

namespace SporeServerConfig
{
	bool Initialize()
	{
		char* appdata = getenv("APPDATA");

		if (appdata == nullptr)
		{
			// shouldn't happen
			return false;
		}

		ConfigPath = appdata;
		ConfigPath += "\\Spore\\Preferences\\SporeServer.ini";

		// create config file when it doesn't exist
		if (GetFileAttributesA(ConfigPath.c_str()) == INVALID_FILE_ATTRIBUTES)
		{
			SetValue("OverrideHost", "1");
			SetValue("Host", "localhost");
			SetValue("OverridePort", "0");
			SetValue("Port", "443");
			SetValue("SslVerification", "1");
		}

		return true;
	}

	std::string GetValue(std::string keyName, std::string defaultValue)
	{
		char buf[MAX_PATH];

		GetPrivateProfileStringA(APP_NAME, keyName.c_str(), defaultValue.c_str(), buf, MAX_PATH, ConfigPath.c_str());

		return std::string(buf);
	}

	bool SetValue(std::string keyName, std::string value)
	{
		return WritePrivateProfileStringA(APP_NAME, keyName.c_str(), value.c_str(), ConfigPath.c_str());
	}
}

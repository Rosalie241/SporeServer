//
// SporeServer - https://github.com/Rosalie241/SporeServer
//  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License version 3.
//  You should have received a copy of the GNU Affero General Public License
//  along with this program. If not, see <https://www.gnu.org/licenses/>.
//
#ifndef CONFIGURATION_HPP
#define CONFIGURATION_HPP

#include <string>

namespace Configuration
{
    enum class Key
    {
        OverrideHost = 0,
        Host = 1,
        OverridePort = 2,
        Port = 3,
        SSLVerification = 4,
        Unknown = 5,
    };

    /// <summary>
    ///     Initializes Configuration
    /// </summary>
    bool Initialize(void);

    /// <summary>
    ///     Retrieves string value
    /// </summary>
    std::string GetStringValue(Configuration::Key key);
    /// <summary>
    ///     Retrieves bool value
    /// </summary>
    bool GetBoolValue(Configuration::Key key);

    /// <summary>
    ///     Sets value
    /// </summary>
    bool SetValue(Configuration::Key key, std::string value);
}



#endif // CONFIGURATION_HPP
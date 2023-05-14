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
#include "AssetBrowserFeed.hpp"

#include <string>

bool AssetBrowserFeed::InjectUrls(void)
{
    struct
    {
        uint32_t Id1;
        uint32_t Id2;
        std::string Url;
    } urls[] =
    {
        { 0x0100000, 0x53dd8c2, "/Moderation/Management/Users"},
        { 0x0100001, 0x53dd8c2, "/community/assetBrowser/singleDownload?manage=1"},
        { 0x0100002, 0x53dd8c2, "/Moderation/Management/UserReports"},
    };

    for (const auto& url : urls)
    {
        if (!STATIC_CALL(Address(ModAPI::ChooseAddress(0x00621740, 0x006216e0)), bool, Args(uint32_t, uint32_t, const char*), Args(url.Id1, url.Id2, url.Url.c_str())))
        {
            return false;
        }
    }

    return true;
}

bool AssetBrowserFeed::InjectModeratorCategory(void)
{
    PropertyListPtr propertyList;
    App::Property* property;
    ResourceKey* resourceKeys;
    size_t resourceKeysCount = 0;
    static eastl::vector<ResourceKey> resourceKeysVector;
    ResourceKey moderatorCategoryResourceKey;
    static bool hasInjectedCategory = false;

    if (hasInjectedCategory)
    { // never inject twice
        return true;
    }
   

    // attempt to retrieve property list
    if (!PropManager.GetPropertyList(0x2ea8fb98, 0x6edc12d4, propertyList))
    {
        return false;
    }

    // attempt to retrieve property
    if (!propertyList->GetProperty(0x744717C0, property))
    {
        return false;
    }
    
    // get resource keys value from property
    if (!App::Property::GetArrayKey(propertyList.get(), 0x744717C0, resourceKeysCount, resourceKeys))
    {
        return false;
    }

    // add resource keys to vector
    for (size_t i = 0; i < resourceKeysCount; i++)
    {
        resourceKeysVector.push_back(resourceKeys[i]);
    }
    
    // attempt to retrieve resource key of the moderator category
    if (!ResourceKey::Parse(moderatorCategoryResourceKey, u"AssetBrowserFeedCategories!SporeServerModeration.prop"))
    {
        return false;
    }

    // inject it into the vector
    resourceKeysVector.push_back(moderatorCategoryResourceKey);

    // change the property with the injected category
    property->SetArrayKey(resourceKeysVector.data(), resourceKeysVector.size());
    hasInjectedCategory = true;
    return true;
}

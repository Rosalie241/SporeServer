/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
using SporeServer.Areas.Identity.Data;
using System;

/*
<?xml version="1.0" encoding="UTF-8"?>
<handshake xmlns="http://www.w3.org/2005/Atom" xmlns:sp="http://spore.com/atom">
    <user-id>501089776600</user-id>
    <screen-name>ExampleUserName</screen-name>
    <next-id>501091100295</next-id>
    <refresh-rate>120</refresh-rate>
    <maxis-feeds>
        <sp:entry>
            <sp:id>tag:spore.com,2006:maxis/adventures/en_US</sp:id>
            <sp:title>Maxis Adventures</sp:title>
            <sp:updated>2009-06-22T23:02:41.431Z</sp:updated>
            <sp:subtitle>Free creations from Maxis.</sp:subtitle>
            <sp:subcount>0</sp:subcount>
            <sp:link rel="self" href="http://pollinator.spore.com/pollinator/atom/maxis/adventures/en_US" />
        </sp:entry>
    </maxis-feeds>
    <my-feeds>
        <entry>
            <id>tag:spore.com,2006:user/501089776639</id>
            <title>ExampleUserName</title>
            <updated>2021-04-19T10:53:41.754Z</updated>
            <author>
                <name>ExampleUserName</name>
                <uri>501089776600</uri>
            </author>
            <subcount>0</subcount>
            <link rel="self" href="http://pollinator.spore.com/pollinator/atom/user/501089776600" />
        </entry>
    </my-feeds>
    <subscriptions>
        <entry>
            <id>tag:spore.com,2006:user/501071978458</id>
            <title>TheAWKLORD</title>
            <updated>2021-04-24T03:04:40.824Z</updated>
            <author>
                <name>TheAWKLORD</name>
                <uri>501071978458</uri>
            </author>
            <subcount>0</subcount>
            <link rel="self" href="http://pollinator.spore.com/pollinator/atom/user/501071978458" />
        </entry>
    </subscriptions>
    <invisible-feeds>
        <sp:entry>
            <sp:id>tag:spore.com,2006:downloadQueue</sp:id>
            <sp:title>ExampleUserName</sp:title>
            <sp:updated>2021-04-30T14:29:54.268Z</sp:updated>
            <sp:author>
                <sp:name>ExampleUserName</sp:name>
                <sp:uri>501089776600</sp:uri>
            </sp:author>
            <sp:subcount>0</sp:subcount>
            <sp:link rel="self" href="https://pollinator.spore.com/pollinator/atom/downloadQueue" />
        </sp:entry>
    </invisible-feeds>
</handshake>
*/

namespace SporeServer.Builder.AtomFeed.Templates.Pollinator
{
    public class HandshakeTemplate : IAtomFeedTemplate
    {
        private readonly string _xml;

        public HandshakeTemplate(SporeServerUser user, SporeServerSubscription[] subscriptions)
        {
            // <handshake />
            //
            var document = AtomFeedBuilder.CreateDocument("handshake");
            // <user-id />
            AtomFeedBuilder.AddCustomElement(document, "user-id", $"{user.Id}");
            // <screen-name />
            AtomFeedBuilder.AddCustomElement(document, "screen-name", $"{user.UserName}");
            // <next-id />
            AtomFeedBuilder.AddCustomElement(document, "next-id", $"{user.NextAssetId}");
            // <refresh-rate />
            AtomFeedBuilder.AddCustomElement(document, "refresh-rate", "120");

            // <maxis-feeds />
            //
            AtomFeedBuilder.AddCustomElement(document, "maxis-feeds");
            /* We can remove this because
             * the official server returns an empty feed at the url anyways
             * maybe allow custom sporecasts here in the future?
            AtomFeedBuilder.AddFeedEntry(document, maxisFeeds,
                id: "tag:spore.com,2006:maxis/adventures/en_US",
                title: "Maxis Adventures",
                updated: DateTime.Now,
                subtitle: "Free creations from Maxis.",
                authorName: null,
                authorUri: null,
                subCount: 0,
                link: "https://pollinator.spore.com/pollinator/atom/maxis/adventures/en_US");
            */

            // <my-feeds />
            //
            var myFeeds = AtomFeedBuilder.AddCustomElement(document, "my-feeds");
            AtomFeedBuilder.AddFeedEntry(document, myFeeds,
                id: $"tag:spore.com,2006:user/{user.Id}",
                title: $"{user.UserName}",
                updated: DateTime.Now,
                subtitle: null,
                authorName: $"{user.UserName}",
                authorUri: $"{user.Id}",
                subCount: 0,
                link: $"https://pollinator.spore.com/pollinator/atom/user/{user.Id}");

            // <subscriptions />
            //
            var subscriptionsFeed = AtomFeedBuilder.AddCustomElement(document, "subscriptions");
            foreach (var subscription in subscriptions)
            {
                AtomFeedBuilder.AddFeedEntry(document, subscriptionsFeed,
                    id: $"tag:spore.com,2006:user/{subscription.UserId}",
                    title: $"{subscription.User.UserName}",
                    updated: DateTime.Now,
                    subtitle: null,
                    authorName: $"{subscription.User.UserName}",
                    authorUri: $"{subscription.UserId}",
                    subCount: 0,
                    link: $"https://pollinator.spore.com/pollinator/atom/user/{subscription.UserId}");
            }

            // <invisible-feeds />
            //
            var invisibleFeed = AtomFeedBuilder.AddCustomElement(document, "invisible-feeds");
            AtomFeedBuilder.AddFeedEntry(document, invisibleFeed,
                id: "tag:spore.com,2006:downloadQueue",
                title: $"{user.UserName}",
                updated: DateTime.Now,
                subtitle: null,
                authorName: $"{user.UserName}",
                authorUri: $"{user.Id}",
                subCount: 0,
                link: "https://pollinator.spore.com/pollinator/atom/downloadQueue");

            // save xml
            _xml = document.OuterXml;
        }

        public string Serialize()
        {
            return _xml;
        }
    }
}

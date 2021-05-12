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
    <user-id>501089776639</user-id>
    <screen-name>Rosalie1</screen-name>
    <next-id>501091342362</next-id>
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
        <sp:entry>
            <sp:id>tag:spore.com,2006:maxis/robotchicken/en_US</sp:id>
            <sp:title>Robot Chicken Adventures</sp:title>
            <sp:updated>2009-06-22T23:02:41.138Z</sp:updated>
            <sp:subtitle />
            <sp:subcount>0</sp:subcount>
            <sp:link rel="self" href="http://pollinator.spore.com/pollinator/atom/maxis/robotchicken/en_US" />
        </sp:entry>
    </maxis-feeds>
    <my-feeds>
        <entry>
            <id>tag:spore.com,2006:aggregator/501091337611</id>
            <title>adsasd</title>
            <updated>2021-05-09T12:53:39.413Z</updated>
            <author>
                <name>Rosalie1</name>
                <uri>501089776639</uri>
            </author>
            <subcount>0</subcount>
            <link rel="self" href="http://pollinator.spore.com/pollinator/atom/aggregator/501091337611" />
        </entry>
        <entry>
            <id>tag:spore.com,2006:aggregator/501091336836</id>
            <title>test sporecast</title>
            <updated>2021-05-09T12:22:01.768Z</updated>
            <subtitle>test sporecast description</subtitle>
            <author>
                <name>Rosalie1</name>
                <uri>501089776639</uri>
            </author>
            <subcount>0</subcount>
            <link rel="self" href="http://pollinator.spore.com/pollinator/atom/aggregator/501091336836" />
        </entry>
        <entry>
            <id>tag:spore.com,2006:user/501089776639</id>
            <title>Rosalie1</title>
            <updated>2021-04-19T10:53:41.754Z</updated>
            <author>
                <name>Rosalie1</name>
                <uri>501089776639</uri>
            </author>
            <subcount>0</subcount>
            <link rel="self" href="http://pollinator.spore.com/pollinator/atom/user/501089776639" />
        </entry>
    </my-feeds>
    <subscriptions>
        <entry>
            <id>tag:spore.com,2006:user/500590056227</id>
            <title>mark_live</title>
            <updated>2021-05-09T16:23:20.246Z</updated>
            <author>
                <name>mark_live</name>
                <uri>500590056227</uri>
            </author>
            <subcount>0</subcount>
            <link rel="self" href="http://pollinator.spore.com/pollinator/atom/user/500590056227" />
        </entry>
        <entry>
            <id>tag:spore.com,2006:user/501071978458</id>
            <title>TheAWKLORD</title>
            <updated>2021-05-05T04:37:29.476Z</updated>
            <author>
                <name>TheAWKLORD</name>
                <uri>501071978458</uri>
            </author>
            <subcount>0</subcount>
            <link rel="self" href="http://pollinator.spore.com/pollinator/atom/user/501071978458" />
        </entry>
        <entry>
            <id>tag:spore.com,2006:aggregator/500848614948</id>
            <title>現代と未来の融合</title>
            <updated>2021-05-09T16:04:50.050Z</updated>
            <subtitle>This Sporecast's a set of vehicles &amp; buildings that combine with modern &amp; future. 11/24/15 FEATURED!</subtitle>
            <author>
                <name>Itapenguin</name>
                <uri>500636289275</uri>
            </author>
            <subcount>18321</subcount>
            <link rel="self" href="http://pollinator.spore.com/pollinator/atom/aggregator/500848614948" />
        </entry>
    </subscriptions>
    <invisible-feeds>
        <sp:entry>
            <sp:id>tag:spore.com,2006:downloadQueue</sp:id>
            <sp:title>Rosalie1</sp:title>
            <sp:updated>2021-05-09T16:23:20.247Z</sp:updated>
            <sp:author>
                <sp:name>Rosalie1</sp:name>
                <sp:uri>501089776639</sp:uri>
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

        public HandshakeTemplate(SporeServerUser user, SporeServerAggregator[] aggregators, SporeServerUserSubscription[] userSubscriptions, SporeServerAggregatorSubscription[] aggregatorSubscriptions)
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
            foreach (var aggregator in aggregators)
            {
                AtomFeedBuilder.AddFeedEntry(document, myFeeds,
                    id: $"tag:spore.com,2006:aggregator/{aggregator.AggregatorId}",
                    title: $"{aggregator.Name}",
                    updated: DateTime.Now,
                    subtitle: null,
                    authorName: $"{aggregator.Author.UserName}",
                    authorUri: $"{aggregator.AuthorId}",
                    subCount: 0,
                    link: $"https://pollinator.spore.com/pollinator/atom/aggregator/{aggregator.AggregatorId}");
            }
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
            foreach (var subscription in userSubscriptions)
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
            foreach (var subscription in aggregatorSubscriptions)
            {
                AtomFeedBuilder.AddFeedEntry(document, myFeeds,
                    id: $"tag:spore.com,2006:aggregator/{subscription.AggregatorId}",
                    title: $"{subscription.Aggregator.Name}",
                    updated: subscription.Aggregator.Timestamp,
                    subtitle: $"{subscription.Aggregator.Description}",
                    authorName: $"{subscription.Aggregator.Author.UserName}",
                    authorUri: $"{subscription.Aggregator.AuthorId}",
                    subCount: 0,
                    link: $"https://pollinator.spore.com/pollinator/atom/aggregator/{subscription.AggregatorId}");
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

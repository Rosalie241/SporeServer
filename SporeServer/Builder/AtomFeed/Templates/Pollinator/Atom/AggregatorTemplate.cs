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
using SporeServer.Builder.AtomFeed.Helpers;
using SporeServer.SporeTypes;
using System;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

/*
<?xml version="1.0" encoding="UTF-8"?>
<feed xmlns="http://www.w3.org/2005/Atom" xmlns:sp="http://spore.com/atom" xml:lang="en_US">
    <id>tag:spore.com,2006:aggregator/501091337611</id>
    <title>adsasd</title>
    <updated>2021-05-09T12:53:39.413Z</updated>
    <subtitle />
    <author>
        <name>Rosalie1</name>
        <uri>501089776639</uri>
    </author>
    <subcount>0</subcount>
    <link rel="self" href="http://www.spore.com/atom/aggregator/501091337611" />
    <entry>
        <id>tag:spore.com,2006:asset/501090401010</id>
        <title>Nogamuu</title>
        <link rel="alternate" href="http://www.spore.com/sporepedia#qry=sast-501090401010" />
        <updated>2021-04-08T21:26:39.375Z</updated>
        <published>2021-05-09T12:53:39.417Z</published>
        <author>
            <name>guppysaurus</name>
            <uri>500219676025</uri>
        </author>
        <modeltype>0x9ea3031a</modeltype>
        <locale>en_US</locale>
        <modeltype>0x9ea3031a</modeltype>
        <sp:ownership>
            <sp:original name="id" type="int">501090401010</sp:original>
            <sp:parent name="id" type="int">0</sp:parent>
        </sp:ownership>
        <content type="html">
            <img src="http://static.spore.com/static/thumb/501/090/401/501090401010.png" />
        </content>
        <link rel="enclosure" href="http://static.spore.com/static/thumb/501/090/401/501090401010.png" type="image/png" length="27952" />
        <link rel="enclosure" href="http://static.spore.com/static/model/501/090/401/501090401010.xml" type="application/x-creature+xml" />
        <summary />
    </entry>
</feed>
*/

namespace SporeServer.Builder.AtomFeed.Templates.Pollinator.Atom
{
    public class AggregatorTemplate : IAtomFeedTemplate
    {
        private readonly string _xml;

        public AggregatorTemplate(SporeServerAggregator aggregator, int subscriberCount)
        {
            // <feed />
            //
            var document = AtomFeedBuilder.CreateDocument("feed");
            // <id />
            AtomFeedBuilder.AddCustomElement(document, "id", $"tag:spore.com,2006:aggregator/{aggregator.AggregatorId}");
            // <title />
            AtomFeedBuilder.AddCustomElement(document, "title", $"{aggregator.Name}");
            // <updated />
            AtomFeedBuilder.AddCustomElement(document, "updated", $"{XmlConvert.ToString(aggregator.Timestamp, XmlDateTimeSerializationMode.Utc)}");
            // <subtitle />
            AtomFeedBuilder.AddCustomElement(document, "subtitle", $"{aggregator.Description}");
            // <author />
            AtomFeedBuilder.AddAuthorElement(document, $"{aggregator.Author.UserName}", $"{aggregator.Author.Id}");
            // <subcount />
            AtomFeedBuilder.AddCustomElement(document, "subcount", $"{subscriberCount}");
            // <link />
            AtomFeedBuilder.AddLinkElement(document, "self", $"https://www.spore.com/pollinator/atom/aggregator/{aggregator.AggregatorId}", null, null);

            // add assets to feed
            SporeAtomFeedHelper.AddAssetsToFeed(document, aggregator.Assets.ToArray());

            // save xml
            _xml = document.OuterXml;
        }

        public string Serialize()
        {
            return _xml;
        }
    }
}

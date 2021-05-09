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
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

/*
<?xml version="1.0" encoding="UTF-8"?>
<feed xmlns="http://www.w3.org/2005/Atom" xmlns:sp="http://spore.com/atom" xml:lang="en_US">
    <id>tag:spore.com,2006:user/501089776639</id>
    <title>Rosalie1</title>
    <updated>2021-04-19T10:53:41.754Z</updated>
    <author>
        <name>Rosalie1</name>
        <uri>501089776639</uri>
    </author>
    <subcount>0</subcount>
    <link rel="self" href="http://pollinator.spore.com/pollinator/atom/user/501089776639" />
    <entry>
        <id>tag:spore.com,2006:asset/501090305614</id>
        <title>Pluffy</title>
        <link rel="alternate" href="http://pollinator.spore.com/pollinator/sporepedia#qry=sast-501090305614" />
        <updated>2021-04-05T18:26:27.449Z</updated>
        <author>
            <name>Rosalie1</name>
            <uri>501089776639</uri>
        </author>
        <modeltype>0x372e2c04</modeltype>
        <locale>en_US</locale>
        <modeltype>0x372e2c04</modeltype>
        <sp:ownership>
            <sp:original name="id" type="int">0</sp:original>
            <sp:parent name="id" type="int">501089944899</sp:parent>
        </sp:ownership>
        <content type="html">
            <img src="http://static.spore.com/static/thumb/501/090/305/501090305614.png" />
        </content>
        <link rel="enclosure" href="http://static.spore.com/static/thumb/501/090/305/501090305614.png" type="image/png" length="25725" />
        <link rel="enclosure" href="http://static.spore.com/static/model/501/090/305/501090305614.xml" type="application/x-creature+xml" />
        <summary />
        <category scheme="consequence" term="0xcac124d" />
        <category scheme="consequence" term="0x17e5ef84" />
        <category scheme="consequence" term="0xcfb01b93" />
    </entry>
</feed>
*/

namespace SporeServer.Builder.AtomFeed.Templates.Pollinator.Atom
{
    public class UserTemplate : IAtomFeedTemplate
    {
        private readonly string _xml;

        public UserTemplate(SporeServerUser author, SporeServerAsset[] assets)
        {
            // <feed />
            //
            var document = AtomFeedBuilder.CreateDocument("feed");
            // <id />
            AtomFeedBuilder.AddCustomElement(document, "id", $"tag:spore.com,2006:user/{author.Id}");
            // <title />
            AtomFeedBuilder.AddCustomElement(document, "title", $"{author.UserName}");
            // <updated />
            // TODO
            AtomFeedBuilder.AddCustomElement(document, "updated", $"{XmlConvert.ToString(DateTime.Now, XmlDateTimeSerializationMode.Utc)}");
            // <author />
            AtomFeedBuilder.AddAuthorElement(document, $"{author.UserName}", $"{author.Id}");
            // <subcount />
            // TODO?
            AtomFeedBuilder.AddCustomElement(document, "subcount", "0");
            // <link />
            AtomFeedBuilder.AddLinkElement(document, "self", $"https://pollinator.spore.com/pollinator/atom/user/{author.Id}", null, null);

            // add assets to feed
            SporeAtomFeedHelper.AddAssetsToFeed(document, assets);

            // save xml
            _xml = document.OuterXml;
        }

        public string Serialize()
        {
            return _xml;
        }
    }
}

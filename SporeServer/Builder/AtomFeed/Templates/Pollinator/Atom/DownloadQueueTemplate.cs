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
using System.Xml;
using System.Xml.Serialization;

/*
<?xml version="1.0" encoding="UTF-8"?>
<feed xmlns="http://www.w3.org/2005/Atom" xmlns:sp="http://spore.com/atom" xml:lang="en_US">
    <id>tag:spore.com,2006:downloadQueue</id>
    <title>Rosalie1</title>
    <updated>2021-05-04T13:37:29.031Z</updated>
    <author>
        <name>Rosalie1</name>
        <uri>501089776639</uri>
    </author>
    <subcount>0</subcount>
    <link rel="self" href="https://pollinator.spore.com/pollinator/atom/downloadQueue" />
    <entry>
        <id>tag:spore.com,2006:asset/501090913915</id>
        <title>Olliphantus</title>
        <link rel="alternate" href="http://pollinator.spore.com/pollinator/sporepedia#qry=sast-501090913915" />
        <updated>2021-04-24T05:24:12.442Z</updated>
        <author>
            <name>TheAWKLORD</name>
            <uri>501071978458</uri>
        </author>
        <modeltype>0x9ea3031a</modeltype>
        <locale>en_US</locale>
        <modeltype>0x9ea3031a</modeltype>
        <sp:ownership>
            <sp:original name="id" type="int">501090913915</sp:original>
            <sp:parent name="id" type="int">0</sp:parent>
        </sp:ownership>
        <content type="html">
            <img src="http://static.spore.com/static/thumb/501/090/913/501090913915.png" />
        </content>
        <link rel="enclosure" href="http://static.spore.com/static/thumb/501/090/913/501090913915.png" type="image/png" length="26076" />
        <link rel="enclosure" href="http://static.spore.com/static/model/501/090/913/501090913915.xml" type="application/x-creature+xml" />
        <summary>The largest living terrestrial organism on Sammar, these herd creatures have almost no natural predators when fully grown. They live in close knit family groups and use a variety of trumpet vocals &amp; tusks to intimidate/communicate.</summary>
        <category scheme="tag" term="set:sarak" />
        <category scheme="tag" term="theawklord" />
        <category scheme="tag" term="wildlife" />
    </entry>
</feed>
*/

namespace SporeServer.Builder.AtomFeed.Templates.Pollinator.Atom
{
    [XmlRoot("feed", Namespace = "http://www.w3.org/2005/Atom")]
    public class DownloadQueueTemplate : IAtomFeedTemplate
    {
        private readonly string _xml;

        public DownloadQueueTemplate(SporeServerUser user, SporeServerAsset[] assets)
        {
            // <feed />
            //
            var document = AtomFeedBuilder.CreateDocument("feed");
            // <id />
            AtomFeedBuilder.AddCustomElement(document, "id", "tag:spore.com,2006:downloadQueue");
            // <title />
            AtomFeedBuilder.AddCustomElement(document, "title", $"{user.UserName}");
            // <updated />
            AtomFeedBuilder.AddCustomElement(document, "updated", $"{XmlConvert.ToString(DateTime.Now, XmlDateTimeSerializationMode.Utc)}");
            // <author />
            AtomFeedBuilder.AddAuthorElement(document, $"{user.UserName}", $"{user.Id}");
            // <subcount />
            AtomFeedBuilder.AddCustomElement(document, "subcount", "0");
            // <link />
            AtomFeedBuilder.AddLinkElement(document, "self", "https://pollinator.spore.com/pollinator/atom/downloadQueue", null, null);

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

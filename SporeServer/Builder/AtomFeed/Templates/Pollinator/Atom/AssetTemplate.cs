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
using System.Xml;
using System.Xml.Serialization;

/*
<?xml version="1.0" encoding="UTF-8"?>
<feed xmlns="http://www.w3.org/2005/Atom" xmlns:sp="http://spore.com/atom">
    <id>tag:spore.com,2006:asset</id>
    <title>asset</title>
    <updated>2021-04-30T14:33:53.298Z</updated>
    <link rel="self" href="http://pollinator.spore.com/pollinator/atom/asset" />
    <entry>
        <id>tag:spore.com,2006:asset/501090966660</id>
        <title>Daeva Warrior</title>
        <link rel="alternate" href="http://pollinator.spore.com/pollinator/sporepedia#qry=sast-501090966660" />
        <updated>2021-04-25T18:26:28.531Z</updated>
        <author>
            <name>Meelnur</name>
            <uri>500899537400</uri>
        </author>
        <modeltype>0x9ea3031a</modeltype>
        <locale>en_US</locale>
        <modeltype>0x9ea3031a</modeltype>
        <sp:ownership>
            <sp:original name="id" type="int">501090966660</sp:original>
            <sp:parent name="id" type="int">0</sp:parent>
        </sp:ownership>
        <content type="html">
            <img src="http://static.spore.com/static/thumb/501/090/966/501090966660.png" />
        </content>
        <link rel="enclosure" href="http://static.spore.com/static/thumb/501/090/966/501090966660.png" type="image/png" length="35587" />
        <link rel="enclosure" href="http://static.spore.com/static/model/501/090/966/501090966660.xml" type="application/x-creature+xml" />
        <summary />
        <category scheme="tag" term="alien" />
        <category scheme="tag" term=" daeva" />
        <category scheme="tag" term=" meelnur" />
    </entry>
</feed>
*/

namespace SporeServer.Builder.AtomFeed.Templates.Pollinator.Atom
{
    [XmlRoot("feed", Namespace = "http://www.w3.org/2005/Atom")]
    public class AssetTemplate : IAtomFeedTemplate
    {
        private readonly string _xml;

        public AssetTemplate(SporeServerAsset[] assets)
        {
            // <feed />
            //
            var document = AtomFeedBuilder.CreateDocument("feed");
            // <id />
            AtomFeedBuilder.AddCustomElement(document, "id", "tag:spore.com,2006:asset");
            // <title />
            AtomFeedBuilder.AddCustomElement(document, "title", "asset");
            // <updated />
            AtomFeedBuilder.AddCustomElement(document, "updated", $"{XmlConvert.ToString(DateTime.Now, XmlDateTimeSerializationMode.Utc)}");
            // <link />
            AtomFeedBuilder.AddLinkElement(document, "self", "https://pollinator.spore.com/pollinator/atom/asset", null, null);

            // when the asset doesn't exist,
            // return an empty feed
            if (assets == null)
            {
                _xml = document.OuterXml;
                return;
            }

            // add each asset to the atom feed
            foreach (var asset in assets)
            {
                AddAssetFeedEntry(document, asset);
            }

            // save xml
            _xml = document.OuterXml;
        }

        public string Serialize()
        {
            return _xml;
        }

        /// <summary>
        ///     Creates an ATOM feed entry for the given asset
        /// </summary>
        /// <param name="document"></param>
        /// <param name="asset"></param>
        private void AddAssetFeedEntry(XmlDocument document, SporeServerAsset asset)
        {
            // <entry />
            //
            var entryFeed = AtomFeedBuilder.AddFeedEntry(document,
               id: $"tag:spore.com,2006:asset/{asset.AssetId}",
               title: $"{asset.Name}",
               updated: asset.Timestamp ?? DateTime.Now,
               subtitle: null,
               authorName: $"{asset.Author.UserName}",
               authorUri: $"{asset.Author.Id}",
               subCount: null,
               link: null);
            // <locale />
            // TODO
            AtomFeedBuilder.AddCustomElement(document, entryFeed, "locale", "en_US");
            // <modeltype />
            AtomFeedBuilder.AddCustomElement(document, entryFeed, "modeltype", $"{((Int64)asset.ModelType):x2}");

            // <ownership />
            //
            var ownershipElement = AtomFeedBuilder.AddCustomElement(document, entryFeed, "ownership");
            // <original />
            // TODO?
            var originalElement = AtomFeedBuilder.AddCustomElement(document, ownershipElement, "original", "0");
            AtomFeedBuilder.AddCustomAttribute(document, originalElement, "name", "id");
            AtomFeedBuilder.AddCustomAttribute(document, originalElement, "type", "int");
            // <parent />
            var parentElement = AtomFeedBuilder.AddCustomElement(document, ownershipElement, "parent", $"{asset.ParentAssetId}");
            AtomFeedBuilder.AddCustomAttribute(document, parentElement, "name", "id");
            AtomFeedBuilder.AddCustomAttribute(document, parentElement, "type", "int");

            // <content />
            //
            var contentElement = AtomFeedBuilder.AddCustomElement(document, entryFeed, "content");
            AtomFeedBuilder.AddCustomAttribute(document, contentElement, "type", "html");
            // <img />
            var imgElement = AtomFeedBuilder.AddCustomElement(document, contentElement, "img");
            AtomFeedBuilder.AddCustomAttribute(document, imgElement, "src", "https://static.spore.com/static/thumb/600/000/000/600000000002.png");

            // <link />
            //
            AtomFeedBuilder.AddLinkElement(document, entryFeed,
                rel: "enclosure",
                url: "https://static.spore.com/static/thumb/600/000/000/600000000002.png",
                type: "image/png",
                length: $"{asset.Size}");

            // <link />
            //
            AtomFeedBuilder.AddLinkElement(document, entryFeed,
                rel: "enclosure",
                url: "https://static.spore.com/static/model/600/000/000/600000000002.xml",
                type: "application/x-creature+xml",
                length: null);

            // <summary />
            //
            AtomFeedBuilder.AddCustomElement(document, entryFeed, "summary", $"{asset.Description}");

            // TODO, tags
        }
    }
}

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
using SporeServer.SporeTypes;
using System;
using System.Xml;
using System.Xml.Serialization;

/* Creature
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
/* Adventure
<?xml version="1.0" encoding="UTF-8"?>
<feed xmlns="http://www.w3.org/2005/Atom" xmlns:sp="http://spore.com/atom">
    <id>tag:spore.com,2006:asset</id>
    <title>asset</title>
    <updated>2021-05-02T22:10:23.462Z</updated>
    <link rel="self" href="http://pollinator.spore.com/pollinator/atom/asset" />
    <entry>
        <id>tag:spore.com,2006:asset/501090702061</id>
        <title>Чивилевич-финал</title>
        <link rel="alternate" href="http://pollinator.spore.com/pollinator/sporepedia#qry=sast-501090702061" />
        <updated>2021-04-17T15:24:16.033Z</updated>
        <author>
            <name>ArtyomZulg</name>
            <uri>501072193207</uri>
        </author>
        <modeltype>0x287adcdc</modeltype>
        <locale>ru_RU</locale>
        <modeltype>0x287adcdc</modeltype>
        <sp:stats>
            <sp:stat name="playcount" type="int">0</sp:stat>
            <sp:stat name="difficulty" type="int">3</sp:stat>
            <sp:stat name="rating" type="float">6.0</sp:stat>
            <sp:stat name="pointvalue" type="int">15</sp:stat>
        </sp:stats>
        <sp:images>
            <sp:image_1 type="image/png" href="http://static.spore.com/static/image/501/090/702/501090702061_lrg.png" />
            <sp:image_2 type="image/png" href="http://static.spore.com/static/image/501/090/702/501090702061_2_lrg.png" />
            <sp:image_3 type="image/png" href="http://static.spore.com/static/image/501/090/702/501090702061_3_lrg.png" />
        </sp:images>
        <sp:ownership>
            <sp:original name="id" type="int">501090702061</sp:original>
            <sp:parent name="id" type="int">0</sp:parent>
        </sp:ownership>
        <content type="html">
            <img src="http://static.spore.com/static/thumb/501/090/702/501090702061.png" />
        </content>
        <link rel="enclosure" href="http://static.spore.com/static/thumb/501/090/702/501090702061.png" type="image/png" length="64311" />
        <link rel="enclosure" href="http://static.spore.com/static/model/501/090/702/501090702061.xml" type="application/x-adventure+xml" />
        <summary>Чивилевич - Опаснейшее существо! Помоги Чивилевичу поймать Чивилевича! Это последний шанс...</summary>
        <category scheme="tag" term="artyomzulg" />
        <category scheme="tag" term=" truboglaz" />
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

            // add each asset to the atom feed
            foreach (var asset in assets)
            {
                if (asset != null)
                {
                    AddAssetFeedEntry(document, asset);
                }
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
            AtomFeedBuilder.AddCustomElement(document, entryFeed, "modeltype", $"0x{((Int64)asset.ModelType):x2}");

            // <sp:stats />
            // TODO
            if (asset.Type == SporeAssetType.Adventure)
            {
                var statsElement = AtomFeedBuilder.AddCustomElement(document, entryFeed, "sp:stats");

                // <sp:stat name="playcount" />
                var statElement = AtomFeedBuilder.AddCustomElement(document, statsElement, "sp:stat", "0");
                AtomFeedBuilder.AddCustomAttribute(document, statElement, "name", "playcount");
                AtomFeedBuilder.AddCustomAttribute(document, statElement, "type", "int");
                // <sp:stat name="difficulty" />
                statElement = AtomFeedBuilder.AddCustomElement(document, statsElement, "sp:stat", "0");
                AtomFeedBuilder.AddCustomAttribute(document, statElement, "name", "difficulty");
                AtomFeedBuilder.AddCustomAttribute(document, statElement, "type", "int");
                // <sp:stat name="rating" />
                statElement = AtomFeedBuilder.AddCustomElement(document, statsElement, "sp:stat", "10.0");
                AtomFeedBuilder.AddCustomAttribute(document, statElement, "name", "rating");
                AtomFeedBuilder.AddCustomAttribute(document, statElement, "type", "float");
                // <sp:stat name="pointvalue" />
                statElement = AtomFeedBuilder.AddCustomElement(document, statsElement, "sp:stat", "0");
                AtomFeedBuilder.AddCustomAttribute(document, statElement, "name", "pointvalue");
                AtomFeedBuilder.AddCustomAttribute(document, statElement, "type", "int");
            }

            // <sp:images />
            //
            if (asset.Type == SporeAssetType.Adventure)
            {
                var imagesElement = AtomFeedBuilder.AddCustomElement(document, entryFeed, "sp:images", null);

                // <sp:image_1 />
                if (asset.ImageFileUrl != null)
                {
                    AtomFeedBuilder.AddLinkElement(document, imagesElement,
                            name: "sp:image_1",
                            rel: null,
                            url: $"https://static.spore.com/{asset.ImageFileUrl}",
                            type: "image/png",
                            length: null);
                }
                // <sp:image_2 />
                if (asset.ImageFile2Url != null)
                {
                    AtomFeedBuilder.AddLinkElement(document, imagesElement,
                            name: "sp:image_2",
                            rel: null,
                            url: $"https://static.spore.com/{asset.ImageFile2Url}",
                            type: "image/png",
                            length: null);
                }
                // <sp:image_3 />
                if (asset.ImageFile3Url != null)
                {
                    AtomFeedBuilder.AddLinkElement(document, imagesElement,
                            name: "sp:image_3",
                            rel: null,
                            url: $"https://static.spore.com/{asset.ImageFile3Url}",
                            type: "image/png",
                            length: null);
                }
                // <sp:image_4 />
                if (asset.ImageFile4Url != null)
                {
                    AtomFeedBuilder.AddLinkElement(document, imagesElement,
                            name: "sp:image_4",
                            rel: null,
                            url: $"https://static.spore.com/{asset.ImageFile4Url}",
                            type: "image/png",
                            length: null);
                }
            }

            // <sp:ownership />
            //
            var ownershipElement = AtomFeedBuilder.AddCustomElement(document, entryFeed, "sp:ownership");
            // <sp:original />
            // TODO?
            var originalElement = AtomFeedBuilder.AddCustomElement(document, ownershipElement, "sp:original", $"{asset.AssetId}");
            AtomFeedBuilder.AddCustomAttribute(document, originalElement, "name", "id");
            AtomFeedBuilder.AddCustomAttribute(document, originalElement, "type", "int");
            // <sp:parent />
            var parentElement = AtomFeedBuilder.AddCustomElement(document, ownershipElement, "sp:parent", $"{asset.ParentAssetId}");
            AtomFeedBuilder.AddCustomAttribute(document, parentElement, "name", "id");
            AtomFeedBuilder.AddCustomAttribute(document, parentElement, "type", "int");

            // <content />
            //
            var contentElement = AtomFeedBuilder.AddCustomElement(document, entryFeed, "content");
            AtomFeedBuilder.AddCustomAttribute(document, contentElement, "type", "html");
            // <img />
            var imgElement = AtomFeedBuilder.AddCustomElement(document, contentElement, "img");
            AtomFeedBuilder.AddCustomAttribute(document, imgElement, "src", $"https://static.spore.com/{asset.ThumbFileUrl}");

            // <link />
            //
            AtomFeedBuilder.AddLinkElement(document, entryFeed,
                rel: "enclosure",
                url: $"https://static.spore.com/{asset.ThumbFileUrl}",
                type: "image/png",
                length: $"{asset.Size}");

            // <link />
            //
            AtomFeedBuilder.AddLinkElement(document, entryFeed,
                rel: "enclosure",
                url: $"https://static.spore.com/{asset.ModelFileUrl}",
                type: $"application/x-{asset.Type.ToString().ToLower()}+xml",
                length: null);

            // <summary />
            //
            AtomFeedBuilder.AddCustomElement(document, entryFeed, "summary", $"{asset.Description}");

            // TODO, tags
        }
    }
}

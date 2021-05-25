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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace SporeServer.Builder.AtomFeed.Helpers
{
    public static class SporeAtomFeedHelper
    {
        /// <summary>
        ///     Creates an ATOM feed entry for the given asset
        /// </summary>
        /// <param name="document"></param>
        /// <param name="asset"></param>
        public static void AddAssetFeedEntry(XmlDocument document, SporeServerAsset asset)
        {
            // <entry />
            //
            var entryFeed = AtomFeedBuilder.AddFeedEntry(document,
               id: $"tag:spore.com,2006:asset/{asset.AssetId}",
               title: $"{asset.Name}",
               updated: asset.Timestamp,
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

            // <category />
            //
            if (asset.Tags != null)
            {
                for (int i = 0; i < asset.Tags.Count; i++)
                {
                    // <category scheme="tag" term="tag1" />
                    // <category scheme="tag" term=" tag2" />
                    var tag = asset.Tags.ElementAt(i);
                    var categoryElement = AtomFeedBuilder.AddCustomElement(document, entryFeed, "category");
                    AtomFeedBuilder.AddCustomAttribute(document, categoryElement, "scheme", "tag");
                    AtomFeedBuilder.AddCustomAttribute(document, categoryElement, "term", $"{(i == 0 ? "" : " ")}{tag.Tag}");
                }
            }
            if (asset.Traits != null)
            {
                foreach (var trait in asset.Traits)
                {
                    // <category scheme="consequence" term="0x2b35f523" />
                    var categoryElement = AtomFeedBuilder.AddCustomElement(document, entryFeed, "category");
                    AtomFeedBuilder.AddCustomAttribute(document, categoryElement, "scheme", "consequence");
                    AtomFeedBuilder.AddCustomAttribute(document, categoryElement, "term", $"0x{(Int64)trait.TraitType:x}");
                }
            }
        }

        /// <summary>
        ///     Creates ATOM feed entries for the given assets
        /// </summary>
        /// <param name="document"></param>
        /// <param name="asset"></param>
        public static void AddAssetsToFeed(XmlDocument document, SporeServerAsset[] assets)
        {
            if (assets == null)
            {
                return;
            }

            foreach (var asset in assets)
            {
                if (asset != null)
                {
                    AddAssetFeedEntry(document, asset);
                }
            }
        }
    }
}

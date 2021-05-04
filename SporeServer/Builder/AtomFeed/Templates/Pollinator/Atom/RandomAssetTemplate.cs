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
<feed xmlns="http://www.w3.org/2005/Atom" xmlns:sp="http://spore.com/atom">
   <id>tag:spore.com,2006:randomAsset</id>
   <title>randomAsset</title>
   <updated>2021-04-08T22:00:46.968Z</updated>
   <link rel="self" href="http://pollinator.spore.com/pollinator/atom/randomAsset" />
   <entry>
      <id>tag:spore.com,2006:asset/501090401010</id>
      <title>Nogamuu</title>
      <link rel="alternate" href="http://pollinator.spore.com/pollinator/sporepedia#qry=sast-501090401010" />
      <updated>2021-04-08T21:26:39.375Z</updated>
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
   <entry>
      <id>tag:spore.com,2006:asset/501090396778</id>
      <title>лолчикус</title>
      <link rel="alternate" href="http://pollinator.spore.com/pollinator/sporepedia#qry=sast-501090396778" />
      <updated>2021-04-08T18:59:22.441Z</updated>
      <author>
         <name>truboglaz</name>
         <uri>501082990799</uri>
      </author>
      <modeltype>0x9ea3031a</modeltype>
      <locale>ru_RU</locale>
      <modeltype>0x9ea3031a</modeltype>
      <sp:ownership>
         <sp:original name="id" type="int">501090396778</sp:original>
         <sp:parent name="id" type="int">0</sp:parent>
      </sp:ownership>
      <content type="html">
         <img src="http://static.spore.com/static/thumb/501/090/396/501090396778.png" />
      </content>
      <link rel="enclosure" href="http://static.spore.com/static/thumb/501/090/396/501090396778.png" type="image/png" length="27881" />
      <link rel="enclosure" href="http://static.spore.com/static/model/501/090/396/501090396778.xml" type="application/x-creature+xml" />
      <summary />
   </entry>
</feed>
*/

namespace SporeServer.Builder.AtomFeed.Templates.Pollinator.Atom
{
    [XmlRoot("feed", Namespace = "http://www.w3.org/2005/Atom")]
    public class RandomAssetTemplate : IAtomFeedTemplate
    {
        private readonly string _xml;

        public RandomAssetTemplate(SporeServerAsset[] assets)
        {
            // <feed />
            //
            var document = AtomFeedBuilder.CreateDocument("feed");
            // <id />
            AtomFeedBuilder.AddCustomElement(document, "id", "tag:spore.com,2006:randomAsset");
            // <title />
            AtomFeedBuilder.AddCustomElement(document, "title", "randomAsset");
            // <updated />
            AtomFeedBuilder.AddCustomElement(document, "updated", $"{XmlConvert.ToString(DateTime.Now, XmlDateTimeSerializationMode.Utc)}");
            // <link />
            AtomFeedBuilder.AddLinkElement(document, "self", "https://pollinator.spore.com/pollinator/atom/randomAsset", null, null);

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

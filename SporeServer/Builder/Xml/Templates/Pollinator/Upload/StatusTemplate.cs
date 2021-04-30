/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
using System;
using System.Xml.Serialization;

/*
<?xml version="1.0" encoding="UTF-8"?>
<PollinatorResponse>
    <next-id>501091100422</next-id>
    <Status>Success</Status>
</PollinatorResponse>
*/

namespace SporeServer.Builder.Xml.Templates.Pollinator.Upload
{
    [XmlRoot("PollinatorResponse")]
    public class StatusTemplate : IXmlTemplate
    {
        private readonly string _xml;

        public StatusTemplate(Int64 nextId, bool success)
        {
            // <PollinatorResponse />
            //
            var document = XmlBuilder.CreateDocument("PollinatorResponse");
            // <next-id />
            XmlBuilder.AddElement(document, "next-id", $"{nextId}");
            // <Status />
            XmlBuilder.AddElement(document, "Status", success ? "Success" : "Failed");

            // save xml
            _xml = document.OuterXml;
        }

        public string Serialize()
        {
            return _xml;
        }
    }
}

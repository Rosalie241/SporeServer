/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
using Microsoft.AspNetCore.Mvc;
using System.Xml.Serialization;

namespace SporeServer.ContentResultHelper.Xml
{

    public class XmlBuilder
    {
        private readonly string _xml;

        public XmlBuilder(string xml)
        {
            _xml = xml;
        }

        public static XmlBuilder CreateFromTemplate<Type>(object template)
        {
            using var stringWriter = new Utf8StringWriter();
            var xmlSerializer = new XmlSerializer(typeof(Type));

            xmlSerializer.Serialize(stringWriter, template);

            return new XmlBuilder(stringWriter.ToString());
        }

        public ContentResult ToContentResult()
        {
            return new ContentResult()
            {
                ContentType = "text/xml",
                Content = _xml
            };
        }
    }
}

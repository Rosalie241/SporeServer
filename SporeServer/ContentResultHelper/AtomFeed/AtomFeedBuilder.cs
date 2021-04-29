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
using SporeServer.ContentResultHelper.AtomFeed.Types;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SporeServer.ContentResultHelper.AtomFeed
{
    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }

    public class AtomFeedBuilder
    {
        private readonly string _xml;

        public AtomFeedBuilder(string xml)
        {
            _xml = xml;
        }

        public static AtomFeedBuilder CreateFromTemplate<Type>(object template)
        {
            using var stringWriter = new Utf8StringWriter();
            var xmlSerializer = new XmlSerializer(typeof(Type));

            xmlSerializer.Serialize(stringWriter, template, ((BaseClass)template).Namespaces);

            return new AtomFeedBuilder(stringWriter.ToString());
        }

        public ContentResult ToContentResult()
        {
            return new ContentResult()
            {
                ContentType = "application/atom+xml",
                Content = _xml
            };
        }
    }
}

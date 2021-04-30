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
using System.Xml;

namespace SporeServer.Builder.Xml
{

    public class XmlBuilder
    {
        private readonly string _xml;

        public XmlBuilder(string xml)
        {
            _xml = xml;
        }

        public static XmlBuilder CreateFromTemplate(object template)
        {
            return new XmlBuilder(((IXmlTemplate)template).Serialize());
        }

        public ContentResult ToContentResult()
        {
            return new ContentResult()
            {
                ContentType = "application/atom+xml",
                Content = _xml,
            };
        }

        public static XmlDocument CreateDocument(string name)
        {
            var document = new XmlDocument();

            // <?xml />
            //
            var declaration = document.CreateXmlDeclaration("1.0", "UTF-8", null);
            document.InsertBefore(declaration, document.DocumentElement);

            // <{name} />
            //
            var element = document.CreateElement(name);
            document.AppendChild(element);

            return document;
        }

        public static XmlElement AddElement(XmlDocument document, XmlElement parent, string name, string value)
        {
            XmlElement element = document.CreateElement(name);
            if (value != null)
            {
                element.InnerText = value;
            }
            parent.AppendChild(element);
            return element;
        }

        public static XmlElement AddElement(XmlDocument document, string name, string value)
        {
            return AddElement(document, (XmlElement)document.LastChild, name, value);
        }
    }
}

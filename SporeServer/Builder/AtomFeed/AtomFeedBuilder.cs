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
using System;
using System.Xml;

namespace SporeServer.Builder.AtomFeed
{
    public class AtomFeedBuilder
    {
        private readonly string _xml;

        public AtomFeedBuilder(string xml)
        {
            _xml = xml;
        }

        public static AtomFeedBuilder CreateFromTemplate(object template)
        {
            return new AtomFeedBuilder(((IAtomFeedTemplate)template).Serialize());
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
            AddCustomAttribute(document, element, "xmlns", "http://www.w3.org/2005/Atom");
            AddCustomAttribute(document, element, "xmlns:sp", "http://spore.com/atom");
            document.AppendChild(element);

            return document;
        }

        public static XmlElement AddCustomElement(XmlDocument document, XmlElement parent, string name, string value)
        {
            var element = document.CreateElement(name);
            if (value != null)
            {
                element.InnerText = value;
            }
            parent.AppendChild(element);
            return element;
        }

        public static XmlElement AddCustomElement(XmlDocument document, XmlElement parent, string name)
        {
            return AddCustomElement(document, parent, name, null);
        }

        public static XmlElement AddCustomElement(XmlDocument document, string name, string value)
        {
            return AddCustomElement(document, (XmlElement)document.LastChild, name, value);
        }

        public static XmlElement AddCustomElement(XmlDocument document, string name)
        {
            return AddCustomElement(document, name, null);
        }

        public static XmlAttribute AddCustomAttribute(XmlDocument document, XmlElement element, string name, string value)
        {
            var attribute = document.CreateAttribute(name);
            attribute.InnerText = value;
            element.Attributes.Append(attribute);
            return attribute;
        }

        public static XmlElement AddFeedEntry(XmlDocument document, XmlElement parent, string id, string title, DateTime updated, string subtitle, string authorName, string authorUri, int? subCount, string link)
        {
            // <entry />
            //
            var entryElement = AddCustomElement(document, parent, "entry", null);
            // <id />
            AddCustomElement(document, entryElement, "id", id);
            // <title />
            AddCustomElement(document, entryElement, "title", title);
            // <updated />
            AddCustomElement(document, entryElement, "updated", XmlConvert.ToString(updated, XmlDateTimeSerializationMode.Utc));
            // <subtitle />
            if (subtitle != null)
            {
                AddCustomElement(document, entryElement, "subtitle", subtitle);
            }

            // <author />
            //
            if (authorName != null || authorUri != null)
            {
                var authorElement = AddCustomElement(document, entryElement, "author", null);
                // <name />
                AddCustomElement(document, authorElement, "name", authorName);
                // <uri />
                AddCustomElement(document, authorElement, "uri", authorUri);
            }

            // <subcount />
            if (subCount != null)
            {
                AddCustomElement(document, entryElement, "subcount", subCount.ToString());
            }

            // <link />
            if (link != null)
            {
                AddLinkElement(document, entryElement, "self", link, null, null);
            }

            return entryElement;
        }

        public static XmlElement AddFeedEntry(XmlDocument document, string id, string title, DateTime updated, string subtitle, string authorName, string authorUri, int? subCount, string link)
        {
            return AddFeedEntry(document, (XmlElement)document.LastChild, id, title, updated, subtitle, authorName, authorUri, subCount, link);
        }

        public static XmlElement AddLinkElement(XmlDocument document, XmlElement parent, string rel, string url, string type, string length)
        {
            // <link />
            //
            var linkElement = AddCustomElement(document, parent, "link");
            AddCustomAttribute(document, linkElement, "rel", rel);
            AddCustomAttribute(document, linkElement, "href", url);

            if (type != null)
            {
                AddCustomAttribute(document, linkElement, "type", type);
            }

            if (length != null)
            {
                AddCustomAttribute(document, linkElement, "length", length);
            }

            return linkElement;
        }

        public static XmlElement AddLinkElement(XmlDocument document, string rel, string url, string type, string length)
        {
            return AddLinkElement(document, (XmlElement)document.LastChild, rel, url, type, length);
        }

    }
}

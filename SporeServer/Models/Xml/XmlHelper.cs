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
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

#nullable enable annotations

namespace SporeServer.Models.Xml
{
    public static class XmlHelper
    {
        /// <summary>
        ///     Parses Int64 from XmlNode (supports 0x hex format)
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Int64 ParseInt64(XmlNode node)
        {
            string value = node.InnerText;

            if (value.StartsWith("0x"))
            {
                // hex
                value = value.Remove(0, 2);
                return Convert.ToInt64(value, 16);
            }
            else
            {
                // not hex
                return Convert.ToInt64(value);
            }
        }

        /// <summary>
        ///     Parses Int64 from XmlNode (supports 0x hex format),
        ///     returns null when node is null
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Int64? ParseInt64_Null(XmlNode? node)
        {
            if (node == null)
            {
                return null;
            }

            string value = node.InnerText;

            if (value.StartsWith("0x"))
            {
                // hex
                value = value.Remove(0, 2);
                return Convert.ToInt64(value, 16);
            }
            else
            {
                // not hex
                return Convert.ToInt64(value);
            }
        }

        /// <summary>
        ///     Parses Int32 from XmlNode,
        ///     returns null when node is null
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Int32? ParseInt32_Null(XmlNode? node)
        {
            if (node == null)
            {
                return null;
            }

            string value = node.InnerText;

            if (value.StartsWith("0x"))
            {
                // hex
                value = value.Remove(0, 2);
                return Convert.ToInt32(value, 16);
            }
            else
            {
                // not hex
                return Convert.ToInt32(value);
            }
        }

        /// <summary>
        ///     Parses Double from XmlNode,
        ///     returns null when node is null
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static double? ParseDouble(XmlNode? node)
        {
            if (node == null)
            {
                return null;
            }

            return Convert.ToDouble(node.InnerText, CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Parses Double List from XmlNode,
        ///     returns null when node is null
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static double[]? ParseDoubleList(XmlNode? node)
        {
            if (node == null)
            {
                return null;
            }

            List<double> items = new List<double>();

            foreach (string item in node.InnerText.Split(','))
            {
                items.Add(double.Parse(item.Trim(), CultureInfo.InvariantCulture));
            }

            return items.ToArray();
        }

        /// <summary>
        ///     Parses Int64 List from XmlNode,
        ///     returns null when node is null
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Int64[]? ParseInt64List(XmlNode? node)
        {
            if (node == null)
            {
                return null;
            }

            List<Int64> items = new List<Int64>();

            foreach (string item in node.InnerText.Split(','))
            {
                string realItem = item.Trim();
                if (realItem.StartsWith("0x"))
                {
                    // hex
                    realItem = realItem.Remove(0, 2);
                    items.Add(Convert.ToInt64(realItem, 16));
                }
                else
                {
                    // not hex
                    items.Add(Convert.ToInt64(realItem));
                }
            }

            return items.ToArray();
        }

        /// <summary>
        ///     Parses string from XmlNode,
        ///     returns null when node is null
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string ParseString_Null(XmlNode? node)
        {
            if (node == null)
            {
                return null;
            }

            return node.InnerText;
        }

        /// <summary>
        ///     Creates XmlElement with name and value in XmlDocument with XmlElement as parent
        /// </summary>
        /// <param name="document"></param>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static XmlElement CreateXmlElement(XmlDocument document, XmlElement parent, string name, string value)
        {
            XmlElement element = document.CreateElement(name);
            if (value != null)
            {
                element.InnerText = value;
            }
            parent.AppendChild(element);
            return element;
        }

        /// <summary>
        ///     Creates XmlAttribute with name and value in XmlDocument with XmlElement as parent
        /// </summary>
        /// <param name="document"></param>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static XmlAttribute CreateXmlAttribute(XmlDocument document, XmlElement parent, string name, string value)
        {
            XmlAttribute attribute = document.CreateAttribute(name);
            if (value != null)
            {
                attribute.InnerText = value;
            }
            parent.Attributes.Append(attribute);
            return attribute;
        }
    }
}

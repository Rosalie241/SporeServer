/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
using System.Xml.Serialization;

namespace SporeServer.ContentResultHelper.AtomFeed.Types
{
    public class Link
    {
        public Link()
        {

        }

        public Link(string url)
        {
            Href = url;
            Rel = "self";
        }

        [XmlAttribute("rel")]
        public string Rel { get; set; }

        [XmlAttribute("href")]
        public string Href { get; set; }
    }
}

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
using System.Xml;
using System.Xml.Serialization;

namespace SporeServer.ContentResultHelper.AtomFeed.Types
{
    [XmlType("entry")]
    public class Entry
    {
        [XmlElement("id")]
        public string Id { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        private DateTime _updated { get; set; }

        [XmlElement("updated")]
        public DateTime Updated 
        {
            get { return XmlConvert.ToDateTime(XmlConvert.ToString(_updated, XmlDateTimeSerializationMode.Utc), XmlDateTimeSerializationMode.Utc);  }
            set { _updated = value; }  
        }

        [XmlElement("author")]
        public Author Author { get; set; }

        [XmlElement("subtitle")]
        public string Subtitle { get; set; }

        [XmlElement("subcount")]
        public int SubCount { get; set; }

        [XmlElement("link")]
        public Link Link { get; set; }
    }
}

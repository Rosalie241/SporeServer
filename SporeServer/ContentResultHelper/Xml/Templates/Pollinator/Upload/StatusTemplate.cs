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

namespace SporeServer.ContentResultHelper.Xml.Templates.Pollinator.Upload
{
    [XmlRoot("PollinatorResponse")]
    public class StatusTemplate
    {
        public StatusTemplate()
        {
        }

        public StatusTemplate(Int64 nextId, bool success)
        {
            NextId = nextId;
            Status = success ? "Success" : "Failed";
        }

        [XmlElement("next-id")]
        public Int64 NextId { get; set; }

        [XmlElement("Status")]
        public string Status { get; set; }
    }
}

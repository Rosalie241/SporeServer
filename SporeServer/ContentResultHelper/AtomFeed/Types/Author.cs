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
using System.Xml.Serialization;

namespace SporeServer.ContentResultHelper.AtomFeed.Types
{
    public class Author
    {
        public Author()
        {

        }

        public Author(SporeServerUser user)
        {
            Name = user.UserName;
            Uri = user.Id.ToString();
        }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("uri")]
        public string Uri { get; set; }
    }
}

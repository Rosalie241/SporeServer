/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
using System.Xml;
using System.Xml.Serialization;

namespace SporeServer.ContentResultHelper.AtomFeed.Types
{
    public class BaseClass
    {
        public BaseClass()
        {
            this.Namespaces = new XmlSerializerNamespaces(new XmlQualifiedName[]
            {
                // TODO, use actual current URL?
                new XmlQualifiedName("sp", "http://spore.com/atom"),
            });
        }

        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Namespaces { get; set; }
    }
}

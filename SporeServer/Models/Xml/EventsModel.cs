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
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SporeServer.Models.Xml
{
    public class EventsModel
    {
        public EventsModelEvent[] Events { get; set; }

        public static async Task<EventsModel> SerializeAsync(Stream stream)
        {
            var eventsModel = new EventsModel();
            var events = new List<EventsModelEvent>();
            var document = await XDocument.LoadAsync(stream, LoadOptions.None, new CancellationToken());
            var rootElement = document.Root;

            eventsModel.Events = rootElement.Descendants("event").Select(eventNode => new EventsModelEvent()
            {
                Verb = Int64.Parse(eventNode.Attribute("verb").Value.TrimStart('0', 'x'),
                                    System.Globalization.NumberStyles.HexNumber),
                AssetId = Int64.Parse(eventNode.Attribute("assetid").Value.TrimStart('0', 'x'),
                                    System.Globalization.NumberStyles.HexNumber),
                Args = eventNode.Descendants("arg").Select(a => Int64.Parse(a.Value)).ToArray()
            }).ToArray();

            return eventsModel;
        }
    }
}

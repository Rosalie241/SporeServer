using Microsoft.AspNetCore.Mvc;
using SporeServer.AtomFeed.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SporeServer.AtomFeed
{
    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }

    public class AtomFeedBuilder
    {
        public AtomFeedBuilder(string xml)
        {
            _xml = xml;
        }

        public static AtomFeedBuilder CreateFromTemplate<Type>(object template)
        {
            Utf8StringWriter stringWriter = new Utf8StringWriter();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Type));

            xmlSerializer.Serialize(stringWriter, template, ((BaseClass)template).Namespaces);

            return new AtomFeedBuilder(stringWriter.ToString());
        }


        public ContentResult ToContentResult()
        {
            return new ContentResult()
            {
                ContentType = "text/xml",
                Content = _xml
            };
        }

        private string _xml { get; set; }
    }
}

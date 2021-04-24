using SporeServer.Areas.Identity.Data;
using SporeServer.AtomFeed.Types;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SporeServer.AtomFeed.Templates.Pollinator
{
    [XmlRoot("handshake", Namespace = "http://www.w3.org/2005/Atom")]
    public class HandshakeTemplate : BaseClass
    {
        public HandshakeTemplate()
        {
        }

        public HandshakeTemplate(SporeServerUser user)
        {
            //BigInteger a = BigInteger.Parse(Guid.NewGuid().ToString().Replace("-", ""), NumberStyles.AllowHexSpecifier);

            Author author = new Author()
            {
                Name = user.UserName,
                Uri = user.Id.ToString()
            };

            //UserId = "501089776639";
            UserId = user.Id.ToString();
            ScreenName = author.Name;


            Console.WriteLine(user.NextAssetId);
            // TODO
            NextId = user.NextAssetId; // 501089776639; // a.ToString();

            // TODO, what does this mean?
            RefreshRate = 120;

            MaxisFeeds = new List<Entry>()
            {
                new Entry()
                {
                    Id = "tag:spore.com,2006:maxis/adventures/en_US",
                    Title = "Maxis Adventures",
                    Updated = DateTime.Now,
                    Subtitle = "Free creations from Maxis.",
                    SubCount = 0,
                    Link = new Link("https://pollinator.spore.com/pollinator/atom/maxis/adventures/en_US")
                }
            };
            MyFeeds = new List<Entry>()
            {
                new Entry()
                {
                    //Id = "tag:spore.com,2006:user/501089776639",
                    Id = $"tag:spore.com,2006:user/{user.Id}",
                    Title = user.UserName,
                    Updated = DateTime.Now,
                    Author = author,
                    SubCount = 0,
                    //Link = new Link("http://pollinator.spore.com/pollinator/atom/user/501089776639")
                    Link = new Link($"https://pollinator.spore.com/pollinator/atom/user/{user.Id}")
                }
            };
            /*
            Subscriptions = new List<Entry>();
            if (user.Buddies != null)
            {
                foreach (SporeServerUser buddy in user.Buddies)
                {
                    Console.WriteLine(buddy.UserName);
                    Subscriptions.Add(new Entry()
                    {
                        //Id = $"tag:spore.com,2006:user/501071978458",
                        Id = $"tag:spore.com,2006:user/{buddy.Id}",
                        Title = buddy.UserName,
                        Updated = DateTime.Now,
                        Author = new Author()
                        {
                            Name = buddy.UserName,
                            //Uri = "501071978458"
                            Uri = buddy.Id
                        },
                        SubCount = 0,
                        // Link = new Link("http://pollinator.spore.com/pollinator/atom/user/501071978458")
                        Link = new Link($"http://pollinator.spore.com/pollinator/atom/user/{buddy.Id}")
                    });
                }
                
            }*/

            InvisibleFeeds = new List<Entry>()
            {
                new Entry()
                {
                    Id = "tag:spore.com,2006:downloadQueue",
                    Title = user.UserName,
                    Updated = DateTime.Now,
                    Author = author,
                    SubCount = 0,
                    Link = new Link("https://pollinator.spore.com/pollinator/atom/downloadQueue")
                }
            };
        }

        [XmlElement("user-id")]
        public string UserId { get; set; }

        [XmlElement("screen-name")]
        public string ScreenName { get; set; }

        [XmlElement("next-id")]
        public Int64 NextId { get; set; }

        [XmlElement("refresh-rate")]
        public int RefreshRate { get; set; }

        [XmlArray("maxis-feeds")]
        public List<Entry> MaxisFeeds { get; set; }

        [XmlArray("my-feeds")]
        public List<Entry> MyFeeds { get; set; }

        [XmlArray("subscriptions")]
        public List<Entry> Subscriptions { get; set; }

        [XmlArray("invisible-feeds")]
        public List<Entry> InvisibleFeeds { get; set; }
    }
}

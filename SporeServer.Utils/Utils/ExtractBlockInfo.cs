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

namespace SporeServer.Utils.Utils
{
    internal class ExtractBlockInfo : IUtil
    {
        private static void TryReadFile(string filename, string id, string[] directories)
        {
            string fullFileName = "";

            foreach (string dir in directories)
            {
                string tmpFullFileName = Path.Combine(dir, filename + ".prop.prop_t");
                if (File.Exists(tmpFullFileName))
                {
                    fullFileName = tmpFullFileName;
                }
            }

            if (String.IsNullOrEmpty(fullFileName))
            {
                Console.WriteLine($"{filename} {id} doesn't exist!");
                return;
            }

            string[] lines = File.ReadAllLines(fullFileName);
            string[] items =
            {
                "modelPrice",
                "modelComplexityScore",
                "modelCapabilityHealth",

                "modelCapabilityBite",
                "modelCapabilityVocalize",
                "modelCapabilityMouth",
                "modelCapabilityCarnivorous",
                "modelCapabilityHerbivorous",

                "modelCapabilityCharge",
                "modelCapabilityStrike",
                "modelCapabilityGrasper",
                "modelCapabilityFlaunt",

                "modelCapabilityPosture",

                "modelCapabilityDance",
                "modelCapabilityCreatureSpeed",
                "modelCapabilityJump",
                "modelCapabilityFoot",

                "modelCapabilitySpit",
                "modelCapabilitySpine"
            };

            Console.WriteLine("                new SporeBlock()");
            Console.WriteLine("                {");
            Console.WriteLine($"                    // filename: {filename}");
            Console.WriteLine($"                    BlockId = {id},");
            if (fullFileName.Contains("creature_rigblock~"))
            {
                // 0x40626000
                Console.WriteLine("                    BlockType = SporeBlockType.Creature,");
            }
            else if (fullFileName.Contains("tool_rigblock~"))
            {
                // 0x40686000
                Console.WriteLine("                    BlockType = SporeBlockType.Tribal,");
            }

            foreach (string line in lines)
            {
                foreach (string str in items)
                {
                    if (line.Contains(str))
                    {
                        Console.WriteLine($"                    {str.Remove(0, 5)} = {line.Split(str)[1].Trim()},");
                    }
                }
            }
            Console.WriteLine("                },");
        }

        public void Execute(string[] args)
        {
            string blockMapFileName = args[0];
            string[] searchDirectories =
            {
                Path.Combine(args[1], "building_rigblocks~"),
                Path.Combine(args[1], "vehicle_rigblock~"),
                Path.Combine(args[1], "creature_rigblock~"),
                Path.Combine(args[1], "tool_rigblock~"),
                Path.Combine(args[2], "creature_rigblock~"),
            };

            XmlDocument xmlDocument = new XmlDocument();

            xmlDocument.Load(blockMapFileName);

            var rootElement = xmlDocument.DocumentElement;
            if (rootElement == null)
            {
                throw new Exception("rootElement == null!");
            }

            foreach (XmlElement blockXmlElement in rootElement.SelectNodes("block"))
            {
                string id = blockXmlElement.SelectSingleNode("id").InnerText;
                string filename = blockXmlElement.SelectSingleNode("filename").InnerText;

                TryReadFile(filename, id, searchDirectories);
            }
        }

        public string GetName()
        {
            return "ExtractBlockInfo";
        }

        public string GetUsage()
        {
            return "  ExtractBlockInfo [block map file] [SporeModder FX Spore Game & Graphics directory] [SporeModder FX BoosterPack_01 directory]";
        }
    }
}

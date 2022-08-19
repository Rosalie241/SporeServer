/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
using System.Globalization;

namespace SporeServer.Utils.Utils
{
    internal class ExtractArcheTypeInfo : IUtil
    {
        struct Vector3
        {
            public double Num1;
            public double Num2;
            public double Num3;
        };

        struct Vector3s
        {
            public string Name;
            public Vector3[] Vector3Array;
        };

        private static Vector3s ReadVector3sFromLines(string[] lines, int startingIndex)
        {
            string startingLine = lines[startingIndex];
            if (!startingLine.StartsWith("vector3s"))
            {
                throw new Exception("not a vector!");
            }

            List<Vector3> vector3List = new List<Vector3>();

            int currentIndex = startingIndex + 1;
            do
            {
                string vectorLine = lines[currentIndex];
                string[] vectorNumbers;
                vectorLine = vectorLine.Split("(")[1].Split(")")[0];
                vectorNumbers = vectorLine.Split(",");

                vector3List.Add(new Vector3()
                {
                    Num1 = Double.Parse(vectorNumbers[0], CultureInfo.InvariantCulture),
                    Num2 = Double.Parse(vectorNumbers[1], CultureInfo.InvariantCulture),
                    Num3 = Double.Parse(vectorNumbers[2], CultureInfo.InvariantCulture)
                });

                currentIndex++;
            } while (!lines[currentIndex].StartsWith("end"));

            return new Vector3s()
            {
                Name = startingLine.Split(' ')[1],
                Vector3Array = vector3List.ToArray()
            };
        }

        private static string GetRangeString(List<Vector3s> parentVector3sList, List<Vector3s> vector3sList, string name)
        {
            Vector3s vector3s = vector3sList.Where(v => v.Name == name).FirstOrDefault();
            if (String.IsNullOrEmpty(vector3s.Name))
            {
                vector3s = parentVector3sList.Where(v => v.Name == name).FirstOrDefault();
                if (String.IsNullOrEmpty(vector3s.Name))
                {
                    return "N/A";
                }
            }

            double isNA = vector3s.Vector3Array[0].Num1;
            double minimum = vector3s.Vector3Array[0].Num2;
            double maximum = vector3s.Vector3Array[0].Num3;

            if (isNA == 2)
            {
                return "N/A";
            }

            if (minimum == maximum)
            {
                return $"{minimum}";
            }

            return $"{minimum}-{maximum}";
        }

        private static List<Vector3s> AddParentPropFileVector3s(string keyLine, string directory)
        {
            List<Vector3s> vector3sList = new List<Vector3s>();
            List<Vector3s> parentVector3sList = new List<Vector3s>();

            string parentFile = keyLine.Split("key parent herdtypes~!")[1];
            if (parentFile == "default.prop")
            {
                return vector3sList;
            }

            string fullPath = Path.Combine(directory, parentFile + ".prop_t");
            string[] lines = File.ReadAllLines(fullPath);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                if (line.StartsWith("key parent herdtypes~!"))
                {
                    parentVector3sList.AddRange(AddParentPropFileVector3s(line, directory));
                }
                else if (line.StartsWith("vector3s"))
                {
                    vector3sList.Add(ReadVector3sFromLines(lines, i));
                }
            }

            // parent properties go last
            vector3sList.AddRange(parentVector3sList);

            return vector3sList;
        }

        private static void ExtractPropInfo(string path)
        {
            string[] lines = File.ReadAllLines(path);

            List<Vector3s> vector3sList = new List<Vector3s>();
            List<Vector3s> parentVector3sList = new List<Vector3s>();

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                // handle parent
                if (line.StartsWith("key parent herdtypes~!"))
                {
                    parentVector3sList.AddRange(AddParentPropFileVector3s(line, Path.GetDirectoryName(path)));
                }

                if (line.StartsWith("vector3s"))
                {
                    vector3sList.Add(ReadVector3sFromLines(lines, i));
                }
            }

            string propFile = Path.GetFileName(path).Split(".prop.prop_t")[0];
            string mouthType = "N/A";
            string cost = "N/A";
            string graspers = "N/A";
            string feet = "N/A";
            string singLevel = "N/A";
            string danceLevel = "N/A";
            string charmLevel = "N/A";
            string poseLevel = "N/A";
            string totalSocial = "N/A";
            string biteLevel = "N/A";
            string strikeLevel = "N/A";
            string chargeLevel = "N/A";
            string spitLevel = "N/A";
            string totalAttack = "N/A";
            string healthLevel = "N/A";

            Vector3s carnivoreVector3s = vector3sList.Where(v => v.Name == "carnivore").FirstOrDefault();
            Vector3s herbivoreVector3s = vector3sList.Where(v => v.Name == "herbivore").FirstOrDefault();
            if (!String.IsNullOrEmpty(carnivoreVector3s.Name) && !String.IsNullOrEmpty(herbivoreVector3s.Name))
            {
                Vector3 carnivoreVector = carnivoreVector3s.Vector3Array[0];
                Vector3 herbivoreVector = herbivoreVector3s.Vector3Array[0];

                if ((carnivoreVector.Num2 == 1 && carnivoreVector.Num3 == 2) &&
                    (herbivoreVector.Num2 == 1 && herbivoreVector.Num3 == 2))
                {
                    mouthType = "Omni.";
                }
                else if ((carnivoreVector.Num1 == 2) &&
                        (herbivoreVector.Num2 == 1 && herbivoreVector.Num3 == 2))
                {
                    mouthType = "Herb. and Omni.";
                }
                else if ((carnivoreVector.Num2 == 1 && carnivoreVector.Num3 == 2) &&
                        (herbivoreVector.Num1 == 2))
                {
                    mouthType = "Carn. and Omni.";
                }
                else if ((carnivoreVector.Num2 == 0 && carnivoreVector.Num3 == 0) &&
                        (herbivoreVector.Num2 == 1 && herbivoreVector.Num3 == 2))
                {
                    mouthType = "Herb.";
                }
                else if ((carnivoreVector.Num2 == 1 && carnivoreVector.Num3 == 2) &&
                        (herbivoreVector.Num2 == 0 && herbivoreVector.Num3 == 0))
                {
                    mouthType = "Carn.";
                }
            }
            else if (!String.IsNullOrEmpty(carnivoreVector3s.Name))
            {
                Vector3 carnivoreVector = carnivoreVector3s.Vector3Array[0];

                if (carnivoreVector.Num2 == 1 && carnivoreVector.Num3 == 2)
                {
                    mouthType = "Carn.";
                }
            }
            else if (!String.IsNullOrEmpty(herbivoreVector3s.Name))
            {
                Vector3 herbivoreVector = herbivoreVector3s.Vector3Array[0];

                if (herbivoreVector.Num2 == 1 && herbivoreVector.Num3 == 2)
                {
                    mouthType = "Herb.";
                }
            }

            cost = GetRangeString(parentVector3sList, vector3sList, "Cost");
            graspers = GetRangeString(parentVector3sList, vector3sList, "NumGraspers");
            feet = GetRangeString(parentVector3sList, vector3sList, "NumFeet");

            singLevel = GetRangeString(parentVector3sList, vector3sList, "singCapRange");
            danceLevel = GetRangeString(parentVector3sList, vector3sList, "danceCapRange");
            charmLevel = GetRangeString(parentVector3sList, vector3sList, "gestureCapRange");
            poseLevel = GetRangeString(parentVector3sList, vector3sList, "postureCapRange");
            totalSocial = GetRangeString(parentVector3sList, vector3sList, "TotalSocial");

            biteLevel = GetRangeString(parentVector3sList, vector3sList, "biteCapRange");
            strikeLevel = GetRangeString(parentVector3sList, vector3sList, "strikeCapRange");
            chargeLevel = GetRangeString(parentVector3sList, vector3sList, "chargeCapRange");
            spitLevel = GetRangeString(parentVector3sList, vector3sList, "spitCapRange");
            totalAttack = GetRangeString(parentVector3sList, vector3sList, "TotalAttack");

            healthLevel = GetRangeString(parentVector3sList, vector3sList, "healthCapRange");

            Console.WriteLine($"| {propFile} | {mouthType} | {cost} | {graspers} | {feet} | {singLevel} | {danceLevel} | {charmLevel} | {poseLevel} | {totalSocial} | {biteLevel} | {strikeLevel} | {chargeLevel} | {spitLevel} | {totalAttack} | {healthLevel} |");
        }

        public void Execute(string[] args)
        {
            string path = args[0];

            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException("path doesn't exist!");
            }

            Console.WriteLine("| Prop File     | Mouth Type  | Cost  | Graspers | Feet | Sing | Dance | Charm | Pose | Total Social | Bite | Strike | Charge | Spit | Total Attack | Health |");
            Console.WriteLine("|---------------|-------------|-------|----------|------|------|-------|-------|------|-------|------|--------|--------|------|-------|--------|");

            foreach (string file in Directory.GetFiles(path))
            {
                ExtractPropInfo(file);
            }
        }

        public string GetUsage()
        {
            return "  ExtractArcheTypeInfo [path of archetype props]";
        }

        public string GetName()
        {
            return "ExtractArcheTypeInfo";
        }
    }
}

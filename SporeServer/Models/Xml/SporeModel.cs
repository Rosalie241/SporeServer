using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace SporeServer.Models.Xml
{
    public class SporeModel
    {
        public Int32 FormatVersion { get; set; }
        public SporeModelProperties Properties { get; set; }
        public Int32 BlocksCount { get; set; }
        public SporeModelBlockRef[] Blocks { get; set; }

        public static SporeModel SerializeFromXml(Stream stream)
        {
            SporeModel model = new SporeModel();

            XmlDocument document = new XmlDocument();

            document.Load(stream);

            XmlElement rootElement = document.DocumentElement;
            XmlNode propertiesNode = rootElement.SelectSingleNode("properties");
            XmlNode blocksNode = rootElement.SelectSingleNode("blocks");

            model.FormatVersion = Int32.Parse(rootElement.SelectSingleNode("formatversion").InnerText);
            model.Properties = new SporeModelProperties()
            {
                ModelType = XmlHelper.ParseInt64(propertiesNode.SelectSingleNode("modeltype")),
                SkinEffect1 = XmlHelper.ParseInt64_Null(propertiesNode.SelectSingleNode("skineffect1")),
                SkinEffect2 = XmlHelper.ParseInt64_Null(propertiesNode.SelectSingleNode("skineffect2")),
                SkinEffect3 = XmlHelper.ParseInt64_Null(propertiesNode.SelectSingleNode("skineffect3")),
                SkinEffectSeed1 = XmlHelper.ParseInt64_Null(propertiesNode.SelectSingleNode("skineffectseed1")),
                SkinEffectSeed2 = XmlHelper.ParseInt64_Null(propertiesNode.SelectSingleNode("skineffectseed2")),
                SkinEffectSeed3 = XmlHelper.ParseInt64_Null(propertiesNode.SelectSingleNode("skineffectseed3")),
                SkinColor1 = XmlHelper.ParseDoubleList(propertiesNode.SelectSingleNode("skincolor1")),
                SkinColor2 = XmlHelper.ParseDoubleList(propertiesNode.SelectSingleNode("skincolor2")),
                SkinColor3 = XmlHelper.ParseDoubleList(propertiesNode.SelectSingleNode("skincolor3")),
                ZcorpScore = Int32.Parse(propertiesNode.SelectSingleNode("zcorpscore").InnerText)
            };
            model.BlocksCount = Int32.Parse(blocksNode.Attributes.GetNamedItem("count").InnerText);

            List<SporeModelBlockRef> blocks = new List<SporeModelBlockRef>();

            foreach (XmlNode blockNode in blocksNode.SelectNodes("blockref"))
            {
                XmlNode transformNode = blockNode.SelectSingleNode("transform");
                XmlNode transformOrientationNode = transformNode.SelectSingleNode("orientation");
                XmlNode transformUserOrientationNode = transformNode.SelectSingleNode("userorientation");
                XmlNode paintlistNode = blockNode.SelectSingleNode("paintlist");
                XmlNode childlistNode = blockNode.SelectSingleNode("childlist");
                XmlNode handlesNode = blockNode.SelectSingleNode("handles");

                SporeModelBlockRef block = new SporeModelBlockRef()
                {
                    BlockId = XmlHelper.ParseInt64List(blockNode.SelectSingleNode("blockid")),
                    Transform = new SporeModelBlockRefTransform()
                    {
                        LimbType = XmlHelper.ParseInt32(transformNode?.SelectSingleNode("limbtype")),
                        MuscleScale = XmlHelper.ParseDouble(transformNode?.SelectSingleNode("musclescale")),
                        BaseMuscleScale = XmlHelper.ParseDouble(transformNode?.SelectSingleNode("basemusclescale")),
                        Scale = double.Parse(transformNode.SelectSingleNode("scale").InnerText),
                        Position = XmlHelper.ParseDoubleList(transformNode.SelectSingleNode("position")),
                        TriangleDirection = XmlHelper.ParseDoubleList(transformNode.SelectSingleNode("triangledirection")),
                        TrianglePickOrigin = XmlHelper.ParseDoubleList(transformNode.SelectSingleNode("trianglepickorigin")),
                        Orientation = new SporeModelBlockRefTransformOrientation()
                        {
                            Row0 = XmlHelper.ParseDoubleList(transformOrientationNode.SelectSingleNode("row0")),
                            Row1 = XmlHelper.ParseDoubleList(transformOrientationNode.SelectSingleNode("row1")),
                            Row2 = XmlHelper.ParseDoubleList(transformOrientationNode.SelectSingleNode("row2"))
                        }
                    },
                    Snapped = bool.Parse(blockNode.SelectSingleNode("snapped").InnerText),
                    PaintListCount = XmlHelper.ParseInt32(paintlistNode?.Attributes.GetNamedItem("count")),
                    ChildListCount = XmlHelper.ParseInt32(childlistNode?.Attributes.GetNamedItem("count")),
                    HandlesCount = XmlHelper.ParseInt32(handlesNode?.Attributes.GetNamedItem("count")),
                    Symmetric = XmlHelper.ParseInt32(blockNode.SelectSingleNode("symmetric")),
                    IsAsymmetric = bool.Parse(blockNode.SelectSingleNode("isasymmetric").InnerText)
                };

                if (transformUserOrientationNode != null)
                {
                    block.Transform.UserOrientation = new SporeModelBlockRefTransformUserOrientation()
                    {
                        Row0 = XmlHelper.ParseDoubleList(transformUserOrientationNode.SelectSingleNode("userorientation_row0")),
                        Row1 = XmlHelper.ParseDoubleList(transformUserOrientationNode.SelectSingleNode("userorientation_row1")),
                        Row2 = XmlHelper.ParseDoubleList(transformUserOrientationNode.SelectSingleNode("userorientation_row2")),
                    };
                }

                if (paintlistNode != null)
                {
                    List<SporeModelBlockRefPaint> paintList = new List<SporeModelBlockRefPaint>();
                    foreach (XmlNode paintNode in paintlistNode.SelectNodes("paint"))
                    {
                        paintList.Add(new SporeModelBlockRefPaint()
                        {
                            PaintRegion = XmlHelper.ParseInt64(paintNode.SelectSingleNode("paintregion")),
                            PaintId = XmlHelper.ParseInt64(paintNode.SelectSingleNode("paintid")),
                            Color1 = XmlHelper.ParseDoubleList(paintNode.SelectSingleNode("color1")),
                            Color2 = XmlHelper.ParseDoubleList(paintNode.SelectSingleNode("color2"))
                        });
                    }
                    block.PaintList = paintList.ToArray();
                }

                if (childlistNode != null)
                {
                    List<Int64> childIdList = new List<Int64>();
                    foreach (XmlNode childIdNode in childlistNode.SelectNodes("childid"))
                    {
                        childIdList.Add(Int64.Parse(childIdNode.InnerText));
                    }
                    block.ChildList = childIdList.ToArray();
                }

                if (handlesNode != null)
                {
                    List<SporeModelBlockRefHandle> handles = new List<SporeModelBlockRefHandle>();
                    foreach (XmlNode handleNode in handlesNode.SelectNodes("weight"))
                    {
                        handles.Add(new SporeModelBlockRefHandle()
                        {
                            Channel = XmlHelper.ParseInt64(handleNode.Attributes.GetNamedItem("channel")),
                            Value = double.Parse(handleNode.InnerText)
                        });
                    }
                    block.Handles = handles.ToArray();
                }

                blocks.Add(block);
            }

            model.Blocks = blocks.ToArray();

            return model;
        }

        public static string DeserializeToxml(SporeModel model)
        {
            XmlDocument document = new XmlDocument();

            XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = document.DocumentElement;

            document.InsertBefore(declaration, root);

            // <sporemodel />
            XmlElement rootElement = document.CreateElement("sporemodel");
            document.AppendChild(rootElement);

            // <formatversion />
            XmlElement element = document.CreateElement("formatversion");
            element.InnerText = model.FormatVersion.ToString();
            rootElement.AppendChild(element);

            // <properties />
            element = document.CreateElement("properties");
            XmlHelper.CreateXmlElement(document, element, "modeltype", $"0x{model.Properties.ModelType:x}");
            if (model.Properties.SkinEffect1 != null)
            {
                XmlHelper.CreateXmlElement(document, element, "skineffect1", $"0x{model.Properties.SkinEffect1:x}");
                XmlHelper.CreateXmlElement(document, element, "skineffect2", $"0x{model.Properties.SkinEffect2:x}");
                XmlHelper.CreateXmlElement(document, element, "skineffect3", $"0x{model.Properties.SkinEffect3:x}");
                XmlHelper.CreateXmlElement(document, element, "skineffectseed1", model.Properties.SkinEffectSeed1.ToString());
                XmlHelper.CreateXmlElement(document, element, "skineffectseed2", model.Properties.SkinEffectSeed2.ToString());
                XmlHelper.CreateXmlElement(document, element, "skineffectseed3", model.Properties.SkinEffectSeed3.ToString());
                XmlHelper.CreateXmlElement(document, element, "skincolor1", String.Join(',', model.Properties.SkinColor1));
                XmlHelper.CreateXmlElement(document, element, "skincolor2", String.Join(',', model.Properties.SkinColor2));
                XmlHelper.CreateXmlElement(document, element, "skincolor3", String.Join(',', model.Properties.SkinColor3));
            }
            XmlHelper.CreateXmlElement(document, element, "zcorpscore", model.Properties.ZcorpScore.ToString());
            rootElement.AppendChild(element);

            // <blocks />
            element = document.CreateElement("blocks");
            XmlHelper.CreateXmlAttribute(document, element, "count", model.BlocksCount.ToString());

            foreach (var block in model.Blocks)
            {
                // <blockref />
                XmlElement blockElement = document.CreateElement("blockref");
                XmlHelper.CreateXmlElement(document, blockElement, "blockid", String.Join(", ", block.BlockId.Select(x => $"0x{x:x}")));

                // <transform />
                XmlElement transformElement = document.CreateElement("transform");
                if (block.Transform.LimbType != null)
                {
                    XmlHelper.CreateXmlElement(document, transformElement, "limbtype", block.Transform.LimbType.ToString());
                }
                if (block.Transform.MuscleScale != null)
                {
                    XmlHelper.CreateXmlElement(document, transformElement, "musclescale", block.Transform.MuscleScale.ToString());
                }
                if (block.Transform.BaseMuscleScale != null)
                {
                    XmlHelper.CreateXmlElement(document, transformElement, "basemusclescale", block.Transform.BaseMuscleScale.ToString());
                }
                XmlHelper.CreateXmlElement(document, transformElement, "scale", block.Transform.Scale.ToString());
                XmlHelper.CreateXmlElement(document, transformElement, "position", String.Join(',', block.Transform.Position));
                XmlHelper.CreateXmlElement(document, transformElement, "triangledirection", String.Join(',', block.Transform.TriangleDirection));
                XmlHelper.CreateXmlElement(document, transformElement, "trianglepickorigin", String.Join(',', block.Transform.TrianglePickOrigin));
                blockElement.AppendChild(transformElement);

                // <orientation />
                XmlElement orientationElement = document.CreateElement("orientation");
                XmlHelper.CreateXmlElement(document, orientationElement, "row0", String.Join(',', block.Transform.Orientation.Row0));
                XmlHelper.CreateXmlElement(document, orientationElement, "row1", String.Join(',', block.Transform.Orientation.Row1));
                XmlHelper.CreateXmlElement(document, orientationElement, "row2", String.Join(',', block.Transform.Orientation.Row2));
                transformElement.AppendChild(orientationElement);

                // <userorientation />
                if (block.Transform.UserOrientation != null)
                {
                    XmlElement userOrientationElement = document.CreateElement("userorientation");
                    XmlHelper.CreateXmlElement(document, userOrientationElement, "userorientation_row0", String.Join(',', block.Transform.UserOrientation.Row0));
                    XmlHelper.CreateXmlElement(document, userOrientationElement, "userorientation_row1", String.Join(',', block.Transform.UserOrientation.Row1));
                    XmlHelper.CreateXmlElement(document, userOrientationElement, "userorientation_row2", String.Join(',', block.Transform.UserOrientation.Row2));
                    transformElement.AppendChild(userOrientationElement);
                }

                XmlHelper.CreateXmlElement(document, blockElement, "snapped", block.Snapped.ToString().ToLower());

                // <paintlist />
                if (block.PaintListCount > 0)
                {
                    XmlElement paintListElement = document.CreateElement("paintlist");
                    XmlHelper.CreateXmlAttribute(document, paintListElement, "count", block.PaintListCount.ToString());
                    foreach (var paint in block.PaintList)
                    {
                        XmlElement paintElement = document.CreateElement("paint");
                        XmlHelper.CreateXmlElement(document, paintElement, "paintregion", $"0x{paint.PaintRegion:x}");
                        XmlHelper.CreateXmlElement(document, paintElement, "paintid", $"0x{paint.PaintId:x}");
                        XmlHelper.CreateXmlElement(document, paintElement, "color1", String.Join(',', paint.Color1));
                        XmlHelper.CreateXmlElement(document, paintElement, "color2", String.Join(',', paint.Color2));
                        paintListElement.AppendChild(paintElement);
                    }
                    blockElement.AppendChild(paintListElement);
                }

                // <childlist />
                if (block.ChildListCount > 0)
                {
                    XmlElement childlistElement = document.CreateElement("childlist");
                    XmlHelper.CreateXmlAttribute(document, childlistElement, "count", block.ChildListCount.ToString());
                    foreach (var childId in block.ChildList)
                    {
                        XmlHelper.CreateXmlElement(document, childlistElement, "childid", childId.ToString());
                    }
                    blockElement.AppendChild(childlistElement);
                }

                if (block.Symmetric != null)
                {
                    XmlHelper.CreateXmlElement(document, blockElement, "symmetric", block.Symmetric.ToString().ToLower());
                }

                // <handles />
                if (block.HandlesCount > 0)
                {
                    XmlElement handlesElement = document.CreateElement("handles");
                    XmlHelper.CreateXmlAttribute(document, handlesElement, "count", block.HandlesCount.ToString());
                    foreach (var handle in block.Handles)
                    {
                        XmlElement handleElement = XmlHelper.CreateXmlElement(document, handlesElement, "weight", handle.Value.ToString());
                        XmlHelper.CreateXmlAttribute(document, handleElement, "channel", $"0x{handle.Channel:x}");
                    }
                    blockElement.AppendChild(handlesElement);
                }

                XmlHelper.CreateXmlElement(document, blockElement, "isasymmetric", block.IsAsymmetric.ToString().ToLower());

                element.AppendChild(blockElement);
            }

            rootElement.AppendChild(element);

            return document.OuterXml;
        }

        public static void Validate(SporeModel model)
        {
            // TODO, add proper validation

            if (model.FormatVersion != 18)
            {
                throw new Exception($"Unsupported FormatVersion: {model.FormatVersion}");
            }

            if (model.Blocks.Length != model.BlocksCount)
            {
                throw new Exception("Block count != actual length");
            }

            if (model.BlocksCount <= 0)
            {
                throw new Exception("Block count cannot be 0");
            }

            foreach (var block in model.Blocks)
            {
                if (block.Handles != null)
                {
                    if (block.Handles.Length != block.HandlesCount)
                    {
                        throw new Exception("Handles count != actual length");
                    }
                }
            }
        }
    }
}

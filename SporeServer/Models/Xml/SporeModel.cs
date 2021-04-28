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
using System.Xml;

#nullable enable annotations

namespace SporeServer.Models.Xml
{
    public class SporeModel
    {
        public Int32? FormatVersion { get; set; }
        public SporeModelProperties Properties { get; set; }
        public Int32? BlocksCount { get; set; }
        public SporeModelBlockRef[] Blocks { get; set; }
        public Int64[]? Assets { get; set; }
        public SporeModelScenarioResource ScenarioResource { get; set; }

        public static SporeModel SerializeFromXml(Stream stream)
        {
            SporeModel model = new SporeModel();

            XmlDocument document = new XmlDocument();

            document.Load(stream);

            XmlElement rootElement = document.DocumentElement;
            XmlNode propertiesNode = rootElement.SelectSingleNode("properties");
            XmlNode blocksNode = rootElement.SelectSingleNode("blocks");
            XmlNode assetsNode = rootElement.SelectSingleNode("assets");
            XmlNode scenarioResourceNode = rootElement.SelectSingleNode("cScenarioResource");

            model.FormatVersion = XmlHelper.ParseInt32_Null(rootElement.SelectSingleNode("formatversion"));
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
                ZcorpScore = XmlHelper.ParseInt32_Null(propertiesNode.SelectSingleNode("zcorpscore")),
                Version = XmlHelper.ParseInt32_Null(propertiesNode.SelectSingleNode("version"))
            };

            if (blocksNode != null)
            {
                model.BlocksCount = XmlHelper.ParseInt32_Null(blocksNode.Attributes.GetNamedItem("count"));

                var blocks = new List<SporeModelBlockRef>();
                foreach (XmlNode blockNode in blocksNode.SelectNodes("blockref"))
                {
                    XmlNode transformNode = blockNode.SelectSingleNode("transform");
                    XmlNode transformOrientationNode = transformNode.SelectSingleNode("orientation");
                    XmlNode transformUserOrientationNode = transformNode.SelectSingleNode("userorientation");
                    XmlNode paintlistNode = blockNode.SelectSingleNode("paintlist");
                    XmlNode childlistNode = blockNode.SelectSingleNode("childlist");
                    XmlNode handlesNode = blockNode.SelectSingleNode("handles");

                    var block = new SporeModelBlockRef()
                    {
                        BlockId = XmlHelper.ParseInt64List(blockNode.SelectSingleNode("blockid")),
                        Transform = new SporeModelBlockRefTransform()
                        {
                            LimbType = XmlHelper.ParseInt32_Null(transformNode?.SelectSingleNode("limbtype")),
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
                        PaintListCount = XmlHelper.ParseInt32_Null(paintlistNode?.Attributes.GetNamedItem("count")),
                        ChildListCount = XmlHelper.ParseInt32_Null(childlistNode?.Attributes.GetNamedItem("count")),
                        HandlesCount = XmlHelper.ParseInt32_Null(handlesNode?.Attributes.GetNamedItem("count")),
                        Symmetric = XmlHelper.ParseInt32_Null(blockNode.SelectSingleNode("symmetric")),
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
                        var paintList = new List<SporeModelBlockRefPaint>();
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
                        var childIdList = new List<Int64>();
                        foreach (XmlNode childIdNode in childlistNode.SelectNodes("childid"))
                        {
                            childIdList.Add(Int64.Parse(childIdNode.InnerText));
                        }
                        block.ChildList = childIdList.ToArray();
                    }

                    if (handlesNode != null)
                    {
                        var handles = new List<SporeModelBlockRefHandle>();
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
            }

            if (assetsNode != null)
            {
                List<Int64> assets = new List<Int64>();
                foreach (XmlNode assetElement in assetsNode.SelectNodes("asset"))
                {
                    assets.Add(XmlHelper.ParseInt64(assetElement));
                }
                model.Assets = assets.ToArray();
            }

            if (scenarioResourceNode != null)
            {
                XmlNode actsNode = scenarioResourceNode.SelectSingleNode("acts");
                XmlNode classesNode = scenarioResourceNode.SelectSingleNode("classes");

                var scenarioResource = new SporeModelScenarioResource
                {
                    AvatarLocked = XmlHelper.ParseInt32_Null(scenarioResourceNode.SelectSingleNode("bAvatarLocked")),
                    AllowedPosseMembers = XmlHelper.ParseInt32_Null(scenarioResourceNode.SelectSingleNode("numAllowedPosseMembers")),
                    WinText = scenarioResourceNode.SelectSingleNode("winText").InnerText,
                    LoseText = scenarioResourceNode.SelectSingleNode("loseText").InnerText,
                    IntroText = scenarioResourceNode.SelectSingleNode("introText").InnerText,
                    Type = XmlHelper.ParseInt64_Null(scenarioResourceNode.SelectSingleNode("type")),
                    AvatarAsset = XmlHelper.ParseInt64List(scenarioResourceNode.SelectSingleNode("mAvatarAsset").SelectSingleNode("ID"))
                };

                if (actsNode != null)
                {
                    var acts = new List<SporeModelScenarioResourceAct>();

                    foreach (XmlNode actNode in actsNode.SelectNodes("cScenarioAct"))
                    {
                        var goals = new List<SporeModelScenarioResourceActGoal>();

                        var act = new SporeModelScenarioResourceAct
                        {
                            TimeLimitSecs = XmlHelper.ParseInt32_Null(actNode.SelectSingleNode("timeLimitSecs")),
                            Name = XmlHelper.ParseString_Null(actNode.SelectSingleNode("name")),
                            Description = XmlHelper.ParseString_Null(actNode.SelectSingleNode("description")),
                            TimeVisible = XmlHelper.ParseInt32_Null(actNode.SelectSingleNode("bTimeVisible")),
                            MusicId = XmlHelper.ParseInt64_Null(actNode.SelectSingleNode("actMusicId"))
                        };

                        XmlNode goalNode = actNode.SelectSingleNode("goals");

                        foreach (XmlNode scenarioNode in goalNode.SelectNodes("cScenarioGoal"))
                        {
                            XmlNode dialogsNode = scenarioNode.SelectSingleNode("dialog");

                            var goal = new SporeModelScenarioResourceActGoal();
                            var dialogs = new List<string>();

                            goal.Type = XmlHelper.ParseInt32_Null(scenarioNode.SelectSingleNode("type"));
                            if (dialogsNode != null)
                            {
                                foreach (XmlNode dialogNode in dialogsNode.SelectNodes("cScenarioDialog"))
                                {
                                    string text = XmlHelper.ParseString_Null(dialogNode.SelectSingleNode("text"));
                                    if (text != null)
                                    {
                                        dialogs.Add(text);
                                    }
                                }
                            }

                            goal.Dialogs = dialogs.ToArray();
                            goals.Add(goal);
                        }
                        act.Goals = goals.ToArray();

                        acts.Add(act);
                    }
                    scenarioResource.Acts = acts.ToArray();
                }
                
                if (classesNode != null)
                {
                    var classes = new List<SporeModelScenarioResourceClass>();

                    foreach (XmlNode classNode in classesNode.SelectNodes("cScenarioClass"))
                    {
                        var classActs = new List<SporeModelScenarioResourceClassAct>();
                        var singleClass = new SporeModelScenarioResourceClass();
                        var classActsNode = classNode.SelectSingleNode("acts");

                        foreach (XmlNode actNode in classActsNode.SelectNodes("cScenarioClassAct"))
                        {
                            XmlNode dialogChatterNode = actNode.SelectSingleNode("dialogs_chatter");
                            XmlNode dialogInspectNode = actNode.SelectSingleNode("dialogs_inspect");

                            var act = new SporeModelScenarioResourceClassAct();

                            var dialogsChatter = new List<string>();
                            var dialogsInspect = new List<string>();

                            if (dialogChatterNode != null)
                            {
                                foreach (XmlNode dialogNode in dialogChatterNode.SelectNodes("cScenarioDialog"))
                                {
                                    string text = XmlHelper.ParseString_Null(dialogNode.SelectSingleNode("text"));
                                    if (text != null)
                                    {
                                        dialogsChatter.Add(text);
                                    }
                                }
                                act.DialogsChatter = dialogsChatter.ToArray();
                            }

                            if (dialogInspectNode != null)
                            {
                                foreach (XmlNode dialogNode in dialogInspectNode.SelectNodes("cScenarioDialog"))
                                {
                                    string text = XmlHelper.ParseString_Null(dialogNode.SelectSingleNode("text"));
                                    if (text != null)
                                    {
                                        dialogsInspect.Add(text);
                                    }
                                }
                                act.DialogsInspect = dialogsInspect.ToArray();
                            }

                            classActs.Add(act);
                        }
                        singleClass.Acts = classActs.ToArray();

                        singleClass.CastName = XmlHelper.ParseString_Null(classNode.SelectSingleNode("castName"));

                        XmlNode assetNode = classNode.SelectSingleNode("mAsset");
                        if (assetNode != null)
                        {
                            var asset = new SporeModelScenarioResourceClassAsset
                            {
                                Key = XmlHelper.ParseInt64List(assetNode.SelectSingleNode("key")),
                                Id = XmlHelper.ParseInt64_Null(assetNode.SelectSingleNode("ID"))
                            };

                            singleClass.Asset = asset;
                        }

                        singleClass.GfxOverrideType = XmlHelper.ParseInt32_Null(classNode.SelectSingleNode("gfxOverideType"));
                        singleClass.GfxOverideTypeSecondary = XmlHelper.ParseInt32_Null(classNode.SelectSingleNode("gfxOverideTypeSecondary"));

                        XmlNode gfxOverrideAssetNode = classNode.SelectSingleNode("gameplayObjectGfxOverrideAsset");
                        if (gfxOverrideAssetNode != null)
                        {
                            var asset = new SporeModelScenarioResourceClassAsset
                            {
                                Key = XmlHelper.ParseInt64List(gfxOverrideAssetNode.SelectSingleNode("key")),
                                Id = XmlHelper.ParseInt64_Null(gfxOverrideAssetNode.SelectSingleNode("ID"))
                            };

                            singleClass.GfxOverrideAsset = asset;
                        }

                        classes.Add(singleClass);
                    }

                    scenarioResource.Classes = classes.ToArray();
                }
                

                model.ScenarioResource = scenarioResource;
            }

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

            XmlElement element;

            // <formatversion />
            if (model.FormatVersion != null)
            {
                element = document.CreateElement("formatversion");
                element.InnerText = model.FormatVersion.ToString();
                rootElement.AppendChild(element);
            }

            // <properties />
            element = document.CreateElement("properties");
            XmlHelper.CreateXmlElement(document, element, "modeltype", $"0x{model.Properties.ModelType:x}");
            if (model.Properties.SkinEffect1 != null)
            {
                XmlHelper.CreateXmlElement(document, element, "skineffect1", $"0x{model.Properties.SkinEffect1:x}");
            }
            if (model.Properties.SkinEffect2 != null)
            {
                XmlHelper.CreateXmlElement(document, element, "skineffect2", $"0x{model.Properties.SkinEffect2:x}");
            }
            if (model.Properties.SkinEffect3 != null)
            {
                XmlHelper.CreateXmlElement(document, element, "skineffect3", $"0x{model.Properties.SkinEffect3:x}");
            }
            if (model.Properties.SkinEffectSeed1 != null)
            {
                XmlHelper.CreateXmlElement(document, element, "skineffectseed1", model.Properties.SkinEffectSeed1.ToString());
            }
            if (model.Properties.SkinEffectSeed2 != null)
            {
                XmlHelper.CreateXmlElement(document, element, "skineffectseed2", model.Properties.SkinEffectSeed2.ToString());
            }
            if (model.Properties.SkinEffectSeed3 != null)
            {
                XmlHelper.CreateXmlElement(document, element, "skineffectseed3", model.Properties.SkinEffectSeed3.ToString());
            }
            if (model.Properties.SkinColor1 != null)
            {
                XmlHelper.CreateXmlElement(document, element, "skincolor1", String.Join(',', model.Properties.SkinColor1));
            }
            if (model.Properties.SkinColor2 != null)
            {
                XmlHelper.CreateXmlElement(document, element, "skincolor2", String.Join(',', model.Properties.SkinColor2));
            }
            if (model.Properties.SkinColor3 != null)
            {
                XmlHelper.CreateXmlElement(document, element, "skincolor3", String.Join(',', model.Properties.SkinColor3));
            }
            if (model.Properties.ZcorpScore != null)
            {
                XmlHelper.CreateXmlElement(document, element, "zcorpscore", model.Properties.ZcorpScore.ToString());
            }
            if (model.Properties.Version != null)
            {
                XmlHelper.CreateXmlElement(document, element, "version", $"0x{model.Properties.Version:x}");
            }
            rootElement.AppendChild(element);

            // <blocks />
            if (model.Blocks != null)
            {
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
            }

            // <assets />
            if (model.Assets != null)
            {
                element = document.CreateElement("assets");

                foreach (Int64 asset in model.Assets)
                {
                    XmlHelper.CreateXmlElement(document, element, "asset", asset.ToString());
                }

                rootElement.AppendChild(element);
            }

            // <cScenarioResource />
            if (model.ScenarioResource != null)
            {
                element = document.CreateElement("cScenarioResource");

                // <bAvatarLocked />
                if (model.ScenarioResource.AvatarLocked != null)
                {
                    XmlHelper.CreateXmlElement(document, element, "bAvatarLocked", model.ScenarioResource.AvatarLocked.ToString());
                }

                // <numAllowedPosseMembers />
                if (model.ScenarioResource.AllowedPosseMembers != null)
                {
                    XmlHelper.CreateXmlElement(document, element, "numAllowedPosseMembers", model.ScenarioResource.AllowedPosseMembers.ToString());
                }

                // <winText />
                if (model.ScenarioResource.WinText != null)
                {
                    XmlHelper.CreateXmlElement(document, element, "winText", model.ScenarioResource.WinText);
                }

                // <loseText />
                if (model.ScenarioResource.LoseText != null)
                {
                    XmlHelper.CreateXmlElement(document, element, "loseText", model.ScenarioResource.LoseText);
                }

                // <introText />
                if (model.ScenarioResource.IntroText != null)
                {
                    XmlHelper.CreateXmlElement(document, element, "introText", model.ScenarioResource.IntroText);
                }

                // <type />
                if (model.ScenarioResource.Type != null)
                {
                    XmlHelper.CreateXmlElement(document, element, "type", model.ScenarioResource.Type.ToString());
                }

                // <mAvatarAsset />
                if (model.ScenarioResource.AvatarAsset != null)
                {
                    XmlElement avatarAssetElement = document.CreateElement("mAvatarAsset");
                    foreach (Int64 id in model.ScenarioResource.AvatarAsset)
                    {
                        XmlHelper.CreateXmlElement(document, avatarAssetElement, "ID", id.ToString());
                    }
                    element.AppendChild(avatarAssetElement);
                }

                // <acts />
                if (model.ScenarioResource.Acts.Length > 0)
                {
                    XmlElement actsElement = document.CreateElement("acts");

                    // <cScenarioAct />
                    foreach (var act in model.ScenarioResource.Acts)
                    {
                        XmlElement actElement = document.CreateElement("cScenarioAct");

                        // <goals />
                        if (act.Goals.Length > 0)
                        {
                            XmlElement goalsElement = document.CreateElement("goals");

                            // <cScenarioGoal />
                            foreach (var goal in act.Goals)
                            {
                                XmlElement goalElement = document.CreateElement("cScenarioGoal");

                                if (goal.Type != null)
                                {
                                    XmlHelper.CreateXmlElement(document, goalElement, "type", goal.Type.ToString());
                                }

                                XmlElement dialogElement = document.CreateElement("dialog");
                                if (goal.Dialogs.Length > 0)
                                {
                                    foreach (string dialog in goal.Dialogs)
                                    {
                                        XmlElement scenarioDialogElement = document.CreateElement("cScenarioDialog");
                                        XmlHelper.CreateXmlElement(document, scenarioDialogElement, "text", dialog);
                                        dialogElement.AppendChild(scenarioDialogElement);
                                    }
                                }
                                goalElement.AppendChild(dialogElement);
                                goalsElement.AppendChild(goalElement);
                            }

                            actElement.AppendChild(goalsElement);
                        }

                        if (act.TimeLimitSecs != null)
                        {
                            XmlHelper.CreateXmlElement(document, actElement, "timeLimitSecs", act.TimeLimitSecs.ToString());
                        }
                        if (act.Name != null)
                        {
                            XmlHelper.CreateXmlElement(document, actElement, "name", act.Name.ToString());
                        }
                        if (act.Description != null)
                        {
                            XmlHelper.CreateXmlElement(document, actElement, "description", act.Description.ToString());
                        }
                        if (act.TimeVisible != null)
                        {
                            XmlHelper.CreateXmlElement(document, actElement, "bTimeVisible", act.TimeVisible.ToString());
                        }
                        if (act.MusicId != null)
                        {
                            XmlHelper.CreateXmlElement(document, actElement, "actMusicId", act.MusicId.ToString());
                        }

                        actsElement.AppendChild(actElement);
                    }
                    element.AppendChild(actsElement);
                }
               
                // <classes />
                if (model.ScenarioResource.Classes.Length > 0)
                {
                    XmlElement classesElement = document.CreateElement("classes");

                    // <cScenarioClass />
                    foreach (var singleClass in model.ScenarioResource.Classes)
                    {
                        XmlElement classElement = document.CreateElement("cScenarioClass");

                        // <acts />
                        XmlElement actsElement = document.CreateElement("acts");
                        foreach (var act in singleClass.Acts)
                        {
                            // <cScenarioClassAct />
                            XmlElement actElement = document.CreateElement("cScenarioClassAct");

                            XmlElement dialogsChatterElement = document.CreateElement("dialogs_chatter");
                            XmlElement dialogsInspectElement = document.CreateElement("dialogs_inspect");

                            // <dialogs_chatter />
                            foreach (var chatter in act.DialogsChatter)
                            {
                                XmlElement dialogElement = document.CreateElement("cScenarioDialog");
                                XmlHelper.CreateXmlElement(document, dialogElement, "text", chatter);
                                dialogsChatterElement.AppendChild(dialogElement);
                            }

                            // <dialogs_inspect />
                            foreach (var inspect in act.DialogsInspect)
                            {
                                XmlElement dialogElement = document.CreateElement("cScenarioDialog");
                                XmlHelper.CreateXmlElement(document, dialogElement, "text", inspect);
                                dialogsInspectElement.AppendChild(dialogElement);
                            }

                            actElement.AppendChild(dialogsChatterElement);
                            actElement.AppendChild(dialogsInspectElement);

                            actsElement.AppendChild(actElement);
                        }
                        classElement.AppendChild(actsElement);

                        // <castName />
                        if (singleClass.CastName != null)
                        {
                            XmlHelper.CreateXmlElement(document, classElement, "castName", singleClass.CastName);
                        }

                        // <mAsset />
                        XmlElement assetElement = document.CreateElement("mAsset");
                        if (singleClass.Asset != null)
                        {
                            if (singleClass.Asset.Id != null)
                            {
                                XmlHelper.CreateXmlElement(document, assetElement, "ID", singleClass.Asset.Id.ToString());
                            }
                            if (singleClass.Asset.Key != null)
                            {
                                XmlHelper.CreateXmlElement(document, assetElement, "key", String.Join(",", singleClass.Asset.Key.Select(x => $"0x{x:x}")));
                            }
                        }
                        classElement.AppendChild(assetElement);

                        // <gfxOverideType />
                        XmlHelper.CreateXmlElement(document, classElement, "gfxOverideType", singleClass.GfxOverrideType.ToString());

                        // <gfxOverideTypeSecondary />
                        XmlHelper.CreateXmlElement(document, classElement, "gfxOverideTypeSecondary", singleClass.GfxOverideTypeSecondary.ToString());

                        // <gameplayObjectGfxOverrideAsset />
                        if (singleClass.GfxOverrideAsset != null)
                        {
                            XmlElement gfxOverrideAssetElement = document.CreateElement("gameplayObjectGfxOverrideAsset");
                            if (singleClass.GfxOverrideAsset.Id != null)
                            {
                                XmlHelper.CreateXmlElement(document, gfxOverrideAssetElement, "ID", singleClass.GfxOverrideAsset.Id.ToString());
                            }
                            if (singleClass.GfxOverrideAsset.Key != null)
                            {
                                XmlHelper.CreateXmlElement(document, gfxOverrideAssetElement, "key", String.Join(",", singleClass.GfxOverrideAsset.Key.Select(x => $"0x{x:x}")));
                            }
                            classElement.AppendChild(gfxOverrideAssetElement);
                        }

                        classesElement.AppendChild(classElement);
                    }

                    element.AppendChild(classesElement);
                }

                rootElement.AppendChild(element);
            }

            return document.OuterXml;
        }

        public static void Validate(SporeModel model)
        {
            // TODO, add proper validation


            if (model.FormatVersion != null && model.FormatVersion != 18)
            {
                throw new Exception($"Unsupported FormatVersion: {model.FormatVersion}");
            }

            if (model.BlocksCount != null)
            {
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
}

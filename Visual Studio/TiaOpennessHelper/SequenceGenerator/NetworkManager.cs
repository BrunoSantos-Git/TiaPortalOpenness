using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace TiaOpennessHelper.ExcelTree
{
    public class NetworkManager
    {
        public static string SavePath { get; set; }

        /// <summary>
        /// Generates the NetWork Part
        /// </summary>
        public static void GenerateNetWork()
        {
            //ThemePlate Part
            StepHandler.MultilingualCounter = 1;
            GenerateNetWorkThemePlate();

            //Renaming Part
            foreach (List<StepHandler> list in StepHandler.GrafcetList)
            {
                string path;
                string sheetName = StepHandler.GrafcetList[0][0].SheetName;
                string blockName = sheetName.Substring(3, 6) + "_" + sheetName.Substring(sheetName.Length - 3);
                if (sheetName.Length == 12) // If sheet name is like AS_000000V01
                {
                    GenerateXMLWithCorrectNameNet(blockName + "#AST", Path.Combine(SavePath, blockName + "#AST_N.xml"));
                    path = Path.Combine(SavePath, blockName + "#AST_N.xml");
                    TreeViewManager.BlocksCreated.Add(blockName + "#AST_N");
                }
                else if (sheetName.Length == 9) // If sheet name is like AS_000000
                {
                    GenerateXMLWithCorrectNameNet(sheetName.Substring(3) + "#AST", Path.Combine(SavePath, sheetName.Substring(3) + "#AST_N.xml"));
                    path = Path.Combine(SavePath, sheetName.Substring(3) + "#AST_N.xml");
                    TreeViewManager.BlocksCreated.Add(sheetName.Substring(3) + "#AST_N");
                }
                else return;

                foreach (StepHandler step in list)
                {
                    try
                    {
                        Add_VerriegelungHand_VerriegelungAutomatik_Transitionbedingung(step, list, path);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    Add_Transitionsverwaltung(step, list, path);
                }
            }

            File.Delete("C:/Temp/NetWorkThemePlate.xml");
        }

        /// <summary>
        /// Changes the names in the NetWork XML
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        public static void GenerateXMLWithCorrectNameNet(string name, string path)
        {
            XDocument xmlDocNet = XDocument.Load("C:/Temp/NetWorkThemePlate.xml");
            XmlNamespaceManager oManager = new XmlNamespaceManager(new NameTable());
            oManager.AddNamespace("xxx", "http://www.siemens.com/automation/Openness/SW/Interface/v3");
            var elementNet = xmlDocNet.XPathSelectElement("/Document/SW.Blocks.FC[@ID='0']/AttributeList/Name", oManager);
            var elementBlockNetTitle = xmlDocNet.XPathSelectElement("/Document/SW.Blocks.FC[@ID='0']/ObjectList/MultilingualText[@ID='14']/ObjectList/MultilingualTextItem[@ID='15']/AttributeList/Text", oManager);
            elementNet.Value = name;
            elementBlockNetTitle.Value = ("AST_" + name).ToString();

            xmlDocNet.Save(path);
        }

        /// <summary>
        /// Generates the base NetWork
        /// </summary>
        public static void GenerateNetWorkThemePlate()
        {
            XNamespace xn = "http://www.siemens.com/automation/Openness/SW/Interface/v3";
            new XDocument(
                new XElement("Document"
                        , new XElement("Engineering", new XAttribute("version", "V15"))
                    , new XElement("DocumentInfo"
                        , new XElement("Created")
                        , new XElement("ExportSettings", "WithDefaults, WithReadOnly")
                        , new XElement("InstalledProdutcs"
                            , new XElement("Product"
                                , new XElement("DisplayName", "Totally Integrated Automation Portal")
                                , new XElement("DisplayVersion", "V15"))
                            , new XElement("OptionPackage"
                                , new XElement("DisplayName", "TIA Portal Openness")
                                , new XElement("DisplayVersion", "V15"))
                            , new XElement("Product"
                                , new XElement("DisplayName", "STEP 7 Professional")
                                , new XElement("DisplayVersion", "V15 Update 2"))
                            , new XElement("OptionPackage"
                                , new XElement("DisplayName", "STEP 7 Safety")
                                , new XElement("DisplayVersion", "V15"))
                            , new XElement("Product"
                                , new XElement("DisplayName", "WinCC Professional")
                                , new XElement("DisplayVersion", "V15"))
                            , new XElement("OptionPackage"
                                , new XElement("DisplayName", "SIMATIC Visualization Architect")
                                , new XElement("DisplayVersion", "V15"))))
                    , new XElement("SW.Blocks.FC", new XAttribute("ID", "0")
                        , new XElement("AttributeList"
                              , new XElement("AutoNumber", "true")
                              , new XElement("CodeModifiedDate", new XAttribute("ReadOnly", "true"))
                              , new XElement("CompileDate", new XAttribute("ReadOnly", "true"))
                              , new XElement("CreationDate", new XAttribute("ReadOnly", "true"))
                              , new XElement("HandleErrorsWithinBlock", new XAttribute("ReadOnly", "true"), "false")
                              , new XElement("HeaderAuthor")
                              , new XElement("HeaderFamily")
                              , new XElement("HeaderName")
                              , new XElement("HeaderVersion", "1.1")
                              , new XElement("Interface"
                                    , new XElement(xn + "Sections", new XAttribute("xmlns", "http://www.siemens.com/automation/Openness/SW/Interface/v3")
                                        , new XElement("Section", new XAttribute("Name", "Input"))
                                        , new XElement("Section", new XAttribute("Name", "Output"))
                                        , new XElement("Section", new XAttribute("Name", "InOut"))
                                        , new XElement("Section", new XAttribute("Name", "Temp")
                                            , new XElement("Member", new XAttribute("Name", "Temp"), new XAttribute("Datatype", "Struct")
                                                , new XElement("Member", new XAttribute("Name", "_Bool"), new XAttribute("Datatype", "Bool"))
                                                , new XElement("Member", new XAttribute("Name", "_Byte"), new XAttribute("Datatype", "Byte"))
                                                , new XElement("Member", new XAttribute("Name", "_Word"), new XAttribute("Datatype", "Word"))
                                                , new XElement("Member", new XAttribute("Name", "_DWord"), new XAttribute("Datatype", "DWord"))
                                                , new XElement("Member", new XAttribute("Name", "_Int"), new XAttribute("Datatype", "Int"))
                                                , new XElement("Member", new XAttribute("Name", "_DInt"), new XAttribute("Datatype", "DInt"))
                                                , new XElement("Member", new XAttribute("Name", "Real"), new XAttribute("Datatype", "Real"))
                                                , new XElement("Member", new XAttribute("Name", "_S5Time"), new XAttribute("Datatype", "S5Time"))
                                                , new XElement("Member", new XAttribute("Name", "_Time"), new XAttribute("Datatype", "Time"))
                                                           )
                                                       )
                                        , new XElement("Section", new XAttribute("Name", "Constant"))
                                        , new XElement("Section", new XAttribute("Name", "Return")
                                            , new XElement("Member", new XAttribute("Name", "Ret_Val"), new XAttribute("Datatype", "Void"), new XAttribute("Accessibility", "Public"))
                                                       )
                                                   )
                                             )
                              , new XElement("InterfaceModifiedDate", new XAttribute("ReadOnly", "true"))
                              , new XElement("IsConsistent", new XAttribute("ReadOnly", "true"), "true")
                              , new XElement("IsIECCheckEnabled", "false")
                              , new XElement("IsKnowHowProtected", new XAttribute("ReadOnly", "true"), "false")
                              , new XElement("IsWriteProtected", new XAttribute("ReadOnly", "true"), "false")
                              , new XElement("LibraryConformanceStatus", new XAttribute("ReadOnly", "true"), "The object is library-conformant.")
                              , new XElement("MemoryLayout", "Optimized")
                              , new XElement("ModifiedDate", new XAttribute("ReadOnly", "true"))
                              , new XElement("Name", "NetWorkThemePlate")
                              , new XElement("Number", "777")
                              , new XElement("ParameterModified", new XAttribute("ReadOnly", "true"))
                              , new XElement("PLCSimAdvancedSupport", new XAttribute("ReadOnly", "true"), "true")
                              , new XElement("ProgrammingLanguage", "LAD")
                              , new XElement("SetENOAutomatically", "False")
                              , new XElement("StructureModified", new XAttribute("ReadOnly", "true"))
                              , new XElement("UDABlockProperties")
                              , new XElement("UDAEnableTagReadback", "false")
                                   )
                        , new XElement("ObjectList"
                            , new XElement("MultilingualText", new XAttribute("ID", StepHandler.MultilingualCounter), new XAttribute("CompositionName", "Comment")
                                  , new XElement("ObjectList"
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 1), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "de-DE")
                                              , new XElement("Text", "BlockTitle Comment")))
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 2), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "es-ES")
                                              , new XElement("Text", "BlockTitle Comment")))
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 3), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "en-US")
                                              , new XElement("Text", "BlockTitle Comment")))
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 4), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "fr-FR")
                                              , new XElement("Text", "BlockTitle Comment")))
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 5), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "zh-CN")
                                              , new XElement("Text", "BlockTitle Comment")))

                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 7), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "pl-PL")
                                              , new XElement("Text", "BlockTitle Comment")))
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 8), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "pt-BR")
                                              , new XElement("Text", "BlockTitle Comment")))
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 9), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "ru-RU")
                                              , new XElement("Text", "BlockTitle Comment")))
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 10), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "sk-SK")
                                              , new XElement("Text", "BlockTitle Comment")))
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 11), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "nl-BE")
                                              , new XElement("Text", "BlockTitle Comment")))
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 12), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "hu-HU")
                                              , new XElement("Text", "BlockTitle Comment")))
                                              )
                                            )
                            , new XElement("MultilingualText", new XAttribute("ID", StepHandler.MultilingualCounter + 13), new XAttribute("CompositionName", "Title")
                                                  , new XElement("ObjectList"
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 14), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "de-DE")
                                                              , new XElement("Text", "Station Name")))
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 15), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "es-ES")
                                                              , new XElement("Text", "")))
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 16), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "en-US")
                                                              , new XElement("Text", "")))
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 17), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "fr-FR")
                                                              , new XElement("Text", "")))
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 18), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "zh-CN")
                                                              , new XElement("Text", "")))

                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 20), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "pl-PL")
                                                              , new XElement("Text", "")))
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 21), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "pt-BR")
                                                              , new XElement("Text", "")))
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 22), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "ru-RU")
                                                              , new XElement("Text", "")))
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 23), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "sk-SK")
                                                              , new XElement("Text", "")))
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 24), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "nl-BE")
                                                              , new XElement("Text", "")))
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 25), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "hu-HU")
                                                              , new XElement("Text", "")))
                                                              )
                                                             )
                                       )
                                )
                             )
                    ).Save("C:/Temp/NetWorkThemePlate.xml");
            StepHandler.MultilingualCounter += 26;
        }

        /// <summary>
        /// Adds the Verriegelung Hand,Verriegelung Automatik and Transitionbedingung to the FC file
        /// </summary>
        /// <param name="step"></param>
        /// <param name="list"></param>
        /// <param name="path"></param>
        public static void Add_VerriegelungHand_VerriegelungAutomatik_Transitionbedingung(StepHandler step, List<StepHandler> list, string path)
        {
            foreach (string nextStep in step.NextSteps)
            {
                for (int z = 0; z < 4; z++)
                {
                    if (list.ElementAtOrDefault(Int32.Parse(nextStep) - 1) != null)
                    {
                        if (list[Int32.Parse(nextStep) - 1].StepName.Contains("MM") && z == 2)
                        {
                            foreach (string s in list[Int32.Parse(nextStep) - 1].StepActions)
                            {
                                if (s.Contains("V1"))
                                {
                                    Default_NetWork(step, 4, s.Substring(0, 4), path);
                                }
                            }
                        }

                        Default_NetWork(step, z, "AS_" + list[Int32.Parse(nextStep) - 1].StepName, path);
                    } else
                        throw new Exception("Invalid Graph");
                }
            }
        }

        /// <summary>
        /// Adds the Transitionsverwaltungc to the FC file
        /// </summary>
        /// <param name="step"></param>
        /// <param name="list"></param>
        /// <param name="path"></param>
        public static void Add_Transitionsverwaltung(StepHandler step, List<StepHandler> list, string path)
        {
            string fcName = "";
            List<string> NetList = new List<string>
            {
                "VerHand",
                "VerAuto",
                "TransBed"
            };

            if (Int32.Parse(step.StepNumber) == 1)
            {
                fcName = "_Init";
            }

            StepHandler.CounterUId = 21;
            XDocument xmlDoc = XDocument.Load(path);
            XmlNamespaceManager oManager = new XmlNamespaceManager(new NameTable());
            XNamespace n1 = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v2";
            oManager.AddNamespace("xxx", "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v2");
            var element = xmlDoc.XPathSelectElement("/Document/SW.Blocks.FC[@ID='0']/ObjectList");

            XElement VerriegelungHand = new XElement("SW.Blocks.CompileUnit", new XAttribute("ID", StepHandler.MultilingualCounter), new XAttribute("CompositionName", "CompileUnits")
                                , new XElement("AttributeList"
                                    , new XElement("NetworkSource"
                                        , new XElement(n1 + "FlgNet", new XAttribute("xmlns", "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v2")
                                            , new XElement("Parts", "")
                                            , new XElement("Wires")))
                                    , new XElement("ProgrammingLanguage", "LAD"))
                                , new XElement("ObjectList"
                                    , new XElement("MultilingualText", new XAttribute("ID", StepHandler.MultilingualCounter + 1), new XAttribute("CompositionName", "Comment")
                                        , new XElement("ObjectList"
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 2), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "de-DE")
                                                    , new XElement("Text", "Comment")))
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 3), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "es-ES")
                                                    , new XElement("Text", "Comment")))
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 4), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "en-US")
                                                    , new XElement("Text", "Comment")
                                                               )
                                                           )
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 5), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "fr-FR")
                                                    , new XElement("Text", "Comment")
                                                               )
                                                           )
                                             , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 6), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "zh-CN")
                                                    , new XElement("Text", "Comment")
                                                               )
                                                           )
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 7), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "pl-PL")
                                                    , new XElement("Text", "Comment")
                                                               )
                                                           )
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 8), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "pt-BR")
                                                    , new XElement("Text", "Comment")
                                                               )
                                                           )
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 9), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "ru-RU")
                                                    , new XElement("Text", "Comment")
                                                               )
                                                           )
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 10), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "sk-SK")
                                                    , new XElement("Text", "Comment")
                                                               )
                                                           )
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 11), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "nl-BE")
                                                    , new XElement("Text", "Comment")
                                                               )
                                                           )
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 12), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "hu-HU")
                                                    , new XElement("Text", "Comment")
                                                               )
                                                           )
                                                       )
                                                   )
                                    , new XElement("MultilingualText", new XAttribute("ID", StepHandler.MultilingualCounter + 13), new XAttribute("CompositionName", "Title")
                                        , new XElement("ObjectList"
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 14), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "de-DE")
                                                    , new XElement("Text", "Transitionsverwaltung: (" + step.StepName + " : " + step.StepDescription + ")")
                                                               )
                                                           )
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 15), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "es-ES")
                                                    , new XElement("Text", "")
                                                               )
                                                           )
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 16), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "en-US")
                                                    , new XElement("Text", "")
                                                               )
                                                           )
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 17), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "fr-FR")
                                                    , new XElement("Text", "")
                                                               )
                                                           )
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 18), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "zh-CN")
                                                    , new XElement("Text", "")
                                                               )
                                                           )
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 19), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "pl-PL")
                                                    , new XElement("Text", "")
                                                               )
                                                           )
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 20), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "pt-BR")
                                                    , new XElement("Text", "")
                                                               )
                                                           )
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 21), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "ru-RU")
                                                    , new XElement("Text", "")
                                                               )
                                                           )
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 22), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "sk-SK")
                                                    , new XElement("Text", "")
                                                               )
                                                           )
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 23), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "nl-BE")
                                                    , new XElement("Text", "")
                                                               )
                                                           )
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 24), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "hu-HU")
                                                    , new XElement("Text", "")
                                                               )
                                                           )
                                                       )
                                                   )
                                               )
                                           );

            element.Add(VerriegelungHand);
            StepHandler.MultilingualCounter += 25;
            var elementParts = xmlDoc.XPathSelectElement("/Document/SW.Blocks.FC[@ID='0']/ObjectList/SW.Blocks.CompileUnit[@ID='" + (StepHandler.MultilingualCounter - 25) + "']/AttributeList/NetworkSource/xxx:FlgNet/Parts", oManager);
            var elementWires = xmlDoc.XPathSelectElement("/Document/SW.Blocks.FC[@ID='0']/ObjectList/SW.Blocks.CompileUnit[@ID='" + (StepHandler.MultilingualCounter - 25) + "']/AttributeList/NetworkSource/xxx:FlgNet/Wires", oManager);

            List<string> scopes = new List<string>
            {
                "LiteralConstant",
                "GlobalVariable",
                "TypedConstant",
                "LocalVariable"
            };

            for (int z = 0; z < 3; z++)
            {
                XElement Access = new XElement("Access", new XAttribute("Scope", scopes[1]), new XAttribute("UId", StepHandler.CounterUId.ToString())
                                , new XElement("Symbol"
                                    , new XElement("Component", new XAttribute("Name", step.SheetName.Substring(3, 6) + step.SheetName.Substring(step.SheetName.Length - 3) + "#AS_DB"))
                                    , new XElement("Component", new XAttribute("Name", "AS_" + list[Int32.Parse(step.NextSteps[0]) - 1].StepName))
                                    , new XElement("Component", new XAttribute("Name", NetList[z]))
                                    , new XElement("Address", new XAttribute("Area", "None"), new XAttribute("Type", "Bool"), new XAttribute("BlockNumber", "111"), new XAttribute("BitOffset", "17000"), new XAttribute("Informative", "true"))
                                               )
                                          );
                elementParts.Add(Access);
                StepHandler.CounterUId += 1;
            }

            XElement Access2 = new XElement("Access", new XAttribute("Scope", scopes[0]), new XAttribute("UId", StepHandler.CounterUId.ToString())
                                , new XElement("Constant"
                                    , new XElement("ConstantType", "Word")
                                    , new XElement("ConstantValue", "2#0000_0000_0000_0000")
                                    , new XElement("StringAttribute", new XAttribute("Name", "Format"), new XAttribute("Informative", "true"), "Bin")
                                               )
                                          );
            elementParts.Add(Access2);

            StepHandler.CounterUId += 1;

            XElement Access3 = new XElement("Access", new XAttribute("Scope", scopes[1]), new XAttribute("UId", StepHandler.CounterUId.ToString())
                                , new XElement("Symbol"
                                    , new XElement("Component", new XAttribute("Name", step.SheetName.Substring(3, 6) + step.SheetName.Substring(step.SheetName.Length - 3) + "#AS_DB"))
                                    , new XElement("Component", new XAttribute("Name", "S_" + step.StepName))
                                    , new XElement("Address", new XAttribute("Area", "None"), new XAttribute("Type", "Bool"), new XAttribute("BlockNumber", "111"), new XAttribute("BitOffset", "17000"), new XAttribute("Informative", "true"))
                                               )
                                          );
            elementParts.Add(Access3);

            StepHandler.CounterUId += 1;

            XElement Access4 = new XElement("Access", new XAttribute("Scope", scopes[2]), new XAttribute("UId", StepHandler.CounterUId.ToString())
                                , new XElement("Constant"
                                    , new XElement("ConstantValue", "T#" + step.StepTime + "MS")
                                    , new XElement("StringAttribute", new XAttribute("Name", "Format"), new XAttribute("Informative", "true"), "Time")
                                    , new XElement("StringAttribute", new XAttribute("Name", "FormatFlags"), new XAttribute("Informative", "true"), "TypeQualifier")
                                               )
                                          );
            elementParts.Add(Access4);

            StepHandler.CounterUId += 1;

            XElement Access5 = new XElement("Access", new XAttribute("Scope", scopes[1]), new XAttribute("UId", StepHandler.CounterUId.ToString())
                                , new XElement("Symbol"
                                    , new XElement("Component", new XAttribute("Name", step.SheetName.Substring(3, 6) + step.SheetName.Substring(step.SheetName.Length - 3) + "#AS_DB"))
                                    , new XElement("Component", new XAttribute("Name", "RT_DATA"))
                                    , new XElement("Component", new XAttribute("Name", "MOP"))
                                    , new XElement("Component", new XAttribute("Name", "HALT"))
                                    , new XElement("Address", new XAttribute("Area", "None"), new XAttribute("Type", "Bool"), new XAttribute("BlockNumber", "111"), new XAttribute("BitOffset", "17000"), new XAttribute("Informative", "true"))
                                               )
                                          );
            elementParts.Add(Access5);

            StepHandler.CounterUId += 1;

            XElement Access6 = new XElement("Access", new XAttribute("Scope", scopes[1]), new XAttribute("UId", StepHandler.CounterUId.ToString())
                                , new XElement("Symbol"
                                    , new XElement("Component", new XAttribute("Name", step.SheetName.Substring(3, 6) + step.SheetName.Substring(step.SheetName.Length - 3) + "#AS_DB"))
                                    , new XElement("Component", new XAttribute("Name", "AS_" + list[Int32.Parse(step.NextSteps[0]) - 1].StepName))
                                    , new XElement("Component", new XAttribute("Name", "SNO_Time"))
                                    , new XElement("Address", new XAttribute("Area", "None"), new XAttribute("Type", "Time"), new XAttribute("BlockNumber", "111"), new XAttribute("BitOffset", "17000"), new XAttribute("Informative", "true"))
                                               )
                                          );
            elementParts.Add(Access6);

            StepHandler.CounterUId += 1;

            XElement Access7 = new XElement("Access", new XAttribute("Scope", scopes[1]), new XAttribute("UId", StepHandler.CounterUId.ToString())
                                , new XElement("Symbol"
                                    , new XElement("Component", new XAttribute("Name", step.SheetName.Substring(3, 2)))
                                    , new XElement("Component", new XAttribute("Name", "BA"))
                                    , new XElement("Address", new XAttribute("Area", "None"), new XAttribute("Type", "ST_Betriebsarten"), new XAttribute("BlockNumber", "111"), new XAttribute("BitOffset", "17000"), new XAttribute("Informative", "true"))
                                               )
                                          );
            elementParts.Add(Access7);

            StepHandler.CounterUId += 1;

            XElement Access8 = new XElement("Access", new XAttribute("Scope", scopes[3]), new XAttribute("UId", StepHandler.CounterUId.ToString())
                                , new XElement("Symbol"
                                    , new XElement("Component", new XAttribute("Name", "Temp"))
                                    , new XElement("Component", new XAttribute("Name", "_Bool"))
                                               )
                                          );
            elementParts.Add(Access8);

            StepHandler.CounterUId += 1;

            XElement Access9 = new XElement("Access", new XAttribute("Scope", scopes[1]), new XAttribute("UId", StepHandler.CounterUId.ToString())
                                , new XElement("Symbol"
                                    , new XElement("Component", new XAttribute("Name", step.SheetName.Substring(3, 6) + step.SheetName.Substring(step.SheetName.Length - 3)))
                                    , new XElement("Component", new XAttribute("Name", "AS_" + list[Int32.Parse(step.NextSteps[0]) - 1].StepName))
                                    , new XElement("Address", new XAttribute("Area", "None"), new XAttribute("Type", "Bool"), new XAttribute("BlockNumber", "111"), new XAttribute("BitOffset", "17000"), new XAttribute("Informative", "true"))
                                               )
                                          );
            elementParts.Add(Access9);

            StepHandler.CounterUId += 1;

            XElement Access10 = new XElement("Access", new XAttribute("Scope", scopes[3]), new XAttribute("UId", StepHandler.CounterUId.ToString())
                                , new XElement("Symbol"
                                    , new XElement("Component", new XAttribute("Name", "Temp"))
                                    , new XElement("Component", new XAttribute("Name", "_Time"))
                                               )
                                          );
            elementParts.Add(Access10);

            StepHandler.CounterUId += 1;

            XElement Access11 = new XElement("Access", new XAttribute("Scope", scopes[3]), new XAttribute("UId", StepHandler.CounterUId.ToString())
                                , new XElement("Symbol"
                                    , new XElement("Component", new XAttribute("Name", "Temp"))
                                    , new XElement("Component", new XAttribute("Name", "_Time"))
                                               )
                                          );
            elementParts.Add(Access11);


            XElement Call = new XElement("Call", new XAttribute("UId", "34")
                                , new XElement("CallInfo", new XAttribute("Name", "FC_Trans" + fcName), new XAttribute("BlockType", "FC")
                                     , new XElement("IntegerAttribute", new XAttribute("Name", "BlockNumber"), new XAttribute("Informative", "true"), "640")
                                     , new XElement("DateAttribute", new XAttribute("Name", "ParameterModifiedTS"), new XAttribute("Informative", "true"), "2016-10-26T11:24:35")
                                               )
                                         );
            elementParts.Add(Call);

            var elementCall = xmlDoc.XPathSelectElement("/Document/SW.Blocks.FC[@ID='0']/ObjectList/SW.Blocks.CompileUnit[@ID='" + (StepHandler.MultilingualCounter - 25) + "']/AttributeList/NetworkSource/xxx:FlgNet/Parts/Call/CallInfo", oManager);

            XElement Parameter = new XElement("Parameter", new XAttribute("Name", "VerHand"), new XAttribute("Section", "Input"), new XAttribute("Type", "Bool")
                                        , new XElement("StringAttribute", new XAttribute("Name", "InterfaceFlags"), new XAttribute("Informative", "true"), "S7_Visible")
                                                  );
            elementCall.Add(Parameter);

            XElement Parameter2 = new XElement("Parameter", new XAttribute("Name", "VerAuto"), new XAttribute("Section", "Input"), new XAttribute("Type", "Bool")
                                        , new XElement("StringAttribute", new XAttribute("Name", "InterfaceFlags"), new XAttribute("Informative", "true"), "S7_Visible")
                                                  );
            elementCall.Add(Parameter2);

            XElement Parameter3 = new XElement("Parameter", new XAttribute("Name", "TransBed"), new XAttribute("Section", "Input"), new XAttribute("Type", "Bool")
                                        , new XElement("StringAttribute", new XAttribute("Name", "InterfaceFlags"), new XAttribute("Informative", "true"), "S7_Visible")
                                                  );
            elementCall.Add(Parameter3);

            XElement Parameter4 = new XElement("Parameter", new XAttribute("Name", "Cfg"), new XAttribute("Section", "Input"), new XAttribute("Type", "Word")
                                        , new XElement("StringAttribute", new XAttribute("Name", "InterfaceFlags"), new XAttribute("Informative", "true"), "S7_Visible")
                                                  );
            elementCall.Add(Parameter4);

            XElement Parameter5 = new XElement("Parameter", new XAttribute("Name", "Schritt"), new XAttribute("Section", "Input"), new XAttribute("Type", "G7_StepPlus_V4")
                                        , new XElement("StringAttribute", new XAttribute("Name", "InterfaceFlags"), new XAttribute("Informative", "true"), "S7_Visible")
                                                  );
            elementCall.Add(Parameter5);

            XElement Parameter6 = new XElement("Parameter", new XAttribute("Name", "TUe_S"), new XAttribute("Section", "Input"), new XAttribute("Type", "Time")
                                        , new XElement("StringAttribute", new XAttribute("Name", "InterfaceFlags"), new XAttribute("Informative", "true"), "S7_Visible")
                                                  );
            elementCall.Add(Parameter6);

            XElement Parameter7 = new XElement("Parameter", new XAttribute("Name", "VerGes"), new XAttribute("Section", "Output"), new XAttribute("Type", "Bool")
                                        , new XElement("StringAttribute", new XAttribute("Name", "InterfaceFlags"), new XAttribute("Informative", "true"), "S7_Visible")
                                                  );
            elementCall.Add(Parameter7);

            XElement Parameter8 = new XElement("Parameter", new XAttribute("Name", "FrgTrans"), new XAttribute("Section", "Output"), new XAttribute("Type", "Bool")
                                        , new XElement("StringAttribute", new XAttribute("Name", "InterfaceFlags"), new XAttribute("Informative", "true"), "S7_Visible")
                                                  );
            elementCall.Add(Parameter8);

            XElement Parameter9 = new XElement("Parameter", new XAttribute("Name", "SNO_T_Aktuell"), new XAttribute("Section", "Output"), new XAttribute("Type", "Time")
                                        , new XElement("StringAttribute", new XAttribute("Name", "InterfaceFlags"), new XAttribute("Informative", "true"), "S7_Visible")
                                                  );
            elementCall.Add(Parameter9);

            XElement Parameter10 = new XElement("Parameter", new XAttribute("Name", "SNO_T_Gespeichert"), new XAttribute("Section", "Output"), new XAttribute("Type", "Time")
                                        , new XElement("StringAttribute", new XAttribute("Name", "InterfaceFlags"), new XAttribute("Informative", "true"), "S7_Visible")
                                                  );
            elementCall.Add(Parameter10);

            XElement Parameter11 = new XElement("Parameter", new XAttribute("Name", "MOP_HALT"), new XAttribute("Section", "InOut"), new XAttribute("Type", "Bool")
                                        , new XElement("StringAttribute", new XAttribute("Name", "InterfaceFlags"), new XAttribute("Informative", "true"), "S7_Visible")
                                                  );
            elementCall.Add(Parameter11);

            XElement Parameter12 = new XElement("Parameter", new XAttribute("Name", "SNO_Time"), new XAttribute("Section", "InOut"), new XAttribute("Type", "Time")
                                        , new XElement("StringAttribute", new XAttribute("Name", "InterfaceFlags"), new XAttribute("Informative", "true"), "S7_Visible")
                                                  );
            elementCall.Add(Parameter12);

            XElement Parameter13 = new XElement("Parameter", new XAttribute("Name", "ST_BA"), new XAttribute("Section", "InOut"), new XAttribute("Type", "ST_Betriebsarten")
                                        , new XElement("StringAttribute", new XAttribute("Name", "InterfaceFlags"), new XAttribute("Informative", "true"), "S7_Visible")
                                                  );
            elementCall.Add(Parameter13);

            XElement ParameterWire35 = new XElement("Wire", new XAttribute("UId", "35")
                                       , new XElement("Powerrail")
                                       , new XElement("NameCon", new XAttribute("UId", "34"), new XAttribute("Name", "en"))
                                                 );
            elementWires.Add(ParameterWire35);

            XElement ParameterWire36 = new XElement("Wire", new XAttribute("UId", "36")
                                       , new XElement("IdentCon", new XAttribute("UId", "21"))
                                       , new XElement("NameCon", new XAttribute("UId", "34"), new XAttribute("Name", "VerHand"))
                                                 );
            elementWires.Add(ParameterWire36);

            XElement ParameterWire37 = new XElement("Wire", new XAttribute("UId", "37")
                                       , new XElement("IdentCon", new XAttribute("UId", "22"))
                                       , new XElement("NameCon", new XAttribute("UId", "34"), new XAttribute("Name", "VerAuto"))
                                                 );
            elementWires.Add(ParameterWire37);

            XElement ParameterWire38 = new XElement("Wire", new XAttribute("UId", "38")
                                       , new XElement("IdentCon", new XAttribute("UId", "23"))
                                       , new XElement("NameCon", new XAttribute("UId", "34"), new XAttribute("Name", "TransBed"))
                                                 );
            elementWires.Add(ParameterWire38);

            XElement ParameterWire39 = new XElement("Wire", new XAttribute("UId", "39")
                                       , new XElement("IdentCon", new XAttribute("UId", "24"))
                                       , new XElement("NameCon", new XAttribute("UId", "34"), new XAttribute("Name", "Cfg"))
                                                 );
            elementWires.Add(ParameterWire39);

            XElement ParameterWire40 = new XElement("Wire", new XAttribute("UId", "40")
                                       , new XElement("IdentCon", new XAttribute("UId", "25"))
                                       , new XElement("NameCon", new XAttribute("UId", "34"), new XAttribute("Name", "Schritt"))
                                                 );
            elementWires.Add(ParameterWire40);

            XElement ParameterWire41 = new XElement("Wire", new XAttribute("UId", "41")
                                       , new XElement("IdentCon", new XAttribute("UId", "26"))
                                       , new XElement("NameCon", new XAttribute("UId", "34"), new XAttribute("Name", "TUe_S"))
                                                 );
            elementWires.Add(ParameterWire41);

            XElement ParameterWire42 = new XElement("Wire", new XAttribute("UId", "42")
                                       , new XElement("IdentCon", new XAttribute("UId", "27"))
                                       , new XElement("NameCon", new XAttribute("UId", "34"), new XAttribute("Name", "MOP_HALT"))
                                                 );
            elementWires.Add(ParameterWire42);

            XElement ParameterWire43 = new XElement("Wire", new XAttribute("UId", "43")
                                       , new XElement("IdentCon", new XAttribute("UId", "28"))
                                       , new XElement("NameCon", new XAttribute("UId", "34"), new XAttribute("Name", "SNO_Time"))
                                                 );
            elementWires.Add(ParameterWire43);

            XElement ParameterWire44 = new XElement("Wire", new XAttribute("UId", "44")
                                       , new XElement("IdentCon", new XAttribute("UId", "29"))
                                       , new XElement("NameCon", new XAttribute("UId", "34"), new XAttribute("Name", "ST_BA"))
                                                 );
            elementWires.Add(ParameterWire44);

            XElement ParameterWire45 = new XElement("Wire", new XAttribute("UId", "45")
                                       , new XElement("NameCon", new XAttribute("UId", "34"), new XAttribute("Name", "VerGes"))
                                       , new XElement("IdentCon", new XAttribute("UId", "30"))
                                                 );
            elementWires.Add(ParameterWire45);

            XElement ParameterWire46 = new XElement("Wire", new XAttribute("UId", "46")
                                       , new XElement("NameCon", new XAttribute("UId", "34"), new XAttribute("Name", "FrgTrans"))
                                       , new XElement("IdentCon", new XAttribute("UId", "31"))
                                                 );
            elementWires.Add(ParameterWire46);

            XElement ParameterWire47 = new XElement("Wire", new XAttribute("UId", "47")
                                       , new XElement("NameCon", new XAttribute("UId", "34"), new XAttribute("Name", "SNO_T_Aktuell"))
                                       , new XElement("IdentCon", new XAttribute("UId", "32"))
                                                 );
            elementWires.Add(ParameterWire47);

            XElement ParameterWire48 = new XElement("Wire", new XAttribute("UId", "48")
                                       , new XElement("NameCon", new XAttribute("UId", "34"), new XAttribute("Name", "SNO_T_Gespeichert"))
                                       , new XElement("IdentCon", new XAttribute("UId", "33"))
                                                 );
            elementWires.Add(ParameterWire48);

            xmlDoc.Save(path);
        }

        /// <summary>
        /// Uses a default theme plate to create the networks
        /// </summary>
        /// <param name="step"></param>
        /// <param name="index"></param>
        /// <param name="name"></param>
        /// <param name="path"></param>
        public static void Default_NetWork(StepHandler step, int index, string name, string path)
        {
            List<string> NetList = new List<string>
            {
                "VerHand",
                "VerAuto",
                "TransBed1",
                "TransBed",
                "AutoVR"
            };

            List<string> NetListTitle = new List<string>
            {
                "Verriegelung Hand",
                "Verriegelung Automatik",
                "Transitionbedingung Hifu",
                "Transitionbedingung",
                "Automatik Vor Rück"
            };

            string stationName = step.SheetName.Substring(3, 6) + step.SheetName.Substring(step.SheetName.Length - 3) + "#AS_DB";

            if (NetList[index] == "AutoVR")
                stationName = step.SheetName.Substring(3, 6) + step.SheetName.Substring(step.SheetName.Length - 3);

            StepHandler.CounterUId = 21;
            XDocument xmlDoc = XDocument.Load(path);
            XmlNamespaceManager oManager = new XmlNamespaceManager(new NameTable());
            XNamespace n1 = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v2";
            oManager.AddNamespace("xxx", "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v2");
            var element = xmlDoc.XPathSelectElement("/Document/SW.Blocks.FC[@ID='0']/ObjectList");

            XElement VerriegelungHand = new XElement("SW.Blocks.CompileUnit", new XAttribute("ID", StepHandler.MultilingualCounter), new XAttribute("CompositionName", "CompileUnits")
                                , new XElement("AttributeList"
                                    , new XElement("NetworkSource"
                                        , new XElement(n1 + "FlgNet", new XAttribute("xmlns", "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v2")
                                            , new XElement("Parts", "")
                                            , new XElement("Wires")))
                                    , new XElement("ProgrammingLanguage", "LAD"))
                                , new XElement("ObjectList"
                                    , new XElement("MultilingualText", new XAttribute("ID", StepHandler.MultilingualCounter + 1), new XAttribute("CompositionName", "Comment")
                                        , new XElement("ObjectList"
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 2), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "de-DE")
                                                    , new XElement("Text", "")))
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 3), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "es-ES")
                                                    , new XElement("Text", "")))
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 4), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "en-US")
                                                    , new XElement("Text", "")))
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 5), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "fr-FR")
                                                    , new XElement("Text", "")))
                                             , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 6), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "zh-CN")
                                                    , new XElement("Text", "")))
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 7), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "pl-PL")
                                                    , new XElement("Text", "")))
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 8), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "pt-BR")
                                                    , new XElement("Text", "")))
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 9), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "ru-RU")
                                                    , new XElement("Text", "")))
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 10), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "sk-SK")
                                                    , new XElement("Text", "")))
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 11), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "nl-BE")
                                                    , new XElement("Text", "")))
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 12), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "hu-HU")
                                                    , new XElement("Text", "")))))
                                    , new XElement("MultilingualText", new XAttribute("ID", StepHandler.MultilingualCounter + 13), new XAttribute("CompositionName", "Title")
                                        , new XElement("ObjectList"
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 14), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "de-DE")
                                                    , new XElement("Text", NetListTitle[index])))
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 15), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "es-ES")
                                                    , new XElement("Text", "")))
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 16), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "en-US")
                                                    , new XElement("Text", "")))
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 17), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "fr-FR")
                                                    , new XElement("Text", "")))
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 18), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "zh-CN")
                                                    , new XElement("Text", "")))
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 19), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "pl-PL")
                                                    , new XElement("Text", "")))
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 20), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "pt-BR")
                                                    , new XElement("Text", "")))
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 21), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "ru-RU")
                                                    , new XElement("Text", "")))
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 22), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "sk-SK")
                                                    , new XElement("Text", "")))
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 23), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "nl-BE")
                                                    , new XElement("Text", "")))
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 24), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "hu-HU")
                                                    , new XElement("Text", "")))))));

            element.Add(VerriegelungHand);
            StepHandler.MultilingualCounter += 25;
            var elementParts = xmlDoc.XPathSelectElement("/Document/SW.Blocks.FC[@ID='0']/ObjectList/SW.Blocks.CompileUnit[@ID='" + (StepHandler.MultilingualCounter - 25) + "']/AttributeList/NetworkSource/xxx:FlgNet/Parts", oManager);
            var elementWires = xmlDoc.XPathSelectElement("/Document/SW.Blocks.FC[@ID='0']/ObjectList/SW.Blocks.CompileUnit[@ID='" + (StepHandler.MultilingualCounter - 25) + "']/AttributeList/NetworkSource/xxx:FlgNet/Wires", oManager);

            if (NetListTitle[index] == "Transitionbedingung")
            {
                XElement Access = new XElement("Access", new XAttribute("Scope", "GlobalVariable"), new XAttribute("UId", StepHandler.CounterUId.ToString())
                                     , new XElement("Symbol"
                                         , new XElement("Component", new XAttribute("Name", stationName)) 
                                         , new XElement("Component", new XAttribute("Name", name))
                                         , new XElement("Component", new XAttribute("Name", NetList[index] + "1")) // + "1" because hifu has same name and the number of hifu in front. In this case, "1"
                                         , new XElement("Address", new XAttribute("Area", "None"), new XAttribute("Type", "Bool"), new XAttribute("BlockNumber", "111"), new XAttribute("BitOffset", "17000"), new XAttribute("Informative", "true"))));
                elementParts.Add(Access);
            } 
            else
            {
                XElement Access = new XElement("Access", new XAttribute("Scope", "GlobalVariable"), new XAttribute("UId", StepHandler.CounterUId.ToString())
                                     , new XElement("Symbol"
                                         , new XElement("Component", new XAttribute("Name", "DB_ARG"))
                                         , new XElement("Component", new XAttribute("Name", "IBN=0"))
                                         , new XElement("Address", new XAttribute("Area", "None"), new XAttribute("Type", "Bool"), new XAttribute("BlockNumber", "111"), new XAttribute("BitOffset", "17000"), new XAttribute("Informative", "true"))));
                elementParts.Add(Access);
            }

            StepHandler.CounterUId += 1;

            XElement Access2 = new XElement("Access", new XAttribute("Scope", "GlobalVariable"), new XAttribute("UId", StepHandler.CounterUId.ToString())
                                , new XElement("Symbol"
                                    , new XElement("Component", new XAttribute("Name", stationName))
                                    , new XElement("Component", new XAttribute("Name", name))
                                    , new XElement("Component", new XAttribute("Name", NetList[index]))
                                    , new XElement("Address", new XAttribute("Area", "None"), new XAttribute("Type", "Bool"), new XAttribute("BlockNumber", "111"), new XAttribute("BitOffset", "17000"), new XAttribute("Informative", "true"))));
            elementParts.Add(Access2);

            StepHandler.CounterUId += 1;

            XElement part = new XElement("Part", new XAttribute("Name", "Contact"), new XAttribute("UId", 23));
            elementParts.Add(part);

            XElement part2 = new XElement("Part", new XAttribute("Name", "Coil"), new XAttribute("UId", 24));
            elementParts.Add(part2);

            XElement ParameterWire25 = new XElement("Wire", new XAttribute("UId", "25")
                                       , new XElement("Powerrail")
                                       , new XElement("NameCon", new XAttribute("UId", "23"), new XAttribute("Name", "in")));
            elementWires.Add(ParameterWire25);

            XElement ParameterWire26 = new XElement("Wire", new XAttribute("UId", "26")
                                       , new XElement("IdentCon", new XAttribute("UId", "21"))
                                       , new XElement("NameCon", new XAttribute("UId", "23"), new XAttribute("Name", "operand")));
            elementWires.Add(ParameterWire26);

            XElement ParameterWire27 = new XElement("Wire", new XAttribute("UId", "27")
                                       , new XElement("NameCon", new XAttribute("UId", "23"), new XAttribute("Name", "out"))
                                       , new XElement("NameCon", new XAttribute("UId", "24"), new XAttribute("Name", "in")));
            elementWires.Add(ParameterWire27);

            XElement ParameterWire28 = new XElement("Wire", new XAttribute("UId", "28")
                                       , new XElement("IdentCon", new XAttribute("UId", "22"))
                                       , new XElement("NameCon", new XAttribute("UId", "24"), new XAttribute("Name", "operand")));
            elementWires.Add(ParameterWire28);

            xmlDoc.Save(path);
        }
    }
}

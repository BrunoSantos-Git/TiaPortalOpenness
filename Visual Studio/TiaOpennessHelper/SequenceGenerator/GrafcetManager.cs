using Siemens.Engineering.SW;
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
    public class GrafcetManager
    {
        public static string SavePath { get; set; }
        private static string path;
        private static List<string> specialStepsEnd;
        private static List<string> specialStepsBegin;
        private static List<string> names;
        private static List<string> descriptions;
        private static int stepNumber;
        private static int first;
        private static int connectionCounter;
        private static int transCounter;
        private static int transCounter2;
        private static int stepListCounter;
        private static int counterRN;

        /// <summary>
        /// Constructor
        /// </summary>
        public GrafcetManager()
        {
            names = new List<string>();
            descriptions = new List<string>();
            stepNumber = 1;
            first = 1;
            connectionCounter = 0;
            transCounter = 1;
            transCounter2 = 1;
            stepListCounter = 0;
            counterRN = 1;
    }

        /// <summary>
        /// Generates the Grafcet Part
        /// </summary>
        /// <param name="matrizs"></param>
        /// <param name="Names"></param>
        /// <param name="plcSoftware"></param>
        public static void GenerateGrafcet(List<object[,]> matrizs, List<string> Names, PlcSoftware plcSoftware)
        {
            stepListCounter = 0;
            string[] dataTypes = 
            {
                "G7_MOPPlus_V6",
                "G7_SQFlagsPlus_V6",
                "G7_OffsetsPlus_V6",
                "G7_GCFlagsPlus_V6",
                "G7_TransitionPlus_V6",
                "G7_StepPlus_V6",
                "G7_RTDataPlus_V6"
            };

            if (plcSoftware != null)
            {
                dataTypes = GetDataTypes(plcSoftware);
                if(dataTypes.Contains(null) || dataTypes.Contains(""))
                {
                    MessageBox.Show("All data types required to create steps were not found.\nSome variables will be created with data type \"Struct\"", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    dataTypes = dataTypes.Select(x => x.Replace(null, "Struct")).ToArray();
                }
            }

            int counter = 0;
            foreach (object[,] matriX in matrizs)
            {
                stepNumber = 1;
                first = 1;
                transCounter = 1;
                transCounter2 = 1;

                StepHandler.GrafcetList = new List<List<StepHandler>>();
                ExcelManager.PrepareExcelValues(matriX, Names[counter]);

                NetworkManager.SavePath = SavePath;

                NetworkManager.GenerateNetWork();

                counter++;
                string sheetName = StepHandler.GrafcetList[0][0].SheetName;
                
                GenerateGrafcetThemePlate(dataTypes);
                if (sheetName.Length == 12) // If sheet name is like AS_000000V01
                {
                    string blockName = sheetName.Substring(3, 6) + "_" + sheetName.Substring(sheetName.Length - 3);
                    GenerateXMLWithTheCorrectNameGraf(blockName + "#AS");
                    path = System.IO.Path.Combine(SavePath, blockName + "#AS_G.xml");
                    TreeViewManager.BlocksCreated.Add(blockName + "#AS_G");
                }
                else if (sheetName.Length == 9) // If sheet name is like AS_000000
                {
                    GenerateXMLWithTheCorrectNameGraf(sheetName.Substring(3) + "#AS");
                    path = System.IO.Path.Combine(SavePath, sheetName.Substring(3) + "#AS_G.xml");
                    TreeViewManager.BlocksCreated.Add(sheetName.Substring(3) + "#AS_G");
                }
                else return;

                connectionCounter = 1;
                specialStepsEnd = new List<string>();
                specialStepsBegin = new List<string>();

                AddStepVariableValues(StepHandler.GrafcetList[0], path, dataTypes);

                foreach (StepHandler step in StepHandler.GrafcetList[0])
                {
                    //VX(step);
                    CreateBlockStaticFieldT(step, path, dataTypes);
                    CreateBlockStaticFieldC(step, path, dataTypes);
                    if (first == 1)
                    {
                        //Inserts Steps
                        CreateBlockSteps(step, "true", path);
                    }
                    else
                    {
                        //Inserts Steps
                        CreateBlockSteps(step, "false", path);
                    }

                    //Makes a list of Steps that are special and are not to be used in the making of connects
                    FillSpecialList();

                    //Opens the file and Corrects the & problem generated by the xml 
                    ReplaceAmps(path);

                    //Creates the Transition
                    CreateBlockTransition(step, path);

                    //Creates the Branchs
                    CreateNewBranch(step, path);

                    //Creates the Connections
                    CreateBlockConnection(step, path);
                    first = 0;
                }
                
                File.Delete("C:/Temp/GrafcetThemePlate.xml");
            }
        }

        /// <summary>
        /// Generates the base Grafcet
        /// </summary>
        /// <param name="dataTypes"></param>
        public static void GenerateGrafcetThemePlate(string[] dataTypes)
        {
            XNamespace xn = "http://www.siemens.com/automation/Openness/SW/Interface/v3";
            XNamespace x3 = "http://www.siemens.com/automation/Openness/SW/NetworkSource/Graph/v4";
            new XDocument(
                    new XElement("Document"
                        , new XElement("Engineering", new XAttribute("version", "V15.1"))
                    , new XElement("DocumentInfo"
                        , new XElement("Created", DateTime.Today.ToString("yyyy-MM-dd") + "T" + DateTime.Now.ToString("HH:mm:ss tt"))
                        , new XElement("ExportSettings", "WithDefaults, WithReadOnly")
                        , new XElement("InstalledProducts"
                            , new XElement("Product"
                                , new XElement("DisplayName", "Totally Integrated Automation Portal")
                                , new XElement("DisplayVersion", "V15.1"))
                            , new XElement("OptionPackage"
                                , new XElement("DisplayName", "TIA Portal Openness")
                                , new XElement("DisplayVersion", "V15.1"))
                            , new XElement("Product"
                                , new XElement("DisplayName", "STEP 7 Professional")
                                , new XElement("DisplayVersion", "V15.1"))
                            , new XElement("OptionPackage"
                                , new XElement("DisplayName", "STEP 7 Safety")
                                , new XElement("DisplayVersion", "V15.1"))
                            , new XElement("Product"
                                , new XElement("DisplayName", "WinCC Professional")
                                , new XElement("DisplayVersion", "V15.1"))
                            , new XElement("OptionPackage"
                                , new XElement("DisplayName", "SIMATIC Visualization Architect")
                                , new XElement("DisplayVersion", "V15.1"))))
                    , new XElement("SW.Blocks.FB", new XAttribute("ID", "0")
                        , new XElement("AttributeList"
                              , new XElement("AcknowledgeErrorsRequired", "false")
                              , new XElement("AutoNumber", "false")
                              , new XElement("CodeModifiedDate", new XAttribute("ReadOnly", "true"), DateTime.Today.ToString("yyyy-MM-dd") + "T" + DateTime.Now.ToString("HH:mm:ss tt"))
                              , new XElement("CompileDate", new XAttribute("ReadOnly", "true"), DateTime.Today.ToString("yyyy-MM-dd") + "T" + DateTime.Now.ToString("HH:mm:ss tt"))
                              , new XElement("CreationDate", new XAttribute("ReadOnly", "true"), DateTime.Today.ToString("yyyy-MM-dd") + "T" + DateTime.Now.ToString("HH:mm:ss tt"))
                              , new XElement("ExtensionBlockName", "FB_S7G_Control_Ext")
                              , new XElement("GraphVersion", "6.0")
                              , new XElement("HandleErrorsWithinBlock", new XAttribute("ReadOnly", "true"), "false")
                              , new XElement("HeaderAuthor")
                              , new XElement("HeaderFamily")
                              , new XElement("HeaderName")
                              , new XElement("HeaderVersion", "1.0")
                              , new XElement("InitialValuesAcquisition", "false")
                              , new XElement("Interface", new XElement(xn + "Sections", new XAttribute("xmlns", "http://www.siemens.com/automation/Openness/SW/Interface/v3")
                                                                                 , new XElement("Section", new XAttribute("Name", "Base")
                                                                                     , new XElement("Sections", new XAttribute("Datatype", "GRAPH_BASE"), new XAttribute("Version", "1.0")
                                                                                         , new XElement("Section", new XAttribute("Name", "Input"))
                                                                                         , new XElement("Section", new XAttribute("Name", "Output"))
                                                                                         , new XElement("Section", new XAttribute("Name", "InOut"))
                                                                                         , new XElement("Section", new XAttribute("Name", "Static")
                                                                                             , new XElement("Member", new XAttribute("Name", "OFF_SQ_BASE"), new XAttribute("Datatype", "Bool"))
                                                                                                      )
                                                                                                  )
                                                                                              )
                                                                                 , new XElement("Section", new XAttribute("Name", "Input")
                                                                                     , new XElement("Member", new XAttribute("Name", "OFF_SQ"), new XAttribute("Datatype", "Bool"), new XAttribute("Remanence", "NonRetain"), new XAttribute("Accessibility", "Public")
                                                                                         , new XElement("AttributeList"
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                                      )
                                                                                         , new XElement("Comment", new XAttribute("Informative", "true")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "en-US"), "Turn sequence off")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), "Schrittkette ausschalten (OFF_SEQUENCE)")
                                                                                                      )
                                                                                                  )
                                                                                     , new XElement("Member", new XAttribute("Name", "INIT_SQ"), new XAttribute("Datatype", "Bool"), new XAttribute("Remanence", "NonRetain"), new XAttribute("Accessibility", "Public")
                                                                                         , new XElement("AttributeList"
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                                      )
                                                                                         , new XElement("Comment", new XAttribute("Informative", "true")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "en-US"), "Set sequence to initial state")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), "Schrittkette in Initialzustand versetzen (INIT_SEQUENCE)")
                                                                                                      )
                                                                                                  )
                                                                                      , new XElement("Member", new XAttribute("Name", "ACK_EF"), new XAttribute("Datatype", "Bool"), new XAttribute("Remanence", "NonRetain"), new XAttribute("Accessibility", "Public")
                                                                                         , new XElement("AttributeList"
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                                      )
                                                                                         , new XElement("Comment", new XAttribute("Informative", "true")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "en-US"), "Acknowledge all errors and faults")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), "Alle Fehler und Störungen quittieren (ACKNOWLEDGE_ERROR_FAULT)")
                                                                                                      )
                                                                                                  )
                                                                                      , new XElement("Member", new XAttribute("Name", "S_PREV"), new XAttribute("Datatype", "Bool"), new XAttribute("Remanence", "NonRetain"), new XAttribute("Accessibility", "Public")
                                                                                         , new XElement("AttributeList"
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                                      )
                                                                                         , new XElement("Comment", new XAttribute("Informative", "true")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "en-US"), "Output previous step in parameter S_NO")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), "Vorherigen Schritt in S_NO anzeigen (PREVIOUS_STEP)")
                                                                                                      )
                                                                                                  )
                                                                                      , new XElement("Member", new XAttribute("Name", "S_NEXT"), new XAttribute("Datatype", "Bool"), new XAttribute("Remanence", "NonRetain"), new XAttribute("Accessibility", "Public")
                                                                                         , new XElement("AttributeList"
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                                      )
                                                                                         , new XElement("Comment", new XAttribute("Informative", "true")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "en-US"), "Indicate next step in parameter S_NO")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), "Nächsten Schritt in S_NO anzeigen (NEXT_STEP)")
                                                                                                      )
                                                                                                  )
                                                                                      , new XElement("Member", new XAttribute("Name", "SW_AUTO"), new XAttribute("Datatype", "Bool"), new XAttribute("Remanence", "NonRetain"), new XAttribute("Accessibility", "Public")
                                                                                         , new XElement("AttributeList"
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                                      )
                                                                                         , new XElement("Comment", new XAttribute("Informative", "true")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "en-US"), "Automatic mode")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), "Betriebsart Automatik einstellen (SWITCH_MODE_AUTOMATIC)")
                                                                                                      )
                                                                                                  )
                                                                                      , new XElement("Member", new XAttribute("Name", "SW_TAP"), new XAttribute("Datatype", "Bool"), new XAttribute("Remanence", "NonRetain"), new XAttribute("Accessibility", "Public")
                                                                                         , new XElement("AttributeList"
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                                      )
                                                                                         , new XElement("Comment", new XAttribute("Informative", "true")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "en-US"), "Semiautomatic/switch with transition")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), "Tippbetrieb 'T und T_PUSH' einstellen (SWITCH_MODE_TRANSITION_AND_PUSH)")
                                                                                                      )
                                                                                                  )
                                                                                      , new XElement("Member", new XAttribute("Name", "SW_MAN"), new XAttribute("Datatype", "Bool"), new XAttribute("Remanence", "NonRetain"), new XAttribute("Accessibility", "Public")
                                                                                         , new XElement("AttributeList"
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                                      )
                                                                                         , new XElement("Comment", new XAttribute("Informative", "true")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "en-US"), "Manual mode")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), "Handbetrieb einstellen (SWITCH_MODE_MANUAL)")
                                                                                                      )
                                                                                                  )
                                                                                      , new XElement("Member", new XAttribute("Name", "S_SEL"), new XAttribute("Datatype", "Int"), new XAttribute("Remanence", "NonRetain"), new XAttribute("Accessibility", "Public")
                                                                                         , new XElement("AttributeList"
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                                      )
                                                                                         , new XElement("Comment", new XAttribute("Informative", "true")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "en-US"), "Select step to be output to S_NO")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), "Schritt zum Anzeigen in S_NO vorgeben (STEP_SELECT)")
                                                                                                      )
                                                                                                  )
                                                                                      , new XElement("Member", new XAttribute("Name", "S_ON"), new XAttribute("Datatype", "Bool"), new XAttribute("Remanence", "NonRetain"), new XAttribute("Accessibility", "Public")
                                                                                         , new XElement("AttributeList"
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                                      )
                                                                                         , new XElement("Comment", new XAttribute("Informative", "true")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "en-US"), "Activate step indicated in S_NO")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), "In S_NO angezeigten Schritt aktivieren (STEP_ON)")
                                                                                                      )
                                                                                                  )
                                                                                      , new XElement("Member", new XAttribute("Name", "S_OFF"), new XAttribute("Datatype", "Bool"), new XAttribute("Remanence", "NonRetain"), new XAttribute("Accessibility", "Public")
                                                                                         , new XElement("AttributeList"
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                                      )
                                                                                         , new XElement("Comment", new XAttribute("Informative", "true")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "en-US"), "Deactivate step indicated S_NO")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), "In S_NO angezeigten Schritt deaktivieren (STEP_OFF)")
                                                                                                      )
                                                                                                  )
                                                                                      , new XElement("Member", new XAttribute("Name", "T_PUSH"), new XAttribute("Datatype", "Bool"), new XAttribute("Remanence", "NonRetain"), new XAttribute("Accessibility", "Public")
                                                                                         , new XElement("AttributeList"
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                                      )
                                                                                         , new XElement("Comment", new XAttribute("Informative", "true")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "en-US"), "Enable transition to switch in semi automatic mode")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), "Schaltfreigabe für Transition bei SW_TAP und SW_TOP (PUSH_TRANSITION)")
                                                                                                      )
                                                                                                  )
                                                                                              )
                                                                                 , new XElement("Section", new XAttribute("Name", "Output")
                                                                                      , new XElement("Member", new XAttribute("Name", "S_NO"), new XAttribute("Datatype", "Int"), new XAttribute("Remanence", "NonRetain"), new XAttribute("Accessibility", "Public")
                                                                                         , new XElement("AttributeList"
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                                      )
                                                                                         , new XElement("Comment", new XAttribute("Informative", "true")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "en-US"), "Step number")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), "Schrittnummer (STEP_NUMBER)")
                                                                                                      )
                                                                                                  )
                                                                                      , new XElement("Member", new XAttribute("Name", "S_MORE"), new XAttribute("Datatype", "Bool"), new XAttribute("Remanence", "NonRetain"), new XAttribute("Accessibility", "Public")
                                                                                         , new XElement("AttributeList"
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                                      )
                                                                                         , new XElement("Comment", new XAttribute("Informative", "true")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "en-US"), "More steps are available and can be shown in S_NO")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), "Weitere Schritte zum Anzeigen verfügbar (MORE_STEPS)")
                                                                                                      )
                                                                                                  )
                                                                                         , new XElement("Member", new XAttribute("Name", "S_ACTIVE"), new XAttribute("Datatype", "Bool"), new XAttribute("Remanence", "NonRetain"), new XAttribute("Accessibility", "Public")
                                                                                         , new XElement("AttributeList"
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                                      )
                                                                                         , new XElement("Comment", new XAttribute("Informative", "true")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "en-US"), "Step indicated in S_NO is active")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), "Schritt S_NO ist aktiv (STEP_ACTIVE)")
                                                                                                      )
                                                                                                  )
                                                                                         , new XElement("Member", new XAttribute("Name", "ERR_FLT"), new XAttribute("Datatype", "Bool"), new XAttribute("Remanence", "NonRetain"), new XAttribute("Accessibility", "Public")
                                                                                         , new XElement("AttributeList"
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                                      )
                                                                                         , new XElement("Comment", new XAttribute("Informative", "true")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "en-US"), "Interlock or supervision group error")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), "Sammelfehler Verriegelungen oder Überwachungen  (IL_ERROR_OR_SV_FAULT)")
                                                                                                      )
                                                                                                  )
                                                                                         , new XElement("Member", new XAttribute("Name", "AUTO_ON"), new XAttribute("Datatype", "Bool"), new XAttribute("Remanence", "NonRetain"), new XAttribute("Accessibility", "Public")
                                                                                         , new XElement("AttributeList"
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                                      )
                                                                                         , new XElement("Comment", new XAttribute("Informative", "true")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "en-US"), "Automatic mode is active")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), "Betriebsart SW_AUTO ist eingeschaltet (AUTOMATIC_IS_ON)")
                                                                                                      )
                                                                                                  )
                                                                                         , new XElement("Member", new XAttribute("Name", "TAP_ON"), new XAttribute("Datatype", "Bool"), new XAttribute("Remanence", "NonRetain"), new XAttribute("Accessibility", "Public")
                                                                                         , new XElement("AttributeList"
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                                      )
                                                                                         , new XElement("Comment", new XAttribute("Informative", "true")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "en-US"), "Semiautomatic mode/step with transition enabled")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), "Betriebsart SW_AUTO ist eingeschaltet (AUTOMATIC_IS_ON)")
                                                                                                      )
                                                                                                  )
                                                                                         , new XElement("Member", new XAttribute("Name", "MAN_ON"), new XAttribute("Datatype", "Bool"), new XAttribute("Remanence", "NonRetain"), new XAttribute("Accessibility", "Public")
                                                                                         , new XElement("AttributeList"
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                              , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                                      )
                                                                                         , new XElement("Comment", new XAttribute("Informative", "true")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "en-US"), "Manual mode is active")
                                                                                              , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), "Betriebsart SW_MAN ist eingeschaltet (MANUAL_IS_ON)")
                                                                                                      )
                                                                                                  )
                                                                                              )
                                                                                 , new XElement("Section", new XAttribute("Name", "InOut"))
                                                                                 , new XElement("Section", new XAttribute("Name", "Static")
                                                                                    , new XElement("Member", new XAttribute("Name", "RT_DATA"), new XAttribute("Datatype", dataTypes[6]), new XAttribute("Version", "1.0"), new XAttribute("Remanence", "NonRetain"), new XAttribute("Accessibility", "Public")
                                                                                        , new XElement("AttributeList"
                                                                                            , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "false")
                                                                                            , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "false")
                                                                                            , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "false")
                                                                                            , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                            , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                            , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "false")
                                                                                                       )
                                                                                        , new XElement("Comment", new XAttribute("Informative", "true")
                                                                                            , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), "Interner Datenbereich")
                                                                                            , new XElement("MultiLanguageText", new XAttribute("Lang", "es-ES"), "Área de datos interna")
                                                                                            , new XElement("MultiLanguageText", new XAttribute("Lang", "en-US"), "Internal data area")
                                                                                            , new XElement("MultiLanguageText", new XAttribute("Lang", "fr-FR"), "Internal data area")
                                                                                            , new XElement("MultiLanguageText", new XAttribute("Lang", "zh-CN"), "内部数据区")

                                                                                            , new XElement("MultiLanguageText", new XAttribute("Lang", "pl-PL"), "Internal data area")
                                                                                            , new XElement("MultiLanguageText", new XAttribute("Lang", "pt-BR"), "Internal data area")
                                                                                            , new XElement("MultiLanguageText", new XAttribute("Lang", "ru-RU"), "Internal data area")
                                                                                            , new XElement("MultiLanguageText", new XAttribute("Lang", "sk-SK"), "Internal data area")
                                                                                            , new XElement("MultiLanguageText", new XAttribute("Lang", "nl-BE"), "Internal data area")
                                                                                            , new XElement("MultiLanguageText", new XAttribute("Lang", "hu-HU"), "Internal data area"))))
                                                                                 , new XElement("Section", new XAttribute("Name", "Temp")
                                                                                    , new XElement("Member", new XAttribute("Name", "no_action"), new XAttribute("Datatype", "Bool")
                                                                                        , new XElement("AttributeList"
                                                                                            , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                            , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "false")
                                                                                            , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                                                                                       )
                                                                                                   )
                                                                                                )
                                                                                 , new XElement("Section", new XAttribute("Name", "Constant"))))
                              , new XElement("InterfaceModifiedDate", new XAttribute("ReadOnly", "true"), DateTime.Today.ToString("yyyy-MM-dd") + "T" + DateTime.Now.ToString("HH:mm:ss tt"))
                              , new XElement("IsConsistent", new XAttribute("ReadOnly", "true"), "true")
                              , new XElement("IsIECCheckEnabled", "false")
                              , new XElement("IsKnowHowProtected", new XAttribute("ReadOnly", "true"), "false")
                              , new XElement("ISMultiInstanceCapable", new XAttribute("ReadOnly", "true"), "false")
                              , new XElement("IsWriteProtected", new XAttribute("ReadOnly", "true"), "false")
                              , new XElement("LanguageInNetworks", "LAD")
                              , new XElement("LibraryConformanceStatus", new XAttribute("ReadOnly", "true"), "The object is library-conformant.")
                              , new XElement("LockOperatingMode", "true")
                              , new XElement("MemoryLayout", "Optimized")
                              , new XElement("ModifiedDate", new XAttribute("ReadOnly", "true"), DateTime.Today.ToString("yyyy-MM-dd") + "T" + DateTime.Now.ToString("HH:mm:ss tt"))
                              , new XElement("Name", "GrafcetThemePlate")
                              , new XElement("Number", "777")
                              , new XElement("ParameterModified", new XAttribute("ReadOnly", "true"), DateTime.Today.ToString("yyyy-MM-dd") + "T" + DateTime.Now.ToString("HH:mm:ss tt"))
                              , new XElement("PermanentILProcessingInMANMode", "false")
                              , new XElement("PLCSimAdvancedSupport", new XAttribute("ReadOnly", "true"), "true")
                              , new XElement("ProgrammingLanguage", "GRAPH")
                              , new XElement("SetENOAutomatically", "false")
                              , new XElement("SkipSteps", "false")
                              , new XElement("StructureModified", new XAttribute("ReadOnly", "true"), DateTime.Today.ToString("yyyy-MM-dd") + "T" + DateTime.Now.ToString("HH:mm:ss tt"))
                              , new XElement("UDABlockProperties")
                              , new XElement("UDAEnableTagReadback", "false")
                              , new XElement("WithAlarmHandling", "true")
                              )
                           , new XElement("ObjectList"
                              , new XElement("MultilingualText", new XAttribute("ID", StepHandler.MultilingualCounter), new XAttribute("CompositionName", "Comment")
                                  , new XElement("ObjectList"
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 1), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "de-DE")
                                              , new XElement("Text")))
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 2), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "es-ES")
                                              , new XElement("Text", "")))
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 3), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "en-US")
                                              , new XElement("Text", "")))
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 4), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "fr-FR")
                                              , new XElement("Text", "")))
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 5), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "zh-CN")
                                              , new XElement("Text", "")))

                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 6), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "pl-PL")
                                              , new XElement("Text", "")))
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 7), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "pt-BR")
                                              , new XElement("Text", "")))
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 8), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "ru-RU")
                                              , new XElement("Text", "")))
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 9), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "sk-SK")
                                              , new XElement("Text", "")))
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 10), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "nl-BE")
                                              , new XElement("Text", "")))
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 11), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "hu-HU")
                                              , new XElement("Text", "")))
                                              )
                                 )
                               , new XElement("MultilingualText", new XAttribute("ID", StepHandler.MultilingualCounter + 12), new XAttribute("CompositionName", "Title")
                                                  , new XElement("ObjectList"
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 13), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "de-DE")
                                                              , new XElement("Text", "")))
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 14), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "es-ES")
                                                              , new XElement("Text", "")))
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 15), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "en-US")
                                                              , new XElement("Text", "")))
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 16), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "fr-FR")
                                                              , new XElement("Text", "")))
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 17), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "zh-CN")
                                                              , new XElement("Text", "")))

                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 18), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "pl-PL")
                                                              , new XElement("Text", "")))
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 19), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "pt-BR")
                                                              , new XElement("Text", "")))
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 20), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "ru-RU")
                                                              , new XElement("Text", "")))
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 21), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "sk-SK")
                                                              , new XElement("Text", "")))
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 22), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "nl-BE")
                                                              , new XElement("Text", "")))
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 23), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "hu-HU")
                                                              , new XElement("Text", "")))
                                                              )
                                                             )
                               , new XElement("SW.Blocks.CompileUnit", new XAttribute("ID", "1ASK"), new XAttribute("CompositionName", "CompileUnits")
                                   , new XElement("AttributeList"
                                       , new XElement("NetworkSource"
                                           , new XElement(x3 + "Graph", new XAttribute("xmlns", "http://www.siemens.com/automation/Openness/SW/NetworkSource/Graph/v4")
                                               , new XElement("PreOperations"
                                                   , new XElement("PermanentOperation", new XAttribute("ProgrammingLanguage", "LAD"))
                                                              )
                                               , new XElement("Sequence"
                                                   , new XElement("Title")
                                                   , new XElement("Comment"
                                                       , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"))
                                                       , new XElement("MultiLanguageText", new XAttribute("Lang", "es-ES"))
                                                       , new XElement("MultiLanguageText", new XAttribute("Lang", "en-US"))
                                                       , new XElement("MultiLanguageText", new XAttribute("Lang", "fr-FR"))
                                                       , new XElement("MultiLanguageText", new XAttribute("Lang", "zh-CN"))

                                                       , new XElement("MultiLanguageText", new XAttribute("Lang", "pl-PL"))
                                                       , new XElement("MultiLanguageText", new XAttribute("Lang", "pt-BR"))
                                                       , new XElement("MultiLanguageText", new XAttribute("Lang", "ru-RU"))
                                                       , new XElement("MultiLanguageText", new XAttribute("Lang", "sk-SK"))
                                                       , new XElement("MultiLanguageText", new XAttribute("Lang", "nl-BE"))
                                                       , new XElement("MultiLanguageText", new XAttribute("Lang", "hu-HU"))
                                                                  )
                                                   , new XElement("Steps")
                                                   , new XElement("Transitions")
                                                   , new XElement("Branches")
                                                   , new XElement("Connections")
                                                              )
                                               , new XElement("PostOperations")
                                               , new XElement("AlarmsSettings"
                                                   , new XElement("AlarmSupervisionCategories"
                                                       , new XElement("AlarmSupervisionCategory", new XAttribute("Id", "1"), new XAttribute("DisplayClass", "0"))
                                                       , new XElement("AlarmSupervisionCategory", new XAttribute("Id", "2"), new XAttribute("DisplayClass", "0"))
                                                       , new XElement("AlarmSupervisionCategory", new XAttribute("Id", "3"), new XAttribute("DisplayClass", "0"))
                                                       , new XElement("AlarmSupervisionCategory", new XAttribute("Id", "4"), new XAttribute("DisplayClass", "0"))
                                                       , new XElement("AlarmSupervisionCategory", new XAttribute("Id", "5"), new XAttribute("DisplayClass", "0"))
                                                       , new XElement("AlarmSupervisionCategory", new XAttribute("Id", "6"), new XAttribute("DisplayClass", "0"))
                                                       , new XElement("AlarmSupervisionCategory", new XAttribute("Id", "7"), new XAttribute("DisplayClass", "0"))
                                                       , new XElement("AlarmSupervisionCategory", new XAttribute("Id", "8"), new XAttribute("DisplayClass", "0"))
                                                                  )
                                                   , new XElement("AlarmInterlockCategory", new XAttribute("Id", "1"))
                                                   , new XElement("AlarmSubcategory1Interlock", new XAttribute("Id", "0"))
                                                   , new XElement("AlarmSubcategory2Interlock", new XAttribute("Id", "0"))
                                                   , new XElement("AlarmCategorySupervision", new XAttribute("Id", "1"))
                                                   , new XElement("AlarmSubcategory1Supervision", new XAttribute("Id", "0"))
                                                   , new XElement("AlarmSubcategory2Supervision", new XAttribute("Id", "0"))
                                                   , new XElement("AlarmWarningCategory", new XAttribute("Id", "0"))
                                                   , new XElement("AlarmSubcategory1Warning", new XAttribute("Id", "0"))
                                                   , new XElement("AlarmSubcategory2Warning", new XAttribute("Id", "0"))
                                                              )
                                                          )
                                                      )
                                       , new XElement("ProgrammingLanguage", "GRAPH")
                                                  )
                                   , new XElement("ObjectList"
                                       , new XElement("MultilingualText", new XAttribute("ID", StepHandler.MultilingualCounter + 24), new XAttribute("CompositionName", "Comment")
                                           , new XElement("ObjectList"
                                               , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 25), new XAttribute("CompositionName", "Items")
                                                   , new XElement("AttributeList"
                                                       , new XElement("Culture", "de-DE")
                                                       , new XElement("Text")
                                                                    )
                                                              )
                                               , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 26), new XAttribute("CompositionName", "Items")
                                                   , new XElement("AttributeList"
                                                       , new XElement("Culture", "es-ES")
                                                       , new XElement("Text")
                                                                    )
                                                              )
                                               , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 27), new XAttribute("CompositionName", "Items")
                                                   , new XElement("AttributeList"
                                                       , new XElement("Culture", "en-US")
                                                       , new XElement("Text")
                                                                    )
                                                              )
                                                , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 28), new XAttribute("CompositionName", "Items")
                                                   , new XElement("AttributeList"
                                                       , new XElement("Culture", "fr-FR")
                                                       , new XElement("Text")
                                                                    )
                                                              )
                                                 , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 29), new XAttribute("CompositionName", "Items")
                                                   , new XElement("AttributeList"
                                                       , new XElement("Culture", "zh-CN")
                                                       , new XElement("Text")
                                                                    )
                                                              )

                                                 , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 30), new XAttribute("CompositionName", "Items")
                                                   , new XElement("AttributeList"
                                                       , new XElement("Culture", "pl-PL")
                                                       , new XElement("Text")
                                                                    )
                                                              )
                                                 , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 31), new XAttribute("CompositionName", "Items")
                                                   , new XElement("AttributeList"
                                                       , new XElement("Culture", "pt-BR")
                                                       , new XElement("Text")
                                                                    )
                                                              )
                                                 , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 32), new XAttribute("CompositionName", "Items")
                                                   , new XElement("AttributeList"
                                                       , new XElement("Culture", "ru-RU")
                                                       , new XElement("Text")
                                                                    )
                                                              )
                                                 , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 33), new XAttribute("CompositionName", "Items")
                                                   , new XElement("AttributeList"
                                                       , new XElement("Culture", "sk-SK")
                                                       , new XElement("Text")
                                                                    )
                                                              )
                                                 , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 34), new XAttribute("CompositionName", "Items")
                                                   , new XElement("AttributeList"
                                                       , new XElement("Culture", "nl-BE")
                                                       , new XElement("Text")
                                                                    )
                                                              )
                                                 , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 35), new XAttribute("CompositionName", "Items")
                                                   , new XElement("AttributeList"
                                                       , new XElement("Culture", "hu-HU")
                                                       , new XElement("Text")
                                                                    )
                                                              )
                                                          )
                                                      )
                                       , new XElement("MultilingualText", new XAttribute("ID", StepHandler.MultilingualCounter + 36), new XAttribute("CompositionName", "Title")
                                           , new XElement("ObjectList"
                                               , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 37), new XAttribute("CompositionName", "Items")
                                                   , new XElement("AttributeList"
                                                       , new XElement("Culture", "de-DE")
                                                       , new XElement("Text")
                                                                    )
                                                              )
                                               , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 38), new XAttribute("CompositionName", "Items")
                                                   , new XElement("AttributeList"
                                                       , new XElement("Culture", "es-ES")
                                                       , new XElement("Text")
                                                                    )
                                                              )
                                               , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 39), new XAttribute("CompositionName", "Items")
                                                   , new XElement("AttributeList"
                                                       , new XElement("Culture", "en-US")
                                                       , new XElement("Text")
                                                                    )
                                                              )
                                                , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 40), new XAttribute("CompositionName", "Items")
                                                   , new XElement("AttributeList"
                                                       , new XElement("Culture", "fr-FR")
                                                       , new XElement("Text")
                                                                    )
                                                              )
                                                 , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 41), new XAttribute("CompositionName", "Items")
                                                   , new XElement("AttributeList"
                                                       , new XElement("Culture", "zh-CN")
                                                       , new XElement("Text")
                                                                    )
                                                              )

                                                 , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 42), new XAttribute("CompositionName", "Items")
                                                   , new XElement("AttributeList"
                                                       , new XElement("Culture", "pl-PL")
                                                       , new XElement("Text")
                                                                    )
                                                              )
                                                 , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 43), new XAttribute("CompositionName", "Items")
                                                   , new XElement("AttributeList"
                                                       , new XElement("Culture", "pt-BR")
                                                       , new XElement("Text")
                                                                    )
                                                              )
                                                 , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 44), new XAttribute("CompositionName", "Items")
                                                   , new XElement("AttributeList"
                                                       , new XElement("Culture", "ru-RU")
                                                       , new XElement("Text")
                                                                    )
                                                              )
                                                 , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 45), new XAttribute("CompositionName", "Items")
                                                   , new XElement("AttributeList"
                                                       , new XElement("Culture", "sk-SK")
                                                       , new XElement("Text")
                                                                    )
                                                              )
                                                 , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 46), new XAttribute("CompositionName", "Items")
                                                   , new XElement("AttributeList"
                                                       , new XElement("Culture", "nl-BE")
                                                       , new XElement("Text")
                                                                    )
                                                              )
                                                 , new XElement("MultilingualTextItem", new XAttribute("ID", StepHandler.MultilingualCounter + 47), new XAttribute("CompositionName", "Items")
                                                   , new XElement("AttributeList"
                                                       , new XElement("Culture", "hu-HU")
                                                       , new XElement("Text")
                                                                    )
                                                              )
                                                          )
                                                      )
                                                  )
                                              )
                           )
                        )
                    )
                )
                .Save("C:/Temp/GrafcetThemePlate.xml");
            StepHandler.MultilingualCounter += 48;
        }

        /// <summary>
        /// Adds the dafaults variables to the static field
        /// </summary>
        /// <param name="stepsList"></param>
        /// <param name="xmlPath"></param>
        /// <param name="dataTypes"></param>
        public static void AddStepVariableValues(List<StepHandler> stepsList, string xmlPath, string[] dataTypes)
        {
            XDocument xmlDocGraf = XDocument.Load(xmlPath);
            XmlNamespaceManager oManager = new XmlNamespaceManager(new NameTable());
            oManager.AddNamespace("xxx", "http://www.siemens.com/automation/Openness/SW/Interface/v3");
            var element = xmlDocGraf.XPathSelectElement("/Document/SW.Blocks.FB[@ID='0']/AttributeList/Interface/xxx:Sections/Section[@Name='Static']", oManager);

            XElement SQ_FLAGS = new XElement("Member", new XAttribute("Name", "SQ_FLAGS"), new XAttribute("Datatype", "Struct"), new XAttribute("Remanence", "NonRetain"), new XAttribute("Accessibility", "Public")
                                    , new XElement("AttributeList"
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "SetPoint"), new XAttribute("SystemDefined", "true"), "false")
                                                   )
                                    , new XElement("Comment"
                                        , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), "Schrittkettenmerker (SEQUENCE_FLAGS)")
                                        , new XElement("MultiLanguageText", new XAttribute("Lang", "sk-SK"), "Schrittkettenmerker (SEQUENCE_FLAGS)")
                                                   )
                                             );

            names = new List<string>();
            descriptions = new List<string>();

            names.Add("ERR_FLT");
            names.Add("ERROR");
            names.Add("FAULT");
            names.Add("RT_FAIL");
            names.Add("NO_SNO");
            names.Add("NF_OFL");
            names.Add("SA_OFL");
            names.Add("TV_OFL");
            names.Add("MSG_OFL");
            names.Add("NO_SWI");
            names.Add("CYC_OP");
            names.Add("AS_MSG");
            names.Add("AS_SEND");
            names.Add("SQ_BUSY");
            names.Add("SA_BUSY");
            names.Add("AS_SIG");

            descriptions.Add("Sammel-Verriegelungs- oder Ueberwachungsfehler (ERROR_OR_FAULT)");
            descriptions.Add("Sammel-Verriegelungsfehler (IL_ERROR)");
            descriptions.Add("Sammel-Ueberwachungsfehler (SV_FAULT)");
            descriptions.Add("Laufzeitfehler (RUNTIME_FAILURE)");
            descriptions.Add("Angeforderte Schrittnummer nicht gefunden (NO_STEP_NO)");
            descriptions.Add("Ueberlauf: zu viele Anforderungen ON oder OFF (ON_OFF_OVERFLOW)");
            descriptions.Add("Ueberlauf: zu viele Schritte waeren aktiv (S_ACTIVE_OVERFLOW)");
            descriptions.Add("Ueberlauf: zu viele Transitionen waeren gueltig (T_VALID_OVERFLOW)");
            descriptions.Add("Ueberlauf: keine Meldungs-Handles mehr fuer SFC17 (MESSAGE_OVEFLOW)");
            descriptions.Add("Nicht Schalten in diesem Zyklus (NO_SWITCH)");
            descriptions.Add("Zyklische Bearbeitung der Schrittkette nach Initialisierung (CYCLIC_OPERATION)");
            descriptions.Add("Meldungsbearbeitung ueber SFCs aktiviert oder deaktiviert (AS_MESSAGE_ACTIVATED)");
            descriptions.Add("Meldungen von SFC52 senden oder nur speichern (SEND_STORE)");
            descriptions.Add("Interner Merker fuer Vorgangsbearbeitung (SEQUENCE_PROCESSING_BUSY)");
            descriptions.Add("Interner Merker fuer Vorgangsbearbeitung (S_ACTIVE_PROCESSING_BUSY)");
            descriptions.Add("Kommt-Geht-Merker fuer Meldungen von SFC17 und SFC18 (SIGNAL_RISING_FALLING)");


            for (int i = 0; i < 16; i++)
            {
                XElement InnerValue = new XElement("Member", new XAttribute("Name", names[i]), new XAttribute("Datatype", "Bool")
                                    , new XElement("AttributeList"
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "SetPoint"), new XAttribute("SystemDefined", "true"), "false")
                                                   )
                                    , new XElement("Comment"
                                        , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), descriptions[i])
                                        , new XElement("MultiLanguageText", new XAttribute("Lang", "sk-SK"), descriptions[i])
                                                   )
                                             );
                SQ_FLAGS.Add(InnerValue);
            }

            element.Add(SQ_FLAGS);

            XElement TICKS = new XElement("Member", new XAttribute("Name", "TICKS"), new XAttribute("Datatype", "Struct"), new XAttribute("Remanence", "NonRetain"), new XAttribute("Accessibility", "Public")
                                    , new XElement("AttributeList"
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "SetPoint"), new XAttribute("SystemDefined", "true"), "false")
                                                   )
                                    , new XElement("Comment"
                                        , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), "Zeittakte (TIME_TICKS)")
                                        , new XElement("MultiLanguageText", new XAttribute("Lang", "sk-SK"), "Zeittakte (TIME_TICKS)")
                                                   )
                                             );

            names = new List<string>();
            descriptions = new List<string>();

            names.Add("DELTA");
            names.Add("OLD");
            names.Add("NEW");
            descriptions.Add("Zeitdifferenz zwischen Zyklen");
            descriptions.Add("10-ms-Taktzaehlerstand im letzten Zyklus");
            descriptions.Add("10-ms-Taktzaehlerstand in diesem Zyklus");

            for (int i = 0; i < 3; i++)
            {
                XElement InnerValue = new XElement("Member", new XAttribute("Name", names[i]), new XAttribute("Datatype", "Time")
                                    , new XElement("AttributeList"
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "SetPoint"), new XAttribute("SystemDefined", "true"), "false")
                                                   )
                                    , new XElement("Comment"
                                        , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), descriptions[i])
                                        , new XElement("MultiLanguageText", new XAttribute("Lang", "sk-SK"), descriptions[i])
                                                   )
                                             );
                TICKS.Add(InnerValue);
            }

            element.Add(TICKS);

            XElement MOP = new XElement("Member", new XAttribute("Name", "MOP"), new XAttribute("Datatype", "Struct"), new XAttribute("Remanence", "NonRetain"), new XAttribute("Accessibility", "Public")
                                   , new XElement("AttributeList"
                                       , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "false")
                                       , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "false")
                                       , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "false")
                                       , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                       , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "false")
                                       , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                       , new XElement("BooleanAttribute", new XAttribute("Name", "SetPoint"), new XAttribute("SystemDefined", "true"), "false")
                                                  )
                                   , new XElement("Comment"
                                       , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), "Betriebsart (MODE_OF_OPERATION)")
                                       , new XElement("MultiLanguageText", new XAttribute("Lang", "sk-SK"), "Betriebsart (MODE_OF_OPERATION)")
                                                  )
                                   , new XElement("Member", new XAttribute("Name", "AUTO"), new XAttribute("Datatype", "Bool")
                                        , new XElement("AttributeList"
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "false")
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "false")
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "false")
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "false")
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "SetPoint"), new XAttribute("SystemDefined", "true"), "false")
                                                       )
                                        , new XElement("Comment"
                                            , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), "Zustand: Betriebsart SW_AUTO (AUTOMATIC)")
                                            , new XElement("MultiLanguageText", new XAttribute("Lang", "sk-SK"), "Zustand: Betriebsart SW_AUTO (AUTOMATIC)")
                                                       )
                                        , new XElement("StartValue", "true")
                                                 )
                                            );

            names = new List<string>();
            descriptions = new List<string>();

            names.Add("MAN");
            names.Add("TAP");
            names.Add("TOP");
            names.Add("ACK_S");
            names.Add("REG_S");
            names.Add("T_PREV");
            names.Add("T_NEXT");
            names.Add("LOCK");
            names.Add("SUP");
            names.Add("ACKREQ");
            names.Add("SSKIP");
            names.Add("OFF");
            names.Add("INIT");
            names.Add("HALT");
            names.Add("TMS_HALT");
            names.Add("OPS_ZERO");
            names.Add("SACT_DISP");
            names.Add("SEF_DISP");
            names.Add("SALL_DISP");
            names.Add("S_PREV");
            names.Add("S_NEXT");
            names.Add("S_SELOK");
            names.Add("S_ON");
            names.Add("S_OFF");
            names.Add("T_PUSH");
            names.Add("REG");
            names.Add("ACK");
            names.Add("IL_PERM");
            names.Add("T_PERM");
            names.Add("ILP_MAN");
            descriptions.Add("Zustand: Betriebsart SW_MAN (MANUAL)");
            descriptions.Add("Zustand: Betriebsart SW_TAP (TRANSITION_AND_PUSH)");
            descriptions.Add("Zustand: Betriebsart SW_TOP (TRANSITION_OR_PUSH)");
            descriptions.Add("Anforderung: Quittierung des in S_NO angezeigten Schritts (ACKNOWLEDGE_EF_STEP)");
            descriptions.Add("Anforderung: Registrierung des in S_NO angezeigten Schritts (REGISTER_EF_STEP)");
            descriptions.Add("Anforderung: vorherige gueltige Transition in T_NO anzeigen (PREVIOUS_TRANSIT...");
            descriptions.Add("Anforderung: naechste gueltige Transition in T_NO anzeigen (NEXT_TRANSITION)");
            descriptions.Add("Zustand: Verriegelungsbearbeitung ein (INTERLOCKS_ACTIVE)");
            descriptions.Add("Zustand: Ueberwachungsbearbeitung ein (SUPERVISIONS_ACTIVE)");
            descriptions.Add("Zustand: Quittierung erforderlich  (ACKNOWLEDGEMENT_REQUIRED_ACTIVE)");
            descriptions.Add("Zustand: Schritt ueberspringen aktiviert (STEP_SKIPPING_ACTIVE)");
            descriptions.Add("Anforderung: alle Schritte deaktivieren (SET_STATE_TO_OFF)");
            descriptions.Add("Anforderung: in Initialzustand schalten (SET_STATE_TO_INIT)");
            descriptions.Add("Zustand: Kein Schalten der Schrittkette mehr (SEQUENCE_HALT_ACTIVE)");
            descriptions.Add("Zustand: Alle internen Zeitzellen gestoppt (TIMES_HALT_ACTIVE)");
            descriptions.Add("Zustand: Alle Schrittoperanden mit Operation N, L, D auf Null gesetzt (OPERA...");
            descriptions.Add("Zustand: nur aktive Schritte anzeigen (DISPLAY_ACTIVE_STEPS)");
            descriptions.Add("Zustand: nur fehlerhafte oder gestoerte Schritte anzeigen (DISPLAY_STEPS_WITH...");
            descriptions.Add("Zustand: alle Schritte anzeigen (DISPLAY_ALL_STEPS)");
            descriptions.Add("Anforderung: in S_NO vorherigen Schritt anzeigen (PREVIOUS_STEP)");
            descriptions.Add("Anforderung: in S_NO naechsten Schritt anzeigen (NEXT_STEP)");
            descriptions.Add("Anforderung: S_NO auf Schrittnummer aus S_SEL setzen (STEP_SELECTED_OK)");
            descriptions.Add("Anforderung: in S_NO angezeigten Schritt aktivieren (STEP_ON)");
            descriptions.Add("Anforderung: in S_NO angezeigten Schritt deaktivieren (STEP_OFF)");
            descriptions.Add("Anforderung: Schaltfreigabe fuer Transition (TRANSITION_PUSH)");
            descriptions.Add("Anforderung: Registrierung aller Verriegelungs- und Ueberwachungsfehler (REGI...");
            descriptions.Add("Anforderung: Quittierung aller Verriegelungs- und Ueberwachungsfehler (ACKNOW...");
            descriptions.Add("Zustand: Permanente Bearbeitung aller Verriegelungen (INTERLOCKS_PERMANENT)");
            descriptions.Add("Zustand: Permanente Bearbeitung aller Transitionen (TRANSITIONS_PERMANENT)");
            descriptions.Add("Zustand: Permanente Interlockbearbeitung verknuepft mit SW_MAN (IL_PERMANENT_...");

            for (int i = 0; i < 30; i++)
            {
                XElement InnerValue = new XElement("Member", new XAttribute("Name", names[i]), new XAttribute("Datatype", "Bool")
                                        , new XElement("AttributeList"
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "false")
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "false")
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "false")
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "false")
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "SetPoint"), new XAttribute("SystemDefined", "true"), "false"))
                                        , new XElement("Comment"
                                            , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), descriptions[i])
                                            , new XElement("MultiLanguageText", new XAttribute("Lang", "sk-SK"), descriptions[i])));
                MOP.Add(InnerValue);
            }

            element.Add(MOP);

            names = new List<string>();
            descriptions = new List<string>();

            names.Add("S_DISPLAY");
            names.Add("S_SEL_OLD");
            names.Add("S_DISPIDX");
            names.Add("T_DISPIDX");
            descriptions.Add("Interne Anzeige S_NO (STEP_DISPLAY_INTERNAL)");
            descriptions.Add("Letzter Wert S_SEL (S_SEL_PREVIOUS_VALUE)");
            descriptions.Add("Index des in S_NO angezeigten Schritts (STEP_DISPLAY_INDEX");
            descriptions.Add("Index der in T_NO angezeigten Transition (TRANSITION_DISPLAY_INDEX)");

            for (int i = 0; i < 2; i++)
            {
                XElement InnerValue = new XElement("Member", new XAttribute("Name", names[i]), new XAttribute("Datatype", "Int"), new XAttribute("Remanence", "NonRetain"), new XAttribute("Accessibility", "Public")
                                        , new XElement("AttributeList"
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "false")
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "false")
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "false")
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "false")
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "SetPoint"), new XAttribute("SystemDefined", "true"), "false"))
                                        , new XElement("Comment"
                                            , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), descriptions[i])
                                            , new XElement("MultiLanguageText", new XAttribute("Lang", "sk-SK"), descriptions[i])));
                element.Add(InnerValue);
            }

            for (int i = 2; i < 4; i++)
            {
                XElement InnerValue = new XElement("Member", new XAttribute("Name", names[i]), new XAttribute("Datatype", "Byte"), new XAttribute("Remanence", "NonRetain"), new XAttribute("Accessibility", "Public")
                                        , new XElement("AttributeList"
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "false")
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "false")
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "false")
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "false")
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "SetPoint"), new XAttribute("SystemDefined", "true"), "false"))
                                        , new XElement("Comment"
                                            , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), descriptions[i])
                                            , new XElement("MultiLanguageText", new XAttribute("Lang", "sk-SK"), descriptions[i])));
                element.Add(InnerValue);
            }

            XElement Ext_Block = new XElement("Member", new XAttribute("Name", "Ext_Block"), new XAttribute("Datatype", "\"FB_S7G_Control_Ext\""), new XAttribute("Accessibility", "Public")
                                    , new XElement("AttributeList"
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "false")
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "false")
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "false")
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "false")
                                            , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true"))
                                    , new XElement("Sections"
                                        , new XElement("Section", new XAttribute("Name", "Input"))
                                        , new XElement("Section", new XAttribute("Name", "Output"))
                                        , new XElement("Section", new XAttribute("Name", "InOut")
                                            , new XElement("Member", new XAttribute("Name", "io_RT_DATA"), new XAttribute("Datatype", dataTypes[6]), new XAttribute("Version", "1.0"))
                                            , new XElement("Member", new XAttribute("Name", "io_G7T"), new XAttribute("Datatype", "Array[*] of " + dataTypes[4]), new XAttribute("Version", "1.0"))
                                            , new XElement("Member", new XAttribute("Name", "io_G7S"), new XAttribute("Datatype", "Array[*] of " + dataTypes[5]), new XAttribute("Version", "1.0"))
                                            , new XElement("Member", new XAttribute("Name", "io_G7Arrays"), new XAttribute("Datatype", "Array[*] of USInt")
                                                , new XElement("Sections"
                                                    , new XElement("Section", new XAttribute("Name", "None")))))
                                            , new XElement("Section", new XAttribute("Name", "Static")
                                                , new XElement("Member", new XAttribute("Name", "Ext"), new XAttribute("Datatype", "\"ST_S7G_Control_Ext\"")
                                                    , new XElement("Sections"
                                                        , new XElement("Section", new XAttribute("Name", "None")
                                                            , new XElement("Member", new XAttribute("Name", "Version_GRAPH_FB"), new XAttribute("Datatype", "String[10]"))
                                                            , new XElement("Member", new XAttribute("Name", "SQ_CNT"), new XAttribute("Datatype", "USInt"))
                                                            , new XElement("Member", new XAttribute("Name", "S_CNT"), new XAttribute("Datatype", "USInt"))
                                                            , new XElement("Member", new XAttribute("Name", "T_CNT"), new XAttribute("Datatype", "USInt"))
                                                            , new XElement("Member", new XAttribute("Name", "MAX_SACT"), new XAttribute("Datatype", "USInt"))
                                                            , new XElement("Member", new XAttribute("Name", "MAX_TACT"), new XAttribute("Datatype", "USInt"))
                                                            , new XElement("Member", new XAttribute("Name", "GC_FLAGS"), new XAttribute("Datatype", "Struct")
                                                                , new XElement("Member", new XAttribute("Name", "COND_ED"), new XAttribute("Datatype", "USInt"))
                                                                , new XElement("Member", new XAttribute("Name", "SSKIP_ON"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "ACK_REQ"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "ILP_MAN"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "SWM_LOCKED"), new XAttribute("Datatype", "Bool")))
                                                            , new XElement("Member", new XAttribute("Name", "MOP_View"), new XAttribute("Datatype", "Struct")
                                                                , new XElement("Member", new XAttribute("Name", "AUTO"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "MAN"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "TAP"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "TOP"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "OFF"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "INIT"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "HALT"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "TMS_HALT"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "OPS_ZERO"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "T_PUSH"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "T_PERM"), new XAttribute("Datatype", "Bool")))
                                                            , new XElement("Member", new XAttribute("Name", "MOP_Control"), new XAttribute("Datatype", "Struct")
                                                                , new XElement("Member", new XAttribute("Name", "AUTO"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "MAN"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "TAP"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "TOP"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "OFF"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "INIT"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "HALT"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "TMS_HALT"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "OPS_ZERO"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "T_PERM"), new XAttribute("Datatype", "Bool")))
                                                            , new XElement("Member", new XAttribute("Name", "SQ_FLAGS"), new XAttribute("Datatype", "Struct")
                                                                , new XElement("Member", new XAttribute("Name", "ERR_FLT"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "ERROR"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "FAULT"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "NO_SNO"), new XAttribute("Datatype", "Bool")))
                                                            , new XElement("Member", new XAttribute("Name", "Fault"), new XAttribute("Datatype", "Struct")
                                                                , new XElement("Member", new XAttribute("Name", "xF_Seq_Stoe"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "xF_MoreTransitionsTrue"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "xF_FB_Para_NIO"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "xF_FB_Para_NIO_Schrittketten"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "xF_FB_Para_NIO_Simultanzweige"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "xF_FB_Para_NIO_GRAPH_Version"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "xF_MoreT2"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "xF_MoreT2_T"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "xF_NoT"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "xF_NoT_T"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "xF_STNIO"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "xF_STNIO_T"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "mFehlerSK"), new XAttribute("Datatype", "Bool")))
                                                            , new XElement("Member", new XAttribute("Name", "R_TRIG"), new XAttribute("Datatype", "Struct")
                                                                , new XElement("Member", new XAttribute("Name", "BA_Quit"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "xF_moreT_Q"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "xF_MoreT_Edge"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "xF_NoT_Q"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "xF_Not_Edge"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "xF_STNIO_Q"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "xF_STNIO_Edge"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "SK_AUTO_Q"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "SK_AUTO_Edge"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "BA_AUTO_Q"), new XAttribute("Datatype", "Bool")))
                                                            , new XElement("Member", new XAttribute("Name", "Control"), new XAttribute("Datatype", "Struct")
                                                                , new XElement("Member", new XAttribute("Name", "K9_BA_Auto"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "K26_FrAuto"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "cfg_X0"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "cfg_X1"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "cfg_X2"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "FrgAutoVR"), new XAttribute("Datatype", "Bool")))
                                                            , new XElement("Member", new XAttribute("Name", "SNo_active"), new XAttribute("Datatype", "Int"))
                                                            , new XElement("Member", new XAttribute("Name", "TNo_active"), new XAttribute("Datatype", "Int"))
                                                            , new XElement("Member", new XAttribute("Name", "T_True"), new XAttribute("Datatype", "Bool"))
                                                            , new XElement("Member", new XAttribute("Name", "Seq_Auto"), new XAttribute("Datatype", "Bool"))
                                                            , new XElement("Member", new XAttribute("Name", "Start_Sync"), new XAttribute("Datatype", "Bool"))
                                                            , new XElement("Member", new XAttribute("Name", "Sync_1S"), new XAttribute("Datatype", "Bool"))
                                                            , new XElement("Member", new XAttribute("Name", "Sync_1S_No"), new XAttribute("Datatype", "Int"))
                                                            , new XElement("Member", new XAttribute("Name", "Sync_ready"), new XAttribute("Datatype", "Bool"))
                                                            , new XElement("Member", new XAttribute("Name", "Schritt"), new XAttribute("Datatype", "Int"))
                                                            , new XElement("Member", new XAttribute("Name", "Trans"), new XAttribute("Datatype", "Int"))
                                                            , new XElement("Member", new XAttribute("Name", "SyncT2"), new XAttribute("Datatype", "Int"))
                                                            , new XElement("Member", new XAttribute("Name", "SyncT3"), new XAttribute("Datatype", "Int"))
                                                            , new XElement("Member", new XAttribute("Name", "NIO_Trans"), new XAttribute("Datatype", "Int"))
                                                            , new XElement("Member", new XAttribute("Name", "AnzMoreTrans"), new XAttribute("Datatype", "Int"))
                                                            , new XElement("Member", new XAttribute("Name", "AnzNoTrans"), new XAttribute("Datatype", "Int"))
                                                            , new XElement("Member", new XAttribute("Name", "AnzSTNIOTrans"), new XAttribute("Datatype", "Int"))
                                                            , new XElement("Member", new XAttribute("Name", "Sync_intern"), new XAttribute("Datatype", "Struct")
                                                                , new XElement("Member", new XAttribute("Name", "index_Ty"), new XAttribute("Datatype", "USInt"))
                                                                , new XElement("Member", new XAttribute("Name", "index_Sx"), new XAttribute("Datatype", "USInt"))
                                                                , new XElement("Member", new XAttribute("Name", "index_TN"), new XAttribute("Datatype", "USInt"))
                                                                , new XElement("Member", new XAttribute("Name", "prev_index_TN"), new XAttribute("Datatype", "USInt"))
                                                                , new XElement("Member", new XAttribute("Name", "prev_Sync_1S"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "Sync_Sx"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "Sync_Sx_min1TT"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "Sync_Sx_check"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "Sync_Sx_activate"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "TW_Sync_run"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "TW_Sync_ready"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "TW_Sync_value"), new XAttribute("Datatype", "Time"))
                                                                , new XElement("Member", new XAttribute("Name", "aux_TW_Sync_value"), new XAttribute("Datatype", "Time")))
                                                            , new XElement("Member", new XAttribute("Name", "Save_intern"), new XAttribute("Datatype", "Struct")
                                                                , new XElement("Member", new XAttribute("Name", "MOP_before_Manual"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "MOP_T_PERM"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "MOP_HALT"), new XAttribute("Datatype", "Bool"))
                                                                , new XElement("Member", new XAttribute("Name", "MOP_TMS_HALT"), new XAttribute("Datatype", "Bool")))
                                                            , new XElement("Member", new XAttribute("Name", "Load_intern"), new XAttribute("Datatype", "Struct")
                                                                , new XElement("Member", new XAttribute("Name", "MOP_before_Manual"), new XAttribute("Datatype", "Bool"))))))
                                                , new XElement("Member", new XAttribute("Name", "TON_TW_Sync"), new XAttribute("Datatype", "TON_TIME"), new XAttribute("Version", "1.0")
                                                    , new XElement("Sections"
                                                        , new XElement("Section", new XAttribute("Name", "None")
                                                            , new XElement("Member", new XAttribute("Name", "PT"), new XAttribute("Datatype", "Time"))
                                                            , new XElement("Member", new XAttribute("Name", "ET"), new XAttribute("Datatype", "Time"))
                                                            , new XElement("Member", new XAttribute("Name", "IN"), new XAttribute("Datatype", "Bool"))
                                                            , new XElement("Member", new XAttribute("Name", "Q"), new XAttribute("Datatype", "Bool"))))))));

            element.Add(Ext_Block);

            names = new List<string>();

            foreach (StepHandler s in stepsList)
            {
                names.Add(s.StepName);
            };

            for (int z = 0; z < names.Count; z++)
            {
                XElement Member = new XElement("Member", new XAttribute("Name", "As_" + names[z]), new XAttribute("Datatype", "\"STB_AS\""), new XAttribute("Remanence", "NonRetain"), new XAttribute("Accessibility", "Public")
                                    , new XElement("AttributeList"
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true"))
                                    , new XElement("Sections"
                                        , new XElement("Section", new XAttribute("Name", "None")
                                            , new XElement("Member", new XAttribute("Name", "TransBed1"), new XAttribute("Datatype", "Bool"))
                                            , new XElement("Member", new XAttribute("Name", "TransBed2"), new XAttribute("Datatype", "Bool"))
                                            , new XElement("Member", new XAttribute("Name", "TransBed3"), new XAttribute("Datatype", "Bool"))
                                            , new XElement("Member", new XAttribute("Name", "TransBed4"), new XAttribute("Datatype", "Bool"))
                                            , new XElement("Member", new XAttribute("Name", "TransBed"), new XAttribute("Datatype", "Bool"))
                                            , new XElement("Member", new XAttribute("Name", "VerHand1"), new XAttribute("Datatype", "Bool"))
                                            , new XElement("Member", new XAttribute("Name", "VerHand2"), new XAttribute("Datatype", "Bool"))
                                            , new XElement("Member", new XAttribute("Name", "VerHand3"), new XAttribute("Datatype", "Bool"))
                                            , new XElement("Member", new XAttribute("Name", "VerHand4"), new XAttribute("Datatype", "Bool"))
                                            , new XElement("Member", new XAttribute("Name", "VerHand"), new XAttribute("Datatype", "Bool"))
                                            , new XElement("Member", new XAttribute("Name", "VerAuto1"), new XAttribute("Datatype", "Bool"))
                                            , new XElement("Member", new XAttribute("Name", "VerAuto2"), new XAttribute("Datatype", "Bool"))
                                            , new XElement("Member", new XAttribute("Name", "VerAuto3"), new XAttribute("Datatype", "Bool"))
                                            , new XElement("Member", new XAttribute("Name", "VerAuto4"), new XAttribute("Datatype", "Bool"))
                                            , new XElement("Member", new XAttribute("Name", "VerAuto"), new XAttribute("Datatype", "Bool"))
                                            , new XElement("Member", new XAttribute("Name", "SNO_Time"), new XAttribute("Datatype", "Time")))));


                element.Add(Member);
            }

            xmlDocGraf.Save(xmlPath);
        }

        /// <summary>
        /// Changes the names in the Grafcet Xml
        /// </summary>
        /// <param name="name"></param>
        public static void GenerateXMLWithTheCorrectNameGraf(string name)
        {
            XDocument xmlDocGraf = XDocument.Load("C:/Temp/GrafcetThemePlate.xml");
            XmlNamespaceManager oManager = new XmlNamespaceManager(new NameTable());
            oManager.AddNamespace("xxx", "http://www.siemens.com/automation/Openness/SW/Interface/v3");
            var elementGraf = xmlDocGraf.XPathSelectElement("/Document/SW.Blocks.FB[@ID='0']/AttributeList/Name", oManager);
            elementGraf.Value = name;

            xmlDocGraf.Save(SavePath + "/" + name + "_G.xml");
            TreeViewManager.BlocksCreated.Add(name + "_G");
        }

        /// <summary>
        /// Adds the Tras to the static Section 
        /// </summary>
        /// <param name="step"></param>
        /// <param name="path"></param>
        /// <param name="dataTypes"></param>
        public static void CreateBlockStaticFieldT(StepHandler step, string path, string[] dataTypes)
        {
            XDocument xmlDoc = XDocument.Load(path);
            XmlNamespaceManager oManager = new XmlNamespaceManager(new NameTable());
            oManager.AddNamespace("xxx", "http://www.siemens.com/automation/Openness/SW/Interface/v3");
            var element = xmlDoc.XPathSelectElement("/Document/SW.Blocks.FB[@ID='0']/AttributeList/Interface/xxx:Sections/Section[@Name='Static']", oManager);
            int StepNumberCounter = 1;

            if (step.PreviousSteps.Count > 1)
            {
                foreach (string previousStep in step.PreviousSteps)
                {
                    XElement Trans = new XElement("Member", new XAttribute("Name", "T_" + step.StepName + "_" + StepNumberCounter), new XAttribute("Datatype", dataTypes[4]), new XAttribute("Version", "1.0"), new XAttribute("Remanence", "NonRetain"), new XAttribute("Accessibility", "Public")
                                       , new XElement("AttributeList"
                                           , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "true")
                                           , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "true")
                                           , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "false")
                                           , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                           , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                           , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "false")
                                                      )
                                       , new XElement("Comment", new XAttribute("Informative", "true")
                                           , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), "Transitionsstruktur")
                                           , new XElement("MultiLanguageText", new XAttribute("Lang", "es-ES"), "Transition structure")
                                           , new XElement("MultiLanguageText", new XAttribute("Lang", "en-US"), "Transition structure")
                                           , new XElement("MultiLanguageText", new XAttribute("Lang", "fr-FR"), "Transition structure")
                                           , new XElement("MultiLanguageText", new XAttribute("Lang", "zh-CN"), "Transition structure")

                                           , new XElement("MultiLanguageText", new XAttribute("Lang", "pl-PL"), "Transition structure")
                                           , new XElement("MultiLanguageText", new XAttribute("Lang", "pt-BR"), "Transition structure")
                                           , new XElement("MultiLanguageText", new XAttribute("Lang", "ru-RU"), "Transition structure")
                                           , new XElement("MultiLanguageText", new XAttribute("Lang", "sk-SK"), "Transition structure")
                                           , new XElement("MultiLanguageText", new XAttribute("Lang", "nl-BE"), "Transition structure")
                                           , new XElement("MultiLanguageText", new XAttribute("Lang", "hu-HU"), "Transition structure")
                                                      ));
                    StepNumberCounter += 1;
                    transCounter += 1;
                    element.Add(Trans);
                }
            }
            else
            {
                XElement Trans = new XElement("Member", new XAttribute("Name", "T_" + step.StepName), new XAttribute("Datatype", dataTypes[4]), new XAttribute("Version", "1.0"), new XAttribute("Remanence", "NonRetain"), new XAttribute("Accessibility", "Public")
                                       , new XElement("AttributeList"
                                           , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "false")
                                           , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "false")
                                           , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "false")
                                           , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                           , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                           , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "false")
                                                      )
                                       , new XElement("Comment", new XAttribute("Informative", "true")
                                           , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), "Transitionsstruktur")
                                           , new XElement("MultiLanguageText", new XAttribute("Lang", "es-ES"), "Transition structure")
                                           , new XElement("MultiLanguageText", new XAttribute("Lang", "en-US"), "Transition structure")
                                           , new XElement("MultiLanguageText", new XAttribute("Lang", "fr-FR"), "Transition structure")
                                           , new XElement("MultiLanguageText", new XAttribute("Lang", "zh-CN"), "Transition structure")

                                           , new XElement("MultiLanguageText", new XAttribute("Lang", "pl-PL"), "Transition structure")
                                           , new XElement("MultiLanguageText", new XAttribute("Lang", "pt-BR"), "Transition structure")
                                           , new XElement("MultiLanguageText", new XAttribute("Lang", "ru-RU"), "Transition structure")
                                           , new XElement("MultiLanguageText", new XAttribute("Lang", "sk-SK"), "Transition structure")
                                           , new XElement("MultiLanguageText", new XAttribute("Lang", "nl-BE"), "Transition structure")
                                           , new XElement("MultiLanguageText", new XAttribute("Lang", "hu-HU"), "Transition structure")
                                                      ));
                transCounter += 1;
                element.Add(Trans);
            }
            xmlDoc.Save(path);
        }

        /// <summary>
        /// Adds the Commentary to the Static Section
        /// </summary>
        /// <param name="step"></param>
        /// <param name="path"></param>
        /// <param name="dataTypes"></param>
        public static void CreateBlockStaticFieldC(StepHandler step, string path, string[] dataTypes)
        {
            XDocument xmlDoc = XDocument.Load(path);
            XmlNamespaceManager oManager = new XmlNamespaceManager(new NameTable());
            oManager.AddNamespace("xxx", "http://www.siemens.com/automation/Openness/SW/Interface/v3");
            var element = xmlDoc.XPathSelectElement("/Document/SW.Blocks.FB[@ID='0']/AttributeList/Interface/xxx:Sections/Section[@Name='Static']", oManager);

            XElement newBlock = new XElement("Member", new XAttribute("Name", "S_" + step.StepName), new XAttribute("Datatype", dataTypes[5]), new XAttribute("Version", "1.0"), new XAttribute("Remanence", "NonRetain"), new XAttribute("Accessibility", "Public")
                                    , new XElement("AttributeList"
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "false")
                                                   )
                                    , new XElement("Comment", new XAttribute("Informative", "true")
                                        , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), "Schrittstruktur")
                                        , new XElement("MultiLanguageText", new XAttribute("Lang", "es-ES"), "Estructura de la etapa")
                                        , new XElement("MultiLanguageText", new XAttribute("Lang", "en-US"), "Step structure")
                                        , new XElement("MultiLanguageText", new XAttribute("Lang", "fr-FR"), "Step structure")
                                        , new XElement("MultiLanguageText", new XAttribute("Lang", "zh-CN"), "步结构")

                                        , new XElement("MultiLanguageText", new XAttribute("Lang", "pl-PL"), "Step structure")
                                        , new XElement("MultiLanguageText", new XAttribute("Lang", "pt-BR"), "Step structure")
                                        , new XElement("MultiLanguageText", new XAttribute("Lang", "ru-RU"), "Step structure")
                                        , new XElement("MultiLanguageText", new XAttribute("Lang", "sk-SK"), "Step structure")
                                        , new XElement("MultiLanguageText", new XAttribute("Lang", "nl-BE"), "Step structure")
                                        , new XElement("MultiLanguageText", new XAttribute("Lang", "hu-HU"), "Step structure")));
            element.Add(newBlock);

            xmlDoc.Save(path);
        }

        /// <summary>
        /// Adds the Step part of the grafcet Xml 
        /// </summary>
        /// <param name="step"></param>
        /// <param name="Init"></param>
        /// <param name="path"></param>
        public static void CreateBlockSteps(StepHandler step, string Init, string path)
        {
            XDocument xmlDoc = XDocument.Load(path);
            XmlNamespaceManager oManager = new XmlNamespaceManager(new NameTable());
            oManager.AddNamespace("xxx", "http://www.siemens.com/automation/Openness/SW/NetworkSource/Graph/v4");
            var element = xmlDoc.XPathSelectElement("/Document/SW.Blocks.FB[@ID='0']/ObjectList/SW.Blocks.CompileUnit[@ID='1ASK' and @CompositionName='CompileUnits']/AttributeList/NetworkSource/xxx:Graph/Sequence/Steps", oManager);
            XElement Step = new XElement("Step", new XAttribute("Number", stepNumber), new XAttribute("Init", Init), new XAttribute("Name", "S_" + step.StepName + ": " + step.StepDescription), new XAttribute("MaximumStepTime", "T#10S"), new XAttribute("WarningTime", "T#7S")
                                    , new XElement("Comment"
                                        , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), step.StepDescription)
                                        , new XElement("MultiLanguageText", new XAttribute("Lang", "sk-SK"), step.StepDescription))
                                    , new XElement("Actions"
                                        , new XElement("Title"
                                            , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), step.StepDescription)))
                                    , new XElement("Supervisions"
                                        , new XElement("Supervision", new XAttribute("ProgrammingLanguage", "LAD")
                                            , new XElement("AlarmText"
                                                , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), "S_" + step.StepName + ": " + step.StepDescription)
                                                , new XElement("MultiLanguageText", new XAttribute("Lang", "sk-SK"), "S_" + step.StepName + ": " + step.StepDescription))
                                            , new XElement("FlgNet"
                                                , new XElement("Parts"
                                                    , new XElement("Part", new XAttribute("Name", "SvCoil"), new XAttribute("UId", "21")))
                                                , new XElement("Wires"
                                                    , new XElement("Wire", new XAttribute("UId", "22")
                                                        , new XElement("Powerrail")
                                                        , new XElement("NameCon", new XAttribute("UId", "21"), new XAttribute("Name", "in")))))))
                                    , new XElement("Interlocks"
                                        , new XElement("Interlock", new XAttribute("ProgrammingLanguage", "LAD")
                                            , new XElement("AlarmText"
                                                , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), "S_" + step.StepName + ": " + step.StepDescription)
                                                , new XElement("MultiLanguageText", new XAttribute("Lang", "sk-SK"), "S_" + step.StepName + ": " + step.StepDescription))
                                            , new XElement("FlgNet"
                                                , new XElement("Parts"
                                                    , new XElement("Part", new XAttribute("Name", "IlCoil"), new XAttribute("UId", "21")))
                                                , new XElement("Wires"
                                                    , new XElement("Wire", new XAttribute("UId", "22")
                                                        , new XElement("Powerrail")
                                                        , new XElement("NameCon", new XAttribute("UId", "21"), new XAttribute("Name", "in"))))))));
            element.Add(Step);
            var currentstep = xmlDoc.XPathSelectElement("/Document/SW.Blocks.FB[@ID='0']/ObjectList/SW.Blocks.CompileUnit[@ID='1ASK' and @CompositionName='CompileUnits']/AttributeList/NetworkSource/xxx:Graph/Sequence/Steps/Step[@Number=" + stepNumber + "]/Actions", oManager);

            foreach (string s in step.StepActions)
            {
                if (s == "#no_action")
                {
                    XElement action = new XElement("Action", new XAttribute("Qualifier", "N")
                                    , new XElement("Token", new XAttribute("Text", s))
                                    , new XElement("Token", new XAttribute("Text", "&#xA;"))
                                               );
                    currentstep.Add(action);
                }

                else
                {
                    XElement action = new XElement("Action", new XAttribute("Qualifier", "N")
                                    , new XElement("Token", new XAttribute("Text", '"' + step.SheetName.Substring(3, 6) + step.SheetName.Substring(step.SheetName.Length - 3) + '"' + "." + s))
                                    , new XElement("Token", new XAttribute("Text", "&#xA;")));
                    currentstep.Add(action);
                }

            }

            stepNumber += 1;
            xmlDoc.Save(path);
        }

        /// <summary>
        /// Adds the Trans part of the grafcet Xml 
        /// </summary>
        /// <param name="step"></param>
        /// <param name="path"></param>
        public static void CreateBlockTransition(StepHandler step, string path)
        {
            XDocument xmlDoc = XDocument.Load(path);
            XmlNamespaceManager oManager = new XmlNamespaceManager(new NameTable());
            oManager.AddNamespace("xxx", "http://www.siemens.com/automation/Openness/SW/NetworkSource/Graph/v4");
            var element = xmlDoc.XPathSelectElement("/Document/SW.Blocks.FB[@ID='0']/ObjectList/SW.Blocks.CompileUnit[@ID='1ASK' and @CompositionName='CompileUnits']/AttributeList/NetworkSource/xxx:Graph/Sequence/Transitions", oManager);

            if (step.PreviousSteps.Count > 1) // && found == 0
            {
                transCounter = 1;
                foreach (string s in step.PreviousSteps)
                {
                    XElement trans = new XElement("Transition", new XAttribute("IsMissing", "false"), new XAttribute("Name", "T_" + step.StepName + "_" + transCounter), new XAttribute("Number", transCounter2), new XAttribute("ProgrammingLanguage", "LAD")
                                        , new XElement("FlgNet"
                                            , new XElement("Parts"
                                                , new XElement("Access", new XAttribute("Scope", "GlobalVariable"), new XAttribute("UId", "21")
                                                    , new XElement("Symbol"
                                                        , new XElement("Component", new XAttribute("Name", step.SheetName.Substring(3, 6) + step.SheetName.Substring(step.SheetName.Length - 3)))
                                                        , new XElement("Component", new XAttribute("Name", "AS_" + step.StepName))
                                                        , new XElement("Address", new XAttribute("Area", "None"), new XAttribute("Type", "Bool"), new XAttribute("BlockNumber", "125"), new XAttribute("BitOffset", "17240"), new XAttribute("Informative", "true"))))
                                                , new XElement("Part", new XAttribute("Name", "Contact"), new XAttribute("UId", "22"))
                                                , new XElement("Part", new XAttribute("Name", "TrCoil"), new XAttribute("UId", "23")))
                                            , new XElement("Wires"
                                                , new XElement("Wire", new XAttribute("UId", "24")
                                                    , new XElement("Powerrail")
                                                    , new XElement("NameCon", new XAttribute("UId", "22"), new XAttribute("Name", "in")))
                                                , new XElement("Wire", new XAttribute("UId", "25")
                                                    , new XElement("IdentCon", new XAttribute("UId", "21"))
                                                    , new XElement("NameCon", new XAttribute("UId", "22"), new XAttribute("Name", "operand")))
                                                , new XElement("Wire", new XAttribute("UId", "26")
                                                    , new XElement("NameCon", new XAttribute("UId", "22"), new XAttribute("Name", "out"))
                                                    , new XElement("NameCon", new XAttribute("UId", "23"), new XAttribute("Name", "in"))))));
                    element.Add(trans);
                    transCounter += 1;
                    transCounter2 += 1;
                }

                xmlDoc.Save(path);

            }

            else
            {

                XElement trans = new XElement("Transition", new XAttribute("IsMissing", "false"), new XAttribute("Name", "T_" + step.StepName), new XAttribute("Number", transCounter2), new XAttribute("ProgrammingLanguage", "LAD")
                                        , new XElement("FlgNet"
                                            , new XElement("Parts"
                                                , new XElement("Access", new XAttribute("Scope", "GlobalVariable"), new XAttribute("UId", "21")
                                                    , new XElement("Symbol"
                                                        , new XElement("Component", new XAttribute("Name", step.SheetName.Substring(3, 6) + step.SheetName.Substring(step.SheetName.Length - 3)))
                                                        , new XElement("Component", new XAttribute("Name", "AS_" + step.StepName))
                                                        , new XElement("Address", new XAttribute("Area", "None"), new XAttribute("Type", "Bool"), new XAttribute("BlockNumber", "125"), new XAttribute("BitOffset", "17240"), new XAttribute("Informative", "true"))))
                                                , new XElement("Part", new XAttribute("Name", "Contact"), new XAttribute("UId", "22"))
                                                , new XElement("Part", new XAttribute("Name", "TrCoil"), new XAttribute("UId", "23")))
                                            , new XElement("Wires"
                                                , new XElement("Wire", new XAttribute("UId", "24")
                                                    , new XElement("Powerrail")
                                                    , new XElement("NameCon", new XAttribute("UId", "22"), new XAttribute("Name", "in")))
                                                , new XElement("Wire", new XAttribute("UId", "25")
                                                    , new XElement("IdentCon", new XAttribute("UId", "21"))
                                                    , new XElement("NameCon", new XAttribute("UId", "22"), new XAttribute("Name", "operand")))
                                                , new XElement("Wire", new XAttribute("UId", "26")
                                                    , new XElement("NameCon", new XAttribute("UId", "22"), new XAttribute("Name", "out"))
                                                    , new XElement("NameCon", new XAttribute("UId", "23"), new XAttribute("Name", "in"))))));
                element.Add(trans);
                transCounter2 += 1;

                xmlDoc.Save(path);

            }
        }

        /// <summary>
        /// Creates a new Existing Branch
        /// </summary>
        /// <param name="step"></param>
        /// <param name="path"></param>
        public static void CreateNewBranch(StepHandler step, string path)
        {
            XDocument xmlDoc = XDocument.Load(path);
            XmlNamespaceManager oManager = new XmlNamespaceManager(new NameTable());
            oManager.AddNamespace("xxx", "http://www.siemens.com/automation/Openness/SW/NetworkSource/Graph/v4");

            var element = xmlDoc.XPathSelectElement("/ Document / SW.Blocks.FB[@ID = '0'] / ObjectList / SW.Blocks.CompileUnit[@ID='1ASK' and @CompositionName = 'CompileUnits'] / AttributeList / NetworkSource / xxx:Graph / Sequence / Branches", oManager);

            if (step.NextSteps.Count() > 1)
            {

                //counterRN += 1;
                XElement newBegin = new XElement("Branch", new XAttribute("Number", "000" + counterRN), new XAttribute("Type", "AltBegin"), new XAttribute("Cardinality", step.NextSteps.Count()));
                element.Add(newBegin);
                int innerCounter = 0;
                foreach (StepHandler Step in StepHandler.GrafcetList[stepListCounter])
                {

                    if (step.StepNumber == Step.StepNumber)
                    {
                        StepHandler.GrafcetList[stepListCounter][innerCounter].BranchNumberBegin = "000" + counterRN.ToString();
                    }

                    innerCounter += 1;
                }

            }

            if (step.PreviousSteps.Count() > 1)
            {
                counterRN += 1;
                XElement newEnd = new XElement("Branch", new XAttribute("Number", "000" + counterRN), new XAttribute("Type", "AltEnd"), new XAttribute("Cardinality", step.PreviousSteps.Count()));
                element.Add(newEnd);

                int innerCounter = 0;
                foreach (StepHandler Step in StepHandler.GrafcetList[stepListCounter])
                {

                    if (step.StepNumber == Step.StepNumber)
                    {
                        StepHandler.GrafcetList[stepListCounter][innerCounter].BranchNumberEnd = "000" + counterRN.ToString();
                    }

                    innerCounter += 1;
                }

            }

            xmlDoc.Save(path);
        }

        /// <summary>
        /// Adds the connection to the grafcet Xml 
        /// </summary>
        /// <param name="step"></param>
        /// <param name="path"></param>
        public static void CreateBlockConnection(StepHandler step, string path)
        {
            bool foundBranchPartE = false;
            bool foundBranchPartB = false;

            XDocument xmlDoc = XDocument.Load(path);
            XmlNamespaceManager oManager = new XmlNamespaceManager(new NameTable());
            oManager.AddNamespace("xxx", "http://www.siemens.com/automation/Openness/SW/NetworkSource/Graph/v4");
            var element = xmlDoc.XPathSelectElement("/Document/SW.Blocks.FB[@ID='0']/ObjectList/SW.Blocks.CompileUnit[@ID='1ASK' and @CompositionName='CompileUnits']/AttributeList/NetworkSource/xxx:Graph/Sequence/Connections", oManager);

            foreach (string Step in specialStepsBegin)
            {
                if (Step == step.StepNumber)
                {
                    foundBranchPartB = true;
                    break;
                }
            }
            foreach (string Step in specialStepsEnd)
            {
                if (Step == step.StepNumber)
                {
                    foundBranchPartE = true;
                    break;
                }
            }

            if (step.PreviousSteps.Count == 1 && !foundBranchPartB)
            {
                string linkType = "Direct";

                if (step.StepNumber == "1")
                    linkType = "Jump";

                XElement elementB = new XElement("Connection"
                                            , new XElement("NodeFrom"
                                                , new XElement("StepRef", new XAttribute("Number", step.PreviousSteps[0])))
                                            , new XElement("NodeTo"
                                                , new XElement("TransitionRef", new XAttribute("Number", connectionCounter)))
                                            , new XElement("LinkType", "Direct"));

                XElement elementE = new XElement("Connection"
                                         , new XElement("NodeFrom"
                                            , new XElement("TransitionRef", new XAttribute("Number", connectionCounter)))
                                                , new XElement("NodeTo"
                                                    , new XElement("StepRef", new XAttribute("Number", step.StepNumber)))
                                                        , new XElement("LinkType", linkType));
                element.Add(elementB);
                element.Add(elementE);
                connectionCounter += 1;
            }
            if (step.PreviousSteps.Count > 1 && !foundBranchPartE)
            {
                int inCounter = 0;
                int outCounter = 0;
                string linkType = "Direct";

                if (step.StepNumber == "1")
                    linkType = "Jump";

                foreach (string s in step.PreviousSteps)
                {
                    XElement elementB = new XElement("Connection"
                                                , new XElement("NodeFrom"
                                                    , new XElement("StepRef", new XAttribute("Number", s)))
                                                , new XElement("NodeTo"
                                                    , new XElement("TransitionRef", new XAttribute("Number", connectionCounter)))
                                                , new XElement("LinkType", "Direct"));
                    XElement elementC = new XElement("Connection"
                                               , new XElement("NodeFrom"
                                                   , new XElement("TransitionRef", new XAttribute("Number", connectionCounter)))
                                               , new XElement("NodeTo"
                                                   , new XElement("BranchRef", new XAttribute("Number", step.BranchNumberEnd), new XAttribute("In", inCounter.ToString())))
                                               , new XElement("LinkType", linkType));
                    element.Add(elementC);
                    element.Add(elementB);
                    inCounter += 1;
                    connectionCounter += 1;
                }

                XElement elementD = new XElement("Connection"
                                           , new XElement("NodeFrom"
                                               , new XElement("BranchRef", new XAttribute("Number", step.BranchNumberEnd), new XAttribute("Out", outCounter.ToString())))
                                           , new XElement("NodeTo"
                                               , new XElement("StepRef", new XAttribute("Number", step.StepNumber)))
                                           , new XElement("LinkType", "Direct"));
                element.Add(elementD);
                //outCounter += 1;
            }
            if (step.NextSteps.Count > 1 && !foundBranchPartB)
            {
                int inCounter = 0;
                int outCounter = 0;

                XElement elementB = new XElement("Connection"
                                            , new XElement("NodeFrom"
                                                , new XElement("StepRef", new XAttribute("Number", step.StepNumber)))
                                            , new XElement("NodeTo"
                                                , new XElement("BranchRef", new XAttribute("Number", step.BranchNumberBegin), new XAttribute("In", inCounter.ToString())))
                                            , new XElement("LinkType", "Direct"));

                element.Add(elementB);

                foreach (string s in step.NextSteps)
                {
                    XElement elementC = new XElement("Connection"
                                               , new XElement("NodeFrom"
                                                   , new XElement("BranchRef", new XAttribute("Number", step.BranchNumberBegin), new XAttribute("Out", outCounter.ToString())))
                                               , new XElement("NodeTo"
                                                   , new XElement("TransitionRef", new XAttribute("Number", connectionCounter)))
                                               , new XElement("LinkType", "Direct"));

                    element.Add(elementC);

                    XElement elementD = new XElement("Connection"
                                           , new XElement("NodeFrom"
                                               , new XElement("TransitionRef", new XAttribute("Number", connectionCounter)))
                                           , new XElement("NodeTo"
                                               , new XElement("StepRef", new XAttribute("Number", s)))
                                           , new XElement("LinkType", "Direct"));
                    element.Add(elementD);
                    inCounter += 1;
                    outCounter += 1;
                    connectionCounter += 1;
                }
            }

            xmlDoc.Save(path);
        }

        /// <summary>
        /// Populates the special list
        /// </summary>
        public static void FillSpecialList()
        {
            foreach (StepHandler step in StepHandler.GrafcetList[stepListCounter])
            {
                if (step.NextSteps.Count > 1)
                {
                    foreach (string nextStep in step.NextSteps)
                    {
                        specialStepsBegin.Add(nextStep);
                    }
                }

                if (step.PreviousSteps.Count > 1)
                {
                    foreach (string previousStep in step.PreviousSteps)
                    {
                        specialStepsEnd.Add(previousStep);
                    }
                }
            }

        }

        /// <summary>
        /// Opens the XML file and subs '&'
        /// </summary>
        /// <param name="path"></param>
        public static void ReplaceAmps(string path)
        {
            string text = File.ReadAllText(path);
            text = text.Replace("&amp;#xA;", "&#xA;");
            File.WriteAllText(path, text);
        }

        /// <summary>
        /// Get datatypes from TiaPortal
        /// </summary>
        /// <param name="plcSoftware"></param>
        /// <returns></returns>
        private static string[] GetDataTypes(PlcSoftware plcSoftware)
        {
            string[] dataTypes = new string[7];
            
            dataTypes[0] = GetHigherDataTypeVersion("MOPPlus", plcSoftware);
            dataTypes[1] = GetHigherDataTypeVersion("SQFlagsPlus", plcSoftware);
            dataTypes[2] = GetHigherDataTypeVersion("OffsetsPlus", plcSoftware);
            dataTypes[3] = GetHigherDataTypeVersion("GCFlagsPlus", plcSoftware);
            dataTypes[4] = GetHigherDataTypeVersion("TransitionPlus", plcSoftware);
            dataTypes[5] = GetHigherDataTypeVersion("StepPlus", plcSoftware);
            dataTypes[6] = GetHigherDataTypeVersion("RTDataPlus", plcSoftware);

            return dataTypes;
        }

        /// <summary>
        /// Get higher version of tia portal data type
        /// </summary>
        /// <param name="s">data type name</param>
        /// <param name="plcSoftware">PLC Sofware of TIA Portal</param>
        /// <returns></returns>
        private static string GetHigherDataTypeVersion(string s, PlcSoftware plcSoftware)
        {
            var typesGroup = plcSoftware.TypeGroup.SystemTypeGroups;
            string dataType = null;

            foreach (var group in typesGroup)
            {
                var matchingvalues = group.Types.Where(stringToCheck => stringToCheck.Name.Contains(s));
                dataType = matchingvalues.First().Name;
                if (matchingvalues.Count() > 1)
                {
                    foreach (var value in matchingvalues)
                    {
                        int currentVersion = dataType.Last();   // Current datatype version
                        int valueVersion = value.Name.Last();   // Current foreach "value" version
                        if (currentVersion < valueVersion)      // If "value" version is higher than the current dataType version...
                            dataType = value.Name;              // ... datatype value will become the "value" name
                    }
                }
            }

            return dataType;
        }
    }
}

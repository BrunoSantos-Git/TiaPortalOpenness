using TiaOpennessHelper.ExcelTree;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Excel = Microsoft.Office.Interop.Excel;

namespace TiaOpennessHelper.SafetyMaker
{
    public static class NetworkDBMaker
    {
        /// <summary>
        /// Generate the themeplate of a xml data base
        /// </summary>
        public static void GenerateDataBaseThemePlate()
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
                                , new XElement("DisplayVersion", "V15 Update 2"))
                            , new XElement("OptionPackage"
                                , new XElement("DisplayName", "TIA Portal Openness")
                                , new XElement("DisplayVersion", "V15 Update 2"))
                            , new XElement("Product"
                                , new XElement("DisplayName", "STEP 7 Professional")
                                , new XElement("DisplayVersion", "V15 Update 2"))
                            , new XElement("OptionPackage"
                                , new XElement("DisplayName", "STEP 7 Safety")
                                , new XElement("DisplayVersion", "V15"))
                            , new XElement("Product"
                                , new XElement("DisplayName", "WinCC Professional")
                                , new XElement("DisplayVersion", "V15 Update 2"))
                            , new XElement("OptionPackage"
                                , new XElement("DisplayName", "SIMATIC Visualization Architect")
                                , new XElement("DisplayVersion", "V15 Update 1"))))
                    , new XElement("SW.Blocks.GlobalDB", new XAttribute("ID", "0")
                        , new XElement("AttributeList"
                              , new XElement("AutoNumber", "true")
                              , new XElement("CodeModifiedDate", new XAttribute("ReadOnly", "true"))
                              , new XElement("CompileDate", new XAttribute("ReadOnly", "true"))
                              , new XElement("CreationDate", new XAttribute("ReadOnly", "true"))
                              , new XElement("DBAccessibleFromOPCUA", "true")
                              , new XElement("HeaderAuthor")
                              , new XElement("HeaderFamily")
                              , new XElement("HeaderName")
                              , new XElement("HeaderVersion", "0.1")
                              , new XElement("Interface"
                                    , new XElement(xn + "Sections", new XAttribute("xmlns", "http://www.siemens.com/automation/Openness/SW/Interface/v3")
                                     , new XElement("Section", new XAttribute("Name", "Static")
                                                       )
                                                    )
                                                   )
                              , new XElement("InterfaceModifiedDate", new XAttribute("ReadOnly", "true"))
                              , new XElement("IsConsistent", new XAttribute("ReadOnly", "true"), "true")
                              , new XElement("IsKnowHowProtected", new XAttribute("ReadOnly", "true"), "false")
                              , new XElement("IsPLCDB", new XAttribute("ReadOnly", "true"), "false")
                              , new XElement("MemoryLayout", "Optimized")
                              , new XElement("ModifiedDate", new XAttribute("ReadOnly", "true"))
                              , new XElement("Name", "DBTHEMEPLATE")
                              , new XElement("Number", "183")
                              , new XElement("ParameterModified", new XAttribute("ReadOnly", "true"))
                              , new XElement("ProgrammingLanguage", "DB")
                              , new XElement("StructureModified", new XAttribute("ReadOnly", "true"))

                                   )
                        , new XElement("ObjectList"
                            , new XElement("MultilingualText", new XAttribute("ID", "1"), new XAttribute("CompositionName", "Comment")
                                  , new XElement("ObjectList"
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", "2"), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "de-DE")
                                              , new XElement("Text")))
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", "3"), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "es-ES")
                                              , new XElement("Text")))
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", "4"), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "en-Us")
                                              , new XElement("Text")))
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", "5"), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "fr-FR")
                                              , new XElement("Text")))
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", "6"), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "zh-CN")
                                              , new XElement("Text")))
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", "7"), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "cs-CZ")
                                              , new XElement("Text")))
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", "8"), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "pl-PL")
                                              , new XElement("Text")))
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", "9"), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "pt-BR")
                                              , new XElement("Text")))
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", "A"), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "ru-RU")
                                              , new XElement("Text")))
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", "B"), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "sk-SK")
                                              , new XElement("Text")))
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", "C"), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "nl-BE")
                                              , new XElement("Text")))
                                      , new XElement("MultilingualTextItem", new XAttribute("ID", "D"), new XAttribute("CompositionName", "Items")
                                          , new XElement("AttributeList"
                                              , new XElement("Culture", "hu-HU")
                                              , new XElement("Text")))
                                              )
                                            )
                            , new XElement("MultilingualText", new XAttribute("ID", "E"), new XAttribute("CompositionName", "Title")
                                                  , new XElement("ObjectList"
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", "F"), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "de-DE")
                                                              , new XElement("Text")))
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", "10"), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "es-ES")
                                                              , new XElement("Text")))
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", "11"), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "en-US")
                                                              , new XElement("Text")))
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", "112"), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "fr-FR")
                                                              , new XElement("Text")))
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", "13"), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "zh-CN")
                                                              , new XElement("Text")))
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", "14"), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "cs-CZ")
                                                              , new XElement("Text")))
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", "15"), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "pl-PL")
                                                              , new XElement("Text")))
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", "16"), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "pt-BR")
                                                              , new XElement("Text")))
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", "17"), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "ru-RU")
                                                              , new XElement("Text")))
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", "18"), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "sk-SK")
                                                              , new XElement("Text")))
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", "19"), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "nl-BE")
                                                              , new XElement("Text")))
                                                      , new XElement("MultilingualTextItem", new XAttribute("ID", "A1"), new XAttribute("CompositionName", "Items")
                                                          , new XElement("AttributeList"
                                                              , new XElement("Culture", "hu-HU")
                                                              , new XElement("Text")))
                                             )
                                            )
                                       )
                                )
                             )
                    ).Save("C:/Temp/DBThemePlate.xml");
        }

        /// <summary>
        /// Populates the current empty data base with the values in the List of variables
        /// </summary>
        /// <param name="variables">List that will be used to populate the data base</param>
        /// <param name="DBtype">Type of data base it will make</param>
        /// <param name="SavePath"></param>
        public static void PopulateDB(List<Variable> variables, string DBtype, string SavePath)
        {
            string currentSK = "";
            string currentSC1 = "";
            string currentDT1 = "";
            string currentSF1 = "";
            string currentSTA = "";
            string currentString = "";
            int RemotePanelsCount = GetQntRemotePanels();
            string arbeitsgruppe = DBMaker.EngValues[0].Arbeitsgruppe_ARG;
            Worksheet xlWorksheet = null;

            if (DBtype == "SPS")
            {
                SavePath = Path.Combine(SavePath, "40_Betriebsarten", "DB-Anwender");

                List<List<string>> Lists = new List<List<string>>();
                int listCounter = 0;

                foreach (ReplaceActions action in DBMaker.SPSActions)
                {
                    List<string> List = new List<string>();
                    List.Add(action.ReplaceAction);
                    Lists.Add(List);
                }

                foreach (List<string> list in Lists)
                {
                    if (list[0] == "Change to PLC Number")
                    {
                        Lists[listCounter].Add(arbeitsgruppe);
                    }

                    if (list[0] == "MaxPLC")
                    {
                        int userCounter = 0;
                        foreach (UserConfig userConfig in DBMaker.UserConfigs)
                        {
                            if (userConfig.Name == "Max Arbeitsgruppe [ARG]")
                            {
                                Lists[listCounter].Add(DBMaker.UserConfigs[userCounter].Value.ToString());
                            }
                            userCounter += 1;
                        }
                    }
                    listCounter += 1;
                }

                foreach (PLC_Tag tag in DBMaker.PLC_Tags)
                {
                    listCounter = 0;
                    if (tag.Comment == "Not-Halt" && char.IsLetter(tag.Name[arbeitsgruppe.Length]) || tag.Comment == "Not-Halt Comfort Panel" && char.IsLetter(tag.Name[arbeitsgruppe.Length]) || tag.Comment == "Not-Halt KTP Mobile" && char.IsLetter(tag.Name[arbeitsgruppe.Length]))
                    {
                        foreach (List<string> list in Lists)
                        {
                            if (list[0] == "PLC E-Stops")
                            {
                                Lists[listCounter].Add(tag.Name.Substring(1));
                            }
                            listCounter += 1;
                        }
                    }
                }

                XmlNamespaceManager oManager = new XmlNamespaceManager(new NameTable());
                oManager.AddNamespace("xxx", "http://www.siemens.com/automation/Openness/SW/Interface/v3");
                XDocument xmlDocBD = XDocument.Load("C:/Temp/DBThemePlate.xml");
                var elementName = xmlDocBD.XPathSelectElement("/Document/SW.Blocks.GlobalDB[@ID='0']/AttributeList/Name", oManager);
                elementName.Value = arbeitsgruppe;
                xmlDocBD.Save(SavePath + "/" + arbeitsgruppe + ".xml");
                DBMaker.BlocksCreated.Add(arbeitsgruppe);

                // Get Beschreibung from Schnittstelle
                Excel.Application xlApp = new Excel.Application();
                Workbook xlWorkbook = xlApp.Workbooks.Open(DBMaker.SchnittstelleListPath);
                List<string> beschreibungs = new List<string>();
                foreach (Worksheet sheet in xlWorkbook.Worksheets)
                {
                    if (sheet.Name.Length >= 6 && ExcelManager.IsDigitsOnly(sheet.Name.Substring(0, 6)) && sheet.Name[6] == 'R' && sheet.Name.Substring(0, 6) != "000000")
                    {
                        string typ1 = sheet.Cells[4, 3]?.Value?.ToString() ?? "";
                        string typ2 = sheet.Cells[5, 3]?.Value?.ToString() ?? "";
                        string typ3 = sheet.Cells[6, 3]?.Value?.ToString() ?? "";
                        string typ4 = sheet.Cells[7, 3]?.Value?.ToString() ?? "";

                        if (typ1 == "" || typ2 == "" || typ3 == "" || typ4 == "") continue;

                        if (beschreibungs.Contains(typ1) || beschreibungs.Contains(typ2) || beschreibungs.Contains(typ3) || beschreibungs.Contains(typ4)) continue;

                        beschreibungs.Add(typ1);
                        beschreibungs.Add(typ2);
                        beschreibungs.Add(typ3);
                        beschreibungs.Add(typ4);
                    }
                }
                xlWorkbook.Close(0);
                xlApp.Quit();

                List<Variable> typs = new List<Variable>();
                Variable vStruct = new Variable()
                {
                    Name = "Ist_StZ",
                    Type = "Struct",
                    Comment = "FB_Statistik_Typ gemessene Stueckzahl"
                };
                foreach (string typ in beschreibungs)
                {
                    Variable v = new Variable
                    {
                        Name = typ,
                        Type = "Int",
                        Comment = typ
                    };
                    typs.Add(v);
                }

                if (typs.Any())
                    CreateStruct(SavePath + "/" + arbeitsgruppe + ".xml", vStruct, typs);

                foreach (Variable v in variables)
                {
                    int cross = 0;
                    CreateTag(SavePath + "/" + arbeitsgruppe + ".xml", v);

                    //Per Remote Panels
                    foreach (ReplaceActions action in DBMaker.SPSActions.Where(act => v.Name.Contains(act.ToBeReplace) && act.ReplaceAction == "Per Remote Panels"))
                    {
                        for (int i = 1; i <= RemotePanelsCount; i++)
                        {
                            ReplacePs(SavePath + "/" + arbeitsgruppe + currentSK + ".xml", action.ToBeReplace, i.ToString());
                            if (i != RemotePanelsCount) // Only create new Tag if there is 1 more iteration
                                CreateTag(SavePath + "/" + arbeitsgruppe + currentSK + ".xml", v);
                        } 
                    }

                    //PLC E-Stops
                    foreach (ReplaceActions action in DBMaker.SPSActions.Where(act => v.Name.Contains(act.ToBeReplace)))
                    {
                        if (action.ReplaceAction == "PLC E-Stops" && cross == 0)
                        {
                            int counter = 1;
                            foreach (List<string> list in Lists)
                            {
                                if (list[0] == "PLC E-Stops")
                                {
                                    foreach (string s in list)
                                    {
                                        foreach (PLC_Tag tag in DBMaker.PLC_Tags)
                                        {
                                            if (counter == 1)
                                            {
                                                if (tag.Name == arbeitsgruppe + list[counter])
                                                {
                                                    ReplacePs(SavePath + "/" + arbeitsgruppe + currentSK + ".xml", "<MultiLanguageText Lang=\"de-DE\"></MultiLanguageText>", "<MultiLanguageText Lang=\"de-DE\">" + tag.Comment + "</MultiLanguageText>");
                                                    ReplacePs(SavePath + "/" + arbeitsgruppe + currentSK + ".xml", "<MultiLanguageText Lang=\"sk-SK\"></MultiLanguageText>", "<MultiLanguageText Lang=\"sk-SK\">" + tag.Comment + "</MultiLanguageText>");
                                                    ReplacePs(SavePath + "/" + arbeitsgruppe + ".xml", action.ToBeReplace, list[counter]);
                                                }
                                            }
                                            else if (list.Count() > counter && counter > 1)
                                            {
                                                if (tag.Name == arbeitsgruppe + list[counter])
                                                {
                                                    CreateTag(SavePath + "/" + arbeitsgruppe + ".xml", v);
                                                    ReplacePs(SavePath + "/" + arbeitsgruppe + currentSK + ".xml", "<MultiLanguageText Lang=\"de-DE\"></MultiLanguageText>", "<MultiLanguageText Lang=\"de-DE\">" + tag.Comment + "</MultiLanguageText>");
                                                    ReplacePs(SavePath + "/" + arbeitsgruppe + currentSK + ".xml", "<MultiLanguageText Lang=\"sk-SK\"></MultiLanguageText>", "<MultiLanguageText Lang=\"sk-SK\">" + tag.Comment + "</MultiLanguageText>");
                                                    ReplacePs(SavePath + "/" + arbeitsgruppe + ".xml", action.ToBeReplace, list[counter]);
                                                }
                                            }
                                        }
                                        counter += 1;
                                    }
                                }
                            }
                        }
                    }
                }

                //Change to Plc Number and Max Plc
                foreach (ReplaceActions action in DBMaker.SPSActions.Where(act => act.ReplaceAction == "MaxPLC" || act.ReplaceAction == "Change to PLC Number"))
                {
                    if (action.ReplaceAction == "MaxPLC")
                    {
                        listCounter = 0;
                        foreach (List<string> list in Lists)
                        {
                            if (list[0] == "MaxPLC")
                            {
                                ReplacePs(SavePath + "/" + arbeitsgruppe + ".xml", action.ToBeReplace, list[1]);
                            }
                            listCounter += 1;
                        }
                    }

                    if (action.ReplaceAction == "Change to PLC Number")
                    {
                        listCounter = 0;
                        foreach (List<string> list in Lists)
                        {
                            if (list[0] == "Change to PLC Number")
                            {
                                ReplacePs(SavePath + "/" + arbeitsgruppe + ".xml", action.ToBeReplace, list[1]);
                            }
                            listCounter += 1;
                        }
                    }
                }
            }

            if (DBtype == "Schutzkreis")
            {
                SavePath = Path.Combine(SavePath, "40_Betriebsarten", "DB-Anwender");
                string Arbeitsgruppe = arbeitsgruppe;
                foreach (EngAssist eng in DBMaker.EngValues)
                {
                    if (currentSK != eng.Schutzkreis_SK)
                    {
                        currentSK = eng.Schutzkreis_SK;
                        XmlNamespaceManager oManager = new XmlNamespaceManager(new NameTable());
                        oManager.AddNamespace("xxx", "http://www.siemens.com/automation/Openness/SW/Interface/v3");
                        XDocument xmlDocBD = XDocument.Load("C:/Temp/DBThemePlate.xml");
                        var elementName = xmlDocBD.XPathSelectElement("/Document/SW.Blocks.GlobalDB[@ID='0']/AttributeList/Name", oManager);
                        elementName.Value = arbeitsgruppe + currentSK;
                        xmlDocBD.Save(SavePath + "/" + arbeitsgruppe + currentSK + ".xml");
                        DBMaker.BlocksCreated.Add(arbeitsgruppe + currentSK);
                        List<string> usedTypes = new List<string>();

                        foreach (Variable v in variables)
                        {
                            if (!v.Name.Contains("%"))
                                CreateTag(SavePath + "/" + arbeitsgruppe + currentSK + ".xml", v);
                            else
                            {
                                foreach (ReplaceActions action in DBMaker.SCHActions.Where(a => v.Name.Contains(a.ToBeReplace)))
                                {
                                    if (action.ReplaceAction == "Change to Safety door" || action.ReplaceAction == "Change to Safety door E-Stop")
                                    {
                                        foreach (PLC_Tag tag in DBMaker.PLC_Tags.Where(tag => tag.Comment == "Schutzgitter geschlossen" || tag.Comment == "Not-Halt Schutztür"))
                                        {
                                            char lastChar = tag.Name.Last();
                                            int number = -1;

                                            if (char.IsDigit(lastChar))
                                                int.TryParse(lastChar.ToString(), out number);
                                            else 
                                                continue;

                                            string type = "SFN" + number;   // E-Stop
                                            if (tag.Comment == "Schutzgitter geschlossen")
                                                type = "BGS" + number;      // Normal

                                            if (tag.Name.Contains(arbeitsgruppe + currentSK) && !usedTypes.Contains(type))
                                            {
                                                CreateTag(SavePath + "/" + arbeitsgruppe + currentSK + ".xml", v);
                                                ReplacePs(SavePath + "/" + arbeitsgruppe + currentSK + ".xml", "<MultiLanguageText Lang=\"de-DE\"></MultiLanguageText>", "<MultiLanguageText Lang=\"de-DE\">" + tag.Comment + "</MultiLanguageText>");
                                                ReplacePs(SavePath + "/" + arbeitsgruppe + currentSK + ".xml", "<MultiLanguageText Lang=\"sk-SK\"></MultiLanguageText>", "<MultiLanguageText Lang=\"sk-SK\">" + tag.Comment + "</MultiLanguageText>");
                                                ReplacePs(SavePath + "/" + arbeitsgruppe + currentSK + ".xml", action.ToBeReplace, type);
                                                usedTypes.Add(type);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (DBtype == "Safety_Standart" || DBtype == "Standart_Safety")
            {
                string name = "";
                string _name = "";

                if (DBtype == "Safety_Standart")
                {
                    name = "F>_";
                    _name = "F_";
                }
                else
                {
                    name = ">F";
                    _name = "_F";
                }

                SavePath = Path.Combine(SavePath, "2_Safety", "DB-Anwender");

                bool firstPLC = false;
                bool lastPLC = false;

                if (arbeitsgruppe == "1")
                {
                    firstPLC = true;
                }

                var checkLastPLC = DBMaker.UserConfigs.Where(conf => arbeitsgruppe == conf.Value 
                                                                     && conf.Name == "Max Arbeitsgruppe [ARG]");
                
                if (checkLastPLC != null) 
                    lastPLC = true;

                List<ReplaceActions> BaseActions = new List<ReplaceActions>();
                List<Variable> PLCList = new List<Variable>();
                List<Variable> SKList = new List<Variable>();
                List<Variable> DTList = new List<Variable>();
                List<Variable> RList = new List<Variable>();
                List<Variable> K100List = new List<Variable>();
                List<Variable> SF1List = new List<Variable>();
                List<Variable> SC1List = new List<Variable>();
                List<Variable> STAList = new List<Variable>();
                List<Variable> RemoteList = new List<Variable>();

                foreach (ReplaceActions action in DBMaker.SAFActions)
                {
                    if (!action.ReplaceAction.Contains("Per"))
                    {
                        BaseActions.Add(action);
                    }
                }

                XmlNamespaceManager oManager = new XmlNamespaceManager(new NameTable());
                oManager.AddNamespace("xxx", "http://www.siemens.com/automation/Openness/SW/Interface/v3");
                XDocument xmlDocBD = XDocument.Load("C:/Temp/DBThemePlate.xml");
                var elementName = xmlDocBD.XPathSelectElement("/Document/SW.Blocks.GlobalDB[@ID='0']/AttributeList/Name", oManager);
                elementName.Value = name;
                xmlDocBD.Save(SavePath + "/" + _name + ".xml");
                DBMaker.BlocksCreated.Add(_name);

                foreach (Variable v in variables)
                {
                    switch (v.Action)
                    {
                        case "Per PLC":
                            PLCList.Add(v);
                            break;
                        case "For each SK":
                            SKList.Add(v);
                            break;
                        case "######DT1AE1TAF1K100":
                            DTList.Add(v);
                            break;
                        case "######R01_NHL":
                            RList.Add(v);
                            break;
                        case "*K100":
                            K100List.Add(v);
                            break;
                        case "######SF1K16A":
                            SF1List.Add(v);
                            break;
                        case "######SC1":
                            SC1List.Add(v);
                            break;
                        case "Per Station":
                            STAList.Add(v);
                            break;
                        case "Per Remote Panels":
                            RemoteList.Add(v);
                            break;
                    }
                }

                //PLCs
                foreach (Variable v in PLCList)
                {
                    CreateTag(SavePath + "/" + _name + ".xml", v);

                    foreach (ReplaceActions action in DBMaker.SAFActions)
                    {
                        if (action.ReplaceAction == "Per Remote Panels" && v.Name.Contains(action.ToBeReplace))
                        {
                            for (int i = 1; i <= RemotePanelsCount; i++)
                            {
                                ReplacePs(SavePath + "/" + _name + ".xml", action.ToBeReplace, i.ToString());
                                if(i != RemotePanelsCount) // Only create new Tag if there is 1 more iteration
                                    CreateTag(SavePath + "/" + _name + ".xml", v);
                            }
                        }

                        if (action.ReplaceAction == "Change to PLC Number" && DBtype == "Safety_Standart")
                        {
                            ReplacePs(SavePath + "/" + _name + ".xml", action.ToBeReplace, arbeitsgruppe);
                        }
                    }
                }

                //SKs && DTs && SC1s && STAs
                foreach (EngAssist eng in DBMaker.EngValues)
                {
                    if (eng.Schutzkreis_SK != currentSK)
                    {
                        currentSK = eng.Schutzkreis_SK;

                        foreach (Variable v in SKList)
                        {
                            CreateTag(SavePath + "/" + _name + ".xml", v);

                            foreach (ReplaceActions action in DBMaker.SAFActions)
                            {
                                if (action.ReplaceAction == "Per Remote Panels" && v.Name.Contains(action.ToBeReplace))
                                {
                                    for (int i = 1; i <= RemotePanelsCount; i++)
                                    {
                                        ReplacePs(SavePath + "/" + _name + ".xml", action.ToBeReplace, i.ToString());
                                        if (i != RemotePanelsCount) // Only create new Tag if there is 1 more iteration
                                            CreateTag(SavePath + "/" + _name + ".xml", v);
                                    }
                                }

                                if (action.ReplaceAction == "Per SK")
                                {
                                    ReplacePs(SavePath + "/" + _name + ".xml", action.ToBeReplace, arbeitsgruppe + currentSK);
                                }
                            }
                        }
                    }

                    if (eng.Station.Contains("DT1"))
                    {
                        if (currentDT1 != eng.Arbeitsgruppe_ARG + eng.Schutzkreis_SK + eng.Station.Substring(0, 4))
                        {
                            currentDT1 = eng.Arbeitsgruppe_ARG + eng.Schutzkreis_SK + eng.Station.Substring(0, 4);

                            foreach (Variable v in DTList)
                            {
                                CreateTag(SavePath + "/" + _name + ".xml", v);

                                foreach (ReplaceActions action in DBMaker.SAFActions)
                                {
                                    if (action.ReplaceAction == "Per Remote Panels" && v.Name.Contains(action.ToBeReplace))
                                    {
                                        for (int i = 1; i <= RemotePanelsCount; i++)
                                        {
                                            ReplacePs(SavePath + "/" + _name + ".xml", action.ToBeReplace, i.ToString());
                                            if (i != RemotePanelsCount) // Only create new Tag if there is 1 more iteration
                                                CreateTag(SavePath + "/" + _name + ".xml", v);
                                        }
                                    }

                                    if (action.ReplaceAction == "Per ######DT1AE1TAF1K100")
                                    {
                                        ReplacePs(SavePath + "/" + _name + ".xml", action.ToBeReplace, currentDT1);
                                    }
                                }
                            }
                        }
                    }

                    if (eng.Station.Contains("SC1"))
                    {
                        if (currentSC1 != eng.Arbeitsgruppe_ARG + eng.Schutzkreis_SK + eng.Station.Substring(0, 4))
                        {
                            currentSC1 = eng.Arbeitsgruppe_ARG + eng.Schutzkreis_SK + eng.Station.Substring(0, 4);

                            foreach (Variable v in SC1List)
                            {
                                CreateTag(SavePath + "/" + _name + ".xml", v);

                                foreach (ReplaceActions action in DBMaker.SAFActions)
                                {
                                    if (action.ReplaceAction == "Per Remote Panels" && v.Name.Contains(action.ToBeReplace))
                                    {
                                        for (int i = 1; i <= RemotePanelsCount; i++)
                                        {
                                            ReplacePs(SavePath + "/" + _name + ".xml", action.ToBeReplace, i.ToString());
                                            if (i != RemotePanelsCount) // Only create new Tag if there is 1 more iteration
                                                CreateTag(SavePath + "/" + _name + ".xml", v);
                                        }
                                    }

                                    if (action.ReplaceAction == "Per ######SC1*")
                                    {
                                        ReplacePs(SavePath + "/" + _name + ".xml", action.ToBeReplace, currentSC1);
                                    }
                                }
                            }
                        }
                    }

                    if (currentSTA != eng.Station.Substring(0, 4))
                    {
                        if (DBtype == "Standart_Safety")
                            currentSTA = eng.Station.Substring(0, 4);

                        foreach (Variable v in STAList)
                        {
                            CreateTag(SavePath + "/" + _name + ".xml", v);

                            foreach (ReplaceActions action in DBMaker.SAFActions)
                            {
                                if (action.ReplaceAction == "Per Remote Panels" && v.Name.Contains(action.ToBeReplace))
                                {
                                    for (int i = 1; i <= RemotePanelsCount; i++)
                                    {
                                        ReplacePs(SavePath + "/" + _name + ".xml", action.ToBeReplace, i.ToString());
                                        if (i != RemotePanelsCount) // Only create new Tag if there is 1 more iteration
                                            CreateTag(SavePath + "/" + _name + ".xml", v);
                                    }
                                }

                                if (action.ReplaceAction == "Per Station")
                                {
                                    ReplacePs(SavePath + "/" + _name + ".xml", action.ToBeReplace, eng.Arbeitsgruppe_ARG + eng.Schutzkreis_SK + currentSTA);
                                }
                            }
                        }
                    }
                }

                //Robot
                Excel.Application xlApp = new Excel.Application();
                Workbook xlWorkbook = xlApp.Workbooks.Open(DBMaker.SchnittstelleListPath);
                foreach (Worksheet sheet in xlWorkbook.Worksheets)
                {
                    if (sheet.Name.Length >= 6 && ExcelManager.IsDigitsOnly(sheet.Name.Substring(0, 6)) && sheet.Name[6] == 'R' && sheet.Name.Substring(0, 6) != "000000")
                        {
                            foreach (Variable v in RList)
                            {
                                CreateTag(SavePath + "/" + _name + ".xml", v);

                                foreach (ReplaceActions action in DBMaker.SAFActions)
                                {
                                    if (action.ReplaceAction == "Per Remote Panels" && v.Name.Contains(action.ToBeReplace))
                                    {
                                        for (int i = 1; i <= RemotePanelsCount; i++)
                                        {
                                            ReplacePs(SavePath + "/" + _name + ".xml", action.ToBeReplace, i.ToString());
                                            if (i != RemotePanelsCount) // Only create new Tag if there is 1 more iteration
                                                CreateTag(SavePath + "/" + _name + ".xml", v);
                                        }
                                    }

                                    if (action.ReplaceAction == "Per Robot ######R##_NHL")
                                    {
                                        ReplacePs(SavePath + "/" + _name + ".xml", action.ToBeReplace, sheet.Name);
                                    }
                                }
                            }
                        }
                }
                xlWorkbook.Close(0);
                xlApp.Quit();

                //K100s && SF1s
                foreach (PLC_Tag tag in DBMaker.PLC_Tags)
                {
                    if (tag.Name.Contains("K100") && !tag.Name.Contains("AE1TAF1") && !tag.Name.Contains("SC"))
                    {
                        foreach (Variable v in K100List)
                        {
                            CreateTag(SavePath + "/" + _name + ".xml", v);

                            foreach (ReplaceActions action in DBMaker.SAFActions)
                            {
                                if (action.ReplaceAction == "Per Remote Panels" && v.Name.Contains(action.ToBeReplace))
                                {
                                    for (int i = 1; i <= RemotePanelsCount; i++)
                                    {
                                        ReplacePs(SavePath + "/" + _name + ".xml", action.ToBeReplace, i.ToString());
                                        if (i != RemotePanelsCount) // Only create new Tag if there is 1 more iteration
                                            CreateTag(SavePath + "/" + _name + ".xml", v);
                                    }
                                }

                                if (action.ReplaceAction == "Per K100")
                                {
                                    ReplacePs(SavePath + "/" + _name + ".xml", action.ToBeReplace, tag.Name.Substring(0, tag.Name.Count() - 4));
                                }
                            }
                        }
                    }

                    if (tag.Name.Contains("SF1K16A"))
                    {
                        if (currentSF1 != tag.Name.Substring(0, 6))
                        {
                            currentSF1 = tag.Name.Substring(0, 6);

                            foreach (Variable v in SF1List)
                            {
                                CreateTag(SavePath + "/" + _name + ".xml", v);

                                foreach (ReplaceActions action in DBMaker.SAFActions)
                                {
                                    if (action.ReplaceAction == "Per Remote Panels" && v.Name.Contains(action.ToBeReplace))
                                    {
                                        for (int i = 1; i <= RemotePanelsCount; i++)
                                        {
                                            ReplacePs(SavePath + "/" + _name + ".xml", action.ToBeReplace, i.ToString());
                                            if (i != RemotePanelsCount) // Only create new Tag if there is 1 more iteration
                                                CreateTag(SavePath + "/" + _name + ".xml", v);
                                        }
                                    }

                                    if (action.ReplaceAction == "Per ######SF1K16A")
                                    {
                                        ReplacePs(SavePath + "/" + _name + ".xml", action.ToBeReplace, currentSF1);
                                    }
                                }
                            }
                        }
                    }
                }

                //Remotes
                foreach (Variable v in RemoteList)
                {
                    CreateTag(SavePath + "/" + _name + ".xml", v);
                }

                //Static Value Changes
                foreach (ReplaceActions action in BaseActions)
                {
                    if (action.ReplaceAction == "Change to PLC Number")
                    {
                        ReplacePs(SavePath + "/" + _name + ".xml", action.ToBeReplace, arbeitsgruppe);
                    }

                    if (action.ReplaceAction == "Nº of PLCs on this line (SPS Page)")
                    {
                        foreach (UserConfig config in DBMaker.UserConfigs)
                        {
                            if (config.Name == "Max Arbeitsgruppe [ARG]")
                            {
                                ReplacePs(SavePath + "/" + _name + ".xml", action.ToBeReplace, config.Value);
                            }
                        }
                    }

                    if (action.ReplaceAction == "Previous PLC" && !firstPLC)
                    {
                        ReplacePs(SavePath + "/" + _name + ".xml", action.ToBeReplace, (Convert.ToInt32(arbeitsgruppe) - 1).ToString());
                    }

                    if (action.ReplaceAction == "Next PLC" && !lastPLC)
                    {
                        ReplacePs(SavePath + "/" + _name + ".xml", action.ToBeReplace, (Convert.ToInt32(arbeitsgruppe) + 1).ToString());
                    }

                    if (action.ReplaceAction == "Central PLC (Safety) (SPS Page)")
                    {
                        foreach (UserConfig config in DBMaker.UserConfigs)
                        {
                            if (config.Name == "Central SPS (Safety) [ARG]")
                            {
                                ReplacePs(SavePath + "/" + _name + ".xml", action.ToBeReplace, config.Value);
                            }
                        }
                    }
                }
            }

            if (DBtype == "ARG")
            {
                SavePath = Path.Combine(SavePath, "100_ARG_Typ_Strg", "DB-Anwender");

                string path = SavePath + "/" + "Name ARG_Typ_Strg_DB.xml";
                List<ReplaceActions> BaseActions = new List<ReplaceActions>();
                List<Variable> STAList = new List<Variable>();
                List<Variable> Other = new List<Variable>();

                foreach (ReplaceActions action in DBMaker.STAActions)
                {
                    if (!action.ReplaceAction.Contains("Per"))
                    {
                        BaseActions.Add(action);
                    }
                }

                foreach (Variable v in variables)
                {
                    if (v.Action == "PerStation")
                    {
                        STAList.Add(v);
                    }
                    else
                    {
                        Other.Add(v);
                    }
                }

                XmlNamespaceManager oManager = new XmlNamespaceManager(new NameTable());
                oManager.AddNamespace("xxx", "http://www.siemens.com/automation/Openness/SW/Interface/v3");
                XDocument xmlDocBD = XDocument.Load("C:/Temp/DBThemePlate.xml");
                var elementName = xmlDocBD.XPathSelectElement("/Document/SW.Blocks.GlobalDB[@ID='0']/AttributeList/Name", oManager);
                elementName.Value = "ARG_Typ-Strg_DB";
                xmlDocBD.Save(path);
                DBMaker.BlocksCreated.Add("Name ARG_Typ_Strg_DB");

                //STAs
                foreach (EngAssist eng in DBMaker.EngValues)
                {
                    if (currentSTA != eng.Arbeitsgruppe_ARG + eng.Schutzkreis_SK + eng.Station)
                    {
                        currentSTA = eng.Arbeitsgruppe_ARG + eng.Schutzkreis_SK + eng.Station;
                        foreach (Variable v in STAList)
                        {
                            CreateTag(path, v);

                            foreach (ReplaceActions action in DBMaker.STAActions)
                            {
                                if (action.ReplaceAction == "Per Station")
                                {
                                    ReplacePs(path, action.ToBeReplace, currentSTA);
                                }
                            }
                        }
                    }
                }

                //others
                foreach (Variable v in Other)
                {
                    bool Empty = false;
                    foreach (ReplaceActions action in BaseActions)
                    {
                        if (v.Name.Contains(action.ToBeReplace) && action.ReplaceAction == "Nr Types")
                        {
                            foreach (UserConfig config in DBMaker.UserConfigs)
                            {
                                if (config.Name == "Nr Types" && config.Value == "")
                                {
                                    Empty = true;
                                }
                            }
                        }

                        if (v.Name.Contains(action.ToBeReplace) && action.ReplaceAction == "Nr BAND")
                        {
                            foreach (UserConfig config in DBMaker.UserConfigs)
                            {
                                if (config.Name == "Nr Bands" && config.Value == "")
                                {
                                    Empty = true;
                                }
                            }
                        }

                        if (v.Name.Contains(action.ToBeReplace) && action.ReplaceAction == "TURMSP")
                        {
                            foreach (UserConfig config in DBMaker.UserConfigs)
                            {
                                if (config.Name == "Nr TeilSpeicher" && config.Value == "")
                                {
                                    Empty = true;
                                }
                            }
                        }
                    }

                    if (Empty) continue;

                    CreateTag(path, v);
                    foreach (ReplaceActions action in BaseActions)
                    {
                        if (v.Name.Contains(action.ToBeReplace) && action.ReplaceAction == "Nr Types")
                        {
                            foreach (UserConfig config in DBMaker.UserConfigs)
                            {
                                if (config.Name == "Nr Types" && config.Value != "")
                                {
                                    for (int z = 1; z <= Int32.Parse(config.Value); z++)
                                    {
                                        if (z == 1)
                                        {
                                            foreach (ReplaceActions actionX in BaseActions)
                                            {
                                                if (v.Name.Contains(actionX.ToBeReplace) && actionX.ReplaceAction == "Nr BAND")
                                                {
                                                    foreach (UserConfig configX in DBMaker.UserConfigs)
                                                    {
                                                        if (configX.Name == "Nr Bands" && configX.Value != "")
                                                        {
                                                            for (int zX = 1; zX <= Int32.Parse(configX.Value); zX++)
                                                            {
                                                                if (zX == 1)
                                                                {
                                                                    ReplacePs(path, actionX.ToBeReplace, "0" + zX);
                                                                }

                                                                else
                                                                {
                                                                    if (zX.ToString().Length < 2)
                                                                    {
                                                                        CreateTag(path, v);
                                                                        ReplacePs(path, actionX.ToBeReplace, "0" + zX.ToString());
                                                                    }

                                                                    else
                                                                    {
                                                                        CreateTag(path, v);
                                                                        ReplacePs(path, actionX.ToBeReplace, zX.ToString());
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                if (v.Name.Contains(actionX.ToBeReplace) && actionX.ReplaceAction == "TURMSP")
                                                {
                                                    foreach (UserConfig configX in DBMaker.UserConfigs)
                                                    {
                                                        if (configX.Name == "Nr TeilSpeicher" && configX.Value != "")
                                                        {
                                                            for (int zX = 1; zX <= Int32.Parse(configX.Value); zX++)
                                                            {
                                                                if (zX == 1)
                                                                {
                                                                    ReplacePs(path, actionX.ToBeReplace, "0" + zX);
                                                                }

                                                                else
                                                                {
                                                                    if (zX.ToString().Length < 2)
                                                                    {
                                                                        CreateTag(path, v);
                                                                        ReplacePs(path, actionX.ToBeReplace, "0" + zX.ToString());
                                                                    }

                                                                    else
                                                                    {
                                                                        CreateTag(path, v);
                                                                        ReplacePs(path, actionX.ToBeReplace, zX.ToString());
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                else
                                                {
                                                    if (z.ToString().Length < 2)
                                                    {
                                                        ReplacePs(path, action.ToBeReplace, "0" + z.ToString());
                                                    }

                                                    else
                                                    {
                                                        ReplacePs(path, action.ToBeReplace, z.ToString());
                                                    }
                                                }
                                            }
                                        }

                                        else
                                        {
                                            int counterFound = 0;

                                            if (z.ToString().Length < 2)
                                            {
                                                foreach (ReplaceActions actionX in BaseActions)
                                                {
                                                    if (v.Name.Contains(actionX.ToBeReplace) && actionX.ReplaceAction == "TURMSP")
                                                    {
                                                        foreach (UserConfig configX in DBMaker.UserConfigs)
                                                        {
                                                            if (configX.Name == "Nr TeilSpeicher" && configX.Value != "")
                                                            {
                                                                counterFound = 1;
                                                                for (int zX = 1; zX <= Int32.Parse(configX.Value); zX++)
                                                                {
                                                                    if (zX == 1)
                                                                    {
                                                                        CreateTag(path, v);
                                                                        ReplacePs(path, actionX.ToBeReplace, "0" + zX);
                                                                    }

                                                                    else
                                                                    {
                                                                        if (zX.ToString().Length < 2)
                                                                        {
                                                                            CreateTag(path, v);
                                                                            ReplacePs(path, actionX.ToBeReplace, "0" + zX.ToString());
                                                                            ReplacePs(path, action.ToBeReplace, "0" + z.ToString());
                                                                        }

                                                                        else
                                                                        {
                                                                            CreateTag(path, v);
                                                                            ReplacePs(path, actionX.ToBeReplace, zX.ToString());
                                                                            ReplacePs(path, action.ToBeReplace, z.ToString());
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                if (counterFound == 0)
                                                {
                                                    CreateTag(path, v);
                                                    ReplacePs(path, action.ToBeReplace, "0" + z.ToString());
                                                }
                                            }

                                            else
                                            {
                                                foreach (ReplaceActions actionX in BaseActions)
                                                {
                                                    if (v.Name.Contains(actionX.ToBeReplace) && actionX.ReplaceAction == "TURMSP")
                                                    {
                                                        foreach (UserConfig configX in DBMaker.UserConfigs)
                                                        {
                                                            if (configX.Name == "Nr TeilSpeicher" && configX.Value != "")
                                                            {
                                                                counterFound = 1;
                                                                for (int zX = 1; zX <= Int32.Parse(configX.Value); zX++)
                                                                {
                                                                    if (zX == 1)
                                                                    {
                                                                        ReplacePs(path, actionX.ToBeReplace, "0" + zX);
                                                                        ReplacePs(path, action.ToBeReplace, z.ToString());
                                                                    }

                                                                    else
                                                                    {
                                                                        if (zX.ToString().Length < 2)
                                                                        {
                                                                            CreateTag(path, v);
                                                                            ReplacePs(path, actionX.ToBeReplace, "0" + zX.ToString());
                                                                            ReplacePs(path, action.ToBeReplace, z.ToString());
                                                                        }

                                                                        else
                                                                        {
                                                                            CreateTag(path, v);
                                                                            ReplacePs(path, actionX.ToBeReplace, zX.ToString());
                                                                            ReplacePs(path, action.ToBeReplace, z.ToString());
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                if (counterFound == 0)
                                                {
                                                    CreateTag(path, v);
                                                    ReplacePs(path, action.ToBeReplace, z.ToString());
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (v.Name.Contains(action.ToBeReplace) && action.ReplaceAction == "Nr BAND")
                        {
                            int typeCounter = 0;

                            foreach (ReplaceActions actionX in BaseActions)
                            {
                                if (v.Name.Contains(actionX.ToBeReplace) && actionX.ReplaceAction == "Nr Types")
                                {
                                    typeCounter = 1;
                                }
                            }

                            if (typeCounter == 0)
                            {
                                foreach (UserConfig configX in DBMaker.UserConfigs)
                                {
                                    if (configX.Name == "Nr Bands" && configX.Value != "")
                                    {
                                        for (int z = 1; z <= Int32.Parse(configX.Value); z++)
                                        {
                                            if (z == 1)
                                            {
                                                ReplacePs(path, action.ToBeReplace, "0" + z);
                                            }

                                            else
                                            {
                                                if (z.ToString().Length < 2)
                                                {
                                                    CreateTag(path, v);
                                                    ReplacePs(path, action.ToBeReplace, "0" + z.ToString());
                                                }

                                                else
                                                {
                                                    CreateTag(path, v);
                                                    ReplacePs(path, action.ToBeReplace, z.ToString());
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (v.Name.Contains(action.ToBeReplace) && action.ReplaceAction == "TURMSP")
                        {
                            int typeCounter = 0;

                            foreach (ReplaceActions actionX in BaseActions)
                            {
                                if (v.Name.Contains(actionX.ToBeReplace) && actionX.ReplaceAction == "Nr Types")
                                {
                                    typeCounter = 1;
                                }
                            }

                            if (typeCounter == 0)
                            {
                                foreach (UserConfig configX in DBMaker.UserConfigs)
                                {
                                    if (configX.Name == "Nr TeilSpeicher" && configX.Value != "")
                                    {
                                        for (int z = 1; z <= Int32.Parse(configX.Value); z++)
                                        {
                                            if (z == 1)
                                            {
                                                ReplacePs(path, action.ToBeReplace, "0" + z);
                                            }

                                            else
                                            {
                                                if (z.ToString().Length < 2)
                                                {
                                                    CreateTag(path, v);
                                                    ReplacePs(path, action.ToBeReplace, "0" + z.ToString());
                                                }

                                                else
                                                {
                                                    CreateTag(path, v);
                                                    ReplacePs(path, action.ToBeReplace, z.ToString());
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (DBtype == "Station1")
            {
                SavePath = Path.Combine(SavePath, "50_Stationen", "DB-Anwender");

                foreach (EngAssist eng in DBMaker.EngValues)
                {
                    bool hasLettersInStation = true;

                    if (eng.Erw_Stationsbez_SBZ == "")
                    {
                        currentString = eng.Arbeitsgruppe_ARG + eng.Schutzkreis_SK + eng.Station;
                        if(ExcelManager.IsDigitsOnly(eng.Station.Substring(eng.Station.Length - 3)))
                            hasLettersInStation = false;
                    }
                    else
                        currentString = eng.Arbeitsgruppe_ARG + eng.Schutzkreis_SK + eng.Station + eng.Erw_Stationsbez_SBZ;
                    

                    if (currentSTA != currentString)
                    {
                        currentSTA = currentString;

                        string path = SavePath + "/" + currentSTA + ".xml";

                        XmlNamespaceManager oManager = new XmlNamespaceManager(new NameTable());
                        oManager.AddNamespace("xxx", "http://www.siemens.com/automation/Openness/SW/Interface/v3");
                        XDocument xmlDocBD = XDocument.Load("C:/Temp/DBThemePlate.xml");
                        var elementName = xmlDocBD.XPathSelectElement("/Document/SW.Blocks.GlobalDB[@ID='0']/AttributeList/Name", oManager);
                        elementName.Value = currentSTA;
                        xmlDocBD.Save(path);
                        DBMaker.BlocksCreated.Add(currentSTA);

                        foreach (Variable v in DBMaker.Variables)
                        {
                            if (!v.Name.Contains("%"))
                            {
                                // Stations without letters (SB, VR, V01, etc...) does not have KT
                                if (v.Name.Contains("KT") && !hasLettersInStation) continue;
                                CreateTag(path, v);
                            }
                            else
                            {
                                foreach (ReplaceActions action in DBMaker.STAActions.Where(a => v.Name.Contains(a.ToBeReplace)))
                                {
                                    //Per K100
                                    if (action.ReplaceAction == "PerK100")
                                    {
                                        if (hasLettersInStation)
                                        {
                                            foreach (PLC_Tag tag in DBMaker.PLC_Tags.Where(t => t.Name.Contains("K100")))
                                            {
                                                string tagName = tag.Name;
                                                if (tagName.Contains(currentSTA) && !tagName.Contains("MA"))
                                                {
                                                    CreateTag(path, v);
                                                    ReplacePs(path, action.ToBeReplace, tagName.Substring(tagName.Length - 7));
                                                }
                                            }
                                        }
                                    }

                                    //Per KKP01E
                                    if (action.ReplaceAction == "PerKKP01E")
                                    {
                                        if (hasLettersInStation)
                                        {
                                            List<int> digits = new List<int>();
                                            foreach (PLC_Tag tag in DBMaker.PLC_Tags.Where(t => t.Name.Contains("VI") && t.Name.Contains("KKP01E")))
                                            {
                                                string tagName = tag.Name;
                                                if (tagName.Contains(currentSTA) && !tagName.Contains("BER"))
                                                {
                                                    int pFrom = tagName.IndexOf("VI") + "VI".Length;
                                                    int pTo = tagName.LastIndexOf("KKP01E");
                                                    char digit = tagName.Substring(pFrom, pTo - pFrom)[0]; // Get char between "VI" and "KKP01E"

                                                    if (char.IsDigit(digit) && !digits.Contains(digit)) // Check if char is a number/digit
                                                    {
                                                        digits.Add(digit);

                                                        CreateTag(path, v); // Creates XML tag
                                                        ReplacePs(path, action.ToBeReplace, digit.ToString()); // Replace "%2" with digit found
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    //Per Cylinder
                                    if (action.ReplaceAction == "PerCylinder")
                                    {
                                        List<string> cylindersUsed = new List<string>();
                                        foreach (string s in eng.Valves)
                                        {
                                            if (string.IsNullOrEmpty(s)) break;

                                            string cylinder;
                                            if (s.Contains("|"))
                                                cylinder = s.Substring(0, s.IndexOf('|')).Replace(" ", string.Empty); // Gets cylinder value before '|' char
                                            else
                                                cylinder = s;
                                            if (cylindersUsed.Contains(cylinder)) continue;
                                                
                                            cylindersUsed.Add(cylinder);
                                            CreateTag(path, v);
                                            ReplacePs(path, action.ToBeReplace, cylinder);
                                        }
                                    }

                                    //Per Part present
                                    if (action.ReplaceAction == "PerPartpresent")
                                    {
                                        List<string> partsUsed = new List<string>();
                                        foreach (string s in eng.Parts)
                                        {
                                            if (string.IsNullOrEmpty(s)) continue;

                                            string part;
                                            if (s.Contains("|"))
                                                part = s.Substring(0, s.IndexOf('|')).Replace(" ", string.Empty);  // Gets part value before '|' char
                                            else
                                                part = s;    

                                            if (partsUsed.Contains(part)) continue;

                                            partsUsed.Add(part);
                                            CreateTag(path, v);
                                            ReplacePs(path, action.ToBeReplace, part);
                                        }
                                    }

                                    bool Exists = true;
                                    //Steps Transitionsbedingung
                                    if (action.ReplaceAction == "StepsTransitionsbedingung")
                                    {
                                        List<string> steps = new List<string>();

                                        Excel.Application xlApp = new Excel.Application();
                                        Workbook xlWorkbook = xlApp.Workbooks.Open(DBMaker.SequenceListPath);

                                        try
                                        {
                                            xlWorksheet = xlWorkbook.Sheets["AS_" + eng.Arbeitsgruppe_ARG + eng.Schutzkreis_SK + eng.Station + eng.Erw_Stationsbez_SBZ];
                                        }
                                        catch (Exception)
                                        {
                                            Exists = false;
                                        }

                                        if (Exists)
                                        {
                                            object[,] Matriz = OpennessHelper.ExcelToMatrix(xlWorksheet);

                                            steps = ExcelManager.Steps(Matriz);

                                            foreach (string s in steps)
                                            {
                                                CreateTag(path, v);
                                                ReplacePs(path, action.ToBeReplace, s);
                                            }
                                        }

                                        xlWorkbook.Close(0);
                                        xlApp.Quit();
                                    }

                                    //Robot Frag
                                    if (action.ReplaceAction == "RobotFrgVer")
                                    {
                                        List<Frag> frags = new List<Frag>();
                                        if (hasLettersInStation)
                                        {
                                            Excel.Application xlApp = new Excel.Application();
                                            Workbook xlWorkbook = xlApp.Workbooks.Open(DBMaker.SchnittstelleListPath);

                                            foreach (Worksheet sheet in xlWorkbook.Worksheets)
                                            {
                                                if (sheet.Name.Count() > 6 && sheet.Name[6] == 'R' && ExcelManager.IsDigitsOnly(sheet.Name.Substring(0, 6)) && sheet.Name.Substring(0, 6) != "000000")
                                                {
                                                    object[,] AS_Matriz = OpennessHelper.ExcelToMatrix(sheet);
                                                    frags = ExcelManager.Frags(currentSTA, sheet.Name, AS_Matriz);
                                                    frags.Reverse();

                                                    foreach (Frag f in frags)
                                                    {
                                                        int number = 0;

                                                        // String after "f.Profil" underscore
                                                        var s = f.Profil.Substring(f.Profil.LastIndexOf('_') + 1);

                                                        if (char.IsDigit(s[2]))
                                                            number = int.Parse(s[2].ToString());

                                                        CreateTag(path, v);

                                                        ReplacePs(path, "<MultiLanguageText Lang=\"sk-SK\">Freigabe " + action.ToBeReplace + "</MultiLanguageText>", "<MultiLanguageText Lang=\"sk-SK\">Freigabe" + number + " " + f.Funktion + " " + currentSTA + "</MultiLanguageText>");
                                                        ReplacePs(path, "<MultiLanguageText Lang=\"de-DE\">Freigabe " + action.ToBeReplace + "</MultiLanguageText>", "<MultiLanguageText Lang=\"de-DE\">Freigabe" + number + " " + f.Funktion + " " + currentSTA + "</MultiLanguageText>");
                                                        ReplacePs(path, action.ToBeReplace, "Frg" + number + "_" + f.SheetName);

                                                        CreateTag(path, v);

                                                        ReplacePs(path, "<MultiLanguageText Lang=\"sk-SK\">Freigabe " + action.ToBeReplace + "</MultiLanguageText>", "<MultiLanguageText Lang=\"sk-SK\">Verriegelung" + number + " " + f.Funktion + " " + currentSTA + "</MultiLanguageText>");
                                                        ReplacePs(path, "<MultiLanguageText Lang=\"de-DE\">Freigabe " + action.ToBeReplace + "</MultiLanguageText>", "<MultiLanguageText Lang=\"de-DE\">Verriegelung" + number + " " + f.Funktion + " " + currentSTA + "</MultiLanguageText>");
                                                        ReplacePs(path, action.ToBeReplace, "Ver" + number + "_" + f.SheetName);
                                                    }
                                                }
                                            }

                                            xlWorkbook.Close(0);
                                            xlApp.Quit();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (DBtype == "Station2")
            {
                SavePath = Path.Combine(SavePath, "50_Stationen", "DB-Anwender");

                if (DBMaker.Variables.Count() > 0)
                {
                    DBMaker.Variables[0].Action = "Per DT1";

                    foreach (EngAssist eng in DBMaker.EngValues)
                    {
                        if (eng.Station.Contains("DT1") && eng.Arbeitsgruppe_ARG + eng.Schutzkreis_SK + eng.Station != currentDT1)
                        {
                            currentDT1 = eng.Arbeitsgruppe_ARG + eng.Schutzkreis_SK + eng.Station;

                            string path = SavePath + "/" + "Name " + currentDT1 + ".xml";

                            XmlNamespaceManager oManager = new XmlNamespaceManager(new NameTable());
                            oManager.AddNamespace("xxx", "http://www.siemens.com/automation/Openness/SW/Interface/v3");
                            XDocument xmlDocBD = XDocument.Load("C:/Temp/DBThemePlate.xml");
                            var elementName = xmlDocBD.XPathSelectElement("/Document/SW.Blocks.GlobalDB[@ID='0']/AttributeList/Name", oManager);
                            elementName.Value = currentDT1;
                            xmlDocBD.Save(path);
                            DBMaker.BlocksCreated.Add("Name " + currentDT1);

                            foreach (Variable v in DBMaker.Variables)
                            {
                                CreateTag(path, v);
                            }
                        }
                    }
                }
            }

            if (DBtype == "Station3")
            {
                SavePath = Path.Combine(SavePath, "50_Stationen", "DB-Anwender");

                if (DBMaker.Variables.Count() > 0)
                {
                    DBMaker.Variables[0].Action = "Per SF1";

                    foreach (PLC_Tag tag in DBMaker.PLC_Tags)
                    {
                        if (tag.Name.Contains("SF1") && tag.Name.Substring(0, 6) + "SF1" != currentSF1)
                        {
                            currentSF1 = tag.Name.Substring(0, 6) + "SF1";
                            string path = SavePath + "/" + "Name " + currentSF1 + ".xml";

                            XmlNamespaceManager oManager = new XmlNamespaceManager(new NameTable());
                            oManager.AddNamespace("xxx", "http://www.siemens.com/automation/Openness/SW/Interface/v3");
                            XDocument xmlDocBD = XDocument.Load("C:/Temp/DBThemePlate.xml");
                            var elementName = xmlDocBD.XPathSelectElement("/Document/SW.Blocks.GlobalDB[@ID='0']/AttributeList/Name", oManager);
                            elementName.Value = currentSF1;
                            xmlDocBD.Save(path);
                            DBMaker.BlocksCreated.Add("Name " + currentSF1);

                            foreach (Variable v in DBMaker.Variables)
                            {
                                CreateTag(path, v);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Opens the Xml and replaces the %s with the right values
        /// </summary>
        /// <param name="path">Current using path</param>
        /// <param name="oldValue">Number that will be replaced</param>
        /// <param name="newValue">Value that will be replacing the %</param>
        public static void ReplacePs(string path, string oldValue, string newValue)
        {
            string text = File.ReadAllText(path);
            text = text.Replace(oldValue, newValue);
            File.WriteAllText(path, text);
        }

        /// <summary>
        /// Open Xml and creates TAG
        /// </summary>
        /// <param name="path">File path</param>
        /// <param name="v">The Variable that will be used</param>
        public static void CreateTag(string path, Variable v)
        {
            string name = v.Name.Replace("\"", "");
            XDocument xmlDocBD = XDocument.Load(path);
            XmlNamespaceManager oManager = new XmlNamespaceManager(new NameTable());
            oManager.AddNamespace("xxx", "http://www.siemens.com/automation/Openness/SW/Interface/v3");
            var elementStatic = xmlDocBD.XPathSelectElement("/Document/SW.Blocks.GlobalDB[@ID='0']/AttributeList/Interface/xxx:Sections/Section[@Name='Static']", oManager);

            XElement vari = new XElement("Member", new XAttribute("Name", name), new XAttribute("Datatype", v.Type)
                                    , new XElement("AttributeList"
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "SetPoint"), new XAttribute("SystemDefined", "true"), "false"))
                                    , new XElement("Comment"
                                        , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), v.Comment)
                                        , new XElement("MultiLanguageText", new XAttribute("Lang", "sk-SK"), v.Comment)));
            elementStatic.Add(vari);
            xmlDocBD.Save(path);
        }

        /// <summary>
        /// Open XML and creates a Struct
        /// </summary>
        /// <param name="path"></param>
        /// <param name="vStruct"></param>
        /// <param name="variables"></param>
        public static void CreateStruct(string path, Variable vStruct, List<Variable> variables)
        {
            XDocument xmlDocBD = XDocument.Load(path);
            XmlNamespaceManager oManager = new XmlNamespaceManager(new NameTable());
            oManager.AddNamespace("xxx", "http://www.siemens.com/automation/Openness/SW/Interface/v3");
            var elementStatic = xmlDocBD.XPathSelectElement("/Document/SW.Blocks.GlobalDB[@ID='0']/AttributeList/Interface/xxx:Sections/Section[@Name='Static']", oManager);

            XElement xStruct = new XElement("Member", new XAttribute("Name", vStruct.Name), new XAttribute("Datatype", vStruct.Type)
                                    , new XElement("AttributeList"
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "SetPoint"), new XAttribute("SystemDefined", "true"), "false"))
                                    , new XElement("Comment"
                                        , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), vStruct.Comment)
                                        , new XElement("MultiLanguageText", new XAttribute("Lang", "sk-SK"), vStruct.Comment)));

            foreach (var tag in variables)
            {
                XElement xTag = new XElement("Member", new XAttribute("Name", tag.Name), new XAttribute("Datatype", tag.Type)
                                    , new XElement("AttributeList"
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalAccessible"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "UserVisible"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "UserReadOnly"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "false")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "UserDeletable"), new XAttribute("Informative", "true"), new XAttribute("SystemDefined", "true"), "true")
                                        , new XElement("BooleanAttribute", new XAttribute("Name", "SetPoint"), new XAttribute("SystemDefined", "true"), "false"))
                                    , new XElement("Comment"
                                        , new XElement("MultiLanguageText", new XAttribute("Lang", "de-DE"), tag.Comment)
                                        , new XElement("MultiLanguageText", new XAttribute("Lang", "sk-SK"), tag.Comment)));

                xStruct.Add(xTag);
            }

            elementStatic.Add(xStruct);
            xmlDocBD.Save(path);
        }

        /// <summary>
        /// Return quantity of remote panels
        /// </summary>
        /// <returns></returns>
        private static int GetQntRemotePanels()
        {
            int counter = 0;
            List<string> strings = new List<string>();
            foreach (var tag in DBMaker.PLC_Tags.Where(t => t.Name.Substring(1, 3).Contains("IM")))
            {
                // Get 3 next chars after first. 
                // ex: "3IM1KFM1ZUST1" returns "IM1"
                string tagSub = tag.Name.Substring(1, 3);   
                if (!strings.Contains(tagSub) && char.IsDigit(tagSub.Last())) // If is a remote Panel
                {
                    counter++;
                    strings.Add(tagSub);
                }
            }
            return counter;
        }
    }
}

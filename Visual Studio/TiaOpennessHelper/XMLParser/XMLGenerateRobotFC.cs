using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using TiaOpennessHelper.XMLParser;
using Excel = Microsoft.Office.Interop.Excel;

namespace TiaOpennessHelper
{
    public partial class OpennessHelper
    {
        /// <summary>
        /// Generate Empty RobotFC XML
        /// </summary>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <returns>XMLDocument containing the RobotFC</returns>
        public static XmlDocument GenerateRobotFC(string rob, string workPath)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlRobot = new XmlDocument();

            xmlRobot.Load(workPath + @"\Templates\FC\Robot Networks\00 - EmptyRobotFC.xml");
            XmlParser.ReplaceXML(xmlRobot, "ROBNAME", rob);

            return xmlRobot;
        }

        /// <summary>
        /// Generate Network "Eingaben lesen"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateEingabenLesen(XmlDocument xmlRobot, string rob, string workPath)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\01 - EingabenLesen.xml");
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);

            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Network "Freigabe Folge"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <param name="folgeNum"></param>
        /// <param name="folgeDesc"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateFreigabeFolge(XmlDocument xmlRobot, string rob, string workPath, string folgeNum, string folgeDesc)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\02 - FreigabeFolge.xml");
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);
            XmlParser.ReplaceXML(xmlPrgProcess, "FOLGENR", folgeNum);
            XmlParser.ReplaceXML(xmlPrgProcess, "FOLGECOMMENT", folgeDesc);

            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Network "Bildung Folgen"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <param name="xlWorksheet"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateBildungFolgen(XmlDocument xmlRobot, string rob, string workPath, Excel.Worksheet xlWorksheet)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            Dictionary<int, string> Sequences = GetSequences(xlWorksheet);

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\03 - BildungFolgen.xml");
            XmlNamespaceManager ns = new XmlNamespaceManager(xmlPrgProcess.NameTable);
            ns.AddNamespace("msbld", "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v2");
            XmlNode parts = xmlPrgProcess.SelectSingleNode("//msbld:Parts", ns);

            int id = 27;
            int counter = 1;
            foreach (var sequence in Sequences)
            {
                // INSERT GLOBAL VARIABLE
                XmlNode importNode = OpennessHelper.ConvertXElement(CreateFrgFolgeGlobalVariable(sequence.Key, rob, id.ToString()), parts.OwnerDocument);
                parts.PrependChild(importNode);
                xmlPrgProcess = OpennessHelper.OpenToIdentCon(xmlPrgProcess, ns, id);
                id++;

                // INSERT LITERAL CONSTANT
                importNode = OpennessHelper.ConvertXElement(CreateFrgFolgeLiteralConstant(id.ToString(), counter), parts.OwnerDocument);
                parts.PrependChild(importNode);
                xmlPrgProcess = OpennessHelper.OpenToIdentCon(xmlPrgProcess, ns, id);
                id++;

                counter++;
            }

            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);

            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Network "TypRoboter"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateTypRoboter(XmlDocument xmlRobot, string rob, string workPath)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\04 - TypRoboter.xml");
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);

            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Network "StartArbeitsfolge"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateStartArbeitsfolge(XmlDocument xmlRobot, string rob, string workPath)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\05 - StartArbeitsfolge.xml");
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);

            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Network "StartWartungsfolge"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateStartWartungsfolge(XmlDocument xmlRobot, string rob, string workPath)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\06 - StartWartungsfolge.xml");
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);

            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Network "FreigabeMaschinensicherheitHifu"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateFreigabeMaschinensicherheitHifu(XmlDocument xmlRobot, string rob, string workPath)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\07 - FreigabeMaschinensicherheitHifu.xml");
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);

            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Network "FreigabeMaschinensicherheit"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateFreigabeMaschinensicherheit(XmlDocument xmlRobot, string rob, string workPath)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\08 - FreigabeMaschinensicherheit.xml");
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);
            XmlParser.ReplaceXML(xmlPrgProcess, "F2DRBNAME", rob.Substring(0, 2));

            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Network "UberbruckungFolgenkonsistenzprufung"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateUberbruckungFolgenkonsistenzprufung(XmlDocument xmlRobot, string rob, string workPath)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\09 - UberbruckungFolgenkonsistenzprufung.xml");
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);

            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Network "Robotersystemschnittstelle"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateRobotersystemschnittstelle(XmlDocument xmlRobot, string rob, string workPath)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\10 - Robotersystemschnittstelle.xml");
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);
            XmlParser.ReplaceXML(xmlPrgProcess, "F2DRBNAME", rob.Substring(0,2));

            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Network "RoboterHaltKorrigieren"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateRoboterHaltKorrigieren(XmlDocument xmlRobot, string rob, string workPath)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\11 - RoboterHaltKorrigieren.xml");
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);

            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Network "AnwahlWartungWechsel"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateAnwahlWartungWechsel(XmlDocument xmlRobot, string rob, string workPath)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\12 - AnwahlWartungWechsel.xml");
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);

            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Network "Roboterfertigmeldungen"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateRoboterfertigmeldungen(XmlDocument xmlRobot, string rob, string workPath)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\13 - Roboterfertigmeldungen.xml");
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);
            XmlParser.ReplaceXML(xmlPrgProcess, "F2DRBNAME", rob.Substring(0, 2));

            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Network "StatusFertigmeldung"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <param name="fmNum"></param>
        /// <param name="fmDesc"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateStatusFertigmeldung(XmlDocument xmlRobot, string rob, string workPath, string fmNum, string fmDesc)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\14 - StatusFertigmeldung.xml");
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);
            XmlParser.ReplaceXML(xmlPrgProcess, "FMNR", fmNum);
            XmlParser.ReplaceXML(xmlPrgProcess, "FMCOMMENT", fmDesc);

            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Network "StatusFertigmeldungGesamt"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateStatusFertigmeldungGesamt(XmlDocument xmlRobot, string rob, string workPath)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\15 - StatusFertigmeldungGesamt.xml");
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);

            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Network "Roboterverriegelungen"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <param name="colisions"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateRoboterverriegelungen(XmlDocument xmlRobot, string rob, string workPath, List<List<string>> colisions)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\16 - Roboterverriegelungen.xml");
            XmlNamespaceManager ns = new XmlNamespaceManager(xmlPrgProcess.NameTable);
            ns.AddNamespace("msbld", "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v2");

            int idFrg = 44;
            int idVer = 23;
            foreach (var col in colisions)
            {
                string[] cols = col[3].Split(',');

                for (int i = 0; i < cols.Length; i++)
                {
                    idFrg = 44 + int.Parse(cols[i]);
                    idVer = 23 + int.Parse(cols[i]);

                    // ADD FRG
                    XmlNode Frg = xmlPrgProcess.SelectSingleNode("//msbld:Frg"+cols[i], ns);
                    XmlNode importNode = OpennessHelper.ConvertXElement(CreateFrg_an(col[0], cols[i], idFrg), xmlPrgProcess);
                    Frg.AppendChild(importNode);
                    while (Frg.HasChildNodes)
                        Frg.ParentNode.InsertBefore(Frg.ChildNodes[0], Frg);
                    Frg.ParentNode.RemoveChild(Frg);

                    //ADD VER
                    XmlNode Ver = xmlPrgProcess.SelectSingleNode("//msbld:Ver" + cols[i], ns);
                    importNode = OpennessHelper.ConvertXElement(CreateVer(idVer, col[1]), xmlPrgProcess);
                    Ver.AppendChild(importNode);
                    while (Ver.HasChildNodes)
                        Ver.ParentNode.InsertBefore(Ver.ChildNodes[0], Ver);
                    Ver.ParentNode.RemoveChild(Ver);

                    xmlPrgProcess = OpennessHelper.OpenToIdentCon(xmlPrgProcess, ns, idVer);
                }
            }

            // Replace the rest of Frg_an with Temp_Bool
            for (int i = 1; i <= 16; i++)
            {
                XmlNode Frg = xmlPrgProcess.SelectSingleNode("//msbld:Frg"+i, ns);
                if (Frg != null)
                {
                    idFrg = 44 + i;
                    XmlNode importNode = OpennessHelper.ConvertXElement(CreateFrg_anTempBool(idFrg), xmlPrgProcess);
                    Frg.AppendChild(importNode);
                    while(Frg.HasChildNodes)
                        Frg.ParentNode.InsertBefore(Frg.ChildNodes[0], Frg);
                    Frg.ParentNode.RemoveChild(Frg);
                }

                XmlNode Ver = xmlPrgProcess.SelectSingleNode("//msbld:Ver" + i, ns);
                if(Ver != null)
                    Ver.ParentNode.RemoveChild(Ver);
            }

            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNR", colisions[0][2]);

            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Network "WerkzeugfreigabenAnlage"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <param name="Frgs"></param>
        /// <param name="mask"></param>
        /// <param name="frgNr"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateWerkzeugfreigabenAnlage(XmlDocument xmlRobot, string rob, string workPath, List<string> Frgs, string mask, string frgNr)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\17 - WerkzeugfreigabenAnlage.xml");

            for (int i = 0; i < Frgs.Count; i++)
            {
                XmlParser.ReplaceXML(xmlPrgProcess, "ANUM" + (i+1), Frgs[i]);
            }

            XmlParser.ReplaceXML(xmlPrgProcess, "ANUMFIRST", Frgs.First().Substring(0, 2));
            XmlParser.ReplaceXML(xmlPrgProcess, "ANUMLAST", Frgs.Last().Substring(0, 2));
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);
            XmlParser.ReplaceXML(xmlPrgProcess, "BINARY", Convert.ToString(Convert.ToInt32(mask, 16), 2));
            XmlParser.ReplaceXML(xmlPrgProcess, "FRGNR", frgNr); 
            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Network "Stellungsfreigabe"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <param name="ANum"></param>
        /// <param name="bescheibung"></param>
        /// <param name="funktion"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateStellungsfreigabe(XmlDocument xmlRobot, string rob, string workPath, string ANum, string bescheibung, string funktion)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();
            bescheibung = "(E" + ANum.Substring(0, 2) + ") " + bescheibung;
            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\18 - Stellungsfreigabe.xml");
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);
            XmlParser.ReplaceXML(xmlPrgProcess, "ANUM", ANum);
            XmlParser.ReplaceXML(xmlPrgProcess, "BESCHEIBUNG", bescheibung);
            XmlParser.ReplaceXML(xmlPrgProcess, "FUNKTION", funktion);
            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Network "Roboterfehlernummer"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateRoboterfehlernummer(XmlDocument xmlRobot, string rob, string workPath)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\20 - Roboterfehlernummer.xml");
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);
            XmlParser.ReplaceXML(xmlPrgProcess, "F2DRBNAME", rob.Substring(0, 2));

            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Network "TaktzeitStoppHifu"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <param name="sequences"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateTaktzeitStoppHifu(XmlDocument xmlRobot, string rob, string workPath, Dictionary<int, string> sequences)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\21 - TaktzeitStoppHifu.xml");
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);

            int counter = 1;
            foreach (var seq in sequences)
            {
                XmlParser.ReplaceXML(xmlPrgProcess, "FOLGENR" + counter++, seq.Key.ToString());
            }

            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Network "TaktzeitRoboter"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateTaktzeitRoboter(XmlDocument xmlRobot, string rob, string workPath)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\22 - TaktzeitRoboter.xml");
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);
            XmlParser.ReplaceXML(xmlPrgProcess, "F2DRBNAME", rob.Substring(0, 2));

            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Network "AusgabenSchreiben"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateAusgabenSchreiben(XmlDocument xmlRobot, string rob, string workPath)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\23 - AusgabenSchreiben.xml");
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);

            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Network "StatusRoboter"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateStatusRoboter(XmlDocument xmlRobot, string rob, string workPath)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\24 - StatusRoboter.xml");
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);

            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Network "StatusProzessgerate"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateStatusProzessgerate(XmlDocument xmlRobot, string rob, string workPath)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\25 - StatusProzessgerate.xml");
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);

            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Network "Bausteinende"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="workPath"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateBausteinende(XmlDocument xmlRobot, string workPath)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\26 - Bausteinende.xml");

            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Network "Stellungsfreigaben"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <param name="Frgs"></param>
        /// <param name="mask"></param>
        /// <param name="FrgNr"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateStellungsfreigaben(XmlDocument xmlRobot, string rob, string workPath, List<string> Frgs, string mask, string FrgNr)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\19 - Stellungsfreigaben.xml");
            for (int i = 0; i < Frgs.Count; i++)
            {
                XmlParser.ReplaceXML(xmlPrgProcess, "ANUM" + (i + 1), Frgs[i]);
                XmlParser.ReplaceXML(xmlPrgProcess, "ADIGITS" + (i + 1), Frgs[i].Substring(0, 2));
            }

            XmlParser.ReplaceXML(xmlPrgProcess, "ANUMFIRST", Frgs.First().Substring(0, 2));
            XmlParser.ReplaceXML(xmlPrgProcess, "ANUMLAST", Frgs.Last().Substring(0, 2));
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);
            XmlParser.ReplaceXML(xmlPrgProcess, "BINARY", Convert.ToString(Convert.ToInt32(mask, 16), 2));
            XmlParser.ReplaceXML(xmlPrgProcess, "FRGNR", FrgNr);

            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Robot Database
        /// </summary>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <param name="outputs"></param>
        /// <param name="inputs"></param>
        /// <param name="sequences"></param>
        /// <param name="fms"></param>
        /// <param name="tecnologies"></param>
        public static XmlDocument GenerateDB(string rob, string workPath, List<List<string>> outputs, List<List<string>> inputs, Dictionary<int, string> sequences, Dictionary<int, string> fms, Dictionary<string, string> tecnologies)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\DBRobot.xml");
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);

            XmlNamespaceManager ns = new XmlNamespaceManager(xmlPrgProcess.NameTable);
            ns.AddNamespace("msbld", "http://www.siemens.com/automation/Openness/SW/Interface/v3");

            XmlNode Tecnologies = xmlPrgProcess.SelectSingleNode("//msbld:Tecnologies", ns);
            foreach (var tec in tecnologies)
            {
                XElement member = CreateMemberTag(tec.Key, "\"STB_PG\"", tec.Value, isTecnologie: true);
                InsertNewMember(xmlPrgProcess, member, Tecnologies);
            }
            while (Tecnologies.HasChildNodes)
                Tecnologies.ParentNode.InsertBefore(Tecnologies.ChildNodes[0], Tecnologies);
            Tecnologies.ParentNode.RemoveChild(Tecnologies);

            XmlNode nFMs = xmlPrgProcess.SelectSingleNode("//msbld:Fms", ns);
            foreach (var fm in fms)
            {
                XElement member = CreateMemberTag("FM" + fm.Key.ToString(), "Bool", fm.Value);
                InsertNewMember(xmlPrgProcess, member, nFMs);
            }
            for (int i = fms.Count + 1; i <= 14; i++)
            {
                XElement member = CreateMemberTag("FM" + i, "Bool", "Fertigmeldung " + i);
                InsertNewMember(xmlPrgProcess, member, nFMs);
            }
            while (nFMs.HasChildNodes)
                nFMs.ParentNode.InsertBefore(nFMs.ChildNodes[0], nFMs);
            nFMs.ParentNode.RemoveChild(nFMs);

            XmlNode AandE = xmlPrgProcess.SelectSingleNode("//msbld:AandE", ns);
            foreach (var output in outputs)
            {
                // [0] = E/A Number
                // [1] = Description
                XElement member = CreateMemberTag("E" + output[0], "Bool", output[1]);
                InsertNewMember(xmlPrgProcess, member, AandE);
            }
            foreach (var input in inputs)
            {
                // [0] = E/A Number
                // [1] = Description
                XElement member = CreateMemberTag("A" + input[0], "Bool", input[1]);
                InsertNewMember(xmlPrgProcess, member, AandE);
            }
            while (AandE.HasChildNodes)
                AandE.ParentNode.InsertBefore(AandE.ChildNodes[0], AandE);
            AandE.ParentNode.RemoveChild(AandE);

            XmlNode FrgFolges = xmlPrgProcess.SelectSingleNode("//msbld:FrgFolges", ns);
            foreach (var seq in sequences)
            {
                XElement member = CreateMemberTag("FrgFolge" + seq.Key.ToString(), "Bool", seq.Value);
                InsertNewMember(xmlPrgProcess, member, FrgFolges);
            }
            while (FrgFolges.HasChildNodes)
                FrgFolges.ParentNode.InsertBefore(FrgFolges.ChildNodes[0], FrgFolges);
            FrgFolges.ParentNode.RemoveChild(FrgFolges);

            return xmlPrgProcess;
        }

        #region ROBOT TECNOLOGIES
        /// <summary>
        /// Generate Network "Schweibsteuerung"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <param name="tecName"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateSchweibsteuerung(XmlDocument xmlRobot, string rob, string workPath, string tecName)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\Tecnologies\Schweibsteuerung.xml");
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);
            XmlParser.ReplaceXML(xmlPrgProcess, "F2DRBNAME", rob.Substring(0, 2));
            XmlParser.ReplaceXML(xmlPrgProcess, "TECNAME", tecName);
            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Network "Medien"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateMedien(XmlDocument xmlRobot, string rob, string workPath)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\Tecnologies\Medien.xml");
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);
            XmlParser.ReplaceXML(xmlPrgProcess, "F2DRBNAME", rob.Substring(0, 2));
            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Network "Greifer"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <param name="tecName"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateGreifer(XmlDocument xmlRobot, string rob, string workPath, string tecName)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\Tecnologies\Greifer.xml");
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);
            XmlParser.ReplaceXML(xmlPrgProcess, "F2DRBNAME", rob.Substring(0, 2));
            XmlParser.ReplaceXML(xmlPrgProcess, "TECNAME", tecName);
            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Network "Kleben"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <param name="tecName"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateKleben(XmlDocument xmlRobot, string rob, string workPath, string tecName)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\Tecnologies\Kleben.xml");
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);
            XmlParser.ReplaceXML(xmlPrgProcess, "F2DRBNAME", rob.Substring(0, 2));
            XmlParser.ReplaceXML(xmlPrgProcess, "TECNAME", tecName);
            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Network "Kappenwechsler"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <param name="tecName"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateKappenwechsler(XmlDocument xmlRobot, string rob, string workPath, string tecName)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\Tecnologies\Kappenwechsler.xml");
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);
            XmlParser.ReplaceXML(xmlPrgProcess, "F2DRBNAME", rob.Substring(0, 2));
            XmlParser.ReplaceXML(xmlPrgProcess, "TECNAME", tecName);
            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }

        /// <summary>
        /// Generate Network "Mutternstanzen"
        /// </summary>
        /// <param name="xmlRobot"></param>
        /// <param name="rob"></param>
        /// <param name="workPath"></param>
        /// <param name="tecName"></param>
        /// <returns>XMLDocument containing the Network</returns>
        public static void GenerateStanzen(XmlDocument xmlRobot, string rob, string workPath, string tecName)
        {
            //Reading of the xml file that contains the new Robot FC without networks
            XmlDocument xmlPrgProcess = new XmlDocument();

            xmlPrgProcess.Load(workPath + @"\Templates\FC\Robot Networks\Tecnologies\Stanzen.xml");
            XmlParser.ReplaceXML(xmlPrgProcess, "ROBNAME", rob);
            XmlParser.ReplaceXML(xmlPrgProcess, "F2DRBNAME", rob.Substring(0, 2));
            XmlParser.ReplaceXML(xmlPrgProcess, "TECNAME", tecName);
            InsertNewNetwork(xmlRobot, xmlPrgProcess);
        }
        #endregion

        /// <summary>
        /// Insert new member inside section tag
        /// </summary>
        /// <param name="xmlPrgProcess"></param>
        /// <param name="memberToAppend"></param>
        /// <param name="nodeToAppend"></param>
        public static void InsertNewMember(XmlDocument xmlPrgProcess, XElement memberToAppend, XmlNode nodeToAppend)
        {
            XmlDocument xD = new XmlDocument();
            xD.LoadXml(memberToAppend.ToString());
            XmlNode xMember = xD.FirstChild;
            XmlNode networkImported = xmlPrgProcess.ImportNode(xMember, true);
            nodeToAppend.AppendChild(networkImported);
        }

        /// <summary>
        /// Save RobotFC as XML
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="xmlName"></param>
        /// <param name="workPath"></param>
        public static void SaveXMLDocument(XmlDocument xmlDoc, string xmlName, string workPath)
        {
            if (!Directory.Exists(workPath))
                Directory.CreateDirectory(workPath);

            string destPath = $"{workPath}\\{xmlName}.xml";
            xmlDoc.Save(destPath);
        }

        /// <summary>
        /// Insert of the new networks inside the Robot FC
        /// </summary>
        /// <param name="originalXml"></param>
        /// <param name="newNetwork"></param>
        public static void InsertNewNetwork(XmlDocument originalXml, XmlDocument newNetwork)
        {
            XmlNodeList Network = originalXml.SelectNodes("/Document/SW.Blocks.FC/ObjectList");
            XmlNode networkToImport = newNetwork.FirstChild;
            XmlNode nodePointer = Network[0];
            XmlNode networkImported = originalXml.ImportNode(networkToImport, true);
            nodePointer.InsertBefore(networkImported, nodePointer.LastChild);
        }

        #region Create Elements
        /// <summary>
        /// Create element "Access" to the network "Bildung Folgen" with scope = GlobalVariable
        /// </summary>
        /// <param name="num"></param>
        /// <param name="robName"></param>
        /// <param name="id"></param>
        /// <returns>XElement containing the Access element</returns>
        private static XElement CreateFrgFolgeGlobalVariable(int num, string robName, string id)
        {
            XElement AccessGlobalVariable = new XElement("Access", new XAttribute("Scope", "GlobalVariable"), new XAttribute("UId", id)
                                                                 , new XElement("Symbol", new XElement("Component", new XAttribute("Name", robName))
                                                                                        , new XElement("Component", new XAttribute("Name", "FrgFolge" + num))
                                                                                        , new XElement("Address", new XAttribute("Area", "None"), new XAttribute("Type", "Bool")
                                                                                                                , new XAttribute("BlockNumber", "180"), new XAttribute("BitOffset", "32688")
                                                                                                                , new XAttribute("Informative", "true"))));

            return AccessGlobalVariable;
        }
        /// <summary>
        /// Create element "Access" to the network "Bildung Folgen" with scope = LiteralConstant
        /// </summary>
        /// <returns>XElement containing the Access element</returns>
        private static XElement CreateFrgFolgeLiteralConstant(string id, int @byte)
        {
            XElement AccessLiteralConstant = new XElement("Access", new XAttribute("Scope", "LiteralConstant"), new XAttribute("UId", id)
                                                                  , new XElement("Constant", new XElement("ConstantType", "Byte")
                                                                                           , new XElement("ConstantValue", $"16#0{@byte}")
                                                                                           , new XElement("StringAttribute", new XAttribute("Name", "Format"), new XAttribute("Informative", "true"), "Hex")));

            return AccessLiteralConstant;
        }

        /// <summary>
        /// Create element "Access" to the network "Roboterverriegelungen" with custom Frg_an
        /// </summary>
        /// <returns>XElement containing the Access element</returns>
        private static XElement CreateFrg_an(string colRob, string frgNum, int id)
        {
            int ENumber = 0;

            switch (frgNum)
            {
                case "1":
                    ENumber = 41;
                    break;
                case "2":
                    ENumber = 42;
                    break;
                case "3":
                    ENumber = 43;
                    break;
                case "4":
                    ENumber = 44;
                    break;
                case "5":
                    ENumber = 45;
                    break;
                case "6":
                    ENumber = 46;
                    break;
                case "7":
                    ENumber = 47;
                    break;
                case "8":
                    ENumber = 48;
                    break;
                case "9":
                    ENumber = 49;
                    break;
                case "10":
                    ENumber = 50;
                    break;
                case "11":
                    ENumber = 51;
                    break;
                case "12":
                    ENumber = 52;
                    break;
                case "13":
                    ENumber = 53;
                    break;
                case "14":
                    ENumber = 54;
                    break;
                case "15":
                    ENumber = 55;
                    break;
                case "16":
                    ENumber = 56;
                    break;
            }

            XElement Access = new XElement("Access", new XAttribute("Scope", "GlobalVariable"), new XAttribute("UId", id)
                                                   , new XElement("Symbol", new XElement("Component", new XAttribute("Name", colRob))
                                                                          , new XElement("Component", new XAttribute("Name", "Rob"))
                                                                          , new XElement("Component", new XAttribute("Name", "E"))
                                                                          , new XElement("Component", new XAttribute("Name", "E" + ENumber + "_Roboter_FRG"))
                                                                          , new XElement("Address", new XAttribute("Area", "None"), new XAttribute("Type", "Bool")
                                                                                                  , new XAttribute("BlockNumber", "185"), new XAttribute("BitOffset", "304")
                                                                                                  , new XAttribute("Informative", "true"))));
            return Access;
        }
        /// <summary>
        /// Create element "Access" to the network "Roboterverriegelungen" with Temp_Bool Frg_an
        /// </summary>
        /// <returns>XElement containing the Access element</returns>
        private static XElement CreateFrg_anTempBool(int id)
        {
            XElement Access = new XElement("Access", new XAttribute("Scope", "LocalVariable"), new XAttribute("UId", id)
                                                   , new XElement("Symbol", new XElement("Component", new XAttribute("Name", "Temp"))
                                                                          , new XElement("Component", new XAttribute("Name", "_Bool"))));
            return Access;
        }
        /// <summary>
        /// Create element "Access" to the network "Roboterverriegelungen" to Ver1_Rob
        /// </summary>
        /// <returns>XElement containing the Access element</returns>
        private static XElement CreateVer(int id, string robNr)
        {
            XElement Access = new XElement("Access", new XAttribute("Scope", "LiteralConstant"), new XAttribute("UId", id)
                                                   , new XElement("Constant", new XElement("ConstantType", "Int")
                                                                            , new XElement("ConstantValue", robNr)
                                                                            , new XElement("StringAttribute", "Dec_signed", new XAttribute("Name", "Format"), new XAttribute("Informative", "true"))));
            return Access;
        }

        /// <summary>
        /// Create Memeber Tag for Robot Database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dataType"></param>
        /// <param name="comment"></param>
        /// <param name="isTecnologie"></param>
        /// <returns></returns>
        private static XElement CreateMemberTag(string name, string dataType, string comment, bool isTecnologie = false)
        {
            XElement member = new XElement("Member", new XAttribute("Name", name), new XAttribute("Datatype", dataType), new XAttribute("Remanence", "Retain")
                                                   , new XAttribute("Accessibility", "Public"));

            XElement attributeList = new XElement("AttributeList", new XElement("BooleanAttribute", "false", new XAttribute("Name", "ExternalAccessible")
                                                                                                           , new XAttribute("SystemDefined", "true"))
                                                                 , new XElement("BooleanAttribute", "false", new XAttribute("Name", "ExternalVisible")
                                                                                                           , new XAttribute("SystemDefined", "true"))
                                                                 , new XElement("BooleanAttribute", "false", new XAttribute("Name", "ExternalWritable")
                                                                                                           , new XAttribute("SystemDefined", "true"))
                                                                 , new XElement("BooleanAttribute", "true" , new XAttribute("Name", "UserVisible")
                                                                                                           , new XAttribute("Informative", "true")
                                                                                                           , new XAttribute("SystemDefined", "true"))
                                                                 , new XElement("BooleanAttribute", "false", new XAttribute("Name", "UserReadOnly")
                                                                                                           , new XAttribute("Informative", "true")
                                                                                                           , new XAttribute("SystemDefined", "true"))
                                                                 , new XElement("BooleanAttribute", "true" , new XAttribute("Name", "UserDeletable")
                                                                                                           , new XAttribute("Informative", "true")
                                                                                                           , new XAttribute("SystemDefined", "true")));

            if(!isTecnologie)
            {
                XElement setPoint = new XElement("BooleanAttribute", "false", new XAttribute("Name", "SetPoint")
                                                                            , new XAttribute("SystemDefined", "true"));

                attributeList.Add(setPoint);
            }

            XElement com = new XElement("Comment", new XElement("MultiLanguageText", comment, new XAttribute("Lang", "de-DE")));

            member.Add(attributeList);
            member.Add(com);

            if (isTecnologie)
            {
                XElement sections = new XElement("Sections");

                XElement section = new XElement("Section", new XAttribute("Name", "None"), new XElement("Member", new XAttribute("Name", "mProz"), new XAttribute("Datatype", "Bool"))
                                                                                         , new XElement("Member", new XAttribute("Name", "oProz"), new XAttribute("Datatype", "Bool"))
                                                                                         , new XElement("Member", new XAttribute("Name", "FrgSchw"), new XAttribute("Datatype", "Bool"))
                                                                                         , new XElement("Member", new XAttribute("Name", "AnfSchw"), new XAttribute("Datatype", "Bool"))
                                                                                         , new XElement("Member", new XAttribute("Name", "PA"), new XAttribute("Datatype", "Bool"))
                                                                                         , new XElement("Member", new XAttribute("Name", "Vorw"), new XAttribute("Datatype", "Bool"))
                                                                                         , new XElement("Member", new XAttribute("Name", "NIO"), new XAttribute("Datatype", "Bool"))
                                                                                         , new XElement("Member", new XAttribute("Name", "IO"), new XAttribute("Datatype", "Bool"))
                                                                                         , new XElement("Member", new XAttribute("Name", "MxStnMgn"), new XAttribute("Datatype", "Bool"))
                                                                                         , new XElement("Member", new XAttribute("Name", "PMF"), new XAttribute("Datatype", "Bool"))
                                                                                         , new XElement("Member", new XAttribute("Name", "Stoe"), new XAttribute("Datatype", "Bool"))
                                                                                         , new XElement("Member", new XAttribute("Name", "Stoe_PZ"), new XAttribute("Datatype", "Bool"))
                                                                                         , new XElement("Member", new XAttribute("Name", "Stoe_St"), new XAttribute("Datatype", "Bool"))
                                                                                         , new XElement("Member", new XAttribute("Name", "Stoe_AP"), new XAttribute("Datatype", "Bool"))
                                                                                         , new XElement("Member", new XAttribute("Name", "Stoe_HP"), new XAttribute("Datatype", "Bool")));

                sections.Add(section);
                member.Add(sections);
            }

            return member;
        }
        #endregion

        #region Rob_SPS EXCEL
        /// <summary>
        /// Get all FM's information from excel file
        /// </summary>
        /// <param name="xlWorksheet"></param>
        /// <returns>Dictionary containing the number and the description</returns>
        public static Dictionary<int, string> GetFMs(Worksheet xlWorksheet)
        {
            Dictionary<int, string> FMInfo = new Dictionary<int, string>();
            Range xlRange = xlWorksheet.UsedRange;

            for (int i = 4; i <= 17; i++)
            {
                if ((xlRange.Cells[i, 20] as Range).Value != null)
                {
                    int.TryParse((xlRange.Cells[i, 19] as Range).Value.ToString(), out int FMNum);
                    string FMComment = ((xlRange.Cells[i, 20] as Range).Value).ToString();

                    FMInfo.Add(FMNum, FMComment);
                }
                else break;
            }

            return FMInfo;
        }

        /// <summary>
        /// Get all Tecnologie Descriptions from excel file
        /// </summary>
        /// <param name="xlWorksheet"></param>
        /// <returns>List containing the descriptions</returns>
        public static List<string> GetTecnologies(Worksheet xlWorksheet)
        {
            List<string> Tecnologies = new List<string>();
            Range xlRange = xlWorksheet.UsedRange;

            for (int i = 4; i <= 17; i++)
            {
                if ((xlRange.Cells[i, 22] as Range).Value != null)
                {
                    string tecDesc = ((xlRange.Cells[i, 22] as Range).Value).ToString();

                    Tecnologies.Add(tecDesc);
                }
                else break;
            }

            return Tecnologies;
        }

        /// <summary>
        /// Get all Sequence Informations from excel file
        /// </summary>
        /// <param name="xlWorksheet"></param>
        /// <returns>Dictionary containing the number and the description</returns>
        public static Dictionary<int, string> GetSequences(Worksheet xlWorksheet)
        {
            Dictionary<int, string> Sequences = new Dictionary<int, string>();
            Range xlRange = xlWorksheet.UsedRange;

            for (int i = 4; i <= 27; i++)
            {
                if ((xlRange.Cells[i, 1] as Range).Value != null)
                {
                    int.TryParse((xlRange.Cells[i, 1] as Range).Value.ToString(), out int num);
                    string desc = ((xlRange.Cells[i, 2] as Range).Value).ToString();

                    Sequences.Add(num, desc);
                }
                else break;
            }

            return Sequences;
        }

        /// <summary>
        /// Get Robot Outputs Informations from excel file
        /// [0] = E/A Number
        /// [1] = Description
        /// [2] = Function
        /// [3] = Mask
        /// </summary>
        /// <param name="xlWorksheet"></param>
        /// <returns>List containing Robot Outputs Information</returns>
        public static List<List<string>> GetRobotOutputs(Worksheet xlWorksheet)
        {
            List<List<string>> RobotOutputInfo = new List<List<string>>();
            Range xlRange = xlWorksheet.UsedRange;

            for (int i = 4; i <= 27; i++)
            {
                if ((xlRange.Cells[i, 8] as Range).Value != null)
                {
                    string Desc = Convert.ToString((xlRange.Cells[i, 16] as Range).Value);
                    string Function = Convert.ToString((xlRange.Cells[i, 17] as Range).Value);
                    string Profile = Convert.ToString((xlRange.Cells[i, 14] as Range).Value);
                    string ANum = Convert.ToString((xlRange.Cells[i, 5] as Range).Value);
                    string Mask = Convert.ToString((xlRange.Cells[i, 12] as Range).Value);

                    if (Function != null)
                        ANum = ANum + "_" + Profile.Substring(Profile.LastIndexOf('_') + 1);

                    RobotOutputInfo.Add(new List<string>() { ANum, Desc, Function, Mask });
                }
                else break;
            }

            return RobotOutputInfo;
        }

        /// <summary>
        /// Get Robot Inputs Informations from excel file
        /// [0] = E/A Number
        /// [1] = Description
        /// [2] = Function
        /// [3] = Mask
        /// </summary>
        /// <param name="xlWorksheet"></param>
        /// <returns>List containing Robot Inputs Information</returns>
        public static List<List<string>> GetRobotInputs(Worksheet xlWorksheet)
        {
            List<List<string>> RobotInputInfo = new List<List<string>>();
            Range xlRange = xlWorksheet.UsedRange;

            for (int i = 4; i <= 27; i++)
            {
                if ((xlRange.Cells[i, 8] as Range).Value != null)
                {
                    string Desc = Convert.ToString((xlRange.Cells[i, 10] as Range).Value);
                    string Function = Convert.ToString((xlRange.Cells[i, 11] as Range).Value);
                    string Profile = Convert.ToString((xlRange.Cells[i, 8] as Range).Value);
                    string ANum = Convert.ToString((xlRange.Cells[i, 5] as Range).Value);
                    string Mask = Convert.ToString((xlRange.Cells[i, 6] as Range).Value);

                    if (Function != null)
                        ANum = ANum + "_" + Profile.Substring(Profile.LastIndexOf('_') + 1);

                    RobotInputInfo.Add(new List<string>() { ANum, Desc, Function, Mask });
                }
                else break;
            }

            return RobotInputInfo;
        }

        /// <summary>
        /// Get Robot Colisions Informations from excel file
        /// Pos[0] = Colision Robot Name
        /// Pos[1] = Colision Robot Number
        /// Pos[2] = Current Robot Number
        /// Pos[3] = Colision Numbers
        /// </summary>
        /// <param name="xlWorksheet"></param>
        /// <param name="robName"></param>
        /// <returns>List containing Robot Colisions</returns>
        public static List<List<string>> GetRobotColisions(Worksheet xlWorksheet, string robName)
        {
            List<List<string>> Colisions = new List<List<string>>();

            //Range startCell = xlWorksheet.Cells[1, 1];
            //Range endCell = xlWorksheet.Cells[25, 23];
            object[,] matrixRead = ExcelToMatrix(xlWorksheet);

            for (int rows = 6; rows <= 25; rows++)
            {
                if (Convert.ToString(matrixRead[rows, 3]) == robName)
                {
                    string colision = "";
                    string robCol = "";
                    string robColNum = "";
                    string robNum = "";
                    int col = 4;
                    do
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(matrixRead[rows, col])) && Convert.ToString(matrixRead[rows, col]) != "0")
                        {
                            colision = matrixRead[rows, col].ToString();
                            robCol = matrixRead[5, col].ToString();
                            robColNum = matrixRead[4, col].ToString();
                            robNum = matrixRead[rows, 2].ToString();
                            Colisions.Add(new List<string>() { robCol, robColNum, robNum, colision });
                        }

                        col++;
                    } while (col != 23);
                }
            }

            return Colisions;
        }

        /// <summary>
        /// Convert XElement to XmlNode
        /// </summary>
        /// <param name="element"></param>
        /// <param name="doc"></param>
        /// <returns>XmlNode converted</returns>
        public static XmlNode ConvertXElement(XElement element, XmlDocument doc)
        {
            XmlDocument xD = new XmlDocument();
            xD.LoadXml(element.ToString());
            XmlNode xN = xD.FirstChild;
            XmlNode convertedNode = doc.ImportNode(xN, true);

            return convertedNode;
        }

        /// <summary>
        /// Change OpenCon to IdentCon
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="ns"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static XmlDocument OpenToIdentCon(XmlDocument doc, XmlNamespaceManager ns, int id)
        {
            XmlNode verWireOpen = doc.SelectSingleNode("//msbld:OpenCon[@UId='" + id + "']", ns);
            XmlNode verWireIdent = doc.CreateNode(XmlNodeType.Element, "IdentCon", "");
            XmlAttribute attr = doc.CreateAttribute("UId");
            attr.Value = id.ToString();
            verWireIdent.Attributes.SetNamedItem(attr);
            verWireOpen.ParentNode.ReplaceChild(verWireIdent, verWireOpen);

            return doc;
        }
        #endregion
    }
}

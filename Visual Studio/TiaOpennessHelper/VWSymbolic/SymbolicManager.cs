using Microsoft.Office.Interop.Excel;
using Siemens.Engineering;
using Siemens.Engineering.SW.Tags;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using TiaOpennessHelper.XMLParser;
using Excel = Microsoft.Office.Interop.Excel;

namespace TiaOpennessHelper.VWSymbolic
{
    public class SymbolicManager
    {
        public string SavePath { get; set; }
        public string PlcDBPath { get; set; }
        public object Current { get; set; }
        private string name;
        private RobotInfo robInfo;
        private List<List<RobotBase>> robBase;
        private List<List<RobotTecnologie>> robTecnologies;
        private List<List<RobotSafeRangeMonitoring>> robSafeRangeMonitoring;
        private List<List<RobotSafeOperation>> robSafeOperations;
        private bool isChangeSymbolic;
        private string workingDirectory;
        private readonly string workPath;

        /// <summary>
        /// Normal Constructor
        /// </summary>
        public SymbolicManager()
        {
            workingDirectory = Environment.CurrentDirectory;
            workPath = Directory.GetParent(workingDirectory).FullName;

            InitializeLists();
        }

        /// <summary>
        /// Change Robot Constructor
        /// </summary>
        public SymbolicManager(RobotInfo RobInfo, List<List<RobotBase>> RobBase, List<List<RobotTecnologie>> RobTecnologies, List<List<RobotSafeRangeMonitoring>> RobSafeRangeMonitoring, List<List<RobotSafeOperation>> RobSafeOperations)
        {
            workingDirectory = Environment.CurrentDirectory;
            workPath = Directory.GetParent(workingDirectory).FullName;

            this.robInfo = RobInfo;
            this.robBase = RobBase;
            this.robTecnologies = RobTecnologies;
            this.robSafeRangeMonitoring = RobSafeRangeMonitoring;
            this.robSafeOperations = RobSafeOperations;

            isChangeSymbolic = true;
        }

        /// <summary>
        /// Initialize Lists
        /// </summary>
        private void InitializeLists()
        {
            robBase = new List<List<RobotBase>>();
            robTecnologies = new List<List<RobotTecnologie>>();
            robSafeRangeMonitoring = new List<List<RobotSafeRangeMonitoring>>();
            robSafeOperations = new List<List<RobotSafeOperation>>();

            // RobBase List
            var secRobBase = new List<RobotBase>();
            foreach (var o in Robot.RobBase[0])
            {
                secRobBase.Add(new RobotBase(o.Symbolic, o.DataType, o.Address, o.Comment));
            }
            robBase.Add(secRobBase);
            secRobBase = new List<RobotBase>();
            foreach (var i in Robot.RobBase[1])
            {
                secRobBase.Add(new RobotBase(i.Symbolic, i.DataType, i.Address, i.Comment));
            }
            robBase.Add(secRobBase);

            // RobSafeOperations List
            var secRobSafeOperations = new List<RobotSafeOperation>();
            foreach (var o in Robot.RobSafeOperations[0])
            {
                secRobSafeOperations.Add(new RobotSafeOperation(o.Symbolic, o.DataType, o.Address, o.Comment));
            }
            robSafeOperations.Add(secRobSafeOperations);
            secRobSafeOperations = new List<RobotSafeOperation>();
            foreach (var i in Robot.RobSafeOperations[1])
            {
                secRobSafeOperations.Add(new RobotSafeOperation(i.Symbolic, i.DataType, i.Address, i.Comment));
            }
            robSafeOperations.Add(secRobSafeOperations);

            // RobSafeRangeMonitoring List
            var secRobSafeRangeMonitoring = new List<RobotSafeRangeMonitoring>();
            foreach (var o in Robot.RobSafeRangeMonitoring[0])
            {
                secRobSafeRangeMonitoring.Add(new RobotSafeRangeMonitoring(o.Symbolic, o.DataType, o.Address, o.Comment));
            }
            robSafeRangeMonitoring.Add(secRobSafeRangeMonitoring);
            secRobSafeRangeMonitoring = new List<RobotSafeRangeMonitoring>();
            foreach (var i in Robot.RobSafeRangeMonitoring[1])
            {
                secRobSafeRangeMonitoring.Add(new RobotSafeRangeMonitoring(i.Symbolic, i.DataType, i.Address, i.Comment));
            }
            robSafeRangeMonitoring.Add(secRobSafeRangeMonitoring);

            // RobTecnologies List
            var secRobTecnologies = new List<RobotTecnologie>();
            foreach (var o in Robot.RobTecnologies[0])
            {
                secRobTecnologies.Add(new RobotTecnologie(o.FBNumber, o.Name, o.Type, o.Symbolic, o.DataType, o.Address, o.Comment));
            }
            robTecnologies.Add(secRobTecnologies);
            secRobTecnologies = new List<RobotTecnologie>();
            foreach (var i in Robot.RobTecnologies[1])
            {
                secRobTecnologies.Add(new RobotTecnologie(i.FBNumber, i.Name, i.Type, i.Symbolic, i.DataType, i.Address, i.Comment));
            }
            robTecnologies.Add(secRobTecnologies);
        }

        /// <summary>
        /// Creates new Robot
        /// </summary>
        /// <param name="startAddress"></param>
        /// <param name="name"></param>
        /// <param name="robSafe"></param>
        /// <param name="tecnologies"></param>
        /// <param name="type"></param>
        /// <param name="importToTia"></param>
        public void NewRobot(int startAddress, string name, string robSafe, List<string> tecnologies, string type, bool importToTia)
        {
            this.name = name;

            ChangeAddresses(startAddress);

            if (isChangeSymbolic)
                robInfo.StartAddress = startAddress;

            if (robSafe.Equals("Range Monitoring"))
            {
                foreach (var item in robSafeRangeMonitoring[0]) //Outputs
                {
                    robBase[0].Add(item);
                }
                foreach (var item in robSafeRangeMonitoring[1]) //Inputs
                {
                    robBase[1].Add(item);
                }
            }
            else
            {
                foreach (var item in robSafeOperations[0]) //Outputs
                {
                    robBase[0].Add(item);
                }
                foreach (var item in robSafeOperations[1]) //Inputs
                {
                    robBase[1].Add(item);
                }
            }

            int outputIndexRob = -1;
            int inputIndexRob = -1;

            foreach (string tec in tecnologies)
            {
                foreach (var item in robTecnologies[0].Where(t => t.Name.Equals(tec)).ToList())
                {
                    outputIndexRob = robBase[0].FindIndex(a => a.Address.Equals(item.Address));

                    if (outputIndexRob != -1)
                        robBase[0][outputIndexRob] = new RobotBase(item.Symbolic, item.DataType, item.Address, item.Comment);
                }

                foreach (var item in robTecnologies[1].Where(t => t.Name.Equals(tec)).ToList()) //Inputs
                {
                    inputIndexRob = robBase[1].FindIndex(a => a.Address.Equals(item.Address));

                    if (inputIndexRob != -1)
                        robBase[1][inputIndexRob] = new RobotBase(item.Symbolic, item.DataType, item.Address, item.Comment);
                }
            }

            AddPlcDbTags(name);

            if (!Directory.Exists(Path.Combine(SavePath, "To Import Manually")))
                Directory.CreateDirectory(Path.Combine(SavePath, "To Import Manually"));
            string path = Path.Combine(SavePath, "To Import Manually", name + ".xml");
            XmlDocument doc = GenerateTagsXmlDoc();
            XmlParser.IDRenumbering(doc.SelectNodes("/Document/SW.Tags.PlcTagTable//*"));
            doc.Save(path); // Creates file temporarily just to import to Tia Portal automatically

            if (Current != null & importToTia)
            {
                PlcTagTableComposition tagTables = (Current as PlcTagTableSystemGroup).TagTables;

                var tag = tagTables.Find(name);

                if (tag == null)
                    tagTables.Create(name);
                else
                {
                    try
                    {
                        tag.Delete();
                        tagTables.Create(name);
                    }
                    catch (Exception)
                    {
                        //Continue
                    }
                }

                try
                {
                    tagTables.Import(new FileInfo(path), ImportOptions.Override);
                }
                catch (Exception)
                {
                    MessageBox.Show("There was an error importing tags.\nPlease, import manually\n\nPath: " + path, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            // When process finish, deletes the file and creates a new one
            if (File.Exists(path))
                File.Delete(path);

            doc = GenerateManualImportXML();
            doc.Save(path);

            SavePath = Path.Combine(SavePath, name + ".xml");
            GenerateSaveFile(name, startAddress.ToString(), robSafe, type, tecnologies);
        }

        /// <summary>
        /// Add PLC Tags in PLC DB file
        /// </summary>
        private void AddPlcDbTags(string robName)
        {
            Excel.Application xlApp = new Excel.Application();
            Workbook xlWorkBook = OpennessHelper.GetExcelFile(PlcDBPath, xlApp);
            Worksheet xlWorksheet = null;

            if (xlWorkBook == null) return;

            try
            {
                xlWorksheet = xlWorkBook.Sheets["PLC Tags"];
            }
            catch (Exception)
            {
                MessageBox.Show("Could not find sheet named \"PLC Tags\" in file: \"" + PlcDBPath + "\"\n\nPLC DB Tags will not be imported.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                xlWorkBook.Close(0);
                xlApp.Quit();
                return;
            }

            int lastRow = xlWorksheet.Cells.Find("*", System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                        Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlPrevious, false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Row;

            object[,] matrix = OpennessHelper.ExcelToMatrix(xlWorksheet);

            //SEARCH TAGS IN EXCEL AND ADD TO ROBBASE
            for (int row = 1; row <= lastRow; row++)
            {
                if (Convert.ToString(matrix[row, 1]) != null) //If Column 'A' is not Null
                {
                    //Check if name equals Robot Name
                    if (Convert.ToString(matrix[row, 1]).Contains(robName))
                    {
                        string symbolic = Convert.ToString(matrix[row, 1]);
                        string datatype = Convert.ToString(matrix[row, 3]);
                        string address = Convert.ToString(matrix[row, 4]);
                        string comment = Convert.ToString(matrix[row, 5]);

                        RobotBase rb = new RobotBase(symbolic, datatype, address, comment);

                        if (address.Contains('A')) // If startaddress is output
                        {
                            int outputIndexRob = -1;
                            outputIndexRob = robBase[0].FindIndex(a => a.Address.Equals(rb.Address));

                            if (outputIndexRob != -1)
                                robBase[0][outputIndexRob] = rb;
                        } else if (address.Contains('E')) // If startaddress is input
                        {
                            int inputIndexRob = -1;
                            inputIndexRob = robBase[1].FindIndex(a => a.Address.Equals(rb.Address));

                            if (inputIndexRob != -1)
                                robBase[1][inputIndexRob] = rb;
                        }
                    }
                }
            }

            xlWorkBook.Close(0);
            xlApp.Quit();
        }

        /// <summary>
        /// Change Robot Addresses
        /// </summary>
        /// <param name="startAddress"></param>
        private void ChangeAddresses(int startAddress)
        {
            var rBase = new List<List<RobotBase>>();
            var rTecs = new List<List<RobotTecnologie>>();
            var rSafeMonitoring = new List<List<RobotSafeRangeMonitoring>>();
            var rSafeOperation = new List<List<RobotSafeOperation>>();

            var rBaseProperties = new List<RobotBase>();
            var rTecsProperties = new List<RobotTecnologie>();
            var rSafeMonitoringProperties = new List<RobotSafeRangeMonitoring>();
            var rSafeOperationProperties = new List<RobotSafeOperation>();

            #region Change RobBase start addresses
            foreach (var item in robBase[0]) //Outputs
            {
                ChangeAddress(item, startAddress);
                rBaseProperties.Add(item);
            }

            rBase.Add(rBaseProperties);
            rBaseProperties = new List<RobotBase>();

            foreach (var item in robBase[1]) //Inputs
            {
                ChangeAddress(item, startAddress);
                rBaseProperties.Add(item);
            }
            rBase.Add(rBaseProperties);

            robBase = rBase;
            #endregion

            #region Change RobTecnologies start addresses
            foreach (var item in robTecnologies[0]) //Outputs
            {
                ChangeAddress(item, startAddress);
                rTecsProperties.Add(item);
            }

            rTecs.Add(rTecsProperties);
            rTecsProperties = new List<RobotTecnologie>();

            foreach (var item in robTecnologies[1]) //Inputs
            {
                ChangeAddress(item, startAddress);
                rTecsProperties.Add(item);
            }

            rTecs.Add(rTecsProperties);

            robTecnologies = rTecs;
            #endregion

            #region Change RobSafeRangeMonitoring start addresses
            foreach (var item in robSafeRangeMonitoring[0]) //Outputs
            {
                ChangeAddress(item, startAddress);
                rSafeMonitoringProperties.Add(item);
            }

            rSafeMonitoring.Add(rSafeMonitoringProperties);
            rSafeMonitoringProperties = new List<RobotSafeRangeMonitoring>();

            foreach (var item in robSafeRangeMonitoring[1]) //Inputs
            {
                ChangeAddress(item, startAddress);
                rSafeMonitoringProperties.Add(item);
            }

            rSafeMonitoring.Add(rSafeMonitoringProperties);

            robSafeRangeMonitoring = rSafeMonitoring;
            #endregion

            #region Change RobSafeOperation start addresses
            foreach (var item in robSafeOperations[0]) //Outputs
            {
                ChangeAddress(item, startAddress);
                rSafeOperationProperties.Add(item);
            }

            rSafeOperation.Add(rSafeOperationProperties);
            rSafeOperationProperties = new List<RobotSafeOperation>();

            foreach (var item in robSafeOperations[1]) //Inputs
            {
                ChangeAddress(item, startAddress);
                rSafeOperationProperties.Add(item);
            }

            rSafeOperation.Add(rSafeOperationProperties);

            robSafeOperations = rSafeOperation;
            #endregion
        }

        /// <summary>
        /// Change address and name of a single item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="startAddress"></param>
        /// <returns></returns>
        private RobotBase ChangeAddress(RobotBase item, int startAddress)
        {
            int firstNumIndex = item.Address.IndexOfAny("0123456789".ToCharArray());
            double fAddress = double.Parse(item.Address.Substring(firstNumIndex), CultureInfo.InvariantCulture);

            if (isChangeSymbolic)
                fAddress = fAddress - robInfo.StartAddress;

            item.Address = item.Address[0].ToString() + (fAddress + startAddress);
            item.Address = item.Address.Replace('A', 'Q');
            item.Address = item.Address.Replace('E', 'I');
            item.Address = item.Address.Replace(',', '.');
            if (!item.Address.Contains('.'))
                item.Address = item.Address + ".0";
            item.Symbolic = item.Symbolic.Replace("<<BMK>>", name);

            return item;
        }

        #region Generate XML Doc
        /// <summary>
        /// Generate XmlDocument containing created robot tags
        /// </summary>
        /// <returns></returns>
        private XmlDocument GenerateTagsXmlDoc()
        {
            XmlDocument doc = new XmlDocument();
            XmlDocument docWithTag = new XmlDocument();

            doc.Load(workPath + @"\Templates\PLCTAGS\EmptyTagTable.xml");
            XmlParser.ReplaceXML(doc, "ROBNAME", name);

            foreach (var o in robBase[0])   //Outputs
            {
                docWithTag.Load(workPath + @"\Templates\PLCTAGS\Tag.xml");
                XmlParser.InsertTag(doc, docWithTag, o.Symbolic, o.DataType, o.Address, o.Comment, false, false, false);
            }

            foreach (var i in robBase[1])   //Inputs
            {
                docWithTag.Load(workPath + @"\Templates\PLCTAGS\Tag.xml");
                XmlParser.InsertTag(doc, docWithTag, i.Symbolic, i.DataType, i.Address, i.Comment, false, false, false);
            }

            return doc;
        }

        /// <summary>
        /// Generate XmlDocument to import manually into Tia Portal
        /// </summary>
        public XmlDocument GenerateManualImportXML()
        {
            XmlDocument doc = new XmlDocument();

            XElement tagTable = new XElement("Tagtable", new XAttribute("name", name));

            foreach (var o in robBase[0])   //Outputs
            {
                tagTable.Add(XmlParser.CreateTag(o.Symbolic, o.DataType, o.Comment, o.Address));
            }

            foreach (var i in robBase[1])   //Inputs
            {
                tagTable.Add(XmlParser.CreateTag(i.Symbolic, i.DataType, i.Comment, i.Address));
            }

            doc.Load(tagTable.CreateReader());
            XmlNode xmldecl = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.PrependChild(xmldecl);
            return doc;
        }
        #endregion  

        #region Generate XML Save File
        /// <summary>
        /// Generate and save XML file containing created robot info
        /// </summary>
        /// <param name="name"></param>
        /// <param name="startAddress"></param>
        /// <param name="robSafe"></param>
        /// <param name="type"></param>
        /// <param name="tecnologies"></param>
        private void GenerateSaveFile(string name, string startAddress, string robSafe, string type, List<string> tecnologies)
        {
            XDocument Doc = new XDocument();

            XElement Document = new XElement("Document");
            XElement Robot = new XElement("Robot", new XAttribute("name", name), new XAttribute("startaddress", startAddress)
                                                 , new XAttribute("robsafe", robSafe), new XAttribute("type", type));
            XElement Default = new XElement("Default");

            Default.Add(GenerateBase());
            Default.Add(GenerateTecnologies());
            Default.Add(GenerateRobSafe());
            Robot.Add(Default);
            Robot.Add(GenerateSelectedTecnologies(tecnologies));
            Document.Add(Robot);
            Doc.Add(Document);

            if (File.Exists(SavePath))
            {
                if (System.Windows.MessageBox.Show("A file named \"" + name + ".xml" + "\" already exists in path \"" + Path.GetDirectoryName(SavePath) + "\".\n\nDo you want to overwrite it?", "Overwrite", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    File.Delete(SavePath);
                    Doc.Save(SavePath);
                }
            }
            else
                Doc.Save(SavePath);

            MessageBox.Show("File successfully created on path \"" + SavePath + "\"", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Generate XElement with default robbase
        /// </summary>
        /// <returns>XML Default RobBase</returns>
        private XElement GenerateBase()
        {
            XElement rBase = new XElement("Base");

            foreach (var tag in robBase[0])  //Outputs
            {
                rBase.Add(new XElement("Tag", new XAttribute("symbolic", tag.Symbolic), new XAttribute("datatype", tag.DataType)
                                              , new XAttribute("address", tag.Address), new XAttribute("comment", tag.Comment)));
            }

            foreach (var tag in robBase[1])  //Inputs
            {
                rBase.Add(new XElement("Tag", new XAttribute("symbolic", tag.Symbolic), new XAttribute("datatype", tag.DataType)
                                              , new XAttribute("address", tag.Address), new XAttribute("comment", tag.Comment)));
            }

            return rBase;
        }

        /// <summary>
        /// Generate XElement with default tecnologies
        /// </summary>
        /// <returns>XML Default Tecnologies</returns>
        private XElement GenerateTecnologies()
        {
            XElement tecnologies = new XElement("Tecnologies");

            tecnologies.Add(GenerateTecBasicSlave());
            tecnologies.Add(GenerateTecLaserSlave());

            return tecnologies;
        }

        /// <summary>
        /// Generate XElement with default robsafes
        /// </summary>
        /// <returns>XML Default RobSafes</returns>
        private XElement GenerateRobSafe()
        {
            XElement robsafe = new XElement("Robsafe");

            robsafe.Add(GenerateRobSafeMonitoring());
            robsafe.Add(GenerateRobSafeOperation());

            return robsafe;
        }

        /// <summary>
        /// Generate XElement with default basic slave tecnologies
        /// </summary>
        /// <returns>XML Default Tecnologies Basic Slave</returns>
        private XElement GenerateTecBasicSlave()
        {
            XElement robTecBasic = new XElement("Basicslave");

            foreach (var tag in robTecnologies[0].Where(item => item.Type == "Basic Slave"))  //Outputs
            {
                robTecBasic.Add(new XElement("Tag", tag.Name, new XAttribute("fbnumber", tag.FBNumber), new XAttribute("symbolic", tag.Symbolic)
                                                  , new XAttribute("datatype", tag.DataType)
                                                  , new XAttribute("address", tag.Address), new XAttribute("comment", tag.Comment)));
            }

            foreach (var tag in robTecnologies[1].Where(item => item.Type == "Basic Slave"))  //Inputs
            {
                robTecBasic.Add(new XElement("Tag", tag.Name, new XAttribute("fbnumber", tag.FBNumber), new XAttribute("symbolic", tag.Symbolic)
                                                  , new XAttribute("datatype", tag.DataType)
                                                  , new XAttribute("address", tag.Address), new XAttribute("comment", tag.Comment)));
            }

            return robTecBasic;
        }

        /// <summary>
        /// Generate XElement with default laser slave tecnologies
        /// </summary>
        /// <returns>XML Default Tecnologies Laser Slave</returns>
        private XElement GenerateTecLaserSlave()
        {
            XElement robTecLaser = new XElement("Laserslave");

            foreach (var tag in robTecnologies[0].Where(item => item.Type == "Laser Slave"))  //Outputs
            {
                robTecLaser.Add(new XElement("Tag", tag.Name, new XAttribute("fbnumber", tag.FBNumber), new XAttribute("symbolic", tag.Symbolic)
                                                  , new XAttribute("datatype", tag.DataType)
                                                  , new XAttribute("address", tag.Address), new XAttribute("comment", tag.Comment)));
            }

            foreach (var tag in robTecnologies[1].Where(item => item.Type == "Laser Slave"))  //Inputs
            {
                robTecLaser.Add(new XElement("Tag", tag.Name, new XAttribute("fbnumber", tag.FBNumber), new XAttribute("symbolic", tag.Symbolic)
                                                  , new XAttribute("datatype", tag.DataType)
                                                  , new XAttribute("address", tag.Address), new XAttribute("comment", tag.Comment)));
            }

            return robTecLaser;
        }

        /// <summary>
        /// Generate XElement with default robot safe monitoring
        /// </summary>
        /// <returns>XML Default Robot Safe Monitoring</returns>
        private XElement GenerateRobSafeMonitoring()
        {
            XElement robSafeMonitoring = new XElement("Rangemonitoring");

            foreach (var tag in robSafeOperations[0])  //Outputs
            {
                robSafeMonitoring.Add(new XElement("Tag", new XAttribute("symbolic", tag.Symbolic), new XAttribute("datatype", tag.DataType)
                                                  , new XAttribute("address", tag.Address), new XAttribute("comment", tag.Comment)));
            }

            foreach (var tag in robSafeOperations[1])  //Inputs
            {
                robSafeMonitoring.Add(new XElement("Tag", new XAttribute("symbolic", tag.Symbolic), new XAttribute("datatype", tag.DataType)
                                                  , new XAttribute("address", tag.Address), new XAttribute("comment", tag.Comment)));
            }

            return robSafeMonitoring;
        }

        /// <summary>
        /// Generate XElement with default robot safe operation
        /// </summary>
        /// <returns>XML Default Robot Safe Operation</returns>
        private XElement GenerateRobSafeOperation()
        {
            XElement robSafeOperation = new XElement("Operation");

            foreach (var tag in robSafeOperations[0])  //Outputs
            {
                robSafeOperation.Add(new XElement("Tag", new XAttribute("symbolic", tag.Symbolic), new XAttribute("datatype", tag.DataType)
                                                  , new XAttribute("address", tag.Address), new XAttribute("comment", tag.Comment)));
            }

            foreach (var tag in robSafeOperations[1])  //Inputs
            {
                robSafeOperation.Add(new XElement("Tag", new XAttribute("symbolic", tag.Symbolic), new XAttribute("datatype", tag.DataType)
                                                  , new XAttribute("address", tag.Address), new XAttribute("comment", tag.Comment)));
            }

            return robSafeOperation;
        }

        /// <summary>
        /// Generate XElement with selected tecnologies
        /// </summary>
        /// <param name="tecnologies"></param>
        /// <returns>XML Selected Tecnologies</returns>
        private XElement GenerateSelectedTecnologies(List<string> tecnologies)
        {
            XElement selectedTecnologies = new XElement("Tecnologies");

            foreach (string tec in tecnologies)
            {
                selectedTecnologies.Add(new XElement("Tecnologie", tec));
            }

            return selectedTecnologies;
        }
        #endregion
    }
}

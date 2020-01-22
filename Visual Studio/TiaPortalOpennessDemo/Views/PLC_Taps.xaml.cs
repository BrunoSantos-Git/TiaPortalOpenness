using TiaOpennessHelper.ExcelTree;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;
using Color = System.Drawing.Color;
using MessageBox = System.Windows.Forms.MessageBox;
using TiaOpennessHelper;
using Siemens.Engineering;
using Siemens.Engineering.SW.Tags;
using TiaOpennessHelper.XMLParser;
using System.Xml;
using System.Windows.Media;
using TiaOpennessHelper.SafetyMaker;
using System.Reflection;

namespace TiaPortalOpennessDemo.Views
{
    /// <summary>
    /// Interaction logic for PLC_Taps.xaml
    /// </summary>
    public partial class PLC_Taps
    {
        //Events to update cache on MainWindowViewModel
        public event Action<List<object[,]>> MatrixList;
        public event Action<List<string>> SheetNamesList;
        public event Action<object[,]> PlcTagsMatrix;

        public bool IsTiaConnected { get; set; }
        public bool Changes { get; set; }
        private static object[,] matriZ;
        private static DataGridView plcGrid;
        private static string filePath;
        private static List<PLC_Tag> tags;
        private static int id;
        private static List<string> parts;
        private string savePath;
        private static List<string> plcTagsCreated;
        private bool isXmlTag;

        private TiaPortal tiaPortal;
        private Project tiaPortalProject;
        private object current;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path"></param>
        /// <param name="Matriz"></param>
        /// <param name="SavePath"></param>
        /// <param name="tiaPortal"></param>
        /// <param name="tiaPortalProject"></param>
        /// <param name="current"></param>
        public PLC_Taps(string path, object[,] Matriz, string SavePath, TiaPortal tiaPortal, Project tiaPortalProject, object current)
        {
            isXmlTag = false;
            matriZ = Matriz;
            InitializeComponent();
            Init(path, SavePath, tiaPortal, tiaPortalProject, current);
        }

        /// <summary>
        /// Constructor without Matrix
        /// </summary>
        /// <param name="path"></param>
        /// <param name="SavePath"></param>
        /// <param name="tiaPortal"></param>
        /// <param name="tiaPortalProject"></param>
        /// <param name="current"></param>
        public PLC_Taps(string path, string SavePath, TiaPortal tiaPortal, Project tiaPortalProject, object current)
        {
            isXmlTag = true;
            InitializeComponent();
            Init(path, SavePath, tiaPortal, tiaPortalProject, current);
        }

        /// <summary>
        /// Initialize Parts List
        /// </summary>
        private void Init(string path, string SavePath, TiaPortal tiaPortal, Project tiaPortalProject, object current)
        {
            id = 0;
            plcTagsCreated = new List<string>();
            savePath = Path.Combine(SavePath, "PLC Tags");
            Changes = false;
            this.tiaPortal = tiaPortal;
            this.tiaPortalProject = tiaPortalProject;
            this.current = current;

            filePath = path;
            tags = new List<PLC_Tag>();
            parts = new List<string>
            {
                "SD",
                "BH",
                "SB",
                "DT",
                "SC",
                "VR",
                "V0",
                "SF",
                "R0"
            };

            CreateGrid();
            LoadValues();
            WindowFormPlcTags.Child = plcGrid;
            ColorDataGridRows();
        }

        /// <summary>
        /// Creates the left gridView with the wanted settings 
        /// </summary>
        private void CreateGrid()
        {
            plcGrid = new DataGridView();
            plcGrid.Sorted += DataGridView_Sorted;
            plcGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            plcGrid.AllowUserToOrderColumns = false;
            plcGrid.RowHeadersVisible = false;
            plcGrid.GridColor = Color.Black;
            plcGrid.BackgroundColor = ColorTranslator.FromHtml("#3B4461");
            plcGrid.BorderStyle = BorderStyle.None;
            plcGrid.Columns.Add("ID", "*");
            plcGrid.Columns.Add("Name", "Name");
            if(!isXmlTag)
                plcGrid.Columns.Add("Path", "Path");
            plcGrid.Columns.Add("Data Type", "Data Type");
            plcGrid.Columns.Add("Logical Address", "Logical Address");
            plcGrid.Columns.Add("Comment", "Comment");

            DataGridViewCheckBoxColumn cbVisible = new DataGridViewCheckBoxColumn
            {
                HeaderText = "Hmi Visible",
                Name = "Hmi Visible"
            };
            plcGrid.Columns.Add(cbVisible);
            DataGridViewCheckBoxColumn cbAccessible = new DataGridViewCheckBoxColumn
            {
                HeaderText = "Hmi Accessible",
                Name = "Hmi Accessible"
            };
            plcGrid.Columns.Add(cbAccessible);
            DataGridViewCheckBoxColumn cbWritable = new DataGridViewCheckBoxColumn
            {
                HeaderText = "Hmi Writable",
                Name = "Hmi Writable"
            };
            plcGrid.Columns.Add(cbWritable);

            // Align all column header text to middle and change font
            foreach (DataGridViewColumn col in plcGrid.Columns)
            {
                col.HeaderCell.Style.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, GraphicsUnit.Pixel);
                col.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }

            plcGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            plcGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            plcGrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            if (!SystemInformation.TerminalServerSession)
            {
                Type dgvType = plcGrid.GetType();
                PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                pi.SetValue(plcGrid, true, null);
            }
        }

        /// <summary>
        /// Retrieves the values of the datagridview
        /// </summary>
        private void RetrieveValues()
        {
            plcGrid.CurrentCell = null;
            tags = new List<PLC_Tag>();
            for (int z = 0; z < plcGrid.Rows.Count - 1; z++)
            {
                DataGridViewRow dgvr = plcGrid.Rows[z];
                PLC_Tag plcT = new PLC_Tag()
                {
                    Name = dgvr.Cells["Name"].Value.ToString().Replace(" ", string.Empty),
                    DataType = dgvr.Cells["Data Type"].Value.ToString().Replace(" ", string.Empty),
                    Address = dgvr.Cells["Logical Address"].Value.ToString().Replace(" ", string.Empty),
                    Comment = dgvr.Cells["Comment"].Value.ToString(),
                    Visible = (bool)dgvr.Cells["Hmi Visible"].Value,
                    Accessible = (bool)dgvr.Cells["Hmi Accessible"].Value,
                    Writable = (bool)dgvr.Cells["Hmi Writable"].Value
                };

                if (!isXmlTag)
                    plcT.Symbols = dgvr.Cells["Path"].Value.ToString();

                tags.Add(plcT);
            }
        }

        /// <summary>
        /// Function used to Load the values into the grid views
        /// </summary>
        private void LoadValues()
        {
            List<DataGridViewRow> rows = new List<DataGridViewRow>();

            if (!isXmlTag)
                ExcelManager.PLC_Tags(matriZ);
            else
            {
                try
                {
                    XmlParser.XmlToPlcTags(filePath);
                } 
                catch(Exception e)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                btnSave.IsEnabled = false;
            }

            if (DBMaker.PLC_Tags.Count != 0)
            {
                plcGrid.Rows.Clear();
                int counter = 1;
                foreach (PLC_Tag plc in DBMaker.PLC_Tags)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(plcGrid);
                    row.Cells[0].Value = counter++;
                    row.Cells[1].Value = plc.Name;
                    if (isXmlTag)
                    {
                        row.Cells[2].Value = plc.DataType;
                        row.Cells[3].Value = plc.Address;
                        row.Cells[4].Value = plc.Comment;
                        row.Cells[5].Value = plc.Visible;
                        row.Cells[6].Value = plc.Accessible;
                        row.Cells[7].Value = plc.Writable;
                    }
                    else
                    {
                        row.Cells[2].Value = plc.Symbols;
                        row.Cells[3].Value = plc.DataType;
                        row.Cells[4].Value = plc.Address;
                        row.Cells[5].Value = plc.Comment;
                        row.Cells[6].Value = plc.Visible;
                        row.Cells[7].Value = plc.Accessible;
                        row.Cells[8].Value = plc.Writable;
                    }
                    rows.Add(row);
                    //PlcGrid.Rows.Add(counter++, plc.Name, plc.Symbols, plc.DataType, plc.Address, plc.Comment, plc.Visible, plc.Accessible, plc.Writable);
                }
            }
            plcGrid.Rows.AddRange(rows.ToArray());
            plcGrid.Refresh();
        }

        #region Button Actions
        /// <summary>
        /// Saves the Datagridview values on the Excel file
        /// </summary>
        private void Save(object sender, RoutedEventArgs e)
        {
            WriteSavingLabelText("Saving...");

            if (ExcelManager.IsOpened(filePath))
            {
                MessageBox.Show("Close the current workbook before saving", "Close Workbook", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                RetrieveValues();

                if (filePath.Contains(".xlsx") || filePath.Contains(".xlsm") || filePath.Contains(".xltx") || filePath.Contains(".xltm"))
                {
                    Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                    Workbook xlWorkbook = xlApp.Workbooks.Open(filePath);
                    xlApp.DisplayAlerts = false;
                    int row = 2;
                    Worksheet ws = null;
                    object[,] matrix;
                    var matrixs = new List<object[,]>();
                    var sheetNames = new List<string>();
                    if (tags.Count > 0)
                    {
                        foreach (Worksheet sheet in xlWorkbook.Worksheets)
                        {
                            switch (sheet.Name.ToLower())
                            {
                                case "rel notes":
                                case "export db":
                                    break;
                                case "plc tags":
                                    ws = sheet;
                                    object[,] PlcTagsSheet = OpennessHelper.ExcelToMatrix(ws);

                                    while (ws.Cells[row, 1].Value != null)
                                    {
                                        ws.Cells[row, 1].Value = null;
                                        ws.Cells[row, 2].Value = null;
                                        ws.Cells[row, 3].Value = null;
                                        ws.Cells[row, 4].Value = null;
                                        ws.Cells[row, 5].Value = null;
                                        ws.Cells[row, 6].Value = null;
                                        ws.Cells[row, 7].Value = null;

                                        if(!isXmlTag)
                                            ws.Cells[row, 8].Value = null;

                                        row += 1;
                                    }

                                    row = 2;

                                    foreach (PLC_Tag tag in tags)
                                    {
                                        ws.Cells[row, 1].Value = tag.Name;
                                        PlcTagsSheet[row, 1] = tag.Name;
                                        if (!isXmlTag)
                                        {
                                            ws.Cells[row, 2].Value = tag.Symbols;
                                            ws.Cells[row, 3].Value = tag.DataType;
                                            ws.Cells[row, 4].Value = tag.Address;
                                            ws.Cells[row, 5].Value = tag.Comment;
                                            ws.Cells[row, 6].Value = tag.Visible;
                                            ws.Cells[row, 7].Value = tag.Accessible;
                                            ws.Cells[row, 8].Value = tag.Writable;

                                            PlcTagsSheet[row, 2] = tag.Symbols;
                                            PlcTagsSheet[row, 3] = tag.DataType;
                                            PlcTagsSheet[row, 4] = tag.Address;
                                            PlcTagsSheet[row, 5] = tag.Comment;
                                            PlcTagsSheet[row, 6] = tag.Visible;
                                            PlcTagsSheet[row, 7] = tag.Accessible;
                                            PlcTagsSheet[row, 8] = tag.Writable;
                                        }
                                        else
                                        {
                                            ws.Cells[row, 2].Value = tag.DataType;
                                            ws.Cells[row, 3].Value = tag.Address;
                                            ws.Cells[row, 4].Value = tag.Comment;
                                            ws.Cells[row, 5].Value = tag.Visible;
                                            ws.Cells[row, 6].Value = tag.Accessible;
                                            ws.Cells[row, 7].Value = tag.Writable;

                                            PlcTagsSheet[row, 2] = tag.DataType;
                                            PlcTagsSheet[row, 3] = tag.Address;
                                            PlcTagsSheet[row, 4] = tag.Comment;
                                            PlcTagsSheet[row, 5] = tag.Visible;
                                            PlcTagsSheet[row, 6] = tag.Accessible;
                                            PlcTagsSheet[row, 7] = tag.Writable;
                                        }

                                        row += 1;
                                    }

                                    matrixs.Add(PlcTagsSheet);
                                    PlcTagsMatrix(PlcTagsSheet);
                                    sheetNames.Add(sheet.Name);
                                    break;
                                default:
                                    matrix = OpennessHelper.ExcelToMatrix(sheet);
                                    matrixs.Add(matrix);
                                    sheetNames.Add(sheet.Name);
                                    break;
                            }
                        }

                        MatrixList(matrixs);
                        SheetNamesList(sheetNames);

                        if (ws == null)
                        {
                            MessageBox.Show("This Excel does not contain a usable worksheet", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    xlWorkbook.Save();
                    xlWorkbook.Close(0);
                    xlApp.Quit();
                }
            }

            WriteSavingLabelText("");
        }

        /// <summary>
        /// Clears the Datagridview values
        /// </summary>
        private void Clear(object sender, RoutedEventArgs e)
        {
            plcGrid.Rows.Clear();
        }

        /// <summary>
        /// Creates the xml Version of the plc tags
        /// </summary>
        private void CreateXML(object sender, RoutedEventArgs e)
        {
            WriteSavingLabelText("Creating XML...");

            RetrieveValues();
            string currentSta = "";
            string currentComp = "";
            string plcNum = GetPlcNum();
            string savePathPlcNum = Path.Combine(savePath, plcNum);
            List<string> Stations = new List<string>();
            List<string> Components = new List<string>();
            plcTagsCreated = new List<string>();
            bool importToTia = (bool)cbImportToTia.IsChecked;
            
            if (!isXmlTag)
            {
                id = 0;
                XMLPLCTagsThemePlate();

                foreach (PLC_Tag tag in tags)
                {
                    if (tag.Name[0] == '/')
                    {
                        if (plcNum == tag.Name[1].ToString() && !Char.IsDigit(tag.Name[2]))
                        {
                            CreateTag(tag);
                        }
                    }
                    else
                    {
                        if (plcNum == tag.Name[0].ToString() && !Char.IsDigit(tag.Name[1]))
                        {
                            CreateTag(tag);
                        }
                    }
                }

                if (tags[0].Name[0] == '/')
                {
                    CreateTagXML(tags[0].Name[1].ToString(), savePathPlcNum);
                }
                else
                {
                    CreateTagXML(tags[0].Name[0].ToString(), savePathPlcNum);
                }

                //bool found;

                //Station
                //Solo
                foreach (PLC_Tag tag in tags)
                {
                    //found = false;

                    if (tag.Name[0] == '/')
                    {
                        if (ExcelManager.IsDigitsOnly(tag.Name[1].ToString() + tag.Name[2].ToString()))
                        {
                            currentSta = tag.Name[1].ToString() + tag.Name[2].ToString();
                        }
                    }
                    else
                    {
                        if (ExcelManager.IsDigitsOnly(tag.Name[0].ToString() + tag.Name[1].ToString()))
                        {
                            currentSta = tag.Name[0].ToString() + tag.Name[1].ToString();
                        }
                    }

                    if (currentSta != "" && !Stations.Contains(currentSta))
                    {
                        Stations.Add(currentSta);
                    }
                }

                foreach (string s in Stations)
                {
                    id = 0;
                    XMLPLCTagsThemePlate();
                    foreach (PLC_Tag tag in tags)
                    {
                        if (!tag.Name.Contains("IG0K") && !tag.Name.Contains("IG1K") && !tag.Name.Contains("IG2K") && !tag.Name.Contains("IG3K") && !tag.Name.Contains("IG4K") 
                           && !tag.Name.Contains("IG5K") && !tag.Name.Contains("IG6K") && !tag.Name.Contains("IG7K") && !tag.Name.Contains("IG8K") && !tag.Name.Contains("IG9K"))
                        {
                            if (tag.Name[0] == '/')
                            {
                                if (tag.Name.Substring(1, 2) == s && !Char.IsDigit(tag.Name[3]))
                                {
                                    CreateTag(tag);
                                }
                            }
                            else
                            {
                                if (tag.Name.Substring(0, 2) == s && !Char.IsDigit(tag.Name[2]))
                                {
                                    CreateTag(tag);
                                }
                            }
                        }
                    }

                    CreateTagXML(s.ToString(), savePathPlcNum);
                }

                //IG_K
                foreach (string s in Stations)
                {
                    List<string> s_IG_K = new List<string>();
                    string currentIG_K = "";

                    for (int z = 0; z <= 9; z++)
                    {
                        //found = false;
                        foreach (PLC_Tag tag in tags)
                        {
                            if (tag.Name[0] == '/')
                            {
                                if ((tag.Name[1].ToString() + tag.Name[2].ToString()) == s && tag.Name.Substring(3, 4) == "IG" + z + "K")
                                {
                                    currentIG_K = "IG" + z + "K";
                                }
                            }
                            else
                            {
                                if ((tag.Name[0].ToString() + tag.Name[1].ToString()) == s && tag.Name.Substring(2, 4) == "IG" + z + "K")
                                {
                                    currentIG_K = "IG" + z + "K";
                                }
                            }
                        }

                        if (currentIG_K != "" && !s_IG_K.Contains(currentIG_K))
                        {
                            s_IG_K.Add(currentIG_K);
                        }
                    }

                    foreach (string sX in s_IG_K)
                    {
                        id = 0;
                        XMLPLCTagsThemePlate();
                        foreach (PLC_Tag tag in tags)
                        {
                            if (tag.Name.Contains(sX))
                            {
                                if (tag.Name[0] == '/')
                                {
                                    if (tag.Name.Substring(1, 2) == s && !Char.IsDigit(tag.Name[3]))
                                    {
                                        CreateTag(tag);
                                    }
                                }
                                else
                                {
                                    if (tag.Name.Substring(0, 2) == s && !Char.IsDigit(tag.Name[2]))
                                    {
                                        CreateTag(tag);
                                    }
                                }
                            }
                        }
                        CreateTagXML(s + sX, savePathPlcNum);
                    }
                }

                //Components
                foreach (PLC_Tag tag in tags)
                {
                    //found = false;

                    if (tag.Name.Count() > 6)
                    {
                        if (tag.Name[0] == '/')
                        {
                            if (ExcelManager.IsDigitsOnly(tag.Name.Substring(1, 6)))
                            {
                                currentComp = tag.Name.Substring(1, 6);
                            }
                        }
                        else
                        {
                            if (ExcelManager.IsDigitsOnly(tag.Name.Substring(0, 6)))
                            {
                                currentComp = tag.Name.Substring(0, 6);
                            }
                        }

                        if (currentComp != "" && !Components.Contains(currentComp))
                        {
                            Components.Add(currentComp);
                        }
                    }
                }

                foreach (string s in Components)
                {
                    id = 0;

                    XMLPLCTagsThemePlate();

                    foreach (PLC_Tag tag in tags)
                    {
                        if (tag.Name[0] == '/')
                        {
                            if (tag.Name.Substring(1, 6) == s && AllCharDigits(tag.Name) == true)
                            {
                                CreateTag(tag);
                            }
                        }
                        else
                        {
                            if (tag.Name.Substring(0, 6) == s && AllCharDigits(tag.Name) == true)
                            {
                                CreateTag(tag);
                            }
                        }
                    }

                    CreateTagXML(s.ToString(), savePathPlcNum);
                }

                //Rest
                foreach (string component in Components)
                {
                    foreach (string p in parts)
                    {
                        List<string> part = new List<string>();
                        string currentIpart = "";

                        for (int z = 0; z <= 9; z++)
                        {
                            //found = false;
                            foreach (PLC_Tag tag in tags)
                            {
                                if (tag.Name[0] == '/')
                                {
                                    if (tag.Name.Substring(1, 6) == component && tag.Name.Contains(p + z))
                                    {
                                        currentIpart = p + z;
                                    }
                                }
                                else
                                {
                                    if (tag.Name.Substring(0, 6) == component && tag.Name.Contains(p + z))
                                    {
                                        currentIpart = p + z;
                                    }
                                }
                            }

                            if (currentIpart != "" && !part.Contains(currentIpart))
                            {
                                part.Add(currentIpart);
                            }
                        }

                        foreach (string sXX in part)
                        {
                            id = 0;

                            XMLPLCTagsThemePlate();

                            foreach (PLC_Tag tag in tags)
                            {
                                if (tag.Name[0] == '/')
                                {
                                    if (tag.Name.Substring(1, 6) == component && tag.Name.Contains(sXX))
                                    {
                                        CreateTag(tag);
                                    }
                                }
                                else
                                {
                                    if (tag.Name.Substring(0, 6) == component && tag.Name.Contains(sXX))
                                    {
                                        CreateTag(tag);
                                    }
                                }
                            }

                            CreateTagXML(component + sXX, savePathPlcNum);
                        }
                    }
                }

                if (importToTia)
                {
                    #region Import Tags to TIA
                    plcTagsCreated = plcTagsCreated.Distinct().ToList();
                    if (current != null)
                    {
                        var notImportedTags = new List<string>();
                        var blocksDirectory = new DirectoryInfo(savePathPlcNum);
                        using (var access = tiaPortal.ExclusiveAccess("Importing tags"))
                        {
                            foreach (var file in blocksDirectory.GetFiles())
                            {
                                string tagName = Path.GetFileNameWithoutExtension(file.FullName);
                                if (plcTagsCreated.Contains(tagName))
                                {
                                    PlcTagTableComposition tagTables = (current as PlcTagTableSystemGroup).TagTables;

                                    var tagTable = tagTables.Find(tagName);

                                    if (tagTable == null)
                                        (current as PlcTagTableSystemGroup).TagTables.Create(tagName);

                                    try
                                    {
                                        tagTables.Import(new FileInfo(file.FullName), ImportOptions.Override);
                                    }
                                    catch (Exception)
                                    {
                                        notImportedTags.Add(tagName);
                                    }
                                }
                            }
                        }

                        if (notImportedTags.Any())
                        {
                            string error = "An error occurred importing these PLC Tags: \n\n";
                            foreach (string t in notImportedTags)
                            {
                                error += "- " + t + "\n";
                            }
                            error += "\nPlease, import them manually.\n\n";
                            error += "Path: " + Path.Combine(savePathPlcNum, "To Import Manually");
                            MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    #endregion
                }
            }
            else
            {
                var workingDirectory = Environment.CurrentDirectory;
                var workPath = Directory.GetParent(workingDirectory).FullName;
                MessageBox.Show(workPath);
                XmlDocument emptyTagTableDoc = new XmlDocument();
                XmlDocument tagDoc = new XmlDocument();
                string fileName = Path.GetFileNameWithoutExtension(filePath);

                emptyTagTableDoc.Load(workPath + @"\Templates\PLCTAGS\EmptyTagTable.xml");
                XmlParser.ReplaceXML(emptyTagTableDoc, "ROBNAME", fileName);

                foreach (var tag in tags)
                {
                    tagDoc.Load(workPath + @"\Templates\PLCTAGS\Tag.xml");
                    XmlParser.InsertTag(emptyTagTableDoc, tagDoc, tag.Name, tag.DataType, tag.Address.Replace("%", string.Empty), tag.Comment, tag.Accessible, tag.Visible, tag.Writable);
                }

                XmlParser.IDRenumbering(emptyTagTableDoc.SelectNodes("/Document/SW.Tags.PlcTagTable//*"));
                File.Delete(filePath);
                emptyTagTableDoc.Save(filePath);

                #region Create XML Document to import manually
                XmlDocument doc = new XmlDocument();
                XElement xTagTable = new XElement("Tagtable", new XAttribute("name", fileName));
                foreach (var tag in tags)
                {
                    xTagTable.Add(XmlParser.CreateTag(tag.Name, tag.DataType, tag.Comment, tag.Address.Replace("%", string.Empty)));
                }
                doc.Load(xTagTable.CreateReader());
                XmlNode xmldecl = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.PrependChild(xmldecl);
                Directory.CreateDirectory(Path.Combine(savePath, "To Import"));
                doc.Save(Path.Combine(savePath, "To Import", fileName + ".xml"));
                #endregion

                if (importToTia)
                {
                    PlcTagTableComposition tagTables = (current as PlcTagTableSystemGroup).TagTables;
                    if (tagTables == null) return;

                    bool isCreate = tagTables.Find(fileName) != null;

                    if (!isCreate)
                        tagTables.Create(fileName);

                    try
                    {
                        tagTables.Import(new FileInfo(filePath), ImportOptions.Override);
                    }
                    catch (Exception)
                    {
                        System.Windows.MessageBox.Show("An error occured while importing tag table.\nPlease, import manually.\n\nPath: " + Path.Combine(savePathPlcNum, "To Import Manually"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            Changes = true;
            WriteSavingLabelText("");
        }
        #endregion

        /// <summary>
        /// Generates the ThemePlate of the PlcTags
        /// </summary>
        private void XMLPLCTagsThemePlate()
        {
            new XDocument(
                    new XElement("Document"
                        , new XElement("Engineering", new XAttribute("version", "V15"))
                    , new XElement("DocumentInfo"
                        , new XElement("Created", DateTime.Today.ToString("yyyy-MM-dd") + "T" + DateTime.Now.ToString("HH:mm:ss tt"))
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
                                , new XElement("DisplayVersion", "V15"))
                            , new XElement("OptionPackage"
                                , new XElement("DisplayName", "STEP 7 Safety")
                                , new XElement("DisplayVersion", "V15"))
                            , new XElement("Product"
                                , new XElement("DisplayName", "WinCC Professional")
                                , new XElement("DisplayVersion", "V15"))
                            , new XElement("OptionPackage"
                                , new XElement("DisplayName", "SIMATIC Visualization Architect")
                                , new XElement("DisplayVersion", "V15"))))
                    , new XElement("SW.Tags.PlcTagTable", new XAttribute("ID", "0")
                        , new XElement("AttributeList"
                            , new XElement("Name", "ThemePlate")
                                       )
                        , new XElement("ObjectList")
                                   ))
                          ).Save("C:/Temp/TagsThemePlate.xml");
        }

        /// <summary>
        /// Inserts a tag into the Plc ThemePlate
        /// </summary>
        /// <param name="Tag">Tag that will e used</param>
        public static void CreateTag(PLC_Tag tag)
        {
            id += 1;

            XDocument xmlDocTag = XDocument.Load("C:/Temp/TagsThemePlate.xml");
            var elementObjectList = xmlDocTag.XPathSelectElement("/Document/SW.Tags.PlcTagTable[@ID='0']/ObjectList");

            XElement vari = new XElement("SW.Tags.PlcTag", new XAttribute("ID", id), new XAttribute("CompositionName", "Tags")
                                , new XElement("AttributeList"
                                    , new XElement("DataTypeName", tag.DataType)
                                    , new XElement("ExternalAccessible", tag.Accessible)
                                    , new XElement("ExternalVisible", tag.Visible)
                                    , new XElement("ExternalWritable", tag.Writable)
                                    , new XElement("LogicalAddress", tag.Address)
                                    , new XElement("Name", tag.Name))
                                , new XElement("ObjectList"
                                    , new XElement("MultilingualText", new XAttribute("ID", id + 1), new XAttribute("CompositionName", "Comment")
                                        , new XElement("ObjectList"
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", id + 2), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "de-DE")
                                                    , new XElement("Text", tag.Comment)))
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", id + 3), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "en-US")
                                                    , new XElement("Text")))
                                             , new XElement("MultilingualTextItem", new XAttribute("ID", id + 4), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "es-ES")
                                                    , new XElement("Text")))
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", id + 5), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "fr-FR")
                                                    , new XElement("Text")))
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", id + 6), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "zh-CN")
                                                    , new XElement("Text")))
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", id + 7), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "nl-BE")
                                                    , new XElement("Text")))
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", id + 8), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "pl-PL")
                                                    , new XElement("Text")))
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", id + 9), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "pt-BR")
                                                    , new XElement("Text")))
                                             , new XElement("MultilingualTextItem", new XAttribute("ID", id + 10), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "ru-RU")
                                                    , new XElement("Text")))
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", id + 11), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "sk-SK")
                                                    , new XElement("Text")))
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", id + 12), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "cs-CZ")
                                                    , new XElement("Text")))
                                            , new XElement("MultilingualTextItem", new XAttribute("ID", id + 13), new XAttribute("CompositionName", "Items")
                                                , new XElement("AttributeList"
                                                    , new XElement("Culture", "hu-HU")
                                                    , new XElement("Text")))))));
            elementObjectList.Add(vari);
            xmlDocTag.Save("C:/Temp/TagsThemePlate.xml");
            id += 14;
        }

        /// <summary>
        /// Renames the PLc Xml
        /// </summary>
        /// <param name="name">New Name</param>
        /// <param name="path">Save path</param>
        public void CreateTagXML(string name, string path)
        {
            XDocument xmlDocBD = XDocument.Load("C:/Temp/TagsThemePlate.xml");
            var elementName = xmlDocBD.XPathSelectElement("/Document/SW.Tags.PlcTagTable[@ID='0']/AttributeList/Name");
            elementName.Value = name;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (HasPlcTags(xmlDocBD))
            {
                xmlDocBD.Save(Path.Combine(path, name + ".xml"));
                plcTagsCreated.Add(name);
                GenerateImportManTag(name, path, xmlDocBD);
            }

            File.Delete("C:/Temp/TagsThemePlate.xml");
        }

        /// <summary>
        /// Check if xml tag has tags created on it
        /// </summary>
        /// <param name="doc">XML Document</param>
        private bool HasPlcTags(XDocument doc)
        {
            var tags = doc.Descendants("SW.Tags.PlcTag");

            if (tags.Any()) 
                return true;
            else 
                return false;
        }

        /// <summary>
        /// Generate tag to import manually
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="path"></param>
        /// <param name="tagsDoc"></param>
        private void GenerateImportManTag(string tagName, string path, XDocument tagsDoc)
        {
            // Create XML Document to import manually
            XmlDocument xDoc = new XmlDocument();
            XElement xTagTable = new XElement("Tagtable", new XAttribute("name", tagName));
            foreach (var tag in tagsDoc.Descendants("SW.Tags.PlcTag"))
            {
                string name = tag.Descendants("Name").First()?.Value ?? "";
                string dataType = tag.Descendants("DataTypeName").First()?.Value ?? "";
                if (dataType != "")
                    dataType = char.ToUpper(dataType[0]) + dataType.Substring(1).ToLower();
                string comment = tag.Descendants().Where(x => x.Value == "de-DE").Select(x => x).First().Parent.Descendants("Text").First()?.Value ?? "";
                string address = tag.Descendants("LogicalAddress").First()?.Value ?? "";

                xTagTable.Add(XmlParser.CreateTag(name, dataType, comment, address.Replace("%", string.Empty)));
            }
            xDoc.Load(xTagTable.CreateReader());
            XmlNode xmldecl = xDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xDoc.PrependChild(xmldecl);
            Directory.CreateDirectory(Path.Combine(path, "To Import Manually"));
            xDoc.Save(Path.Combine(path, "To Import Manually", tagName + ".xml"));
        }

        /// <summary>
        /// Verifies if all the chars in a string are digits
        /// </summary>
        /// <param name="tag">string that is gonna be used</param>
        public static bool AllCharDigits(string tag)
        {
            bool state = true;

            foreach (string s in parts)
            {
                for (int z = 1; z <= 9; z++)
                {
                    if (tag.Contains(s + z))
                    {
                        state = false;
                    }
                }
            }

            return state;
        }

        /// <summary>
        /// Color datagridview rows
        /// </summary>
        private void ColorDataGridRows()
        {
            int rowNumber = 1;
            foreach (DataGridViewRow row in ((DataGridView)WindowFormPlcTags.Child).Rows)
            {
                if ((rowNumber % 2) == 0) row.DefaultCellStyle.BackColor = Color.LightGray;
                else row.DefaultCellStyle.BackColor = Color.White;
                rowNumber++;
            }
        }

        /// <summary>
        /// Handles window loaded event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Check if Tia Portal is connected
            if (current == null)
            {
                IsTiaConnected = false;
                cbImportToTia.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#D3D3D3");
            }
            else
                IsTiaConnected = true;

            cbImportToTia.DataContext = this;
        }

        /// <summary>
        /// Column sort mode changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView_Sorted(object sender, EventArgs e)
        {
            ColorDataGridRows();
        }
    
        /// <summary>
        /// Write text on label "Saving"
        /// </summary>
        /// <param name="text"></param>
        private void WriteSavingLabelText(string text)
        {
            Saving.Text = text;
            Saving.Update();
        }
    
        /// <summary>
        /// Get PLCNum from datagridview tags
        /// </summary>
        /// <returns></returns>
        private string GetPlcNum()
        {
            string plcNum = "";

            foreach (PLC_Tag tag in tags)
            {
                if (char.IsDigit(tag.Name[0]))  // If first char of name is a digit
                {
                    plcNum = tag.Name[0].ToString(); // It is the PLC Num
                    break;
                }
            }
            return plcNum;
        }
    }
}

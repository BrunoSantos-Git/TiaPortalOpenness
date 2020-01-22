using TiaOpennessHelper.ExcelTree;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using Color = System.Drawing.Color;
using Microsoft.Office.Interop.Excel;
using Window = System.Windows.Window;
using Siemens.Engineering;
using System.Linq;
using System.Windows.Media;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Threading;

namespace TiaOpennessHelper.SafetyMaker
{
    /// <summary>
    /// Interaction logic for DBMaker.xaml
    /// </summary>
    public partial class DBMaker : Window
    {
        //Events to update cache on MainWindowViewModel
        public static event Action<List<object[,]>> MatrixList;
        public static event Action<List<string>> SheetNamesList;

        public static string SequenceListPath { get; set; }
        public static string SchnittstelleListPath { get; set; }
        public static bool IsTiaConnected { get; set; }
        public static List<PLC_Tag> PLC_Tags { get; set; }
        public bool Changes { get; set; }
        public static List<UserConfig> UserConfigs;
        public static List<EngAssist> EngValues;
        public static List<Variable> Variables;
        public static List<ReplaceActions> SPSActions;
        public static List<ReplaceActions> SCHActions;
        public static List<ReplaceActions> SAFActions;
        public static List<ReplaceActions> STAActions;
        public static List<string> BlocksCreated;
        public static List<string> NameS;
        public static List<object[,]> WorkSheetS;
        public int sheetCounterX;

        private static int columnNumber;
        private static int maxColumns;
        private static string filePath;
        private static string savePath;
        private static DataGridView gridViewRight;
        private static DataGridView gridViewLeft;
        private static List<EngAssist> newEngValues;
        private TiaPortal tiaPortal;
        private Project tiaPortalProject;
        private object current;

        //Constructors
        /// <summary>
        /// DBMaker Initializer With Parameters
        /// </summary>
        /// <param name="Names"></param>
        /// <param name="WorkSheets"></param>
        /// <param name="path"></param>
        /// <param name="SavePath"></param>
        /// <param name="tiaPortal"></param>
        /// <param name="tiaPortalProject"></param>
        /// <param name="current"></param>
        public DBMaker(List<string> Names, List<object[,]> WorkSheets, string path, string SavePath, TiaPortal tiaPortal, Project tiaPortalProject, object current)
        {
            InitializeComponent();
            Init();

            foreach (string s in Names)
            {
                if (s == "EngAssist")
                {
                    LoadValues(WorkSheets[sheetCounterX]);
                    break;
                }
                sheetCounterX += 1;
            }

            DBMaker.savePath = SavePath;
            NameS = Names;
            WorkSheetS = WorkSheets;
            filePath = path;
            this.tiaPortal = tiaPortal;
            this.tiaPortalProject = tiaPortalProject;
            this.current = current;
        }

        /// <summary>
        /// DBMaker inicializer
        /// </summary>
        public DBMaker()
        {
            InitializeComponent();
            Init();
            LoadValues();
        }

        /// <summary>
        /// Initialize Components
        /// </summary>
        private void Init()
        {
            newEngValues = new List<EngAssist>();
            PLC_Tags = new List<PLC_Tag>();
            UserConfigs = new List<UserConfig>();
            EngValues = new List<EngAssist>();
            Variables = new List<Variable>();
            SPSActions = new List<ReplaceActions>();
            SCHActions = new List<ReplaceActions>();
            SAFActions = new List<ReplaceActions>();
            STAActions = new List<ReplaceActions>();
            BlocksCreated = new List<string>();
            gridViewRight = new DataGridView();
            gridViewLeft = new DataGridView();
            Changes = false;

            CreateLeftGrid();
            CreateRightGrid();

            sheetCounterX = 0;
            columnNumber = 1;
            maxColumns = 0;

            WindowsForm.Child = gridViewRight;
            WindowsForm_Left.Child = gridViewLeft;
            gridViewRight.CurrentCell = gridViewRight[1, 1];
        }

        /// <summary>
        /// Creates left gridView
        /// </summary>
        private void CreateLeftGrid()
        {
            gridViewLeft = new DataGridView
            {
                AllowUserToOrderColumns = false,
                RowHeadersVisible = false,
                AllowUserToResizeRows = false,
                AllowUserToAddRows = false,
                GridColor = Color.Black,
                BackgroundColor = ColorTranslator.FromHtml("#3B4461"),
                BorderStyle = BorderStyle.None
            };
            gridViewLeft.RowTemplate.Height = 40;

            gridViewLeft.Columns.Add("Arbeitsgruppe [ARG]", "Arbeitsgruppe [ARG]");
            gridViewLeft.Columns.Add("Schutzkreis [SK]", "Schutzkreis [SK]");
            gridViewLeft.Columns.Add("Station", "Station");
            gridViewLeft.Columns.Add("Erw.Stationsbez. [SBZ]", "Erw. Stationsbez. [SBZ]");

            DisableSort(gridViewLeft);
            InsertCommonRowOfTheLeftGrid();
            
            gridViewLeft.CellEndEdit += AddNewRows;

            // Align all column header text to middle and change font
            foreach (DataGridViewColumn col in gridViewLeft.Columns)
            {
                col.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                col.HeaderCell.Style.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, GraphicsUnit.Pixel);
            }
            gridViewLeft.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //gridViewLeft.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            gridViewLeft.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            gridViewLeft.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridViewLeft.ColumnHeadersHeight = 40;
            gridViewLeft.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            if (!SystemInformation.TerminalServerSession)
            {
                Type dgvType = gridViewLeft.GetType();
                PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                pi.SetValue(gridViewLeft, true, null);
            }
        }

        /// <summary>
        /// Creates right gridView
        /// </summary>
        private void CreateRightGrid()
        {
            gridViewRight = new DataGridView
            {
                AllowUserToOrderColumns = false,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                AllowUserToResizeRows = false,
                GridColor = Color.Black,
                BackgroundColor = ColorTranslator.FromHtml("#3B4461"),
                BorderStyle = BorderStyle.None
            };
            gridViewRight.RowTemplate.Height = 20;
            gridViewRight.Columns.Add(columnNumber.ToString(), "");
            InsertColumns(1);
            gridViewRight.Columns[0].ReadOnly = true;
            gridViewRight.RowEnter += PartAndValvesFiller;
            gridViewRight.CellEndEdit += ColumnManager;

            DisableSort(gridViewRight);
            InsertCommonRowsOfTheRightGrid();

            // Align all column header text to middle and change font
            foreach (DataGridViewColumn col in gridViewRight.Columns)
            {
                col.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                col.HeaderCell.Style.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, GraphicsUnit.Pixel);
            }
            gridViewRight.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            gridViewRight.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //gridViewRight.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            gridViewRight.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridViewRight.ColumnHeadersHeight = 40;
            gridViewRight.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            if (!SystemInformation.TerminalServerSession)
            {
                Type dgvType = gridViewRight.GetType();
                PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                pi.SetValue(gridViewRight, true, null);
            }
        }

        #region Button Events
        /// <summary>
        /// Button Event that creates the data base of SPS, Schutzkreis and Safety  
        /// </summary>
        private void Button_CreateDB(object sender, RoutedEventArgs e)
        {
            WriteSavingLabelText("Creating DB's...");

            BlocksCreated = new List<string>();
            bool importToTia = (bool)cbImportToTia.IsChecked;

            EngValues = new List<EngAssist>();
            RetrieveValues();
            EngValues = newEngValues;

            NetworkDBMaker.GenerateDataBaseThemePlate();

            sheetCounterX = 0;

            foreach (string s in NameS)
            {
                if (s == "PLC Tags")
                    ExcelManager.PLC_Tags(WorkSheetS[sheetCounterX]);

                if (s == "User Config")
                    ExcelManager.EngConfig(WorkSheetS[sheetCounterX]);

                sheetCounterX += 1;
            }

            sheetCounterX = 0;

            foreach (string s in NameS)
            {
                switch (s)
                {
                    case "SPS":
                        ExcelManager.SPS(WorkSheetS[sheetCounterX]);
                        NetworkDBMaker.PopulateDB(Variables, "SPS", savePath);
                        break;
                    case "Schutzkreis":
                        ExcelManager.Schutzkreis(WorkSheetS[sheetCounterX]);
                        NetworkDBMaker.PopulateDB(Variables, "Schutzkreis", savePath);
                        break;
                    case "F> Safety <F":
                        ExcelManager.Safety_Standart(WorkSheetS[sheetCounterX]);
                        NetworkDBMaker.PopulateDB(Variables, "Safety_Standart", savePath);

                        ExcelManager.Standart_Safety(WorkSheetS[sheetCounterX]);
                        NetworkDBMaker.PopulateDB(Variables, "Standart_Safety", savePath);
                        break;
                    case "ARG_Typ_Strg":
                        ExcelManager.ARG(WorkSheetS[sheetCounterX]);
                        NetworkDBMaker.PopulateDB(Variables, "ARG", savePath);
                        break;
                    case "Station":
                        ExcelManager.StationName1(WorkSheetS[sheetCounterX]);
                        NetworkDBMaker.PopulateDB(Variables, "Station1", savePath);

                        ExcelManager.StationName2(WorkSheetS[sheetCounterX]);
                        NetworkDBMaker.PopulateDB(Variables, "Station2", savePath);

                        ExcelManager.StationName3(WorkSheetS[sheetCounterX]);
                        NetworkDBMaker.PopulateDB(Variables, "Station3", savePath);
                        break;
                }
                sheetCounterX += 1;
            }

            BlocksCreated = BlocksCreated.Distinct().ToList();

            File.Delete("C:/Temp/DBThemePlate.xml");

            if (importToTia)
            {
                using (var access = tiaPortal.ExclusiveAccess("Importing elements"))
                {
                    ExcelManager.BlocksImporter(savePath, current, "2_Safety", BlocksCreated);
                    ExcelManager.BlocksImporter(savePath, current, "40_Betriebsarten", BlocksCreated);
                    ExcelManager.BlocksImporter(savePath, current, "50_Stationen", BlocksCreated);
                    ExcelManager.BlocksImporter(savePath, current, "100_ARG_Typ_Strg", BlocksCreated);
                }
            }
            WriteSavingLabelText("");
        }

        /// <summary>
        /// Button that saves the current grid view values in the current selected item
        /// </summary>
        private void Button_SaveCurrentValues(object sender, RoutedEventArgs e)
        {
            WriteSavingLabelText("Saving...");

            TreeViewManager.FilePath = filePath;
            SaveEngValues();

            WriteSavingLabelText("");
        }

        /// <summary>
        /// Button used to clear the grids
        /// </summary>
        private void Button_ClearGrid(object sender, RoutedEventArgs e)
        {
            gridViewLeft.Rows.Clear();
            gridViewRight.Rows.Clear();
            InsertCommonRowOfTheLeftGrid();
            InsertCommonRowsOfTheRightGrid();
        }
        #endregion

        //Functions
        /// <summary>
        /// Function used to insert Columns into the right grid when needed
        /// </summary>
        /// <param name="numberOfColumns">Number of columns that the user wishs to make</param>
        private void InsertColumns(int numberOfColumns)
        {
            columnNumber = gridViewRight.ColumnCount;
            for (int z = 1; z <= numberOfColumns; z++)
            {
                gridViewRight.Columns.Add(columnNumber.ToString(), columnNumber.ToString() + "\n" + "Nr | Qty");
                gridViewRight.Columns[gridViewRight.ColumnCount - 1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                columnNumber += 1;
            }
            DisableSort(gridViewRight);
        }

        /// <summary>
        /// Function used disable the sort ability of the grid columns
        /// </summary>
        /// <param name="data">DatagridView that is gonna be used</param>
        private void DisableSort(DataGridView data)
        {
            foreach (DataGridViewColumn column in data.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        /// <summary>
        /// Function used to add the normal row in the left grid view
        /// </summary>
        private void InsertCommonRowOfTheLeftGrid()
        {
            DataGridViewRow row = new DataGridViewRow();
            gridViewLeft.Rows.Add(row);
        }

        /// <summary>
        /// Function used to add the two normal rows in the right grid view
        /// </summary>
        private void InsertCommonRowsOfTheRightGrid()
        {
            DataGridViewRow PartPresence = new DataGridViewRow();
            DataGridViewRow Valves = new DataGridViewRow();
            PartPresence.Height = 30;
            Valves.Height = 30;
            gridViewRight.Rows.Add(PartPresence);
            gridViewRight.Rows.Add(Valves);
        }

        /// <summary>
        /// Function used to Load the values into the grid views
        /// </summary>
        private void LoadValues()
        {
            ExcelManager.EngAssist();
            if (EngValues.Count != 0)
            {
                foreach (EngAssist eng in EngValues)
                {
                    if (maxColumns < eng.Parts.Count)
                    {
                        maxColumns = eng.Parts.Count;
                    }

                    if (maxColumns < eng.Valves.Count)
                    {
                        maxColumns = eng.Valves.Count;
                    }
                }

                InsertColumns(maxColumns);

                gridViewLeft.Rows.Clear();
                gridViewRight.Rows.Clear();

                foreach (EngAssist eng in EngValues)
                {
                    gridViewLeft.Rows.Add(eng.Arbeitsgruppe_ARG, eng.Schutzkreis_SK, eng.Station, eng.Erw_Stationsbez_SBZ);

                    int part = 1;

                    gridViewRight.Rows.Add();

                    foreach (string s in eng.Parts)
                    {
                        gridViewRight.Rows[gridViewRight.RowCount - 1].Cells[part].Value = s;
                        part += 1;
                    }

                    int valve = 1;

                    gridViewRight.Rows.Add();
                    foreach (string s in eng.Valves)
                    {
                        gridViewRight.Rows[gridViewRight.RowCount - 1].Cells[valve].Value = s;
                        valve += 1;
                    }
                }

                //InsertCommonRowOfTheLeftGrid();
                gridViewLeft.Refresh();
                gridViewRight.Refresh();

                DisableSort(gridViewRight);

                var x = gridViewRight.Rows[gridViewRight.RowCount - 1].Cells[1];
                gridViewRight.CurrentCell = gridViewRight[0, 0];
                gridViewRight.CurrentCell = x;
            }
        }

        /// <summary>
        /// Function used to Load the values into the grid views
        /// </summary>
        private void LoadValues(object[,] Matriz)
        {
            ExcelManager.EngAssist(Matriz);
            if (EngValues.Count != 0)
            {
                foreach (EngAssist eng in EngValues)
                {
                    if (maxColumns < eng.Parts.Count)
                    {
                        maxColumns = eng.Parts.Count;
                    }

                    if (maxColumns < eng.Valves.Count)
                    {
                        maxColumns = eng.Valves.Count;
                    }
                }

                InsertColumns(maxColumns);

                gridViewLeft.Rows.Clear();
                gridViewRight.Rows.Clear();

                foreach (EngAssist eng in EngValues)
                {
                    gridViewLeft.Rows.Add(eng.Arbeitsgruppe_ARG, eng.Schutzkreis_SK, eng.Station, eng.Erw_Stationsbez_SBZ);

                    int part = 1;

                    gridViewRight.Rows.Add();

                    foreach (string s in eng.Parts)
                    {
                        gridViewRight.Rows[gridViewRight.RowCount - 1].Cells[part].Value = s;
                        part += 1;
                    }

                    int valve = 1;

                    gridViewRight.Rows.Add();
                    foreach (string s in eng.Valves)
                    {
                        gridViewRight.Rows[gridViewRight.RowCount - 1].Cells[valve].Value = s;
                        valve += 1;
                    }
                }

                InsertCommonRowOfTheLeftGrid();
                gridViewLeft.Refresh();
                gridViewRight.Refresh();

                DisableSort(gridViewRight);

                var x = gridViewRight.Rows[gridViewRight.RowCount - 1].Cells[1];
                gridViewRight.CurrentCell = gridViewRight[0, 0];
                gridViewRight.CurrentCell = x;

                gridViewLeft.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                gridViewRight.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
        }

        /// <summary>
        /// Function used to retrie the values form the 2 grid views
        /// </summary>
        public void RetrieveValues()
        {
            gridViewLeft.CurrentCell = null;
            gridViewRight.CurrentCell = null;

            newEngValues = new List<EngAssist>();

            for (int rows = 0; rows < gridViewLeft.Rows.Count - 1; rows++)
            {
                string ARG = "";
                string SK = "";
                string ST = "";
                string SBZ = "";

                if (gridViewLeft.Rows[rows].Cells[0].Value != null)
                {
                    ARG = gridViewLeft.Rows[rows].Cells[0].Value.ToString().Replace(" ", string.Empty);
                }

                if (gridViewLeft.Rows[rows].Cells[1].Value != null)
                {
                    SK = gridViewLeft.Rows[rows].Cells[1].Value.ToString().Replace(" ", string.Empty);
                }

                if (gridViewLeft.Rows[rows].Cells[2].Value != null)
                {
                    ST = gridViewLeft.Rows[rows].Cells[2].Value.ToString().Replace(" ", string.Empty);
                }

                if (gridViewLeft.Rows[rows].Cells[3].Value != null)
                {
                    SBZ = gridViewLeft.Rows[rows].Cells[3].Value.ToString().Replace(" ", string.Empty);
                }

                EngAssist engValue = new EngAssist(ARG, SK, ST, SBZ);

                for (int col = 1; col < gridViewRight.ColumnCount -1; col++)
                {
                    if (gridViewRight.Rows[(rows*2)].Cells[col].Value != null)
                    {
                        engValue.Parts.Add(gridViewRight.Rows[(rows * 2)].Cells[col].Value.ToString().Replace(" ", string.Empty));
                    }
                    else
                    {
                        engValue.Parts.Add("");
                    }

                    if (gridViewRight.Rows[(rows * 2) + 1].Cells[col].Value != null)
                    {
                        engValue.Valves.Add(gridViewRight.Rows[(rows * 2) + 1].Cells[col].Value.ToString().Replace(" ", string.Empty));
                    }
                    else
                    {
                        engValue.Valves.Add("");
                    }
                    
                }
                newEngValues.Add(engValue);
            }
        }
        
        /// <summary>
        ///Saves the current values of the 2 grid views in the current selected item of the tree view
        /// </summary>
        public void SaveEngValues()
        {
            RetrieveValues();
            var matrixs = new List<object[,]>();
            var sheetNames = new List<string>();

            if (TreeViewManager.FilePath != "" && File.Exists(TreeViewManager.FilePath))
            {
                if (TreeViewManager.FilePath.Contains(".xlsx") || TreeViewManager.FilePath.Contains(".xlsm") || TreeViewManager.FilePath.Contains(".xltx") || TreeViewManager.FilePath.Contains(".xltm"))
                {
                    Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                    xlApp.DisplayAlerts = false;
                    Workbook xlWorkbook = xlApp.Workbooks.Open(TreeViewManager.FilePath);
                    int row = 4;

                    foreach (Worksheet sheet in xlWorkbook.Worksheets)
                    {
                        if (sheet.Name.Contains("EngAssist"))
                        {
                            TreeViewManager.Ws = sheet;
                            object[,] EngAssistSheet = OpennessHelper.ExcelToMatrix(TreeViewManager.Ws);

                            while (TreeViewManager.Ws.Cells[row, 2].Value != null)
                            {
                                TreeViewManager.Ws.Cells[row, 2].Value = null;
                                TreeViewManager.Ws.Cells[row, 3].Value = null;
                                TreeViewManager.Ws.Cells[row, 4].Value = null;
                                TreeViewManager.Ws.Cells[row, 5].Value = null;

                                int column = 8;
                                while (TreeViewManager.Ws.Cells[row, column].Value != null)
                                {
                                    TreeViewManager.Ws.Cells[row, column].Value = null;
                                    TreeViewManager.Ws.Cells[row + 1, column].Value = null;
                                    column += 1;
                                }
                                row += 2;
                            }

                            row = 4;

                            foreach (EngAssist engAssist in DBMaker.newEngValues)
                            {
                                TreeViewManager.Ws.Cells[row, 2].Value = engAssist.Arbeitsgruppe_ARG;
                                TreeViewManager.Ws.Cells[row, 3].Value = engAssist.Schutzkreis_SK;
                                TreeViewManager.Ws.Cells[row, 4].Value = engAssist.Station;
                                TreeViewManager.Ws.Cells[row, 5].Value = engAssist.Erw_Stationsbez_SBZ;

                                EngAssistSheet[row, 2] = engAssist.Arbeitsgruppe_ARG;
                                EngAssistSheet[row, 3] = engAssist.Schutzkreis_SK;
                                EngAssistSheet[row, 4] = engAssist.Station;
                                EngAssistSheet[row, 5] = engAssist.Erw_Stationsbez_SBZ;

                                int column = 8;
                                foreach (string part in engAssist.Parts)
                                {
                                    TreeViewManager.Ws.Cells[row, column].Value = part;
                                    EngAssistSheet[row, column] = part;
                                    column += 1;
                                }

                                column = 8;
                                foreach (string valve in engAssist.Valves)
                                {
                                    TreeViewManager.Ws.Cells[row + 1, column].Value = valve;
                                    EngAssistSheet[row + 1, column] = valve;
                                    column += 1;
                                }

                                row += 2;
                            }

                            matrixs.Add(EngAssistSheet);
                        }
                        else
                        {
                            var matrix = OpennessHelper.ExcelToMatrix(sheet);
                            matrixs.Add(matrix);
                        }
                        sheetNames.Add(sheet.Name);
                    }

                    MatrixList(matrixs);
                    SheetNamesList(sheetNames);

                    xlWorkbook.Save();
                    xlWorkbook.Close(0);
                    xlApp.Quit();
                }
            }
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

        //Events
        /// <summary>
        /// Event used to fill the first column on the right grid with only "Part Presence" and "Valves" whenever a new row is entered
        /// </summary>
        private void PartAndValvesFiller(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCellStyle styleBlue = new DataGridViewCellStyle();
            styleBlue.ForeColor = Color.FromArgb(48, 84, 150);

            for (int i = 0; i < gridViewRight.Rows.Count; i++)
            {
                gridViewRight.Rows[i].Cells[0].Style = styleBlue;

                if (i % 2 == 0)
                    gridViewRight.Rows[i].Cells[0].Value = "Part Presence";
                else
                    gridViewRight.Rows[i].Cells[0].Value = "Valves";
            }
        }

        /// <summary>
        /// Event used to manage the column on the right grid
        /// </summary>
        private void ColumnManager(object sender, DataGridViewCellEventArgs e)
        {
            if (gridViewRight.CurrentCell.Value != null && gridViewRight.CurrentCell.ColumnIndex + 1 == gridViewRight.ColumnCount)
            {
                InsertColumns(1);
            }

            else if (gridViewRight.CurrentCell.Value == null)
            {
                int found = 0;
                foreach (DataGridViewRow row in gridViewRight.Rows)
                {
                    if (row.Cells[gridViewRight.CurrentCell.ColumnIndex].Value != null)
                    {
                        found = 1;
                    }
                }
                if (found == 0 && gridViewRight.ColumnCount != 2)
                {
                    gridViewRight.Columns.Remove((gridViewRight.ColumnCount - 1).ToString());
                    columnNumber = gridViewRight.ColumnCount;
                }
            }

        }

        /// <summary>
        /// Event used to add a new row whenever the user finishs giving a cell in the last row a value 
        /// </summary>
        private void AddNewRows(object sender, DataGridViewCellEventArgs e)
        {
            if (gridViewLeft.CurrentCell.Value != null && gridViewLeft.CurrentCell.RowIndex + 1 == gridViewLeft.RowCount)
            {
                InsertCommonRowOfTheLeftGrid();
                InsertCommonRowsOfTheRightGrid();
                var x = gridViewRight.CurrentCell;
                gridViewRight.CurrentCell = gridViewRight[0, 0];
                gridViewRight.CurrentCell = x;
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
    }
}

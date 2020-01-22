using System;
using System.Collections.Generic;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.Forms.MessageBox;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using TextBox = System.Windows.Controls.TextBox;
using DataGrid = System.Windows.Controls.DataGrid;
using System.ComponentModel;
using System.Linq;
using Siemens.Engineering;
using System.Windows.Media;
using Siemens.Engineering.SW.Blocks;
using Siemens.Engineering.SW;

namespace TiaOpennessHelper.ExcelTree
{
    public partial class TreeViewManager
    {
        //Events to update cache on MainWindowViewModel
        public event Action<List<object[,]>> MatrixList;
        public event Action<List<string>> SheetNamesList;

        public static List<string> BlocksCreated { get; set; }
        public static string FilePath { get; set; }
        public static Worksheet Ws { get; set; }
        public static bool ExcelAskerCancel { get; set; }
        public static bool IsTiaConnected { get; set; }
        public bool Changes { get; set; }

        private static List<Step> steps = new List<Step>();
        private static List<Step> stepsBlank = new List<Step>();
        private static List<WorkSheet> workSheets = new List<WorkSheet>();
        private static List<object[,]> sheetsStepS;
        private static List<string> sheetsNameS;
        
        private readonly TiaPortal tiaPortal;
        private readonly Project tiaPortalProject;
        private readonly object current;

        private static string sheet;
        private static string savePath;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="SheetsNames"></param>
        /// <param name="SheetsSteps"></param>
        /// <param name="path"></param>
        /// <param name="SavePath"></param>
        /// <param name="tiaPortal"></param>
        /// <param name="tiaPortalProject"></param>
        /// <param name="current"></param>
        public TreeViewManager(List<string> SheetsNames, List<object[,]> SheetsSteps, string path, string SavePath, TiaPortal tiaPortal, Project tiaPortalProject, object current)
        {
            InitializeComponent();

            PopulateComboBox(SheetsNames);
            PopulateGridWithMatrixList(SheetsSteps, SheetsNames);

            BlocksCreated = new List<string>();
            MainGridView.ItemsSource = new List<Step>();
            FilePath = path;
            Ws = null;
            Changes = false;
            ExcelAskerCancel = true;
            savePath = SavePath;
            this.tiaPortal = tiaPortal;
            this.tiaPortalProject = tiaPortalProject;
            this.current = current;
            sheetsNameS = SheetsNames;
            sheetsStepS = SheetsSteps;
        }

        #region BUTTON EVENTS
        /// <summary>
        /// Creates a new Excel with the DataGrid Values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            WriteSavingLabelText("Creating New Excel...");

            RetrieveValues();
            ExcelAsker excelAsker = new ExcelAsker
            {
                SavePath = Path.Combine(savePath, "Excel Files"),
                Steps = steps
            };
            excelAsker.Closed += ExcelAsker_Closed;
            excelAsker.Show();

            WriteSavingLabelText("");
        }

        /// <summary>
        /// Event that clears the DataGird
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Are you sure you want to clear grid?", "Clear grid", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                MainGridView.ItemsSource = stepsBlank;
        }

        /// <summary>
        /// Event that calls the method to Generate the NetWork and Grafcet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_GenerateNetNGraf_Click(object sender, RoutedEventArgs e)
        {
            bool importToTia = (bool)cbImportToTia.IsChecked;
            if (FilePath.Contains(".xlsx") || FilePath.Contains(".xlsm") || FilePath.Contains(".xltx") || FilePath.Contains(".xltm"))
            {
                WriteSavingLabelText("Generating NetNGraf...");

                BlocksCreated = new List<string>();
                RetrieveValues();

                // Update lists "SheetsStepS" and "SheetsNameS"
                foreach (WorkSheet ws in workSheets)
                {
                    if (ws.WorkSheetSteps.Count() > 1)
                        SaveWorksheetIntoMatrix(ws, ws.WorkSheetName);
                    else
                        MessageBox.Show("Worksheet \"" + ws.WorkSheetName + "\" will not be generated because it does not have sufficient steps.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                UpdateWorksheetSteps();

                PlcSoftware plcS = null;

                GrafcetManager.SavePath = Path.Combine(savePath, "50_Stationen");

                if (current != null)
                    plcS = (PlcSoftware)(current as PlcBlockUserGroup).Parent.Parent;

                try
                {
                    GrafcetManager.GenerateGrafcet(sheetsStepS, sheetsNameS, plcS);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    WriteSavingLabelText("");
                    return;
                }

                if (importToTia)
                {
                    BlocksCreated = BlocksCreated.Distinct().ToList();
                    using (var access = tiaPortal.ExclusiveAccess("Importing elements"))
                    {
                        ExcelManager.BlocksImporter(savePath, current, "50_Stationen", BlocksCreated);
                    }
                }

                Changes = true;
                WriteSavingLabelText("");
            }
        }

        /// <summary>
        /// Saves the current DataGrid values in the chosen path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            WriteSavingLabelText("Saving...");

            if (ExcelManager.IsOpened(FilePath))
            {
                MessageBox.Show("Please close the current workbook before saving");
            }
            else
            {
                RetrieveValues();

                if (FilePath.Contains(".xlsx") || FilePath.Contains(".xlsm") || FilePath.Contains(".xltx") || FilePath.Contains(".xltm"))
                {
                    Excel.Application xlApp = new Excel.Application
                    {
                        DisplayAlerts = false
                    };
                    Workbook xlWorkbook = xlApp.Workbooks.Open(FilePath);
                    int row = 4;
                    var matrixs = new List<object[,]>();
                    var sheetNames = new List<string>();

                    foreach (Worksheet sheet in xlWorkbook.Worksheets)
                    {
                        string sheetName = sheet.Name;
                        if (!sheetName.Contains("AS_") || sheetName.Equals("AS_000000")) continue;

                        object[,] matrix = OpennessHelper.ExcelToMatrix(sheet);
                        if (sheet.Name.Contains(ComboSheet.SelectedItem.ToString()))
                        {
                            Ws = sheet;
                            while (Ws.Cells[row, 3].Value != null)
                            {
                                Ws.Cells[row, 3].Value = null;
                                Ws.Cells[row, 4].Value = null;
                                Ws.Cells[row, 5].Value = null;
                                Ws.Cells[row, 6].Value = null;
                                Ws.Cells[row, 7].Value = null;
                                Ws.Cells[row, 8].Value = null;

                                row += 1;
                            }

                            row = 4;

                            foreach (Step step in steps)
                            {
                                Ws.Cells[row, 2].Value = step.StepNumber;
                                Ws.Cells[row, 3].Value = step.Schritt.Replace(" ", string.Empty);
                                Ws.Cells[row, 4].Value = step.Beschreibung;
                                Ws.Cells[row, 5].Value = step.Aktion.Replace(" ", string.Empty);
                                Ws.Cells[row, 6].Value = step.Vorheriger_Schritt.Replace(" ", string.Empty);
                                Ws.Cells[row, 7].Value = step.Nächster_Schritt.Replace(" ", string.Empty);
                                Ws.Cells[row, 8].Value = step.Zeit_Schritt.ToLower().Replace(" ", string.Empty).Replace("ms", string.Empty);

                                matrix[row, 2] = step.StepNumber;
                                matrix[row, 3] = step.Schritt.Replace(" ", string.Empty);
                                matrix[row, 4] = step.Beschreibung;
                                matrix[row, 5] = step.Aktion.Replace(" ", string.Empty);
                                matrix[row, 6] = step.Vorheriger_Schritt.Replace(" ", string.Empty);
                                matrix[row, 7] = step.Nächster_Schritt.Replace(" ", string.Empty);
                                matrix[row, 8] = step.Zeit_Schritt.ToLower().Replace(" ", string.Empty).Replace("ms", string.Empty);

                                row += 1;
                            }
                        }
                        matrixs.Add(matrix);
                        sheetNames.Add(sheet.Name);
                    }

                    MatrixList(matrixs);
                    SheetNamesList(sheetNames);

                    xlWorkbook.Save();
                    xlWorkbook.Close(0);
                    xlApp.Quit();

                    sheetsStepS = new List<object[,]>();
                    foreach (var m in matrixs)
                    {
                        sheetsStepS.Add(m);
                    }
                    sheetsNameS = new List<string>();
                    foreach (var n in sheetNames)
                    {
                        sheetsNameS.Add(n);
                    }

                    Saving.Text = "";
                    Saving.Update();

                    if (Ws == null)
                        System.Windows.MessageBox.Show("This Excel does not contain a usable worksheet", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            WriteSavingLabelText("");
        }
        #endregion

        /// <summary>
        /// Populate datagrid with a list of matrixs
        /// </summary>
        /// <param name="SheetsSteps"></param>
        /// <param name="SheetsNames"></param>
        private void PopulateGridWithMatrixList(List<object[,]> SheetsSteps, List<string> SheetsNames)
        {
            int counter = 0;
            foreach (string s in SheetsNames)
            {
                workSheets.Add(new WorkSheet()
                {
                    WorkSheetName = s,
                    WorkSheetSteps = new List<Step>()
                });

                steps = new List<Step>();
                int row = 4;
                int collumn = 3;

                while (SheetsSteps[counter][row, collumn] != null)
                {
                    workSheets[workSheets.Count - 1].WorkSheetSteps.Add(new Step()
                    {
                        StepNumber = Convert.ToInt32(SheetsSteps[counter][row, 2].ToString()),
                        Schritt = SheetsSteps[counter][row, 3].ToString(),
                        Beschreibung = SheetsSteps[counter][row, 4].ToString(),
                        Aktion = SheetsSteps[counter][row, 5].ToString(),
                        Vorheriger_Schritt = SheetsSteps[counter][row, 6].ToString(),
                        Nächster_Schritt = SheetsSteps[counter][row, 7].ToString(),
                        Zeit_Schritt = SheetsSteps[counter][row, 8].ToString() + " ms"
                    });

                    row += 1;

                    if (row == SheetsSteps[counter].GetLength(0)) break;
                }
                counter += 1;
            }
        }

        /// <summary>
        /// Transform worksheet into matrix
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="sheetName"></param>
        /// <returns>matrix generated</returns>
        private void SaveWorksheetIntoMatrix(WorkSheet ws, string sheetName)
        {
            var matrix = new object[ws.WorkSheetSteps.Count() + 5, 9];
            int row = 4;

            foreach (Step step in ws.WorkSheetSteps)
            {
                matrix[row, 2] = step.StepNumber;
                matrix[row, 3] = step.Schritt.Replace(" ", string.Empty);
                matrix[row, 4] = step.Beschreibung;
                matrix[row, 5] = step.Aktion.Replace(" ", string.Empty);
                matrix[row, 6] = step.Vorheriger_Schritt.Replace(" ", string.Empty);
                matrix[row, 7] = step.Nächster_Schritt.Replace(" ", string.Empty);
                matrix[row, 8] = step.Zeit_Schritt.ToLower().Replace(" ", string.Empty).Replace("ms", string.Empty);

                row += 1;
            }

            for (int i = 0; i < sheetsNameS.Count; i++)
            {
                if (sheetsNameS[i] == sheetName)
                {
                    sheetsStepS[i] = matrix;
                    break;
                }
            }
        }

        /// <summary>
        /// Populate combobox with sheet names
        /// </summary>
        /// <param name="SheetsNames"></param>
        private void PopulateComboBox(List<string> SheetsNames)
        {
            ComboSheet.Items.Clear();

            foreach (string s in SheetsNames)
            {
                if(!s.Equals("AS_000000"))
                    ComboSheet.Items.Add(s.ToString());
            }

            workSheets = new List<WorkSheet>();
        }

        /// <summary>
        /// Fills the Steps List with the Values of the DataGrid
        /// </summary>
        private void RetrieveValues()
        {
            MainGridView.UnselectAllCells();
            steps = new List<Step>();
            int rowcount = MainGridView.Items.Count;
            for (int z = 0; z < rowcount; z++)
            {
                TextBlock StepNumber = MainGridView.Columns[0].GetCellContent(MainGridView.Items[z]) as TextBlock;
                TextBlock StepName = MainGridView.Columns[1].GetCellContent(MainGridView.Items[z]) as TextBlock;
                TextBlock StepDescription = MainGridView.Columns[2].GetCellContent(MainGridView.Items[z]) as TextBlock;
                TextBlock StepActions = MainGridView.Columns[3].GetCellContent(MainGridView.Items[z]) as TextBlock;
                TextBlock StepsBefore = MainGridView.Columns[4].GetCellContent(MainGridView.Items[z]) as TextBlock;
                TextBlock StepsAfter = MainGridView.Columns[5].GetCellContent(MainGridView.Items[z]) as TextBlock;
                TextBlock StepTime = MainGridView.Columns[6].GetCellContent(MainGridView.Items[z]) as TextBlock;

                if (StepName.Text != "")
                {
                    steps.Add(new Step()
                    {
                        StepNumber = Convert.ToInt32(StepNumber.Text.Replace(" ", string.Empty)),
                        Schritt = StepName.Text,
                        Beschreibung = StepDescription.Text,
                        Aktion = StepActions.Text,
                        Vorheriger_Schritt = StepsBefore.Text,
                        Nächster_Schritt = StepsAfter.Text,
                        Zeit_Schritt = StepTime.Text
                    });
                }
                else
                    break;
            }
            sheet = ComboSheet.SelectedItem.ToString();
        }

        /// <summary>
        /// Sorts the Data Grid
        /// </summary>
        /// <param name="dataGrid"></param>
        /// <param name="columnIndex"></param>
        /// <param name="sortDirection"></param>
        public static void SortDataGrid(DataGrid dataGrid, int columnIndex, ListSortDirection sortDirection)
        {
            var column = dataGrid.Columns[columnIndex];

            // Clear current sort descriptions
            dataGrid.Items.SortDescriptions.Clear();

            // Add the new sort description
            dataGrid.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, sortDirection));

            // Apply sort
            foreach (var col in dataGrid.Columns)
            {
                col.SortDirection = null;
            }
            column.SortDirection = sortDirection;

            // Refresh items to display sort
            dataGrid.Items.Refresh();
        }

        /// <summary>
        /// Updates Worksheet with selected sheet in combobox
        /// </summary>
        private void UpdateWorksheetSteps()
        {
            foreach (WorkSheet ws in workSheets)
            {
                if (ws.WorkSheetName == ComboSheet.Text)
                {
                    RetrieveValues();
                    ws.WorkSheetSteps = new List<Step>();
                    foreach (var step in steps)
                    {
                        ws.WorkSheetSteps.Add(step);
                    }

                    SaveWorksheetIntoMatrix(ws, ComboSheet.Text);
                    break;
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

        #region Events
        /// <summary>
        /// Event that allows the grid to have multiline
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainGridView_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter) && Keyboard.Modifiers == ModifierKeys.Alt)
            {
                if (e.OriginalSource is FrameworkElement DataGridText)
                {
                    TextBox dgText = DataGridText as TextBox;
                    dgText.Text += "\r\n";
                    dgText.SelectionStart = dgText.Text.Length;
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Event that changes the Data Grid Values if the item is a usable excel worksheet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboSheet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateWorksheetSteps();

            MainGridView.ItemsSource = stepsBlank;

            foreach (WorkSheet ws in workSheets)
            {
                if (ws.WorkSheetName == ComboSheet.SelectedItem.ToString())
                {
                    MainGridView.ItemsSource = ws.WorkSheetSteps;
                    SortDataGrid(MainGridView, 0, ListSortDirection.Ascending);
                    break;
                }
            }
        }

        /// <summary>
        /// Event that handles window loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ComboSheet.SelectedIndex = 0;

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
        /// Handle ExcelAsker closed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExcelAsker_Closed(object sender, EventArgs e)
        {
            if (!ExcelAskerCancel)
            {
                this.Close();
                Changes = true;
            }
        }
        #endregion
    }
}

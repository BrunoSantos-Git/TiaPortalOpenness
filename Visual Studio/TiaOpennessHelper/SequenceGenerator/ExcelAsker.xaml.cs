using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using Microsoft.Office.Interop.Excel;

namespace TiaOpennessHelper.ExcelTree
{
    public partial class ExcelAsker 
    {
        public List<Step> Steps = new List<Step>();
        public static string ExcelName { get; set; }
        public static string WorksheetName { get; set; }
        public string SavePath { get; set; }
        public static Worksheet Ws { get; set; }
        private string workingDirectory;
        private string workPath;

        /// <summary>
        /// Constructor
        /// </summary>
        public ExcelAsker()
        {
            InitializeComponent();
            ExcelNameTextBox.Text = "";
            ExcelWorkSheetTextBox.Text = "";
            workingDirectory = Environment.CurrentDirectory;
            workPath = Directory.GetParent(workingDirectory).FullName;
        }

        /// <summary>
        /// Button Create Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Create(object sender, RoutedEventArgs e)
        {
            ExcelName = ExcelNameTextBox.Text;
            WorksheetName = ExcelWorkSheetTextBox.Text;
            Microsoft.Office.Interop.Excel.Application xlApp = null;
            Workbook xlWorkbook = null;

            if (ExcelName == "" || ExcelName.Length < 3)
            {
                ExcelNameTextBox.BorderBrush = Brushes.Red;
                return;
            } 
            else
                ExcelNameTextBox.ClearValue(System.Windows.Controls.Border.BorderBrushProperty);

            if (WorksheetName == "" || (WorksheetName.Length != 6 && WorksheetName.Length != 9))
            {
                ExcelWorkSheetTextBox.BorderBrush = Brushes.Red;
                return;
            }
            else
                ExcelWorkSheetTextBox.ClearValue(System.Windows.Controls.Border.BorderBrushProperty);

            try
            {
                xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlWorkbook = xlApp.Workbooks.Open(workPath + @"\Templates\Excel\ExcelThemePlate.xlsm");
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                xlWorkbook.Close(0);
                xlApp.Quit();
                return;
            }

            Ws = xlWorkbook.Worksheets[1];
            Ws.Name = "AS_" + WorksheetName;
            int row = 4;
            foreach (Step step in Steps)
            {
                Ws.Cells[row, 2].Value = step.StepNumber;
                Ws.Cells[row, 3].Value = step.Schritt;
                Ws.Cells[row, 4].Value = step.Beschreibung;
                Ws.Cells[row, 5].Value = step.Aktion;
                Ws.Cells[row, 6].Value = step.Vorheriger_Schritt;
                Ws.Cells[row, 7].Value = step.Nächster_Schritt;
                Ws.Cells[row, 8].Value = step.Zeit_Schritt.Replace("ms", "");

                row += 1;
            }

            xlWorkbook.SaveAs(SavePath + "\\" + ExcelName + ".xlsm", XlFileFormat.xlOpenXMLWorkbookMacroEnabled);
            System.Windows.MessageBox.Show("Excel created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            xlWorkbook.Close(0);
            xlApp.Quit();
            TreeViewManager.ExcelAskerCancel = false;
            this.Close();
            
        }

        /// <summary>
        /// Button Cancel Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            TreeViewManager.ExcelAskerCancel = true;
            this.Close();
        }
    }
}

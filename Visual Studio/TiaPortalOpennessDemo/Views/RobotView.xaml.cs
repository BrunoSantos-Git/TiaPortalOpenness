using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TiaOpennessHelper.Utils;
using TiaOpennessHelper.VWSymbolic;

namespace TiaPortalOpennessDemo.Views
{
    /// <summary>
    /// Interaction logic for RobotView.xaml
    /// </summary>
    public partial class RobotView : Window
    {
        public string SavePath { get; set; }
        public string PlcDBPath { get; set; }
        public object Current { get; set; }
        public bool Changes { get; set; }
        public bool IsTiaConnected { get; set; }

        private readonly bool isChangeSymbolic;
        private readonly RobotInfo robInfo;
        private List<string> tecnologies;
        private static List<List<RobotBase>> RobBase;
        private static List<List<RobotTecnologie>> RobTecnologies;
        private static List<List<RobotSafeRangeMonitoring>> RobSafeRangeMonitoring;
        private static List<List<RobotSafeOperation>> RobSafeOperations;
        private List<string> checkedTecnologies;

        /// <summary>
        /// Normal Constructor
        /// </summary>
        /// <param name="dataContext"></param>
        public RobotView(object dataContext)
        {
            DataContext = dataContext;
            InitializeComponent();
            InitializeLists();
        }

        /// <summary>
        /// Constructor with RobInfo
        /// </summary>
        /// <param name="dataContext"></param>
        public RobotView(object dataContext, RobotInfo robInfo)
        {
            DataContext = dataContext;
            InitializeComponent();
            this.robInfo = robInfo;

            InitializeLists();

            FillWithRobInfo();
        }

        /// <summary>
        /// Constructor used to change Symbolic
        /// </summary>
        /// <param name="dataContext"></param>
        public RobotView(object dataContext, RobotInfo robInfo, List<List<RobotBase>> robBase, List<List<RobotTecnologie>> robTecnologies, List<List<RobotSafeRangeMonitoring>> robSafeRangeMonitoring, List<List<RobotSafeOperation>> robSafeOperations)
        {
            DataContext = dataContext;
            InitializeComponent();
            this.robInfo = robInfo;
            tecnologies = new List<string>();
            checkedTecnologies = new List<string>();
            Changes = false;

            RobBase = robBase;
            RobTecnologies = robTecnologies;
            RobSafeRangeMonitoring = robSafeRangeMonitoring;
            RobSafeOperations = robSafeOperations;

            FillWithRobInfo();

            isChangeSymbolic = true;
        }

        /// <summary>
        /// Initialize Lists
        /// </summary>
        private void InitializeLists()
        {
            Changes = false;
            tecnologies = new List<string>();
            checkedTecnologies = new List<string>();
            RobTecnologies = new List<List<RobotTecnologie>>();

            // RobTecnologies List
            var secRobTecnologies = new List<RobotTecnologie>();
            foreach (var o in Robot.RobTecnologies[0])
            {
                secRobTecnologies.Add(new RobotTecnologie(o.FBNumber, o.Name, o.Type, o.Symbolic, o.DataType, o.Address, o.Comment));
            }
            RobTecnologies.Add(secRobTecnologies);
            secRobTecnologies = new List<RobotTecnologie>();
            foreach (var i in Robot.RobTecnologies[1])
            {
                secRobTecnologies.Add(new RobotTecnologie(i.FBNumber, i.Name, i.Type, i.Symbolic, i.DataType, i.Address, i.Comment));
            }
            RobTecnologies.Add(secRobTecnologies);
        }

        /// <summary>
        /// Handles AddRobot button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateRobot_Click(object sender, RoutedEventArgs e)
        {
            checkedTecnologies = new List<string>();

            ComboBoxItem cbiRobSafe = (ComboBoxItem)cbRobSafe.SelectedItem;
            ComboBoxItem cbiType = (ComboBoxItem)cbType.SelectedItem;
            if (string.IsNullOrEmpty(RobName.Text) || string.IsNullOrEmpty(StartAddress.Text) || cbiRobSafe == null || cbiType == null) return;

            string robSafe = cbiRobSafe.Content.ToString();
            string name = RobName.Text;
            string startAddress = StartAddress.Text;
            string type = cbiType.Content.ToString();
            int iStartAddress;
            try
            {
                int.TryParse(startAddress, out iStartAddress);
            }
            catch (Exception)
            {
                StartAddress.BorderBrush = Brushes.Red;
                return;
            }

            if (!ValidateName(name))
            {
                RobName.BorderBrush = Brushes.Red;
                return;
            }

            for (int i = 0; i < datagrid.Items.Count; i++)
            {
                var item = datagrid.Items[i];
                var checkboxTemplate = datagrid.Columns[1].GetCellContent(item);
                CheckBox checkbox = uFindVisualChild.FindVisualChild<CheckBox>(checkboxTemplate);

                if (checkbox != null && (bool)checkbox.IsChecked)
                    checkedTecnologies.Add(tecnologies[i]);
            }

            SymbolicManager sm;

            if (isChangeSymbolic)
                sm = new SymbolicManager(robInfo, RobBase, RobTecnologies, RobSafeRangeMonitoring, RobSafeOperations) { SavePath = SavePath, Current  = Current, PlcDBPath = PlcDBPath };
            else
                sm = new SymbolicManager() { SavePath = SavePath, Current = Current, PlcDBPath = PlcDBPath };

            bool importToTia = (bool)cbImportToTia.IsChecked;
            sm.NewRobot(iStartAddress, name, robSafe, checkedTecnologies, type, importToTia);
            Changes = true;
        }

        /// <summary>
        /// Used to validate robot name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool ValidateName(string name)
        {
            bool valid = true;

            if (name.Length != 9 || !name.Contains('R') || !name[6].Equals('R'))
                valid = false;

            return valid;
        }

        /// <summary>
        /// Add Tecnologies to ComboBoxes of DataGrid
        /// </summary>
        private void AddTecnologies(string type)
        {
            tecnologies = new List<string>();
            
            for (int i = 0; i < RobTecnologies.Count; i++)
            {
                for (int x = 0; x < RobTecnologies[i].Count; x++)
                {
                    var name = RobTecnologies[i][x].Name;
                    var tecType = RobTecnologies[i][x].Type;

                    if (char.IsWhiteSpace(name[0]))
                        name = name.Substring(1);

                    if (!tecnologies.Contains(name) && tecType == type)
                        tecnologies.Add(name);
                }
            }

            tecnologies.Sort();
            datagrid.ItemsSource = null;
            datagrid.ItemsSource = tecnologies;
        }

        /// <summary>
        /// Fill "form" with Robot Information
        /// </summary>
        private void FillWithRobInfo()
        {
            if (robInfo.Safe.Contains("Monitoring"))
                cbRobSafe.SelectedItem = cbRobSafe.Items[0];
            else
                cbRobSafe.SelectedItem = cbRobSafe.Items[1];

            if (robInfo.Type.Contains("Basic"))
                cbType.SelectedItem = cbType.Items[0];
            else
                cbType.SelectedItem = cbType.Items[1];

            // Tecnologies are added in "Window_Loaded" event

            RobName.Text = robInfo.Name;
            robNameImage.Text = robInfo.Name;
            StartAddress.Text = robInfo.StartAddress.ToString();
        }

        #region Window Events
        /// <summary>
        /// Auto-increment row number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        /// <summary>
        /// Only digits in StartAddress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartAddress_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// On text change inside "Name" textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RobName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            RobName.ClearValue(Border.BorderBrushProperty);
            robNameImage.Text = textBox.Text;
        }

        /// <summary>
        /// On text change inside "StartAddress" textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            StartAddress.ClearValue(Border.BorderBrushProperty);
        }

        /// <summary>
        /// Handles CbType_SelectionChanged event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            string selected = cmb.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last();

            AddTecnologies(selected);
        }

        /// <summary>
        /// Handles "Window_Loaded" event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Check if Tia Portal is connected
            if (Current == null)
            {
                IsTiaConnected = false;
                cbImportToTia.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#D3D3D3");
            }
            else
                IsTiaConnected = true;

            cbImportToTia.DataContext = this;

            if (robInfo == null) return;

            List<string> robTecnologies = robInfo.Tecnologies.Split(',').ToList();

            for (int i = 0; i < datagrid.Items.Count; i++)
            {
                var item = datagrid.Items[i];
                var checkboxTemplate = datagrid.Columns[1].GetCellContent(item);
                CheckBox checkbox = uFindVisualChild.FindVisualChild<CheckBox>(checkboxTemplate);

                for (int x = 0; x < robTecnologies.Count; x++)
                {
                    if (checkbox != null && tecnologies[i].Contains(robTecnologies[x]))
                        checkbox.IsChecked = true;
                }
            }
        }

        /// <summary>
        /// Handles "Window_Closed" event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}

using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using Siemens.Engineering;
using Siemens.Engineering.Library.MasterCopies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using TiaOpennessHelper;
using TiaPortalOpennessDemo.Commands;
using TiaPortalOpennessDemo.Utilities;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Threading;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Threading;

namespace TiaPortalOpennessDemo.ViewModels
{
    public class HardwareGeneratorViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Properties
        /// <summary>
        /// The Libraries
        /// </summary>
        private List<string> _libraries;
        /// <summary>
        /// Gets or sets the Libraries.
        /// </summary>
        /// <value>The Libraries.</value>
        public List<string> Libraries
        {
            get { return _libraries; }
            private set
            {
                if (_libraries == value)
                {
                    return;
                }
                _libraries = value;
                RaisePropertyChanged("Libraries");
            }
        }
        /// <summary>
        /// The worksheets
        /// </summary>
        private List<string> _worksheets;
        /// <summary>
        /// Gets or sets the worksheets.
        /// </summary>
        /// <value>The worksheets.</value>
        public List<string> Worksheets
        {
            get { return _worksheets; }
            private set
            {
                if (_worksheets == value)
                {
                    return;
                }
                _worksheets = value;
                RaisePropertyChanged("Worksheets");
            }
        }

        /// <summary>
        /// The SelectedLib
        /// </summary>
        private string _selectedLib;
        /// <summary>
        /// Gets or sets the SelectedLib
        /// </summary>
        /// <value>The SelectedLib.</value>
        public string SelectedLib
        {
            get { return _selectedLib; }
            set
            {
                if (!string.Equals(_selectedLib, value))
                {
                    _selectedLib = value;
                    RaisePropertyChanged("SelectedLib");
                }
            }
        }
        /// <summary>
        /// The LibraryPath
        /// </summary>
        private string _libraryPath;
        /// <summary>
        /// Gets or sets the LibraryPath
        /// </summary>
        /// <value>The LibraryPath.</value>
        public string LibraryPath
        {
            get { return _libraryPath; }
            set
            {
                if (!string.Equals(_libraryPath, value))
                {
                    _libraryPath = value;
                    RaisePropertyChanged("LibraryPath");
                }
            }
        }
        /// <summary>
        /// The TxtChooseLibType
        /// </summary>
        private string _txtChooseLibType;
        /// <summary>
        /// Gets or sets the TxtChooseLibType
        /// </summary>
        /// <value>The TxtChooseLibType.</value>
        public string TxtChooseLibType
        {
            get { return _txtChooseLibType; }
            set
            {
                if (!string.Equals(_txtChooseLibType, value))
                {
                    _txtChooseLibType = value;
                    RaisePropertyChanged("TxtChooseLibType");
                }
            }
        }
        /// <summary>
        /// The TxtStatus
        /// </summary>
        private string _txtStatus;
        /// <summary>
        /// Gets or sets the TxtStatus
        /// </summary>
        /// <value>The TxtStatus.</value>
        public string TxtStatus
        {
            get { return _txtStatus; }
            set
            {
                if (!string.Equals(_txtStatus, value))
                {
                    _txtStatus = value;
                    RaisePropertyChanged("TxtStatus");
                }
            }
        }

        /// <summary>
        /// The BtnEnabled
        /// </summary>
        public bool _btnEnabled;
        /// <summary>
        /// Gets or sets the BtnEnabled
        /// </summary>
        /// <value>The BtnEnabled.</value>
        public bool BtnEnabled
        {
            get { return _btnEnabled; }
            set
            {
                if (_btnEnabled == value)
                    return;
                _btnEnabled = value;
                RaisePropertyChanged("BtnEnabled");
            }
        }
        /// <summary>
        /// The WindowEnabled
        /// </summary>
        public bool _windowEnabled;
        /// <summary>
        /// Gets or sets the WindowEnabled
        /// </summary>
        /// <value>The WindowEnabled.</value>
        public bool WindowEnabled
        {
            get { return _windowEnabled; }
            set
            {
                if (_windowEnabled == value)
                    return;
                _windowEnabled = value;
                RaisePropertyChanged("WindowEnabled");
            }
        }

        /// <summary>
        /// The TIALibrary
        /// </summary>
        private Visibility _tiaLibrary;
        /// <summary>
        /// Gets or sets the TIALibrary
        /// </summary>
        /// <value>The TIALibrary.</value>
        public Visibility TIALibrary
        {
            get { return _tiaLibrary; }
            set
            {
                if (!string.Equals(_tiaLibrary, value))
                {
                    _tiaLibrary = value;
                    RaisePropertyChanged("TIALibrary");
                }
            }
        }
        /// <summary>
        /// The FromPathLibrary
        /// </summary>
        private Visibility _fromPathLibrary;
        /// <summary>
        /// Gets or sets the FromPathLibrary
        /// </summary>
        /// <value>The FromPathLibrary.</value>
        public Visibility FromPathLibrary
        {
            get { return _fromPathLibrary; }
            set
            {
                if (!string.Equals(_fromPathLibrary, value))
                {
                    _fromPathLibrary = value;
                    RaisePropertyChanged("FromPathLibrary");
                }
            }
        }

        /// <summary>
        /// The Project Tree
        /// </summary>
        private TreeViewHandler _projectTree;
        /// <summary>
        /// Gets or sets the project tree
        /// </summary>
        /// <value>The Project Tree.</value>
        public TreeViewHandler ProjectTree
        {
            get { return _projectTree; }
            set
            {
                if (_projectTree == value)
                    return;
                _projectTree = value;
                RaisePropertyChanged("ProjectTree");
            }
        }

        /// <summary>
        /// The GenerateHardware Command
        /// </summary>
        public CommandBase GenerateHardwareCommand { get; set; }
        /// <summary>
        /// The ChooseLibraryPath Command
        /// </summary>
        public CommandBase ChooseLibraryPathCommand { get; set; }
        /// <summary>
        /// The ChooseLibTypeCommand Command
        /// </summary>
        public CommandBase ChooseLibTypeCommand { get; set; }

        private TiaPortal tiaPortal;
        private Project tiaPortalProject;
        private string networkListPath;
        private string eplanPath;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public HardwareGeneratorViewModel()
        {
            Initialize();
        }

        /// <summary>
        /// Constructor w/Paramateres
        /// </summary>
        /// <param name="libraries"></param>
        /// <param name="worksheets"></param>
        /// <param name="tiaPortal"></param>
        /// <param name="tiaPortalProject"></param>
        public HardwareGeneratorViewModel(string networkListPath, string eplanPath, TiaPortal tiaPortal, Project tiaPortalProject)
        {
            Initialize();
            this.tiaPortal = tiaPortal;
            this.tiaPortalProject = tiaPortalProject;
            this.networkListPath = networkListPath;
            this.eplanPath = eplanPath;
            PopulateTreeView();
            PopulateLibs();
        }

        /// <summary>
        /// Initialize Class Components
        /// </summary>
        private void Initialize()
        {
            WindowEnabled = true;
            BtnEnabled = true;
            TxtStatus = "Status:  Waiting...";
            TIALibrary = Visibility.Visible;
            FromPathLibrary = Visibility.Hidden;
            TxtChooseLibType = "Choose Path  ";
            Libraries = new List<string>();
            Worksheets = new List<string>();
            GenerateHardwareCommand = new CommandBase(GenerateHardwareCommand_Executed);
            ChooseLibraryPathCommand = new CommandBase(ChooseLibraryPathCommand_Executed);
            ChooseLibTypeCommand = new CommandBase(ChooseLibTypeCommand_Executed);
            ProjectTree = new TreeViewHandler(new RelayCommand<DependencyPropertyEventArgs>(TreeViewItemSelectedChangedCallback));
        }

        /// <summary>
        /// Generate Tree View
        /// </summary>
        private void PopulateTreeView()
        {
            var xlApp = new Excel.Application();
            var xlWorkbook = OpennessHelper.GetExcelFile(networkListPath, xlApp);

            foreach (Excel.Worksheet xlWorksheet in xlWorkbook.Worksheets)
            {
                string sheetName = xlWorksheet.Name;
                var matrix = OpennessHelper.ExcelToMatrix(xlWorksheet);

                if (OpennessHelper.IsNetworkList(matrix))
                {
                    Worksheets.Add(sheetName);
                }
            }

            xlWorkbook.Close(0);
            xlApp.Quit();

            var projectTreeView = new TreeView();

            foreach (var ws in Worksheets)
            {
                TreeViewItem tvi = new TreeViewItem
                {
                    Tag = ws,
                    Header = new CheckBox()
                    {
                        Content = new TextBlock()
                        {
                            Text = ws
                        },
                        Tag = ws
                    }
                };
                projectTreeView.Items.Add(tvi);
            }

            if(projectTreeView.Items.Count == 0)
            {
                TreeViewItem tvi = new TreeViewItem
                {
                    Header = "Excel does not contain a valid Worksheet"
                };
                projectTreeView.Items.Add(tvi);

                BtnEnabled = false;
            }

            ProjectTree.Refresh(projectTreeView);
        }

        /// <summary>
        /// Populate libraries combobox
        /// </summary>
        private void PopulateLibs()
        {
            List<string> libs = OpennessHelper.GetLibrariesNamesFromTIA(tiaPortal);

            if (libs.Count == 0)
                libs.Add("No libraries found.");

            Libraries = libs;
        }

        /// <summary>TreeViews the item selected changed callback.</summary>
        /// <param name="e">The <see cref="DependencyPropertyEventArgs"/> instance containing the event data.</param>
        public void TreeViewItemSelectedChangedCallback(DependencyPropertyEventArgs e)
        {
            _projectTree.SelectedItem = e.DependencyPropertyChangedEventArgs.NewValue as TreeViewItem;
            var selectedTreeViewObject = _projectTree.SelectedItem;
            RaisePropertyChanged("ProjectTree");

            if (selectedTreeViewObject != null)
            {

            }
        }

        #region Commands
        /// <summary>
        /// Event handler generate hardware button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateHardwareCommand_Executed(object sender, EventArgs e)
        {
            List<string> sheetsToUse = GetSelectedSheets();
            if (!StartConditions(sheetsToUse)) return;

            TaskScheduler uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            Task.Factory.StartNew(() =>
            {
                WindowEnabled = false;
                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkbook = OpennessHelper.GetExcelFile(networkListPath, xlApp);
                Excel.Worksheet xlWorksheet;
                List<AddressData> addressDataList = new List<AddressData>(); //1st IP 2nd Start Address
                List<DeviceData> DeviceDataList = new List<DeviceData>();
                List<int> pneuList = new List<int>();
                List<int> t200List = new List<int>();
                List<int> motorsList = new List<int>();
                List<int> murrDi6List = new List<int>();
                List<int> listCoupler = new List<int>();
                List<List<string>> HWInformation = null;
                List<string> linesL = new List<string>();
                List<string> delimL = new List<string>();

                UpdateStatus("Generating...", uiScheduler);
                List<int> garetelistPages = OpennessHelper.GetGaretelistePages(eplanPath);

                //UpdateStatus("Getting devices from Worksheets...", uiScheduler);
                //GET ALL THE DEVICES FROM EXCEL FILE
                foreach (var sheet in sheetsToUse)
                {
                    xlWorksheet = xlWorkbook.Sheets[sheet];
                    DeviceDataList = OpennessHelper.GetAllDevicesNetworkList(xlWorksheet);
                }

                //UpdateStatus("Getting Library...", uiScheduler);

                xlWorkbook.Close(0);
                xlApp.Quit();

                MasterCopySystemFolder MasterCopy = null;

                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(() =>
                {
                    if (FromPathLibrary == Visibility.Visible)
                        MasterCopy = OpennessHelper.GetHWMasterCopies(tiaPortal, _libraryPath, true);
                    else
                        MasterCopy = OpennessHelper.GetHWMasterCopies(tiaPortal, _selectedLib, false);
                }));

                //UpdateStatus("Creating Hardware...", uiScheduler);

                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(() =>
                {
                    using (var access = tiaPortal.ExclusiveAccess("Creating Hardware..."))
                    {
                        foreach (DeviceData device in DeviceDataList)
                        {
                            string type = device.FGroup.Substring(3, 3);

                            switch (type)
                            {
                                case "TRR":
                                    OpennessHelper.InsertHWOperatorDoor(device.deviceName, device.addressData.IPAdress, device.addressData.StartAddress, tiaPortalProject, MasterCopy); //ALBANY
                                    break;

                                case "RB_":
                                    if (device.deviceName.Substring(14, 3) == "-kf")
                                        OpennessHelper.InsertHWRobot(device.deviceName, device.addressData.IPAdress, device.addressData.StartAddress, tiaPortalProject); //ROBOT
                                    else
                                        OpennessHelper.InsertHWRobotScalance(device.deviceName, device.addressData.IPAdress, tiaPortalProject); //ROBOT SWITCH
                                    break;

                                case "STU":///SAFETY DOOR
                                    OpennessHelper.InsertHWEuchner(device.deviceName, device.addressData.IPAdress, device.addressData.StartAddress, !device.option, tiaPortalProject, MasterCopy);
                                    break;

                                case "VRF": ///FESTO
                                case "DTW":
                                case "LME":
                                case "VR_":
                                case "GST":
                                case "LE_":
                                case "VRX":
                                case "SAE":
                                case "VRE":
                                case "ABS":
                                    pneuList = new List<int>(); // Reset List

                                    HWInformation = OpennessHelper.HWInfo(eplanPath, device.FGroup, device.identifier, garetelistPages);

                                    if (HWInformation != null)    // If List has elements
                                    {
                                        int count16DI = 0, countDoH = 0, countFDI = 0, countFDO = 0;
                                        foreach (List<string> info in HWInformation)
                                        {
                                            string part = OpennessHelper.GetHWPart(info[1]);
                                            if (part.Contains("16DI")) count16DI++;
                                            else if (part.Contains("DO-H")) countDoH++;
                                            else if (part.Contains("F8DI")) countFDI++;
                                            else if (part.Contains("FVDO")) countFDO++;
                                        }

                                        pneuList.Add(count16DI); // 16DI
                                        pneuList.Add(countDoH);  // DO-H
                                        pneuList.Add(countFDI);  // FDI
                                        pneuList.Add(countFDO);  // FDO
                                    }
                                    else
                                    {
                                        pneuList.Add(1); //1 - 16DI
                                        pneuList.Add(1); //1 - DO-H
                                        pneuList.Add(1); //1 - FDI
                                        pneuList.Add(1); //1 - FDO
                                    }

                                    if (device.deviceName.Substring(12, 2) != "ta")
                                        OpennessHelper.InsertHWPneumatic(device.deviceName, device.addressData.IPAdress, device.addressData.StartAddress, pneuList, tiaPortalProject);
                                    break;

                                case "LS_":
                                    OpennessHelper.InsertHwPLS(device.deviceName, device.addressData.IPAdress, device.addressData.StartAddress, tiaPortalProject, device.option, MasterCopy); //PLS CAN BE SICK S3000 or KEYENCE 
                                    break;

                                case "BR_":
                                case "BS_":
                                case "BRT":
                                    if (device.deviceName.Substring(15, 2) == "kf")
                                        OpennessHelper.InsertHWKP32F(device.deviceName, device.addressData.IPAdress, device.addressData.StartAddress, tiaPortalProject, MasterCopy);
                                    if (device.deviceName.Substring(15, 2) == "xf")
                                        OpennessHelper.InsertHWScalanceKP32F(device.deviceName, device.addressData.IPAdress, tiaPortalProject, MasterCopy);
                                    break;

                                //case "ek-": //et200F or LocBox
                                case "PMF": //et200s
                                case "EE_": //gateway or et200sp
                                    t200List = new List<int>(); // Reset List
                                    t200List.Add(1);    // 1 - 8DI
                                    t200List.Add(1);    // 1 - 16DI
                                    t200List.Add(1);    // 1 - 8DQ
                                    t200List.Add(1);    // 1 - 16DQ
                                    t200List.Add(1);    // 1 - F-8DI
                                    t200List.Add(1);    // 1 - F-8DQ
                                    t200List.Add(1);    // 1 - SvModule
                                    OpennessHelper.InsertHWT200(device.deviceName, device.addressData.IPAdress, device.addressData.StartAddress, t200List, tiaPortalProject);
                                    break;

                                case "WAS": // vi switch
                                    OpennessHelper.InsertHWEuchnermgb(device.deviceName, device.addressData.IPAdress, tiaPortalProject);
                                    break;

                                case "ZV_":
                                case "RF_":
                                case "QF_":
                                case "PNE"://MURR
                                case "SPX":
                                    murrDi6List = new List<int>();
                                    murrDi6List.Add(1); // IO-Link Output
                                    murrDi6List.Add(1); // IO-Link input/output
                                    if (device.terminalType.Contains("FDI8"))
                                        OpennessHelper.InsertHWMURRFDI8FDO4MVK(device.deviceName, device.addressData.IPAdress, device.addressData.StartAddress, tiaPortalProject);
                                    else if (device.terminalType.Contains("DI6"))
                                        OpennessHelper.InsertHWMURRDI6DO6MVK(device.deviceName, murrDi6List, device.addressData.IPAdress, device.addressData.StartAddress, tiaPortalProject);
                                    else if (device.terminalType.Contains("Lenze"))
                                    {
                                        motorsList = new List<int>(); // Reset List
                                        motorsList.Add(1);  // PZD
                                        motorsList.Add(1);  // Safety
                                        OpennessHelper.InsertHWLenze(device.deviceName, device.addressData.IPAdress, device.addressData.StartAddress, motorsList, tiaPortalProject, "RF_");
                                    }
                                    break;

                                case "KH_":
                                case "RF2":
                                    break;

                                //Motores
                                case "HE_":
                                case "HER":
                                case "FX_":
                                case "HTS":
                                    motorsList = new List<int>(); // Reset List
                                    motorsList.Add(1);  // PZD
                                    motorsList.Add(1);  // Safety
                                    OpennessHelper.InsertHWLenze(device.deviceName, device.addressData.IPAdress, device.addressData.StartAddress, motorsList, tiaPortalProject, type);
                                    break;

                                case "DGT":
                                    OpennessHelper.InsertHWIDENTControl(device.deviceName, device.addressData.IPAdress, tiaPortalProject);
                                    break;

                                case "DGC":
                                    OpennessHelper.InsertHWMV5(device.deviceName, device.addressData.IPAdress, tiaPortalProject);
                                    break;

                                case "EV_": //Coupler X1
                                case "XEV": //Coupler X2
                                    listCoupler = new List<int>();
                                    listCoupler.Add(1);   // In 32 Bytes
                                    listCoupler.Add(1);   // Out 32 Bytes
                                    listCoupler.Add(1);   // PROFIsafe in/out 6 byte
                                    listCoupler.Add(1);   // PROFIsafe in/out 12 Byte

                                    OpennessHelper.InsertHWCoupler(device.deviceName, device.addressData.IPAdress, device.addressData.StartAddress, listCoupler, type, tiaPortalProject);
                                    break;

                                //CONTINUE WITH ALL DEVICES FROM THE DEVICELIST FROM NETWORK LIST
                                default:
                                    break;
                            }
                        }
                    }
                }));
            }).ContinueWith(t2 =>
            {
                WindowEnabled = true;
                UpdateStatus("Done!", uiScheduler);
            });
        }

        /// <summary>
        /// Event handler choose library path button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseLibraryPathCommand_Executed(object sender, EventArgs e)
        {
            var pathSave = new OpenFileDialog();
            pathSave.Filter = "TIA Library|*.zal15;*.zal15_1";
            pathSave.ShowDialog();
            if (File.Exists(pathSave.FileName))
                LibraryPath = pathSave.FileName;
        }

        /// <summary>
        /// Event handler choose library type button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseLibTypeCommand_Executed(object sender, EventArgs e)
        {
            if (TIALibrary == Visibility.Visible)
            {
                TIALibrary = Visibility.Hidden;
                FromPathLibrary = Visibility.Visible;
                SelectedLib = "";
                TxtChooseLibType = "Choose TIA Library  ";
            }
            else
            {
                FromPathLibrary = Visibility.Hidden;
                TIALibrary = Visibility.Visible;
                LibraryPath = "";
                TxtChooseLibType = "Choose Path  ";
            }
        }
        #endregion

        /// <summary>
        /// Get selected worksheets from TreeView
        /// </summary>
        /// <returns></returns>
        private List<string> GetSelectedSheets()
        {
            List<string> list = new List<string>();
            foreach (TreeViewItem item in ProjectTree.View[0].Items)
            {
                if (((CheckBox)item.Header).IsChecked == true)
                {
                    string itemName = ((TextBlock)((CheckBox)item.Header).Content).Text;
                    list.Add(itemName);
                }
            }
            return list;
        }
    
        /// <summary>
        /// Update label status text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="uiScheduler"></param>
        private void UpdateStatus(string text, TaskScheduler uiScheduler)
        {
            Task.Factory.StartNew(() => 
                Task.Factory.StartNew(() =>
                {
                    TxtStatus = "Status:  " + text;
                }, CancellationToken.None, TaskCreationOptions.None, uiScheduler)
            );
        }

        /// <summary>
        /// Check if program has start conditions
        /// </summary>
        /// <param name="sheetsSelected"></param>
        /// <returns></returns>
        private bool StartConditions(List<string> sheetsSelected)
        {
            if (sheetsSelected.Count == 0) 
                return false;

            if (string.IsNullOrEmpty(_libraryPath) && string.IsNullOrEmpty(_selectedLib))
                return false;

            return true;
        }
    }
}

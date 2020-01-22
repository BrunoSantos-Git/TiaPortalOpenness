using System;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Threading;
using MessageBox = System.Windows.Forms.MessageBox;
using Siemens.Engineering;
using Siemens.Engineering.Compare;
using Siemens.Engineering.Compiler;
using Siemens.Engineering.Hmi;
using Siemens.Engineering.Hmi.Communication;
using Siemens.Engineering.Hmi.Cycle;
using Siemens.Engineering.Hmi.Globalization;
using Siemens.Engineering.Hmi.RuntimeScripting;
using Siemens.Engineering.Hmi.Screen;
using Siemens.Engineering.Hmi.Tag;
using Siemens.Engineering.Hmi.TextGraphicList;
using Siemens.Engineering.HW;
using Siemens.Engineering.HW.Features;
using Siemens.Engineering.Library;
using Siemens.Engineering.Library.MasterCopies;
using Siemens.Engineering.Library.Types;
using Siemens.Engineering.Online;
using Siemens.Engineering.SW;
using Siemens.Engineering.SW.Blocks;
using Siemens.Engineering.SW.ExternalSources;
using Siemens.Engineering.SW.Tags;
using Siemens.Engineering.SW.Types;
using TiaOpennessHelper;
using TiaPortalOpennessDemo.Commands;
using TiaPortalOpennessDemo.Services;
using TiaPortalOpennessDemo.Utilities;
using TiaPortalOpennessDemo.Views;
using Application = System.Windows.Application;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using Screen = Siemens.Engineering.Hmi.Screen.Screen;
using TreeView = System.Windows.Controls.TreeView;
using View = Siemens.Engineering.HW.View;
using TiaPortalOpennessDemo.Properties;
using System.Diagnostics;
using System.Xml;
using TiaOpennessHelper.VWSymbolic;
using TiaOpennessHelper.XMLParser;
using System.Xml.Schema;
using System.Xml.Linq;
using TiaOpennessHelper.Utils;
using TiaOpennessHelper.ExcelTree;
using TiaOpennessHelper.SafetyMaker;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Reflection;

namespace TiaPortalOpennessDemo.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="TiaPortalOpennessDemo.ViewModels.ViewModelBase" />
    /// <seealso cref="System.IDisposable" />
    /// TODO Edit XML Comment Template for MainWindowViewModel
    public class MainWindowViewModel : ViewModelBase, IDisposable
    {
        #region Fields
        /// <summary>The tia portal</summary>
        /// TODO Edit XML Comment Template for tiaPortal
        private TiaPortal _tiaPortal;
        /// <summary>The tia portal project</summary>
        /// TODO Edit XML Comment Template for tiaPortalProject
        private Project _tiaPortalProject;
        /// <summary>The tia portal projects</summary>
        /// TODO Edit XML Comment Template for tiaPortalProjects
        private ProjectComposition _tiaPortalProjects;
        /// <summary>The tia global library</summary>
        /// TODO Edit XML Comment Template for tiaGlobalLibrary
        private UserGlobalLibrary _tiaGlobalLibrary;
        /// <summary>The PLCS to compile</summary>
        /// TODO Edit XML Comment Template for plcsToCompile
        private HashSet<PlcSoftware> _plcsToCompile;

        private Transaction _action;
        private ExclusiveAccess _access;

        /// <summary>The sub window</summary>
        /// TODO Edit XML Comment Template for subWindow
        private Window _subWindow;

        /// <summary>The project name</summary>
        /// TODO Edit XML Comment Template for projectName
        private string _projectName = string.Empty;
        /// <summary>The export path</summary>
        /// TODO Edit XML Comment Template for exportPath
        private string _exportPath = string.Empty;

        #endregion

        #region Properties
        #region Collections
        /// <summary>
        /// The Robot Base
        /// </summary>
        private List<List<RobotBase>> RobBase;
        /// <summary>
        /// The Robot Tecnologies
        /// </summary>
        private List<List<RobotTecnologie>> RobTecnologies;
        /// <summary>
        /// The Robot Safe Range Monitoring
        /// </summary>
        private List<List<RobotSafeRangeMonitoring>> RobSafeRangeMonitoring;
        /// <summary>
        /// The Robot Safe Operations
        /// </summary>
        private List<List<RobotSafeOperation>> RobSafeOperations;
        /// <summary>
        /// The Robot Info
        /// </summary>
        private List<RobotInfo> RobsInfo;
        /// <summary>
        /// Store expanded TreeViewItems
        /// </summary>
        private List<string> expandedTvitems;

        /// <summary>
        /// Save matrix returned from PLC_Taps window
        /// </summary>
        private List<object[,]> MatrixList;
        /// <summary>
        /// Save sheet names returned from PLC_Taps window
        /// </summary>
        private List<string> SheetNamesList;
        /// <summary>
        /// Save plctags matrix returned from PLC_Taps window
        /// </summary>
        private object[,] PlcTagsMatrix;

        /// <summary>
        /// The Folders List
        /// </summary>
        private ObservableCollection<FolderInfo> _foldersList;
        /// <summary>
        /// Gets or sets the Folders List
        /// </summary>
        public ObservableCollection<FolderInfo> FoldersList
        {
            get { return _foldersList; }
            private set
            {
                if (_foldersList == value)
                {
                    return;
                }
                _foldersList = value;
                RaisePropertyChanged("FoldersList");
            }
        }

        /// <summary>The status ListView</summary>
        /// TODO Edit XML Comment Template for statusListView
        private ObservableCollection<string> _statusListView;
        /// <summary>Gets the status ListView.</summary>
        /// <value>The status ListView.</value>
        /// TODO Edit XML Comment Template for StatusListView
        public ObservableCollection<string> StatusListView
        {
            get { return _statusListView; }
            private set
            {
                if (_statusListView == value)
                {
                    return;
                }
                _statusListView = value;
                RaisePropertyChanged("StatusListView");
            }
        }

        /// <summary>The properties ListView</summary>
        /// TODO Edit XML Comment Template for propertiesListView
        private Dictionary<string, string> _propertiesListView;
        /// <summary>Gets the properties ListView.</summary>
        /// <value>The properties ListView.</value>
        /// TODO Edit XML Comment Template for PropertiesListView
        public Dictionary<string, string> PropertiesListView
        {
            get { return _propertiesListView; }
            private set
            {
                if (_propertiesListView == value)
                {
                    return;
                }
                _propertiesListView = value;
                RaisePropertyChanged("PropertiesListView");
            }
        }

        /// <summary>The project tree</summary>
        /// TODO Edit XML Comment Template for projectTree
        private TreeViewHandler _projectTree;
        /// <summary>Gets or sets the project tree.</summary>
        /// <value>The project tree.</value>
        /// TODO Edit XML Comment Template for ProjectTree
        public TreeViewHandler ProjectTree
        {
            get { return _projectTree; }
            set
            {
                if (_projectTree == value)
                {
                    return;
                }
                _projectTree = value;
                RaisePropertyChanged("ProjectTree");
            }
        }

        /// <summary>The main tree</summary>
        /// TODO Edit XML Comment Template for mainTree
        private TreeViewHandler _mainTree;
        /// <summary>Gets or sets the main tree.</summary>
        /// <value>The project tree.</value>
        /// TODO Edit XML Comment Template for MainTree
        public TreeViewHandler MainTree
        {
            get { return _mainTree; }
            set
            {
                if (_mainTree == value)
                {
                    return;
                }
                _mainTree = value;
                RaisePropertyChanged("MainTree");
            }
        }

        /// <summary>The library tree</summary>
        /// TODO Edit XML Comment Template for libraryTree
        private TreeViewHandler _libraryTree;
        /// <summary>Gets or sets the library tree.</summary>
        /// <value>The library tree.</value>
        /// TODO Edit XML Comment Template for LibraryTree
        public TreeViewHandler LibraryTree
        {
            get { return _libraryTree; }
            set
            {
                if (_libraryTree == value)
                    return;
                _libraryTree = value;
                RaisePropertyChanged("LibraryTree");
            }
        }

        /// <summary>The global library tree</summary>
        /// TODO Edit XML Comment Template for globalLibraryTree
        private TreeViewHandler _globalLibraryTree;
        /// <summary>Gets or sets the global library tree.</summary>
        /// <value>The global library tree.</value>
        /// TODO Edit XML Comment Template for GlobalLibraryTree
        public TreeViewHandler GlobalLibraryTree
        {
            get { return _globalLibraryTree; }
            set
            {
                if (_globalLibraryTree == value)
                    return;
                _globalLibraryTree = value;
                RaisePropertyChanged("GlobalLibraryTree");
            }
        }

        /// <summary>The symbolics tree</summary>
        /// TODO Edit XML Comment Template for symbolicsTree
        private TreeViewHandler _symbolicsTree;
        /// <summary>Gets or sets the symbolics tree.</summary>
        /// <value>The global library tree.</value>
        /// TODO Edit XML Comment Template for SymbolicsTree
        public TreeViewHandler SymbolicsTree
        {
            get { return _symbolicsTree; }
            set
            {
                if (_symbolicsTree == value)
                    return;
                _symbolicsTree = value;
                RaisePropertyChanged("SymbolicsTree");
            }
        }
        #endregion

        /// <summary>
        /// Gets or sets the value of combobox selected item
        /// </summary>
        public FolderInfo CbSelectedItem { get; set; }

        #region EnableBits
        /// <summary>The portal opened</summary>
        /// TODO Edit XML Comment Template for portalOpened
        private bool _portalOpened;
        /// <summary>
        /// Gets or sets a value indicating whether [portal opened].
        /// </summary>
        /// <value><c>true</c> if [portal opened]; otherwise, <c>false</c>.</value>
        /// TODO Edit XML Comment Template for PortalOpened
        public bool PortalOpened
        {
            get { return _portalOpened; }
            set
            {
                if (_portalOpened == value)
                    return;
                _portalOpened = value;
                RaisePropertyChanged("PortalOpened");
            }
        }

        /// <summary>
        /// To check if robotview window is open
        /// </summary>
        private bool _robotViewOpened;
        /// <summary>
        /// Gets or sets a value indicating whether [RobotView opened].
        /// </summary>
        /// <value><c>true</c> if [RobotView opened]; otherwise, <c>false</c>.</value>
        public bool RobotViewOpened
        {
            get { return _robotViewOpened; }
            set
            {
                _robotViewOpened = value;
                RaisePropertyChanged("RobotViewOpened");
            }
        }

        /// <summary>
        /// To check if already existed a tia portal connection
        /// before initializing PLC Renamer window
        /// </summary>
        private bool tiaPortalConnected;

        /// <summary>The project opened</summary>
        /// TODO Edit XML Comment Template for projectOpened
        private bool _projectOpened;
        /// <summary>
        /// Gets or sets a value indicating whether [project opened].
        /// </summary>
        /// <value><c>true</c> if [project opened]; otherwise, <c>false</c>.</value>
        /// TODO Edit XML Comment Template for ProjectOpened
        public bool ProjectOpened
        {
            get { return _projectOpened; }
            set
            {
                if (_projectOpened == value)
                {
                    return;
                }

                _projectOpened = value;
                RaisePropertyChanged("ProjectOpened");
            }
        }

        /// <summary>
        /// The global library opened
        /// </summary>
        /// TODO Edit XML Comment Template for globalLibraryOpened
        private bool _globalLibraryOpened;
        /// <summary>
        /// Gets or sets a value indicating whether [global library opened].
        /// </summary>
        /// <value><c>true</c> if [global library opened]; otherwise, <c>false</c>.</value>
        /// TODO Edit XML Comment Template for GlobalLibraryOpened
        public bool GlobalLibraryOpened
        {
            get { return _globalLibraryOpened; }
            set
            {
                if (_globalLibraryOpened == value)
                    return;
                _globalLibraryOpened = value;
                RaisePropertyChanged("GlobalLibraryOpened");
            }
        }

        /// <summary>The create item</summary>
        /// TODO Edit XML Comment Template for createItem
        private bool _createItem;
        /// <summary>
        /// Gets or sets a value indicating whether [create item].
        /// </summary>
        /// <value><c>true</c> if [create item]; otherwise, <c>false</c>.</value>
        /// TODO Edit XML Comment Template for CreateItem
        public bool CreateItem
        {
            get { return _createItem; }
            set
            {
                if (_createItem == value)
                    return;
                _createItem = value;
                RaisePropertyChanged("CreateItem");
            }
        }

        /// <summary>The delete item</summary>
        /// TODO Edit XML Comment Template for deleteItem
        private bool _deleteItem;
        /// <summary>
        /// Gets or sets a value indicating whether [delete item].
        /// </summary>
        /// <value><c>true</c> if [delete item]; otherwise, <c>false</c>.</value>
        /// TODO Edit XML Comment Template for DeleteItem
        public bool DeleteItem
        {
            get { return _deleteItem; }
            set
            {
                if (_deleteItem == value)
                    return;
                _deleteItem = value;
                RaisePropertyChanged("DeleteItem");
            }
        }

        /// <summary>The open editor</summary>
        /// TODO Edit XML Comment Template for openEditor
        private bool _openEditor;
        /// <summary>
        /// Gets or sets a value indicating whether [open editor].
        /// </summary>
        /// <value><c>true</c> if [open editor]; otherwise, <c>false</c>.</value>
        /// TODO Edit XML Comment Template for OpenEditor
        public bool OpenEditor
        {
            get { return _openEditor; }
            set
            {
                if (_openEditor == value)
                    return;
                _openEditor = value;
                RaisePropertyChanged("OpenEditor");
            }
        }

        /// <summary>The compile software</summary>
        /// TODO Edit XML Comment Template for compileSoftware
        private bool _compile;
        /// <summary>
        /// Gets or sets a value indicating whether [compile software].
        /// </summary>
        /// <value><c>true</c> if [compile software]; otherwise, <c>false</c>.</value>
        /// TODO Edit XML Comment Template for CompileSoftware
        public bool Compile
        {
            get { return _compile; }
            set
            {
                if (_compile == value)
                    return;
                _compile = value;
                RaisePropertyChanged("Compile");
            }
        }

        /// <summary>
        /// The plc tag
        /// </summary>
        private bool _plcTagSelected;
        /// <summary>
        /// Gets or sets a value indicating whether [Plc Tag is selected]
        /// </summary>
        public bool PLCTagSelected
        {
            get { return _plcTagSelected; }
            set
            {
                if (_plcTagSelected == value)
                    return;
                _plcTagSelected = value;
                RaisePropertyChanged("PLCTagSelected");
            }
        }

        private bool _caxImportVisible;

        public bool CaxImportVisible
        {
            get { return _caxImportVisible; }
            set
            {
                _caxImportVisible = value;
                RaisePropertyChanged(nameof(CaxImportVisible));
            }
        }

        /// <summary>The import enabled</summary>
        /// TODO Edit XML Comment Template for importEnabled
        private bool _importEnabled;
        /// <summary>
        /// Gets or sets a value indicating whether [import enabled].
        /// </summary>
        /// <value><c>true</c> if [import enabled]; otherwise, <c>false</c>.</value>
        /// TODO Edit XML Comment Template for ImportEnabled
        public bool ImportEnabled
        {
            get { return _importEnabled; }
            set
            {
                if (_importEnabled == value)
                {
                    return;
                }
                _importEnabled = value;
                RaisePropertyChanged("ImportEnabled");
            }
        }

        /// <summary>The export enabled</summary>
        /// TODO Edit XML Comment Template for exportEnabled
        private bool _exportEnabled;
        /// <summary>
        /// Gets or sets a value indicating whether [export enabled].
        /// </summary>
        /// <value><c>true</c> if [export enabled]; otherwise, <c>false</c>.</value>
        /// TODO Edit XML Comment Template for ExportEnabled
        public bool ExportEnabled
        {
            get { return _exportEnabled; }
            set
            {
                if (_exportEnabled == value)
                    return;
                _exportEnabled = value;
                RaisePropertyChanged("ExportEnabled");
            }
        }

        /// <summary>The add external source</summary>
        /// TODO Edit XML Comment Template for addExternalSource
        private bool _addExternalSource;
        /// <summary>
        /// Gets or sets a value indicating whether [add external source].
        /// </summary>
        /// <value><c>true</c> if [add external source]; otherwise, <c>false</c>.</value>
        /// TODO Edit XML Comment Template for AddExternalSource
        public bool AddExternalSource
        {
            get { return _addExternalSource; }
            set
            {
                if (_addExternalSource == value)
                    return;
                _addExternalSource = value;
                RaisePropertyChanged("AddExternalSource");
            }
        }

        /// <summary>
        /// The generate block from source
        /// </summary>
        /// TODO Edit XML Comment Template for generateBlockFromSource
        private bool _generateBlockFromSource;
        /// <summary>
        /// Gets or sets a value indicating whether [generate block from source].
        /// </summary>
        /// <value>
        /// <c>true</c> if [generate block from source]; otherwise, <c>false</c>.
        /// </value>
        /// TODO Edit XML Comment Template for GenerateBlockFromSource
        public bool GenerateBlockFromSource
        {
            get { return _generateBlockFromSource; }
            set
            {
                if (_generateBlockFromSource == value)
                    return;
                _generateBlockFromSource = value;
                RaisePropertyChanged("GenerateBlockFromSource");
            }
        }

        /// <summary>
        /// The generate source from block
        /// </summary>
        /// TODO Edit XML Comment Template for generateSourceFromBlock
        private bool _generateSourceFromBlock;
        /// <summary>
        /// Gets or sets a value indicating whether [generate source from block].
        /// </summary>
        /// <value>
        /// <c>true</c> if [generate source from block]; otherwise, <c>false</c>.
        /// </value>
        /// TODO Edit XML Comment Template for GenerateSourceFromBlock
        public bool GenerateSourceFromBlock
        {
            get { return _generateSourceFromBlock; }
            set
            {
                if (value == _generateSourceFromBlock)
                    return;
                _generateSourceFromBlock = value;
                RaisePropertyChanged("GenerateSourceFromBlock");
            }
        }

        /// <summary>
        /// The configure connection
        /// </summary>
        /// TODO Edit XML Comment Template for configureConnection
        private bool _configureConnection;
        /// <summary>
        /// Gets or sets a value indicating whether [configure connection].
        /// </summary>
        /// <value><c>true</c> if [configure connection]; otherwise, <c>false</c>.</value>
        /// TODO Edit XML Comment Template for ConfigureConnection
        public bool ConfigureConnection
        {
            get { return _configureConnection; }
            set
            {
                if (_configureConnection == value)
                    return;
                _configureConnection = value;
                RaisePropertyChanged("ConfigureConnection");
            }
        }

        /// <summary>The connect PLC</summary>
        /// TODO Edit XML Comment Template for connectPlc
        private bool _connectPlc;
        /// <summary>
        /// Gets or sets a value indicating whether [connect PLC].
        /// </summary>
        /// <value><c>true</c> if [connect PLC]; otherwise, <c>false</c>.</value>
        /// TODO Edit XML Comment Template for ConnectPlc
        public bool ConnectPlc
        {
            get { return _connectPlc; }
            set
            {
                if (_connectPlc == value)
                    return;
                _connectPlc = value;
                RaisePropertyChanged("ConnectPlc");
            }
        }

        /// <summary>The transaction running</summary>
        /// TODO Edit XML Comment Template for transactionRunning
        private bool _transactionRunning;
        /// <summary>
        /// Gets or sets a value indicating whether [transaction running].
        /// </summary>
        /// <value><c>true</c> if [transaction running]; otherwise, <c>false</c>.</value>
        /// TODO Edit XML Comment Template for TransactionRunning
        public bool TransactionRunning
        {
            get { return _transactionRunning; }
            set
            {
                if (_transactionRunning == value)
                    return;
                _transactionRunning = value;
                RaisePropertyChanged("TransactionRunning");
            }
        }

        /// <summary>
        /// The copy library element
        /// </summary>
        /// TODO Edit XML Comment Template for copyLibElement
        private bool _copyLibElement;
        /// <summary>
        /// Gets or sets a value indicating whether [copy library element].
        /// </summary>
        /// <value><c>true</c> if [copy library element]; otherwise, <c>false</c>.</value>
        /// TODO Edit XML Comment Template for CopyLibElement
        public bool CopyLibElement
        {
            get { return _copyLibElement; }
            set
            {
                if (_copyLibElement == value)
                    return;
                _copyLibElement = value;
                RaisePropertyChanged("CopyLibElement");
            }
        }

        /// <summary>The properties shown</summary>
        /// TODO Edit XML Comment Template for propertiesShown
        private bool _propertiesShown;
        /// <summary>
        /// Gets or sets a value indicating whether [properties shown].
        /// </summary>
        /// <value><c>true</c> if [properties shown]; otherwise, <c>false</c>.</value>
        /// TODO Edit XML Comment Template for PropertiesShown
        public bool PropertiesShown
        {
            get { return _propertiesShown; }
            set
            {
                if (_propertiesShown == value)
                    return;
                _propertiesShown = value;
                RaisePropertyChanged("PropertiesShown");
            }
        }

        /// <summary>The is loading</summary>
        /// TODO Edit XML Comment Template for isLoading
        private bool _isLoading;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is loading.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is loading; otherwise, <c>false</c>.
        /// </value>
        /// TODO Edit XML Comment Template for IsLoading
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                RaisePropertyChanged("IsLoading");
            }
        }

        #endregion

        #region Settings
        /// <summary>
        /// The default export folder path
        /// </summary>
        /// TODO Edit XML Comment Template for defaultExportFolderPath
        private string _defaultExportFolderPath = @"C:\Temp";
        /// <summary>
        /// The default robot list file path
        /// </summary>
        private string _defaultSchnittstellePath = "";
        /// <summary>
        /// The default sequence file path
        /// </summary>
        private string _defaultSequencePath = "";
        /// <summary>
        /// The default plc db file path
        /// </summary>
        private string _defaultPlcDBPath = "";
        /// <summary>
        /// The default network list path
        /// </summary>
        private string _defaultNetworkListPath = "";
        /// <summary>
        /// The default EPlanPath
        /// </summary>
        private string _defaultEPlanPath = "";
        /// <summary>
        /// The Main Folder path
        /// </summary>
        private string _mainFolderPath = @"C:\Temp\TiaPortalOpenness";
        /// <summary>Gets or sets the main folder path.</summary>
        /// <value>The main folder path.</value>
        /// TODO Edit XML Comment Template for DefaultExportFolderPath
        public string MainFolderPath
        {
            get
            {
                return _mainFolderPath;
            }

            set
            {
                if (_mainFolderPath == value)
                {
                    return;
                }
                _mainFolderPath = value;
                RaisePropertyChanged("MainFolderPath");
            }
        }
        /// <summary>Gets or sets the default export folder path.</summary>
        /// <value>The default export folder path.</value>
        /// TODO Edit XML Comment Template for DefaultExportFolderPath
        public string DefaultExportFolderPath
        {
            get
            {
                return _defaultExportFolderPath;
            }

            set
            {
                if (_defaultExportFolderPath == value)
                {
                    return;
                }
                _defaultExportFolderPath = value;
                RaisePropertyChanged("DefaultExportFolderPath");
            }
        }
        /// <summary>Gets or sets the default robot list file path.</summary>
        /// <value>The default export folder path.</value>
        public string DefaultSchnittstellePath
        {
            get
            {
                return _defaultSchnittstellePath;
            }

            set
            {
                if (_defaultSchnittstellePath == value)
                {
                    return;
                }
                _defaultSchnittstellePath = value;
                RaisePropertyChanged("DefaultSchnittstellePath");
            }
        }
        /// <summary>Gets or sets the default sequence file path.</summary>
        /// <value>The sequence file path.</value>
        public string DefaultSequencePath
        {
            get
            {
                return _defaultSequencePath;
            }

            set
            {
                if (_defaultSequencePath == value)
                {
                    return;
                }
                _defaultSequencePath = value;
                RaisePropertyChanged("DefaultSequencePath");
            }
        }
        /// <summary>Gets or sets the default PLC DB file path.</summary>
        /// <value>The PLC DB file path.</value>
        public string DefaultPlcDBPath
        {
            get
            {
                return _defaultPlcDBPath;
            }

            set
            {
                if (_defaultPlcDBPath == value)
                {
                    return;
                }
                _defaultPlcDBPath = value;
                RaisePropertyChanged("DefaultPlcDBPath");
            }
        }

        /// <summary>Gets or sets the default network list file path.</summary>
        /// <value>The network list file path.</value>
        public string DefaultNetworkListPath
        {
            get
            {
                return _defaultNetworkListPath;
            }

            set
            {
                if (_defaultNetworkListPath == value)
                {
                    return;
                }
                _defaultNetworkListPath = value;
                RaisePropertyChanged("DefaultNetworkListPath");
            }
        }
        /// <summary>Gets or sets the default eplan file path.</summary>
        /// <value>The EPlan file path.</value>
        public string DefaultEPlanPath
        {
            get
            {
                return _defaultEPlanPath;
            }

            set
            {
                if (_defaultEPlanPath == value)
                {
                    return;
                }
                _defaultEPlanPath = value;
                RaisePropertyChanged("DefaultEPlanPath");
            }
        }

        /// <summary>
        /// The user interface enabled
        /// </summary>
        /// TODO Edit XML Comment Template for userInterfaceEnabled
        private bool _userInterfaceEnabled = true;
        /// <summary>
        /// Gets or sets a value indicating whether [user interface enabled].
        /// </summary>
        /// <value>
        /// <c>true</c> if [user interface enabled]; otherwise, <c>false</c>.
        /// </value>
        /// TODO Edit XML Comment Template for UserInterfaceEnabled
        public bool UserInterfaceEnabled
        {
            get { return _userInterfaceEnabled; }
            set
            {
                if (_userInterfaceEnabled == value)
                    return;
                _userInterfaceEnabled = value;
                RaisePropertyChanged("UserInterfaceEnabled");
            }
        }

        /// <summary>
        /// The export options defaults
        /// </summary>
        /// TODO Edit XML Comment Template for exportOptionsDefaults
        private bool _exportOptionsDefaults;
        /// <summary>
        /// Gets or sets a value indicating whether [export options defaults].
        /// </summary>
        /// <value>
        /// <c>true</c> if [export options defaults]; otherwise, <c>false</c>.
        /// </value>
        /// TODO Edit XML Comment Template for ExportOptionsDefaults
        public bool ExportOptionsDefaults
        {
            get { return _exportOptionsDefaults; }
            set
            {
                if (_exportOptionsDefaults == value)
                    return;

                _exportOptionsDefaults = value;
                RaisePropertyChanged("ExportOptionsDefaults");
            }
        }

        /// <summary>
        /// The export options read only
        /// </summary>
        /// TODO Edit XML Comment Template for exportOptionsReadOnly
        private bool _exportOptionsReadOnly;
        /// <summary>
        /// Gets or sets a value indicating whether [export options read only].
        /// </summary>
        /// <value>
        /// <c>true</c> if [export options read only]; otherwise, <c>false</c>.
        /// </value>
        /// TODO Edit XML Comment Template for ExportOptionsReadOnly
        public bool ExportOptionsReadOnly
        {
            get { return _exportOptionsReadOnly; }
            set
            {
                if (_exportOptionsReadOnly == value)
                    return;

                _exportOptionsReadOnly = value;
                RaisePropertyChanged("ExportOptionsReadOnly");
            }
        }

        private bool _hideAssemblySelection;
        public bool HideAssemblySelection
        {
            get { return _hideAssemblySelection; }
            set
            {
                if (_hideAssemblySelection == value)
                    return;
                _hideAssemblySelection = value;
                RaisePropertyChanged(nameof(HideAssemblySelection));
            }
        }

        private string _engineeringVersion;
        public string EngineeringVersion
        {
            get { return _engineeringVersion; }
            set
            {
                if (_engineeringVersion == value)
                    return;
                _engineeringVersion = value;
                RaisePropertyChanged(nameof(EngineeringVersion));
            }
        }

        private string _assemblyVersion;
        public string AssemblyVersion
        {
            get { return _assemblyVersion; }
            set
            {
                if (_assemblyVersion == value)
                    return;
                _assemblyVersion = value;
                RaisePropertyChanged(nameof(AssemblyVersion));
            }
        }
        #endregion

        /// <summary>The copy destination</summary>
        /// TODO Edit XML Comment Template for copyDestination
        private TreeViewItem _copyDestination;
        /// <summary>Gets or sets the copy destination.</summary>
        /// <value>The copy destination.</value>
        /// TODO Edit XML Comment Template for CopyDestination
        public TreeViewItem CopyDestination
        {
            get { return _copyDestination; }
            set
            {
                if (Equals(value, _copyDestination))
                    return;
                _copyDestination = value;
                RaisePropertyChanged("CopyDestination");
            }
        }

        /// <summary>The copy source</summary>
        /// TODO Edit XML Comment Template for copySource
        private TreeViewItem _copySource;
        /// <summary>Gets or sets the copy source.</summary>
        /// <value>The copy source.</value>
        /// TODO Edit XML Comment Template for CopySource
        public TreeViewItem CopySource
        {
            get { return _copySource; }
            set
            {
                if (Equals(value, _copySource))
                    return;
                _copySource = value;
                RaisePropertyChanged("CopySource");
            }
        }

        /// <summary>
        /// Arg Folder
        /// </summary>
        private TreeViewItem _argFolder;
        /// <summary>
        /// Gets or sets the Arg Folder
        /// </summary>
        public TreeViewItem ArgFolder
        {
            get { return _argFolder; }
            set
            {
                if (Equals(value, _argFolder))
                    return;
                _argFolder = value;
                RaisePropertyChanged("ArgFolder");
            }
        }

        /// <summary>
        /// 0 = none, 1 = global library, 2 = project library
        /// </summary>
        private int _sourceIndex;
        /// <summary>
        /// 0 = none, 1 = project, 2 = project library
        /// </summary>
        private int _destinationIndex;

        private string _caxImportFilePath;
        public string CaxImportFilePath
        {
            get { return _caxImportFilePath; }
            set
            {
                _caxImportFilePath = value;
                RaisePropertyChanged(nameof(CaxImportFilePath));
            }
        }

        private ImportCaxOptions _selectedCaxImportOption;
        public ImportCaxOptions SelectedCaxImportOption
        {
            get { return _selectedCaxImportOption; }
            set
            {
                _selectedCaxImportOption = value;
                RaisePropertyChanged(nameof(SelectedCaxImportOption));
            }
        }

        private object argGroup;
        #endregion

        #region C'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        /// TODO Edit XML Comment Template for #ctor
        public MainWindowViewModel()
        {
            ReadConfiguration();
            InitializeCommands();
            InitializeCollectionsAndLists();

            StatusListView.Clear();
            CreateMainFolders();
            LoadMainTreeView();
        }

        #endregion

        #region TreeViewEvents
        /// <summary>Loads the project TreeView.</summary>
        /// TODO Edit XML Comment Template for LoadProjectTreeView
        private void LoadProjectTreeView()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(() =>
            {
                if (_tiaPortalProject == null)
                {
                    // check if a project was opened in the UI
                    _tiaPortalProjects = _tiaPortal.Projects;
                    _tiaPortalProject = _tiaPortalProjects.FirstOrDefault();
                    ProjectOpened = _tiaPortalProject != null;
                }

                //Root of TreeView
                var projectTreeView = new TreeView();

                //Project TreeViewItem
                var projectTreeViewItem = new TreeViewItem();

                if (ProjectOpened)
                {
                    // ReSharper disable once PossibleNullReferenceException
                    var splitPath = _tiaPortalProject.Path.ToString().Split('\\');
                    _projectName = splitPath[splitPath.Length - 1];

                    projectTreeViewItem.Header = _projectName;
                    projectTreeViewItem.Tag = _tiaPortalProject;

                    //expand Item
                    projectTreeViewItem.ExpandSubtree();


                    // OLD (Get all components inside TiaPortal)
                    ////////////////////////////////////////////////////////////////
                    //foreach (var device in _tiaPortalProject.Devices)
                    //{
                    //    var item = CreateDeviceTreeViewItem(device);
                    //
                    //    projectTreeViewItem.Items.Add(item);
                    //}
                    //foreach (var folder in _tiaPortalProject.DeviceGroups)
                    //{
                    //    FolderCrawler(projectTreeViewItem, folder); //
                    //}
                    ////////////////////////////////////////////////////////////////


                    // Get only PLCSoftware components
                    foreach (var device in _tiaPortalProject.Devices)
                    {
                        PlcSoftware ps = GetPlcSoftware(device);
                        if (ps != null)
                        {
                            var item = CreateDeviceTreeViewItem(device);

                            projectTreeViewItem.Items.Add(item);
                        }
                    }

                    #region Multilingual graphics
                    projectTreeViewItem.Items.Add(OpennessTreeViews.GetGraphicsTreeView(_tiaPortalProject));
                    #endregion

                    projectTreeView.Items.Add(projectTreeViewItem);
                }
                else
                {
                    projectTreeViewItem.Header = "TIA Portal without project connected";

                    projectTreeView.Items.Add(projectTreeViewItem);
                }

                FindArgGroup(projectTreeViewItem);

                ProjectTree.Refresh(projectTreeView);
            }));
        }

        /// <summary>
        /// Returns PlcSoftware
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        private PlcSoftware GetPlcSoftware(Device device)
        {
            DeviceItemComposition deviceItemComposition = device.DeviceItems;
            foreach (DeviceItem deviceItem in deviceItemComposition)
            {
                SoftwareContainer softwareContainer = deviceItem.GetService<SoftwareContainer>();
                if (softwareContainer != null)
                {
                    Software softwareBase = softwareContainer.Software;
                    PlcSoftware plcSoftware = softwareBase as PlcSoftware;
                    return plcSoftware;
                }
            }
            return null;
        }

        /// <summary>TreeViews the item selected changed callback.</summary>
        /// <param name="e">The <see cref="DependencyPropertyEventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for TreeViewItemSelectedChangedCallback
        public void TreeViewItemSelectedChangedCallback(DependencyPropertyEventArgs e)
        {
            _projectTree.SelectedItem = e.DependencyPropertyChangedEventArgs.NewValue as TreeViewItem;
            var selectedTreeViewObject = _projectTree.SelectedItem;
            RaisePropertyChanged("ProjectTree");

            if (_destinationIndex == 1)
                CopyDestination = selectedTreeViewObject;

            ExportEnabled = false;
            ImportEnabled = false;
            Compile = false;
            PLCTagSelected = false;
            OpenEditor = false;
            DeleteItem = false;
            CreateItem = false;
            AddExternalSource = false;
            GenerateBlockFromSource = false;
            GenerateSourceFromBlock = false;
            ConnectPlc = false;
            ConfigureConnection = false;

            if (selectedTreeViewObject != null)
            {
                #region PlcSoftwares

                if (selectedTreeViewObject.Tag is PlcSoftware)
                {
                    Compile = true;
                    ConfigureConnection = true;

                    var softwareContainer = (selectedTreeViewObject.Tag as PlcSoftware).Parent as SoftwareContainer;
                    if (softwareContainer != null)
                    {
                        var item = softwareContainer.OwnedBy;
                        var online = item.GetService<OnlineProvider>();
                        if (online != null)
                        {
                            if (online.Configuration.IsConfigured)
                                ConnectPlc = true;
                        }
                    }
                }

                #endregion

                #region Blocks

                else if (selectedTreeViewObject.Tag is PlcBlockSystemGroup)
                {
                    ImportEnabled = true;
                    CreateItem = true;
                    Compile = true;
                }
                else if (selectedTreeViewObject.Tag is PlcBlockUserGroup)
                {
                    ImportEnabled = true;
                    DeleteItem = true;
                    CreateItem = true;
                    Compile = true;
                }

                else if (selectedTreeViewObject.Tag is PlcBlock)
                {
                    DeleteItem = true;
                    Compile = true;
                    OpenEditor = true;
                    switch ((selectedTreeViewObject.Tag as PlcBlock).ProgrammingLanguage)
                    {
                        case ProgrammingLanguage.DB:
                        case ProgrammingLanguage.SCL:
                        case ProgrammingLanguage.STL:
                            GenerateSourceFromBlock = true;
                            break;
                    }
                }

                #endregion

                #region PlcType

                else if (selectedTreeViewObject.Tag is PlcTypeSystemGroup)
                {
                    ImportEnabled = true;
                    CreateItem = true;
                    Compile = true;
                }
                else if (selectedTreeViewObject.Tag is PlcTypeUserGroup)
                {
                    ImportEnabled = true;
                    DeleteItem = true;
                    CreateItem = true;
                    Compile = true;
                }

                else if (selectedTreeViewObject.Tag is PlcType)
                {
                    DeleteItem = true;
                    Compile = true;
                    OpenEditor = true;
                    GenerateSourceFromBlock = true;
                }

                #endregion

                #region PlcTagTable

                else if (selectedTreeViewObject.Tag is PlcTagTableSystemGroup)
                {
                    PLCTagSelected = true;
                    ImportEnabled = true;
                    CreateItem = true;
                }
                else if (selectedTreeViewObject.Tag is PlcTagTableUserGroup)
                {
                    ImportEnabled = true;
                    DeleteItem = true;
                    CreateItem = true;
                }

                else if (selectedTreeViewObject.Tag is PlcTagTable)
                {
                    DeleteItem = true;
                    OpenEditor = true;
                }

                #endregion

                #region External Source

                else if (selectedTreeViewObject.Tag is PlcExternalSourceSystemGroup)
                {
                    ImportEnabled = true;
                    AddExternalSource = true;
                }

                else if (selectedTreeViewObject.Tag is PlcExternalSource)
                {
                    DeleteItem = true;
                    GenerateBlockFromSource = true;
                }

                #endregion

                #region HmitTarget

                else if (selectedTreeViewObject.Tag is HmiTarget)
                {
                    Compile = true;
                }

                #endregion

                #region TagTable

                else if (selectedTreeViewObject.Tag is TagSystemFolder)
                {
                    ImportEnabled = true;
                    CreateItem = true;
                }
                else if (selectedTreeViewObject.Tag is TagUserFolder)
                {
                    ImportEnabled = true;
                    DeleteItem = true;
                    CreateItem = true;
                }

                else if (selectedTreeViewObject.Tag is TagTable)
                {
                    DeleteItem = true;
                }

                #endregion

                #region ScreenOverview
                else if (selectedTreeViewObject.Tag is ScreenOverview)
                {
                    ImportEnabled = true;
                }
                #endregion

                #region ScreenGlobalElements
                else if (selectedTreeViewObject.Tag is ScreenGlobalElements)
                {
                    ImportEnabled = true;
                }
                #endregion

                #region Screen

                else if (selectedTreeViewObject.Tag is ScreenSystemFolder)
                {
                    ImportEnabled = true;
                    CreateItem = true;
                }

                else if (selectedTreeViewObject.Tag is ScreenUserFolder)
                {
                    ImportEnabled = true;
                    DeleteItem = true;
                    CreateItem = true;
                }

                else if (selectedTreeViewObject.Tag is Screen)
                {
                    DeleteItem = true;
                }

                #endregion

                #region ScreenTemplate

                else if (selectedTreeViewObject.Tag is ScreenTemplateSystemFolder)
                {
                    ImportEnabled = true;
                    CreateItem = true;
                }

                else if (selectedTreeViewObject.Tag is ScreenTemplateUserFolder)
                {
                    ImportEnabled = true;
                    DeleteItem = true;
                    CreateItem = true;
                }

                else if (selectedTreeViewObject.Tag is ScreenTemplate)
                {
                    DeleteItem = true;
                }

                #endregion

                #region PopUps

                else if (selectedTreeViewObject.Tag is ScreenPopupSystemFolder)
                {
                    ImportEnabled = true;
                    CreateItem = true;
                }

                else if (selectedTreeViewObject.Tag is ScreenPopupUserFolder)
                {
                    ImportEnabled = true;
                    DeleteItem = true;
                    CreateItem = true;
                }

                else if (selectedTreeViewObject.Tag is ScreenPopup)
                {
                    DeleteItem = true;
                }

                #endregion

                #region SlideIns

                else if (selectedTreeViewObject.Tag is ScreenSlideinSystemFolder)
                {
                    ImportEnabled = true;
                }

                #endregion

                #region Cycle

                else if (selectedTreeViewObject.Tag is CycleComposition)
                {
                    ImportEnabled = true;
                }

                else if (selectedTreeViewObject.Tag is Cycle)
                {
                    DeleteItem = true;
                }

                #endregion

                #region Connection

                else if (selectedTreeViewObject.Tag is ConnectionComposition)
                {
                    ImportEnabled = true;
                }

                else if (selectedTreeViewObject.Tag is Connection)
                {
                    DeleteItem = true;
                }

                #endregion

                #region VBScript

                else if (selectedTreeViewObject.Tag is VBScriptSystemFolder || selectedTreeViewObject.Tag is VBScriptUserFolder)
                {
                    ImportEnabled = true;
                }

                else if (selectedTreeViewObject.Tag is VBScript)
                {
                    ExportEnabled = true;
                }

                #endregion

                #region GraphicList
                else if (selectedTreeViewObject.Tag is GraphicListComposition)
                {
                    ImportEnabled = true;
                }
                #endregion

                #region TextList
                else if (selectedTreeViewObject.Tag is TextListComposition)
                {
                    ImportEnabled = true;
                }
                #endregion

                #region MultilngualGrafik
                else if (selectedTreeViewObject.Tag is MultiLingualGraphicComposition)
                {
                    ImportEnabled = true;
                }
                #endregion

                if (!(selectedTreeViewObject.Tag is Device || selectedTreeViewObject.Tag is DeviceItem)
                    || selectedTreeViewObject.Tag is PlcSoftware || selectedTreeViewObject.Tag is HmiTarget)
                    ExportEnabled = true;
                if (selectedTreeViewObject.Tag is IEngineeringObject)
                {
                    try
                    {
                        ReadProperties(selectedTreeViewObject.Tag as IEngineeringObject);
                    }
                    catch (Exception ex)
                    {
                        WriteStatusEntry(ex.Message);
                    }
                }
                else
                    PropertiesShown = false;
            }
            
            if (_tiaPortal.GetCurrentProcess().Mode == TiaPortalMode.WithoutUserInterface)
                OpenEditor = false;

        }
        
        /// <summary>Loads the library TreeView.</summary>
        /// TODO Edit XML Comment Template for LoadLibraryTreeView
        private void LoadLibraryTreeView()
        {
           Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(() =>
           {
               var libraryTreeView = new TreeView();
               if (_tiaPortalProject != null)
               {
                   libraryTreeView.Items.Add(OpennessTreeViews.GetLibraryTreeView(_tiaPortalProject.ProjectLibrary));
               }
               if (libraryTreeView.Items.Count == 0)
               {
                   var libraryItem = new TreeViewItem();
                   libraryItem.Header = "No libraries loaded";
                   libraryItem.Tag = new object();

                   libraryTreeView.Items.Add(libraryItem);
               }
               LibraryTree.Refresh(libraryTreeView);
           }));
        }

        /// <summary>TreeViews the library changed callback.</summary>
        /// <param name="e">The <see cref="DependencyPropertyEventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for TreeViewLibraryChangedCallback
        public void TreeViewLibraryChangedCallback(DependencyPropertyEventArgs e)
        {
            _libraryTree.SelectedItem = e.DependencyPropertyChangedEventArgs.NewValue as TreeViewItem;
            var selectedTreeViewObject = _libraryTree.SelectedItem;
            RaisePropertyChanged("LibraryTree");

            if (_sourceIndex == 2)
                CopySource = selectedTreeViewObject;
            if (_destinationIndex == 2)
                CopyDestination = selectedTreeViewObject;

            if (selectedTreeViewObject != null)
            {

            }
        }

        /// <summary>TreeViews the main changed callback.</summary>
        /// <param name="e">The <see cref="DependencyPropertyEventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for TreeViewMainChangedCallback
        public void TreeViewMainChangedCallback(DependencyPropertyEventArgs e)
        {
            _mainTree.SelectedItem = e.DependencyPropertyChangedEventArgs.NewValue as TreeViewItem;
            var selectedTreeViewObject = _mainTree.SelectedItem;
            RaisePropertyChanged("MainTree");

            if (selectedTreeViewObject != null)
            {
                ReadSymbolicProperties(selectedTreeViewObject.Tag.ToString());
            }
        }

        /// <summary>Loads the global library TreeView.</summary>
        /// TODO Edit XML Comment Template for LoadGlobalLibraryTreeView
        private void LoadGlobalLibraryTreeView()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(() =>
            {
                var libraryTreeView = new TreeView();

                if (GlobalLibraryOpened)
                {
                    libraryTreeView.Items.Add(OpennessTreeViews.GetLibraryTreeView(_tiaGlobalLibrary));
                }
                if (libraryTreeView.Items.Count == 0)
                {
                    var libraryItem = new TreeViewItem();
                    libraryItem.Header = "No libraries loaded";
                    libraryItem.Tag = new object();

                    libraryTreeView.Items.Add(libraryItem);
                }
                GlobalLibraryTree.Refresh(libraryTreeView);
            }));
        }

        /// <summary>TreeViews the global library changed callback.</summary>
        /// <param name="e">The <see cref="DependencyPropertyEventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for TreeViewGlobalLibraryChangedCallback
        public void TreeViewGlobalLibraryChangedCallback(DependencyPropertyEventArgs e)
        {
            _libraryTree.SelectedItem = e.DependencyPropertyChangedEventArgs.NewValue as TreeViewItem;
            var selectedTreeViewObject = _libraryTree.SelectedItem;
            RaisePropertyChanged("GlobalLibraryTree");

            if (_sourceIndex == 1)
                CopySource = selectedTreeViewObject;

            if (selectedTreeViewObject != null)
            {

            }
        }

        /// <summary>TreeViews the symbolics changed callback.</summary>
        /// <param name="e">The <see cref="DependencyPropertyEventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for TreeViewSymbolicsChangedCallback
        public void TreeViewSymbolicsChangedCallback(DependencyPropertyEventArgs e)
        {
            _symbolicsTree.SelectedItem = e.DependencyPropertyChangedEventArgs.NewValue as TreeViewItem;
            var selectedTreeViewObject = _symbolicsTree.SelectedItem;
            RaisePropertyChanged("SymbolicsTree");

            if (_sourceIndex == 1)
                CopySource = selectedTreeViewObject;

            if (selectedTreeViewObject != null)
            {
                ReadSymbolicProperties(selectedTreeViewObject.Tag.ToString());
            }
        }

        #region Main Tree View Events
        /// <summary>
        /// Load info to main treeview
        /// </summary>
        private void LoadMainTreeView()
        {
            TreeView mainTreeView = new TreeView();
            
            DirectoryInfo root = new DirectoryInfo(MainFolderPath);
            string name = Path.GetFileNameWithoutExtension(MainFolderPath);

            TreeViewItem tvi = GetTreeViewItem(name, MainFolderPath, "folder");
            //TreeViewItem tvi = new TreeViewItem
            //{
            //    Header = name,
            //    Tag = MainFolderPath,
            //};
            WalkDirectoryTree(root, tvi, onlyXml: false);

            try
            {
                mainTreeView.Items.Add(tvi);
            }
            catch (Exception ex)
            {
                WriteStatusEntry("Error: " + ex.Message);
                return;
            }

            // Check if MainTree has a TreeView on it
            if(MainTree.View.Count > 0)
            {
                CollectExpandedNodes(MainTree.View[0]);
            }
            
            MainTree.Refresh(mainTreeView);

            ExpandNodes(MainTree.View[0]);
        }

        /// <summary>
        /// Iterate through TreeView items
        /// </summary>
        /// <param name="tvi"></param>
        private void CollectExpandedNodes(TreeView tvi)
        {
            foreach (TreeViewItem item in tvi.Items)
            {
                if (item.IsExpanded)
                    expandedTvitems.Add(item.Tag.ToString());

                CollectExpandedNodes(item);
            }
        }

        /// <summary>
        /// Collect all treeview nodes that are expanded and store on a list called "expandedTvitems"
        /// </summary>
        /// <param name="tvi"></param>
        private void CollectExpandedNodes(TreeViewItem tvi)
        {
            foreach (TreeViewItem item in tvi.Items)
            {
                if (item.Items.Count > 0)
                {
                    if (item.IsExpanded)
                        expandedTvitems.Add(item.Tag.ToString());

                    CollectExpandedNodes(item);
                }
            }
        }

        /// <summary>
        /// Iterate through TreeView items
        /// </summary>
        /// <param name="tvi"></param>
        private void ExpandNodes(TreeView tree)
        {
            foreach (TreeViewItem item in tree.Items)
            {
                if (expandedTvitems.Contains(item.Tag.ToString()))
                    item.IsExpanded = true;
                else
                    item.IsExpanded = false;

                ExpandNodes(item);
            }
        }

        /// <summary>
        /// Expand all treeview nodes that are on list "expandedTvitems"
        /// </summary>
        /// <param name="tvi"></param>
        private void ExpandNodes(TreeViewItem tvi)
        {
            foreach (TreeViewItem item in tvi.Items)
            {
                if (item.Items.Count > 0)
                {
                    if (expandedTvitems.Contains(item.Tag.ToString()))
                        item.IsExpanded = true;
                    else
                        item.IsExpanded = false;

                    ExpandNodes(item);
                }
            }
        }

        private void AddFolder(DirectoryInfo folder)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(() =>
            {
                FoldersList.Add(new FolderInfo(folder.Name, folder.FullName));
            }));

            foreach (var f in folder.GetDirectories())
            {
                AddFolder(f);
            }
        }

        private void WalkDirectoryTree(DirectoryInfo root, TreeViewItem tvi, bool onlyXml = true)
        {
            FileInfo[] files = null;
            DirectoryInfo[] subDirs = null;
            string[] extensions = new[] { ".xml", ".xls", ".xlsx", ".xlsm", ".txt"};
            string imageType = "";

            try
            {
                if (onlyXml)
                    files = root.GetFiles("*.xml");
                else
                    files = root.GetFiles().Where(f => extensions.Contains(f.Extension.ToLower())).ToArray();
            }
            catch (UnauthorizedAccessException e)
            {
                Debug.Write(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                Debug.Write(e.Message);
            }

            if (files != null)
            {
                foreach (FileInfo fi in files)
                {
                    if (Path.GetExtension(fi.FullName).Contains("xml"))
                        imageType = "xml";
                    else
                        if (Path.GetExtension(fi.FullName).Contains("txt"))
                        imageType = "log";
                    else
                        imageType = "excel";

                    string name = Path.GetFileNameWithoutExtension(fi.FullName);
                    if (name[0].Equals('~')) continue;
                    TreeViewItem tviFile = GetTreeViewItem(name, fi.FullName, imageType);
                    //TreeViewItem tviFile = new TreeViewItem
                    //{
                    //    Header = name,
                    //    Tag = fi.FullName,
                    //};

                    if (name.ToLower().Contains("plc db"))
                    {
                        bool found = false;
                        Excel.Application xlApp = new Excel.Application();
                        Excel.Workbook xlWorkbook = OpennessHelper.GetExcelFile(fi.FullName, xlApp);

                        if (xlWorkbook.Worksheets["plc tags"] != null)
                            found = true;

                        xlWorkbook.Close(0);
                        xlApp.Quit();

                        if (found)
                        {
                            TreeViewItem tviTags = GetTreeViewItem("PLC Tags (sheet)", fi.FullName, imageType);
                            //TreeViewItem tviTags = new TreeViewItem
                            //{
                            //    Header = "PLC Tags (sheet)",
                            //    Tag = fi.FullName,
                            //};
                            tviFile.Items.Add(tviTags);
                        }
                    }

                    tvi.Items.Add(tviFile);
                }

                // Now find all the subdirectories under this directory.
                subDirs = root.GetDirectories();

                foreach (DirectoryInfo dirInfo in subDirs)
                {
                    // Resursive call for each subdirectory
                    string name = Path.GetFileNameWithoutExtension(dirInfo.FullName);
                    TreeViewItem subTvi = GetTreeViewItem(name, dirInfo.FullName, "folder");
                    //TreeViewItem subTvi = new TreeViewItem
                    //{
                    //    Header = name,
                    //    Tag = dirInfo.FullName,
                    //};
                    tvi.Items.Add(subTvi);
                    WalkDirectoryTree(dirInfo, subTvi, onlyXml);
                }
            }
        }

        /// <summary>
        /// Creates a TreeViewItem with an image
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="text"></param>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        private TreeViewItem GetTreeViewItem(string text, string tag, string imageType)
        {
            string imagePath = "";
            TreeViewItem item = new TreeViewItem
            {
                Tag = tag
            };
            
            switch(imageType)
            {
                case "excel":
                    imagePath = "/TiaPortalOpennessDemo;component/Images/Excel-Icon.png";
                    break;
                case "folder":
                    imagePath = "/TiaPortalOpennessDemo;component/Images/folder-icon.png";
                    break;
                case "xml":
                    imagePath = "/TiaPortalOpennessDemo;component/Images/xmltext-icon.png";
                    break;
                case "log":
                    imagePath = "/TiaPortalOpennessDemo;component/Images/Notepad-icon.png";
                    break;
            }   

            // create stack panel
            StackPanel stack = new StackPanel();
            stack.Orientation = System.Windows.Controls.Orientation.Horizontal;

            // Create Image
            Image image = new Image
            {
                Source = new BitmapImage(new Uri(imagePath, UriKind.Relative)),
                Width = 16,
                Height = 16
            };
            
            // Textblock
            TextBlock txtBlock = new TextBlock 
            { 
                FontFamily = new FontFamily("Segoe UI Emoji"),
                Padding = new Thickness(4),
                Text = text
            };

            // Add into stack
            stack.Children.Add(image);
            stack.Children.Add(txtBlock);

            // Assign stack to header
            item.Header = stack;
            return item;
        }
        #endregion

        #endregion

        #region Command Initialization

        #region Import/Export
        /// <summary>Gets or sets the export structure command.</summary>
        /// <value>The export structure command.</value>
        /// TODO Edit XML Comment Template for ExportStructureCommand
        public CommandBase ExportStructureCommand { get; set; }
        /// <summary>Gets or sets the import element command.</summary>
        /// <value>The import element command.</value>
        /// TODO Edit XML Comment Template for ImportElementCommand
        public CommandBase ImportElementCommand { get; set; }
        /// <summary>Gets or sets the import Symbolic command.</summary>
        /// <value>The import Symbolic command.</value>
        /// TODO Edit XML Comment Template for ImportSymbolicCommand
        public CommandBase ImportSymbolicCommand { get; set; }
        /// <summary>Gets or sets the rename plc command.</summary>
        /// <value>The rename plc command.</value>
        public CommandBase RenamePlcCommand { get; set; }

        public CommandBase EnableCaxImportCommand { get; set; }
        public CommandBase DisableCaxImportCommand { get; set; }
        public CommandBase CaxImportCommand { get; set; }
        public CommandBase CaxExportCommand { get; set; }
        #endregion

        public CommandBase GenerateRobotListCommand { get; set; }
        public CommandBase GenerateHWCommand { get; set; }

        #region File
        /// <summary>Gets or sets the open tia portal command.</summary>
        /// <value>The open tia portal command.</value>
        /// TODO Edit XML Comment Template for OpenTiaPortalCommand
        public CommandBase OpenTiaPortalCommand { get; set; }
        /// <summary>Gets or sets the open project command.</summary>
        /// <value>The open project command.</value>
        /// TODO Edit XML Comment Template for OpenProjectCommand
        public CommandBase OpenProjectCommand { get; set; }
        /// <summary>Gets or sets the show connect command.</summary>
        /// <value>The show connect command.</value>
        /// TODO Edit XML Comment Template for ShowConnectCommand
        public CommandBase ShowConnectCommand { get; set; }
        /// <summary>Gets or sets the dispose portal command.</summary>
        /// <value>The dispose portal command.</value>
        /// TODO Edit XML Comment Template for DisposePortalCommand
        public CommandBase DisposePortalCommand { get; set; }
        /// <summary>Gets or sets the close project command.</summary>
        /// <value>The close project command.</value>
        /// TODO Edit XML Comment Template for CloseProjectCommand
        public CommandBase CloseProjectCommand { get; set; }
        /// <summary>Gets or sets the save project command.</summary>
        /// <value>The save project command.</value>
        /// TODO Edit XML Comment Template for SaveProjectCommand
        public CommandBase SaveProjectCommand { get; set; }
        /// <summary>Gets or sets the open global library command.</summary>
        /// <value>The open global library command.</value>
        /// TODO Edit XML Comment Template for OpenGlobalLibraryCommand
        public CommandBase OpenGlobalLibraryCommand { get; set; }
        /// <summary>Gets or sets the close global library command.</summary>
        /// <value>The close global library command.</value>
        /// TODO Edit XML Comment Template for CloseGlobalLibraryCommand
        public CommandBase CloseGlobalLibraryCommand { get; set; }
        /// <summary>Gets or sets the refresh project command.</summary>
        /// <value>The refresh project command.</value>
        /// TODO Edit XML Comment Template for RefreshProjectCommand
        public CommandBase RefreshProjectCommand { get; set; }
        /// <summary>Gets or sets the settings command.</summary>
        /// <value>The settings command.</value>
        /// TODO Edit XML Comment Template for SettingsCommand
        public CommandBase SettingsCommand { get; set; }
        #endregion
        #region Edit
        /// <summary>Gets or sets the create command.</summary>
        /// <value>The create command.</value>
        /// TODO Edit XML Comment Template for CreateCommand
        public CommandBase CreateCommand { get; set; }
        /// <summary>Gets or sets the delete command.</summary>
        /// <value>The delete command.</value>
        /// TODO Edit XML Comment Template for DeleteCommand
        public CommandBase DeleteCommand { get; set; }
        /// <summary>Gets or sets the update global library command.</summary>
        /// <value>The update global library command.</value>
        /// TODO Edit XML Comment Template for UpdateGlobalLibraryCommand
        public CommandBase UpdateGlobalLibraryCommand { get; set; }
        #endregion
        #region View
        /// <summary>Gets or sets the subnet view command.</summary>
        /// <value>The subnet view command.</value>
        /// TODO Edit XML Comment Template for SubnetViewCommand
        public CommandBase SubnetViewCommand { get; set; }
        /// <summary>Gets or sets the device view command.</summary>
        /// <value>The device view command.</value>
        /// TODO Edit XML Comment Template for DeviceViewCommand
        public CommandBase DeviceViewCommand { get; set; }
        #endregion
        #region Open Editor
        /// <summary>Gets or sets the open editor command.</summary>
        /// <value>The open editor command.</value>
        /// TODO Edit XML Comment Template for OpenEditorCommand
        public CommandBase OpenEditorCommand { get; set; }
        /// <summary>Gets or sets the open topology view command.</summary>
        /// <value>The open topology view command.</value>
        /// TODO Edit XML Comment Template for OpenTopologyViewCommand
        public CommandBase OpenTopologyViewCommand { get; set; }
        /// <summary>Gets or sets the open network view command.</summary>
        /// <value>The open network view command.</value>
        /// TODO Edit XML Comment Template for OpenNetworkViewCommand
        public CommandBase OpenNetworkViewCommand { get; set; }
        #endregion
        #region Compile
        /// <summary>Gets or sets the compile hw build command.</summary>
        /// <value>The compile hw build command.</value>
        /// TODO Edit XML Comment Template for CompileHWBuildCommand
        public CommandBase CompileCommand { get; set; }
        #endregion
        #region External Source
        /// <summary>Gets or sets the add external source command.</summary>
        /// <value>The add external source command.</value>
        /// TODO Edit XML Comment Template for AddExternalSourceCommand
        public CommandBase AddExternalSourceCommand { get; set; }
        /// <summary>Gets or sets the generate block from source command.</summary>
        /// <value>The generate block from source command.</value>
        /// TODO Edit XML Comment Template for GenerateBlockFromSourceCommand
        public CommandBase GenerateBlockFromSourceCommand { get; set; }
        /// <summary>Gets or sets the generate source from block command.</summary>
        /// <value>The generate source from block command.</value>
        /// TODO Edit XML Comment Template for GenerateSourceFromBlockCommand
        public CommandBase GenerateSourceFromBlockCommand { get; set; }
        #endregion
        #region PLC
        /// <summary>Gets or sets the connect PLC command.</summary>
        /// <value>The connect PLC command.</value>
        /// TODO Edit XML Comment Template for ConnectPlcCommand
        public CommandBase ConnectPlcCommand { get; set; }
        /// <summary>Gets or sets the configure connection command.</summary>
        /// <value>The configure connection command.</value>
        /// TODO Edit XML Comment Template for ConfigureConnectionCommand
        public CommandBase ConfigureConnectionCommand { get; set; }
        #endregion
        #region Transaction
        /// <summary>Gets or sets the transaction start command.</summary>
        /// <value>The transaction start command.</value>
        /// TODO Edit XML Comment Template for TransactionStartCommand
        public CommandBase TransactionStartCommand { get; set; }
        /// <summary>Gets or sets the transaction exit command.</summary>
        /// <value>The transaction exit command.</value>
        /// TODO Edit XML Comment Template for TransactionExitCommand
        public CommandBase TransactionExitCommand { get; set; }
        /// <summary>Gets or sets the transaction rollback command.</summary>
        /// <value>The transaction rollback command.</value>
        /// TODO Edit XML Comment Template for TransactionRollbackCommand
        public CommandBase TransactionRollbackCommand { get; set; }
        #endregion
        #region Properties
        /// <summary>Gets or sets the with user interface command.</summary>
        /// <value>The with user interface command.</value>
        /// TODO Edit XML Comment Template for WithUserInterfaceCommand
        public CommandBase WithUserInterfaceCommand { get; set; }
        /// <summary>Gets or sets the without user interface command.</summary>
        /// <value>The without user interface command.</value>
        /// TODO Edit XML Comment Template for WithoutUserInterfaceCommand
        public CommandBase WithoutUserInterfaceCommand { get; set; }
        /// <summary>Gets or sets the none export options command.</summary>
        /// <value>The none export options command.</value>
        /// TODO Edit XML Comment Template for NoneExportOptionsCommand
        public CommandBase NoneExportOptionsCommand { get; set; }
        /// <summary>Gets or sets the with defaults export options command.</summary>
        /// <value>The with defaults export options command.</value>
        /// TODO Edit XML Comment Template for WithDefaultsExportOptionsCommand
        public CommandBase WithDefaultsExportOptionsCommand { get; set; }
        /// <summary>Gets or sets the with read only export options command.</summary>
        /// <value>The with read only export options command.</value>
        /// TODO Edit XML Comment Template for WithReadOnlyExportOptionsCommand
        public CommandBase WithReadOnlyExportOptionsCommand { get; set; }
        /// <summary>Gets or sets the standard export folder command.</summary>
        /// <value>The standard export folder command.</value>
        /// TODO Edit XML Comment Template for StandardExportFolderCommand
        public CommandBase StandardExportFolderCommand { get; set; }
        /// <summary>Gets or sets the standard main folder command.</summary>
        /// <value>The standard main folder command.</value>
        /// TODO Edit XML Comment Template for StandardMainFolderCommand
        public CommandBase StandardMainFolderCommand { get; set; }
        /// <summary>Gets or sets the standard export symbolic folder command.</summary>
        /// <value>The standard symbolic export folder command.</value>
        /// TODO Edit XML Comment Template for StandardExportSymbolicFolderCommand
        public CommandBase StandardExportSymbolicFolderCommand { get; set; }
        /// <summary>Gets or sets the standard import robot schnittstelle path command.</summary>
        /// <value>The standard export robot Schnittstelle path command.</value>
        public CommandBase StandardImportSchnittstelleFileCommand { get; set; }
        /// <summary>Gets or sets the standard export Symbolic path command.</summary>
        /// <value>The standard export Symbolic path command.</value>
        public CommandBase StandardImportSymbolicFileCommand { get; set; }
        /// <summary>Gets or sets the standard plc db file path command.</summary>
        /// <value>The standard plc db file path command.</value>
        public CommandBase StandardPlcDBFileCommand { get; set; }
        /// <summary>Gets or sets the standard sequence file path command.</summary>
        /// <value>The standard sequence file path command.</value>
        public CommandBase StandardSequenceFileCommand { get; set; }
        /// <summary>Gets or sets the standard import network list file command.</summary>
        /// <value>The standard import network list file command.</value>
        public CommandBase StandartImportNetworkListFileCommand { get; set; }
        /// <summary>Gets or sets the standard import eplan file command.</summary>
        /// <value>The standard import eplan file command.</value>
        public CommandBase StandartImportEPlanFileCommand { get; set; }
        /// <summary>Gets or sets the save settings command.</summary>
        /// <value>The save settings command.</value>
        /// TODO Edit XML Comment Template for SaveSettingsCommand
        public CommandBase SaveSettingsCommand { get; set; }

        public CommandBase SelectAssemblyCommand { get; set; }
        #endregion
        #region Library Tab
        /// <summary>Gets or sets the refresh library command.</summary>
        /// <value>The refresh library command.</value>
        /// TODO Edit XML Comment Template for RefreshLibraryCommand
        public CommandBase RefreshLibraryCommand { get; set; }
        /// <summary>Gets or sets the refresh symbolics command.</summary>
        /// <value>The refresh symbolics command.</value>
        /// TODO Edit XML Comment Template for RefreshSymbolicsCommand
        public CommandBase RefreshSymbolicsCommand { get; set; }
        /// <summary>Gets or sets the refresh main tree command.</summary>
        /// <value>The refresh main tree command.</value>
        /// TODO Edit XML Comment Template for RefreshMainTreeCommand
        public CommandBase RefreshMainTreeCommand { get; set; }
        /// <summary>Gets or sets the choose main path command.</summary>
        /// <value>The choose main path command.</value>
        /// TODO Edit XML Comment Template for ChooseFolderCommand
        public CommandBase ChooseFolderCommand { get; set; }
        /// <summary>Gets or sets the import main tree file command.</summary>
        /// <value>The import main tree file command.</value>
        /// TODO Edit XML Comment Template for ImportMainTreeFileCommand
        public CommandBase ImportMainTreeFileCommand { get; set; }
        /// <summary>Gets or sets the edit main tree file command.</summary>
        /// <value>The edit main tree file command.</value>
        /// TODO Edit XML Comment Template for EditMainTreeFileCommand
        public CommandBase EditMainTreeFileCommand { get; set; }
        /// <summary>Gets or sets the invoke library to project command.</summary>
        /// <value>The invoke library to project command.</value>
        /// TODO Edit XML Comment Template for InvokeLibToProjectCommand
        public CommandBase InvokeLibToProjectCommand { get; set; }
        /// <summary>Gets or sets the invoke global to project command.</summary>
        /// <value>The invoke global to project command.</value>
        /// TODO Edit XML Comment Template for InvokeGlobalToProjectCommand
        public CommandBase InvokeGlobalToProjectCommand { get; set; }
        /// <summary>Gets or sets the invoke global to library command.</summary>
        /// <value>The invoke global to library command.</value>
        /// TODO Edit XML Comment Template for InvokeGlobalToLibCommand
        public CommandBase InvokeGlobalToLibCommand { get; set; }
        /// <summary>Gets or sets the cancel copy command.</summary>
        /// <value>The cancel copy command.</value>
        /// TODO Edit XML Comment Template for CancelCopyCommand
        public CommandBase CancelCopyCommand { get; set; }
        /// <summary>Gets or sets the copy library command.</summary>
        /// <value>The copy library command.</value>
        /// TODO Edit XML Comment Template for CopyLibCommand
        public CommandBase CopyLibCommand { get; set; }
        #endregion

        /// <summary>Initializes the commands.</summary>
        /// TODO Edit XML Comment Template for InitializeCommands
        private void InitializeCommands()
        {
            #region Menu
            #region File
            OpenTiaPortalCommand = new CommandBase(OpenTIAPortalCommand_Executed);
            ShowConnectCommand = new CommandBase(ShowConnectCommand_Executed);
            DisposePortalCommand = new CommandBase(DisposePortalCommand_Executed);
            OpenProjectCommand = new CommandBase(OpenProjectCommand_Executed);
            SaveProjectCommand = new CommandBase(SaveProjectCommand_Executed);
            CloseProjectCommand = new CommandBase(CloseProjectCommand_Executed);
            OpenGlobalLibraryCommand = new CommandBase(OpenGlobalLibraryCommand_Executed);
            CloseGlobalLibraryCommand = new CommandBase(CloseGlobalLibraryCommand_Executed);
            RefreshProjectCommand = new CommandBase(RefreshProjectCommand_Executed);
            SettingsCommand = new CommandBase(SettingsCommand_Executed);
            #endregion

            #region Edit
            CreateCommand = new CommandBase(CreateCommand_Executed);
            DeleteCommand = new CommandBase(DeleteCommand_Executed);
            UpdateGlobalLibraryCommand = new CommandBase(UpdateGlobalLibraryCommand_Executed);
            RenamePlcCommand = new CommandBase(RenamePlcCommand_Executed);
            #endregion

            #region Project
            #region Editor
            OpenEditorCommand = new CommandBase(OpenEditorCommand_Executed);
            OpenTopologyViewCommand = new CommandBase(OpenTopologyViewCommand_Executed);
            OpenNetworkViewCommand = new CommandBase(OpenNetworkViewCommand_Executed);
            #endregion

            #region Compile
            CompileCommand = new CommandBase(CompileCommand_Executed);
            #endregion

            #region Import/Export
            CaxImportCommand = new CommandBase(CaxImportCommand_Executed);
            CaxExportCommand = new CommandBase(CaxExportCommand_Executed);
            EnableCaxImportCommand = new CommandBase(EnableCaxImportCommand_Executed);
            DisableCaxImportCommand = new CommandBase(DisableCaxImportCommand_Executed);
            ExportStructureCommand = new CommandBase(ExportStructureCommand_Executed);
            ImportElementCommand = new CommandBase(ImportElementCommand_Executed);
            #endregion

            GenerateHWCommand = new CommandBase(GenerateHWCommand_Executed);
            GenerateRobotListCommand = new CommandBase(GenerateRobotListCommand_Executed);

            #region View
            SubnetViewCommand = new CommandBase(SubnetViewCommand_Executed);
            DeviceViewCommand = new CommandBase(DeviceViewCommand_Executed);
            #endregion

            #endregion

            #region PLC
            #region Source files
            AddExternalSourceCommand = new CommandBase(AddExternalSourceCommand_Executed);
            GenerateBlockFromSourceCommand = new CommandBase(GenerateBlockFromSourceCommand_Executed);
            GenerateSourceFromBlockCommand = new CommandBase(GenerateSourceFromBlockCommand_Executed);
            #endregion
            ConnectPlcCommand = new CommandBase(ConnectPlcCommand_Executed);
            ConfigureConnectionCommand = new CommandBase(ConfigureConnectionCommand_Executed);
            #region Compare
            #endregion

            #endregion

            #region Transaction
            TransactionStartCommand = new CommandBase(TransactionStartCommand_Executed);
            TransactionExitCommand = new CommandBase(TransactionExitCommand_Executed);
            TransactionRollbackCommand = new CommandBase(TransactionRollbackCommand_Executed);
            #endregion

            #endregion

            #region Settings
            StandardExportFolderCommand = new CommandBase(StandardExportFolderCommand_Executed);
            StandardMainFolderCommand = new CommandBase(StandardMainFolderCommand_Executed);
            StandardImportSchnittstelleFileCommand = new CommandBase(StandardImportSchnittstelleFileCommand_Executed);
            StandardPlcDBFileCommand = new CommandBase(StandardPlcDBFileCommand_Executed);
            StandardSequenceFileCommand = new CommandBase(StandardSequenceFileCommand_Executed);
            StandartImportNetworkListFileCommand = new CommandBase(StandartImportNetworkListFileCommand_Executed);
            StandartImportEPlanFileCommand = new CommandBase(StandartImportEPlanFileCommand_Executed);
            SaveSettingsCommand = new CommandBase(SaveSettingsCommand_Executed);
            #endregion

            #region Library Tab
            RefreshLibraryCommand = new CommandBase(RefreshLibraryCommand_Executed);
            RefreshMainTreeCommand = new CommandBase(RefreshMainTreeCommand_Executed);
            ChooseFolderCommand = new CommandBase(ChooseFolderCommand_Executed);
            ImportMainTreeFileCommand = new CommandBase(ImportMainTreeFileCommand_Executed);
            EditMainTreeFileCommand = new CommandBase(EditMainTreeFileCommand_Executed);
            InvokeLibToProjectCommand = new CommandBase(InvokeLibToProjectCommand_Executed);
            InvokeGlobalToProjectCommand = new CommandBase(InvokeGlobalToProjectCommand_Executed);
            InvokeGlobalToLibCommand = new CommandBase(InvokeGlobalToLibCommand_Executed);
            CancelCopyCommand = new CommandBase(CancelCopyCommand_Executed);
            CopyLibCommand = new CommandBase(CopyLibCommand_Executed);
            #endregion
        }
        #endregion

        #region Commands
        #region Menu
        #region File
        /// <summary>
        /// Handles the Executed event of the OpenTIAPortalCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for OpenTIAPortalCommand_Executed
        private void OpenTIAPortalCommand_Executed(object sender, EventArgs e)
        {
            if (_tiaPortal == null)
            {
                try
                {
                    if (UserInterfaceEnabled)
                        _tiaPortal = new TiaPortal(TiaPortalMode.WithUserInterface);
                    else
                        _tiaPortal = new TiaPortal();

                    LoadProjectTreeView();
                    LoadLibraryTreeView();

                    PortalOpened = true;

                    WriteStatusEntry("TIA Portal opened");
                }
                catch
                {
                    // ignored
                }
            }
        }

        /// <summary>Handles the Executed event of the OpenProjectCommand control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for OpenProjectCommand_Executed
        private void OpenProjectCommand_Executed(object sender, EventArgs e)
        {
            try
            {
                var fileSearch = new OpenFileDialog();
                fileSearch.InitialDirectory = @"C:\";
                fileSearch.Filter = "Siemens TIA Portal project| *.ap*";
                fileSearch.FilterIndex = 2;
                fileSearch.RestoreDirectory = true;
                fileSearch.ShowDialog();
                var fileName = fileSearch.FileName;

                if (string.IsNullOrEmpty(fileName) == false)
                {

                    if (_tiaPortal == null)
                    {
                        if (UserInterfaceEnabled)
                            _tiaPortal = new TiaPortal(TiaPortalMode.WithUserInterface);
                        else
                            _tiaPortal = new TiaPortal();
                    }

                    _tiaPortalProjects = _tiaPortal.Projects;
                    _tiaPortalProject = _tiaPortalProjects.Open(new FileInfo(fileName));


                    LoadProjectTreeView();
                    //LoadLibraryTreeView();
                    CheckConsistency();

                    ProjectOpened = true;
                    PortalOpened = true;

                    WriteStatusEntry(string.Format(CultureInfo.InvariantCulture, "Project: {0} opened", fileName));

                }
            }
            catch (EngineeringException ee)
            {
                WriteStatusEntry(ee.Message);
            }
            catch (IOException ie)
            {
                WriteStatusEntry(ie.Message);
            }
        }

        /// <summary>Handles the Executed event of the ShowConnectCommand control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for ShowConnectCommand_Executed
        private void ShowConnectCommand_Executed(object sender, EventArgs e)
        {
            ConnectToTia(false);
        }

        /// <summary>
        /// Handles the Executed event of the DisposePortalCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for DisposePortalCommand_Executed
        private void DisposePortalCommand_Executed(object sender, EventArgs e)
        {
            CloseTiaPortalConnection();
        }

        /// <summary>
        /// Handles the Executed event of the CloseProjectCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for CloseProjectCommand_Executed
        private void CloseProjectCommand_Executed(object sender, EventArgs e)
        {
            if (_tiaPortal != null)
            {
                //close Project
                if (_tiaPortalProject != null)
                {
                    if (_tiaPortalProject.IsModified)
                    {
                        var result =
                        DialogService.ShowWarningMessageBox("The open project contains unsaved changes.", "Should the System save the changes?");

                        switch (result)
                        {
                            case MessageBoxResult.No:
                                break;

                            case MessageBoxResult.Yes:
                                _tiaPortalProject.Save();
                                WriteStatusEntry("Project saved");
                                break;
                        }
                    }
                    _tiaPortalProject.Close();


                    ProjectTree.View.Clear();
                    _plcsToCompile.Clear();
                    StatusListView.Clear();
                    WriteStatusEntry("Project closed.");
                }
                else
                {
                    WriteStatusEntry("Project cannot be closed");
                }
            }
            else
            {
                PortalOpened = false;
                GlobalLibraryOpened = false;
                TransactionRunning = false;
            }
            ProjectOpened = false;
            CreateItem = false;
            DeleteItem = false;
            OpenEditor = false;
            Compile = false;
            Compile = false;
            ImportEnabled = false;
            ExportEnabled = false;
            AddExternalSource = false;
            GenerateBlockFromSource = false;
            ConnectPlc = false;
            PropertiesShown = false;
        }

        /// <summary>Handles the Executed event of the SaveProjectCommand control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for SaveProjectCommand_Executed
        private void SaveProjectCommand_Executed(object sender, EventArgs e)
        {
            //close Project
            if (_tiaPortalProject != null)
            {
                _tiaPortalProject.Save();

                WriteStatusEntry("Project saved.");
            }

            else
            {
                WriteStatusEntry("Project cannot be saved");
            }
        }

        /// <summary>
        /// Handles the Executed event of the OpenGlobalLibraryCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for OpenGlobalLibraryCommand_Executed
        private void OpenGlobalLibraryCommand_Executed(object sender, EventArgs e)
        {
            if (PortalOpened == false)
            {
                System.Windows.MessageBox.Show("Please open or attach to a TIA Portal instance first.");
                return;
            }

            var fileSearch = new OpenFileDialog();
            fileSearch.InitialDirectory = @"C:\";
            fileSearch.Filter = "Siemens TIA Portal Library | *.al1*";
            fileSearch.FilterIndex = 2;
            fileSearch.RestoreDirectory = true;
            fileSearch.ShowDialog();
            if (String.IsNullOrEmpty(fileSearch.FileName))
                return;
            var fileName = fileSearch.FileName;

            try
            {
                _tiaGlobalLibrary = _tiaPortal.GlobalLibraries.Open(new FileInfo(fileName), OpenMode.ReadWrite);
                if (_tiaGlobalLibrary == null)
                    return;

                LoadGlobalLibraryTreeView();
                GlobalLibraryOpened = true;
            }
            catch (EngineeringException ee)
            {
                WriteStatusEntry(ee.Message);
            }
        }

        /// <summary>
        /// Handles the Executed event of the CloseGlobalLibraryCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for CloseGlobalLibraryCommand_Executed
        private void CloseGlobalLibraryCommand_Executed(object sender, EventArgs e)
        {
            GlobalLibraryOpened = false;
            try
            {
                _tiaGlobalLibrary.Close();
                _tiaGlobalLibrary = null;

                LoadLibraryTreeView();
                LoadGlobalLibraryTreeView();
                LibraryTree.SelectedItem = new TreeViewItem();

            }
            catch (EngineeringException ee)
            {
                WriteStatusEntry(ee.Message);
            }
        }

        /// <summary>
        /// Handles the Executed event of the RefreshProjectCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for RefreshProjectCommand_Executed
        private void RefreshProjectCommand_Executed(object sender, EventArgs e)
        {
            LoadProjectTreeView();
            //LoadLibraryTreeView();
            //LoadGlobalLibraryTreeView();
            WriteStatusEntry("Navigation updated.");
        }

        /// <summary>Handles the Executed event of the SettingsCommand control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for SettingsCommand_Executed
        private void SettingsCommand_Executed(object sender, EventArgs e)
        {
            InitSettings();
        }
        #endregion

        #region Main folder files list 
        /// <summary>
        /// Handles the Executed event of the RefreshMainTreeCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for RefreshMainTreeCommand_Executed
        private void RefreshMainTreeCommand_Executed(object sender, EventArgs e)
        {
            LoadMainTreeView();
            StatusListView.Clear();
            WriteStatusEntry("MainTree Refreshed");
            WriteStatusEntry("Cache Cleared");
            OpennessHelper.ClearCacheData();
        }

        /// <summary>
        /// Handles the Executed event of the EditMainTreeFileCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for EditMainTreeFileCommand_Executed
        private void EditMainTreeFileCommand_Executed(object sender, EventArgs e)
        {
            TreeViewItem selected = MainTree.SelectedItem;
            if (selected == null) return;
            if (string.IsNullOrEmpty(Convert.ToString(selected.Header)) || Directory.Exists(selected.Tag.ToString())) return;

            StackPanel sp = (StackPanel)MainTree.SelectedItem.Header;
            TextBlock tb = sp.Children.OfType<TextBlock>().FirstOrDefault();
            string[] extensions = new[] { ".xml", ".xls", ".xlsx", ".xlsm" };

            string SelectedItem = tb.Text;
            var current = ProjectTree.SelectedItem.Tag;
            string clickedFilePath = Convert.ToString(selected.Tag);
            string extension = Path.GetExtension(selected.Tag.ToString().ToLower());
            object[,] matrix;
            var matrixs = new List<object[,]>();
            var sheetNames = new List<string>();

            Excel.Workbook xlWorkbook = null;
            Excel.Application xlApp = null;
            Excel.Worksheet EngAssist = null;
            Excel.Worksheet PlcTags = null;

            bool engAssistExist = true;
            bool plcTagsExist = true;
            bool changed = false;
            XmlSchemaSet schema = new XmlSchemaSet();
            schema.Add(null, "XMLStructure.xsd");

            Task.Factory.StartNew(() =>
            {
                IsLoading = true;
                bool isValid = false;

                if (extensions.Contains(extension))   // is excel file
                {
                    if (OpennessHelper.GetCacheData("Excel;path:" + clickedFilePath) == null)
                    {
                        try
                        {
                            xlApp = new Excel.Application();
                            xlWorkbook = OpennessHelper.GetExcelFile(clickedFilePath, xlApp);
                        }
                        catch (Exception ex)
                        {
                            WriteStatusEntry("Error: " + ex.Message);
                            return;
                        }

                        try
                        {
                            EngAssist = xlWorkbook.Sheets["EngAssist"];
                        }
                        catch (Exception)
                        {
                            engAssistExist = false;
                        }

                        try
                        {
                            PlcTags = xlWorkbook.Sheets["PLC Tags"];
                        }
                        catch (Exception)
                        {
                            plcTagsExist = false;
                        }

                        OpennessHelper.SetCacheData("Excel;path:" + clickedFilePath, true);
                        OpennessHelper.SetCacheData("Excel;plctags:" + clickedFilePath, plcTagsExist);
                        OpennessHelper.SetCacheData("Excel;engassist:" + clickedFilePath, engAssistExist);
                    }

                    if (OpennessHelper.GetCacheData("Excel;engassist:" + clickedFilePath) != null)
                        engAssistExist = (bool)OpennessHelper.GetCacheData("Excel;engassist:" + clickedFilePath);

                    if (OpennessHelper.GetCacheData("Excel;plctags:" + clickedFilePath) != null)
                        plcTagsExist = (bool)OpennessHelper.GetCacheData("Excel;plctags:" + clickedFilePath);


                    if (engAssistExist)
                    {
                        if (OpennessHelper.GetCacheData("EngAssist;path:" + clickedFilePath) != null)
                        {
                            matrix = (object[,])OpennessHelper.GetCacheData("EngAssist;path:" + clickedFilePath);
                        }
                        else
                        {
                            matrix = OpennessHelper.ExcelToMatrix(EngAssist);
                            OpennessHelper.SetCacheData("EngAssist;path:" + clickedFilePath, matrix);
                        }

                        //Check if is a PLC DB
                        if (OpennessHelper.IsPlcDb(matrix))
                        {
                            if (OpennessHelper.GetCacheData("PlcDb;path:" + clickedFilePath) != null)
                            {
                                matrixs = (List<object[,]>)OpennessHelper.GetCacheData("PlcDb;path:" + clickedFilePath);
                                sheetNames = (List<string>)OpennessHelper.GetCacheData("PlcDbSheets;path:" + clickedFilePath);
                            }
                            else
                            {
                                foreach (Excel.Worksheet xlWorksheet in xlWorkbook.Worksheets)
                                {
                                    switch (xlWorksheet.Name.ToLower())
                                    {
                                        case "rel notes":
                                        case "export db":
                                            break;
                                        default:
                                            matrix = OpennessHelper.ExcelToMatrix(xlWorksheet);
                                            matrixs.Add(matrix);
                                            sheetNames.Add(xlWorksheet.Name);

                                            if (xlWorksheet.Name.ToLower().Equals("plc tags"))
                                                OpennessHelper.SetCacheData("PlcTags;path:" + clickedFilePath, matrix);

                                            break;
                                    }
                                }
                                OpennessHelper.SetCacheData("PlcDb;path:" + clickedFilePath, matrixs);
                                OpennessHelper.SetCacheData("PlcDbSheets;path:" + clickedFilePath, sheetNames);
                            }

                            if (SelectedItem != "PLC Tags (sheet)")
                            {
                                IsLoading = false;
                                changed = InitDbMaker(matrixs, sheetNames, clickedFilePath);
                                isValid = true;

                                if (MatrixList.Any() && SheetNamesList.Any())
                                {
                                    OpennessHelper.DisposeCacheData("PlcDb;path:" + clickedFilePath);
                                    OpennessHelper.DisposeCacheData("PlcDbSheets;path:" + clickedFilePath);

                                    OpennessHelper.SetCacheData("PlcDb;path:" + clickedFilePath, MatrixList);
                                    OpennessHelper.SetCacheData("PlcDbSheets;path:" + clickedFilePath, SheetNamesList);
                                }
                            }
                            else
                            {
                                // Is sheet "PLC Tags" from "PLC DB" Excel
                                if (plcTagsExist)
                                {
                                    if (OpennessHelper.GetCacheData("PlcTags;path:" + clickedFilePath) != null)
                                    {
                                        matrix = (object[,])OpennessHelper.GetCacheData("PlcTags;path:" + clickedFilePath);
                                    }
                                    else
                                    {
                                        matrix = OpennessHelper.ExcelToMatrix(PlcTags);
                                        OpennessHelper.SetCacheData("PlcTags;path:" + clickedFilePath, matrix);
                                    }

                                    IsLoading = false;
                                    changed = InitPlcTaps(matrix, clickedFilePath);

                                    if (MatrixList.Any() && SheetNamesList.Any() && PlcTagsMatrix != null)
                                    {
                                        OpennessHelper.DisposeCacheData("PlcDb;path:" + clickedFilePath);
                                        OpennessHelper.DisposeCacheData("PlcDbSheets;path:" + clickedFilePath);
                                        OpennessHelper.DisposeCacheData("PlcTags;path:" + clickedFilePath);

                                        OpennessHelper.SetCacheData("PlcDb;path:" + clickedFilePath, MatrixList);
                                        OpennessHelper.SetCacheData("PlcDbSheets;path:" + clickedFilePath, SheetNamesList);
                                        OpennessHelper.SetCacheData("PlcTags;path:" + clickedFilePath, PlcTagsMatrix);
                                    }
                                    isValid = true;
                                }
                            }
                        }

                        //Check if is a Symbolic
                        if (OpennessHelper.IsSymbolic(matrix))
                        {
                            if (OpennessHelper.GetCacheData("Symbolic;path:" + clickedFilePath) == null)
                            {
                                LoadSymbolic(clickedFilePath);
                                OpennessHelper.SetCacheData("Symbolic;path:" + clickedFilePath, true);
                            }
                            else
                            {
                                RobsInfo = (List<RobotInfo>)OpennessHelper.GetCacheData("SymbolicRobsInfo;path:" + clickedFilePath);
                            }

                            IsLoading = false;
                            changed = InitOptionsRobotView(RobsInfo);
                            isValid = true;
                        }
                    }
                    else
                    {
                        // Is Sequence
                        if (OpennessHelper.GetCacheData("Sequence;path:" + clickedFilePath) != null)
                        {
                            matrixs = (List<object[,]>)OpennessHelper.GetCacheData("Sequence;path:" + clickedFilePath);
                            sheetNames = (List<string>)OpennessHelper.GetCacheData("SequenceSheets;path:" + clickedFilePath);
                        }
                        else
                        {
                            foreach (Excel.Worksheet xlWorksheet in xlWorkbook.Worksheets)
                            {
                                string sheetName = xlWorksheet.Name;
                                if (!sheetName.Contains("AS_") || sheetName.Equals("AS_000000")) continue;

                                matrix = OpennessHelper.ExcelToMatrix(xlWorksheet);

                                if (OpennessHelper.IsSequence(matrix))
                                {
                                    matrixs.Add(matrix);
                                    sheetNames.Add(xlWorksheet.Name);
                                }
                            }

                            OpennessHelper.SetCacheData("Sequence;path:" + clickedFilePath, matrixs);
                            OpennessHelper.SetCacheData("SequenceSheets;path:" + clickedFilePath, sheetNames);
                        }

                        if (matrixs.Any() && sheetNames.Any())
                        {
                            IsLoading = false;
                            changed = InitTreeViewManager(matrixs, sheetNames, clickedFilePath);

                            if (MatrixList.Any() && SheetNamesList.Any())
                            {
                                OpennessHelper.DisposeCacheData("Sequence;path:" + clickedFilePath);
                                OpennessHelper.DisposeCacheData("SequenceSheets;path:" + clickedFilePath);

                                OpennessHelper.SetCacheData("Sequence;path:" + clickedFilePath, MatrixList);
                                OpennessHelper.SetCacheData("SequenceSheets;path:" + clickedFilePath, SheetNamesList);
                            }

                            isValid = true;
                        }
                    }
                }
                if (extension.Equals(".xml"))
                {
                    XmlReader rd = XmlReader.Create(clickedFilePath);
                    XDocument xDoc = XDocument.Load(rd);

                    bool isSymbolic = true;

                    try
                    {
                        xDoc.Validate(schema, ValidationEventHandler);
                    }
                    catch (Exception)
                    {
                        isSymbolic = false;
                    }

                    rd.Close();

                    if (isSymbolic)
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(clickedFilePath);
                        var robBase = GenerateRobBase(doc);
                        var robTecnologies = GenerateRobTecnologies(doc);
                        var robSafeRangeMonitoring = GenerateRobSafeRangeMonitoring(doc);
                        var robSafeOperations = GenerateRobSafeOperation(doc);
                        var robInfo = GenerateRobInfo(doc);

                        IsLoading = false;
                        changed = InitRobotView(robInfo, robBase, robTecnologies, robSafeRangeMonitoring, robSafeOperations);

                        isValid = true;
                    }
                    else
                    if (XmlParser.IsPlcTags(clickedFilePath))
                    {
                        IsLoading = false;
                        changed = InitPlcTaps(null, clickedFilePath);
                        isValid = true;
                    }
                }
                if (extension.Equals(".txt"))
                {
                    isValid = true;
                    Process.Start(clickedFilePath);
                }

                if (!isValid)
                    WriteStatusEntry("Invalid file");
            }).ContinueWith(t2 => {
                IsLoading = false;

                if (xlWorkbook != null)
                    xlWorkbook.Close(0);

                if (xlApp != null)
                    xlApp.Quit();

                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    if (changed)
                        LoadMainTreeView();
                }));
            });
        }

        /// <summary>
        /// Handles the Executed event of the ChooseFolderCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ChooseFolderCommand_Executed(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();

            if (dlg.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(dlg.SelectedPath))
            {
                DirectoryInfo root = new DirectoryInfo(dlg.SelectedPath);

                Task.Factory.StartNew(() =>
                {
                    IsLoading = true;
                    var files = Directory.EnumerateFiles(root.FullName, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".xls") || s.EndsWith(".xlsx") || s.EndsWith(".xlsm")).ToArray();
                    var XMLFiles = Directory.EnumerateFiles(dlg.SelectedPath, "*.xml", SearchOption.AllDirectories);
                    foreach (var file in files)
                    {
                        if (file.Contains('~')) continue;
                        ImportFileToMainTreeView(file);
                    }

                    foreach (var file in XMLFiles)
                    {
                        string fileName = Path.GetFileName(file);

                        if (file.Contains('~')) continue;
                        ImportFileToMainTreeView(file);
                    }

                }).ContinueWith(t2 => {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        LoadMainTreeView();
                        OpennessHelper.ClearCacheData();
                    }));
                    IsLoading = false;
                    WriteStatusEntry("Files loaded successfully");
                });
            }
        }

        /// <summary>
        /// Handles the Executed event of the ImportMainTreeFileCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for ImportMainTreeFileCommand_Executed
        private void ImportMainTreeFileCommand_Executed(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Excel and XML Files|*.xls;*.xlsx;*.xlsm;*.xml";
            dlg.Multiselect = true;
            dlg.ShowDialog();

            if (File.Exists(dlg.FileName))
            {
                Task.Factory.StartNew(() =>
                {
                    IsLoading = true;
                    foreach (var file in dlg.FileNames)
                    {
                        if (file.Contains('~')) continue;

                        string name = Path.GetFileName(file);
                        bool upload = ImportFileToMainTreeView(file);
                        if (!upload)
                        {
                            if (Path.GetExtension(file).ToLower().Contains("xml"))
                                WriteStatusEntry("Error uploading file \"" + name + "\" : Only Symbolic & PLC Tags XML files are allowed");
                            else
                                WriteStatusEntry("Error uploading file \"" + name + "\" : Invalid Excel file format");

                        }
                        else
                            WriteStatusEntry("File \"" + name + "\" uploaded successfully");
                    }
                }).ContinueWith(t2 => {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        LoadMainTreeView();
                        OpennessHelper.ClearCacheData();
                    }));
                    IsLoading = false;
                });
            }
        }
        #endregion

        #region Edit
        /// <summary>Handles the Executed event of the CreateCommand control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for CreateCommand_Executed
        private void CreateCommand_Executed(object sender, EventArgs e)
        {
            if (ProjectTree.SelectedItem != null)
            {
                try
                {
                    _subWindow = new CreateFolderDialog();
                    _subWindow.ShowDialog();
                    var selectedProjectObject = ProjectTree.SelectedItem.Tag;
                    if (selectedProjectObject is VBScriptFolder)
                    {
                        (selectedProjectObject as VBScriptFolder).Folders.Create((_subWindow as CreateFolderDialog).FolderName);
                        WriteStatusEntry(string.Format(CultureInfo.InvariantCulture, "Folder {0} created", (_subWindow as CreateFolderDialog).FolderName));
                        LoadProjectTreeView();
                    }
                    else if (selectedProjectObject is PlcTagTableGroup)
                    {
                        (selectedProjectObject as PlcTagTableGroup).Groups.Create((_subWindow as CreateFolderDialog).FolderName);
                        WriteStatusEntry(string.Format(CultureInfo.InvariantCulture, "Folder {0} created", (_subWindow as CreateFolderDialog).FolderName));
                        LoadProjectTreeView();
                    }
                    else if (selectedProjectObject is TagFolder)
                    {
                        (selectedProjectObject as TagFolder).Folders.Create((_subWindow as CreateFolderDialog).FolderName);
                        WriteStatusEntry(string.Format(CultureInfo.InvariantCulture, "Folder {0} created", (_subWindow as CreateFolderDialog).FolderName));
                        LoadProjectTreeView();
                    }
                    else if (selectedProjectObject is PlcBlockGroup)
                    {
                        (selectedProjectObject as PlcBlockGroup).Groups.Create((_subWindow as CreateFolderDialog).FolderName);
                        WriteStatusEntry(string.Format(CultureInfo.InvariantCulture, "Folder {0} created", (_subWindow as CreateFolderDialog).FolderName));
                        LoadProjectTreeView();
                    }
                    else if (selectedProjectObject is ScreenFolder)
                    {
                        (selectedProjectObject as ScreenFolder).Folders.Create((_subWindow as CreateFolderDialog).FolderName);
                        WriteStatusEntry(string.Format(CultureInfo.InvariantCulture, "Folder {0} created", (_subWindow as CreateFolderDialog).FolderName));
                        LoadProjectTreeView();
                    }
                    else if (selectedProjectObject is PlcTypeGroup)
                    {
                        (selectedProjectObject as PlcTypeGroup).Groups.Create((_subWindow as CreateFolderDialog).FolderName);
                        WriteStatusEntry(string.Format(CultureInfo.InvariantCulture, "Folder {0} created", (_subWindow as CreateFolderDialog).FolderName));
                        LoadProjectTreeView();
                    }
                    else if (selectedProjectObject is ScreenTemplateFolder)
                    {
                        (selectedProjectObject as ScreenTemplateFolder).Folders.Create((_subWindow as CreateFolderDialog).FolderName);
                        WriteStatusEntry(string.Format(CultureInfo.InvariantCulture, "Folder {0} created", (_subWindow as CreateFolderDialog).FolderName));
                        LoadProjectTreeView();
                    }
                    else if (selectedProjectObject is ScreenPopupFolder)
                    {
                        (selectedProjectObject as ScreenPopupFolder).Folders.Create((_subWindow as CreateFolderDialog).FolderName);
                        WriteStatusEntry(string.Format(CultureInfo.InvariantCulture, "Folder {0} created", (_subWindow as CreateFolderDialog).FolderName));
                        LoadProjectTreeView();
                    }
                    else if (selectedProjectObject is PlcExternalSourceGroup)
                    {
                        (selectedProjectObject as PlcExternalSourceGroup).Groups.Create((_subWindow as CreateFolderDialog).FolderName);
                        WriteStatusEntry(string.Format(CultureInfo.InvariantCulture, "Folder {0} created", (_subWindow as CreateFolderDialog).FolderName));
                        LoadProjectTreeView();
                    }
                    else
                    {
                        WriteStatusEntry("Targeted element " + OpennessHelper.GetObjectName(selectedProjectObject as IEngineeringInstance) + " is not a legible target");
                    }
                }
                catch (IOException ex)
                {
                    WriteStatusEntry(ex.Message + ": Folder name does exist.");
                }
            }
            else
            {
                CreateItem = false;
                WriteStatusEntry("No target selected");
            }
        }

        /// <summary>Handles the Executed event of the DeleteCommand control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for DeleteCommand_Executed
        private void DeleteCommand_Executed(object sender, EventArgs e)
        {
            if (ProjectTree.SelectedItem != null)
            {
                var selectedProjectObject = ProjectTree.SelectedItem.Tag;

                try
                {
                    // most SystemFolder don't have a Name property
                    var engineeringObject = selectedProjectObject as IEngineeringObject;
                    engineeringObject?.Invoke("Delete", new Dictionary<Type, object>());
                    var deletedItemName = OpennessHelper.GetObjectName(selectedProjectObject as IEngineeringInstance);
                    WriteStatusEntry("Deleted item " + deletedItemName);
                }
                catch (EngineeringException)
                {
                    WriteStatusEntry("The selected Item cannot be deleted");
                }
            }
            else
            {
                DeleteItem = false;
                WriteStatusEntry("No target selected");
            }
        }

        /// <summary>
        /// Handles the Executed event of the UpdateGlobalLibraryCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for UpdateGlobalLibraryCommand_Executed
        private void UpdateGlobalLibraryCommand_Executed(object sender, EventArgs e)
        {
            if (ProjectOpened && GlobalLibraryOpened)
            {
                var updatecheck = _tiaGlobalLibrary.UpdateCheck(_tiaPortalProject, UpdateCheckMode.ReportOutOfDateAndUpToDate);
                WriteUpdateMessage(updatecheck);
            }
            if (ProjectOpened)
            {
                var updateCheck = _tiaPortalProject.ProjectLibrary.UpdateCheck(_tiaPortalProject, UpdateCheckMode.ReportOutOfDateAndUpToDate);
                WriteUpdateMessage(updateCheck);
            }
        }
        #endregion

        #region Project
        #region Editor
        /// <summary>Handles the Executed event of the OpenEditorCommand control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for OpenEditorCommand_Executed
        private void OpenEditorCommand_Executed(object sender, EventArgs e)
        {
            (ProjectTree.SelectedItem.Tag as IShowable)?.ShowInEditor();
        }

        /// <summary>
        /// Handles the Executed event of the OpenTopologyViewCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for OpenTopologyViewCommand_Executed
        private void OpenTopologyViewCommand_Executed(object sender, EventArgs e)
        {
            if (_tiaPortal.GetCurrentProcess().Mode == TiaPortalMode.WithUserInterface)
            {
                if (_tiaPortalProject != null)
                {
                    _tiaPortalProject.ShowHwEditor(View.Topology);
                }
            }
        }

        /// <summary>
        /// Handles the Executed event of the OpenNetworkViewCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for OpenNetworkViewCommand_Executed
        private void OpenNetworkViewCommand_Executed(object sender, EventArgs e)
        {
            if (_tiaPortal.GetCurrentProcess().Mode == TiaPortalMode.WithUserInterface)
            {
                if (_tiaPortalProject != null)
                {
                    _tiaPortalProject.ShowHwEditor(View.Network);
                }
            }
        }
        #endregion

        #region Compile
        /// <summary>
        /// Handles the Executed event of the CompileHWBuildCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for CompileHWBuildCommand_Executed
        private void CompileCommand_Executed(object sender, EventArgs e)
        {
            try
            {
                var result = OpennessHelper.CompileObject(ProjectTree.SelectedItem.Tag as IEngineeringServiceProvider);
                WriteCompileMessage(result);
            }
            catch (ArgumentNullException ae)
            {
                WriteStatusEntry(ae.Message);
            }
        }
        #endregion

        #region Import/Export
        private void EnableCaxImportCommand_Executed(object sender, EventArgs e)
        {
            CaxImportVisible = true;
        }

        private void DisableCaxImportCommand_Executed(object sender, EventArgs e)
        {
            CaxImportVisible = false;
        }

        private void CaxImportCommand_Executed(object sender, EventArgs e)
        {
            if (ProjectOpened)
            {
                IsLoading = true;
                WriteStatusEntry("Start Cax Import");

                if (string.IsNullOrEmpty(CaxImportFilePath) == false)
                {
                    try
                    {
                        OpennessHelper.CaxImport(_tiaPortalProject, CaxImportFilePath, SelectedCaxImportOption);
                    }
                    catch (EngineeringException invoEx)
                    {
                        DialogService.ShowErrorMessageBox(invoEx.Message);
                        WriteStatusEntry(invoEx.Message);
                    }
                    catch (ArgumentException ae)
                    {
                        DialogService.ShowErrorMessageBox(ae.Message);
                        WriteStatusEntry(ae.Message);
                    }
                    catch (IOException ie)
                    {
                        DialogService.ShowErrorMessageBox(ie.Message);
                        WriteStatusEntry(ie.Message);
                    }
                }

                LoadProjectTreeView();
                CaxImportVisible = false;
                IsLoading = false;
                WriteStatusEntry("Import finished");
            }
        }

        private void CaxExportCommand_Executed(object sender, EventArgs e)
        {
            if (ProjectOpened)
            {
                _exportPath = Path.Combine(_defaultExportFolderPath, _projectName);

                IsLoading = true;
                WriteStatusEntry("Start CAx Export");

                try
                {
                    if (OpennessHelper.CaxExport(_tiaPortalProject, _exportPath))
                        WriteStatusEntry("Export successful");
                    else
                        WriteStatusEntry("Export finished with errors");
                }
                catch (EngineeringTargetInvocationException invoEx)
                {
                    DialogService.ShowErrorMessageBox(invoEx.Message);
                    WriteStatusEntry("Export finished with errors");
                }
                catch (EngineeringException ee)
                {
                    DialogService.ShowErrorMessageBox(ee.Message);
                    WriteStatusEntry("Export finished with errors");
                }
                catch (ArgumentException ae)
                {
                    DialogService.ShowErrorMessageBox(ae.Message);
                    WriteStatusEntry("Export finished with errors");
                }
                catch (IOException ie)
                {
                    DialogService.ShowErrorMessageBox(ie.Message);
                    WriteStatusEntry("Export finished with errors");
                }

                IsLoading = false;
            }
        }

        /// <summary>
        /// Handles the Executed event of the ExportStructureCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for ExportStructureCommand_Executed
        private void ExportStructureCommand_Executed(object sender, EventArgs e)
        {
            if (ProjectTree.SelectedItem != null)
            {
                _exportPath = Path.Combine(_defaultExportFolderPath, _projectName);
                var current = ProjectTree.SelectedItem.Tag;

                //var exportTask = Task<int>.Factory.StartNew(() =>
                //{
                IsLoading = true;
                WriteStatusEntry("Start Export");

                try
                {
                    if (_exportOptionsDefaults && _exportOptionsReadOnly)
                        OpennessHelper.ExportStructure(current as IEngineeringCompositionOrObject, ExportOptions.WithDefaults | ExportOptions.WithReadOnly, _exportPath);
                    else if (_exportOptionsDefaults)
                        OpennessHelper.ExportStructure(current as IEngineeringCompositionOrObject, ExportOptions.WithDefaults, _exportPath);
                    else if (_exportOptionsReadOnly)
                        OpennessHelper.ExportStructure(current as IEngineeringCompositionOrObject, ExportOptions.WithReadOnly, _exportPath);
                    else
                        OpennessHelper.ExportStructure(current as IEngineeringCompositionOrObject, ExportOptions.None, _exportPath);

                    WriteStatusEntry("Export successful");

                }
                catch (EngineeringTargetInvocationException invoEx)
                {
                    DialogService.ShowErrorMessageBox(invoEx.Message);
                    WriteStatusEntry("Export failed");
                }
                catch (EngineeringException ee)
                {
                    DialogService.ShowErrorMessageBox(ee.Message);
                    WriteStatusEntry("Export failed");
                }
                catch (ArgumentException ae)
                {
                    DialogService.ShowErrorMessageBox(ae.Message);
                    WriteStatusEntry("Export failed");
                }
                catch (IOException ie)
                {
                    DialogService.ShowErrorMessageBox(ie.Message);
                    WriteStatusEntry("Export failed");
                }
                IsLoading = false;
            }
            else ExportEnabled = false;
        }

        /// <summary>
        /// Handles the Executed event of the ImportElementCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for ImportElementCommand_Executed
        private void ImportElementCommand_Executed(object sender, EventArgs e)
        {
            if (ProjectTree.SelectedItem != null)
            {
                var current = ProjectTree.SelectedItem.Tag;

                var fileSearch = new OpenFileDialog();
                fileSearch.InitialDirectory = _defaultExportFolderPath;
                fileSearch.Filter = "xml File|*.xml|SCL|*.scl|DB|*.db|AWL|*.awl|UDT|*.udt|All files(*.*)|*.*";
                fileSearch.FilterIndex = 1;
                fileSearch.RestoreDirectory = true;
                fileSearch.Multiselect = true;
                var result = fileSearch.ShowDialog();

                if (result == true)
                {
                    if (fileSearch.FileNames != null)
                    {
                        IsLoading = true;
                        WriteStatusEntry("Start Import");

                        using (var access = _tiaPortal.ExclusiveAccess("Import element"))
                        {
                            foreach (var file in fileSearch.FileNames)
                            {
                                if (string.IsNullOrEmpty(file) == false)
                                {
                                    using (var action = access.Transaction(_tiaPortalProject, "Import element"))
                                    {
                                        try
                                        {
                                            OpennessHelper.ImportItem(current as IEngineeringObject, file, ImportOptions.Override);
                                            action.CommitOnDispose();
                                        }
                                        catch (EngineeringException invoEx)
                                        {
                                            IsLoading = false;
                                            DialogService.ShowErrorMessageBox(invoEx.Message);
                                            WriteStatusEntry(invoEx.Message);
                                        }
                                        catch (ArgumentException ae)
                                        {
                                            IsLoading = false;
                                            DialogService.ShowErrorMessageBox(ae.Message);
                                            WriteStatusEntry(ae.Message);
                                        }
                                        catch (IOException ie)
                                        {
                                            IsLoading = false;
                                            DialogService.ShowErrorMessageBox(ie.Message);
                                            WriteStatusEntry(ie.Message);
                                        }
                                    }
                                }
                            }
                            //access.Dispose();
                        }

                        //LoadProjectTreeView();
                        IsLoading = false;
                        WriteStatusEntry("Import finished");
                    }
                }
                else
                {
                    WriteStatusEntry("Import cancelled");
                }
            }
            else ImportEnabled = false;
        }

        /// <summary>
        /// Handles the Executed event of the RenamePlcCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void RenamePlcCommand_Executed(object sender, EventArgs e)
        {
            // In case there's a valid Tia Portal connection
            if (ProjectTree.SelectedItem != null && ProjectTree.View.Count > 0)
            {
                tiaPortalConnected = true;
            }
            else
            {
                ConnectToTia(true);
                if (_tiaPortalProject == null) return;

                tiaPortalConnected = false;
            }

            InitPLCRenamer();
        }

        /// <summary>
        /// Handles the Executed event of the GenerateHWCommand control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateHWCommand_Executed(object sender, EventArgs e)
        {
            InitHardwareGenerator();
        }

        /// <summary>
        /// Handles the Executed event of the GenerateRobotListCommand control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateRobotListCommand_Executed(object sender, EventArgs e)
        {
            if (_defaultSchnittstellePath == "")
            {
                System.Windows.MessageBox.Show("Please add the Schnittstelle path in Settings", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                if (!File.Exists(_defaultSchnittstellePath))
                {
                    System.Windows.MessageBox.Show("Schnittstelle file does not exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    InitSettings();
                    return;
                }
                else
                {
                    if (System.Windows.MessageBox.Show("Schnittstelle file selected in settings: \"" + DefaultSchnittstellePath + "\".\n\nDo you want to use this path?", "Import", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        InitSettings();
                        return;
                    }
                }
            }

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = OpennessHelper.GetExcelFile(_defaultSchnittstellePath, xlApp);
            XmlDocument xmlRobot = new XmlDocument();
            string workingDirectory = Environment.CurrentDirectory;
            string workPath = Directory.GetParent(workingDirectory).FullName;
            string savePath = Path.Combine(MainFolderPath, "60_Roboter");
            bool importToTia = false;
            object current = null;
            List<List<string>> Colisions = new List<List<string>>();
            List<List<string>> Outputs = new List<List<string>>();
            List<List<string>> Inputs = new List<List<string>>();
            List<string> blocksCreated = new List<string>();
            List<string> frgs = new List<string>();

            if (_tiaPortal != null)
            {
                var RoboterGroup = (argGroup as PlcBlockUserGroup).Groups.Find("60_Roboter");

                if (RoboterGroup == null)
                    System.Windows.MessageBox.Show("Group \"60_Roboter\" not found.\nGenerated files will not be imported.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                else
                {
                    importToTia = true;
                    current = RoboterGroup;
                }
            }

            Task.Factory.StartNew(() =>
            {
                IsLoading = true;

                foreach (Excel.Worksheet xlWorksheet in xlWorkbook.Worksheets)
                {
                    string robName = xlWorksheet.Name;
                    if (robName.Substring(robName.Length - 3).Contains("R") && robName != "000000R01")
                    {
                        Colisions = OpennessHelper.GetRobotColisions(xlWorkbook.Worksheets["Anti-Kollisionen"], robName);
                        Outputs = OpennessHelper.GetRobotOutputs(xlWorksheet);
                        Inputs = OpennessHelper.GetRobotInputs(xlWorksheet);

                        xmlRobot = OpennessHelper.GenerateRobotFC(robName.Insert(6, "_"), workPath);
                        OpennessHelper.GenerateEingabenLesen(xmlRobot, robName, workPath);
                        var Sequences = OpennessHelper.GetSequences(xlWorksheet);
                        foreach (var seq in Sequences)
                        {
                            OpennessHelper.GenerateFreigabeFolge(xmlRobot, robName, workPath, seq.Key.ToString(), seq.Value);
                        }
                        OpennessHelper.GenerateBildungFolgen(xmlRobot, robName, workPath, xlWorksheet);
                        OpennessHelper.GenerateTypRoboter(xmlRobot, robName, workPath);
                        OpennessHelper.GenerateStartArbeitsfolge(xmlRobot, robName, workPath);
                        OpennessHelper.GenerateStartWartungsfolge(xmlRobot, robName, workPath);
                        OpennessHelper.GenerateFreigabeMaschinensicherheitHifu(xmlRobot, robName, workPath);
                        OpennessHelper.GenerateFreigabeMaschinensicherheit(xmlRobot, robName, workPath);
                        OpennessHelper.GenerateUberbruckungFolgenkonsistenzprufung(xmlRobot, robName, workPath);
                        OpennessHelper.GenerateRobotersystemschnittstelle(xmlRobot, robName, workPath);
                        OpennessHelper.GenerateRoboterHaltKorrigieren(xmlRobot, robName, workPath);
                        OpennessHelper.GenerateAnwahlWartungWechsel(xmlRobot, robName, workPath);
                        OpennessHelper.GenerateRoboterfertigmeldungen(xmlRobot, robName, workPath);
                        var FMs = OpennessHelper.GetFMs(xlWorksheet);
                        foreach (var fm in FMs)
                        {
                            OpennessHelper.GenerateStatusFertigmeldung(xmlRobot, robName, workPath, fm.Key.ToString(), fm.Value);
                        }
                        OpennessHelper.GenerateStatusFertigmeldungGesamt(xmlRobot, robName, workPath);
                        OpennessHelper.GenerateRoboterverriegelungen(xmlRobot, robName, workPath, Colisions);

                        string mask = "";
                        int counter = 1;
                        for (int i = 0; i < Inputs.Count; i++)
                        {
                            if (Inputs[i][3] != null)
                                mask = Inputs[i][3];

                            frgs.Add(Inputs[i][0]);
                            if (((i + 1) % 8 == 0) || i == Inputs.Count)
                            {
                                OpennessHelper.GenerateWerkzeugfreigabenAnlage(xmlRobot, robName, workPath, frgs, mask, counter.ToString());
                                frgs = new List<string>();
                                counter++;
                            }
                        }

                        for (int i = Outputs.Count - 1; i >= 0; i--)
                        {
                            if (!string.IsNullOrEmpty(Outputs[i][2]))
                                OpennessHelper.GenerateStellungsfreigabe(xmlRobot, robName, workPath, Outputs[i][0], Outputs[i][1], Outputs[i][2]);
                        }

                        frgs = new List<string>();
                        mask = "";
                        counter = 1;
                        for (int i = 0; i < Outputs.Count; i++)
                        {
                            if (Outputs[i][3] != null)
                                mask = Outputs[i][3];
                            frgs.Add(Outputs[i][0]);
                            if (((i + 1) % 8 == 0) || i == Outputs.Count)
                            {
                                OpennessHelper.GenerateStellungsfreigaben(xmlRobot, robName, workPath, frgs, mask, counter.ToString());
                                frgs = new List<string>();
                                counter++;
                            }
                        }

                        var Tecnologies = OpennessHelper.GetTecnologies(xlWorksheet);
                        var TecnologiesToDb = new Dictionary<string, string>();

                        // Check tecnologies
                        int countGreifer = 0;
                        int countSchweib = 0;
                        int countKleben = 0;
                        int countKappen = 0;
                        int countStanzen = 0;
                        foreach (var tec in Tecnologies)
                        {
                            string tecName;
                            if (tec.ToLower().Contains("greifer"))
                            {
                                countGreifer++;
                                tecName = "G" + countGreifer;
                                OpennessHelper.GenerateGreifer(xmlRobot, robName, workPath, tecName);
                                TecnologiesToDb.Add(tecName, "Greifer");
                            }
                            else
                            if (tec.ToLower().Contains("schweisssteuerung"))
                            {
                                countSchweib++;
                                tecName = "SK" + countSchweib;
                                OpennessHelper.GenerateSchweibsteuerung(xmlRobot, robName, workPath, tecName);
                                OpennessHelper.GenerateMedien(xmlRobot, robName, workPath);
                                TecnologiesToDb.Add(tecName, "Schweissen");
                            }
                            else
                            if (tec.ToLower().Contains("kleben"))
                            {
                                countKleben++;
                                tecName = "KL" + countKleben;
                                OpennessHelper.GenerateKleben(xmlRobot, robName, workPath, tecName);
                                TecnologiesToDb.Add(tecName, "Kleben");
                            }
                            else
                            if (tec.ToLower().Contains("kappenwechsler"))
                            {
                                countKappen++;
                                tecName = "KW" + countKappen;
                                OpennessHelper.GenerateKappenwechsler(xmlRobot, robName, workPath, tecName);
                                TecnologiesToDb.Add(tecName, "Kappenwechsler");
                            }
                            else
                            if (tec.ToLower().Contains("stanzen"))
                            {
                                countStanzen++;
                                tecName = "SM" + countStanzen;
                                OpennessHelper.GenerateStanzen(xmlRobot, robName, workPath, tecName);
                                TecnologiesToDb.Add(tecName, "Stanzen");
                            }
                            // Tecnologias em falta:
                            //  > MIGMAG
                            //  > Praeger
                        }

                        OpennessHelper.GenerateRoboterfehlernummer(xmlRobot, robName, workPath);
                        OpennessHelper.GenerateTaktzeitStoppHifu(xmlRobot, robName, workPath, Sequences);
                        OpennessHelper.GenerateTaktzeitRoboter(xmlRobot, robName, workPath);
                        OpennessHelper.GenerateAusgabenSchreiben(xmlRobot, robName, workPath);
                        OpennessHelper.GenerateStatusRoboter(xmlRobot, robName, workPath);
                        OpennessHelper.GenerateStatusProzessgerate(xmlRobot, robName, workPath);
                        OpennessHelper.GenerateBausteinende(xmlRobot, workPath);

                        XmlParser.IDRenumbering(xmlRobot.SelectNodes("/Document/SW.Blocks.FC//*"));
                        OpennessHelper.SaveXMLDocument(xmlRobot, robName.Insert(6, "_"), savePath);

                        XmlDocument robDB = OpennessHelper.GenerateDB(robName, workPath, Outputs, Inputs, Sequences, FMs, TecnologiesToDb);
                        OpennessHelper.SaveXMLDocument(robDB, robName, Path.Combine(savePath, "DB-Anwender"));

                        blocksCreated.Add(robName.Insert(6, "_"));
                        blocksCreated.Add(robName);
                    }
                }
                xlWorkbook.Close(0);
                xlApp.Quit();

                if (importToTia)
                {
                    var blocksDirectory = new DirectoryInfo(savePath);
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        using (var access = _tiaPortal.ExclusiveAccess("Importing elements"))
                        {
                            foreach (var file in blocksDirectory.GetFiles())
                            {
                                string fileName = Path.GetFileNameWithoutExtension(file.FullName);
                                if (!blocksCreated.Contains(fileName)) continue;

                                try
                                {
                                    OpennessHelper.ImportItem(current as IEngineeringObject, file.FullName, ImportOptions.Override);
                                }
                                catch (Exception ex)
                                {
                                    System.Windows.MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }

                            foreach (var folder in blocksDirectory.GetDirectories())
                            {
                                foreach (var file in folder.GetFiles())
                                {
                                    string fileName = Path.GetFileNameWithoutExtension(file.FullName);
                                    if (!blocksCreated.Contains(fileName)) continue;
                                    var group = (current as PlcBlockUserGroup).Groups.Find(folder.Name);

                                    if (group != null)
                                    {
                                        try
                                        {
                                            OpennessHelper.ImportItem(group, file.FullName, ImportOptions.Override);
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Windows.MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                        }
                                    }
                                    else
                                    {
                                        System.Windows.MessageBox.Show("Group \"" + folder.Name + "\" not found in Tia Portal", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                }
                            }
                        }
                    }));
                }

            }).ContinueWith(t2 =>
            {
                IsLoading = false;
                System.Windows.Forms.Application.DoEvents();
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    LoadMainTreeView();
                    WriteStatusEntry("RobotList Generated Successfully!");
                }));
            });
        }
        #endregion

        #region View
        /// <summary>Handles the Executed event of the SubnetViewCommand control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="NotImplementedException"></exception>
        /// TODO Edit XML Comment Template for SubnetViewCommand_Executed
        private void SubnetViewCommand_Executed(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>Handles the Executed event of the DeviceViewCommand control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="NotImplementedException"></exception>
        /// TODO Edit XML Comment Template for DeviceViewCommand_Executed
        private void DeviceViewCommand_Executed(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion

        #region PLC
        #region Source files
        /// <summary>
        /// Handles the Executed event of the AddExternalSourceCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for AddExternalSourceCommand_Executed
        private void AddExternalSourceCommand_Executed(object sender, EventArgs e)
        {
            if (ProjectTree.SelectedItem != null)
            {
                var current = ProjectTree.SelectedItem.Tag as PlcExternalSourceSystemGroup;

                var fileSearch = new OpenFileDialog();
                fileSearch.InitialDirectory = _defaultExportFolderPath;
                fileSearch.Filter = "Source files (*.awl;*.scl;*.db;*.udt)|*.awl;*.scl;*.db;*.udt|SCL (*.scl)|*.scl|STL (*.awl)|*.awl|UDT (*.udt)|*.udt|DB (*.db)|*.db|All files (*.*)|*.*";
                fileSearch.FilterIndex = 1;
                fileSearch.RestoreDirectory = true;
                fileSearch.Multiselect = true;
                var result = fileSearch.ShowDialog();

                if (result == true)
                {
                    if (fileSearch.FileNames != null)
                    {

                        IsLoading = true;
                        WriteStatusEntry("Start Import");

                        foreach (var file in fileSearch.FileNames)
                        {
                            if (string.IsNullOrEmpty(file) == false)
                            {
                                using (var access = _tiaPortal.ExclusiveAccess("Import element"))
                                {
                                    using (var action = access.Transaction(_tiaPortalProject, "Import element"))
                                    {
                                        try
                                        {
                                            var temp = current.ExternalSources.Find(Path.GetFileName(file));
                                            if (temp != null)
                                                temp.Delete();
                                            current.ExternalSources.CreateFromFile(Path.GetFileName(file), file);
                                            action.CommitOnDispose();
                                        }
                                        catch (EngineeringTargetInvocationException invoEx)
                                        {
                                            IsLoading = false;
                                            DialogService.ShowErrorMessageBox(invoEx.Message);
                                            WriteStatusEntry(invoEx.Message);
                                        }
                                        catch (ArgumentException ae)
                                        {
                                            IsLoading = false;
                                            DialogService.ShowErrorMessageBox(ae.Message);
                                            WriteStatusEntry(ae.Message);
                                        }
                                        catch (IOException ie)
                                        {
                                            IsLoading = false;
                                            DialogService.ShowErrorMessageBox(ie.Message);
                                            WriteStatusEntry(ie.Message);
                                        }
                                    }
                                    access.Dispose();
                                }
                            }
                        }

                        LoadProjectTreeView();
                        IsLoading = false;
                        WriteStatusEntry("Finish Import");
                    }
                }
                else
                {
                    WriteStatusEntry("Import cancelled");
                }
            }
        }

        /// <summary>
        /// Handles the Executed event of the GenerateBlockFromSourceCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for GenerateBlockFromSourceCommand_Executed
        private void GenerateBlockFromSourceCommand_Executed(object sender, EventArgs e)
        {
            if (_projectTree.SelectedItem != null)
            {
                var source = _projectTree.SelectedItem.Tag as PlcExternalSource;
                if (source != null)
                {
                    try
                    {
                        source.GenerateBlocksFromSource();
                    }
                    catch (EngineeringException ee)
                    {
                        DialogService.ShowErrorMessageBox(ee.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Executed event of the GenerateSourceFromBlockCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for GenerateSourceFromBlockCommand_Executed
        private void GenerateSourceFromBlockCommand_Executed(object sender, EventArgs e)
        {
            //TODO: Changed for UDTs
            if (_projectTree.SelectedItem != null && (_projectTree.SelectedItem.Tag is PlcBlock || ProjectTree.SelectedItem.Tag is PlcType))
            {
                var fileSearch = new SaveFileDialog();
                fileSearch.InitialDirectory = _defaultExportFolderPath;
                fileSearch.Filter = "SCL (*.scl)|*.scl|STL (*.awl)|*.awl|UDT (*.udt)|*.udt|DB (*.db)|*.db";
                fileSearch.RestoreDirectory = true;
                fileSearch.FileOk += delegate
                {
                    var fileName = fileSearch.FileName;

                    try
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                        if (File.Exists(fileName))
                            File.Delete(fileName);

                        var temp = _projectTree.SelectedItem.Tag as IEngineeringInstance;

                        do { temp = temp.Parent; }
                        while (!(temp is PlcSoftware));

                        if (ProjectTree.SelectedItem.Tag is PlcBlock)
                        {
                            //TODO: Export Optionen einpflegen (mit oder ohne Abhängigkeiten)
                            (temp as PlcSoftware).ExternalSourceGroup.GenerateSource(new[] { ProjectTree.SelectedItem.Tag as PlcBlock }, new FileInfo(fileName), GenerateOptions.WithDependencies);
                        }
                        else
                        {
                            (temp as PlcSoftware).ExternalSourceGroup.GenerateSource(new[] { ProjectTree.SelectedItem.Tag as PlcType }, new FileInfo(fileName));
                        }



                    }
                    catch (EngineeringException ee)
                    {
                        DialogService.ShowErrorMessageBox(ee.Message);
                        WriteStatusEntry(ee.Message);
                    }
                };
                fileSearch.ShowDialog();
            }
        }
        #endregion
        
        /// <summary>Handles the Executed event of the ConnectPlcCommand control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for ConnectPlcCommand_Executed
        private void ConnectPlcCommand_Executed(object sender, EventArgs e)
        {
            try
            {
                var device = (ProjectTree.SelectedItem.Tag as PlcSoftware).Parent.Parent as DeviceItem;
                var onlineTarget = device.GetService<OnlineProvider>();
                if (onlineTarget.Configuration.IsConfigured == false)
                    ConfigureConnectionCommand_Executed(this, new EventArgs());

                if (onlineTarget.Configuration.IsConfigured)
                {
                    switch (onlineTarget.State)
                    {
                        case OnlineState.Connecting:
                            WriteStatusEntry("Target is connecting.");
                            break;
                        case OnlineState.Disconnecting:
                            WriteStatusEntry("Target is disconnecting.");
                            break;
                        case OnlineState.Incompatible:
                            WriteStatusEntry("Please check your connection configuration. Target is incompatible.");
                            break;
                        case OnlineState.NotReachable:
                            WriteStatusEntry("Please check your connection configuration. Target is not reachable.");
                            break;
                        case OnlineState.Offline:
                            onlineTarget.GoOnline();
                            WriteStatusEntry("Going online.");
                            break;
                        case OnlineState.Online:
                            onlineTarget.GoOffline();
                            WriteStatusEntry("Going offline.");
                            break;
                        case OnlineState.Protected:
                            WriteStatusEntry("Target is protected.");
                            break;
                    }
                }
                else
                {
                    WriteStatusEntry("Please configure the online Connection first.");
                }
            }
            catch (NotImplementedException)
            {
                WriteStatusEntry("ConnectToPLC() not implemented");
            }
        }

        /// <summary>
        /// Handles the Executed event of the ConfigureConnectionCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for ConfigureConnectionCommand_Executed
        private void ConfigureConnectionCommand_Executed(object sender, EventArgs e)
        {
            try
            {
                if (ProjectTree.SelectedItem.Tag is PlcSoftware)
                {
                    //TIAHandler.ConfigureConnectionToPLC(ProjectTree.SelectedItem.Tag as PlcSoftware);
                    _subWindow = new ConnectionConfigurationView();
                    var device = (ProjectTree.SelectedItem.Tag as PlcSoftware).Parent.Parent as DeviceItem;
                    var tmp = device.GetService<OnlineProvider>();
                    var model = new ConnectionConfigurationViewModel(tmp);
                    model.CloseAction = () => _subWindow.Close();
                    _subWindow.DataContext = model;
                    try
                    {
                        _subWindow.ShowDialog();
                    }
                    catch (InvalidOperationException ex)
                    {
                        WriteStatusEntry(ex.Message);
                    }
                    _subWindow = null;
                    if (model.Result || tmp.Configuration.IsConfigured)
                    {
                        ConnectPlc = true;
                    }
                    else
                        ConnectPlc = false;
                }
            }
            catch (EngineeringException ee)
            {
                WriteStatusEntry(ee.Message);
            }
        }
        #endregion

        #region Transactions
        /// <summary>
        /// Handles the Executed event of the TransactionStartCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for TransactionStartCommand_Executed
        private void TransactionStartCommand_Executed(object sender, EventArgs e)
        {
            if (_tiaPortalProject != null)
            {
                _access = _tiaPortal.ExclusiveAccess();
                _action = _access.Transaction(_tiaPortalProject, "Revert");
                TransactionRunning = true;
                WriteStatusEntry("Starting transaction");
            }
        }

        /// <summary>
        /// Handles the Executed event of the TransactionExitCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for TransactionExitCommand_Executed
        private void TransactionExitCommand_Executed(object sender, EventArgs e)
        {
            if (_tiaPortalProject != null)
            {
                _action.CommitOnDispose();
                _action.Dispose();
                _access.Dispose();
                TransactionRunning = false;
                WriteStatusEntry("Transaction finished");
            }
        }

        /// <summary>
        /// Handles the Executed event of the TransactionRollbackCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for TransactionRollbackCommand_Executed
        private void TransactionRollbackCommand_Executed(object sender, EventArgs e)
        {
            if (_tiaPortalProject != null)
            {
                _action.Dispose();
                _access.Dispose();
                TransactionRunning = false;
                WriteStatusEntry("Transaction aborted");
            }
        }
        #endregion

        #endregion

        #region Settings
        /// <summary>
        /// Handles the Executed event of the StandardExportFolderCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for StandardExportFolderCommand_Executed
        private void StandardExportFolderCommand_Executed(object sender, EventArgs e)
        {
            var pathSave = new FolderBrowserDialog();

            if (string.IsNullOrEmpty(DefaultExportFolderPath))
                pathSave.SelectedPath = @"C:\Temp";
            else
                pathSave.SelectedPath = DefaultExportFolderPath;

            if (pathSave.ShowDialog() == DialogResult.OK)
                DefaultExportFolderPath = pathSave.SelectedPath;
        }

        /// <summary>
        /// Handles the Executed event of the StandardExportFolderCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for StandardExportFolderCommand_Executed
        private void StandardMainFolderCommand_Executed(object sender, EventArgs e)
        {
            var pathSave = new FolderBrowserDialog();

            if (string.IsNullOrEmpty(MainFolderPath))
                pathSave.SelectedPath = @"C:\Temp\TiaPortalOpenness";
            else
                pathSave.SelectedPath = MainFolderPath;

            if (pathSave.ShowDialog() == DialogResult.OK)
                MainFolderPath = pathSave.SelectedPath + @"\TiaPortalOpenness";
        }

        /// <summary>
        /// Handles the Executed event of the StandardImportRobotListFileCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void StandardImportSchnittstelleFileCommand_Executed(object sender, EventArgs e)
        {
            var pathSave = new OpenFileDialog();
            pathSave.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            pathSave.ShowDialog();
            if (File.Exists(pathSave.FileName))
                DefaultSchnittstellePath = pathSave.FileName;
        }

        /// <summary>
        /// Handles the Executed event of the StandardPlcDBFileCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void StandardPlcDBFileCommand_Executed(object sender, EventArgs e)
        {
            var pathSave = new OpenFileDialog();
            pathSave.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            pathSave.ShowDialog();
            if (File.Exists(pathSave.FileName))
                DefaultPlcDBPath = pathSave.FileName;
        }

        /// <summary>
        /// Handles the Executed event of the StandardSequenceFileCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void StandardSequenceFileCommand_Executed(object sender, EventArgs e)
        {
            var pathSave = new OpenFileDialog();
            pathSave.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            pathSave.ShowDialog();
            if (File.Exists(pathSave.FileName))
                DefaultSequencePath = pathSave.FileName;
        }

        /// <summary>
        /// Handles the Executed event of the StandartImportNetworkListFileCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void StandartImportNetworkListFileCommand_Executed(object sender, EventArgs e)
        {
            var pathSave = new OpenFileDialog();
            pathSave.Filter = "Excel Files|*.xlsm";
            pathSave.ShowDialog();
            if (File.Exists(pathSave.FileName))
                DefaultNetworkListPath = pathSave.FileName;
        }

        /// <summary>
        /// Handles the Executed event of the StandartImportEPlanFileCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void StandartImportEPlanFileCommand_Executed(object sender, EventArgs e)
        {
            var pathSave = new OpenFileDialog();
            pathSave.Filter = "Pdf Files|*.pdf";
            pathSave.ShowDialog();
            if (File.Exists(pathSave.FileName))
                DefaultEPlanPath = pathSave.FileName;
        }

        /// <summary>
        /// Handles the Executed event of the SaveSettingsCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for SaveSettingsCommand_Executed
        private void SaveSettingsCommand_Executed(object sender, EventArgs e)
        {
            try
            {
                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkbook = null;

                #region Check if paths exist
                if (!String.IsNullOrEmpty(DefaultExportFolderPath) && !Directory.Exists(DefaultExportFolderPath))
                {
                    MessageBox.Show("Export path does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!String.IsNullOrEmpty(DefaultEPlanPath) && !File.Exists(DefaultEPlanPath))
                {
                    MessageBox.Show("EPlan file path does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!String.IsNullOrEmpty(DefaultSchnittstellePath) && !File.Exists(DefaultSchnittstellePath))
                {
                    MessageBox.Show("Schnittstelle file path does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                } 
                else
                {
                    if (!String.IsNullOrEmpty(DefaultSchnittstellePath) && File.Exists(DefaultSchnittstellePath) && DefaultSchnittstellePath != Settings.Default.DefaultSchnittstellePath)
                    {
                        xlWorkbook = OpennessHelper.GetExcelFile(DefaultSchnittstellePath, xlApp);
                        Excel.Worksheet ws = null;
                        try
                        {
                            ws = xlWorkbook.Sheets["000000R01"];
                        }
                        catch (Exception)
                        {
                            ws = null;
                        }

                        if (ws == null)
                        {
                            MessageBox.Show("Invalid Schnittstelle File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            xlWorkbook.Close(0);
                            xlApp.Quit();
                            return;
                        }

                        object[,] matrix = OpennessHelper.ExcelToMatrix(ws);

                        if (!OpennessHelper.IsSchnittstelle(matrix))
                        {
                            MessageBox.Show("Invalid Schnittstelle File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            xlWorkbook.Close(0);
                            xlApp.Quit();
                            return;
                        }

                        xlWorkbook.Close(0);
                    }
                }
                if (!String.IsNullOrEmpty(DefaultNetworkListPath) && !File.Exists(DefaultNetworkListPath))
                {
                    MessageBox.Show("NetworkList file path does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    if (!String.IsNullOrEmpty(DefaultNetworkListPath) && File.Exists(DefaultNetworkListPath) && DefaultNetworkListPath != Settings.Default.DefaultNetworkListPath)
                    {
                        xlWorkbook = OpennessHelper.GetExcelFile(DefaultNetworkListPath, xlApp);
                        Excel.Worksheet ws = null;

                        foreach (Excel.Worksheet item in xlWorkbook.Worksheets)
                        {
                            object[,] matrix = OpennessHelper.ExcelToMatrix(item);

                            if (OpennessHelper.IsNetworkList(matrix))
                            {
                                ws = item;
                                break;
                            }
                        }

                        if (ws == null)
                        {
                            MessageBox.Show("Invalid NetworkList File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            xlWorkbook.Close(0);
                            xlApp.Quit();
                            return;
                        }

                        xlWorkbook.Close(0);
                    }
                }
                if (!String.IsNullOrEmpty(DefaultPlcDBPath) && !File.Exists(DefaultPlcDBPath))
                {
                    MessageBox.Show("PLC DB file path does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    if (!String.IsNullOrEmpty(DefaultPlcDBPath) && File.Exists(DefaultPlcDBPath) && DefaultPlcDBPath != Settings.Default.DefaultPlcDBPath)
                    {
                        xlWorkbook = OpennessHelper.GetExcelFile(DefaultPlcDBPath, xlApp);
                        Excel.Worksheet ws = null;
                        try
                        {
                            ws = xlWorkbook.Sheets["EngAssist"];
                        }
                        catch (Exception)
                        {
                            ws = null;
                        }

                        if (ws == null)
                        {
                            MessageBox.Show("Invalid PLC DB File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            xlWorkbook.Close(0);
                            xlApp.Quit();
                            return;
                        }

                        object[,] matrix = OpennessHelper.ExcelToMatrix(ws);

                        if (!OpennessHelper.IsPlcDb(matrix))
                        {
                            MessageBox.Show("Invalid PLC DB File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            xlWorkbook.Close(0);
                            xlApp.Quit();
                            return;
                        }

                        xlWorkbook.Close(0);
                    }
                }
                if (!String.IsNullOrEmpty(DefaultSequencePath) && !File.Exists(DefaultSequencePath))
                {
                    MessageBox.Show("Sequence file path does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    if (!String.IsNullOrEmpty(DefaultSequencePath) && File.Exists(DefaultSequencePath) && DefaultSequencePath != Settings.Default.DefaultSequencePath)
                    {
                        xlWorkbook = OpennessHelper.GetExcelFile(DefaultSequencePath, xlApp);
                        Excel.Worksheet ws = null;
                        try
                        {
                            ws = xlWorkbook.Sheets["AS_000000"];
                        }
                        catch (Exception)
                        {
                            ws = null;
                        }

                        if (ws == null)
                        {
                            MessageBox.Show("Invalid Sequence File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            xlWorkbook.Close(0);
                            xlApp.Quit();
                            return;
                        }

                        object[,] matrix = OpennessHelper.ExcelToMatrix(ws);

                        if (!OpennessHelper.IsSequence(matrix))
                        {
                            MessageBox.Show("Invalid Sequence File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            xlWorkbook.Close(0);
                            xlApp.Quit();
                            return;
                        }

                        xlWorkbook.Close(0);
                    }
                }
                #endregion

                xlApp.Quit();

                if (Settings.Default.MainFolderPath != MainFolderPath) // If user changed MainFolderPath
                {
                    Directory.CreateDirectory(MainFolderPath);

                    //Change RobotList and PLCDB Excel paths if the files were inside MainFolderPath
                    if (DefaultSchnittstellePath.Contains(Settings.Default.MainFolderPath))
                        DefaultSchnittstellePath = DefaultSchnittstellePath.Replace(Settings.Default.MainFolderPath, MainFolderPath);
                    if (DefaultPlcDBPath.Contains(Settings.Default.MainFolderPath))
                        DefaultPlcDBPath = DefaultPlcDBPath.Replace(Settings.Default.MainFolderPath, MainFolderPath);
                    if (DefaultSequencePath.Contains(Settings.Default.MainFolderPath))
                        DefaultSequencePath = DefaultSequencePath.Replace(Settings.Default.MainFolderPath, MainFolderPath);

                    string SourcePath = Settings.Default.MainFolderPath;

                    //Create all the directories
                    foreach (string dirPath in Directory.GetDirectories(SourcePath, "*", SearchOption.AllDirectories))
                        Directory.CreateDirectory(dirPath.Replace(SourcePath, MainFolderPath));

                    //Copy all the files & Replaces any files with the same name
                    foreach (string newPath in Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories))
                        File.Copy(newPath, newPath.Replace(SourcePath, MainFolderPath), true);

                    try
                    {
                        Directory.Delete(SourcePath, true);
                    } 
                    catch(Exception ex)
                    {
                        WriteStatusEntry("Exception thrown while deleting old directory of mainfolder: " + ex.Message);
                    }

                    LoadMainTreeView();
                }

                Settings.Default.DefaultExportPath = DefaultExportFolderPath;
                Settings.Default.MainFolderPath = MainFolderPath;
                Settings.Default.DefaultSchnittstellePath = DefaultSchnittstellePath;
                Settings.Default.DefaultPlcDBPath = DefaultPlcDBPath;
                Settings.Default.DefaultSequencePath = DefaultSequencePath;
                Settings.Default.DefaultNetworkListPath = DefaultNetworkListPath;
                Settings.Default.DefaultEPlanPath = DefaultEPlanPath;
                Settings.Default.UserInterfaceEnabled = UserInterfaceEnabled;
                Settings.Default.ExportWithDefaults = ExportOptionsDefaults;
                Settings.Default.ExportReadOnly = ExportOptionsReadOnly;
                Settings.Default.HideAssemblySelection = HideAssemblySelection;
                Settings.Default.EngineeringVersion = EngineeringVersion;
                Settings.Default.AssemblyVersion = AssemblyVersion;

                Settings.Default.Save();
            }
            catch (Exception ce)
            {
                DialogService.ShowErrorMessageBox(ce.Message);
            }
        }
        #endregion
        
        #region Libraries
        /// <summary>
        /// Handles the Executed event of the RefreshLibraryCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for RefreshLibraryCommand_Executed
        private void RefreshLibraryCommand_Executed(object sender, EventArgs e)
        {
            LoadLibraryTreeView();
            LoadGlobalLibraryTreeView();
            WriteStatusEntry("Library navigation updated");
        }

        /// <summary>
        /// Handles the event "ValidationEvent"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="exception"></param>
        private void ValidationEventHandler(object sender, ValidationEventArgs exception)
        {
            XmlSeverityType type = XmlSeverityType.Warning;
            if (Enum.TryParse<XmlSeverityType>("Error", out type))
            {
                if (type == XmlSeverityType.Error)
                {
                    throw new Exception(exception.Message);
                }
            }
        }

        /// <summary>
        /// Handles the Executed event of the InvokeLibToProjectCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for InvokeLibToProjectCommand_Executed
        private void InvokeLibToProjectCommand_Executed(object sender, EventArgs e)
        {
            CopyLibElement = true;

            _sourceIndex = 2;
            _destinationIndex = 1;
        }

        /// <summary>
        /// Handles the Executed event of the InvokeGlobalToProjectCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for InvokeGlobalToProjectCommand_Executed
        private void InvokeGlobalToProjectCommand_Executed(object sender, EventArgs e)
        {
            CopyLibElement = true;

            _sourceIndex = 1;
            _destinationIndex = 1;
        }

        /// <summary>
        /// Handles the Executed event of the InvokeGlobalToLibCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for InvokeGlobalToLibCommand_Executed
        private void InvokeGlobalToLibCommand_Executed(object sender, EventArgs e)
        {
            CopyLibElement = true;

            _sourceIndex = 1;
            _destinationIndex = 2;
        }

        /// <summary>Handles the Executed event of the CancelCopyCommand control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for CancelCopyCommand_Executed
        private void CancelCopyCommand_Executed(object sender, EventArgs e)
        {
            CopyLibElement = false;
            CopyDestination = null;
            CopySource = null;
            _sourceIndex = 0;
            _destinationIndex = 0;
        }

        /// <summary>Handles the Executed event of the CopyLibCommand control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for CopyLibCommand_Executed
        private void CopyLibCommand_Executed(object sender, EventArgs e)
        {
            #region Type
            if (CopySource != null && CopyDestination != null)
            {
                var type = CopySource.Tag as CodeBlockLibraryType;
                if (type != null)
                {
                    var target = _copyDestination.Tag as PlcBlockGroup;
                    if (target != null)
                    {
                        try
                        {
                            target.Blocks.CreateFrom((CodeBlockLibraryTypeVersion)type.Versions.Last());
                            WriteStatusEntry("Type successfully copied.");
                            CopyLibElement = false;
                        }
                        catch (EngineeringException ee)
                        {
                            WriteStatusEntry(ee.Message);
                        }
                    }
                    else
                        WriteStatusEntry("Invalid destination selected.");
                    return;
                }
                #endregion
                //TODO Prüfen
                var mcopy = CopySource.Tag as MasterCopy;
                if (mcopy != null)
                {
                    if (_destinationIndex == 1)
                    {
                        try
                        {
                            var realTarget = mcopy.ContentDescriptions.FirstOrDefault().ContentType;
                            var targetIdent = realTarget.BaseType.Name;
                            #region
                            dynamic target;

                            if (targetIdent != "Object")
                            {
                                switch (targetIdent)
                                {
                                    #region
                                    case "Device":
                                        target = _copyDestination.Tag as Project;
                                        InsertMasterCopy(target.DeviceGroup, mcopy);
                                        break;
                                    case "DeviceItem":
                                        target = _copyDestination.Tag as Device;
                                        InsertMasterCopy(target.DeviceItems, mcopy);
                                        break;
                                    case "DeviceUserGroup":
                                        target = _copyDestination.Tag as Project;
                                        InsertMasterCopy(target.DeviceGroups, mcopy);
                                        break;
                                    case "CodeBlock":
                                    case "DataBlock":
                                        target = _copyDestination.Tag as PlcBlockGroup;
                                        InsertMasterCopy(target.Blocks, mcopy);
                                        break;
                                    case "PlcBlockUserGroup":
                                        target = _copyDestination.Tag as PlcBlockSystemGroup;
                                        InsertMasterCopy(target.Groups, mcopy);
                                        break;
                                    case "PlcTag":
                                        target = _copyDestination.Tag as PlcTagTable;
                                        InsertMasterCopy(target.Tags, mcopy);
                                        break;
                                    case "PlcTagTable":
                                        target = _copyDestination.Tag as PlcTagTableGroup;
                                        InsertMasterCopy(target.TagTables, mcopy);
                                        break;
                                    case "PlcTagTableUserGroup":
                                        target = _copyDestination.Tag as PlcTagTableGroup;
                                        InsertMasterCopy(target.Groups, mcopy);
                                        break;
                                    case "PlcType":
                                        target = _copyDestination.Tag as PlcTypeSystemGroup;
                                        InsertMasterCopy(target.Types, mcopy);
                                        break;
                                    case "PlcTypeUserGroup":
                                        target = _copyDestination.Tag as PlcTypeSystemGroup;
                                        InsertMasterCopy(target.Groups, mcopy);
                                        break;
                                        #endregion
                                }
                            }
                            else
                            {
                                targetIdent = realTarget.Name;
                                switch (targetIdent)
                                {
                                    #region
                                    case "VBScript":
                                        target = _copyDestination.Tag as VBScriptFolder;
                                        InsertMasterCopy(target.VBScripts, mcopy);
                                        break;
                                    case "VBScriptUserFolder":
                                        target = _copyDestination.Tag as VBScriptFolder;
                                        InsertMasterCopy(target.Folders, mcopy);
                                        break;
                                    case "Screen":
                                        target = _copyDestination.Tag as ScreenFolder;
                                        InsertMasterCopy(target.Screens, mcopy);
                                        break;
                                    case "ScreenTemplate":
                                        target = _copyDestination.Tag as ScreenTemplateFolder;
                                        InsertMasterCopy(target.ScreenTemplates, mcopy);
                                        break;
                                    case "ScreenTemplateUserFolder":
                                        target = _copyDestination.Tag as ScreenTemplateFolder;
                                        InsertMasterCopy(target.Folders, mcopy);
                                        break;
                                    case "ScreenUserFolder":
                                        target = _copyDestination.Tag as ScreenFolder;
                                        InsertMasterCopy(target.Folders, mcopy);
                                        break;
                                        #endregion
                                }
                            }


                        }
                        catch (Exception)
                        {
                            // ignored
                        }

                        #endregion
                        return;
                    }
                    if (_destinationIndex == 2)
                    {
                        var destinationFolder = _copyDestination.Tag as MasterCopyFolder;
                        if (destinationFolder != null)
                        {
                            destinationFolder.MasterCopies.CreateFrom(mcopy);
                            return;
                        }
                    }
                }
                WriteStatusEntry("Invalid target selected.");
            }
            else
            {
                WriteStatusEntry("Please select your targets first");
            }
        }

        private void InsertMasterCopy(dynamic target, MasterCopy masterC)
        {
            if (target != null)
            {
                try
                {
                    target.CreateFrom(masterC);
                    WriteStatusEntry("Maser Copy successfully copied.");
                    CopyLibElement = false;
                }
                catch (EngineeringException ee)
                {
                    WriteStatusEntry(ee.Message);
                }
            }
            else
                WriteStatusEntry("Invalid destination selected.");
        }
        #endregion

        #endregion

        #region Private Methods
        /// <summary>
        /// Close Tia Portal Connection
        /// </summary>
        private void CloseTiaPortalConnection()
        {
            if (_tiaPortal != null)
            { 
                _tiaPortal.Dispose();
                _tiaPortal = null;
                _tiaPortalProject = null;
                _tiaPortalProjects = null;

                ProjectTree.View.Clear();
                LibraryTree.View.Clear();

                _plcsToCompile.Clear();

                PortalOpened = false;
                GlobalLibraryOpened = false;
                TransactionRunning = false;
                ProjectOpened = false;
                CreateItem = false;
                DeleteItem = false;
                OpenEditor = false;
                Compile = false;
                Compile = false;
                ImportEnabled = false;
                ExportEnabled = false;
                AddExternalSource = false;
                GenerateBlockFromSource = false;
                ConnectPlc = false;
                PropertiesShown = false;

                WriteStatusEntry("TIA Portal connection closed.");
            }
        }

        /// <summary>
        /// Connect to TiaPortal
        /// </summary>
        /// <param name="renamePLC"></param>
        private void ConnectToTia(bool renamePLC)
        {
            var model = new TargetSelectViewModel(CreateInstanceTreeView());
            _subWindow = new TargetSelectView(model);
            model.CloseAction = () => _subWindow.Close();
            _subWindow.ShowDialog();

            if (model.Result && model.SelectedTarget != null)
            {
                try
                {
                    var process = model.SelectedTarget as TiaPortalProcess;
                    ProjectTree.View.Clear();

                    if (_tiaPortalProject != null)
                        _tiaPortal.Dispose();

                    if (process == null) return;
                    _tiaPortal = process.Attach();
                    _tiaPortalProjects = _tiaPortal.Projects;
                    if (_tiaPortalProjects.Count == 0)
                    {
                        var emptyProject = new TreeView();
                        var emptyProjectItem = new TreeViewItem();
                        emptyProjectItem.Header = "TIA Portal without project connected";

                        emptyProject.Items.Add(emptyProjectItem);
                        ProjectTree.View.Clear();
                        ProjectTree.View.Add(emptyProject);

                        ProjectOpened = false;
                        PortalOpened = true;
                        WriteStatusEntry("TIA Portal without project connected");
                        return;
                    }
                    ProjectOpened = true;
                    PortalOpened = true;
                    _tiaPortalProject = _tiaPortalProjects.First();

                    if (!renamePLC) 
                        LoadProjectTreeView();
                    
                    CheckConsistency();

                    WriteStatusEntry(string.Format(CultureInfo.InvariantCulture, "Project: {0} connected", _tiaPortalProject.Path));
                }
                catch (EngineeringException ex)
                {
                    WriteStatusEntry(ex.Message);
                }
            }
        }

        /// <summary>
        /// Find arg group on tree
        /// </summary>
        /// <param name="tvi"></param>
        private void FindArgGroup(TreeViewItem tvi)
        {
            if (argGroup != null) return;

            foreach (TreeViewItem item in tvi.Items)
            {
                if(item.Tag is PlcBlockUserGroup && item.Header.ToString().ToLower().Equals("arg"))
                {
                    argGroup = item.Tag;
                    return;
                }

                if (item.Items.Count > 0)
                    FindArgGroup(item);
            }
        }

        /// <summary>Reads the configuration.</summary>
        /// TODO Edit XML Comment Template for ReadConfiguration
        private void ReadConfiguration()
        {
            DefaultExportFolderPath = Settings.Default.DefaultExportPath;
            MainFolderPath = Settings.Default.MainFolderPath;
            DefaultSchnittstellePath = Settings.Default.DefaultSchnittstellePath;
            DefaultPlcDBPath = Settings.Default.DefaultPlcDBPath;
            DefaultSequencePath = Settings.Default.DefaultSequencePath;
            DefaultNetworkListPath = Settings.Default.DefaultNetworkListPath;
            DefaultEPlanPath = Settings.Default.DefaultEPlanPath;
            UserInterfaceEnabled = Settings.Default.UserInterfaceEnabled;
            ExportOptionsReadOnly = Settings.Default.ExportReadOnly;
            ExportOptionsDefaults = Settings.Default.ExportWithDefaults;
            HideAssemblySelection = Settings.Default.HideAssemblySelection;
            EngineeringVersion = Settings.Default.EngineeringVersion;
            AssemblyVersion = Settings.Default.AssemblyVersion;
        }

        /// <summary>
        /// Create main folder with items 
        /// </summary>
        private void CreateMainFolders()
        {
            Directory.CreateDirectory(MainFolderPath);
            Directory.CreateDirectory(Path.Combine(MainFolderPath, "50_Stationen"));
            Directory.CreateDirectory(Path.Combine(MainFolderPath, "50_Stationen", "DB-Anwender"));
            Directory.CreateDirectory(Path.Combine(MainFolderPath, "50_Stationen", "DB-Instanzen"));
            Directory.CreateDirectory(Path.Combine(MainFolderPath, "60_Roboter"));
            Directory.CreateDirectory(Path.Combine(MainFolderPath, "60_Roboter", "DB-Anwender"));
            Directory.CreateDirectory(Path.Combine(MainFolderPath, "60_Roboter", "DB-Instanzen"));
            Directory.CreateDirectory(Path.Combine(MainFolderPath, "2_Safety"));
            Directory.CreateDirectory(Path.Combine(MainFolderPath, "2_Safety", "DB-Anwender"));
            Directory.CreateDirectory(Path.Combine(MainFolderPath, "2_Safety", "DB-Instanzen"));
            Directory.CreateDirectory(Path.Combine(MainFolderPath, "40_Betriebsarten"));
            Directory.CreateDirectory(Path.Combine(MainFolderPath, "40_Betriebsarten", "DB-Anwender"));
            Directory.CreateDirectory(Path.Combine(MainFolderPath, "40_Betriebsarten", "DB-Instanzen"));
            Directory.CreateDirectory(Path.Combine(MainFolderPath, "100_ARG_Typ_Strg"));
            Directory.CreateDirectory(Path.Combine(MainFolderPath, "100_ARG_Typ_Strg", "DB-Anwender"));
            Directory.CreateDirectory(Path.Combine(MainFolderPath, "100_ARG_Typ_Strg", "DB-Instanzen"));
            Directory.CreateDirectory(Path.Combine(MainFolderPath, "PLC Tags"));
            Directory.CreateDirectory(Path.Combine(MainFolderPath, "Excel Files"));
            Directory.CreateDirectory(Path.Combine(MainFolderPath, "Symbolics"));
            Directory.CreateDirectory(Path.Combine(MainFolderPath, "Logs"));
            Directory.CreateDirectory(Path.Combine(MainFolderPath, "PLC Configs"));
        }

        /// <summary>Initializes the collections and lists.</summary>
        /// TODO Edit XML Comment Template for InitializeCollectionsAndLists
        private void InitializeCollectionsAndLists()
        {
            RobBase = new List<List<RobotBase>>();
            RobTecnologies = new List<List<RobotTecnologie>>();
            RobSafeRangeMonitoring = new List<List<RobotSafeRangeMonitoring>>();
            RobSafeOperations = new List<List<RobotSafeOperation>>();
            RobsInfo = new List<RobotInfo>();
            FoldersList = new ObservableCollection<FolderInfo>();
            MatrixList = new List<object[,]>();
            SheetNamesList = new List<string>();
            expandedTvitems = new List<string>();

            StatusListView = new ObservableCollection<string>();
            PropertiesListView = new Dictionary<string, string>();
            ProjectTree = new TreeViewHandler(new RelayCommand<DependencyPropertyEventArgs>(TreeViewItemSelectedChangedCallback));
            MainTree = new TreeViewHandler(new RelayCommand<DependencyPropertyEventArgs>(TreeViewMainChangedCallback));
            LibraryTree = new TreeViewHandler(new RelayCommand<DependencyPropertyEventArgs>(TreeViewLibraryChangedCallback));
            GlobalLibraryTree = new TreeViewHandler(new RelayCommand<DependencyPropertyEventArgs>(TreeViewGlobalLibraryChangedCallback));
            SymbolicsTree = new TreeViewHandler(new RelayCommand<DependencyPropertyEventArgs>(TreeViewSymbolicsChangedCallback));
           
        _plcsToCompile = new HashSet<PlcSoftware>();
        }

        /// <summary>Writes the status entry.</summary>
        /// <param name="statusText">The status text.</param>
        /// TODO Edit XML Comment Template for WriteStatusEntry
        private void WriteStatusEntry(string statusText)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(() =>
            {
                StatusListView.Insert(0, DateTime.Now + ": " + statusText);
            }));
        }

        /// <summary>Checks the consistency.</summary>
        /// TODO Edit XML Comment Template for CheckConsistency
        private void CheckConsistency()
        {
            //Show Message Box if inconsistent blocks are found
            if (_plcsToCompile.Count != 0)
            {
                var result =
                    DialogService.ShowWarningMessageBox("Inconsistent Elements are found.", "Should the System compile the inconsistent elements?");

                switch (result)
                {
                    case MessageBoxResult.No:
                        break;

                    case MessageBoxResult.Yes:
                        IsLoading = true;
                        WriteStatusEntry("Start Compile");

                        foreach (var plc in _plcsToCompile)
                        {
                            var cpResult = plc.GetService<ICompilable>().Compile();
                            if (cpResult != null)
                            {
                                foreach (var message in cpResult.Messages)
                                {
                                    WriteStatusEntry("Path: " + message.Path + " State: " + message.State + " Description: " + message.Description);
                                }
                            }
                        }
                        _plcsToCompile.Clear();

                        IsLoading = false;
                        WriteStatusEntry("Finish Compile");
                        break;
                }
            }
        }

        /// <summary>Creates the instance TreeView.</summary>
        /// <returns>TreeView</returns>
        /// TODO Edit XML Comment Template for CreateInstanceTreeView
        private static TreeView CreateInstanceTreeView()
        {
            var instanceTreeView = new TreeView();

            foreach (var process in TiaPortal.GetProcesses())
            {
                var uiInfo = string.Empty;
                if (process.Mode == TiaPortalMode.WithUserInterface)
                    uiInfo = " (UI)";
                var processId = " [" + process.Id + "]";

                if (process.ProjectPath != null)
                {
                    var instanceItem = new TreeViewItem();

                    instanceItem.Header = Path.GetFileNameWithoutExtension(process.ProjectPath.ToString());
                    instanceItem.Tag = process;
                    instanceTreeView.Items.Add(instanceItem);
                }
                else
                {
                    var instanceItem = new TreeViewItem();
                    instanceItem.Header = "No project" + uiInfo + processId;
                    instanceItem.Tag = process;
                    instanceTreeView.Items.Add(instanceItem);
                }
            }
            return instanceTreeView;
        }

        /// <summary>
        /// Import Excel or XML file to MainTreeView
        /// </summary>
        /// <param name="file"></param>
        private bool ImportFileToMainTreeView(string file)
        {
            string excelsPath = Path.Combine(MainFolderPath, "Excel Files");
            string symbolicsPath = Path.Combine(MainFolderPath, "Symbolics");
            string plcTagsPath = Path.Combine(MainFolderPath, "PLC Tags");
            XmlSchemaSet schema = new XmlSchemaSet();
            schema.Add(null, "XMLStructure.xsd");
            string fileName = Path.GetFileName(file);
            bool upload = true;

            if (Path.GetExtension(file).ToLower().Contains("xml"))
            {
                XmlReader rd = XmlReader.Create(file);
                XDocument xDoc = XDocument.Load(rd);

                bool isSymbolic = true;
                bool isPlcTagTable = false;

                if (xDoc.Descendants("SW.Tags.PlcTagTable").Any())
                    isPlcTagTable = true;

                try
                {
                    xDoc.Validate(schema, ValidationEventHandler);
                }
                catch (Exception)
                {
                    isSymbolic = false;
                }

                if (isSymbolic || isPlcTagTable)
                {
                    string filePath = "";

                    if (isPlcTagTable)
                        filePath = Path.Combine(plcTagsPath, fileName);
                    else
                        filePath = Path.Combine(symbolicsPath, fileName);

                    if (File.Exists(filePath))
                    {
                        if (System.Windows.MessageBox.Show("File \"" + fileName + "\" already exists.\n\nDo you want to overwrite it?", "Overwrite", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            File.Delete(filePath);
                            File.Copy(file, filePath);
                        }
                    }
                    else
                        File.Copy(file, filePath);
                }
                else 
                    upload = false;

                rd.Close();
            }
            else
            {
                string type = OpennessHelper.CheckFileType(file);
                string folderName = OpennessHelper.CreateKetFolder(file, type);
                string filePath = Path.Combine(excelsPath, fileName);

                if (!string.IsNullOrEmpty(folderName))
                {
                    Directory.CreateDirectory(Path.Combine(excelsPath, folderName));
                    filePath = Path.Combine(excelsPath, folderName, fileName);
                }

                switch (type)
                {
                    //case "rob":
                    case "plcDB":
                    case "sequence":
                    case "symbolic":
                        if (File.Exists(filePath))
                        {
                            if (System.Windows.MessageBox.Show("File \"" + fileName + "\" already exists.\n\nDo you want to overwrite it?", "Overwrite", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                File.Delete(filePath);
                                File.Copy(file, filePath);
                            }
                        }
                        else
                            File.Copy(file, filePath);
                        break;
                    default:
                        upload = false;
                        break;
                }
            }

            return upload;
        }

        /// <summary>
        /// Load Symbolic Info
        /// </summary>
        private void LoadSymbolic(string path)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = OpennessHelper.GetExcelFile(path, xlApp);
            XmlDocument xmlSymbolic = new XmlDocument();

            foreach (Excel.Worksheet xlWorksheet in xlWorkbook.Worksheets)
            {
                if (xlWorksheet.Name.Contains("SEW")) break;    // REMOVE THIS LINE TO PROGRAM PLC

                int lastRow = xlWorksheet.Cells.Find("*", Missing.Value, Missing.Value, Missing.Value,
                    Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlPrevious, false, Missing.Value, Missing.Value).Row;

                int lastCol = xlWorksheet.Cells.Find("*", Missing.Value, Missing.Value, Missing.Value,
                    Excel.XlSearchOrder.xlByColumns, Excel.XlSearchDirection.xlPrevious, false, Missing.Value, Missing.Value).Column;

                Excel.Range min = xlWorksheet.Range["A1"];
                Excel.Range max = xlWorksheet.Range[OpennessHelper.ColumnIndexToColumnLetter(lastCol) + "" + lastRow];

                object[,] matrix = OpennessHelper.ExcelToMatrix(xlWorksheet);
                string type = "";

                switch (xlWorksheet.Name)
                {
                    case "Grund":
                        RobBase = OpennessHelper.GetRobotBase(matrix, max);
                        break;

                    case "Basic_Slave":
                    case "Laser_Slave":

                        if (xlWorksheet.Name.Equals("Basic_Slave"))
                            type = "Basic Slave";
                        else
                            type = "Laser Slave";

                        var robTecs = OpennessHelper.GetRobotTecnologies(matrix, max, type);

                        if (RobTecnologies.Count > 0)
                        {
                            foreach (var tec in robTecs[0])
                            {
                                RobTecnologies[0].Add(tec);
                            }

                            foreach (var tec in robTecs[1])
                            {
                                RobTecnologies[1].Add(tec);
                            }
                        }
                        else
                        {
                            RobTecnologies = robTecs;
                        }
                        break;

                    case "Rob Safe Range Monitoring":
                        RobSafeRangeMonitoring = OpennessHelper.GetRobotSafeRangeMonitoring(matrix, max);
                        break;

                    case "Rob Safe Operation":
                        RobSafeOperations = OpennessHelper.GetRobotSafeOperation(matrix, max);
                        break;

                    case "EngAssist":
                        RobsInfo = OpennessHelper.GetCreatedRobotsInfo(matrix);
                        OpennessHelper.SetCacheData("SymbolicRobsInfo;path:" + path, RobsInfo);
                        break;
                }
            }
            xlWorkbook.Close(0);
            xlApp.Quit();

            // Store RobBase in Robot.RobBase
            var secRobBase = new List<RobotBase>();
            foreach (var o in RobBase[0])
            {
                secRobBase.Add(new RobotBase(o.Symbolic, o.DataType, o.Address, o.Comment));
            }
            Robot.RobBase.Add(secRobBase);
            secRobBase = new List<RobotBase>();
            foreach (var i in RobBase[1])
            {
                secRobBase.Add(new RobotBase(i.Symbolic, i.DataType, i.Address, i.Comment));
            }
            Robot.RobBase.Add(secRobBase);

            // Store RobSafeOperations in Robot.RobSafeOperations
            var secRobSafeOperations = new List<RobotSafeOperation>();
            foreach (var o in RobSafeOperations[0])
            {
                secRobSafeOperations.Add(new RobotSafeOperation(o.Symbolic, o.DataType, o.Address, o.Comment));
            }
            Robot.RobSafeOperations.Add(secRobSafeOperations);
            secRobSafeOperations = new List<RobotSafeOperation>();
            foreach (var i in RobSafeOperations[1])
            {
                secRobSafeOperations.Add(new RobotSafeOperation(i.Symbolic, i.DataType, i.Address, i.Comment));
            }
            Robot.RobSafeOperations.Add(secRobSafeOperations);

            // Store RobSafeRangeMonitoring in Robot.RobSafeRangeMonitoring
            var secRobSafeRangeMonitoring = new List<RobotSafeRangeMonitoring>();
            foreach (var o in RobSafeRangeMonitoring[0])
            {
                secRobSafeRangeMonitoring.Add(new RobotSafeRangeMonitoring(o.Symbolic, o.DataType, o.Address, o.Comment));
            }
            Robot.RobSafeRangeMonitoring.Add(secRobSafeRangeMonitoring);
            secRobSafeRangeMonitoring = new List<RobotSafeRangeMonitoring>();
            foreach (var i in RobSafeRangeMonitoring[1])
            {
                secRobSafeRangeMonitoring.Add(new RobotSafeRangeMonitoring(i.Symbolic, i.DataType, i.Address, i.Comment));
            }
            Robot.RobSafeRangeMonitoring.Add(secRobSafeRangeMonitoring);

            // Store RobTecnologies in Robot.RobTecnologies
            var secRobTecnologies = new List<RobotTecnologie>();
            foreach (var o in RobTecnologies[0])
            {
                secRobTecnologies.Add(new RobotTecnologie(o.FBNumber, o.Name, o.Type, o.Symbolic, o.DataType, o.Address, o.Comment));
            }
            Robot.RobTecnologies.Add(secRobTecnologies);
            secRobTecnologies = new List<RobotTecnologie>();
            foreach (var i in RobTecnologies[1])
            {
                secRobTecnologies.Add(new RobotTecnologie(i.FBNumber, i.Name, i.Type, i.Symbolic, i.DataType, i.Address, i.Comment));
            }
            Robot.RobTecnologies.Add(secRobTecnologies);
        }

        /// <summary>Reads the properties.</summary>
        /// <param name="projectObject">The project object.</param>
        /// TODO Edit XML Comment Template for ReadProperties
        private void ReadProperties(IEngineeringObject projectObject)
        {
            var propertiesList = new Dictionary<string, string>();
            var attributeNames = projectObject.GetAttributeInfos();
            foreach (var info in attributeNames)
            {
                // Ignore property "_public_name"
                if (info.Name.ToUpperInvariant().Equals("_PUBLIC_NAME"))
                {
                    continue;
                }

                string propertyValue;
                if (projectObject.GetAttribute(info.Name) == null)
                {
                    propertyValue = "empty";
                }
                else
                {
                    propertyValue = projectObject.GetAttribute(info.Name).ToString();

                    // Ignore properties with XML-value
                    if (propertyValue.ToUpperInvariant().StartsWith("<?XML", StringComparison.Ordinal))
                    {
                        continue;
                    }
                }
                if (propertiesList.ContainsKey(info.Name) == false)
                    propertiesList.Add(info.Name, propertyValue);
            }
            PropertiesListView.Clear();
            PropertiesListView = propertiesList;

            PropertiesShown = true;
        }

        /// <summary>
        /// Reads symbolic properties.
        /// </summary>
        /// <param name="path"></param>
        private void ReadSymbolicProperties(string path)
        {
            if (string.IsNullOrEmpty(path) || Directory.Exists(path) || !File.Exists(path) || Path.GetExtension(path).ToLower() != ".xml") return;

            var propertiesList = new Dictionary<string, string>();
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode nRobot = doc.SelectSingleNode("//Document/Robot");
            XmlNodeList nTecnologies = doc.SelectNodes("//Document/Robot/Tecnologies/*");

            if (nRobot == null || nTecnologies == null) return;

            string name = nRobot.Attributes["name"].InnerText;
            string startAddress = nRobot.Attributes["startaddress"].InnerText;
            string robSafe = nRobot.Attributes["robsafe"].InnerText;
            string type = nRobot.Attributes["type"].InnerText;
  
            propertiesList.Add("Name", name);
            propertiesList.Add("Start Address", startAddress);
            propertiesList.Add("Robot Safe", robSafe);
            propertiesList.Add("Type", type);

            for (int i = 0; i < nTecnologies.Count; i++)
            {
                propertiesList.Add("Tecnologie " + (i+1), nTecnologies[i].InnerText);
            }

            PropertiesListView.Clear();
            PropertiesListView = propertiesList;

            PropertiesShown = true;
        }

        /// <summary>Writes the update message.</summary>
        /// <param name="result">The result.</param>
        /// TODO Edit XML Comment Template for WriteUpdateMessage
        private void WriteUpdateMessage(UpdateCheckResult result)
        {
            if (result == null)
                return;
            RecursiveWriteUpdateMessage(result.Messages, "");
        }

        /// <summary>Recursives the write update message.</summary>
        /// <param name="messages">The messages.</param>
        /// <param name="indent">The indent.</param>
        /// TODO Edit XML Comment Template for RecursiveWriteUpdateMessage
        private void RecursiveWriteUpdateMessage(UpdateCheckResultMessageComposition messages, string indent)
        {
            indent += "\t";
            foreach (var message in messages)
            {
                RecursiveWriteUpdateMessage(message.Messages, indent);
                WriteStatusEntry(indent + message.Description);
            }
        }

        /// <summary>Writes the compile message.</summary>
        /// <param name="result">The result.</param>
        /// TODO Edit XML Comment Template for WriteCompileMessage
        private void WriteCompileMessage(CompilerResult result)
        {
            if (result == null)
                return;
            RecursiveWriteCompileMessage(result.Messages, "");
        }

        /// <summary>Recursives the write compile message.</summary>
        /// <param name="messages">The messages.</param>
        /// <param name="indent">The indent.</param>
        /// TODO Edit XML Comment Template for RecursiveWriteCompileMessage
        private void RecursiveWriteCompileMessage(CompilerResultMessageComposition messages, string indent)
        {
            indent += "\t";
            foreach (var msg in messages)
            {
                RecursiveWriteCompileMessage(msg.Messages, indent);
                WriteStatusEntry(indent + msg.Description);
            }
        }

        /// <summary>Writes the compare result.</summary>
        /// <param name="result">The result.</param>
        /// TODO Edit XML Comment Template for WriteCompareResult
        private void WriteCompareResult(CompareResult result)
        {
            if (result == null)
                return;
            RecursiveWriteCompareResult(result.RootElement.Elements, "");
            WriteStatusEntry(string.Format(CultureInfo.InvariantCulture, "<{0}> <{1}> <{2}> <{3}> <{4}> ",
                    result.RootElement.LeftName,
                    result.RootElement.ComparisonResult,
                    result.RootElement.RightName,
                    result.RootElement.Parent,
                    result.RootElement.DetailedInformation));
        }

        /// <summary>Recursives the write compare result.</summary>
        /// <param name="compareResultComposition">The compare result Composition.</param>
        /// <param name="indent">The indent.</param>
        /// TODO Edit XML Comment Template for RecursiveWriteCompareResult
        private void RecursiveWriteCompareResult(CompareResultElementComposition compareResultComposition, string indent)
        {
            if (compareResultComposition == null)
                return;

            indent += "\t";
            foreach (var compareResultElement in compareResultComposition)
            {
                RecursiveWriteCompareResult(compareResultElement.Elements, indent);
                WriteStatusEntry(string.Format(CultureInfo.InvariantCulture, "{0}<{1}> <{2}> <{3}> <{4}> <{5}> ",
                    indent,
                    compareResultElement.LeftName,
                    compareResultElement.ComparisonResult,
                    compareResultElement.RightName,
                    compareResultElement.Parent,
                    compareResultElement.DetailedInformation));
            }
        }

        /// <summary>Crawls through the folders.</summary>
        /// <param name="projectTreeViewItem">The project TreeView item.</param>
        /// <param name="folder">The folder.</param>
        /// TODO Edit XML Comment Template for FolderCrawler
        private static void FolderCrawler(TreeViewItem projectTreeViewItem, DeviceUserGroup folder)
        {
            var folderItem = new TreeViewItem
            {
                Header = folder.Name,
                Tag = folder
            };
            projectTreeViewItem.Items.Add(folderItem);

            foreach (var device in folder.Devices)
            {
                var item = CreateDeviceTreeViewItem(device);

                folderItem.Items.Add(item);
            }

            foreach (var subFolder in folder.Groups)
            {
                FolderCrawler(folderItem, subFolder);
            }
        }

        /// <summary>Creates the device TreeView item.</summary>
        /// <param name="device">The device.</param>
        /// <returns>TreeViewItem</returns>
        /// TODO Edit XML Comment Template for CreateDeviceTreeViewItem
        private static TreeViewItem CreateDeviceTreeViewItem(Device device)
        {
            var hw = OpennessTreeViews.GetHardwareTreeView(device);
            TreeViewItem plc = null;
            TreeViewItem hmi = null;
            TreeViewItem item = null;

            var plcSoftware = OpennessHelper.GetPlcSoftware(device);
            var hmiTarget = OpennessHelper.GetHmiTarget(device);

            if (plcSoftware != null)
            {
                plc = new TreeViewItem();
                plc.Header = plcSoftware.Name;
                plc.Tag = plcSoftware;
                if (hmiTarget == null)
                    plc.Items.Add(hw);

                #region Program blocks
                //TreeViewItem for Blocks
                var blocks = OpennessTreeViews.GetBlocksTreeView(plcSoftware);
                plc.Items.Add(blocks);
                #endregion

                #region Tag Tables
                //TreeViewItem for TagTables
                var tagTables = OpennessTreeViews.GetTagTablesTreeView(plcSoftware);
                plc.Items.Add(tagTables);
                #endregion

                #region UDTs
                //TreeViewItem for UDTs
                var udts = OpennessTreeViews.GetDatatypesTreeView(plcSoftware);
                plc.Items.Add(udts);
                #endregion

                #region Extrenal source files
                // TreeViewItem for External source files
                var ext = OpennessTreeViews.GetExternalSourceFilesTreeView(plcSoftware);
                plc.Items.Add(ext);
                #endregion

                item = plc;
            }

            if (hmiTarget != null)
            {
                hmi = new TreeViewItem();
                hmi.Header = hw.Header;
                hmi.Tag = hmiTarget;
                if (plcSoftware == null)
                    hmi.Items.Add(hw);

                #region ScreenOverview
                var screenOverview = new TreeViewItem
                {
                    Header = "ScreenOverview",
                    Tag = hmiTarget.ScreenOverview
                };
                hmi.Items.Add(screenOverview);
                #endregion

                #region ScreenGlobalElements
                var screenGlobalElements = new TreeViewItem
                {
                    Header = "ScreenGlobalElements",
                    Tag = hmiTarget.ScreenGlobalElements
                };
                hmi.Items.Add(screenGlobalElements);
                #endregion

                #region Screens
                var screens = OpennessTreeViews.GetScreensTreeView(hmiTarget);
                hmi.Items.Add(screens);
                #endregion

                #region ScreenTemplates
                var templates = OpennessTreeViews.GetScreenTemplatesTreeView(hmiTarget);
                hmi.Items.Add(templates);
                #endregion

                #region PopUps
                var popUps = OpennessTreeViews.GetScreenPopupTreeView(hmiTarget);
                hmi.Items.Add(popUps);
                #endregion

                #region SlideIns
                var slideIns = OpennessTreeViews.GetScreenSlideInTreeView(hmiTarget);
                hmi.Items.Add(slideIns);
                #endregion

                #region TagTables
                var tagtables = OpennessTreeViews.GetTagTablesTreeView(hmiTarget);
                hmi.Items.Add(tagtables);
                #endregion

                #region Cycles
                var cycles = OpennessTreeViews.GetCyclesTreeView(hmiTarget);
                hmi.Items.Add(cycles);
                #endregion

                #region Connections
                var connections = OpennessTreeViews.GetConnectionsTreeView(hmiTarget);
                hmi.Items.Add(connections);
                #endregion

                #region VBScripts
                var vbScripts = OpennessTreeViews.GetScriptsTreeView(hmiTarget);
                hmi.Items.Add(vbScripts);
                #endregion

                #region GraphicLists
                var graphicLists = new TreeViewItem
                {
                    Header = "GraphicLists",
                    Tag = hmiTarget.GraphicLists
                };
                foreach (var list in hmiTarget.GraphicLists)
                {
                    graphicLists.Items.Add(new TreeViewItem { Header = list.Name, Tag = list });
                }
                hmi.Items.Add(graphicLists);
                #endregion

                #region TextLists
                var textLists = new TreeViewItem
                {
                    Header = "TextLists",
                    Tag = hmiTarget.TextLists
                };
                foreach (var list in hmiTarget.TextLists)
                {
                    textLists.Items.Add(new TreeViewItem { Header = list.Name, Tag = list });
                }
                hmi.Items.Add(textLists);
                #endregion

                item = hmi;
            }

            if (plcSoftware != null && hmiTarget != null && plc != null && hmi != null)
            {
                item = new TreeViewItem();
                item.Header = hw.Header;
                item.Items.Add(hw);
                item.Items.Add(plc);
                hmi.Header = hmiTarget.Name;
                item.Items.Add(hmi);
            }

            if (plcSoftware == null && hmiTarget == null)
                item = hw;

            return item;
        }

        #region Generate Robot Properties from XML
        /// <summary>
        /// Convert given XMLDocument to RobotBase
        /// </summary>
        /// <param name="doc"></param>
        /// <returns>RobotBase</returns>
        private List<List<RobotBase>> GenerateRobBase(XmlDocument doc)
        {
            var robBase = new List<List<RobotBase>>();
            var inputs = new List<RobotBase>();
            var outputs = new List<RobotBase>();

            XmlNodeList nl = doc.SelectNodes("//Document/Robot/Default/Base/*");

            foreach(XmlNode n in nl)
            {
                string symbolic = n.Attributes["symbolic"].Value;
                string datatype = n.Attributes["datatype"].Value;
                string address = n.Attributes["address"].Value;
                string comment = n.Attributes["comment"].Value;

                if (address.Contains('Q'))
                    outputs.Add(new RobotBase(symbolic, datatype, address, comment));
                else
                    inputs.Add(new RobotBase(symbolic, datatype, address, comment));
            }

            robBase.Add(outputs);
            robBase.Add(inputs);

            return robBase;
        }

        /// <summary>
        /// Convert given XMLDocument to RobotBase
        /// </summary>
        /// <param name="doc"></param>
        /// <returns>RobotTecnologies</returns>
        private List<List<RobotTecnologie>> GenerateRobTecnologies(XmlDocument doc)
        {
            var robTecs = new List<List<RobotTecnologie>>();
            var inputs = new List<RobotTecnologie>();
            var outputs = new List<RobotTecnologie>();

            XmlNodeList nlBasicSlave = doc.SelectNodes("//Document/Robot/Default/Tecnologies/Basicslave/*");
            XmlNodeList nlLaserSlave = doc.SelectNodes("//Document/Robot/Default/Tecnologies/Laserslave/*");

            foreach (XmlNode n in nlBasicSlave)
            {
                string type = "Basic Slave";
                string fbnumber = n.Attributes["fbnumber"].Value;
                string symbolic = n.Attributes["symbolic"].Value;
                string datatype = n.Attributes["datatype"].Value;
                string address = n.Attributes["address"].Value;
                string comment = n.Attributes["comment"].Value;
                string name = n.InnerText;

                if (address.Contains('Q'))
                    outputs.Add(new RobotTecnologie(fbnumber, name, type, symbolic, datatype, address, comment));
                else
                    inputs.Add(new RobotTecnologie(fbnumber, name, type, symbolic, datatype, address, comment));
            }

            foreach (XmlNode n in nlLaserSlave)
            {
                string type = "Laser Slave";
                string fbnumber = n.Attributes["fbnumber"].Value;
                string symbolic = n.Attributes["symbolic"].Value;
                string datatype = n.Attributes["datatype"].Value;
                string address = n.Attributes["address"].Value;
                string comment = n.Attributes["comment"].Value;
                string name = n.InnerText;

                if (address.Contains('Q'))
                    outputs.Add(new RobotTecnologie(fbnumber, name, type, symbolic, datatype, address, comment));
                else
                    inputs.Add(new RobotTecnologie(fbnumber, name, type, symbolic, datatype, address, comment));
            }

            robTecs.Add(outputs);
            robTecs.Add(inputs);

            return robTecs;
        }

        /// <summary>
        /// Convert given XMLDocument to RobSafeRangeMonitoring
        /// </summary>
        /// <param name="doc"></param>
        /// <returns>RobSafeRangeMonitoring</returns>
        private List<List<RobotSafeRangeMonitoring>> GenerateRobSafeRangeMonitoring(XmlDocument doc)
        {
            var robSafeRangeMonitoring = new List<List<RobotSafeRangeMonitoring>>();
            var inputs = new List<RobotSafeRangeMonitoring>();
            var outputs = new List<RobotSafeRangeMonitoring>();

            XmlNodeList nl = doc.SelectNodes("//Document/Robot/Default/Robsafe/Rangemonitoring/*");

            foreach (XmlNode n in nl)
            {
                string symbolic = n.Attributes["symbolic"].Value;
                string datatype = n.Attributes["datatype"].Value;
                string address = n.Attributes["address"].Value;
                string comment = n.Attributes["comment"].Value;

                if (address.Contains('Q'))
                    outputs.Add(new RobotSafeRangeMonitoring(symbolic, datatype, address, comment));
                else
                    inputs.Add(new RobotSafeRangeMonitoring(symbolic, datatype, address, comment));
            }

            robSafeRangeMonitoring.Add(outputs);
            robSafeRangeMonitoring.Add(inputs);

            return robSafeRangeMonitoring;
        }

        /// <summary>
        /// Convert given XMLDocument to RobSafeOperation
        /// </summary>
        /// <param name="doc"></param>
        /// <returns>RobSafeOperation</returns>
        private List<List<RobotSafeOperation>> GenerateRobSafeOperation(XmlDocument doc)
        {
            var robSafeOperation = new List<List<RobotSafeOperation>>();
            var inputs = new List<RobotSafeOperation>();
            var outputs = new List<RobotSafeOperation>();

            XmlNodeList nl = doc.SelectNodes("//Document/Robot/Default/Robsafe/Operation/*");

            foreach (XmlNode n in nl)
            {
                string symbolic = n.Attributes["symbolic"].Value;
                string datatype = n.Attributes["datatype"].Value;
                string address = n.Attributes["address"].Value;
                string comment = n.Attributes["comment"].Value;

                if (address.Contains('Q'))
                    outputs.Add(new RobotSafeOperation(symbolic, datatype, address, comment));
                else
                    inputs.Add(new RobotSafeOperation(symbolic, datatype, address, comment));
            }

            robSafeOperation.Add(outputs);
            robSafeOperation.Add(inputs);

            return robSafeOperation;
        }

        /// <summary>
        /// Convert given XMLDocument to RobInfo
        /// </summary>
        /// <param name="doc"></param>
        /// <returns>RobInfo</returns>
        private RobotInfo GenerateRobInfo(XmlDocument doc)
        {
            XmlNode nl = doc.SelectSingleNode("//Document/Robot");

            string name = nl.Attributes["name"].Value;
            string startaddress = nl.Attributes["startaddress"].Value;
            string robsafe = nl.Attributes["robsafe"].Value;
            string type = nl.Attributes["type"].Value;

            XmlNodeList nlTecs = doc.SelectNodes("//Document/Robot/Tecnologies/*");

            var tecs = new List<string>();
            foreach(XmlNode tec in nlTecs)
            {
                tecs.Add(tec.InnerText);
            }

            //robInfo[0] = Safe
            //robInfo[1] = Name
            //robInfo[2] = StartAddress
            //robInfo[3] = Tecnologies
            //robInfo[4] = Type
            RobotInfo robotInfo = new RobotInfo(name, robsafe, Convert.ToInt16(startaddress), string.Join(",", tecs), type);

            return robotInfo;
        }
        #endregion

        #region Initialize Windows
        /// <summary>
        /// Initialise Robot View Window
        /// </summary>
        private bool InitRobotView(RobotInfo robInfo, List<List<RobotBase>> robBase, List<List<RobotTecnologie>> robTecnologies, List<List<RobotSafeRangeMonitoring>> robSafeRangeMonitoring, List<List<RobotSafeOperation>> robSafeOperations)
        {
            bool changed = false;
            RobotViewOpened = true;
            object current = null;

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (_tiaPortal != null)
                {
                    PlcSoftware plcSoftware = (PlcSoftware)(argGroup as PlcBlockUserGroup).Parent.Parent;
                    if (plcSoftware != null)
                    {
                        current = plcSoftware.TagTableGroup;
                        if (current == null)
                            System.Windows.MessageBox.Show("\"PLC Tags\" group not found.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }

                if (String.IsNullOrEmpty(DefaultPlcDBPath))
                {
                    if (System.Windows.MessageBox.Show("PLC DB file path was not specified in settings.\nDo you want to add a path?", "Import", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        InitSettings();
                        return;
                    }
                }
                else
                {
                    if (!File.Exists(DefaultPlcDBPath))
                    {
                        System.Windows.MessageBox.Show("PLC Tags file does not exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        InitSettings();
                        return;
                    }
                    else
                    {
                        if (System.Windows.MessageBox.Show("PLC DB tags will be imported from file \"" + DefaultPlcDBPath + "\".\n\nDo you want to use this path?", "Import", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                        {
                            InitSettings();
                            return;
                        }
                    }
                }

                RobotView rv = new RobotView(this, robInfo, robBase, robTecnologies, robSafeRangeMonitoring, robSafeOperations)
                {
                    SavePath = Path.Combine(MainFolderPath, "Symbolics"),
                    Current = current,
                    PlcDBPath = DefaultPlcDBPath
                };
                rv.Closed += RobotView_Closed;
                rv.ShowDialog();
                changed = rv.Changes;
            }));
            return changed;
        }

        /// <summary>
        /// Initialize DBMaker Window
        /// </summary>
        /// <param name="matrixs"></param>
        /// <param name="sheetNames"></param>
        /// <param name="path"></param>
        private bool InitDbMaker(List<object[,]> matrixs, List<string> sheetNames, string path)
        {
            bool changed = false;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (_tiaPortal != null)
                {
                    if (argGroup == null)
                        System.Windows.MessageBox.Show("\"ARG\" group not found.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                if (String.IsNullOrEmpty(DefaultSequencePath))
                {
                    System.Windows.MessageBox.Show("Please specify sequence file path in settings.", "Import", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    if (!File.Exists(DefaultSequencePath))
                    {
                        System.Windows.MessageBox.Show("Sequence file does not exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        InitSettings();
                        return;
                    }
                    else
                    {
                        if (System.Windows.MessageBox.Show("Sequence file selected in settings: \"" + DefaultSequencePath + "\".\n\nDo you want to use this path?", "Import", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                        {
                            InitSettings();
                            return;
                        }
                    }
                }

                if (String.IsNullOrEmpty(DefaultSchnittstellePath))
                {
                    System.Windows.MessageBox.Show("Please specify schnittstelle excel path in settings.", "Import", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    if (System.Windows.MessageBox.Show("Schnittstelle excel file selected in settings: \"" + DefaultSchnittstellePath + "\".\n\nDo you want to use this path?", "Import", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        InitSettings();
                        return;
                    }
                }

                DBMaker dbMaker = new DBMaker(sheetNames, matrixs, path, MainFolderPath, _tiaPortal, _tiaPortalProject, argGroup);
                DBMaker.SequenceListPath = DefaultSequencePath;
                DBMaker.SchnittstelleListPath = DefaultSchnittstellePath;
                DBMaker.MatrixList += value => MatrixList = value;
                DBMaker.SheetNamesList += value => SheetNamesList = value;
                dbMaker.ShowDialog();
                changed = dbMaker.Changes;
            }));
            return changed;
        }

        /// <summary>
        /// Initialize PLC_Taps Window
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="sheetNames"></param>
        private bool InitPlcTaps(object[,] matrix, string path)
        {
            FoldersList = new ObservableCollection<FolderInfo>();
            MatrixList = new List<object[,]>();
            SheetNamesList = new List<string>();
            PLC_Taps plcTaps = null;
            object current = null;
            bool changed = false;

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (_tiaPortal != null)
                {
                    PlcSoftware plcSoftware = (PlcSoftware)(argGroup as PlcBlockUserGroup).Parent.Parent;
                    if (plcSoftware != null)
                    {
                        current = plcSoftware.TagTableGroup;
                        if (current == null)
                            System.Windows.MessageBox.Show("\"PLC Tags\" group not found.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }

                if (matrix != null)
                    plcTaps = new PLC_Taps(path, matrix, MainFolderPath, _tiaPortal, _tiaPortalProject, current);
                else
                    plcTaps = new PLC_Taps(path, MainFolderPath, _tiaPortal, _tiaPortalProject, current);

                plcTaps.MatrixList += value => MatrixList = value;
                plcTaps.SheetNamesList += value => SheetNamesList = value;
                plcTaps.PlcTagsMatrix += value => PlcTagsMatrix = value;

                plcTaps.ShowDialog();
                changed = plcTaps.Changes;
            }));

            return changed;
        }

        /// <summary>
        /// Initialize TreeView Manager Window
        /// </summary>
        /// <param name="matrixs"></param>
        /// <param name="sheetNames"></param>
        private bool InitTreeViewManager(List<object[,]> matrixs, List<string> sheetNames, string path)
        {
            MatrixList = new List<object[,]>();
            SheetNamesList = new List<string>();
            bool changed = false;

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (_tiaPortal != null)
                {
                    if (argGroup == null)
                        System.Windows.MessageBox.Show("\"ARG\" group not found.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                TreeViewManager tvm = new TreeViewManager(sheetNames, matrixs, path, MainFolderPath, _tiaPortal, _tiaPortalProject, argGroup);
                tvm.MatrixList += value => MatrixList = value;
                tvm.SheetNamesList += value => SheetNamesList = value;
                tvm.ShowDialog();
                changed = tvm.Changes;
            }));

            return changed;
        }

        /// <summary>
        /// Initialize Options Robot View Window
        /// </summary>
        /// <param name="RobBase"></param>
        /// <param name="RobSafeOperations"></param>
        /// <param name="RobSafeRangeMonitoring"></param>
        /// <param name="RobTecnologies"></param>
        private bool InitOptionsRobotView(List<RobotInfo> robsInfo)
        {
            RobotViewOpened = true;
            PlcTagTableSystemGroup tagTables = null;
            bool changed = false;

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (_tiaPortal != null)
                {
                    PlcSoftware plcSoftware = (PlcSoftware)(argGroup as PlcBlockUserGroup).Parent.Parent;
                    if (plcSoftware != null)
                    {
                        tagTables = plcSoftware.TagTableGroup;
                        if (tagTables == null)
                            System.Windows.MessageBox.Show("\"PLC Tags\" group not found.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }

                if (String.IsNullOrEmpty(DefaultPlcDBPath))
                {
                    if (System.Windows.MessageBox.Show("PLC DB excel path was not specified in settings.\nDo you want to add a path?", "Import", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        InitSettings();
                        return;
                    }
                }
                else
                {
                    if (!File.Exists(DefaultPlcDBPath))
                    {
                        System.Windows.MessageBox.Show("PLC DB file path does not exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        InitSettings();
                        return;
                    }
                    else
                    {
                        if (System.Windows.MessageBox.Show("PLC DB tags will be imported from file \"" + DefaultPlcDBPath + "\".\n\nDo you want to use this path?", "Import", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                        {
                            InitSettings();
                            return;
                        }
                    }
                }

                OptionsRobotView orv = new OptionsRobotView(this, robsInfo)
                {
                    SavePath = Path.Combine(MainFolderPath, "Symbolics"),
                    Current = tagTables,
                    PlcDBPath = DefaultPlcDBPath
                };
                orv.Closed += OptionsRobotView_Closed;
                orv.ShowDialog();
                changed = orv.Changes;
            }));

            return changed;
        }
        
        /// <summary>
        /// Initialize Settings Window
        /// </summary>
        private void InitSettings()
        {
            SettingsView view = new SettingsView(this);
            view.Closing += SettingsView_Closing;
            view.ShowDialog();
        }

        /// <summary>
        /// Initialize PLCRenamer Window
        /// </summary>
        private void InitPLCRenamer()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                var splitPath = _tiaPortalProject.Path.ToString().Split('\\');
                _projectName = splitPath[splitPath.Length - 1];
                _exportPath = Path.Combine(_defaultExportFolderPath, _projectName);

                PLCRenamer windowPLCRenamer = new PLCRenamer(_exportPath, _exportOptionsDefaults, _exportOptionsReadOnly, _tiaPortal, _tiaPortalProject, _mainFolderPath);

                windowPLCRenamer.Closed += PLCRenamerView_Closed;
                windowPLCRenamer.ShowDialog();
            }));
        }

        /// <summary>
        /// Initialize HardwareGenerator Window
        /// </summary>
        private void InitHardwareGenerator()
        {
            if (_defaultNetworkListPath == "" || _defaultEPlanPath == "")
            {
                System.Windows.MessageBox.Show("Please specify NetworkList path and EPlan path in Settings", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!File.Exists(_defaultNetworkListPath))
            {
                System.Windows.MessageBox.Show("NetworkList file path does not exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                InitSettings();
                return;
            }
            if (!File.Exists(_defaultEPlanPath))
            {
                System.Windows.MessageBox.Show("EPlan file path does not exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                InitSettings();
                return;
            }

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                HardwareGeneratorView windowGenerateHW = new HardwareGeneratorView(_defaultNetworkListPath, _defaultEPlanPath, _tiaPortal, _tiaPortalProject);
                windowGenerateHW.ShowDialog();
            }));
        }
        #endregion

        #region Windows Closed Events
        /// <summary>
        /// Handle SettingsView closed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Get old path values because button "Save" was not clicked
            ReadConfiguration();
        }

        /// <summary>
        /// Handle OptionsRobotView closed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OptionsRobotView_Closed(object sender, EventArgs e)
        {
            RobotViewOpened = false;
            IsLoading = false;
        }

        /// <summary>
        /// Handle RobotView closed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RobotView_Closed(object sender, EventArgs e)
        {
            RobotViewOpened = false;
            //LoadMainTreeView();
        }

        /// <summary>
        /// Handle PLCRenamerView closed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PLCRenamerView_Closed(object sender, EventArgs e)
        {
            if (!tiaPortalConnected)
                CloseTiaPortalConnection();
        }
        #endregion

        #endregion

        #region IDisposable
        /// <summary>The disposed</summary>
        /// TODO Edit XML Comment Template for disposed
        bool _disposed;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// TODO Edit XML Comment Template for Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        /// TODO Edit XML Comment Template for ~MainWindowViewModel
        ~MainWindowViewModel()
        {
            Dispose(false);
        }

        /// <summary>Releases unmanaged and - optionally - managed resources.</summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        /// TODO Edit XML Comment Template for Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // free other managed objects that implement
                // IDisposable only
                if (_tiaPortal != null)
                    _tiaPortal.Dispose();
            }

            // release any unmanaged objects
            // set the object references to null
            _tiaPortal = null;
            _tiaPortalProject = null;
            _tiaPortalProjects = null;
            _tiaGlobalLibrary = null;
            _plcsToCompile = null;

            _subWindow = null;

            _statusListView = null;
            _propertiesListView = null;
            _plcsToCompile = null;

            _disposed = true;
        }
        #endregion
    }
}

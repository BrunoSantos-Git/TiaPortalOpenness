using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CheckBox = System.Windows.Controls.CheckBox;
using TreeView = System.Windows.Controls.TreeView;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Siemens.Engineering;
using Siemens.Engineering.HW;
using Siemens.Engineering.SW;
using Siemens.Engineering.SW.Tags;
using TiaPortalOpennessDemo.Commands;
using TiaPortalOpennessDemo.Utilities;
using System.Xml;
using System.Xml.Linq;
using MessageBox = System.Windows.Forms.MessageBox;
using System.Windows.Forms.Integration;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using TiaOpennessHelper;
using Siemens.Engineering.SW.Blocks;
using TiaOpennessHelper.XMLParser;
using System.Net;
using System.Windows;
using System.Windows.Media;
using System.ComponentModel;
using System.Diagnostics;
using TiaPortalOpennessDemo.Views;
using Siemens.Engineering.Hmi.Cycle;
using System.Text;
using System.Xml.Schema;
using System.Web.UI.WebControls;

namespace TiaPortalOpennessDemo.ViewModels
{
    public class PLCRenamerViewModel : ViewModelBase
    {
        #region Textbox Values

        #region Old Indexes
        private string _oldIndex1;
        public string OldIndex1
        {
            get { return _oldIndex1; }
            set
            {
                if (!string.Equals(_oldIndex1, value) && NumberValidationTextBox(value))
                {
                    _oldIndex1 = value;
                    RaisePropertyChanged("OldIndex1");
                }
            }
        }

        private string _oldIndex2;
        public string OldIndex2
        {
            get { return _oldIndex2; }
            set
            {
                if (!string.Equals(_oldIndex2, value) && NumberValidationTextBox(value))
                {
                    _oldIndex2 = value;
                    RaisePropertyChanged("OldIndex2");
                }
            }
        }

        private string _oldIndex3;
        public string OldIndex3
        {
            get { return _oldIndex3; }
            set
            {
                if (!string.Equals(_oldIndex3, value) && NumberValidationTextBox(value))
                {
                    _oldIndex3 = value;
                    RaisePropertyChanged("OldIndex3");
                }
            }
        }

        private string _oldIndex4;
        public string OldIndex4
        {
            get { return _oldIndex4; }
            set
            {
                if (!string.Equals(_oldIndex4, value) && NumberValidationTextBox(value))
                {
                    _oldIndex4 = value;
                    RaisePropertyChanged("OldIndex4");
                }
            }
        }
        #endregion

        #region New Indexes
        private string _newIndex1;
        public string NewIndex1
        {
            get { return _newIndex1; }
            set
            {
                if (!string.Equals(_newIndex1, value) && NumberValidationTextBox(value))
                {
                    _newIndex1 = value;
                    RaisePropertyChanged("NewIndex1");
                }
            }
        }

        private string _newIndex2;
        public string NewIndex2
        {
            get { return _newIndex2; }
            set
            {
                if (!string.Equals(_newIndex2, value) && NumberValidationTextBox(value))
                {
                    _newIndex2 = value;
                    RaisePropertyChanged("NewIndex2");
                }
            }
        }

        private string _newIndex3;
        public string NewIndex3
        {
            get { return _newIndex3; }
            set
            {
                if (!string.Equals(_newIndex3, value) && NumberValidationTextBox(value))
                {
                    _newIndex3 = value;
                    RaisePropertyChanged("NewIndex3");
                }
            }
        }

        private string _newIndex4;
        public string NewIndex4
        {
            get { return _newIndex4; }
            set
            {
                if (!string.Equals(_newIndex4, value) && NumberValidationTextBox(value))
                {
                    _newIndex4 = value;
                    RaisePropertyChanged("NewIndex4");
                }
            }
        }
        #endregion

        #region IP's
        private string _ip1;
        public string IP1
        {
            get { return _ip1; }
            set
            {
                if (!string.Equals(_ip1, value) && NumberValidationTextBox(value))
                {
                    _ip1 = value;
                    RaisePropertyChanged("IP1");
                }
            }
        }

        private string _ip2;
        public string IP2
        {
            get { return _ip2; }
            set
            {
                if (!string.Equals(_ip2, value) && NumberValidationTextBox(value))
                {
                    _ip2 = value;
                    RaisePropertyChanged("IP2");
                }
            }
        }

        private string _ip3;
        public string IP3
        {
            get { return _ip3; }
            set
            {
                if (!string.Equals(_ip3, value) && NumberValidationTextBox(value))
                {
                    _ip3 = value;
                    RaisePropertyChanged("IP3");
                }
            }
        }
        #endregion

        #endregion
        
        #region CheckBox Values
        private string _cbOldIndex;
        public string CbOldIndex
        {
            get { return _cbOldIndex; }
            set
            {
                if (!string.Equals(_cbOldIndex, value))
                {
                    _cbOldIndex = value;
                    cbOldIndexSelection(_cbOldIndex);
                    RaisePropertyChanged("CbOldIndex");
                }
            }
        }

        private string _cbNewIndex;
        public string CbNewIndex
        {
            get { return _cbNewIndex; }
            set
            {
                if (!string.Equals(_cbNewIndex, value))
                {
                    _cbNewIndex = value;
                    cbNewIndexSelection(_cbNewIndex);
                    RaisePropertyChanged("CbNewIndex");
                }
            }
        }
        #endregion

        private TreeViewHandler _projectTree;
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

        private ObservableCollection<string> _statusListView;
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

        private DataGridView _dataGridView;
        public DataGridView vDataGridView
        {
            get { return _dataGridView; }
            private set
            {
                if (_dataGridView == value)
                {
                    return;
                }
                _dataGridView = value;
                RaisePropertyChanged("vDataGridView");
            }
        }

        private bool _changeFolgenIsChecked;
        public bool ChangeFolgenIsChecked
        {
            get { return _changeFolgenIsChecked;  }
            set
            {
                if (_changeFolgenIsChecked == value)
                    return;
                _changeFolgenIsChecked = value;
                RaisePropertyChanged("ChangeFolgenIsChecked");
            }
        }

        private bool _changeIpIsChecked;
        public bool ChangeIpIsChecked
        {
            get { return _changeIpIsChecked; }
            set
            {
                if (_changeIpIsChecked == value)
                    return;
                _changeIpIsChecked = value;
                RaisePropertyChanged("ChangeIpIsChecked");
            }
        }

        private bool _logExist;
        public bool LogExist
        {
            get { return _logExist; }
            set
            {
                if (_logExist == value)
                    return;
                _logExist = value;
                RaisePropertyChanged("LogExist");
            }
        }

        private bool _canRename;
        public bool CanRename
        {
            get { return _canRename; }
            set
            {
                if (_canRename == value)
                    return;
                _canRename = value;
                RaisePropertyChanged("CanRename");
            }
        }
        
        public WindowsFormsHost WindowsFormsGrid
        {
            get { return new WindowsFormsHost() { Child = _dataGridView }; }
        }

        private readonly string exportPath;
        private readonly string mainFolderPath;
        private readonly string logPath;
        private readonly bool exportOptionsDefaults;
        private readonly bool exportOptionsReadOnly;
        private readonly TiaPortal tiaPortal;
        private readonly Project tiaPortalProject;

        private object current;
        private bool onlyChangeIP;
        private static bool exportArg;
        private static bool exportAllTags;
        private static bool renameDevices;

        private List<string> blocksImportFail;
        private List<string> deviceChangeIpFail;
        private List<string> unnamedDevices;
        private List<string> plcTagsToImport;
        private List<string> inconsistentBlocks;
        private static List<string> blocksToExport;
        private static List<string> tagsToExport;

        public CommandBase RenameCommand { get; set; }
        public CommandBase OpenLogCommand { get; set; }
        public CommandBase ImportConfigCommand { get; set; }
        public CommandBase ExportConfigCommand { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public PLCRenamerViewModel()
        {
            CreateGrid();
            Initialize();
        }

        /// <summary>
        /// Constructor w/Parameters
        /// </summary>
        /// <param name="exportPath"></param>
        /// <param name="exportOptionsDefaults"></param>
        /// <param name="exportOptionsReadOnly"></param>
        /// <param name="tiaPortal"></param>
        /// <param name="tiaPortalProject"></param>
        /// <param name="mainFolderPath"></param>
        public PLCRenamerViewModel(string exportPath, bool exportOptionsDefaults, bool exportOptionsReadOnly, TiaPortal tiaPortal, Project tiaPortalProject, string mainFolderPath)
        {
            this.exportPath = exportPath;
            this.exportOptionsDefaults = exportOptionsDefaults;
            this.exportOptionsReadOnly = exportOptionsReadOnly;
            this.tiaPortal = tiaPortal;
            this.tiaPortalProject = tiaPortalProject;
            this.mainFolderPath = mainFolderPath;
            logPath = Path.Combine(mainFolderPath, "Logs", tiaPortalProject.Name + ".txt");
            CreateGrid();
            Initialize();
            GeneratePLCRenamerTree();
        }

        /// <summary>
        /// Initialize commands
        /// </summary>
        private void Initialize()
        {
            onlyChangeIP = false;
            ChangeFolgenIsChecked = false;
            ChangeIpIsChecked = false;
            exportArg = false;
            exportAllTags = false;
            renameDevices = false;
            CanRename = true;
            LogExist = File.Exists(logPath);

            blocksToExport = new List<string>();
            tagsToExport = new List<string>();
            unnamedDevices = new List<string>();
            blocksImportFail = new List<string>();
            plcTagsToImport = new List<string>();
            deviceChangeIpFail = new List<string>();
            inconsistentBlocks = new List<string>();

            StatusListView = new ObservableCollection<string>();
            RenameCommand = new CommandBase(RenameCommand_Executed);
            OpenLogCommand = new CommandBase(OpenLogCommand_Executed);
            ImportConfigCommand = new CommandBase(ImportConfigCommand_Executed);
            ExportConfigCommand = new CommandBase(ExportConfigCommand_Executed);
            ProjectTree = new TreeViewHandler(new RelayCommand<DependencyPropertyEventArgs>(TreeViewItemSelectedChangedCallback));
        }

        /// <summary>
        /// Generate the PLCRenamer Tree
        /// </summary>
        private void GeneratePLCRenamerTree()
        {
            var splitPath = tiaPortalProject.Path.ToString().Split('\\');
            string projectName = splitPath[splitPath.Length - 1];

            TreeViewItem plcSoftware = new TreeViewItem();
            var projectTreeView = new TreeView();
            var projectTreeViewItem = new TreeViewItem();
            projectTreeViewItem.Header = projectName;
            projectTreeViewItem.Tag = tiaPortalProject;
            projectTreeViewItem.ExpandSubtree();

            foreach (var device in tiaPortalProject.Devices)
            {
                plcSoftware = CreateDeviceTreeViewItem(device);
                if (plcSoftware.Tag is PlcSoftware)
                {
                    TreeViewItemsCrawler(projectTreeViewItem, plcSoftware);
                    //projectTreeViewItem.Items.Add(plcSoftware);
                    break;
                }
            }

            TreeViewItem hw = new TreeViewItem
            {
                Tag = "hw",
                Header = new CheckBox()
                {
                    Content = new TextBlock()
                    {
                        Text = "Rename Hardware"
                    },
                    Tag = "hw"
                }
            };

            if (plcSoftware == null || !(plcSoftware.Tag is PlcSoftware))
            {
                MessageBox.Show("Tia Portal Project does not have a PLCSoftware.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (current == null)
            {
                MessageBox.Show("\"ARG\" folder not found in project.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CheckConsistency(current as IEngineeringCompositionOrObject);
            if (inconsistentBlocks.Any())
            {
                MessageBox.Show(new Form() { TopMost = true }, "Some blocks inside ARG group are not consistent.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DisableIncosistentGroups(projectTreeViewItem);
            }

            projectTreeView.Items.Add(projectTreeViewItem);
            projectTreeView.Items.Add(hw);

            ProjectTree.Refresh(projectTreeView);
        }

        /// <summary>Crawls through Tia Portal folders</summary>
        /// <param name="projectTreeViewItem">The project TreeView item.</param>
        /// <param name="folder">The folder.</param>
        /// TODO Edit XML Comment Template for FolderCrawler
        private void TreeViewItemsCrawler(TreeViewItem projectTreeViewItem, TreeViewItem subProjectTreeViewItem)
        {
            foreach (TreeViewItem i in subProjectTreeViewItem.Items)
            {
                if ((i.Tag is PlcBlockUserGroup && (((TreeViewItem)i.Parent).Header.ToString() == "ARG" || i.Header.ToString() == "ARG") ||
                    i.Tag is PlcTagTableSystemGroup || i.Tag is PlcTagTable || i.Tag is PlcBlockSystemGroup) && !i.Header.ToString().Contains("DB"))
                {
                    string cbTag = "normal";

                    if (i.Tag is PlcTagTable)
                        cbTag = "plctag";

                    if (i.Tag is PlcBlockUserGroup)
                        cbTag = "block";

                    var newItem = new TreeViewItem
                    {
                        Tag = i,
                        Header = new CheckBox()
                        {
                            Content = new TextBlock()
                            {
                                Text = i.Header.ToString()
                            },
                            Tag = cbTag
                        }
                    };

                    ((CheckBox)newItem.Header).Checked += checkBox_OnCheck;
                    ((CheckBox)newItem.Header).Unchecked += checkBox_Uncheck;

                    if (i.Header.Equals("ARG"))
                    {
                        current = i.Tag;
                    }

                    if (!i.Header.ToString().ToLower().Equals("program blocks"))
                    {
                        projectTreeViewItem.Items.Add(newItem);

                        if (i.Items.Count > 0)
                            TreeViewItemsCrawler(newItem, i);
                    }
                    else
                        TreeViewItemsCrawler(projectTreeViewItem, i);
                }
            } 
        }

        /// <summary>
        /// Create datagridview
        /// </summary>
        private void CreateGrid()
        {
            vDataGridView = new DataGridView
            {
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            };
            vDataGridView.Columns.Add("Old Name", "Old Name");
            vDataGridView.Columns.Add("New Name", "New Name");

            DataGridViewColumn oldName = vDataGridView.Columns[0];
            DataGridViewColumn newName = vDataGridView.Columns[1];
            oldName.MinimumWidth = 6;
            newName.MinimumWidth = 6;
        }

        /// <summary>
        /// Rename PLC
        /// </summary>
        private void RenamePLC()
        {
            var parent = (current as PlcBlockUserGroup).Parent.Parent;
            var groups = (current as PlcBlockUserGroup).Groups;

            #region Change Devices IP
            if (_changeIpIsChecked)
            {
                using (var access = tiaPortal.ExclusiveAccess("Changing Devices IP"))
                {
                    ChangeDeviceIP();
                }
            }
            #endregion

            if (onlyChangeIP) return;

            #region Export Elements

            #region Export Blocks
            if (blocksToExport.Any())   // If has blocks to export
            {
                using (var access = tiaPortal.ExclusiveAccess("Exporting Blocks"))
                {
                    if (exportArg) // If ALL ARG Folders Items are selected
                    {
                        if (exportOptionsDefaults && exportOptionsReadOnly)
                            OpennessHelper.ExportStructure(current as IEngineeringCompositionOrObject, ExportOptions.WithDefaults | ExportOptions.WithReadOnly, exportPath);
                        else if (exportOptionsDefaults)
                            OpennessHelper.ExportStructure(current as IEngineeringCompositionOrObject, ExportOptions.WithDefaults, exportPath);
                        else if (exportOptionsReadOnly)
                            OpennessHelper.ExportStructure(current as IEngineeringCompositionOrObject, ExportOptions.WithReadOnly, exportPath);
                        else
                            OpennessHelper.ExportStructure(current as IEngineeringCompositionOrObject, ExportOptions.None, exportPath);

                        WriteStatusEntry("ARG group exported");
                    }
                    else // If SOME ARG Folder Items are selected
                    {
                        string newPath = Path.Combine(exportPath, "ARG");
                        foreach (var item in groups)
                        {
                            var groupName = (item as IEngineeringObject).GetAttribute("Name").ToString();
                            if (blocksToExport.Contains(groupName))
                            {
                                if (exportOptionsDefaults && exportOptionsReadOnly)
                                    OpennessHelper.ExportStructure(item as IEngineeringCompositionOrObject, ExportOptions.WithDefaults | ExportOptions.WithReadOnly, newPath);
                                else if (exportOptionsDefaults)
                                    OpennessHelper.ExportStructure(item as IEngineeringCompositionOrObject, ExportOptions.WithDefaults, newPath);
                                else if (exportOptionsReadOnly)
                                    OpennessHelper.ExportStructure(item as IEngineeringCompositionOrObject, ExportOptions.WithReadOnly, newPath);
                                else
                                    OpennessHelper.ExportStructure(item as IEngineeringCompositionOrObject, ExportOptions.None, newPath);

                                WriteStatusEntry("\"" + groupName + "\" group exported");
                            }
                        }
                    }
                }
            }
            #endregion

            #region Export PLC Tags
            var tagTables = (parent as PlcSoftware).TagTableGroup.TagTables;
            string plcTagsPath = Path.Combine(exportPath, "PLC Tags");
            if (tagsToExport.Any())
            {
                using (var access = tiaPortal.ExclusiveAccess("Exporting PLC Tags"))
                {
                    ExportAllTagTables(parent as PlcSoftware, plcTagsPath);
                }

                if (exportAllTags)
                    WriteStatusEntry("PLCTags exported");
            }
            #endregion

            #endregion

            #region Delete Elements (TIA PORTAL)
            if (blocksToExport.Any() || tagsToExport.Any())
            {
                using (var access = tiaPortal.ExclusiveAccess("Deleting elements"))
                {
                    #region Delete TIAPortal folders and recreate
                    if (blocksToExport.Any())
                    {
                        for (int i = groups.Count - 1; i >= 0; i--)
                        {
                            var lSubGroups = new List<string>();
                            var groupName = (groups[i] as IEngineeringObject).GetAttribute("Name").ToString();

                            bool validation;

                            if (!exportArg) //If is to export all elements on ARG Folder
                                validation = groups[i] is PlcBlockUserGroup && groupName != "2_Safety" && blocksToExport.Contains(groupName); //Check if blocksToExport contains groupName
                            else
                                validation = groups[i] is PlcBlockUserGroup && groupName != "2_Safety"; //Else do not check if blocksToExport has groupName

                            if (validation)
                            {
                                if (groupName == "110_ProDiag")
                                {
                                    var blocks = (groups[i] as PlcBlockUserGroup).Blocks;
                                    for (int x = blocks.Count - 1; x >= 0; x--)
                                    {
                                        string name = GetKeyThatContains(blocks[x].Name, GetNames());

                                        if (name != "")
                                        {
                                            WriteStatusEntry("Block \"" + blocks[x].Name + "\" deleted");
                                            (blocks[x] as IEngineeringObject)?.Invoke("Delete", new Dictionary<Type, object>());
                                        }
                                    }
                                }
                                else
                                {
                                    var subGroups = (groups[i] as PlcBlockUserGroup).Groups;
                                    for (int x = subGroups.Count - 1; x >= 0; x--)
                                    {
                                        var subGroupName = (subGroups[x] as IEngineeringObject).GetAttribute("Name").ToString();
                                        lSubGroups.Add(subGroupName);

                                        // Delete SubFolder
                                        WriteStatusEntry("Group \"" + (groups[i] as PlcBlockUserGroup).Name + "\" -> \"" + (subGroups[x] as PlcBlockUserGroup).Name + "\" deleted");
                                        (subGroups[x] as PlcBlockUserGroup).Delete();
                                    }

                                    // Delete Group Folder
                                    WriteStatusEntry("Group \"" + (groups[i] as PlcBlockUserGroup).Name + "\" deleted");
                                    (groups[i] as PlcBlockUserGroup).Delete();

                                    // Create Folders and Subfolders Again
                                    (current as PlcBlockUserGroup).Groups.Create(groupName);
                                    WriteStatusEntry("Group \"" + groupName + "\" created");

                                    PlcBlockUserGroup createdGroup = (current as PlcBlockUserGroup).Groups.Find(groupName);

                                    if (lSubGroups.Any())
                                    {
                                        foreach (string sub in lSubGroups)
                                        {
                                            createdGroup.Groups.Create(sub);
                                            WriteStatusEntry("Group \"" + groupName + "\" -> \"" + sub + "\" created");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region Delete PLC Tags
                    if (tagsToExport.Any())
                    {
                        for (int i = tagTables.Count - 1; i >= 0; i--)
                        {
                            string tagName = (tagTables[i] as PlcTagTable).Name;
                            try
                            {
                                if (exportAllTags)
                                {
                                    (tagTables[i] as PlcTagTable).Delete();
                                    WriteStatusEntry("Tag table \"" + tagName + "\" deleted");
                                }
                                else
                                {
                                    if (tagsToExport.Contains(tagTables[i].Name))
                                    {
                                        (tagTables[i] as PlcTagTable).Delete();
                                        WriteStatusEntry("Tag table \"" + tagName + "\" deleted");
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                WriteStatusEntry("Error deleting \"" + tagName + "\" tag table");
                                continue;
                            }
                        }
                    }
                    #endregion
                }
            }
            #endregion

            #region Edit XML
            DirectoryInfo plcTagsDirectory = new DirectoryInfo(plcTagsPath);

            if (!IsDirectoryEmpty(exportPath))
            {
                var folderName = OpennessHelper.GetObjectName(current as IEngineeringCompositionOrObject);
                var newPath = Path.Combine(exportPath, folderName);
                DirectoryInfo blocksDirectory = new DirectoryInfo(newPath);

                using (var access = tiaPortal.ExclusiveAccess("Editing blocks XML"))
                {
                    #region Blocks XML
                    bool safety = false;
                    if (Directory.Exists(newPath) && !IsDirectoryEmpty(newPath))
                    {
                        foreach (var folder in blocksDirectory.GetDirectories())
                        {
                            safety = folder.Name.Equals("2_Safety");

                            foreach (var fileName in folder.GetFiles())
                            {
                                XmlDocument xml = ReplaceXML(fileName.FullName, safety, "block");

                                string name = XmlParser.GetXmlNameAttribute(xml);
                                string newName = XmlParser.RemoveWindowsUnallowedChars(name);
                                string path = Path.Combine(newPath, folder.Name, newName);

                                File.Delete(fileName.FullName);
                                xml.Save(path);
                            }

                            foreach (var subFolder in folder.GetDirectories())
                            {
                                foreach (var subFile in subFolder.GetFiles())
                                {
                                    XmlDocument xml = ReplaceXML(subFile.FullName, safety, "block");

                                    string name = XmlParser.GetXmlNameAttribute(xml);
                                    string newName = XmlParser.RemoveWindowsUnallowedChars(name);
                                    string DBfolderName = subFolder.Name;
                                    string path = Path.Combine(newPath, folder.Name, DBfolderName, newName);

                                    File.Delete(subFile.FullName);
                                    xml.Save(path);
                                }
                            }
                        }
                    }
                    #endregion

                    #region PLCTags XML
                    if (Directory.Exists(plcTagsPath) && !IsDirectoryEmpty(plcTagsPath))
                    {
                        foreach (var tag in plcTagsDirectory.GetFiles())
                        {
                            XmlDocument xml = ReplaceXML(tag.FullName, false, "tag");

                            string name = XmlParser.GetXmlNameAttribute(xml);
                            string newName = XmlParser.RemoveWindowsUnallowedChars(name);
                            string path = Path.Combine(plcTagsPath, newName);

                            File.Delete(tag.FullName);
                            xml.Save(path);
                        }
                    }
                    #endregion
                }
            }
            #endregion

            #region Rename Devices
            if (renameDevices)
            {
                using (var access = tiaPortal.ExclusiveAccess("Renaming Devices"))
                {
                    string plcName = String.Copy((parent as PlcSoftware).Name);
                    try
                    {
                        var names = GetNames();
                        string name = GetKeyThatContains(plcName, names);
                        if (name != "")
                        {
                            string newName = (parent as PlcSoftware).Name.Replace(name, names.First(n => n.Key == name).Value);
                            (parent as PlcSoftware).SetAttributes(new List<KeyValuePair<string, object>>() { new KeyValuePair<string, object>("Name", newName) });
                            WriteStatusEntry("PLC name changed (\"" + plcName + "\" to \"" + newName + "\")");
                        }
                    }
                    catch (Exception)
                    {
                        WriteStatusEntry("Error changing PLC name");
                    }
                    GetDeviceGroups();
                    GetDevices();
                }
            }
            #endregion

            #region Import Element
            if (!IsDirectoryEmpty(exportPath))
            {
                using (var access = tiaPortal.ExclusiveAccess("Importing elements"))
                {
                    var blocksDirectory = new DirectoryInfo(Path.Combine(exportPath, OpennessHelper.GetObjectName(current as IEngineeringCompositionOrObject)));

                    #region Import Blocks
                    if (blocksDirectory.Exists && !IsDirectoryEmpty(blocksDirectory.FullName))
                    {
                        var dataBases = new List<List<string>>();
                        foreach (var folder in blocksDirectory.GetDirectories())
                        {
                            if (folder.Name == "110_ProDiag") continue;

                            var group = (current as PlcBlockUserGroup).Groups.Find(folder.Name);
                            foreach (var file in folder.GetFiles())
                            {
                                string fileName = Path.GetFileNameWithoutExtension(file.FullName);
                                if (fileName.Equals("ARG_") || fileName.Equals("FOB_RTG1")) continue;

                                if (group.Name != null)
                                {
                                    try
                                    {
                                        if (XmlParser.IsDB(file.FullName))
                                            dataBases.Add(new List<string>() { file.FullName, group.Name });
                                        else
                                        {
                                            OpennessHelper.ImportItem(group, file.FullName, ImportOptions.Override);
                                            WriteStatusEntry("Block \"" + fileName + "\" imported");
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        blocksImportFail.Add(file.FullName);
                                        WriteStatusEntry("Block \"" + file.FullName + "\" import failed");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(new Form() { TopMost = true }, "No element called " + group.Name + " found in TIA Project.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    WriteStatusEntry("No element called " + group.Name + " found in TIA Project");
                                    break;
                                }
                            }

                            foreach (var subFolder in folder.GetDirectories())
                            {
                                var subGroup = group.Groups.Find(subFolder.Name);
                                foreach (var subFile in subFolder.GetFiles())
                                {
                                    string subFileName = Path.GetFileNameWithoutExtension(subFile.FullName);

                                    if (subFileName.Equals("F_AblGr_DB")) continue;

                                    if (subGroup.Name != null)
                                    {
                                        try
                                        {
                                            OpennessHelper.ImportItem(subGroup, subFile.FullName, ImportOptions.Override);
                                            WriteStatusEntry("Block \"" + subFileName + "\" imported");
                                        }
                                        catch (Exception)
                                        {
                                            blocksImportFail.Add(subFile.FullName);
                                            WriteStatusEntry("Block \"" + subFile.FullName + "\" import failed");
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show(new Form() { TopMost = true }, "No element called " + subGroup.Name + " found in TIA Project.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        WriteStatusEntry("No element called " + subGroup.Name + " found in TIA Project");
                                        break;
                                    }
                                }
                            }
                        }

                        // Databases found outside DB folders
                        // have to be imported in the end
                        foreach (var db in dataBases)
                        {
                            string dbName = Path.GetFileNameWithoutExtension(db[0]);
                            try
                            {
                                OpennessHelper.ImportItem((current as PlcBlockUserGroup).Groups.Find(db[1]), db[0], ImportOptions.Override);
                                WriteStatusEntry("Block \"" + dbName + "\" imported");
                            }
                            catch (Exception)
                            {
                                blocksImportFail.Add(db[0]);
                                WriteStatusEntry("Block \"" + db[0] + "\" import failed");
                            }
                        }
                    }
                    #endregion

                    #region Import PLC Tags
                    if (Directory.Exists(plcTagsPath) && !IsDirectoryEmpty(plcTagsPath))
                    {
                        foreach (var tag in plcTagsDirectory.GetFiles())
                        {
                            string tagTableName = Path.GetFileNameWithoutExtension(tag.FullName);
                            try
                            {
                                tagTables.Import(new FileInfo(tag.FullName), ImportOptions.Override);
                                WriteStatusEntry("Tag table \"" + tagTableName + "\" imported");
                            }
                            catch (Exception)
                            {
                                XmlDocument doc = new XmlDocument();
                                doc.Load(tag.FullName);
                                string newTagTableName = XmlParser.GetXmlNameAttribute(doc);
                                string TagTableNameNoExt = newTagTableName.Replace(".xml", "");
                                if (tagTables.Find(TagTableNameNoExt) == null)
                                {
                                    tagTables.Create(TagTableNameNoExt);
                                    WriteStatusEntry("Tag table \"" + TagTableNameNoExt + "\" created");
                                }

                                plcTagsToImport.Add(TagTableNameNoExt);
                                WriteStatusEntry("Error importing tag table \"" + TagTableNameNoExt + "\"");
                                doc = FailImportXmlDoc(tag.FullName);
                                Directory.CreateDirectory(tag.Directory.ToString() + "/To Import Manually");
                                doc.Save(Path.Combine(tag.Directory.ToString(), "To Import Manually", newTagTableName));
                            }
                        }
                    }
                    #endregion
                }
            }
            #endregion
        }

        /// <summary>
        /// Get device groups from TiaPortalProject and call function "RenameDevices" 
        /// to change all device names found.
        /// </summary>
        private void GetDeviceGroups()
        {
            var names = GetNames();
            foreach (DeviceUserGroup deviceUserGroup in tiaPortalProject.DeviceGroups)
            {
                RenameDevices(deviceUserGroup, names);
            }
        }

        /// <summary>
        /// Get devices from TiaPortal
        /// </summary>
        private void GetDevices()
        {
            var names = GetNames();
            foreach (Device device in tiaPortalProject.Devices)
            {
                string name = GetKeyThatContains(device.Name, names);
                if (name != "")
                {
                    string newName = device.Name.Replace(name, names.First(n => n.Key == name).Value);
                    //device.SetAttributes(new List<KeyValuePair<string, object>>() { new KeyValuePair<string, object>("Name", newName) });
                    device.SetAttribute("Name", newName);
                }
            }
        }

        /// <summary>
        /// Rename all devices found in a deviceUserGroup
        /// </summary>
        /// <param name="deviceUserGroup"></param>
        /// <param name="names"></param>
        private void RenameDevices(DeviceUserGroup deviceUserGroup, List<KeyValuePair<string, string>> names)
        {
            string deviceUGroupName = String.Copy(deviceUserGroup.Name);
            string name = GetKeyThatContains(deviceUGroupName, names);
            if (name != "")
            {
                string newName = deviceUserGroup.Name.Replace(name, names.First(n => n.Key == name).Value);
                deviceUserGroup.SetAttributes(new List<KeyValuePair<string, object>>() { new KeyValuePair<string, object>("Name", newName) });
                WriteStatusEntry("Device name changed (\"" + deviceUGroupName + "\" to \"" + newName + "\")");
            }

            foreach (var device in deviceUserGroup.Devices)
            {
                string deviceName = String.Copy(device.Name);
                name = GetKeyThatContains(deviceName, names);
                if (name != "")
                {
                    string newName = device.Name.Replace(name, names.First(n => n.Key == name).Value);
                    device.SetAttribute("Name", newName);
                    WriteStatusEntry("Device name changed (\"" + deviceName + "\" to \"" + newName + "\")");
                }

                foreach (var item in device.DeviceItems)
                {
                    string deviceItemName = String.Copy(item.Name);
                    name = GetKeyThatContains(deviceItemName, names);
                    if (name != "")
                    {
                        try
                        {
                            string newName = item.Name.Replace(name, names.First(n => n.Key == name).Value);
                            item.SetAttribute("Name", newName);
                            WriteStatusEntry("Device name changed (\"" + deviceItemName + "\" to \"" + newName + "\")");
                        }
                        catch (Exception)
                        {
                            unnamedDevices.Add(deviceItemName);
                            WriteStatusEntry("Error changing " + item.Name + " name");
                        }
                    }
                }
            }

            foreach (var subDeviceUserGroup in deviceUserGroup.Groups)
            {
                RenameDevices(subDeviceUserGroup, names);
            }
        }

        /// <summary>
        /// Replace XML with new text
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="safetyFolder"></param>
        /// <param name="type"></param>
        /// <returns>XmlDocument with new names</returns>
        private XmlDocument ReplaceXML(string fileName, bool safetyFolder, string type)
        {
            XmlDocument doc = new XmlDocument();
            List<string> xmlReplacedStrings = new List<string>();
            var names = GetNames();
            doc.Load(fileName);

            List<int> indexes = new List<int>();
            string nameTag = doc.SelectSingleNode("//Name").InnerText;
            bool hasChar = false;

            if (nameTag.Contains("_"))  //If name contains '_'
            {
                for (int i = nameTag.IndexOf('_'); i > -1; i = nameTag.IndexOf('_', i + 1)) //Get all indexes where string contains '_'
                {
                    indexes.Add(i);
                }
                nameTag = nameTag.Replace("_", ""); //Remove it from string
                hasChar = true;
            }

            foreach (var name in names)
            {
                if (doc.InnerXml.Contains(name.Key))
                {
                    doc.InnerXml = Regex.Replace(doc.InnerXml, name.Key, name.Value, RegexOptions.IgnoreCase); //Replace innerxml ignoring case
                    xmlReplacedStrings.Add("\"" + name.Key + "\" to \"" + name.Value + "\"");
                }
            }

            if (hasChar) //If string has char
            {
                foreach (var index in indexes)
                {
                    nameTag = nameTag.Insert(index, "_"); //Insert the "_" again
                }
            }

            #region CHANGE FOLGEN NUMBERS
            if (_changeFolgenIsChecked)
            {
                string oldIndex1 = _oldIndex1;
                string oldIndex2 = _oldIndex2;
                string oldIndex3 = _oldIndex3;
                string oldIndex4 = _oldIndex4;
                string newIndex1 = _newIndex1;
                string newIndex2 = _newIndex2;
                string newIndex3 = _newIndex3;
                string newIndex4 = _newIndex4;

                XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);

                ns.AddNamespace("ns2", "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v3");
                ns.AddNamespace("ns3", "http://www.siemens.com/automation/Openness/SW/Interface/v3");

                #region Change FrgFolge
                XmlNodeList elemListArray = doc.SelectNodes("//ns2:Component[@Name='InFolge']/ns2:Access/ns2:Constant/ns2:ConstantValue", ns);
                XmlNodeList FrgFolge1Arr = doc.SelectNodes("//ns2:Component[@Name='FrgFolge" + oldIndex1 + "']", ns);
                XmlNodeList FrgFolge2Arr = doc.SelectNodes("//ns2:Component[@Name='FrgFolge" + oldIndex2 + "']", ns);
                XmlNodeList FrgFolge3Arr = doc.SelectNodes("//ns2:Component[@Name='FrgFolge" + oldIndex3 + "']", ns);
                XmlNodeList FrgFolge4Arr = doc.SelectNodes("//ns2:Component[@Name='FrgFolge" + oldIndex4 + "']", ns);

                foreach (XmlNode FrgFolge in FrgFolge1Arr)
                {
                    FrgFolge.Attributes[0].Value = "FrgFolge" + newIndex1;

                    XmlNode frgByte = FrgFolge.ParentNode.ParentNode.NextSibling.SelectSingleNode("ns2:Constant/ns2:ConstantValue", ns);
                    if (frgByte != null)
                        frgByte.InnerText = newIndex1;
                }
                foreach (XmlNode FrgFolge in FrgFolge2Arr)
                {
                    FrgFolge.Attributes[0].Value = "FrgFolge" + newIndex2;

                    XmlNode frgByte = FrgFolge.ParentNode.ParentNode.NextSibling.SelectSingleNode("ns2:Constant/ns2:ConstantValue", ns);
                    if (frgByte != null)
                        frgByte.InnerText = newIndex2;
                }
                foreach (XmlNode FrgFolge in FrgFolge3Arr)
                {
                    FrgFolge.Attributes[0].Value = "FrgFolge" + newIndex3;

                    XmlNode frgByte = FrgFolge.ParentNode.ParentNode.NextSibling.SelectSingleNode("ns2:Constant/ns2:ConstantValue", ns);
                    if (frgByte != null)
                        frgByte.InnerText = newIndex3;
                }
                foreach (XmlNode FrgFolge in FrgFolge4Arr)
                {
                    FrgFolge.Attributes[0].Value = "FrgFolge" + newIndex4;

                    XmlNode frgByte = FrgFolge.ParentNode.ParentNode.NextSibling.SelectSingleNode("ns2:Constant/ns2:ConstantValue", ns);
                    if (frgByte != null)
                        frgByte.InnerText = newIndex4;
                }

                XmlNode FrgFolge1 = doc.SelectSingleNode("//ns3:Member[@Name='FrgFolge" + oldIndex1 + "']", ns);
                XmlNode FrgFolge2 = doc.SelectSingleNode("//ns3:Member[@Name='FrgFolge" + oldIndex2 + "']", ns);
                XmlNode FrgFolge3 = doc.SelectSingleNode("//ns3:Member[@Name='FrgFolge" + oldIndex3 + "']", ns);
                XmlNode FrgFolge4 = doc.SelectSingleNode("//ns3:Member[@Name='FrgFolge" + oldIndex4 + "']", ns);

                if (FrgFolge1 != null && FrgFolge2 != null && FrgFolge3 != null && FrgFolge4 != null)
                {
                    FrgFolge1.Attributes[0].Value = "FrgFolge" + newIndex1;
                    FrgFolge2.Attributes[0].Value = "FrgFolge" + newIndex2;
                    FrgFolge3.Attributes[0].Value = "FrgFolge" + newIndex3;
                    FrgFolge4.Attributes[0].Value = "FrgFolge" + newIndex4;
                }

                foreach (XmlNode el in elemListArray)
                {
                    if (el.InnerText == oldIndex1)
                    {
                        WriteStatusEntry("Changed \"InFolge[" + oldIndex1 + "]\" to \"InFolge[" + newIndex1 + "\"] on " + type + " \"" + nameTag + "\"");
                        el.InnerText = newIndex1;
                    }
                    if (el.InnerText == oldIndex2)
                    {
                        WriteStatusEntry("Changed \"InFolge[" + oldIndex2 + "]\" to \"InFolge[" + newIndex2 + "\"] on " + type + " \"" + nameTag + "\"");
                        el.InnerText = newIndex2;
                    }
                    if (el.InnerText == oldIndex3)
                    {
                        WriteStatusEntry("Changed \"InFolge[" + oldIndex3 + "]\" to \"InFolge[" + newIndex3 + "\"] on " + type + " \"" + nameTag + "\"");
                        el.InnerText = newIndex3;
                    }
                    if (el.InnerText == oldIndex4)
                    {
                        WriteStatusEntry("Changed \"InFolge[" + oldIndex4 + "]\" to \"InFolge[" + newIndex4 + "\"] on " + type + " \"" + nameTag + "\"");
                        el.InnerText = newIndex4;
                    }
                }
                #endregion

                #region Change FrgFolg
                XmlNodeList FrgFolg1Arr = doc.SelectNodes("//ns2:Component[@Name='FrgFolg" + oldIndex1 + "']", ns);
                XmlNodeList FrgFolg2Arr = doc.SelectNodes("//ns2:Component[@Name='FrgFolg" + oldIndex2 + "']", ns);
                XmlNodeList FrgFolg3Arr = doc.SelectNodes("//ns2:Component[@Name='FrgFolg" + oldIndex3 + "']", ns);
                XmlNodeList FrgFolg4Arr = doc.SelectNodes("//ns2:Component[@Name='FrgFolg" + oldIndex4 + "']", ns);

                foreach (XmlNode FrgFolg in FrgFolg1Arr)
                {
                    FrgFolg.Attributes[0].Value = "FrgFolg" + newIndex1;

                    XmlNode frgByte = FrgFolg.ParentNode.ParentNode.NextSibling.SelectSingleNode("ns2:Constant/ns2:ConstantValue", ns);
                    if (frgByte != null)
                        frgByte.InnerText = newIndex1;
                }
                foreach (XmlNode FrgFolg in FrgFolg2Arr)
                {
                    FrgFolg.Attributes[0].Value = "FrgFolg" + newIndex2;

                    XmlNode frgByte = FrgFolg.ParentNode.ParentNode.NextSibling.SelectSingleNode("ns2:Constant/ns2:ConstantValue", ns);
                    if (frgByte != null)
                        frgByte.InnerText = newIndex2;
                }
                foreach (XmlNode FrgFolg in FrgFolg3Arr)
                {
                    FrgFolg.Attributes[0].Value = "FrgFolg" + newIndex3;

                    XmlNode frgByte = FrgFolg.ParentNode.ParentNode.NextSibling.SelectSingleNode("ns2:Constant/ns2:ConstantValue", ns);
                    if (frgByte != null) 
                        frgByte.InnerText = newIndex3;
                }
                foreach (XmlNode FrgFolg in FrgFolg4Arr)
                {
                    FrgFolg.Attributes[0].Value = "FrgFolg" + newIndex4;

                    XmlNode frgByte = FrgFolg.ParentNode.ParentNode.NextSibling.SelectSingleNode("ns2:Constant/ns2:ConstantValue", ns);
                    if (frgByte != null)
                        frgByte.InnerText = newIndex4;
                }
                #endregion

                doc.InnerXml = doc.InnerXml.Replace("Folge " + oldIndex1, "Folge " + newIndex1);
                doc.InnerXml = doc.InnerXml.Replace("Folge " + oldIndex2, "Folge " + newIndex2);
                doc.InnerXml = doc.InnerXml.Replace("Folge " + oldIndex3, "Folge " + newIndex3);
                doc.InnerXml = doc.InnerXml.Replace("Folge " + oldIndex4, "Folge " + newIndex4);

                doc.InnerXml = doc.InnerXml.Replace("InFolge_" + oldIndex1, "InFolge_" + newIndex1);
                doc.InnerXml = doc.InnerXml.Replace("InFolge_" + oldIndex2, "InFolge_" + newIndex2);
                doc.InnerXml = doc.InnerXml.Replace("InFolge_" + oldIndex3, "InFolge_" + newIndex3);
                doc.InnerXml = doc.InnerXml.Replace("InFolge_" + oldIndex4, "InFolge_" + newIndex4);

                #region Write on Log file
                if (FrgFolge1Arr != null || FrgFolge1 != null)
                    WriteStatusEntry("Changed \"FrgFolge" + oldIndex1 + "\" to \"FrgFolge" + newIndex1 + "\" on " + type + " \"" + nameTag + "\"");
                if (FrgFolge2Arr != null || FrgFolge2 != null)
                    WriteStatusEntry("Changed \"FrgFolge" + oldIndex2 + "\" to \"FrgFolge" + newIndex2 + "\" on " + type + " \"" + nameTag + "\"");
                if (FrgFolge3Arr != null || FrgFolge3 != null)
                    WriteStatusEntry("Changed \"FrgFolge" + oldIndex3 + "\" to \"FrgFolge" + newIndex3 + "\" on " + type + " \"" + nameTag + "\"");
                if (FrgFolge4Arr != null || FrgFolge4 != null)
                    WriteStatusEntry("Changed \"FrgFolge" + oldIndex4 + "\" to \"FrgFolge" + newIndex4 + "\" on " + type + " \"" + nameTag + "\"");

                if (FrgFolg1Arr != null)
                    WriteStatusEntry("Changed \"FrgFolg" + oldIndex1 + "\" to \"FrgFolg" + newIndex1 + "\" on " + type + " \"" + nameTag + "\"");
                if (FrgFolg2Arr != null)
                    WriteStatusEntry("Changed \"FrgFolg" + oldIndex2 + "\" to \"FrgFolg" + newIndex2 + "\" on " + type + " \"" + nameTag + "\"");
                if (FrgFolg3Arr != null)
                    WriteStatusEntry("Changed \"FrgFolg" + oldIndex3 + "\" to \"FrgFolg" + newIndex3 + "\" on " + type + " \"" + nameTag + "\"");
                if (FrgFolg4Arr != null)
                    WriteStatusEntry("Changed \"FrgFolg" + oldIndex4 + "\" to \"FrgFolg" + newIndex4 + "\" on " + type + " \"" + nameTag + "\"");

                if (doc.InnerXml.Contains("Folge " + oldIndex1))
                    WriteStatusEntry("Changed \"Folge " + oldIndex1 + "\" to \"Folge " + newIndex1 + "\" on " + type + " \"" + nameTag + "\"");
                if (doc.InnerXml.Contains("Folge " + oldIndex2))
                    WriteStatusEntry("Changed \"Folge " + oldIndex2 + "\" to \"Folge " + newIndex2 + "\" on " + type + " \"" + nameTag + "\"");
                if (doc.InnerXml.Contains("Folge " + oldIndex3))
                    WriteStatusEntry("Changed \"Folge " + oldIndex3 + "\" to \"Folge " + newIndex3 + "\" on " + type + " \"" + nameTag + "\"");
                if (doc.InnerXml.Contains("Folge " + oldIndex4))
                    WriteStatusEntry("Changed \"Folge " + oldIndex4 + "\" to \"Folge " + newIndex4 + "\" on " + type + " \"" + nameTag + "\"");

                if (doc.InnerXml.Contains("InFolge_ " + oldIndex1))
                    WriteStatusEntry("Changed \"InFolge_ " + oldIndex1 + "\" to \"InFolge_ " + newIndex1 + "\" on " + type + " \"" + nameTag + "\"");
                if (doc.InnerXml.Contains("InFolge_ " + oldIndex2))
                    WriteStatusEntry("Changed \"InFolge_ " + oldIndex2 + "\" to \"InFolge_ " + newIndex2 + "\" on " + type + " \"" + nameTag + "\"");
                if (doc.InnerXml.Contains("InFolge_ " + oldIndex3))
                    WriteStatusEntry("Changed \"InFolge_ " + oldIndex3 + "\" to \"InFolge_ " + newIndex3 + "\" on " + type + " \"" + nameTag + "\"");
                if (doc.InnerXml.Contains("InFolge_ " + oldIndex4))
                    WriteStatusEntry("Changed \"InFolge_ " + oldIndex4 + "\" to \"InFolge_ " + newIndex4 + "\" on " + type + " \"" + nameTag + "\"");
                #endregion
            }
            #endregion

            #region CREATE PRODIAG
            XmlNode prodiagFB = doc.SelectSingleNode("//AssignedProDiagFB");
            if (prodiagFB != null)
            {
                string prodiagFBName = prodiagFB.InnerText;
                var group = (current as PlcBlockUserGroup).Groups.Find("110_ProDiag");
                //PlcBlockGroup blockFolder = (parent as PlcSoftware).BlockGroup;
                PlcBlockComposition blockComposition = group.Blocks;
                if (blockComposition.Find(prodiagFBName) == null && blockComposition != null) // If prodiag block doesn't exist && exist blocks in prodiag folder
                {
                    bool isAutoNumber = true;
                    int number = 1;
                    var progLang = ProgrammingLanguage.ProDiag;
                    string iDBName = prodiagFBName + "_DB";
                    blockComposition.CreateFB(prodiagFBName, isAutoNumber, number, progLang);
                    blockComposition.CreateInstanceDB(iDBName, isAutoNumber, number, prodiagFBName);
                    WriteStatusEntry("ProDiag block created with name: \"" + prodiagFBName + "\"");
                }
            }
            #endregion

            XmlNode author = doc.SelectSingleNode("//HeaderAuthor");
            if (author != null) author.InnerText = "INTROSYS";

            if (safetyFolder)
            {
                XmlNode progLang = doc.SelectSingleNode("//ProgrammingLanguage");
                if (progLang != null && progLang.InnerText.Contains("F_"))
                {
                    // Is Safety block

                    XmlNode blockName = doc.SelectSingleNode("//Name");
                    XmlNode blockHeaderName = doc.SelectSingleNode("//HeaderName");
                    XmlNode isWriteProtectedInAS = doc.SelectSingleNode("//IsWriteProtectedInAS");
                    XmlNode isOnlyStoredInLoadMemory = doc.SelectSingleNode("//IsOnlyStoredInLoadMemory");

                    if (isWriteProtectedInAS != null) isWriteProtectedInAS.Attributes.RemoveAll();
                    if (isOnlyStoredInLoadMemory != null) isOnlyStoredInLoadMemory.Attributes.RemoveAll();

                    //blockName.InnerText = XmlParser.RemoveWindowsUnallowedChars(blockName.InnerText);
                    blockName.InnerText += "_";
                    if (!string.IsNullOrEmpty(blockHeaderName.InnerText))
                        blockHeaderName.InnerText = blockName.InnerText + "_";

                    doc.InnerXml = doc.InnerXml.Replace("F_LAD", "LAD");
                    doc.InnerXml = doc.InnerXml.Replace("F_DB", "DB");
                }
            }

            foreach (var s in xmlReplacedStrings)
            {
                WriteStatusEntry("Replaced XML of " + type + " \"" + nameTag + "\" (" + s + ")");
            }
            
            RemoveQuotsComponent(doc);

            return doc;
        }

        /// <summary>
        /// Get old and new names written in dataGridView
        /// </summary>
        /// <returns>List containing names</returns>
        private List<KeyValuePair<string, string>> GetNames()
        {
            var names = new List<KeyValuePair<string, string>>();

            for (int i = 0; i < _dataGridView.RowCount - 1; i++)
            {
                string oldName = (string)_dataGridView[0, i].Value;
                string newName = (string)_dataGridView[1, i].Value;
                names.Add(new KeyValuePair<string, string>(oldName, newName));
            }

            return names;
        }

        /// <summary>
        /// Check if given string (s) contains any value in "Old Name" strings
        /// </summary>
        /// <param name="s">String to search</param>
        /// <param name="names">Names in WindowsFormsGrid</param>
        /// <returns></returns>
        private string GetKeyThatContains(string s, List<KeyValuePair<string, string>> names)
        {
            string key = "";

            foreach (var d in names)
            {
                if (s.Contains(d.Key))
                {
                    return d.Key;
                }
            }

            return key;
        }

        #region Export PLC Tags
        private void ExportAllTagTables(PlcSoftware plcSoftware, string path)
        {
            PlcTagTableSystemGroup plcTagTableSystemGroup = plcSoftware.TagTableGroup;

            // Export all tables in the system group
            ExportTagTables(plcTagTableSystemGroup.TagTables, path);

            // Export the tables in underlying user groups
            foreach (PlcTagTableUserGroup userGroup in plcTagTableSystemGroup.Groups)
            {
                ExportUserGroupDeep(userGroup, path);
            }
        }

        private void ExportTagTables(PlcTagTableComposition tagTables, string path)
        {
            foreach (PlcTagTable table in tagTables)
            {
                string SafeTableName = string.Join("", table.Name.Split(Path.GetInvalidFileNameChars()));
                FileInfo fInfo = new FileInfo(Path.Combine(path, SafeTableName) + ".xml");

                if (fInfo.Exists)
                {
                    File.Delete(Path.Combine(path, SafeTableName) + ".xml");
                }

                if(exportAllTags)
                    table.Export(fInfo, ExportOptions.WithDefaults);
                else
                {
                    if (tagsToExport.Contains(table.Name))
                    {
                        table.Export(fInfo, ExportOptions.WithDefaults);
                        WriteStatusEntry("Tag table \"" + table.Name + "\" exported");
                    }
                }
            }
        }

        private void ExportUserGroupDeep(PlcTagTableUserGroup group, string path)
        {
            ExportTagTables(group.TagTables, path);
            foreach (PlcTagTableUserGroup userGroup in group.Groups)
            {
                ExportUserGroupDeep(userGroup, path);
            }
        }
        #endregion

        /// <summary>
        /// Generate XmlDocument when import tags to tia portal fails
        /// </summary>
        /// <param name="path"></param>
        private XmlDocument FailImportXmlDoc(string path)
        {
            XmlDocument doc = new XmlDocument();
            XmlDocument tagsDoc = new XmlDocument();
            tagsDoc.Load(path);
            string plcTagTableName = GetNamePlcTagDoc(tagsDoc);
            XElement tagTable = new XElement("Tagtable", new XAttribute("name", plcTagTableName));
            XmlNodeList tags = tagsDoc.SelectNodes("//Document//ObjectList//SW.Tags.PlcTag");

            foreach (XmlNode tag in tags)
            {
                string symbolic = GetSymbolicFromTag(tag);
                string dataType = GetDataTypeFromTag(tag);
                string address = GetAddressFromTag(tag);
                string comment = GetCommentFromTag(tag);

                tagTable.Add(new XElement("Tag", symbolic, new XAttribute("type", dataType), new XAttribute("hmiVisible", "False")
                                                           , new XAttribute("hmiWriteable", "False"), new XAttribute("hmiAccessible", "False")
                                                           , new XAttribute("retain", "False"), new XAttribute("remark", comment)
                                                           , new XAttribute("addr", address)));
            }

            doc.Load(tagTable.CreateReader());
            XmlNode xmldecl = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.PrependChild(xmldecl);
            return doc;
        }

        #region Get info from XML
        /// <summary>
        /// Get PLC Tag Table name
        /// </summary>
        /// <param name="tagsDoc"></param>
        /// <returns></returns>
        private string GetNamePlcTagDoc(XmlDocument tagsDoc)
        {
            XmlNode nameNode = tagsDoc.SelectSingleNode("//Document//SW.Tags.PlcTagTable//AttributeList//Name");
            string name = nameNode.InnerText;
            return name;
        }

        /// <summary>
        /// Get Symbolic from PLC Tag Table node
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        private string GetSymbolicFromTag(XmlNode tag)
        {
            XmlNode SymbolicNode = tag.SelectSingleNode("AttributeList//Name");
            string symbolic = SymbolicNode.InnerText;
            return symbolic;
        }

        /// <summary>
        /// Get DataType from PLC Tag Table node
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        private string GetDataTypeFromTag(XmlNode tag)
        {
            XmlNode DataTypeNameNode = tag.SelectSingleNode("AttributeList//DataTypeName");
            string type = DataTypeNameNode.InnerText;
            return type;
        }

        /// <summary>
        /// Get Address from PLC Tag Table node
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        private string GetAddressFromTag(XmlNode tag)
        {
            XmlNode AddressNode = tag.SelectSingleNode("AttributeList//LogicalAddress");
            string address = AddressNode.InnerText;
            return address;
        }

        /// <summary>
        /// Get Comment from PLC Tag Table node
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        private string GetCommentFromTag(XmlNode tag)
        {
            XmlNode CommentNode = tag.SelectSingleNode("ObjectList//MultilingualText//ObjectList//MultilingualTextItem//AttributeList//Text");
            string comment = CommentNode.InnerText;
            return comment;
        }
        #endregion

        /// <summary>
        /// Remove "&quot;" from XML Component Node
        /// </summary>
        /// <param name="doc">XML File</param>
        private void RemoveQuotsComponent(XmlDocument doc)
        {
            XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace("ns", "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v3");
            XmlNodeList nodes = doc.SelectNodes("//ns:Component[contains(@Name, '\"')]", ns);
            foreach (XmlNode tag in nodes)
            {
                string textInAttribute = tag.Attributes["Name"].InnerText;
                List<string> quotes = textInAttribute.Split('"').ToList();
                foreach (var q in quotes)
                {
                    if (!string.IsNullOrEmpty(q))
                    {
                        XmlNode newComponent = doc.CreateNode(XmlNodeType.Element, "Component", "");
                        XmlAttribute attr = doc.CreateAttribute("Name");
                        if (q.FirstOrDefault() == '.')
                            attr.Value = q.Substring(1);
                        else
                            attr.Value = q;
                        newComponent.Attributes.SetNamedItem(attr);
                        tag.ParentNode.InsertBefore(newComponent, tag);
                    }
                }
            }

            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                nodes[i].ParentNode.RemoveChild(nodes[i]);
            }
        }

        /// <summary>
        /// Validate if input is a number
        /// </summary>
        /// <param name="e"></param>
        private bool NumberValidationTextBox(string e)
        {
            Regex regex = new Regex("[^0-9]+");
            return !regex.IsMatch(e);
        }

        /// <summary>
        /// Old Index Combobox
        /// </summary>
        /// <param name="selectedValue"></param>
        private void cbOldIndexSelection(string selectedValue)
        {
            switch (selectedValue)
            {
                case "KETVOL":
                    OldIndex1 = "1";
                    OldIndex2 = "41";
                    OldIndex3 = "61";
                    OldIndex4 = "81";
                    break;
                case "KETVOR":
                    OldIndex1 = "2";
                    OldIndex2 = "42";
                    OldIndex3 = "62";
                    OldIndex4 = "82";
                    break;
                case "KETHIR":
                    OldIndex1 = "3";
                    OldIndex2 = "43";
                    OldIndex3 = "63";
                    OldIndex4 = "83";
                    break;
                case "KETHIL":
                    OldIndex1 = "4";
                    OldIndex2 = "44";
                    OldIndex3 = "64";
                    OldIndex4 = "84";
                    break;
            }
        }

        /// <summary>
        /// New Index Combobox
        /// </summary>
        /// <param name="selectedValue"></param>
        private void cbNewIndexSelection(string selectedValue)
        {
            switch (selectedValue)
            {
                case "KETVOL":
                    NewIndex1 = "1";
                    NewIndex2 = "41";
                    NewIndex3 = "61";
                    NewIndex4 = "81";
                    break;
                case "KETVOR":
                    NewIndex1 = "2";
                    NewIndex2 = "42";
                    NewIndex3 = "62";
                    NewIndex4 = "82";
                    break;
                case "KETHIR":
                    NewIndex1 = "3";
                    NewIndex2 = "43";
                    NewIndex3 = "63";
                    NewIndex4 = "83";
                    break;
                case "KETHIL":
                    NewIndex1 = "4";
                    NewIndex2 = "44";
                    NewIndex3 = "64";
                    NewIndex4 = "84";
                    break;
            }
        }

        /// <summary>
        /// Change devices IP Address
        /// </summary>
        private void ChangeDeviceIP()
        {
            foreach (var net in tiaPortalProject.Subnets)
            {
                foreach (var node in net.Nodes)
                {
                    bool ipChanged = true;
                    string deviceName = node.GetAttribute("PnDeviceName").ToString().ToUpper();
                    string deviceAddress = node.GetAttribute("Address").ToString();
                    string lastIpDigits = IPAddress.Parse(deviceAddress).GetAddressBytes()[3].ToString();

                    // "deviceAddress" string will change when .SetAttribute is used. 
                    // this way we save the string in a new variable called "deviceOldAddress"
                    string deviceOldAddress = String.Copy(deviceAddress);

                    string deviceNewAddress = _ip1 + "." + _ip2 + "." + _ip3 + "." + lastIpDigits;

                    try
                    {
                        node.SetAttribute("Address", deviceNewAddress);
                    }
                    catch (Exception)
                    {
                        deviceChangeIpFail.Add(deviceName);
                        WriteStatusEntry("Error changing device \"" + deviceName + "\" IP");
                        ipChanged = false;
                    }

                    if(ipChanged) 
                        WriteStatusEntry("IP changed of device \"" + deviceName + "\" (" + deviceNewAddress + ")");

                    try
                    {
                        node.SetAttribute("RouterAddress", _ip1 + "." + _ip2 + "." + _ip3 + ".1");
                    } 
                    catch(Exception)
                    {
                        continue;
                    }
                }
            }
        }

        #region Commands
        /// <summary>
        /// Event handler rename button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RenameCommand_Executed(object sender, EventArgs e)
        {
            vDataGridView.CurrentCell = null;

            string initTime = String.Copy(DateTime.Now.ToString());

            if (!CanInitializeRename())
                return;

            StatusListView.Clear();

            foreach (TreeViewItem item in ProjectTree.View[0].Items)
            {
                GetSelectedTreeNodes(item);
            }

            if (blocksToExport.Contains("ARG")) exportArg = true; 
            if (blocksToExport.Contains("PLC tags")) exportAllTags = true;


            if (blocksToExport.Any() || tagsToExport.Any())
            {
                // If export path exist
                if (Directory.Exists(exportPath))
                {
                    try
                    {
                        Directory.Delete(exportPath, true);    // Delete it
                        Directory.CreateDirectory(exportPath); // Create again
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(new Form() { TopMost = true }, "File Explorer is being used. \nClose it and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        WriteStatusEntry("File Explorer is being used. \nClose it and try again.");
                        return;
                    }
                }
            }

            RenamePLC();

            string endTime = String.Copy(DateTime.Now.ToString());

            if (unnamedDevices.Any())
            {
                string error = "An error occurred while renaming these devices: \n\n";
                foreach (string d in unnamedDevices)
                {
                    error += "- " + d + "\n";
                }
                MessageBox.Show(new Form() { TopMost = true }, error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (plcTagsToImport.Any())
            {
                string error = "An error occurred while creating these PLC Tags (Password Required): \n\n";
                foreach (string t in plcTagsToImport)
                {
                    error += "- " + t + "\n";
                }
                error += "\nPlease, import them manually.\n\n";
                error += "Path: " + Path.Combine(exportPath, "PLC Tags", "To Import Manually");
                MessageBox.Show(new Form() { TopMost = true }, error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (blocksImportFail.Any())
            {
                string error = "An error occurred while importing these blocks: \n\n";
                foreach (string t in blocksImportFail)
                {
                    error += "- " + t + "\n";
                }
                MessageBox.Show(new Form() { TopMost = true }, error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (deviceChangeIpFail.Any())
            {
                string error = "An error occurred while changing these devices IP: \n\n";
                foreach (string d in deviceChangeIpFail)
                {
                    error += "- " + d + "\n";
                }
                MessageBox.Show(new Form() { TopMost = true }, error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            unnamedDevices.Clear();
            plcTagsToImport.Clear();
            blocksImportFail.Clear();
            deviceChangeIpFail.Clear();

            #region Write Log File
            if (!File.Exists(logPath))
            {
                TextWriter tw = new StreamWriter(logPath);

                tw.WriteLine("===========================================");
                tw.WriteLine("Project: " + tiaPortalProject.Name);
                tw.WriteLine("Date: " + initTime);
                tw.WriteLine("===========================================");
                tw.WriteLine();

                foreach (string s in StatusListView)
                    tw.WriteLine(s);

                tw.WriteLine("\n~~~~ INIT TIME: " + initTime + " \tEND TIME: " + endTime + " ~~~~\n");

                tw.Close();
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("\n");
                sb.Append("\n===========================================");
                sb.Append("\nDate: " + initTime);
                sb.Append("\n===========================================");
                sb.Append("\n\n");

                foreach (string s in StatusListView)
                    sb.Append(s + "\n");

                sb.Append("\n~~~~ INIT TIME: " + initTime + " \tEND TIME: " + endTime + " ~~~~\n");

                File.AppendAllText(logPath, sb.ToString());
            }

            LogExist = true;
            #endregion

            WriteStatusEntry("PLC Renamed Successfully!");
        }

        /// <summary>
        /// Event handler openlog button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenLogCommand_Executed(object sender, EventArgs e)
        {
            if(File.Exists(logPath))
                Process.Start(logPath);
        }

        #region Config Commands
        /// <summary>
        /// Event handler export config button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportConfigCommand_Executed(object sender, EventArgs e)
        {
            vDataGridView.CurrentCell = null;
            List<string> groupNamesSelected = new List<string>();
            Dictionary<string, string> strings = new Dictionary<string, string>();

            groupNamesSelected = GetSelectedTreeNodesToList();
            for (int i = 0; i < _dataGridView.Rows.Count-1; i++)
            {
                var row = _dataGridView.Rows[i];
                if (row.Cells[0].Value == null || row.Cells[1].Value == null) continue;
                string oldString = row.Cells[0].Value.ToString();
                string newString = row.Cells[1].Value.ToString();
                strings.Add(oldString, newString);
            }

            GenerateConfigFile(groupNamesSelected, strings);
        }

        /// <summary>
        /// Event handler import config button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportConfigCommand_Executed(object sender, EventArgs e)
        {
            XmlSchemaSet schema = new XmlSchemaSet();
            schema.Add(null, "XMLPLCRenamerStructure.xsd");
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "XML Files|*.xml";
            dlg.InitialDirectory = Path.Combine(mainFolderPath, "PLC Configs");
            dlg.ShowDialog();

            if (File.Exists(dlg.FileName))
            {
                string path = dlg.FileName;
                XmlReader rd = XmlReader.Create(path);
                XDocument xDoc = XDocument.Load(rd);

                try
                {
                    xDoc.Validate(schema, ValidationEventHandler);
                }
                catch (Exception)
                {
                    MessageBox.Show("XML File not valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                vDataGridView.Rows.Clear();
                vDataGridView.Refresh();

                List<string> strings = new List<string>();
                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                #region Fill groups selected
                if (((TreeViewItem)ProjectTree.View[0].Items[0]).Tag.ToString() != "Inconsistent")
                {
                    var groupsSelectedNodes = doc.SelectNodes("//Groups//Group");
                    foreach (XmlNode group in groupsSelectedNodes)
                    {
                        strings.Add(group.InnerText);
                    }
                    foreach (TreeViewItem item in ProjectTree.View[0].Items)
                    {
                        UnselectTreeNodes(item);
                        SelectTreeNodes(item, strings);
                    }
                }
                #endregion

                #region Fill strings
                var stringsNodes = doc.SelectNodes("//Strings//String");

                foreach (XmlNode s in stringsNodes)
                {
                    string oldS = s.SelectSingleNode("OldString").InnerText;
                    string newS = s.SelectSingleNode("NewString").InnerText;

                    string[] row = new string[] { oldS, newS };
                    vDataGridView.Rows.Add(row);
                }
                #endregion

                #region Fill folges
                var folgesNode = doc.SelectSingleNode("//Folges");
                var indexesNode = doc.SelectSingleNode("//Indexes");
                string optionsFrom = folgesNode.SelectSingleNode("//Options//From").InnerText;
                string optionsTo = folgesNode.SelectSingleNode("//Options//To").InnerText;
                string oldIndex1 = indexesNode.SelectSingleNode("//Index1//Old").InnerText;
                string newIndex1 = indexesNode.SelectSingleNode("//Index1//New").InnerText;
                string oldIndex2 = indexesNode.SelectSingleNode("//Index2//Old").InnerText;
                string newIndex2 = indexesNode.SelectSingleNode("//Index2//New").InnerText;
                string oldIndex3 = indexesNode.SelectSingleNode("//Index3//Old").InnerText;
                string newIndex3 = indexesNode.SelectSingleNode("//Index3//New").InnerText;
                string oldIndex4 = indexesNode.SelectSingleNode("//Index4//Old").InnerText;
                string newIndex4 = indexesNode.SelectSingleNode("//Index4//New").InnerText;

                if (folgesNode.Attributes["selected"].Value == "true")
                    ChangeFolgenIsChecked = true;
                else
                    ChangeFolgenIsChecked = false;

                CbOldIndex = optionsFrom;
                CbNewIndex = optionsTo;
                
                OldIndex1 = oldIndex1;
                OldIndex2 = oldIndex2;
                OldIndex3 = oldIndex3;
                OldIndex4 = oldIndex4;
                NewIndex1 = newIndex1;
                NewIndex2 = newIndex2;
                NewIndex3 = newIndex3;
                NewIndex4 = newIndex4;

                #endregion

                #region Fill IP
                var ipNode = doc.SelectSingleNode("//IP");

                if (ipNode.Attributes["selected"].Value == "true")
                    ChangeIpIsChecked = true;
                else
                    ChangeIpIsChecked = false;

                string[] ip = ipNode.InnerText.Split('.');
                IP1 = ip[0];
                IP2 = ip[1];
                IP3 = ip[2];
                #endregion
            }
        }
        #endregion
        #endregion

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
        /// Writes the status entry.
        /// </summary>
        /// <param name="statusText">The status text.</param>
        private void WriteStatusEntry(string statusText)
        {
            StatusListView.Add(DateTime.Now + ": " + statusText);

            System.Windows.Forms.Application.DoEvents();
        }

        /// <summary>
        /// Check if can start renaming the PLC
        /// </summary>
        /// <returns></returns>
        private bool CanInitializeRename()
        {
            var names = GetNames();

            if (_changeFolgenIsChecked) // If change folgen is checked
            {
                // If one of the textboxes are empty
                if (string.IsNullOrEmpty(OldIndex1) || string.IsNullOrEmpty(OldIndex2) || string.IsNullOrEmpty(OldIndex3) || string.IsNullOrEmpty(OldIndex4) ||
                   string.IsNullOrEmpty(NewIndex1) || string.IsNullOrEmpty(NewIndex2) || string.IsNullOrEmpty(NewIndex3) || string.IsNullOrEmpty(NewIndex4))  
                {
                    return false;
                }
            } 
            else // If change folgen is not checked
            {
                if (names.Any())
                {
                    foreach (var item in names) // Check if every row in DataGridView has a old and a new name
                    {
                        if (item.Key == null || item.Value == null)
                            return false;
                    }
                }
                else
                {
                    if (_changeIpIsChecked && IP1 != null && IP2 != null && IP3 != null)
                    {
                        onlyChangeIP = true;
                        return true;
                    }
                    else
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// TreeViews the item selected changed callback.
        /// </summary>
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

        /// <summary>
        /// Creates the device TreeView item.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>TreeViewItem</returns>
        private static TreeViewItem CreateDeviceTreeViewItem(Device device)
        {
            TreeViewItem item = null;

            var plcSoftware = OpennessHelper.GetPlcSoftware(device);

            if (plcSoftware != null)
            {
                TreeViewItem plc = new TreeViewItem
                {
                    Header = plcSoftware.Name,
                    Tag = plcSoftware
                };

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

                item = plc;
            }

            return item;
        }

        #region TreeView Events
        /// <summary>
        /// Checkbox check event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void checkBox_OnCheck(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            ChangeChilds((TreeViewItem)checkbox.Parent, true);
        }

        /// <summary>
        /// To prevent event to fire when checkbox is changed dynamically
        /// </summary>
        private static bool dynamicUncheck = false;

        /// <summary>
        /// Checkbox uncheck event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void checkBox_Uncheck(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;

            if (!dynamicUncheck)
            {
                ChangeChilds((TreeViewItem)checkbox.Parent, false);
                ChangeParents((TreeViewItem)checkbox.Parent);
            }
        }

        /// <summary>
        /// Check or Uncheck childs of selected value
        /// </summary>
        /// <param name="checkChilds"></param>
        /// <param name="tvi"></param>
        private static void ChangeChilds(TreeViewItem tvi, bool checkChilds)
        {
            dynamicUncheck = true;

            foreach (TreeViewItem item in tvi.Items)
            {
                if(((CheckBox)item.Header).IsEnabled)
                    ((CheckBox)item.Header).IsChecked = checkChilds;

                string selectedItemName = ((TextBlock)((CheckBox)item.Header).Content).Text;

                if (item.Items.Count > 0)
                    ChangeChilds(item, checkChilds);
            }

            dynamicUncheck = false;
        }

        /// <summary>
        /// Uncheck parents of selected value
        /// </summary>
        /// <param name="item"></param>
        private static void ChangeParents(TreeViewItem item)
        {
            DependencyObject target = item;

            dynamicUncheck = true;

            while (target != null)
            {
                if (target is TreeView)
                    break;
                if (target is TreeViewItem)
                {
                    if (((TreeViewItem)target).Header is CheckBox)
                    {
                        var tviCheckBox = (CheckBox)((TreeViewItem)target).Header;
                        tviCheckBox.IsChecked = false;
                    }
                }

                target = VisualTreeHelper.GetParent(target);
            }

            dynamicUncheck = false;

            //var checkboxParent = (CheckBox)((TreeViewItem)checkbox.Parent).Header;
            //dynamicUncheck = true;
            //checkboxParent.IsChecked = false;
        }

        /// <summary>
        /// Disable incosistent groups in TreeViewItems
        /// </summary>
        /// <param name="tvi"></param>
        private void DisableIncosistentGroups(TreeViewItem tvi)
        {
            foreach (TreeViewItem item in tvi.Items)
            {
                string itemName = ((TextBlock)((CheckBox)item.Header).Content).Text;

                if(inconsistentBlocks.Contains(itemName))
                    ((CheckBox)item.Header).IsEnabled = false;

                if (item.Items.Count > 0)
                    DisableIncosistentGroups(item);
            }
        }

        /// <summary>
        /// Get TreeView selected items
        /// </summary>
        /// <param name="tvi"></param>
        private void GetSelectedTreeNodes(TreeViewItem tvi)
        {
            if (tvi.Items.Count == 0 && ((CheckBox)tvi.Header).IsChecked == true)
            {
                string cbTag = ((CheckBox)tvi.Header).Tag.ToString();
                if (cbTag == "hw") renameDevices = true;
            }
            else
            {
                foreach (TreeViewItem item in tvi.Items)
                {
                    if (((CheckBox)item.Header).IsChecked == true)
                    {
                        string cbTag = ((CheckBox)item.Header).Tag.ToString();
                        string itemName = ((TextBlock)((CheckBox)item.Header).Content).Text;

                        if (cbTag == "plctag") tagsToExport.Add(itemName);
                        if (cbTag == "block") blocksToExport.Add(itemName);
                    }

                    if (item.Items.Count > 0)
                        GetSelectedTreeNodes(item);
                }
            }
        }

        /// <summary>
        /// Creates the list of selected group names and return 
        /// </summary>
        /// <returns></returns>
        private List<string> GetSelectedTreeNodesToList()
        {
            List<string> groupNamesSelected = new List<string>();
            foreach (TreeViewItem item in ProjectTree.View[0].Items)
            {
                GetSelectedTreeNodesToList(item, groupNamesSelected);
            }
            return groupNamesSelected;
        }

        /// <summary>
        /// Get TreeView selected items and return a list
        /// </summary>
        /// <param name="tvi"></param>
        /// <param name="list"></param>
        private void GetSelectedTreeNodesToList(TreeViewItem tvi, List<string> list)
        {
            if (tvi.Items.Count == 0 && ((CheckBox)tvi.Header).IsChecked == true)
            {
                string cbTag = ((CheckBox)tvi.Header).Tag.ToString();
                if (cbTag == "hw") list.Add("Rename Hardware");
            }
            else
            {
                foreach (TreeViewItem item in tvi.Items)
                {
                    if (((CheckBox)item.Header).IsChecked == true)
                    {
                        string itemName = ((TextBlock)((CheckBox)item.Header).Content).Text;
                        list.Add(itemName);
                    }

                    if (item.Items.Count > 0 && !((CheckBox)item.Header).IsChecked == true)
                        GetSelectedTreeNodesToList(item, list);
                }
            }
        }

        /// <summary>
        /// Select TreeNodes by Name
        /// </summary>
        /// <param name="tvi"></param>
        /// <param name="names"></param>
        private void SelectTreeNodes(TreeViewItem tvi, List<string> names)
        {
            if (tvi.Items.Count == 0)
            {
                string itemName = ((TextBlock)((CheckBox)tvi.Header).Content).Text;

                if (((CheckBox)tvi.Header).IsEnabled && names.Contains(itemName))
                    ((CheckBox)tvi.Header).IsChecked = true;
            }
            else
            {
                foreach (TreeViewItem item in tvi.Items)
                {
                    string itemName = ((TextBlock)((CheckBox)item.Header).Content).Text;

                    if (((CheckBox)item.Header).IsEnabled && names.Contains(itemName))
                        ((CheckBox)item.Header).IsChecked = true;

                    if (item.Items.Count > 0)
                        SelectTreeNodes(item, names);
                }
            }
        }
        
        /// <summary>
        /// Unselect all TreeNodes
        /// </summary>
        /// <param name="tvi"></param>
        private void UnselectTreeNodes(TreeViewItem tvi)
        {
            if (tvi.Items.Count == 0)
            {
                if (tvi.Header is CheckBox)
                    ((CheckBox)tvi.Header).IsChecked = false;
            }
            else
            {
                foreach (TreeViewItem item in tvi.Items)
                {
                    if(tvi.Header is CheckBox)
                        ((CheckBox)tvi.Header).IsChecked = false;

                    if (item.Items.Count > 0)
                        UnselectTreeNodes(item);
                }
            }
        }
        #endregion

        /// <summary>
        /// Generate export config XML file
        /// </summary>
        /// <param name="groups"></param>
        /// <param name="strings"></param>
        private void GenerateConfigFile(List<string> groups, Dictionary<string, string> strings)
        {
            XDocument xDoc = new XDocument
            {
                Declaration = new XDeclaration("1.0", "utf-8", null)
            };
            string ip = IP1 + "." + IP2 + "." + IP3 + ".1";
            XElement xConfig = new XElement("Config", new XAttribute("project", tiaPortalProject.Name));
            XElement xGroups = new XElement("Groups");
            foreach (var group in groups)
            {
                xGroups.Add(new XElement("Group", group));
            }
            XElement xStrings = new XElement("Strings");
            foreach (var item in strings)
            {
                xStrings.Add(new XElement("String", new XElement("OldString", item.Key), new XElement("NewString", item.Value)));
            }
            XElement xFolges = new XElement("Folges", new XAttribute("selected", _changeFolgenIsChecked));
            XElement xOptions = new XElement("Options", new XElement("From", _cbOldIndex), new XElement("To", _cbNewIndex));
            XElement xIndexes = new XElement("Indexes");
            xIndexes.Add(new XElement("Index1", new XElement("Old", _oldIndex1), new XElement("New", _newIndex1)));
            xIndexes.Add(new XElement("Index2", new XElement("Old", _oldIndex2), new XElement("New", _newIndex2)));
            xIndexes.Add(new XElement("Index3", new XElement("Old", _oldIndex3), new XElement("New", _newIndex3)));
            xIndexes.Add(new XElement("Index4", new XElement("Old", _oldIndex4), new XElement("New", _newIndex4)));
            xFolges.Add(xOptions);
            xFolges.Add(xIndexes);
            XElement xIP = new XElement("IP", new XAttribute("selected", _changeIpIsChecked), ip);
            xConfig.Add(xGroups);
            xConfig.Add(xStrings);
            xConfig.Add(xFolges);
            xConfig.Add(xIP);
            xDoc.Add(xConfig);

            try
            {
                string savePath = Path.Combine(mainFolderPath, "PLC Configs", tiaPortalProject.Name + ".xml");
                xDoc.Save(savePath);
                MessageBox.Show("Config successfully exported to \"" + savePath + "\"", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Check if a directory is empty
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        /// <summary>
        /// Checks if TIA Portal blocks are consistent
        /// </summary>
        /// <param name="item">Project, station or folder to export</param>
        public void CheckConsistency(IEngineeringCompositionOrObject item)
        {
            var groupName = (item as IEngineeringObject).GetAttribute("Name").ToString();

            if (groupName.Contains("DB-"))
                groupName = (item.Parent as IEngineeringObject).GetAttribute("Name").ToString();

            foreach (var sub in OpennessHelper.GetSubItem(item))
            {
                if (!(sub is Cycle && (sub as Cycle).IsSystemObject))
                {
                    if (sub is PlcBlock)
                    {
                        var block = sub as PlcBlock;
                        if (block.ProgrammingLanguage == ProgrammingLanguage.ProDiag || block.ProgrammingLanguage == ProgrammingLanguage.ProDiag_OB)
                            return;

                        if (!block.IsConsistent)
                        {
                            inconsistentBlocks.Add(groupName);
                            return;
                        }
                    }
                }
            }

            foreach (var folder in OpennessHelper.GetSubFolder(item))
            {
                CheckConsistency(folder as IEngineeringCompositionOrObject);
            }
        }
    }
}

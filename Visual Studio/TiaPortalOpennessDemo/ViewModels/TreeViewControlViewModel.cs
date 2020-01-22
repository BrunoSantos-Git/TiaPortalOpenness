using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubversionConnector.Commands;
using SubversionConnector.Utils;
using System.Windows.Controls;
using TiaOpennessHelper;
using System.Collections.ObjectModel;

namespace SubversionConnector.ViewModels
{
    class TreeViewControlViewModel : ViewModelBase
    {
        private ObservableCollection<TreeView> _treeView = new ObservableCollection<TreeView>();
        public ObservableCollection<TreeView> TreeView
        {
            get { return _treeView; }
            set
            {
                if (_treeView == value)
                    return;
                base.RaisePropertyChanged("TreeView");
            }
        }

        private object _selectedItem = new object();
        public object CurrentSelectedTreeViewItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem == value)
                    return;
                _selectedItem = value;
                base.RaisePropertyChanged("CurrentSelectedTreeViewItem");
            }
        }

        public RelayCommand<TreeViewHelper.DependencyPropertyEventArgs> MySelItemChgCmd { get; set; }
        public RelayCommand<TreeViewHelper.DependencyPropertyEventArgs> MySelItemChgVarCmd { get; set; }

        public TreeViewControlViewModel(RelayCommand<TreeViewHelper.DependencyPropertyEventArgs> chgCmd, RelayCommand<TreeViewHelper.DependencyPropertyEventArgs> chgVarCmd)
        {
            MySelItemChgCmd = chgCmd;
            MySelItemChgVarCmd = chgVarCmd;
        }
    }
}

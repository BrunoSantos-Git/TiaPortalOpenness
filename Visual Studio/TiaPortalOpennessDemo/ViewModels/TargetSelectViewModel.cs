using System;
using System.Collections.Generic;
using System.Windows.Controls;
using TiaPortalOpennessDemo.Commands;
using TiaPortalOpennessDemo.Utilities;

namespace TiaPortalOpennessDemo.ViewModels
{
    class TargetSelectViewModel : ViewModelBase
    {
        public bool Result { get; set; }
        public Action CloseAction { get; set; }

        private TreeViewHandler _targetTree;
        public TreeViewHandler TargetTree
        {
            get { return _targetTree; }
            set
            {
                if (_targetTree == value)
                    return;
                _targetTree = value;
                RaisePropertyChanged("TargetTree");
            }
        }

        private object _selectedTarget;
        public object SelectedTarget
        {
            get { return _selectedTarget; }
            set
            {
                if (_selectedTarget == value)
                    return;
                _selectedTarget = value;
                RaisePropertyChanged("SelectedTarget");
            }
        }

        public TargetSelectViewModel(IEnumerable<dynamic> list)
        {
            InitializeCommands();
            TargetTree = new TreeViewHandler(new RelayCommand<DependencyPropertyEventArgs>(TreeViewSelectedItemChangedCallback));


            var treeView = new TreeView();
            foreach (var item in list)
            {
                var sub = new TreeViewItem();
                sub.Header = item.Name;
                sub.Tag = item;

                treeView.Items.Add(sub);
            }
            TargetTree.View.Add(treeView);



            Result = false;
        }

        public TargetSelectViewModel(TreeViewHandler handler)
        {
            InitializeCommands();

            TargetTree = handler;

            Result = false;
        }

        public TargetSelectViewModel(TreeView view)
        {
            InitializeCommands();
            TargetTree = new TreeViewHandler(new RelayCommand<DependencyPropertyEventArgs>(TreeViewSelectedItemChangedCallback));
            TargetTree.View.Add(CopyTreeView(view));

            Result = false;
        }

        public void TreeViewSelectedItemChangedCallback(DependencyPropertyEventArgs e)
        {
            TargetTree.SelectedItem = e.DependencyPropertyChangedEventArgs.NewValue as TreeViewItem;
            
        }

        public CommandBase SelectTargetCommand { get; set; }

        private void InitializeCommands()
        {
            SelectTargetCommand = new CommandBase(SelectTargetCommand_Executed);
        }

        private void SelectTargetCommand_Executed(object sender, EventArgs e)
        {
            SelectedTarget = TargetTree.SelectedItem.Tag;

            Result = true;
            CloseAction();
        }

        public void SelectTarget()
        {
            SelectTargetCommand_Executed(null, null);
        }

        private TreeView CopyTreeView(TreeView view)
        {
            var ret = new TreeView();

            foreach (TreeViewItem item in view.Items)
            {
                ret.Items.Add(RecursiveCopyTreeView(item));
            }

            return ret;
        }

        private TreeViewItem RecursiveCopyTreeView(TreeViewItem item)
        {
            var ret = new TreeViewItem();
            ret.Header = item.Header;
            ret.Tag = item.Tag;

            foreach (TreeViewItem subItem in item.Items)
            {
                ret.Items.Add(RecursiveCopyTreeView(subItem));
            }
            
            return ret;
        }
    }
}

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using TiaPortalOpennessDemo.Commands;

namespace TiaPortalOpennessDemo.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    /// TODO Edit XML Comment Template for TreeViewHandler
    public class TreeViewHandler
    {
        /// <summary>Gets the view.</summary>
        /// <value>The view.</value>
        /// TODO Edit XML Comment Template for View
        public ObservableCollection<TreeView> View { get; private set; }
        /// <summary>Gets or sets the selected item.</summary>
        /// <value>The selected item.</value>
        /// TODO Edit XML Comment Template for SelectedItem
        public TreeViewItem SelectedItem { get; set; }
        /// <summary>Gets or sets the selected item CHG command.</summary>
        /// <value>The selected item CHG command.</value>
        /// TODO Edit XML Comment Template for SelectedItemChgCmd
        public RelayCommand<DependencyPropertyEventArgs> SelectedItemChgCmd { get; set; }
        /// <summary>Gets or sets the selected item CHG variable command.</summary>
        /// <value>The selected item CHG variable command.</value>
        /// TODO Edit XML Comment Template for SelectedItemChgVarCmd
        public RelayCommand<DependencyPropertyEventArgs> SelectedItemChgVarCmd { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewHandler"/> class.
        /// </summary>
        /// <param name="chgCmd">The CHG command.</param>
        /// TODO Edit XML Comment Template for #ctor
        public TreeViewHandler(RelayCommand<DependencyPropertyEventArgs> chgCmd)
        {
            SelectedItemChgCmd = chgCmd;
            SelectedItemChgVarCmd = chgCmd;
            SelectedItem = new TreeViewItem();
            View = new ObservableCollection<TreeView>();
        }

        /// <summary>Refreshes the specified view.</summary>
        /// <param name="view">The view.</param>
        /// TODO Edit XML Comment Template for Refresh
        public void Refresh(TreeView view)
        {
            if (View.Count != 0)
            {
                var treeState = SaveTreeState(View[0]);
                View.Clear();
                View.Add(view);
                RestoreTreeState(View[0], treeState);
            }
            else
            {
                View.Clear();
                View.Add(view);
            }
            SelectedItem = new TreeViewItem();
        }

        /// <summary>Saves the state of the tree.</summary>
        /// <param name="view">The view.</param>
        /// <returns></returns>
        /// TODO Edit XML Comment Template for SaveTreeState
        private Dictionary<string, bool> SaveTreeState(ItemsControl view)
        {
            var nodeStates = new Dictionary<string, bool>();

            foreach (var item in view.Items)
            {
                var currentContainer = view.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                if (currentContainer != null && currentContainer.Items.Count > 0)
                {
                    // Expand the current item. 
                    try
                    {
                        nodeStates.Add(currentContainer.Header.ToString(), currentContainer.IsExpanded);
                    }
                    catch
                    {
                        // ignored
                    }
                    SaveTreeState(currentContainer).ToList().ForEach(x => nodeStates[x.Key] = x.Value);
                }
            }

            return nodeStates;
        }

        /// <summary>Restores the state of the tree.</summary>
        /// <param name="view">The view.</param>
        /// <param name="treeState">State of the tree.</param>
        /// TODO Edit XML Comment Template for RestoreTreeState
        private void RestoreTreeState(ItemsControl view, Dictionary<string, bool> treeState)
        {
            foreach (TreeViewItem currentContainer in view.Items)
            {
                if (currentContainer != null && currentContainer.HasItems)
                {
                    if (treeState.ContainsKey(currentContainer.Header.ToString()))
                        currentContainer.IsExpanded = treeState[currentContainer.Header.ToString()];
                    RestoreTreeState(currentContainer, treeState);
                }
            }
        }
    }
}

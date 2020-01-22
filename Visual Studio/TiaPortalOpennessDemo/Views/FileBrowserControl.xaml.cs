using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace TiaPortalOpennessDemo.Views
{
    /// <summary>
    /// Interaction logic for FolderBrowserControl.xaml
    /// </summary>
    public partial class FileBrowserControl
    {
        #region TextBox DP
        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        public static readonly DependencyProperty PathProperty = 
            DependencyProperty.Register("Path", typeof(string), 
                typeof(FileBrowserControl), new PropertyMetadata(""));
        #endregion
        #region Filter DP
        public string Filter
        {
            get { return (string)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register("Filter", typeof(string),
                typeof(FileBrowserControl), new PropertyMetadata(""));
        #endregion
        public FileBrowserControl()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }

        private void BrowseFolder(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = string.IsNullOrEmpty(Filter) == false ? Filter : "All files (*.*)|*.*",
                InitialDirectory = string.IsNullOrEmpty(Path) == false && Directory.Exists(Path) ? Path : @"C:\"
            };
            if (dlg.ShowDialog() == true)
            {
                Path = dlg.FileName;
            }
        }
    }
}

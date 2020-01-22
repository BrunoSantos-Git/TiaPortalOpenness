using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TiaOpennessHelper.ExcelTree;
using TiaPortalOpennessDemo.ViewModels;

namespace TiaPortalOpennessDemo.Views
{
    public sealed partial class MainWindow
    {
        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            this.Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object sender, System.EventArgs e)
        {
            App.Current.Shutdown();
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var path = MainFolderPath.Text;
            if (e.ClickCount == 2)
                Process.Start(path);
        }
    }
}

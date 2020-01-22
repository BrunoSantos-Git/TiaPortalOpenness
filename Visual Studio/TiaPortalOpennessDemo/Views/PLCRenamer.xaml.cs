using Siemens.Engineering;
using System.Windows;
using TiaPortalOpennessDemo.Utilities;
using TiaPortalOpennessDemo.ViewModels;

namespace TiaPortalOpennessDemo.Views
{
    /// <summary>
    /// Interaction logic for PLCRenamer.xaml
    /// </summary>
    public partial class PLCRenamer : Window
    {
        public PLCRenamer()
        {
            DataContext = new PLCRenamerViewModel();
            InitializeComponent();
        }

        public PLCRenamer(string exportPath, bool exportOptionsDefaults, bool exportOptionsReadOnly, TiaPortal tiaPortal, Project tiaPortalProject, string mainFolderPath)
        {
            DataContext = new PLCRenamerViewModel(exportPath, exportOptionsDefaults, exportOptionsReadOnly, tiaPortal, tiaPortalProject, mainFolderPath);
            InitializeComponent();
        }
    }
}

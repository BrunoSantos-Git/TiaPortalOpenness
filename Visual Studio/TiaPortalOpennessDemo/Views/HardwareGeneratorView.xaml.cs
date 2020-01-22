using Siemens.Engineering;
using System.Collections.Generic;
using System.Windows;
using TiaPortalOpennessDemo.ViewModels;

namespace TiaPortalOpennessDemo.Views
{
    /// <summary>
    /// Interaction logic for HardwareGeneratorView.xaml
    /// </summary>
    public partial class HardwareGeneratorView : Window
    {
        public HardwareGeneratorView()
        {
            DataContext = new HardwareGeneratorViewModel();
            InitializeComponent();
        }

        public HardwareGeneratorView(string networkListPath, string eplanPath, TiaPortal tiaPortal, Project tiaPortalProject)
        {
            DataContext = new HardwareGeneratorViewModel(networkListPath, eplanPath, tiaPortal, tiaPortalProject);
            InitializeComponent();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TiaPortalOpennessDemo.Views
{
    /// <summary>
    /// Interaction logic for ImportCaxControl.xaml
    /// </summary>
    public partial class ImportCaxControl : UserControl
    {
        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }

        public static readonly DependencyProperty FilePathProperty =
            DependencyProperty.Register("FilePath", typeof(string),
                typeof(ImportCaxControl), new PropertyMetadata(""));

        public string SelectedOption
        {
            get { return (string)GetValue(SelectedOptionProperty); }
            set { SetValue(SelectedOptionProperty, value); }
        }

        public static readonly DependencyProperty SelectedOptionProperty =
            DependencyProperty.Register("SelectedOption", typeof(TiaOpennessHelper.ImportCaxOptions),
                typeof(ImportCaxControl), new PropertyMetadata(TiaOpennessHelper.ImportCaxOptions.MoveToParkingLot));

        public string CancelCommand
        {
            get { return (string)GetValue(CancelCommandProperty); }
            set { SetValue(CancelCommandProperty, value); }
        }

        public static readonly DependencyProperty CancelCommandProperty =
            DependencyProperty.Register("CancelCommand", typeof(ICommand),
                typeof(ImportCaxControl), new PropertyMetadata(null));

        public string ImportCommand
        {
            get { return (string)GetValue(ImportCommandProperty); }
            set { SetValue(ImportCommandProperty, value); }
        }

        public static readonly DependencyProperty ImportCommandProperty =
            DependencyProperty.Register("ImportCommand", typeof(ICommand),
                typeof(ImportCaxControl), new PropertyMetadata(null));


        public ImportCaxControl()
        {
            InitializeComponent();
        }
    }
}

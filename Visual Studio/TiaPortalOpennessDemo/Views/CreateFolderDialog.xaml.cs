using System.Windows;

namespace TiaPortalOpennessDemo.Views
{
    public sealed partial class CreateFolderDialog
    {
        /// <summary>
        /// 
        /// </summary>
        public string FolderName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CreateFolderDialog()
        {
            InitializeComponent();
        }

        private void btn_Okay_Click(object sender, RoutedEventArgs e)
        {
            FolderName = TextBox1.Text;
            DialogResult = true;
            Close();
        }
    }
}

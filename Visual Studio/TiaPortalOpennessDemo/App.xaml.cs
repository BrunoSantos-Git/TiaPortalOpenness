using System;
using System.IO;
using System.Windows;
using TiaOpennessHelper.Utils;
using TiaPortalOpennessDemo.Properties;
using TiaPortalOpennessDemo.ViewModels;
using TiaPortalOpennessDemo.Views;

namespace TiaPortalOpennessDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            AppDomain.CurrentDomain.AssemblyResolve += Resolver.OnResolve;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Resolver.GetAssemblyPath("15.1", "15.1.0.0");
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}

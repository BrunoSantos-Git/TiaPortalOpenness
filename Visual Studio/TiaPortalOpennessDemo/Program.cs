using System;
using System.Globalization;
using System.Reflection;

namespace TiaPortalOpennessDemo
{
    /// <summary>
    /// 
    /// </summary>
    /// TODO Edit XML Comment Template for Program
    public sealed class Program
    {
        /// <summary>Defines the entry point of the application.</summary>
        /// TODO Edit XML Comment Template for Main
        [STAThread]
        public static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;
            App.Main();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;

            System.Windows.MessageBox.Show(ex.Message);
        }

        /// <summary>
        /// Called when [resolve assembly].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="ResolveEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        /// TODO Edit XML Comment Template for OnResolveAssembly
        private static Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var assemblyName = new AssemblyName(args.Name);

            var path = assemblyName.Name + ".dll";
            if (assemblyName.CultureInfo.Equals(CultureInfo.InvariantCulture) == false)
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}\{1}", assemblyName.CultureInfo, path);
            }

            using (var stream = executingAssembly.GetManifestResourceStream(path))
            {
                if (stream == null)
                    return null;

                var assemblyRawBytes = new byte[stream.Length];
                stream.Read(assemblyRawBytes, 0, assemblyRawBytes.Length);
                return Assembly.Load(assemblyRawBytes);
            }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="Program"/> class from being created.
        /// </summary>
        /// TODO Edit XML Comment Template for #ctor
        private Program() { }
    }
}

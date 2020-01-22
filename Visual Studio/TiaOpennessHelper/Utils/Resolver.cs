using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TiaOpennessHelper.Utils
{
    public static class Resolver
    {
        //OLD
        //private const string BASE_PATH = "SOFTWARE\\Siemens\\Automation\\Openness\\";
        private const string BASE_PATH = "SOFTWARE\\Wow6432Node\\Siemens\\Automation\\Openness\\";
        private static string AssemblyPath = "";
		private static string AssemblyPathHmi = "";

        public static List<string> GetEngineeringVersions()
        {
            RegistryKey key = GetRegistryKey(BASE_PATH);

            if (key != null)
            {
                var names = key.GetSubKeyNames().OrderBy(x => x).ToList();
                key.Dispose();

                return names;
            }

            return new List<string>();
        }

        public static List<string> GetAssmblies(string version)
        {
            RegistryKey key = GetRegistryKey(BASE_PATH + version);

            if (key != null)
            {
                try
                {
                    var subKey = key.OpenSubKey("PublicAPI");

                    var result = subKey.GetSubKeyNames().OrderBy(x => x).ToList();

                    subKey.Dispose();

                    return result;
                }
                finally
                {
                    key.Dispose();
                }
            }

            return new List<string>();
        }

        public static string GetAssemblyPath(string version, string assembly)
        {
            RegistryKey key = GetRegistryKey(BASE_PATH + version + "\\PublicAPI\\" + assembly);

            if(key != null)
            {
                try
                {
                    AssemblyPath = key.GetValue("Siemens.Engineering").ToString();
                    AssemblyPathHmi = key.GetValue("Siemens.Engineering.Hmi").ToString();
                    
                    return AssemblyPath;
                }
                finally
                {
                    key.Dispose();
                }
            }

            return null;
        }

        private static RegistryKey GetRegistryKey(string keyname)
        {
            RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey key = baseKey.OpenSubKey(keyname);
            if (key == null)
            {
                baseKey.Dispose();
                baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default);
                key = baseKey.OpenSubKey(keyname);
            }
            if (key == null)
            {
                baseKey.Dispose();
                baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                key = baseKey.OpenSubKey(keyname);
            }
            baseKey.Dispose();

            return key;
        }

        public static Assembly OnResolve(object sender, ResolveEventArgs args)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var assemblyName = new AssemblyName(args.Name);
            string path = "";
            
            if (assemblyName.Name.EndsWith("Siemens.Engineering"))
                path = AssemblyPath;
            if (assemblyName.Name.EndsWith("Siemens.Engineering.Hmi"))
                path = AssemblyPathHmi;
            
            if (string.IsNullOrEmpty(path) == false
                && File.Exists(path))
            {
                return Assembly.LoadFrom(path);
            }

            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiaOpennessHelper.Utils
{
    public class FolderInfo
    {
        /// <summary>
        /// Folder Name
        /// </summary>
        public string fName { get; private set; }
        /// <summary>
        /// Folder Path
        /// </summary>
        public string fPath { get; private set; }

        /// <summary>
        /// Costructor
        /// </summary>
        /// <param name="fName"></param>
        /// <param name="fPath"></param>
        public FolderInfo(string fName, string fPath)
        {
            this.fName = fName;
            this.fPath = fPath;
        }
    }
}

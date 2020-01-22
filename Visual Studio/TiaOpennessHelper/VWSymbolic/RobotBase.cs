using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiaOpennessHelper.VWSymbolic
{
    public class RobotBase
    {
        public string Symbolic { get; set; }
        public string DataType { get; set; }
        public string Address { get; set; }
        public string Comment { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="symbolic"></param>
        /// <param name="dataType"></param>
        /// <param name="address"></param>
        /// <param name="comment"></param>
        public RobotBase(string symbolic, string dataType, string address, string comment)
        {
            Symbolic = symbolic;
            DataType = dataType;
            Address = address;
            Comment = comment;
        }
    }
}

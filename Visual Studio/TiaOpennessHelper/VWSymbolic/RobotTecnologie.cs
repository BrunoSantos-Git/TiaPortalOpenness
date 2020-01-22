using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiaOpennessHelper.VWSymbolic
{
    public class RobotTecnologie : RobotBase
    {
        public string Name { get; set; }
        public string FBNumber { get; set; }
        public string Type { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="symbolic"></param>
        /// <param name="dataType"></param>
        /// <param name="address"></param>
        /// <param name="comment"></param>
        /// <param name="fbNumber"></param>
        public RobotTecnologie(string fbNumber, string name, string type, string symbolic, string dataType, string address, string comment) : base(symbolic, dataType, address, comment)
        {
            FBNumber = fbNumber;
            Name = name;
            Type = type;
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public RobotTecnologie() : this("", "", "", "", "", "", "") { }
    }
}

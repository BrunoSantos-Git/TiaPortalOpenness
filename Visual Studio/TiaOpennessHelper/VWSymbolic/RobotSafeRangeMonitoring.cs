using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiaOpennessHelper.VWSymbolic
{
    public class RobotSafeRangeMonitoring : RobotBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Symbolic"></param>
        /// <param name="dataType"></param>
        /// <param name="address"></param>
        /// <param name="comment"></param>
        public RobotSafeRangeMonitoring(string Symbolic, string dataType, string address, string comment) : base(Symbolic, dataType, address, comment)
        {
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public RobotSafeRangeMonitoring() : this("", "", "", "")
        {
        }
    }
}

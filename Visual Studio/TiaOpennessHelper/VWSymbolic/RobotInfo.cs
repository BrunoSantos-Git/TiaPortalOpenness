using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiaOpennessHelper.VWSymbolic
{
    public class RobotInfo
    {
        public string Name;
        public string Safe;
        public int StartAddress;
        public string Tecnologies;
        public string Type;

        public RobotInfo(string name, string safe, int startAddress, string tecnologies, string type)
        {
            this.Name = name;
            this.Safe = safe;
            this.StartAddress = startAddress;
            this.Tecnologies = tecnologies;
            this.Type = type;
        }
    }
}

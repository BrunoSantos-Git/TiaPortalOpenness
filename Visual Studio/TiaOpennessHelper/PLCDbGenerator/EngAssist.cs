using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiaOpennessHelper.SafetyMaker
{
    public class EngAssist
    {
        public string Arbeitsgruppe_ARG { get; set; }
        public string Schutzkreis_SK { get; set; }
        public string Station { get; set; }
        public string Erw_Stationsbez_SBZ{ get; set; }
        public List<string> Parts;
        public List<string> Valves;

        /// <summary>
        /// Constructer of the values of a row in in the EngAssist worksheet
        /// </summary>
        /// <param name="Arbeitsgruppe_ARG">Its the "Arbeitsgruppe [ARG]" value</param>
        /// <param name="Schutzkreis_SK">Its the "Schutzkreis [SK]" value</param>
        /// <param name="Station">Its the "Station" value</param>
        /// <param name="Erw_Stationsbez_SBZ">Its the "Erw. Stationsbez. [SBZ]" value</param>
        public EngAssist(string Arbeitsgruppe_ARG, string Schutzkreis_SK, string Station, string Erw_Stationsbez_SBZ)
        {
            this.Arbeitsgruppe_ARG = Arbeitsgruppe_ARG;
            this.Schutzkreis_SK = Schutzkreis_SK;
            this.Station = Station;
            this.Erw_Stationsbez_SBZ = Erw_Stationsbez_SBZ;
            Parts = new List<String>();
            Valves = new List<String>();
        } 
    }
}

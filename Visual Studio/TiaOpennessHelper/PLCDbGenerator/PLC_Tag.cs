using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiaOpennessHelper.SafetyMaker
{
    public class PLC_Tag
    {
        public string Name{get; set;}
        public string Symbols{get; set;}
        public string DataType{get; set;}
        public string Address{get; set;}
        public string Comment{get; set;}
        public bool Accessible{get; set;}
        public bool Writable {get; set;}
        public bool Visible {get; set;}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiaOpennessHelper.ExcelTree
{
    public class WorkSheet
    {
        public string WorkSheetName { get; set; }
        public List<Step> WorkSheetSteps { get; set; }
    }
}

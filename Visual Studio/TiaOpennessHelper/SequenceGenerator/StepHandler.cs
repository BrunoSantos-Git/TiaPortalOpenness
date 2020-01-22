using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Excel = Microsoft.Office.Interop.Excel;

namespace TiaOpennessHelper.ExcelTree
{
    public class StepHandler
    {
        public static List<List<StepHandler>> GrafcetList = new List<List<StepHandler>>();
        public List<string> StepActions { get; set; }
        public List<string> PreviousSteps { get; set; }
        public List<string> NextSteps { get; set; }
        public string SheetName { get; set; }
        public string BranchNumberEnd { get; set; }
        public string BranchNumberBegin { get; set; }
        public string StepName { get; set; }
        public string StepDescription { get; set; }
        public string StepNumber { get; set; }
        public string StepTime { get; set; }
        public static List<StepHandler> StepList = new List<StepHandler>();
        public static int MultilingualCounter = 30;
        public static int CounterUId = 1;

        //Step Builder
        /// <summary>
        /// Class where the steps and all the variables are stored
        /// </summary>
        /// <param name="StepName"></param>
        /// <param name="SheetName">Variable used to know whats the sheet's name from where this step is</param>
        /// <param name="StepDescription">Variable used to know what is the steps description</param>
        /// <param name="StepNumber">Variable used to know what is the steps number</param>
        /// <param name="StepTime">Variable used to know what is the steps time</param>
        public StepHandler(string StepNumber, string StepName, string StepDescription, string StepTime, string SheetName)
        {
            this.StepNumber = StepNumber;
            this.StepName = StepName;
            this.StepDescription = StepDescription;
            this.StepTime = StepTime;
            this.SheetName = SheetName;
            StepActions = new List<string>();
            PreviousSteps = new List<string>();
            NextSteps = new List<string>();
            BranchNumberEnd = "";
            BranchNumberBegin = "";
        }
    }
}

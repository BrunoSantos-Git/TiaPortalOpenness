namespace TiaOpennessHelper.ExcelTree
{
    public class Step
    {
        public int StepNumber { get; set; }
        public string Schritt { get; set;}
        public string Beschreibung { get; set; }
        public string Aktion { get; set; }
        public string Vorheriger_Schritt { get; set; }
        public string Nächster_Schritt { get; set; }
        public string Zeit_Schritt { get; set; }
    }
}

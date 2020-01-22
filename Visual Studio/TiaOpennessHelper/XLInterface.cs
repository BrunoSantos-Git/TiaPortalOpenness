using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using TiaOpennessHelper.VWSymbolic;
using Excel = Microsoft.Office.Interop.Excel;

public struct SequenceData
{
    public string ComponentType;
    public string ComponentName;
    public string Description;
    public string Action;
    public int stepNr;
    public bool simultaneousStepsFlag;
    public int Nsimultaneous;
    public bool Last;
}

public struct AddressData
{
    public string partName;
    public string IPAdress;
    public string StartAddress;
}

public struct DeviceData
{
    public string terminalType;
    public string deviceName;
    public string FGroup;
    public bool option;
    public string identifier;
    public AddressData addressData;
}

namespace TiaOpennessHelper
{
    public partial class OpennessHelper
    {
        /// <summary>
        /// Import Excel File
        /// </summary>
        /// <param name="path"></param>
        /// <param name="xlApp"></param>
        /// <returns></returns>
        public static Workbook GetExcelFile(string path, Application xlApp)
        {
            if (path == "") return null;
            xlApp.DisplayAlerts = false;
            return xlApp.Workbooks.Open(path, 0, true, 5, "", "", true, XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
        }

        /// <summary>
        /// Get column letter by column number
        /// </summary>
        /// <param name="colIndex"></param>
        /// <returns>Column letter</returns>
        public static string ColumnIndexToColumnLetter(int colIndex)
        {
            int div = colIndex;
            string colLetter = String.Empty;
            while (div > 0)
            {
                int mod = (div - 1) % 26;
                colLetter = (char)(65 + mod) + colLetter;
                div = (div - mod) / 26;
            }
            return colLetter;
        }

        /// <summary>
        /// Transform excel sheet in a matrix
        /// </summary>
        /// <param name="xlWorksheet"></param>
        public static object[,] ExcelToMatrix(Worksheet xlWorksheet)
        {
            var lastRow = xlWorksheet.Cells.Find("*", System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                XlSearchOrder.xlByRows, XlSearchDirection.xlPrevious, false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Row;

            var lastCol = xlWorksheet.Cells.Find("*", System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                XlSearchOrder.xlByColumns, XlSearchDirection.xlPrevious, false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Column;

            var min = xlWorksheet.Range["A1"];
            var max = xlWorksheet.Range[OpennessHelper.ColumnIndexToColumnLetter(lastCol) + "" + lastRow];

            Range currentRange = xlWorksheet.get_Range(min, max).Cells;
            object[,] matrix = (object[,])currentRange.Value;

            return matrix;
        }

        /// <summary>
        /// Check first cell containing text and return row[0] and column[1] in a List
        /// </summary>
        /// <returns>List containing the row and column</returns>
        public static List<int> TextPosInRow(object[,] matrix, Range lastCellRange)
        {
            var pos = new List<int>();

            for (int row = 1; row <= lastCellRange.Row; row++)
            {
                for (int col = 1; col <= lastCellRange.Column; col++)
                {
                    //check if the cell is empty or not
                    if (matrix[row, col] != null)
                    {
                        pos.Add(row);
                        pos.Add(col);
                        break;
                    }
                }
            }

            return pos;
        }

        /// <summary>
        /// Search for a certain sheet name on excel file
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="xlWorksheet"></param>
        /// <returns>Sheet index</returns>
        public static int GetIndexOfSheet(string sheetName, Sheets xlWorksheet)
        {
            foreach (Worksheet sheet in xlWorksheet)
            {
                if (sheet.Name == sheetName)
                    return sheet.Index;
            }

            return 0;
        }

        #region Identify File Type
        /// <summary>
        /// Check if excel file is a sequence
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static bool IsSequence(object[,] matrix)
        {
            bool isSequence = false;

            string colShcritt = null;
            string colBeschreibung = null;
            string colAktion = null;
            string colVorheriger = null;
            string colnNachster = null;
            string colZeit = null;

            try
            {
                colShcritt = Convert.ToString(matrix[3, 3]);
                colBeschreibung = Convert.ToString(matrix[3, 4]);
                colAktion = Convert.ToString(matrix[3, 5]);
                colVorheriger = Convert.ToString(matrix[3, 6]);
                colnNachster = Convert.ToString(matrix[3, 7]);
                colZeit = Convert.ToString(matrix[3, 8]);
            }
            catch (Exception)
            {
                // Continue;
            }

            if (colShcritt == null || colBeschreibung == null || colAktion == null || colVorheriger == null || colnNachster == null || colZeit == null) isSequence = false;

            if (colShcritt.ToLower().Contains("schritt") && colBeschreibung.ToLower().Contains("beschreibung") && 
            colAktion.ToLower().Contains("aktion") && colVorheriger.ToLower().Contains("vorheriger") && 
            colnNachster.ToLower().Contains("nächster") && colZeit.ToLower().Contains("zeit"))
            {
                isSequence = true;
            }

            return isSequence;
        }

        /// <summary>
        /// Check if excel file is a PLC Database
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static bool IsPlcDb(object[,] matrix)
        {
            bool isPlcDb = false;

            string colGenerate = null;
            string colArbeitsgruppe = null;
            string colSchutzkreis = null;
            string colStation = null;
            string colErw = null;

            try
            {
                colGenerate = Convert.ToString(matrix[2, 1]);
                colArbeitsgruppe = Convert.ToString(matrix[2, 2]);
                colSchutzkreis = Convert.ToString(matrix[2, 3]);
                colStation = Convert.ToString(matrix[2, 4]);
                colErw = Convert.ToString(matrix[2, 5]);
            }
            catch (Exception)
            {
                // Continue;
            }

            if (colGenerate == null || colArbeitsgruppe == null || colSchutzkreis == null || colStation == null || colErw == null) isPlcDb = false;

            if (colGenerate.ToLower().Contains("generate") && colArbeitsgruppe.ToLower().Contains("arbeitsgruppe") &&
            colSchutzkreis.ToLower().Contains("schutzkreis") && colStation.ToLower().Contains("station") &&
            colErw.ToLower().Contains("erw."))
            {
                isPlcDb = true;
            }

            return isPlcDb;
        }

        /// <summary>
        /// Check if excel file is a Symbolic
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static bool IsSymbolic(object[,] matrix)
        {
            bool isSymbolic = false;

            string colGenerate = null;
            string colPage = null;
            string colStation = null;
            string colNummer = null;
            string colStartaddress = null;

            try
            {
                colGenerate = Convert.ToString(matrix[2, 1]);
                colPage = Convert.ToString(matrix[2, 2]);
                colStation = Convert.ToString(matrix[2, 3]);
                colNummer = Convert.ToString(matrix[2, 4]);
                colStartaddress = Convert.ToString(matrix[2, 5]);
            }
            catch (Exception)
            {
                // Continue;
            }

            if (colGenerate == null || colPage == null || colStation == null || colNummer == null || colStartaddress == null) isSymbolic = false;

            if (colGenerate.ToLower().Contains("generate") && colPage.ToLower().Contains("page") &&
            colStation.ToLower().Contains("station") && colNummer.ToLower().Contains("nummer") &&
            colStartaddress.ToLower().Contains("startaddress"))
            {
                isSymbolic = true;
            }

            return isSymbolic;
        }

        /// <summary>
        /// Check if excel file is a Robot WorkBook
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static bool IsSchnittstelle(object[,] matrix)
        {
            bool isRob = false;

            string colFolgen = null;
            string colRoboterausgangeZur = null;
            string colRoboterausgangeVon = null;
            string colFertigmeldungen = null;
            string colTecnologie = null;

            try 
            { 
                colFolgen = Convert.ToString(matrix[2, 2]);
                colRoboterausgangeZur = Convert.ToString(matrix[2, 7]);
                colRoboterausgangeVon = Convert.ToString(matrix[2, 13]);
                colFertigmeldungen = Convert.ToString(matrix[2, 20]);
                colTecnologie = Convert.ToString(matrix[2, 23]);
            }
            catch (Exception)
            {
                // Continue;
            }

            if (colFolgen == null || colRoboterausgangeZur == null || colRoboterausgangeVon == null || colFertigmeldungen == null || colTecnologie == null) isRob = false;

            if (colFolgen.ToLower().Contains("folgen") && colRoboterausgangeZur.ToLower().Contains("roboterausgänge zur sps") &&
            colRoboterausgangeVon.ToLower().Contains("robotereingänge von sps") && colFertigmeldungen.ToLower().Contains("fertigmeldungen") &&
            colTecnologie.ToLower().Contains("tecnologie"))
            {
                isRob = true;
            }

            return isRob;
        }

        /// <summary>
        /// Check if the excel file contains a Plc Tag sheet
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static bool IsPlcTags(object[,] matrix)
        {
            bool isPlcTags = false;

            string colName = null;
            string colPath = null;
            string colDataType = null;
            string colLogicalAddress = null;
            string colComment = null;

            try
            {
                colName = Convert.ToString(matrix[1, 1]);
                colPath = Convert.ToString(matrix[1, 2]);
                colDataType = Convert.ToString(matrix[1, 3]);
                colLogicalAddress = Convert.ToString(matrix[1, 4]);
                colComment = Convert.ToString(matrix[1, 5]);
            }
            catch (Exception)
            {
                // Continue;
            }

            if (colName == null || colPath == null || colDataType == null || colLogicalAddress == null || colComment == null) isPlcTags = false;

            if (colName.ToLower().Contains("name") && colPath.ToLower().Contains("path") &&
            colDataType.ToLower().Contains("data type") && colLogicalAddress.ToLower().Contains("logical address") &&
            colComment.ToLower().Contains("comment"))
            {
                isPlcTags = true;
            }

            return isPlcTags;
        }

        /// <summary>
        /// Check if the excel file is a NetWorkList
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static bool IsNetworkList(object[,] matrix)
        {
            bool isNL = false;

            string colProfinet = null;
            string colEndgeratetyp = null;
            string colOrts = null;
            string colFunktions = null;
            string colKlassifizierer = null;
            string colPNName1 = null;
            string colPNName2 = null;
            string colEndgeratename = null;
            string colBemerkung = null;
            string colStartaddress = null;

            try
            {
                colProfinet = Convert.ToString(matrix[5, 1]);
                colEndgeratetyp = Convert.ToString(matrix[5, 2]);
                colOrts = Convert.ToString(matrix[5, 3]);
                colFunktions = Convert.ToString(matrix[5, 4]);
                colKlassifizierer = Convert.ToString(matrix[5, 5]);
                colPNName1 = Convert.ToString(matrix[5, 6]);
                colPNName2 = Convert.ToString(matrix[5, 7]);
                colEndgeratename = Convert.ToString(matrix[5, 8]);
                colBemerkung = Convert.ToString(matrix[5, 9]);
                colStartaddress = Convert.ToString(matrix[5, 10]);
            } 
            catch(Exception)
            {
                // Continue
            }

            if (colProfinet == null || colEndgeratetyp == null || colOrts == null || colFunktions == null || colKlassifizierer == null ||
                colPNName1 == null || colPNName2 == null || colEndgeratename == null || colBemerkung == null || colStartaddress == null) 
                isNL = false;

            if (colProfinet.ToLower().Contains("profinet") && colEndgeratetyp.ToLower().Contains("endgerätetyp") &&
            colOrts.ToLower().Contains("orts-") && colFunktions.ToLower().Contains("funktions-") &&
            colKlassifizierer.ToLower().Contains("klassifizierer") && colPNName1.ToLower().Contains("pn-name 1") &&
            colPNName2.ToLower().Contains("pn-name 2") && colEndgeratename.ToLower().Contains("endgerätename") &&
            colBemerkung.ToLower().Contains("bemerkung") && colStartaddress.ToLower().Contains("startadresse"))
            {
                isNL = true;
            }

            return isNL;
        }

        /// <summary>
        /// Check file type
        /// </summary>
        /// <param name="path"></param>
        /// <returns>file type</returns>
        public static string CheckFileType(string path)
        {
            Worksheet EngAssist = null;
            Application xlApp = new Application();
            Workbook xlWorkbook = null;
            string[] extensions = new[] { ".xml", ".xls", ".xlsx", ".xlsm" };
            string fileExtension = Path.GetExtension(path);
            if (extensions.Contains(Path.GetExtension(path)))
            {
                try
                {
                    xlWorkbook = GetExcelFile(path, xlApp);
                }
                catch (Exception)
                {
                    return null;
                }

                bool engAssistExist = true;

                try
                {
                    EngAssist = xlWorkbook.Sheets["EngAssist"];
                }
                catch (Exception)
                {
                    engAssistExist = false;
                }

                if (engAssistExist)
                {
                    var matrix = ExcelToMatrix(EngAssist);

                    if (IsPlcDb(matrix))
                    {
                        xlWorkbook.Close(0);
                        xlApp.Quit();
                        return "plcDB";
                    }

                    if (IsSymbolic(matrix))
                    {
                        xlWorkbook.Close(0);
                        xlApp.Quit();
                        return "symbolic";
                    }
                }
                else
                {
                    foreach (Worksheet xlWorksheet in xlWorkbook.Worksheets)
                    {
                        string sheetName = xlWorksheet.Name;
                        if (!sheetName.Contains("AS_")) break;

                        object[,] matrix;

                        matrix = ExcelToMatrix(xlWorksheet);

                        if (IsSequence(matrix))
                        {
                            xlWorkbook.Close(0);
                            xlApp.Quit();
                            return "sequence";
                        }
                    }
                }

                xlWorkbook.Close(0);
                xlApp.Quit();
            }

            return null;
        }
        #endregion

        /// <summary>
        /// Check if excel sheets names contains a certain PLC Number
        /// </summary>
        /// <param name="sheetsNames"></param>
        /// <param name="plcNum"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsCorrectPlcNumber(List<string> sheetsNames, string plcNum, string type)
        {
            bool isCorrectPlcNumber = false;

            if (type == "Sequence")
            {
                foreach (string s in sheetsNames)
                {
                    if (s[3].ToString() == plcNum)
                    {
                        isCorrectPlcNumber = true;
                        break;
                    }
                }
            }
            else
            {
                foreach (string s in sheetsNames)
                {
                    if (s[0].ToString() == plcNum)
                    {
                        isCorrectPlcNumber = true;
                        break;
                    }
                }
            }

            return isCorrectPlcNumber;
        }

        /// <summary>
        /// Check if a directory exists and if it doesnt creates it
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static void DirectoryExists(string path)
        {
            bool Exists = Directory.Exists(path);
            if (!Exists) Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Identify file "ket" and return folder to save file
        /// </summary>
        /// <returns></returns>
        public static string CreateKetFolder(string path, string type)
        {
            string[] names = new[] { "kethil", "kethir", "ketvol", "ketvor" };

            // Return nome pasta (Ket)
            string ket;
            string fileName = Path.GetFileName(path);

            ket = names.Where(item => fileName.ToLower().Contains(item)).FirstOrDefault();

            if (string.IsNullOrEmpty(ket))
            {
                if (type == "plcDB")
                {
                    try
                    {
                        Application xlApp = new Application();
                        xlApp.DisplayAlerts = false;
                        Workbook xlWorkbook = xlApp.Workbooks.Open(path);
                        Worksheet ws = xlWorkbook.Sheets["User Config"];
                        ket = ws.Cells[4, 2].Value.ToString();
                    }
                    catch (Exception)
                    {
                        // Continue
                    }
                }
            }
            else
            {
                // Get Ketvol name
                int z = 0;

                while (z + ket.Length - 1 < fileName.Length)
                {
                    if (fileName.Substring(z, ket.Length) == ket.ToString().ToUpper())
                    {
                        string number = "";
                        int zz = 0;

                        while (char.IsDigit(fileName[z + ket.Length + zz]))
                        {
                            number += fileName[z + ket.Length + zz];
                            zz += 1;
                        }

                        ket = fileName.Substring(z, ket.Length) + number;
                        break;
                    }

                    z += 1;
                }
            }
            
            return ket;
        }

        #region Network List Excel File
        /// <summary>
        /// Get all devices from Network List Excel File
        /// </summary>
        /// <param name="xlWorksheet"></param>
        /// <returns></returns>
        public static List<DeviceData> GetAllDevicesNetworkList(Worksheet xlWorksheet)
        {
            List<DeviceData> DeviceDataList = new List<DeviceData>();
            DeviceData deviceData = new DeviceData();
            AddressData addressData = new AddressData();
            int row = 7; // DEFS for IPAddress Table 
            int rowMAX = 516;
            string identifier, IPType;
            Range xlRange = xlWorksheet.UsedRange;

            for (int i = row; i < rowMAX; i++)
            {
                identifier = ((xlRange.Cells[i, 2] as Range).Value)?.ToString() ?? "";

                if ((xlRange.Cells[i, 1] as Range).Value.ToString().Contains("DHCP"))
                    IPType = ((xlRange.Cells[i, 1] as Range).Value).ToString();
                else
                    IPType = "IPADDR";

                if (identifier != "Gateway" && identifier != "Router" && identifier != "Broadcastadresse" && identifier != "" && IPType == "IPADDR" && (xlRange.Cells[i, 4] as Range).Value != null)
                {
                    deviceData.option = false;
                    addressData.IPAdress = (xlRange.Cells[i, 1] as Range).Value?.ToString() ?? "";
                    deviceData.FGroup = (xlRange.Cells[i, 4] as Range).Value?.ToString() ?? "";
                    deviceData.identifier = (xlRange.Cells[i, 5] as Range).Value?.ToString() ?? "";
                    deviceData.deviceName = (xlRange.Cells[i, 6] as Range).Value?.ToString() ?? "";
                    addressData.StartAddress = (xlRange.Cells[i, 10] as Range).Value?.ToString() ?? "";
                    deviceData.addressData = addressData;
                    deviceData.terminalType = identifier;

                    // If Excel cell value has an error in formula the return value will be -2146826259 (INT)
                    var intFGroup = int.TryParse(deviceData.FGroup, out _);
                    var intIdentifier = int.TryParse(deviceData.identifier, out _);
                    var intDeviceName = int.TryParse(deviceData.deviceName, out _);
                    var intStartAddress = int.TryParse(addressData.StartAddress, out _);
                    var intTerminalType = int.TryParse(deviceData.terminalType, out _);

                    // Check if any value has only numbers on it
                    if (intFGroup || intIdentifier || intDeviceName || intStartAddress || intTerminalType) continue;

                    //Check on the second column if has Right in the name string, option will be used later to generate right or left door
                    if ((deviceData.deviceName.Contains("stu") && deviceData.terminalType.Contains("Right")) ||
                        (deviceData.deviceName.Contains("ls-") && deviceData.terminalType.Contains("SickS3000")))
                        deviceData.option = true;

                    DeviceDataList.Add(deviceData);
                }
            }
            return DeviceDataList;
        }
        #endregion

        #region VWSymbolism Excel File
        /// <summary>
        /// Get robot base properties from sheet
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="lastCellRange"></param>
        /// <returns>List containing robot inputs[0] and outputs[1]</returns>
        public static List<List<RobotBase>> GetRobotBase(object[,] matrix, Range lastCellRange)
        {
            var Symbols = new List<List<RobotBase>>();
            var RobotProperties = new List<RobotBase>();
            List<int> FirstTextCell = OpennessHelper.TextPosInRow(matrix, lastCellRange);
            int row = FirstTextCell[0];
            int col = FirstTextCell[1];
            int colSymbol = 0;
            int colDataType = 0;
            int colAddress = 0;
            int colComment = 0;

            #region Get outputs
            // Get column numbers
            for (int i = col; i <= lastCellRange.Column; i++)
            {
                string cellVal = Convert.ToString(matrix[row, i]);

                if (string.IsNullOrEmpty(cellVal)) break;
                else if (cellVal.ToLower() == "symbol") colSymbol = i;
                else if (cellVal.ToLower() == "datentyp") colDataType = i;
                else if (cellVal.ToLower() == "adresse") colAddress = i;
                else if (cellVal.ToLower() == "kommentar") colComment = i;
            }

            // Get robot properties
            for (int i = row + 1; i <= lastCellRange.Row; i++)
            {
                string cellVal = Convert.ToString(matrix[i, colSymbol]);
                if (!string.IsNullOrEmpty(cellVal))
                {
                    string symbol = matrix[i, colSymbol].ToString();
                    string dataType = matrix[i, colDataType].ToString();
                    string address = matrix[i, colAddress].ToString();
                    string comment = matrix[i, colComment].ToString();

                    RobotProperties.Add(new RobotBase(symbol, dataType, address, comment));
                }
            }
            #endregion region

            Symbols.Add(RobotProperties);
            RobotProperties = new List<RobotBase>();  // RESET LIST

            #region Get inputs
            // Get inputs start column 
            for (int i = col; i <= lastCellRange.Column; i++)
            {
                if (string.IsNullOrEmpty(Convert.ToString(matrix[row, i])))
                {
                    col = i + 1;
                    break;
                }
            }

            // Get column numbers
            for (int i = col; i <= lastCellRange.Column; i++)
            {
                string cellVal = Convert.ToString(matrix[row, i]);

                if (string.IsNullOrEmpty(cellVal)) break;
                else if (cellVal.ToLower() == "symbol") colSymbol = i;
                else if (cellVal.ToLower() == "datentyp") colDataType = i;
                else if (cellVal.ToLower() == "adresse") colAddress = i;
                else if (cellVal.ToLower() == "kommentar") colComment = i;
            }

            // Get robot properties
            for (int i = row + 1; i <= lastCellRange.Row; i++)
            {
                string cellVal = Convert.ToString(matrix[i, colSymbol]);
                if (!string.IsNullOrEmpty(cellVal))
                {
                    string symbol = matrix[i, colSymbol].ToString();
                    string dataType = matrix[i, colDataType].ToString();
                    string address = matrix[i, colAddress].ToString();
                    string comment = matrix[i, colComment].ToString();

                    RobotProperties.Add(new RobotBase(symbol, dataType, address, comment));
                }
            }
            #endregion

            Symbols.Add(RobotProperties);

            return Symbols;
        }

        /// <summary>
        /// Get robot teclonogies from sheet
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="lastCellRange"></param>
        /// <param name="type"></param>
        /// <returns>List with the input properties[0] and output properties[1]</returns>
        public static List<List<RobotTecnologie>> GetRobotTecnologies(object[,] matrix, Range lastCellRange, string type)
        {
            var Tecnologies = new List<List<RobotTecnologie>>();
            var RobotTecnologieProperties = new List<RobotTecnologie>();
            List<int> FirstTextCell = OpennessHelper.TextPosInRow(matrix, lastCellRange);
            int row = FirstTextCell[0];
            int col = FirstTextCell[1];
            int colSymbolInputs = 0;
            int colSymbolOutputs = 0;
            int colDataType = 0;
            int colAddress = 0;
            int colComment = 0;

            #region Get outputs
            // Get column numbers
            for (int i = col; i <= lastCellRange.Column; i++)
            {
                string cellVal = Convert.ToString(matrix[row, i]);

                if (string.IsNullOrEmpty(cellVal)) break;
                else if (cellVal.ToLower() == "symbolik") colSymbolOutputs = i;
                else if (cellVal.ToLower() == "datentyp") colDataType = i;
                else if (cellVal.ToLower() == "adresse") colAddress = i;
                else if (cellVal.ToLower() == "kommentar") colComment = i;
            }

            // Get tecnologie properties
            for (int i = row + 1; i <= lastCellRange.Row; i++)
            {
                string tecName = Convert.ToString(matrix[i, colSymbolOutputs]);

                if (!tecName.Contains("<<BMK>>") && !string.IsNullOrEmpty(tecName))  //If is a tecnologie name
                {
                    var name = Convert.ToString(matrix[i, colSymbolOutputs]);
                    var fbNr = Convert.ToString(matrix[i, lastCellRange.Column]);

                    for (int x = i + 1; x <= lastCellRange.Row; x++)
                    {
                        string cellVal = Convert.ToString(matrix[x, colSymbolOutputs]);

                        if (!string.IsNullOrEmpty(cellVal) && cellVal.Contains("<<BMK>>"))
                        {
                            string symbol = matrix[x, colSymbolOutputs].ToString();
                            string dataType = matrix[x, colDataType].ToString();
                            string address = matrix[x, colAddress].ToString();
                            string comment = matrix[x, colComment].ToString();

                            RobotTecnologieProperties.Add(new RobotTecnologie(fbNr, name, type, symbol, dataType, address, comment));
                        }
                        else break;
                        i++;
                    }
                }
            }
            #endregion

            Tecnologies.Add(RobotTecnologieProperties);
            RobotTecnologieProperties = new List<RobotTecnologie>();  // RESET LIST

            #region Get inputs
            // Get inputs start column 
            for (int i = col; i <= lastCellRange.Column; i++)
            {
                if (string.IsNullOrEmpty(Convert.ToString(matrix[row, i])))
                {
                    col = i + 1;
                    break;
                }
            }

            // Get column numbers
            for (int i = col; i <= lastCellRange.Column; i++)
            {
                string cellVal = Convert.ToString(matrix[row, i]);

                if (string.IsNullOrEmpty(cellVal)) break;
                else if (cellVal.ToLower() == "symbolik") colSymbolInputs = i;
                else if (cellVal.ToLower() == "datentyp") colDataType = i;
                else if (cellVal.ToLower() == "adresse") colAddress = i;
                else if (cellVal.ToLower() == "kommentar") colComment = i;
            }

            // Get robot properties
            for (int i = row + 1; i <= lastCellRange.Row; i++)
            {
                string tecName = Convert.ToString(matrix[i, colSymbolOutputs]);

                if (!tecName.Contains("<<BMK>>") && !string.IsNullOrEmpty(tecName))  //If is a tecnologie name
                {
                    var name = Convert.ToString(matrix[i, colSymbolOutputs]);
                    var fbNr = Convert.ToString(matrix[i, lastCellRange.Column - 1]);

                    for (int x = i + 1; x <= lastCellRange.Row; x++)
                    {
                        string cellVal = Convert.ToString(matrix[x, colSymbolInputs]);

                        if (!string.IsNullOrEmpty(cellVal) && cellVal.Contains("<<BMK>>"))
                        {
                            string symbol = matrix[x, colSymbolInputs].ToString();
                            string dataType = matrix[x, colDataType].ToString();
                            string address = matrix[x, colAddress].ToString();
                            string comment = matrix[x, colComment].ToString();

                            RobotTecnologieProperties.Add(new RobotTecnologie(fbNr, name, type, symbol, dataType, address, comment));
                        }
                        else break;
                        i++;
                    }
                }
            }
            #endregion

            Tecnologies.Add(RobotTecnologieProperties);

            return Tecnologies;
        }

        /// <summary>
        /// Get robot safe range monitoring  from sheet
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="lastCellRange"></param>
        /// <returns>List with the input properties[0] and output properties[1]</returns>
        public static List<List<RobotSafeRangeMonitoring>> GetRobotSafeRangeMonitoring(object[,] matrix, Range lastCellRange)
        {
            var SafeRangeMonitoring = new List<List<RobotSafeRangeMonitoring>>();
            var RobotProperties = new List<RobotSafeRangeMonitoring>();
            List<int> FirstTextCell = OpennessHelper.TextPosInRow(matrix, lastCellRange);
            int row = FirstTextCell[0];
            int col = FirstTextCell[1];
            int colSymbol = 0;
            int colDataType = 0;
            int colAddress = 0;
            int colComment = 0;

            #region Get outputs
            // Get column numbers
            for (int i = col; i <= lastCellRange.Column; i++)
            {
                string cellVal = Convert.ToString(matrix[row, i]);

                if (string.IsNullOrEmpty(cellVal)) break;
                else if (cellVal.ToLower() == "symbol") colSymbol = i;
                else if (cellVal.ToLower() == "datentyp") colDataType = i;
                else if (cellVal.ToLower() == "adresse") colAddress = i;
                else if (cellVal.ToLower() == "kommentar") colComment = i;
            }

            // Get robot properties
            for (int i = row + 1; i <= lastCellRange.Row; i++)
            {
                string cellVal = Convert.ToString(matrix[i, colSymbol]);
                if (!string.IsNullOrEmpty(cellVal))
                {
                    string symbol = matrix[i, colSymbol].ToString();
                    string dataType = matrix[i, colDataType].ToString();
                    string address = matrix[i, colAddress].ToString();
                    string comment = matrix[i, colComment].ToString();

                    RobotProperties.Add(new RobotSafeRangeMonitoring(symbol, dataType, address, comment));
                }
            }
            #endregion region

            SafeRangeMonitoring.Add(RobotProperties);
            RobotProperties = new List<RobotSafeRangeMonitoring>();  // RESET LIST

            #region Get inputs
            // Get inputs start column 
            for (int i = col; i <= lastCellRange.Column; i++)
            {
                if (string.IsNullOrEmpty(Convert.ToString(matrix[row, i])))
                {
                    col = i + 1;
                    break;
                }
            }

            // Get column numbers
            for (int i = col; i <= lastCellRange.Column; i++)
            {
                string cellVal = Convert.ToString(matrix[row, i]);

                if (string.IsNullOrEmpty(cellVal)) break;
                else if (cellVal.ToLower() == "symbol") colSymbol = i;
                else if (cellVal.ToLower() == "datentyp") colDataType = i;
                else if (cellVal.ToLower() == "adresse") colAddress = i;
                else if (cellVal.ToLower() == "kommentar") colComment = i;
            }

            // Get robot properties
            for (int i = row + 1; i <= lastCellRange.Row; i++)
            {
                string cellVal = Convert.ToString(matrix[i, colSymbol]);
                if (!string.IsNullOrEmpty(cellVal))
                {
                    string symbol = matrix[i, colSymbol].ToString();
                    string dataType = matrix[i, colDataType].ToString();
                    string address = matrix[i, colAddress].ToString();
                    string comment = matrix[i, colComment].ToString();

                    RobotProperties.Add(new RobotSafeRangeMonitoring(symbol, dataType, address, comment));
                }
            }
            #endregion

            SafeRangeMonitoring.Add(RobotProperties);

            return SafeRangeMonitoring;
        }

        /// <summary>
        /// Get robot safe operation from sheet
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="lastCellRange"></param>
        /// <returns>List with the input properties[0] and output properties[1]</returns>
        public static List<List<RobotSafeOperation>> GetRobotSafeOperation(object[,] matrix, Range lastCellRange)
        {
            var SafeOperations = new List<List<RobotSafeOperation>>();
            var RobotProperties = new List<RobotSafeOperation>();
            List<int> FirstTextCell = OpennessHelper.TextPosInRow(matrix, lastCellRange);
            int row = FirstTextCell[0];
            int col = FirstTextCell[1];
            int colSymbol = 0;
            int colDataType = 0;
            int colAddress = 0;
            int colComment = 0;

            #region Get outputs
            // Get column numbers
            for (int i = col; i <= lastCellRange.Column; i++)
            {
                string cellVal = Convert.ToString(matrix[row, i]);

                if (string.IsNullOrEmpty(cellVal)) break;
                else if (cellVal.ToLower() == "symbol") colSymbol = i;
                else if (cellVal.ToLower() == "datentyp") colDataType = i;
                else if (cellVal.ToLower() == "adresse") colAddress = i;
                else if (cellVal.ToLower() == "kommentar") colComment = i;
            }

            // Get robot properties
            for (int i = row + 1; i <= lastCellRange.Row; i++)
            {
                string cellVal = Convert.ToString(matrix[i, colSymbol]);
                if (!string.IsNullOrEmpty(cellVal))
                {
                    string symbol = matrix[i, colSymbol].ToString();
                    string dataType = matrix[i, colDataType].ToString();
                    string address = matrix[i, colAddress].ToString();
                    string comment = matrix[i, colComment].ToString();

                    RobotProperties.Add(new RobotSafeOperation(symbol, dataType, address, comment));
                }
            }
            #endregion region

            SafeOperations.Add(RobotProperties);
            RobotProperties = new List<RobotSafeOperation>();  // RESET LIST

            #region Get inputs
            // Get inputs start column 
            for (int i = col; i <= lastCellRange.Column; i++)
            {
                if (string.IsNullOrEmpty(Convert.ToString(matrix[row, i])))
                {
                    col = i + 1;
                    break;
                }
            }

            // Get column numbers
            for (int i = col; i <= lastCellRange.Column; i++)
            {
                string cellVal = Convert.ToString(matrix[row, i]);

                if (string.IsNullOrEmpty(cellVal)) break;
                else if (cellVal.ToLower() == "symbol") colSymbol = i;
                else if (cellVal.ToLower() == "datentyp") colDataType = i;
                else if (cellVal.ToLower() == "adresse") colAddress = i;
                else if (cellVal.ToLower() == "kommentar") colComment = i;
            }

            // Get robot properties
            for (int i = row + 1; i <= lastCellRange.Row; i++)
            {
                string cellVal = Convert.ToString(matrix[i, colSymbol]);
                if (!string.IsNullOrEmpty(cellVal))
                {
                    string symbol = matrix[i, colSymbol].ToString();
                    string dataType = matrix[i, colDataType].ToString();
                    string address = matrix[i, colAddress].ToString();
                    string comment = matrix[i, colComment].ToString();

                    RobotProperties.Add(new RobotSafeOperation(symbol, dataType, address, comment));
                }
            }
            #endregion

            SafeOperations.Add(RobotProperties);

            return SafeOperations;
        }

        /// <summary>
        /// Get created robots info from sheet
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static List<RobotInfo> GetCreatedRobotsInfo(object[,] matrix)
        {
            var namesUsed = new List<string>();
            var info = new List<RobotInfo>();

            for (int row = 4; row <= 30; row++)
            {
                string name = Convert.ToString(matrix[row, 12]);
                if (!string.IsNullOrEmpty(name))
                {
                    if (!char.IsDigit(name[0]))
                        name.Substring(1);

                    if (namesUsed.Contains(name))
                        continue;
                    else
                        namesUsed.Add(name);
                }
                else break;

                string type = Convert.ToString(matrix[row, 11]);
                string safe = Convert.ToString(matrix[row, 11]);

                if (safe.Contains("Slave"))
                {
                    //Is not "Save Range Monitoring" or "Safe Operation"
                    safe = Convert.ToString(matrix[row + 1, 11]);
                    type = Convert.ToString(matrix[row, 11]);
                }

                type = type.Replace("_", " ");

                int startAddress = Convert.ToInt16(matrix[row, 13]);
                List<string> tecnologies = new List<string>();

                for (int col = 14; col <= 19; col++)
                {
                    string tec = Convert.ToString(matrix[row, col]);

                    if (!string.IsNullOrEmpty(tec))
                        tecnologies.Add(tec);
                    else break;
                }

                string tecs = string.Join(",", tecnologies);

                info.Add(new RobotInfo(name, safe, startAddress, tecs, type));
            }

            return info;
        }
        #endregion
    }
}

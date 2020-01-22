using TiaOpennessHelper.SafetyMaker;
using Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System;
using Siemens.Engineering;
using System.Windows.Controls;
using System.Windows;
using Siemens.Engineering.SW.Blocks;

namespace TiaOpennessHelper.ExcelTree
{
    public class ExcelManager
    {
        private static string tagName;
        private static string tagValue;
        private static string symbols;
        private static string tagDataType;
        private static string tagAddress;
        private static bool accessible;
        private static bool writable;
        private static bool visible;
        private static string type;
        private static string comment;
        private static string action;
        private static string arbeitsgruppe;
        private static string schutzkreis;
        private static string station;
        private static string stationsbez;

        /// <summary>
        /// Prepare worksheet from sequence excel
        /// </summary>
        /// <param name="ObjectX"></param>
        /// <param name="name"></param>
        public static void PrepareExcelValues(object[,] ObjectX, string name)
        {
            StepHandler.StepList.Clear();

            for (int z = 4; z < ObjectX.GetLength(0); z++)
            {
                string stepNumber = ObjectX[z, 2]?.ToString() ?? null;
                string stepName = ObjectX[z, 3]?.ToString() ?? null;
                string stepDesc = ObjectX[z, 4]?.ToString() ?? null;
                string stepTime = ObjectX[z, 8]?.ToString() ?? null;

                // If Step name is null
                if (string.IsNullOrEmpty(stepName)) break;

                StepHandler step = new StepHandler(stepNumber, stepName, stepDesc, stepTime, name);
                string[] actions = ObjectX[z, 5].ToString().Split('\n');
                foreach (string s in actions)
                {
                    step.StepActions.Add(s);
                }

                string[] previousSteps = ObjectX[z, 6].ToString().Split('\n');
                foreach (string s in previousSteps)
                {
                    step.PreviousSteps.Add(s);
                }

                string[] nextSteps = ObjectX[z, 7].ToString().Split('\n');
                foreach (string s in nextSteps)
                {
                    step.NextSteps.Add(s);
                }

                StepHandler.StepList.Add(step);
            }

            StepHandler.GrafcetList.Add(StepHandler.StepList);
        }

        /// <summary>
        /// Retrives the values of the worksheet named "EngAssist" and saves them on the DBMaker EngValues list
        /// </summary>
        public static void EngAssist()
        {
            if (TreeViewManager.FilePath != "" && System.IO.File.Exists(TreeViewManager.FilePath))
            {
                int sheetCounter = 1;
                int rowCounter = 4;
                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(TreeViewManager.FilePath);

                foreach (Excel.Worksheet sheet in xlWorkbook.Worksheets)
                {
                    if (sheet.Name.Contains("EngAssist"))
                    {
                        DBMaker.EngValues.Clear();

                        while (xlWorkbook.Sheets[sheetCounter].Cells(rowCounter, 2).Value != null)
                        {
                            if (xlWorkbook.Sheets[sheetCounter].Cells(rowCounter, 2).Value != null)
                            {
                                arbeitsgruppe = xlWorkbook.Sheets[sheetCounter].Cells(rowCounter, 2).Value.ToString();
                            }

                            if (xlWorkbook.Sheets[sheetCounter].Cells(rowCounter, 3).Value != null)
                            {
                                schutzkreis = xlWorkbook.Sheets[sheetCounter].Cells(rowCounter, 3).Value.ToString();
                            }

                            if (xlWorkbook.Sheets[sheetCounter].Cells(rowCounter, 4).Value != null)
                            {
                                station = xlWorkbook.Sheets[sheetCounter].Cells(rowCounter, 4).Value.ToString();
                            }

                            if (xlWorkbook.Sheets[sheetCounter].Cells(rowCounter, 5).Value == null)
                            {
                                stationsbez = "";
                            }
                            else
                            {
                                stationsbez = xlWorkbook.Sheets[sheetCounter].Cells(rowCounter, 5).Value.ToString();
                            }

                            EngAssist Value = new EngAssist(arbeitsgruppe, schutzkreis, station, stationsbez);

                            int column = 8;
                            while (xlWorkbook.Sheets[sheetCounter].Cells(rowCounter, column).Value != null)
                            {
                                Value.Parts.Add(xlWorkbook.Sheets[sheetCounter].Cells(rowCounter, column).Value.ToString());
                                column += 1;
                            }

                            column = 8;
                            while (xlWorkbook.Sheets[sheetCounter].Cells(rowCounter + 1, column).Value != null)
                            {
                                Value.Valves.Add(xlWorkbook.Sheets[sheetCounter].Cells(rowCounter + 1, column).Value.ToString());
                                column += 1;
                            }

                            DBMaker.EngValues.Add(Value);

                            rowCounter += 2;
                        }
                    }
                    sheetCounter += 1;
                }
                xlWorkbook.Close(0);
                xlApp.Quit();
            }
        }

        /// <summary>
        /// Used to see if a string is all Digits
        /// </summary>
        public static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Retrives the values of the worksheet named "SPS" and saves them on the DBMaker Variables list
        /// </summary>
        public static void SPS(object[,] Matriz)
        {
            int row = 2;
            DBMaker.Variables.Clear();
            DBMaker.SPSActions.Clear();

            while (Matriz[row, 1] != null)
            {
                if (Matriz[row, 1] != null && Matriz[row, 2] != null)
                {
                    if (Matriz[row, 1].ToString().Contains("\""))
                    {
                        tagName = Matriz[row, 1].ToString().Substring(1, Matriz[row, 1].ToString().Length - 2);
                    }
                    else
                    {
                        tagName = Matriz[row, 1].ToString();
                    }

                    if (Matriz[row, 2] == null)
                    {
                        type = "";
                    }
                    else
                    {
                        type = Matriz[row, 2].ToString();
                    }

                    if (Matriz[row, 3] == null)
                    {
                        comment = "";
                    }
                    else
                    {
                        comment = Matriz[row, 3].ToString();
                    }

                    DBMaker.Variables.Add(new Variable()
                    {
                        Name = tagName.Replace(" ", string.Empty),
                        Type = type.Replace(" ", string.Empty),
                        Comment = comment
                    });
                }
                row += 1;
                if (row > Matriz.GetLength(0)) break;
            }

            row = 1;
            while (Matriz[row, 8] != null)
            {
                if (Matriz[row, 9] != null)
                {
                    DBMaker.SPSActions.Add(new ReplaceActions()
                    {
                        ToBeReplace = Matriz[row, 8].ToString().Replace(" ", string.Empty),
                        ReplaceAction = Matriz[row, 9].ToString()
                    });
                }

                row += 1;
                if (row > Matriz.GetLength(0)) break;
            }
            DBMaker.SPSActions.Reverse();
        }

        /// <summary>
        /// Retrives the values of the worksheet named "Schutzkreis" and saves them on the DBMaker Variables list
        /// </summary>
        public static void Schutzkreis(object[,] Matriz)
        {
            int row = 2;
            DBMaker.Variables.Clear();
            DBMaker.SCHActions.Clear();

            while (Matriz[row, 1] != null)
            {
                if (Matriz[row, 1] != null)
                {
                    if (Matriz[row, 1].ToString().Contains("\""))
                    {
                        tagName = Matriz[row, 1].ToString().Substring(1, Matriz[row, 1].ToString().Length - 2);
                    }
                    else
                    {
                        tagName = Matriz[row, 1].ToString();
                    }

                    if (Matriz[row, 2] == null)
                    {
                        type = "";
                    }
                    else
                    {
                        type = Matriz[row, 2].ToString();
                    }

                    if (Matriz[row, 3] == null)
                    {
                        comment = "";
                    }
                    else
                    {
                        comment = Matriz[row, 3].ToString();
                    }

                    DBMaker.Variables.Add(new Variable()
                    {
                        Name = tagName.Replace(" ", string.Empty),
                        Type = type.Replace(" ", string.Empty),
                        Comment = comment
                    });

                }
                row += 1;
                if (row > Matriz.GetLength(0)) break;
            }

            row = 1;
            while (Matriz[row, 8] != null)
            {
                if (Matriz[row, 9] != null)
                {
                    DBMaker.SCHActions.Add(new ReplaceActions()
                    {
                        ToBeReplace = Matriz[row, 8].ToString().Replace(" ", string.Empty),
                        ReplaceAction = Matriz[row, 9].ToString()
                    });
                }

                row += 1;
                if (row > Matriz.GetLength(0)) break;
            }

            DBMaker.SCHActions.Reverse();
        }

        /// <summary>
        /// Retrives the values of the worksheet named Safety that are from the Safety to Standart part and saves them on the DBMaker Variables list"
        /// </summary>
        public static void Safety_Standart(object[,] Matriz)
        {
            int row = 2;
            string LatestIns = "";
            DBMaker.Variables.Clear();
            DBMaker.SAFActions.Clear();

            while (Matriz[row, 1].ToString() != "Name >F")
            {
                if (Matriz[row, 1] != null)
                {
                    if (Matriz[row, 1].ToString().Contains("\""))
                    {
                        tagName = Matriz[row, 1].ToString().Substring(1, Matriz[row, 1].ToString().Length - 2);
                    }
                    else
                    {
                        tagName = Matriz[row, 1].ToString();
                    }
                    

                    if (Matriz[row, 2] == null)
                    {
                        type = "";
                    }
                    else
                    {
                        type = Matriz[row, 2].ToString();
                    }

                    if (Matriz[row, 3] == null)
                    {
                        comment = "";
                    }
                    else
                    {
                        comment = Matriz[row, 3].ToString();
                    }

                    if (Matriz[row, 4] == null)
                    {
                        action = LatestIns;
                    }
                    else
                    {
                        LatestIns = Matriz[row, 4].ToString();
                        action = Matriz[row, 4].ToString();
                    }

                    DBMaker.Variables.Add(new Variable()
                    {
                        Name = tagName.Replace(" ", string.Empty),
                        Type = type.Replace(" ", string.Empty),
                        Comment = comment,
                        Action = action
                    });

                }
                row += 1;
                if (row > Matriz.GetLength(0)) break;
            }

            row = 1;
            while (Matriz[row, 7] != null)
            {
                if (Matriz[row, 8] != null)
                {
                    DBMaker.SAFActions.Add(new ReplaceActions()
                    {
                        ToBeReplace = Matriz[row, 7].ToString().Replace(" ", string.Empty),
                        ReplaceAction = Matriz[row, 8].ToString()
                    });
                }
                row += 1;
                if (row > Matriz.GetLength(0)) break;
            }

            DBMaker.SAFActions.Reverse();
        }

        /// <summary>
        /// Retrives the values of the worksheet named Safety that are from the Standart to Safety part and saves them on the DBMaker Variables list
        /// </summary>
        public static void Standart_Safety(object[,] Matriz)
        {
            int row = 1;
            string LatestIns = "";

            DBMaker.Variables.Clear();
            DBMaker.SAFActions.Clear();

            while (Matriz[row, 1].ToString() != "Name >F")
            {
                row += 1;
                if (row > Matriz.GetLength(0)) break;
            }

            row += 1;

            while (Matriz[row, 1] != null)
            {
                if (Matriz[row, 1] != null)
                {
                    if (Matriz[row, 1].ToString().Contains("\""))
                    {
                        tagName = Matriz[row, 1].ToString().Substring(1, Matriz[row, 1].ToString().Length - 2);
                    }
                    else
                    {
                        tagName = Matriz[row, 1].ToString();
                    }

                    if (Matriz[row, 2] == null)
                    {
                        type = "";
                    }
                    else
                    {
                        type = Matriz[row, 2].ToString();
                    }

                    if (Matriz[row, 3] == null)
                    {
                        comment = "";
                    }
                    else
                    {
                        comment = Matriz[row, 3].ToString();
                    }

                    if (Matriz[row, 4] == null)
                    {
                        action = LatestIns;
                    }
                    else
                    {
                        LatestIns = Matriz[row, 4].ToString();
                        action = Matriz[row, 4].ToString();
                    }

                    DBMaker.Variables.Add(new Variable()
                    {
                        Name = tagName.Replace(" ", string.Empty),
                        Type = type.Replace(" ", string.Empty),
                        Comment = comment,
                        Action = action
                    });

                }
                row += 1;
                if (row > Matriz.GetLength(0)) break;
            }

            row = 1;
            while (Matriz[row, 7] != null)
            {
                if (Matriz[row, 8] != null)
                {
                    DBMaker.SAFActions.Add(new ReplaceActions()
                    {
                        ToBeReplace = Matriz[row, 7].ToString(),
                        ReplaceAction = Matriz[row, 8].ToString()
                    });
                }
                row += 1;
                if (row > Matriz.GetLength(0)) break;
            }

            DBMaker.SAFActions.Reverse();
        }

        /// <summary>
        /// Retrives the values of the worksheet named "EngAssisst" and saves them on the DBMaker EngValues list
        /// </summary>
        public static void EngAssist(object[,] Matriz)
        {
            int rowCounter = 4;

            DBMaker.EngValues.Clear();

            while (Matriz[rowCounter, 2] != null)
            {

                if (Matriz[rowCounter, 2] != null)
                {
                    arbeitsgruppe = Matriz[rowCounter, 2].ToString().Replace(" ", string.Empty);
                }

                if (Matriz[rowCounter, 3] != null)
                {
                    schutzkreis = Matriz[rowCounter, 3].ToString().Replace(" ", string.Empty);
                }

                if (Matriz[rowCounter, 4] != null)
                {
                    station = Matriz[rowCounter, 4].ToString().Replace(" ", string.Empty);
                }

                if (Matriz[rowCounter, 5] == null)
                {
                    stationsbez = "";
                }
                else
                {
                    stationsbez = Matriz[rowCounter, 5].ToString().Replace(" ", string.Empty);
                }

                EngAssist Value = new EngAssist(arbeitsgruppe, schutzkreis, station, stationsbez);

                int column = 8;
                while (Matriz[rowCounter, column] != null)
                {
                    Value.Parts.Add(Matriz[rowCounter, column].ToString().Replace(" ", string.Empty));
                    column += 1;
                    if (column > Matriz.GetLength(0)) break;
                }

                column = 8;
                while (Matriz[rowCounter + 1, column] != null)
                {
                    Value.Valves.Add(Matriz[rowCounter + 1, column].ToString().Replace(" ", string.Empty));
                    column += 1;
                    if (column > Matriz.GetLength(0)) break;
                }

                DBMaker.EngValues.Add(Value);

                rowCounter += 2;
                if (rowCounter > Matriz.GetLength(0)) break;
            }

        }

        /// <summary>
        /// Retrives the values of the worksheet named "PLC Tags" and saves them on the DBMaker PLC_Tags list
        /// </summary>
        public static void PLC_Tags(object[,] Matriz)
        {
            DBMaker.PLC_Tags = new List<PLC_Tag>();

            for (int row = 2; row <= Matriz.GetLength(0); row++)
            {
                if (Matriz[row, 1] != null)
                {
                    if (Matriz[row, 1].ToString().Contains("\""))
                    {
                        tagName = Matriz[row, 1].ToString().Substring(1, Matriz[row, 1].ToString().Length - 2);
                    }
                    else
                    {
                        tagName = Matriz[row, 1].ToString();
                    } 

                    if (Matriz[row, 2] == null)
                    {
                        symbols = "";
                    }
                    else
                    {
                        symbols = Matriz[row, 2].ToString();
                    }

                    if (Matriz[row, 3] == null)
                    {
                        tagDataType = "";
                    }
                    else
                    {
                        tagDataType = Matriz[row, 3].ToString();
                    }

                    if (Matriz[row, 4] == null)
                    {
                        tagAddress = "";
                    }
                    else
                    {
                        tagAddress = Matriz[row, 4].ToString();
                    }

                    if (Matriz[row, 5] == null)
                    {
                        comment = "";
                    }
                    else
                    {
                        comment = Matriz[row, 5].ToString();
                    }

                    if (Matriz[row, 6] == null)
                    {
                        visible = false;
                    }
                    else
                    {
                        if (Matriz[row, 6].ToString().ToLower() == "true")
                            visible = true;
                        else
                            visible = false;
                    }

                    if (Matriz[row, 7] == null)
                    {
                        accessible = false;
                    }
                    else
                    {
                        if (Matriz[row, 7].ToString().ToLower() == "true")
                            accessible = true;
                        else
                            accessible = false;
                    }

                    if (Matriz[row, 8] == null)
                    {
                        writable = false;
                    }
                    else
                    {
                        if (Matriz[row, 8].ToString().ToLower() == "true")
                            writable = true;
                        else
                            writable = false;
                    }

                    DBMaker.PLC_Tags.Add(new PLC_Tag()
                    {
                        Name = tagName.Replace(" ", string.Empty),
                        Symbols = symbols.Replace(" ", string.Empty),
                        DataType = tagDataType.Replace(" ", string.Empty),
                        Address = tagAddress.Replace(" ", string.Empty),
                        Comment = comment,
                        Accessible = accessible,
                        Writable = writable,
                        Visible = visible
                    });

                }
            }
        }

        /// <summary>
        /// Retrives the values of the worksheet named "User Config" and saves them on the DBMaker UserConfigs list
        /// </summary>
        public static void EngConfig(object[,] Matriz)
        {
            int row = 4;
            DBMaker.UserConfigs.Clear();

            while (Matriz[row, 1] != null)
            {
                if (Matriz[row, 1] != null)
                {
                    if (Matriz[row, 1].ToString().Contains("\""))
                    {
                        tagName = Matriz[row, 1].ToString().Substring(1, Matriz[row, 1].ToString().Length - 2);
                    }
                    else
                    {
                        tagName = Matriz[row, 1].ToString();
                    }

                    if (Matriz[row, 2] == null)
                    {
                        tagValue = "";
                    }
                    else
                    {
                        tagValue = Matriz[row, 2].ToString();
                    }

                    DBMaker.UserConfigs.Add(new UserConfig()
                    {
                        Name = tagName,
                        Value = tagValue.Replace(" ", string.Empty)
                    });

                }
                row += 1;
                if (row > Matriz.GetLength(0)) break;
            }
        }

        /// <summary>
        /// Retrives the values of the first part of the Station WorkSheet
        /// </summary>
        public static void StationName1(object[,] Matriz)
        {
            int row = 2;
            DBMaker.Variables.Clear();
            DBMaker.STAActions.Clear();

            while (Matriz[row, 1] != null)
            {
                if (Matriz[row, 1] != null)
                {
                    tagName = Matriz[row, 1].ToString();
                    tagName = tagName.Replace("\"", "");

                    if (Matriz[row, 2] == null)
                    {
                        type = "";
                    }
                    else
                    {
                        type = Matriz[row, 2].ToString();
                    }

                    if (Matriz[row, 3] == null)
                    {
                        comment = "";
                    }
                    else
                    {
                        comment = Matriz[row, 3].ToString();
                    }

                    if (Matriz[row, 4] == null)
                    {
                        action = "";
                    }
                    else
                    {
                        action = Matriz[row, 4].ToString();
                    }

                    DBMaker.Variables.Add(new Variable()
                    {
                        Name = tagName.Replace(" ", string.Empty),
                        Type = type.Replace(" ", string.Empty),
                        Comment = comment,
                        Action = action
                    });
                }
                row += 1;
                if (row > Matriz.GetLength(0)) break;
            }

            row = 1;
            while (Matriz[row, 7] != null)
            {
                if (Matriz[row, 8] == null)
                {
                    tagValue = "";
                }
                else
                {
                    tagValue = Matriz[row, 8].ToString();
                }

                DBMaker.STAActions.Add(new ReplaceActions()
                {
                    ToBeReplace = Matriz[row, 7].ToString(),
                    ReplaceAction = tagValue.Replace(" ", string.Empty)
                });

                row += 1;
                if (row > Matriz.GetLength(0)) break;
            }

            DBMaker.STAActions.Reverse();
        }

        /// <summary>
        /// Retrives the values of the second part of the Station WorkSheet
        /// </summary>
        public static void StationName2(object[,] Matriz)
        {
            int row = 2;
            DBMaker.Variables.Clear();
            DBMaker.STAActions.Clear();

            while (Matriz[row, 1] != null)
            {
                string s = Matriz[row, 1].ToString();

                if (!s.Contains("Name %"))
                {
                    row += 1;
                }
                if (row > Matriz.GetLength(0)) break;
            }

            row += 2;

            while (Matriz[row, 1] != null)
            {
                if (Matriz[row, 1] != null)
                {
                    tagName = Matriz[row, 1].ToString();
                    tagName = tagName.Replace("\"", "");

                    if (Matriz[row, 2] == null)
                    {
                        type = "";
                    }
                    else
                    {
                        type = Matriz[row, 2].ToString();
                    }

                    if (Matriz[row, 3] == null)
                    {
                        comment = "";
                    }
                    else
                    {
                        comment = Matriz[row, 3].ToString();
                    }

                    if (Matriz[row, 4] == null)
                    {
                        action = "";
                    }
                    else
                    {
                        action = Matriz[row, 4].ToString();
                    }

                    DBMaker.Variables.Add(new Variable()
                    {
                        Name = tagName.Replace(" ", string.Empty),
                        Type = type.Replace(" ", string.Empty),
                        Comment = comment,
                        Action = action
                    });

                }
                row += 1;
                if (row > Matriz.GetLength(0)) break;
            }

            row = 1;
            while (Matriz[row, 7] != null)
            {
                if (Matriz[row, 8] == null)
                {
                    tagValue = "";
                }

                else
                {
                    tagValue = Matriz[row, 8].ToString();
                }

                DBMaker.STAActions.Add(new ReplaceActions()
                {
                    ToBeReplace = Matriz[row, 7].ToString().Replace(" ", string.Empty),
                    ReplaceAction = tagValue
                });

                row += 1;
                if (row > Matriz.GetLength(0)) break;
            }

            DBMaker.STAActions.Reverse();
        }

        /// <summary>
        /// Retrives the values of the third part of the Station WorkSheet
        /// </summary>
        public static void StationName3(object[,] Matriz)
        {
            int row = 2;
            DBMaker.Variables.Clear();
            DBMaker.STAActions.Clear();

            while (Matriz[row, 1] != null)
            {
                string s = Matriz[row, 1].ToString();

                if (!s.Contains("Name %"))
                {
                    row += 1;
                }
                if (row > Matriz.GetLength(0)) break;
            }


            row += 2;

            while (Matriz[row, 1] != null)
            {
                string s = Matriz[row, 1].ToString();

                if (!s.Contains("Name %"))
                {
                    row += 1;
                }
                if (row > Matriz.GetLength(0)) break;
            }

            row += 2;

            while (Matriz[row, 1] != null)
            {
                if (Matriz[row, 1] != null)
                {
                    tagName = Matriz[row, 1].ToString();
                    tagName = tagName.Replace("\"", "");

                    if (Matriz[row, 2] == null)
                    {
                        type = "";
                    }
                    else
                    {
                        type = Matriz[row, 2].ToString();
                    }

                    if (Matriz[row, 3] == null)
                    {
                        comment = "";
                    }
                    else
                    {
                        comment = Matriz[row, 3].ToString();
                    }

                    if (Matriz[row, 4] == null)
                    {
                        action = "";
                    }
                    else
                    {
                        action = Matriz[row, 4].ToString();
                    }

                    DBMaker.Variables.Add(new Variable()
                    {
                        Name = tagName.Replace(" ", string.Empty),
                        Type = type.Replace(" ", string.Empty),
                        Comment = comment,
                        Action = action
                    });

                }
                row += 1;
                if (row > Matriz.GetLength(0)) break;
            }

            row = 1;
            while (Matriz[row, 7] != null)
            {
                if (Matriz[row, 8] != null)
                {
                    tagValue = Matriz[row, 8].ToString();

                    DBMaker.STAActions.Add(new ReplaceActions()
                    {
                        ToBeReplace = Matriz[row, 7].ToString().Replace(" ", string.Empty),
                        ReplaceAction = tagValue
                    });
                }

                row += 1;
                if (row > Matriz.GetLength(0)) break;
            }

            DBMaker.STAActions.Reverse();
        }

        /// <summary>
        /// Retrives the values of WorkSheet Arg
        /// </summary>
        public static void ARG(object[,] Matriz)
        {
            int row = 2;
            DBMaker.Variables.Clear();

            while (Matriz[row, 1] != null)
            {
                if (Matriz[row, 1] != null)
                {
                    tagName = Matriz[row, 1].ToString();

                    if (Matriz[row, 2] == null)
                    {
                        type = "";
                    }
                    else
                    {
                        type = Matriz[row, 2].ToString();
                    }

                    if (Matriz[row, 3] == null)
                    {
                        comment = "";
                    }
                    else
                    {
                        comment = Matriz[row, 3].ToString();
                    }

                    if (Matriz[row, 4] == null)
                    {
                        action = "";
                    }
                    else
                    {
                        action = Matriz[row, 4].ToString();
                    }

                    DBMaker.Variables.Add(new Variable()
                    {
                        Name = tagName.Replace(" ", string.Empty),
                        Type = type.Replace(" ", string.Empty),
                        Comment = comment,
                        Action = action
                    });

                }
                row += 1;
                if (row > Matriz.GetLength(0)) break;
            }

            row = 1;
            while (Matriz[row, 7] != null)
            {
                if (Matriz[row, 8] != null)
                {
                    DBMaker.STAActions.Add(new ReplaceActions()
                    {
                        ToBeReplace = Matriz[row, 7].ToString().Replace(" ", string.Empty),
                        ReplaceAction = Matriz[row, 8].ToString()
                    });

                    row += 1;
                }
                if (row > Matriz.GetLength(0)) break;
            }

            DBMaker.STAActions.Reverse();
        }

        /// <summary>
        /// Retrives the steps of a WoorkBook inside the folder of the selected item 
        /// </summary>
        public static List<string> Steps(object[,] AS_Matriz)
        {
            List<string> steps = new List<string>();

            int rowCounter = 4;

            while (AS_Matriz[rowCounter, 3] != null)
            {
                steps.Add(AS_Matriz[rowCounter, 3].ToString().Replace(" ", string.Empty));
                rowCounter += 1;
                if (rowCounter > AS_Matriz.GetLength(0)) break;
            }

            return steps;
        }

        /// <summary>
        /// Retrives the frags of a WoorkBook inside the folder of the selected item 
        /// </summary>
        public static List<Frag> Frags(string currentStat, string N_Matriz, object[,] AS_Matriz)
        {
            List<Frag> frags = new List<Frag>();

            for (int i = 4; i <= AS_Matriz.GetLength(0); i++)
            {
                string profil = AS_Matriz[i, 15]?.ToString() ?? "";
                string station = AS_Matriz[i, 16]?.ToString() ?? "";
                string funktion = AS_Matriz[i, 18]?.ToString() ?? "";

                if (profil == "") break;

                if (station == currentStat.Substring(0, currentStat.Length - 1) + "x" && profil.Contains("_MS") 
                    || (station == currentStat && profil.Contains("_MS")))
                {
                    frags.Add(new Frag()
                    {
                        Profil = profil,
                        Station = station,
                        Funktion = funktion,
                        SheetName = N_Matriz
                    });
                }
            }

            return frags;
        }

        /// <summary>
        /// Tests to see if a file is open
        /// </summary>
        /// <param name="path">Path of the file that is going to be tested if its open</param>
        public static bool IsOpened(string path)
        {
            bool opened;
            try
            {
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Write,
                FileShare.None);
                fs.Dispose();
                opened = false;
            }

            catch (IOException)
            {
                opened = true;
            }
            return opened;
        }

        /// <summary>
        /// Import blocks to Tia Portal
        /// </summary>
        /// <param name="savePath"></param>
        /// <param name="current"></param>
        /// <param name="Folder"></param>
        /// <param name="BlocksCreated"></param>
        public static void BlocksImporter(string savePath, object current, string Folder, List<string> BlocksCreated)
        {
            if (current != null)
            {
                var group = (current as PlcBlockUserGroup).Groups.Find(Folder);

                if (group != null)
                {
                    var blocksDirectory = new DirectoryInfo(savePath + "\\" + Folder);

                    // Get files inside Folder
                    foreach (var file in blocksDirectory.GetFiles())
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file.FullName);

                        // Check files inside Folder and check if that file was created by method that called this function
                        if (BlocksCreated.Contains(fileName))
                        {
                            try
                            {
                                OpennessHelper.ImportItem(group, file.FullName, ImportOptions.Override);
                            }
                            catch (Exception ex)
                            {
                                System.Windows.MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }

                    // Get Subfolders inside Folder
                    foreach (var folder in blocksDirectory.GetDirectories())
                    {
                        // Get files inside SubFolder
                        foreach (var file in folder.GetFiles())
                        {
                            string fileName = Path.GetFileNameWithoutExtension(file.FullName);

                            // Check files inside Folder and check if that file was created by method that called this function
                            if (BlocksCreated.Contains(fileName))
                            {
                                var subGroup = group.Groups.Find(folder.Name);
                                if (subGroup != null)
                                {
                                    try
                                    {
                                        OpennessHelper.ImportItem(subGroup, file.FullName, ImportOptions.Override);
                                    }
                                    catch (Exception)
                                    {
                                        // Continue
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

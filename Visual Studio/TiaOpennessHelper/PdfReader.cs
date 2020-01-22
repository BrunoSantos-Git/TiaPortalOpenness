using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace TiaOpennessHelper
{
    public partial class OpennessHelper
    {
        /// <summary>
        /// Get all page numbers that contains Geräteliste
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>List of page numbers</returns>
        public static List<int> GetGaretelistePages(string fileName)
        {
            List<int> pages = new List<int>();
            if (File.Exists(fileName))
            {
                PdfReader pdfReader = new PdfReader(fileName);

                for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    string currentPageText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);
                    if (currentPageText.Contains("Geräteliste"))
                    {
                        pages.Add(page);
                    }
                }
                pdfReader.Close();
            }
            return pages;
        }

        /// <summary>
        /// Return the name of a module
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns>Name of the module with order number</returns>
        public static string GetHWPart(string orderNumber)
        {
            string name = "";

            switch (orderNumber)
            {
                case "550202":
                    name = "16DI-D [16DE]";
                break;

                case "1971599":
                    name = "FVDO-P2 [3DA-F]";
                break;

                case "550663":
                    name = "VTSA DIL 4[32DA]";
                break;
            }

            return name;
        }

        /// <summary>
        /// Look for a device on EPlan PDF and returns it's information
        /// </summary>
        /// <param name="path"></param>
        /// <param name="deviceFGroup"></param>
        /// <param name="deviceIdentifier"></param>
        /// <param name="garetelistPages"></param>
        /// <returns>HW Information</returns>
        public static List<List<string>> HWInfo(string path, string deviceFGroup, string deviceIdentifier, List<int> garetelistPages)
        {
            StringBuilder text = new StringBuilder();
            List<List<string>> HWInformation = null;
            string[] delim = { Environment.NewLine, "\n" };
            string[] lines;

            List<int> pages = OpennessHelper.GetPagesByString(path, "=" + deviceFGroup, garetelistPages);

            // If pages list has elements
            if (pages.Any())
            {
                text = OpennessHelper.ReadPdf(path, pages);
                lines = text.ToString().Split(delim, StringSplitOptions.None);
                HWInformation = OpennessHelper.GetHWRawInformation(lines, deviceFGroup, deviceIdentifier);
            }

            return HWInformation;
        }

        #region Private Methods
        /// <summary>
        /// Get Hardware Raw information from the pdf text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="HWName"></param>
        /// <param name="HWIdentifier"></param>
        private static List<List<string>> GetHWRawInformation(string[] text, string HWName, string HWIdentifier)
        {
            List<List<string>> info = new List<List<string>>();
            StringBuilder hwInfo = new StringBuilder();

            string FGroup = "", orderNumber = "";

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].Contains(HWName) && !text[i].Contains("CAD"))
                    hwInfo.Append(text[i] + " LINEBREAK\n");
            }

            string[] ssize = hwInfo.ToString().Split(new char[0]);

            for (int i = 0; i < ssize.Length; i++)
            {
                if (!ssize[i].Contains("LINEBREAK"))
                {
                    if (ssize[i].Contains("=") && ssize[i + 1].Contains(HWIdentifier))
                    {
                        FGroup = ssize[i].Substring(1);     // Resource
                        //identifier = ssize[i + 1];          // Identifier
                    }
                }
                else
                {
                    if (IsPartNumber(ssize[i - 1]))
                        orderNumber = ssize[i - 2];   // Order Number
                    else
                        orderNumber = ssize[i - 1];   // Order Number

                    info.Add(new List<string>() { FGroup, orderNumber });
                }
            }
            return info;
        }

        /// <summary>
        /// Check if a given string is a Part Number
        /// </summary>
        /// <param name="part"></param>
        /// <returns>True or False</returns>
        private static bool IsPartNumber(string part)
        {
            bool partNumber = false;

            if (Char.IsLetter(part[0]))
            {
                switch (part[0])
                {
                    case 'Q':   // Part number starting with Q have 12 Lenght
                        if (part.Length == 12) partNumber = true;
                        break;

                    default:    // Part number starting with another char have 13 Lenght
                        if (part.Length == 13) partNumber = true;
                        break;
                }
            }

            return partNumber;
        }

        /// <summary>
        /// Get all page numbers that contains a string
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="searchText"></param>
        /// <param name="garetelistPages"></param>
        /// <returns>List of page numbers</returns>
        private static List<int> GetPagesByString(string fileName, String searchText, List<int> garetelistPages)
        {
            List<int> pages = new List<int>();
            if (File.Exists(fileName))
            {
                PdfReader pdfReader = new PdfReader(fileName);
                foreach (int page in garetelistPages)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();

                    string currentPageText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);
                    if (currentPageText.Contains(searchText))
                    {
                        pages.Add(page);
                    }
                }
                pdfReader.Close();
            }
            return pages;
        }

        /// <summary>
        /// Read text from PDF file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pages"></param>
        /// <returns>Text from pdf File</returns>
        private static StringBuilder ReadPdf(string path, List<int> pages)
        {
            StringBuilder text = new StringBuilder();
            PdfReader pdfReader = new PdfReader(path);

            foreach (int page in pages)
            {
                ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

                currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                text.Append(currentText);
            }
            pdfReader.Close();

            return text;
        }
        #endregion
    }
}

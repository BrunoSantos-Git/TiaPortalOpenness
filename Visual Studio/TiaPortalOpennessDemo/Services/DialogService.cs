using System;
using System.Windows;

namespace TiaPortalOpennessDemo.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// TODO Edit XML Comment Template for DialogService
    public static class DialogService 
    {
        #region ErrorMessageDialog

        /// <summary>Shows the error message box.</summary>
        /// <param name="errorText">The error text.</param>
        /// TODO Edit XML Comment Template for ShowErrorMessageBox
        public static void ShowErrorMessageBox(string errorText)
        {
            ShowErrorMessageBox(errorText, "");
        }

        /// <summary>Shows the error message box.</summary>
        /// <param name="errorText">The error text.</param>
        /// <param name="exceptionText">The exception text.</param>
        /// TODO Edit XML Comment Template for ShowErrorMessageBox
        public static void ShowErrorMessageBox(string errorText, string exceptionText)
        {
            var sText = "Error:" + Environment.NewLine + errorText + Environment.NewLine + Environment.NewLine + Environment.NewLine;
            if (string.IsNullOrEmpty(exceptionText) == false)
            {
                sText += "Exception:" + Environment.NewLine + exceptionText;
            }

            MessageBox.Show(sText, "Error",MessageBoxButton.OK,MessageBoxImage.Error);
        }

        #endregion

        #region WarningMessageDialog

        /// <summary>Shows the warning message box.</summary>
        /// <param name="warningText">The warning text.</param>
        /// <returns>MessageBoxResult</returns>
        /// TODO Edit XML Comment Template for ShowWarningMessageBox
        public static MessageBoxResult ShowWarningMessageBox(string warningText)
        {
            return ShowWarningMessageBox(warningText, "");
        }

        /// <summary>Shows the warning message box.</summary>
        /// <param name="warningText">The warning text.</param>
        /// <param name="exceptionText">The exception text.</param>
        /// <returns>MessageBoxResult</returns>
        /// TODO Edit XML Comment Template for ShowWarningMessageBox
        public static MessageBoxResult ShowWarningMessageBox(string warningText, string exceptionText)
        {
            var sText = "Warning:" + Environment.NewLine + warningText + Environment.NewLine + Environment.NewLine + Environment.NewLine;
            if (string.IsNullOrEmpty(exceptionText) == false)
            {
                sText += exceptionText;
            }

            return (MessageBox.Show(sText, "Information", MessageBoxButton.YesNo, MessageBoxImage.Question));
        }

        #endregion

    }
}

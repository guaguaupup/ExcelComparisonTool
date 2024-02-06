using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExcelComparison.Helper
{
    class NewMessageBox
    {
        public static MessageBoxResult ShowMessageBox(
            string messageBoxText,
            string caption = null,
            MessageBoxButton button = MessageBoxButton.OK,
            MessageBoxImage icon = MessageBoxImage.Information,
            MessageBoxResult defaultResult = MessageBoxResult.None)
        {
            return HandyControl.Controls.MessageBox.Show(messageBoxText, caption, button, icon, defaultResult);
        }

        public static void ShowErrorMessage(string messageBoxText)
        {
            if (!string.IsNullOrEmpty(messageBoxText))
            {
                ShowMessageBox(messageBoxText, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void ShowWarningMessage(string messageBoxText)
        {
            if (!string.IsNullOrEmpty(messageBoxText))
            {
                ShowMessageBox(messageBoxText, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public static MessageBoxResult ShowConfirmMessage(string messageBoxText)
        {
            if (!string.IsNullOrEmpty(messageBoxText))
            {
                return ShowMessageBox(messageBoxText, "Confirm", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            }
            return MessageBoxResult.None;
        }

        public static void ShowInformationMessage(string messageBoxText)
        {
            if (!string.IsNullOrEmpty(messageBoxText))
            {
                ShowMessageBox(messageBoxText, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}

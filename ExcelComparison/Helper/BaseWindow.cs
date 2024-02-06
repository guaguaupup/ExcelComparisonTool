using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ExcelComparison.Helper
{
    public class CustomerWindow : HandyControl.Controls.Window
    {
        public CustomerWindow()
        {
            ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Background = FindResource("PrimaryBackBrush") as SolidColorBrush;
            NonClientAreaBackground = FindResource("WinTitleBackBrush") as SolidColorBrush;
            if (this != Application.Current.MainWindow)
            {
                if (Application.Current.MainWindow.IsLoaded)
                {
                    Owner = Application.Current.MainWindow;
                }
            }
        }

        private void CustomerWindow_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            }
            catch (Exception ex)
            {
                NewMessageBox.ShowErrorMessage(ex.ToString());
            }
        }
    }
}

using ExcelComparison.UserControls.ExcelExport;
using ExcelComparison.UserControls.OverViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExcelComparison
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public OverViewDataModel overViewDM;
        public ExcelExportViewDataModel excelDM;
        public static ExcelExportView excelView;

        public MainWindow()
        {
            InitializeComponent();
            excelDM = new ExcelExportViewDataModel();
            overViewDM = new OverViewDataModel();

            excelView = new ExcelExportView();
        }

        private void ExportFile(object sender, RoutedEventArgs e)
        {
            excelView.DataContext = excelDM;
            excelView.ShowDialog();
            if (excelDM.result == System.Windows.Forms.DialogResult.OK)
            {
                overViewPage.LoadExcel(excelDM.LeftExcelPath, excelDM.RightExcelPath);
            }
            compareButton.IsEnabled = true;
            preButton.IsEnabled = true;
            nextButton.IsEnabled = true;
        }

        private void Compare(object sender, RoutedEventArgs e)
        {
            overViewPage.CompareTwoSheet();
        }

        private void PreDifferent(object sender, RoutedEventArgs e)
        {
            overViewPage.SelectPreviousDifference();
        }

        private void NextDifferent(object sender, RoutedEventArgs e)
        {
            overViewPage.SelectNextDifference();
        }
    }
}

using ExcelComparison.UserControls.ExcelExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace ExcelComparison.Helper
{
    public class WindowOpenHelper
    {
        //public static ExcelExportViewDataModel excelDM;
        public static ExcelExportView excelView;

        public WindowOpenHelper()
        {
            
        }
        internal static void ShowExportExcelVersionVew()
        {
            excelView = new ExcelExportView();
            //excelDM = new ExcelExportViewDataModel();
            //excelView.DataContext = excelDM;
            excelView.ShowDialog();
        }
    }
}

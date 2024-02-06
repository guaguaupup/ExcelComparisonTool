using ExcelComparison.Helper;
using ExcelComparison.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExcelComparison.UserControls.ExcelExport
{
    public class ExcelExportViewDataModel: ObservableObject
    {
        public DialogResult result;
        public ExcelExportViewDataModel()
        {

        }
        #region Property
        public const string FILE_FILTER = "所有Excel文件(*.xls;*.xlsx)|*.xls;*.xlsx|Excel工作簿(*.xlsx)|*.xlsx|Excel 97-2003 工作簿(*.xls)|*.xls";
        private static readonly string[] FILTER_NAMES = new string[]
        {
            ".xlsx",
            ".xls"
        };
        #endregion

        #region PropertyChanged
        private string leftExcelPath = "";
        public string LeftExcelPath
        {
            get { return leftExcelPath; }
            set
            {
                leftExcelPath = value;
                RaisePropertyChanged(() => LeftExcelPath);
            }
        }

        private string rightExcelPath = "";
        public string RightExcelPath
        {
            get { return rightExcelPath; }
            set
            {
                rightExcelPath = value;
                RaisePropertyChanged(() => RightExcelPath);
            }
        }

        private bool okEnable = false;
        public bool OKEnable
        {
            get { return okEnable; }
            set
            {
                okEnable = value;
                RaisePropertyChanged(() => OKEnable);
            }
        }
        #endregion

        #region Command
        private RelayCommand leftExcelExportCommand = null;
        public RelayCommand LeftExcelExportCommand
        {
            get { return leftExcelExportCommand = leftExcelExportCommand ?? new RelayCommand(ExportLeftExcelFile); }
        }

        private RelayCommand rightExcelExportCommand = null;
        public RelayCommand RightExcelExportCommand
        {
            get { return rightExcelExportCommand = rightExcelExportCommand ?? new RelayCommand(ExportRightExcelFile); }
        }

        private RelayCommand beginToCompare = null;
        public RelayCommand BeginToCompare
        {
            get { return beginToCompare = beginToCompare ?? new RelayCommand(ResetDialogResult); }
        }
        #endregion

        #region Tools Function
        private void ResetDialogResult()
        {
            result = DialogResult.OK;
            MainWindow.excelView.Hide();
            //MainWindow.excelView.Close();
        }

        private void ExportLeftExcelFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = FILE_FILTER;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LeftExcelPath = dialog.FileName;
                CheckInput();
            }
        }

        private void ExportRightExcelFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = FILE_FILTER;
            dialog.Multiselect = false; //该值确定是否可以选择多个文件
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                RightExcelPath = dialog.FileName;
                CheckInput();
            }
        }


        private void CheckInput()
        {
            bool allowStart = false;
            if (!string.IsNullOrEmpty(leftExcelPath) && !string.IsNullOrEmpty(rightExcelPath))
            {
                if (File.Exists(leftExcelPath) && File.Exists(rightExcelPath))
                {
                    allowStart = true;
                }
            }
            OKEnable = allowStart;
        }
        #endregion
    }
}

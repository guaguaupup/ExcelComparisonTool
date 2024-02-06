using DiffMatchPatch;
using ExcelComparison.Helper;
using ExcelComparison.UserControls;
using ExcelComparison.UserControls.ExcelExport;
using ExcelComparison.UserControls.OverViews;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelComparison.ViewModel
{
    public  class MainWindowViewModel
    {
        public OverViewDataModel overViewDM;
        public ExcelExportViewDataModel excelDM;
        public static ExcelExportView excelView;
        public MainWindowViewModel()
        {
            excelDM = new ExcelExportViewDataModel();
            overViewDM = new OverViewDataModel();

            excelView = new ExcelExportView();
        }


        #region Mode Bind
        /// <summary>
        /// 顶部SystemConfig 菜单项
        /// </summary>
        private RelayCommand<object> clickCommand = null;
        public RelayCommand<object> ClickCommand
        {
            get { return clickCommand = clickCommand ?? new RelayCommand<object>(ExecuteMenu); }
        }

        private void ExecuteMenu(object obj)
        {
            try
            {
                string index = obj?.ToString();
                if (!string.IsNullOrEmpty(index))
                {
                    switch (index)
                    {
                        case "0":
                            //excelView.DataContext = excelDM;
                            //excelView.ShowDialog();
                            //if (excelDM.result == System.Windows.Forms.DialogResult.OK)
                            //{
                            //    overViewDM.LoadExcel(excelDM.LeftExcelPath, excelDM.RightExcelPath);
                            //}
                            return;
                    }
                }

            }
            catch (Exception ex)
            {
                NewMessageBox.ShowErrorMessage(ex.Message);
            }
        }
        
        #endregion

        
    }
}

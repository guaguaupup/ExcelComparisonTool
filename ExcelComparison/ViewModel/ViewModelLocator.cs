using ExcelComparison.UserControls.ExcelExport;
using ExcelComparison.UserControls.OverViews;
using ExcelComparison.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelComparison
{
    public class ViewModelLocator 
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<ExcelExportViewDataModel>();
            SimpleIoc.Default.Register<OverViewDataModel>();
        }

        public MainWindowViewModel MainWindowViewModel
        {
            get { return ServiceLocator.Current.GetInstance<MainWindowViewModel>(); }
        }

        public ExcelExportViewDataModel ExcelExportViewDataModel
        {
            get { return ServiceLocator.Current.GetInstance<ExcelExportViewDataModel>(); }
        }

        public OverViewDataModel OverViewDataModel
        {
            get { return ServiceLocator.Current.GetInstance<OverViewDataModel>(); }
        }
    }
}

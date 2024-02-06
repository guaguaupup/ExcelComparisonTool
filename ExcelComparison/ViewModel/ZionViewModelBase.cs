using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelComparison.ViewModel
{
    class ZionViewModelBase : ViewModelBase
    {
        /// <summary>
        /// 这个属性绑定了窗口的附加属性ViewCloser.DialogResult,对其赋值就可以关闭窗口
        /// </summary>
        private bool? dialogResult = null;
        public bool? DialogResult
        {
            get { return dialogResult; }
            set
            {
                dialogResult = value;
                RaisePropertyChanged(() => DialogResult);
            }
        }

        internal void RecoverData()
        {
            //如果ViewModel 使用了单例，则在窗口关闭后或在窗口打开前检查这个属性值，
            //如果值不为Null会导致窗口无法显示或者无法关闭
            //需要在窗口打开前将其重置为Null
            DialogResult = null;
        }
    }
}

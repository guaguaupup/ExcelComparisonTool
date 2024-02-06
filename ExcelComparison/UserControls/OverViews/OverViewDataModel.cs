using DiffMatchPatch;
using ExcelComparison.Helper;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelComparison.UserControls.OverViews
{
    public class OverViewDataModel : ObservableObject
    {
        private ExcelWorkbook leftWorkbook;
        private ExcelWorkbook rightWorkbook;
        private Dictionary<string, bool> _sheetDifferences = new Dictionary<string, bool>();
        public ObservableCollection<string> leftSheetList { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> rightSheetList { get; set; } = new ObservableCollection<string>();

        public ObservableCollection<DataTable> leftDT { get; set; } = new ObservableCollection<DataTable>();
        public ObservableCollection<DataTable> rightDT { get; set; } = new ObservableCollection<DataTable>();

        //public DataTable leftDT { get; set; } 
        //public DataTable rightDT { get; set; }
        private SheetComparer currentSheet = null;
        private bool stopUpdate = false;

        public OverViewDataModel()
        {
            
        }

        #region Mode Bind
        private string leftFileName = "";
        public string LeftFileName
        {
            get { return leftFileName; }
            set
            {
                leftFileName = value;
                RaisePropertyChanged(() => LeftFileName);
            }
        }

        private string rightFileName = "";
        public string RightFileName
        {
            get { return rightFileName; }
            set
            {
                rightFileName = value;
                RaisePropertyChanged(() => RightFileName);
            }
        }

        public ObservableCollection<DataTable> LeftDT
        {
            get { return leftDT; }
            set
            {
                leftDT = value;
                RaisePropertyChanged(() => LeftDT);
            }
        }

        public ObservableCollection<DataTable> RightDT
        {
            get { return rightDT; }
            set
            {
                rightDT = value;
                RaisePropertyChanged(() => RightDT);
            }
        }

        //public DataTable LeftDT
        //{
        //    get { return leftDT; }
        //    set
        //    {
        //        leftDT = value;
        //        RaisePropertyChanged(() => LeftDT);
        //    }
        //}

        //public DataTable RightDT
        //{
        //    get { return rightDT; }
        //    set
        //    {
        //        rightDT = value;
        //        RaisePropertyChanged(() => RightDT);
        //    }
        //}
        #endregion

        #region Tools
        public void LoadExcel(string leftPath, string rightPath)
        {
            leftWorkbook = new ExcelWorkbook();
            leftWorkbook.Load(leftPath);

            rightWorkbook = new ExcelWorkbook();
            rightWorkbook.Load(rightPath);

            //GetSummary();

            //CompareSheet(leftWorkbook.sheetNames[0], rightWorkbook.sheetNames[0]);

            leftSheetList.Clear();
            rightSheetList.Clear();

            for (int i = 0; i < leftWorkbook.sheetNames.Count; i++)
            {
                string sheetName = leftWorkbook.sheetNames[i];
                leftSheetList.Add(sheetName + (IsSheetDifferent(sheetName) ? " *" : string.Empty));
            }

            //leftComboBox.SelectedIndex = 0;
            //rightComboBox.SelectedIndex = 0;


            for (int i = 0; i < rightWorkbook.sheetNames.Count; i++)
            {
                string sheetName = rightWorkbook.sheetNames[i];
                rightSheetList.Add(sheetName + (IsSheetDifferent(sheetName) ? " *" : string.Empty));
            }


            LeftFileName = leftPath;
            RightFileName = rightPath;
        }

        private void GetSummary()
        {
            _sheetDifferences.Clear();
            for (int i = 0; i < leftWorkbook.sheetNames.Count; i++)
            {
                string sheetName = leftWorkbook.sheetNames[i];
                if (rightWorkbook.sheetNames.Contains(sheetName))
                {
                    ExcelSheet leftExcelSheet = leftWorkbook.LoadSheet(sheetName);
                    ExcelSheet rightExcelSheet = rightWorkbook.LoadSheet(sheetName);
                    int columnCount = Math.Max(leftExcelSheet.columnCount, rightExcelSheet.columnCount);
                    VSheet leftSheet = new VSheet(leftExcelSheet, columnCount);
                    VSheet rightSheet = new VSheet(rightExcelSheet, columnCount);

                    diff_match_patch comparer = new diff_match_patch();
                    string leftContent = leftSheet.GetContent();
                    string rightContent = rightSheet.GetContent();
                    List<Diff> diffs = comparer.diff_main(leftContent, rightContent, true);
                    comparer.diff_cleanupSemanticLossless(diffs);

                    bool isDifferent = false;
                    for (int diffIndex = 0; diffIndex < diffs.Count; diffIndex++)
                    {
                        if (diffs[diffIndex].operation != Operation.EQUAL)
                        {
                            isDifferent = true;
                            break;
                        }
                    }

                    _sheetDifferences.Add(sheetName, isDifferent);
                }
            }
        }

        private void CompareSheet(string leftSheetName, string rightSheetName)
        {
            ClearAlignment();
            currentSheet = new SheetComparer();
            currentSheet.Execute(leftWorkbook.LoadSheet(leftSheetName), rightWorkbook.LoadSheet(rightSheetName));
            stopUpdate = true;

            LeftDT.Add(currentSheet.left.GetSource());
            RightDT.Add(currentSheet.right.GetSource());
            stopUpdate = false;

            //differentTable = new DataTable();

            //for (int i = 0; i < currentSheet.columnCount; i++)
            //{
            //    differentTable.Columns.Add(new DataColumn(CellReference.ConvertNumToColString(i)));
            //}

            //rowDiffGrid.DataSource = differentTable;

            //UpdateNextPreviousButton();
            //AdjustRowHeaderSize();
            //compareToolStripButton.Enabled = false;
            //UpdateGridViewFocus();
            //differenceScrollBar.ForceUpdate();
            //differenceScrollBar.Selection = -1;
        }

        private bool IsSheetDifferent(string name)
        {
            bool different = false;
            if (!_sheetDifferences.TryGetValue(name, out different))
            {
                different = false;
            }
            return different;
        }

        private void ClearAlignment()
        {
            //leftGrid.Cursor = Cursors.Default;
            //rightGrid.Cursor = Cursors.Default;
            //_leftAlignIndex = -1;
            //_rightAlignIndex = -1;
        }
        #endregion
    }
}

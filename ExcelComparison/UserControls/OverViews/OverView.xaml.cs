using DiffMatchPatch;
using ExcelComparison.Helper;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExcelComparison.UserControls
{
    /// <summary>
    /// OverView.xaml 的交互逻辑
    /// </summary>
    public partial class OverView : UserControl
    {
        private ExcelWorkbook leftWorkbook;
        private ExcelWorkbook rightWorkbook;
        private Dictionary<string, bool> _sheetDifferences = new Dictionary<string, bool>();
        private SheetComparer currentSheet = null;
        private bool stopUpdate = false;
        private DataTable differentTable = new DataTable();
        private DataGrid updatingGrid = null;

        //private int leftRow = 0;
        //private int leftColumn = 0;
        //private int rightRow = 0;
        //private int rightColumn = 0;
        private int compareRow = 0;
        private int compareColumn = 0;
        //private DataGrid differentGrid;
        //private DataGrid otherGrid;
        ScrollViewer sv1, sv2;
        private List<int> differentRowCollect;
        private Dictionary<string, int> gridCountDic = new Dictionary<string, int>();
        public OverView()
        {
            InitializeComponent();
            
        }

        #region Tools
        public void LoadExcel(string leftPath, string rightPath)
        {
            leftWorkbook = new ExcelWorkbook();
            leftWorkbook.Load(leftPath);

            rightWorkbook = new ExcelWorkbook();
            rightWorkbook.Load(rightPath);

            GetSummary();

            CompareSheet(leftWorkbook.sheetNames[0], rightWorkbook.sheetNames[0]);

            leftSheetList.Items.Clear();
            rightSheetList.Items.Clear();

            for (int i = 0; i < leftWorkbook.sheetNames.Count; i++)
            {
                string sheetName = leftWorkbook.sheetNames[i];
                leftSheetList.Items.Add(sheetName + (IsSheetDifferent(sheetName) ? " *" : string.Empty));
            }

            leftSheetList.SelectedIndex = 0;

            differentNote.Text = $"您选择的Sheet名称叫做{leftSheetList.Items[0].ToString()}";

            for (int i = 0; i < rightWorkbook.sheetNames.Count; i++)
            {
                string sheetName = rightWorkbook.sheetNames[i];
                rightSheetList.Items.Add(sheetName + (IsSheetDifferent(sheetName) ? " *" : string.Empty));
            }
            rightSheetList.SelectedIndex = 0;


            leftfilePath.Text = leftPath;
            rightfilePath.Text = rightPath;
        }

        private void GetSummary()
        {
            _sheetDifferences.Clear();
            gridCountDic.Clear();
            for (int i = 0; i < leftWorkbook.sheetNames.Count; i++)
            {
                string sheetName = leftWorkbook.sheetNames[i];
                if (rightWorkbook.sheetNames.Contains(sheetName))
                {
                    ExcelSheet leftExcelSheet = leftWorkbook.LoadSheet(sheetName);
                    ExcelSheet rightExcelSheet = rightWorkbook.LoadSheet(sheetName);
                    int columnCount = System.Math.Max(leftExcelSheet.columnCount, rightExcelSheet.columnCount);
                    VSheet leftSheet = new VSheet(leftExcelSheet, columnCount);
                    VSheet rightSheet = new VSheet(rightExcelSheet, columnCount);

                    //leftRow = leftSheet.rowCount;
                    //leftColumn = leftSheet.columnCount;
                    //rightRow = rightSheet.rowCount;
                    //rightColumn = rightSheet.columnCount;

                    

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
            currentSheet = new SheetComparer();
            currentSheet.Execute(leftWorkbook.LoadSheet(leftSheetName), rightWorkbook.LoadSheet(rightSheetName));
            stopUpdate = true;

            
            leftGrid.ItemsSource = currentSheet.left.GetSource().DefaultView;
            rightGrid.ItemsSource = currentSheet.right.GetSource().DefaultView;
            GridInfor leftGridInfo = new GridInfor(currentSheet.left.rowCount, currentSheet.left.columnCount, leftGrid);
            GridInfor rightGridInfo = new GridInfor(currentSheet.right.rowCount, currentSheet.right.columnCount, rightGrid);

            //获取当前表中的最多的行数和列数
            compareRow = currentSheet.left.rowCount > currentSheet.right.rowCount ? currentSheet.left.rowCount : currentSheet.right.rowCount;
            compareColumn = currentSheet.left.columnCount > currentSheet.right.columnCount ? currentSheet.left.columnCount : currentSheet.right.columnCount;

            UseTheScrollViewerScrolling(leftGridInfo.currentGrid, rightGridInfo.currentGrid);

            stopUpdate = false;

            DrawDifferent(leftGridInfo, rightGridInfo);


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




        /// <summary>
        /// load sheet and compare
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CompareTwoSheet()
        {
            int leftSelectionIndex = leftSheetList.SelectedIndex;
            int rightSelectionIndex = leftSheetList.SelectedIndex;
            if (leftSelectionIndex == -1 || rightSelectionIndex == -1)
            {
                return;
            }

            CompareSheet(leftWorkbook.sheetNames[leftSelectionIndex], rightWorkbook.sheetNames[rightSelectionIndex]);
        }

        private string GetLeftSheetName()
        {
            string sheetName = string.Empty;
            if (leftWorkbook != null && leftSheetList.SelectedIndex >= 0 && leftSheetList.SelectedIndex < leftWorkbook.sheetNames.Count)
            {
                sheetName = leftWorkbook.sheetNames[leftSheetList.SelectedIndex];
            }
            differentNote.Text = $"您选择的Sheet是{sheetName}";
            return sheetName;
        }

        private string GetRightSheetName()
        {
            string sheetName = string.Empty;
            if (rightWorkbook != null && rightSheetList.SelectedIndex >= 0 && rightSheetList.SelectedIndex < rightWorkbook.sheetNames.Count)
            {
                sheetName = rightWorkbook.sheetNames[rightSheetList.SelectedIndex];
            }
            return sheetName;
        }

        //private void DrawDifferent(DataGrid differentGrid, DataGrid otherGrid)
        //{
        //    int differentCount = 0;
        //    differentNote.Text = $"{leftSheetList.Text}表格中的差异如下：\n";
        //    for (int i = 0; i < compareRow; i++)
        //    {
        //        if (currentSheet.IsRowDifferent(i))
        //        {
        //            for (int j = 0; j < compareColumn; j++)
        //            {
        //                if (currentSheet.IsCellDifferent(i, j))
        //                {
        //                    differentCount++;

        //                    //不同的地方上色
        //                    DataGridRow rowContainer = (DataGridRow)differentGrid.ItemContainerGenerator.ContainerFromIndex(i);
        //                    if (rowContainer == null)
        //                    {
        //                        differentGrid.UpdateLayout();
        //                        rowContainer = (DataGridRow)differentGrid.ItemContainerGenerator.ContainerFromIndex(i);
        //                        rowContainer.Foreground = new SolidColorBrush(Colors.Red);

        //                    }

        //                    differentNote.Text += $" 第{differentCount}处不同: 第{i - 1}行，第{j + 1}列 \n";
        //                }
        //            }
        //        }
        //    }
        //    if (differentCount == 0)
        //    {
        //        differentNote.Text = $"您选择的Sheet名称叫做{leftSheetList.Text}, 当前Sheet下没有不同元素!";
        //    }
        //}

        private void DrawDifferent(GridInfor leftGridInfo, GridInfor rightGridInfo)
        {
            int differentCount = 0;
            differentRowCollect = new List<int>();

            differentNote.Text = $"{leftSheetList.Text}表格中的差异如下：\n";
            for (int i = 0; i < compareRow; i++)
            {
                if (currentSheet.IsRowDifferent(i))
                {
                    differentCount++;
                    differentRowCollect.Add(i);

                    for (int j = 0; j < compareColumn; j++)
                    {
                        if (currentSheet.IsCellDifferent(i, j))
                        {

                            

                            differentNote.Text += $" 第{differentCount}处不同: 第{i+1}行，第{j+1}列 \n";
                        }
                    }//j

                    


                    ////跳转到最后一个不同行
                    //if (i > leftGridInfo.rowCount)
                    //{
                    //    rightGridInfo.currentGrid.ScrollIntoView(rightGridInfo.currentGrid.Items[i]);
                    //    leftGridInfo.currentGrid.ScrollIntoView(leftGridInfo.currentGrid.Items[leftGridInfo.rowCount - 1]);
                    //}
                    //else if (i > rightGridInfo.rowCount)
                    //{
                    //    leftGridInfo.currentGrid.ScrollIntoView(leftGridInfo.currentGrid.Items[i]);
                    //    rightGridInfo.currentGrid.ScrollIntoView(rightGridInfo.currentGrid.Items[rightGridInfo.rowCount - 1]);
                    //}
                    //else if (i <= leftGridInfo.rowCount && i <= rightGridInfo.rowCount)
                    //{
                    //    rightGridInfo.currentGrid.ScrollIntoView(rightGridInfo.currentGrid.Items[i]);
                    //    leftGridInfo.currentGrid.ScrollIntoView(leftGridInfo.currentGrid.Items[i]);
                    //}
                }
            }

            DrawDifferentLine(differentRowCollect, leftGridInfo, rightGridInfo);

            if (differentCount == 0)
            {
                differentNote.Text = $"您选择的Sheet名称叫做{leftSheetList.Text}, 当前Sheet下没有不同元素!";
            }
        }


        public void SelectNextDifference()
        {
            int rowIndex = GetNextDifferenctRowIndex();
            if (rowIndex != -1)
            {
                JumpToRow(rowIndex);
            }
        }

        public void SelectPreviousDifference()
        {
            int rowIndex = GetPreviousDifferenctRowIndex();
            if (rowIndex != -1)
            {
                JumpToRow(rowIndex);
            }
        }

        /// <summary>
        /// 默认从最后一行开始选
        /// </summary>
        /// <returns></returns>
        private int GetPreviousDifferenctRowIndex()
        {
            int currentRowIndex = compareRow;
            for (int i = currentRowIndex - 1; i >= 0; i--)
            {
                if (currentSheet.IsRowDifferent(i))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 默认从首行开始
        /// </summary>
        /// <returns></returns>
        private int GetNextDifferenctRowIndex()
        {
            int currentRowIndex = 0;
            for (int i = currentRowIndex + 1; i < currentSheet.rowCount; i++)
            {
                if (currentSheet.IsRowDifferent(i))
                {
                    return i;
                }
            }
            return -1;
        }

        private void JumpToRow(int rowIndex)
        {
            //if (differentGrid == null)
            //{
            //    NewMessageBox.ShowWarningMessage("请先点击比较按钮进行比较！");
            //    return;
            //}
            //differentGrid.SelectedIndex = rowIndex;
            //differentGrid.UpdateLayout();
            //differentGrid.ScrollIntoView(differentGrid.SelectedItem);
        }

        //private void DrawDifferentLine(List<int> differentRow, GridInfor leftGridInfo, GridInfor rightGridInfo)
        //{
        //    rightGridInfo.currentGrid.UpdateLayout();
        //    leftGridInfo.currentGrid.UpdateLayout();
        //    foreach (var i in differentRow)
        //    {
        //        //不同的地方上色
        //        if (i > leftGridInfo.rowCount)
        //        {
        //            DataGridRow rowContainer = (DataGridRow)rightGridInfo.currentGrid.ItemContainerGenerator.ContainerFromIndex(i);
        //            if (rowContainer == null)
        //            {
        //                //rightGridInfo.currentGrid.UpdateLayout();
        //                rowContainer = (DataGridRow)rightGridInfo.currentGrid.ItemContainerGenerator.ContainerFromIndex(i);
        //                rowContainer.Foreground = new SolidColorBrush(Colors.Red);
        //                rowContainer.FontWeight = FontWeights.Bold;
        //            }
        //        }
        //        else if (i > rightGridInfo.rowCount)
        //        {
        //            DataGridRow rowContainer = (DataGridRow)leftGridInfo.currentGrid.ItemContainerGenerator.ContainerFromIndex(i);
        //            if (rowContainer == null)
        //            {
        //                //leftGridInfo.currentGrid.UpdateLayout();
        //                rowContainer = (DataGridRow)leftGridInfo.currentGrid.ItemContainerGenerator.ContainerFromIndex(i);
        //                rowContainer.Foreground = new SolidColorBrush(Colors.Red);
        //                rowContainer.FontWeight = FontWeights.Bold;
        //            }
        //        }
        //        else if (i <= leftGridInfo.rowCount && i <= rightGridInfo.rowCount)
        //        {
        //            DataGridRow rowContainerLeft = (DataGridRow)rightGridInfo.currentGrid.ItemContainerGenerator.ContainerFromIndex(i);
        //            //if (rowContainerLeft == null)
        //            //{
        //            //    //rightGridInfo.currentGrid.UpdateLayout();
        //            //    rowContainerLeft = (DataGridRow)rightGridInfo.currentGrid.ItemContainerGenerator.ContainerFromIndex(i);
        //            //    rowContainerLeft.Background = new SolidColorBrush(Colors.Red);
        //            //    rowContainerLeft.FontWeight = FontWeights.Bold;
        //            //}
        //            rowContainerLeft = (DataGridRow)rightGridInfo.currentGrid.ItemContainerGenerator.ContainerFromIndex(i);
        //            rowContainerLeft.Background = new SolidColorBrush(Colors.Red);
        //            rowContainerLeft.FontWeight = FontWeights.Bold;
        //            DataGridRow rowContainerRight = (DataGridRow)leftGridInfo.currentGrid.ItemContainerGenerator.ContainerFromIndex(i);
        //            //if (rowContainerRight == null)
        //            //{
        //            //    leftGridInfo.currentGrid.UpdateLayout();
        //            //    rowContainerRight = (DataGridRow)leftGridInfo.currentGrid.ItemContainerGenerator.ContainerFromIndex(i);
        //            //    rowContainerRight.Background = new SolidColorBrush(Colors.Red);
        //            //    rowContainerRight.FontWeight = FontWeights.Bold;
        //            //}
        //            leftGridInfo.currentGrid.UpdateLayout();
        //            rowContainerRight = (DataGridRow)leftGridInfo.currentGrid.ItemContainerGenerator.ContainerFromIndex(i);
        //            rowContainerRight.Background = new SolidColorBrush(Colors.Red);
        //            rowContainerRight.FontWeight = FontWeights.Bold;
        //        }
        //    }
        //}

        private void DrawDifferentLine(List<int> differentRow, GridInfor leftGridInfo, GridInfor rightGridInfo)
        {
            rightGridInfo.currentGrid.UpdateLayout();
            leftGridInfo.currentGrid.UpdateLayout();
            foreach (var i in differentRow)
            {
                DataGridRow rowContainerLeft = (DataGridRow)rightGridInfo.currentGrid.ItemContainerGenerator.ContainerFromIndex(i);
                rowContainerLeft = (DataGridRow)rightGridInfo.currentGrid.ItemContainerGenerator.ContainerFromIndex(i);
                rowContainerLeft.Background = new SolidColorBrush(Colors.Red);
                rowContainerLeft.FontWeight = FontWeights.Bold;
                DataGridRow rowContainerRight = (DataGridRow)leftGridInfo.currentGrid.ItemContainerGenerator.ContainerFromIndex(i);
                leftGridInfo.currentGrid.UpdateLayout();
                rowContainerRight = (DataGridRow)leftGridInfo.currentGrid.ItemContainerGenerator.ContainerFromIndex(i);
                rowContainerRight.Background = new SolidColorBrush(Colors.Red);
                rowContainerRight.FontWeight = FontWeights.Bold;
            }
        }

        //两个grid实现同步滚动

        void sv1_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            sv2.ScrollToHorizontalOffset(sv1.HorizontalOffset);
        }

        void sv2_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            sv1.ScrollToHorizontalOffset(sv2.HorizontalOffset);
        }


        public static ScrollViewer GetScrollViewer(UIElement element)
        {
            if (element == null) return null;

            ScrollViewer retour = null;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element) && retour == null; i++)
            {
                if (VisualTreeHelper.GetChild(element, i) is ScrollViewer)
                {
                    retour = (ScrollViewer)(VisualTreeHelper.GetChild(element, i));
                }
                else
                {
                    retour = GetScrollViewer(VisualTreeHelper.GetChild(element, i) as UIElement);
                }
            }
            return retour;
        }
        #endregion



        #region component changed
        private void SelectSheet(object sender, SelectionChangedEventArgs e)
        {
            if (currentSheet == null)
            {
                return;
            }
            string leftName = GetLeftSheetName();
            string rightName = GetRightSheetName();
            ComboBox target = null;
            if (sender == leftSheetList)
            {
                target = rightSheetList;
            }
            else
            {
                target = leftSheetList;
            }

            if (sender == leftSheetList && _sheetDifferences.ContainsKey(leftName))
            {
                target.SelectedItem = ((ComboBox)sender).SelectedItem;
            }
            if (sender == rightSheetList && _sheetDifferences.ContainsKey(rightName))
            {
                target.SelectedItem = ((ComboBox)sender).SelectedItem;
            }
        }

        //实现滑轮滚动
        public void UseTheScrollViewerScrolling(FrameworkElement fElement1, FrameworkElement fElement2)
        {
            
            fElement1.PreviewMouseWheel += (sender, e) =>
            {
                var eventArgLeft = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArgLeft.RoutedEvent = UIElement.MouseWheelEvent;
                eventArgLeft.Source = sender;
                fElement1.RaiseEvent(eventArgLeft);

                var eventArgRight = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArgRight.RoutedEvent = UIElement.MouseWheelEvent;
                eventArgRight.Source = fElement2;
                fElement2.RaiseEvent(eventArgRight);
            };
            fElement2.PreviewMouseWheel += (sender, e) =>
            {
                var eventArgRight = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArgRight.RoutedEvent = UIElement.MouseWheelEvent;
                eventArgRight.Source = sender;
                fElement2.RaiseEvent(eventArgRight);

                var eventArgLeft = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArgLeft.RoutedEvent = UIElement.MouseWheelEvent;
                eventArgLeft.Source = fElement1;
                fElement1.RaiseEvent(eventArgLeft);
            };

        }
        

        private void OnGridViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        #endregion


    }
    
}

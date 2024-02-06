using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ExcelComparison.Helper
{
   public class GridInfor
    {
        public int rowCount;
        public int columnCount;
        public DataGrid currentGrid;

        public GridInfor(int rowCount, int columnCount, DataGrid currentGrid)
        {
            this.rowCount = rowCount;
            this.columnCount = columnCount;
            this.currentGrid = currentGrid;
        }
    }
}

using System.Data;
using System.Text;

namespace ITCE.SNOW.Data.Excel.Extensions
{
    public static class DataRowExtensions
    {
        public static string ToReference(this DataRow dataRow)
        {
            var builder = new StringBuilder();
            foreach (DataColumn column in dataRow.Table.Columns)
            {
                builder.AppendFormat("{0}:{1},", column.ColumnName, dataRow[column]);
            }
            return builder.ToString().Trim(',');
        }
    }
}

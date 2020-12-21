using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Bán_hàng.Areas.Admin.Models
{
    public class Sql
    {
        public static DataTable GetDataTable(string Sql)
        {
            SqlConnection Connection = new SqlConnection(@"Data Source=HIEU-PC\SQLEXPRESS;
            Initial Catalog = BanHang;Integrated Security = True");
            var Data = new SqlDataAdapter(Sql, Connection);
            var DataTable = new DataTable();
            Data.Fill(DataTable);
            return DataTable;
        }
    }
}

using System;
using System.Data.SqlClient;

namespace TheGioiTho.Config
{
    public static class DBConnection
    {
        // Chuỗi kết nối tới cơ sở dữ liệu từ Settings
        private static readonly string connectionString = @"Data Source=LAPTOP-7H9D7KEU\CSDL_SQLSEVER;Initial Catalog=DoAnTheGioiTho1;User ID=sa;Password=haolkj123;Encrypt=False";

        // Phương thức tạo kết nối tới cơ sở dữ liệu
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}

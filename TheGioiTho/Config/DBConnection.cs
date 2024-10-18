using System;
using System.Data.SqlClient;

namespace TheGioiTho.Config
{
    public static class DBConnection
    {
        // Chuỗi kết nối tới cơ sở dữ liệu từ Settings
        private static readonly string connectionString = Properties.Settings.Default.conn;

        // Phương thức tạo kết nối tới cơ sở dữ liệu
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}

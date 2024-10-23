using System;
using System.Data.SqlClient;
using System.Drawing;

namespace TheGioiTho.Config
{
    public static class DBConnection
    {
        // Chuỗi kết nối tới cơ sở dữ liệu từ Settings
        private static readonly string connectionString = @"Server=LAPTOP-M10LPRA9\CONGDON;Initial Catalog=TheGioiTho;Integrated Security=True;TrustServerCertificate=True";

        // Phương thức tạo kết nối tới cơ sở dữ liệu
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }

}

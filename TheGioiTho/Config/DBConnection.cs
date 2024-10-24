using System;
using System.Data.SqlClient;

namespace TheGioiTho.Config
{
    public static class DBConnection
    {
        // Chuỗi kết nối tới cơ sở dữ liệu từ Settings
<<<<<<< HEAD
        private static readonly string connectionString = @"Data Source=LAPTOP-QTEB4KQ5\SQLEXPRESS;Initial Catalog=TheGioiTho1;Integrated Security=True;TrustServerCertificate=True";
=======
        private static readonly string connectionString = "Data Source=LAPTOP-DTKDJMOS\\SQLEXPRESS;Initial Catalog=TheGioiTho1;Integrated Security=True";
>>>>>>> d6fa94791ae98e8f1752eb22ba4133fd01084daa

        // Phương thức tạo kết nối tới cơ sở dữ liệu
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}

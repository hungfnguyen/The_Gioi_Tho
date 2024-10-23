using System;
using System.Data.SqlClient;
using TheGioiTho.Config;
using TheGioiTho.Model;

namespace TheGioiTho.DAO
{
    public class BaiDangNguoiDungDAO
    {
        public bool ThemBaiDangNguoiDung(BaiDangNguoiDung baiDangNguoiDung)
        {
            string query = "INSERT INTO BaiDangNguoiDung (IDBaiDang, IDNguoiDung, NgayThoDen, GioThoDen) VALUES (@IDBaiDang, @IDNguoiDung, @NgayThoDen, @GioThoDen)";
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IDBaiDang", baiDangNguoiDung.IDBaiDang);
                    cmd.Parameters.AddWithValue("@IDNguoiDung", baiDangNguoiDung.IDNguoiDung);
                    cmd.Parameters.AddWithValue("@NgayThoDen", baiDangNguoiDung.NgayThoDen);
                    cmd.Parameters.AddWithValue("@GioThoDen", baiDangNguoiDung.GioThoDen);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }

        public bool XoaBaiDangNguoiDung(int idBaiDang)
        {
            string query = "DELETE FROM BaiDangNguoiDung WHERE IDBaiDang = @IDBaiDang";
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IDBaiDang", idBaiDang);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }
    }
}
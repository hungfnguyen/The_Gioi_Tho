using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using TheGioiTho.Model;
using TheGioiTho.Config;

namespace TheGioiTho.DAO
{
    public class DanhGiaDAO
    {
        public bool ThemDanhGia(DanhGia danhGia)
        {
            const string query = @"
            INSERT INTO DanhGia (IDNguoiDung, IDCongViec, SoSao, NhanXet, HinhAnh)
            VALUES (@IDNguoiDung, @IDCongViec, @SoSao, @NhanXet, @HinhAnh)";

            using (SqlConnection conn = DBConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                try
                {
                    cmd.Parameters.AddWithValue("@IDNguoiDung", danhGia.IDNguoiDung);
                    cmd.Parameters.AddWithValue("@IDCongViec", danhGia.IDLichHen);
                    cmd.Parameters.AddWithValue("@SoSao", danhGia.SoSao);
                    cmd.Parameters.AddWithValue("@NhanXet", (object)danhGia.NhanXet ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@HinhAnh", (object)danhGia.HinhAnh ?? DBNull.Value);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi khi thêm đánh giá: {ex.Message}");
                }
            }
        }

        public DanhGia GetDanhGiaByIDCongViec(int idCongViec)
        {
            const string query = "SELECT * FROM View_ChiTietDanhGia WHERE IDCongViec = @IDCongViec";


            using (SqlConnection conn = DBConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IDCongViec", idCongViec);

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapDanhGiaFromReader(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi khi lấy đánh giá: {ex.Message}");
                }
                return null;
            }
        }

        public List<DanhGia> GetDanhGiaByIDTho(int idTho)
        {
            const string query = "SELECT * FROM View_ChiTietDanhGia WHERE IDTho = @IDTho";

            var danhSachDanhGia = new List<DanhGia>();

            using (SqlConnection conn = DBConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IDTho", idTho);

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            danhSachDanhGia.Add(MapDanhGiaFromReader(reader));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi khi lấy danh sách đánh giá: {ex.Message}");
                }
            }
            return danhSachDanhGia;
        }

        public decimal TinhDiemTrungBinh(int idTho)
        {
            const string query = "SELECT DiemTrungBinh FROM View_DiemTrungBinhTho WHERE IDTho = @IDTho";


            using (SqlConnection conn = DBConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IDTho", idTho);
                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return result != null && result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi khi tính điểm trung bình: {ex.Message}");
                }
            }
        }

        private DanhGia MapDanhGiaFromReader(SqlDataReader reader)
        {
            return new DanhGia
            {
                IDNguoiDung = reader.GetInt32(reader.GetOrdinal("IDNguoiDung")),
                IDLichHen = reader.GetInt32(reader.GetOrdinal("IDCongViec")), // Vẫn giữ IDCongViec từ DB
                SoSao = reader.GetInt32(reader.GetOrdinal("SoSao")),
                NhanXet = reader.IsDBNull(reader.GetOrdinal("NhanXet")) ?
                    null : reader.GetString(reader.GetOrdinal("NhanXet")),
                HinhAnh = reader.IsDBNull(reader.GetOrdinal("HinhAnh")) ?
                    null : reader.GetString(reader.GetOrdinal("HinhAnh"))
            };
        }
    }
}
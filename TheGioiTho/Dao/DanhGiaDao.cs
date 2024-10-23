using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using TheGioiTho.Model;
using TheGioiTho.Config;

namespace TheGioiTho.DAO
{
    public class DanhGiaDAO
    {
        // Thêm đánh giá mới
        public bool ThemDanhGia(DanhGia danhGia)
        {
            string query = @"
                INSERT INTO DanhGia (IDNguoiDung, IDCongViec, SoSao, NhanXet, HinhAnh)
                VALUES (@IDNguoiDung, @IDCongViec, @SoSao, @NhanXet, @HinhAnh)";

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@IDNguoiDung", danhGia.IDNguoiDung);
                        cmd.Parameters.AddWithValue("@IDCongViec", danhGia.IDLichHen);
                        cmd.Parameters.AddWithValue("@SoSao", danhGia.SoSao);
                        cmd.Parameters.AddWithValue("@NhanXet",
                            (object)danhGia.NhanXet ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@HinhAnh",
                            (object)danhGia.HinhAnh ?? DBNull.Value);

                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in ThemDanhGia: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        // Lấy đánh giá theo IDCongViec
        public DanhGia GetDanhGiaByIDCongViec(int idCongViec)
        {
            string query = @"
                SELECT dg.*, nd.HoTen as TenNguoiDung
                FROM DanhGia dg
                JOIN NguoiDung nd ON dg.IDNguoiDung = nd.IDNguoiDung
                WHERE dg.IDCongViec = @IDCongViec";

            using (SqlConnection conn = DBConnection.GetConnection())
            {
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
                                return new DanhGia
                                {
                                    IDNguoiDung = reader.GetInt32(reader.GetOrdinal("IDNguoiDung")),
                                    IDLichHen = idCongViec,
                                    SoSao = reader.GetInt32(reader.GetOrdinal("SoSao")),
                                    NhanXet = reader.IsDBNull(reader.GetOrdinal("NhanXet")) ?
                                        null : reader.GetString(reader.GetOrdinal("NhanXet")),
                                    HinhAnh = reader.IsDBNull(reader.GetOrdinal("HinhAnh")) ?
                                        null : reader.GetString(reader.GetOrdinal("HinhAnh"))
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in GetDanhGiaByIDCongViec: {ex.Message}");
                        throw;
                    }
                }
            }
            return null;
        }

        // Lấy danh sách đánh giá của một thợ
        public List<DanhGia> GetDanhGiaByIDTho(int idTho)
        {
            List<DanhGia> danhSachDanhGia = new List<DanhGia>();
            string query = @"
                SELECT dg.*, nd.HoTen as TenNguoiDung
                FROM DanhGia dg
                JOIN NguoiDung nd ON dg.IDNguoiDung = nd.IDNguoiDung
                JOIN CongViec cv ON dg.IDCongViec = cv.IDCongViec
                JOIN BaiDang bd ON cv.IDBaiDang = bd.IDBaiDang
                JOIN BaiDangTho bdt ON bd.IDBaiDang = bdt.IDBaiDang
                WHERE bdt.IDTho = @IDTho";

            using (SqlConnection conn = DBConnection.GetConnection())
            {
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
                                danhSachDanhGia.Add(new DanhGia
                                {
                                    IDNguoiDung = reader.GetInt32(reader.GetOrdinal("IDNguoiDung")),
                                    IDLichHen = reader.GetInt32(reader.GetOrdinal("IDCongViec")),
                                    SoSao = reader.GetInt32(reader.GetOrdinal("SoSao")),
                                    NhanXet = reader.IsDBNull(reader.GetOrdinal("NhanXet")) ?
                                        null : reader.GetString(reader.GetOrdinal("NhanXet")),
                                    HinhAnh = reader.IsDBNull(reader.GetOrdinal("HinhAnh")) ?
                                        null : reader.GetString(reader.GetOrdinal("HinhAnh"))
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in GetDanhGiaByIDTho: {ex.Message}");
                        throw;
                    }
                }
            }
            return danhSachDanhGia;
        }

        // Tính điểm đánh giá trung bình của thợ
        public decimal TinhDiemTrungBinh(int idTho)
        {
            string query = @"
                SELECT AVG(CAST(SoSao AS DECIMAL(3,2)))
                FROM DanhGia dg
                JOIN CongViec cv ON dg.IDCongViec = cv.IDCongViec
                JOIN BaiDang bd ON cv.IDBaiDang = bd.IDBaiDang
                JOIN BaiDangTho bdt ON bd.IDBaiDang = bdt.IDBaiDang
                WHERE bdt.IDTho = @IDTho";

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IDTho", idTho);
                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToDecimal(result);
                        }
                        return 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in TinhDiemTrungBinh: {ex.Message}");
                        throw;
                    }
                }
            }
        }
    }
}
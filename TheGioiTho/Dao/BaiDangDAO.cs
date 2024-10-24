using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TheGioiTho.Config;
using TheGioiTho.Model;

namespace TheGioiTho.DAO
{
    public class BaiDangDAO
    {
        public int ThemBaiDang(BaiDang baiDang)
        {
            string checkQuery = "SELECT COUNT(*) FROM LinhVuc WHERE IDLinhVuc = @IDLinhVuc";
            string insertQuery = "INSERT INTO BaiDang (IDLinhVuc, TieuDe, MoTa, HinhAnh) OUTPUT INSERTED.IDBaiDang VALUES (@IDLinhVuc, @TieuDe, @MoTa, @HinhAnh)";

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();

                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@IDLinhVuc", baiDang.IDLinhVuc);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count == 0)
                    {
                        throw new Exception("IDLinhVuc không tồn tại trong bảng LinhVuc");
                    }
                }

                using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                {
                    insertCmd.Parameters.AddWithValue("@IDLinhVuc", baiDang.IDLinhVuc);
                    insertCmd.Parameters.AddWithValue("@TieuDe", baiDang.TieuDe);
                    insertCmd.Parameters.AddWithValue("@MoTa", baiDang.MoTa);
                    insertCmd.Parameters.AddWithValue("@HinhAnh", baiDang.HinhAnh);

                    return (int)insertCmd.ExecuteScalar();
                }
            }
        }

        public bool XoaBaiDang(int idBaiDang)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.Transaction = transaction;

                        // 1. Xóa từ bảng DanhGia
                        cmd.CommandText = @"
                    DELETE FROM DanhGia 
                    WHERE IDCongViec IN (SELECT IDCongViec FROM CongViec WHERE IDBaiDang = @IDBaiDang)";
                        cmd.Parameters.AddWithValue("@IDBaiDang", idBaiDang);
                        cmd.ExecuteNonQuery();

                        // 2. Xóa từ bảng CongViec
                        cmd.CommandText = "DELETE FROM CongViec WHERE IDBaiDang = @IDBaiDang";
                        cmd.ExecuteNonQuery();

                        // 3. Xóa từ bảng DatLich
                        cmd.CommandText = "DELETE FROM DatLich WHERE IDBaiDang = @IDBaiDang";
                        cmd.ExecuteNonQuery();

                        // 4. Xóa từ bảng BaiDangNguoiDung
                        cmd.CommandText = "DELETE FROM BaiDangNguoiDung WHERE IDBaiDang = @IDBaiDang";
                        cmd.ExecuteNonQuery();

                        // 5. Xóa từ bảng NhanViec
                        cmd.CommandText = "DELETE FROM NhanViec WHERE IDBaiDang = @IDBaiDang";
                        cmd.ExecuteNonQuery();

                        // 6. Xóa từ bảng BaiDangTho
                        cmd.CommandText = "DELETE FROM BaiDangTho WHERE IDBaiDang = @IDBaiDang";
                        cmd.ExecuteNonQuery();

                        // 7. Kiểm tra xem BaiDang có tồn tại không
                        cmd.CommandText = "SELECT COUNT(*) FROM BaiDang WHERE IDBaiDang = @IDBaiDang";
                        int count = (int)cmd.ExecuteScalar();

                        if (count == 0)
                        {
                            // BaiDang không tồn tại
                            transaction.Rollback();
                            return false;
                        }

                        // 8. Cuối cùng, xóa từ bảng BaiDang
                        cmd.CommandText = "DELETE FROM BaiDang WHERE IDBaiDang = @IDBaiDang";
                        int result = cmd.ExecuteNonQuery();

                        transaction.Commit();
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    // Log the exception
                    Console.WriteLine($"Error deleting BaiDang: {ex.Message}");
                    return false;
                }
            }
        }

        public (BaiDang, BaiDangNguoiDung, NguoiDung) GetBaiDangChiTietById(int idBaiDang)
        {
            // Thay đổi tên view
            string query = "SELECT * FROM vw_BaiDangNguoiDungChiTiet WHERE IDBaiDang = @IDBaiDang";

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IDBaiDang", idBaiDang);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            BaiDang baiDang = new BaiDang
                            {
                                IDBaiDang = reader.GetInt32(reader.GetOrdinal("IDBaiDang")),
                                IDLinhVuc = reader.GetInt32(reader.GetOrdinal("IDLinhVuc")),
                                TieuDe = reader.GetString(reader.GetOrdinal("TieuDe")),
                                MoTa = reader.GetString(reader.GetOrdinal("MoTa")),
                                HinhAnh = reader.GetString(reader.GetOrdinal("HinhAnh"))
                            };

                            BaiDangNguoiDung baiDangNguoiDung = new BaiDangNguoiDung
                            {
                                IDBaiDang = reader.GetInt32(reader.GetOrdinal("IDBaiDang")),
                                IDNguoiDung = reader.GetInt32(reader.GetOrdinal("IDNguoiDung")),
                                NgayThoDen = reader.GetDateTime(reader.GetOrdinal("NgayThoDen")),
                                GioThoDen = reader.GetTimeSpan(reader.GetOrdinal("GioThoDen"))
                            };

                            NguoiDung nguoiDung = new NguoiDung
                            {
                                IDNguoiDung = reader.GetInt32(reader.GetOrdinal("IDNguoiDung")),
                                TaiKhoan = reader.GetString(reader.GetOrdinal("TaiKhoan")),
                                HoTen = reader.GetString(reader.GetOrdinal("HoTen")),
                                SoDienThoai = reader.GetString(reader.GetOrdinal("SoDienThoai")),
                                DiaChi = reader.GetString(reader.GetOrdinal("DiaChi"))
                            };

                            return (baiDang, baiDangNguoiDung, nguoiDung);
                        }
                    }
                }
            }

            return (null, null, null);
        }

        public List<BaiDang> GetAllBaiDang()
        {
            List<BaiDang> danhSachBaiDang = new List<BaiDang>();
            // Thay đổi tên view
            string query = "SELECT * FROM vw_DanhSachBaiDangNguoiDung ORDER BY IDBaiDang DESC";

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BaiDang baiDang = new BaiDang
                            {
                                IDBaiDang = reader.GetInt32(reader.GetOrdinal("IDBaiDang")),
                                IDLinhVuc = reader.GetInt32(reader.GetOrdinal("IDLinhVuc")),
                                TieuDe = reader.GetString(reader.GetOrdinal("TieuDe")),
                                MoTa = reader.GetString(reader.GetOrdinal("MoTa")),
                                HinhAnh = reader.GetString(reader.GetOrdinal("HinhAnh"))
                            };

                            danhSachBaiDang.Add(baiDang);
                        }
                    }
                }
            }

            return danhSachBaiDang;
        }

        public string GetTenLinhVuc(int idLinhVuc)
        {
            string query = "SELECT TenLinhVuc FROM LinhVuc WHERE IDLinhVuc = @IDLinhVuc";
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IDLinhVuc", idLinhVuc);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return result != null ? result.ToString() : string.Empty;
                }
            }
        }

        public List<LinhVuc> GetDanhSachLinhVuc()
        {
            List<LinhVuc> danhSachLinhVuc = new List<LinhVuc>();
            string query = "SELECT IDLinhVuc, TenLinhVuc FROM LinhVuc";
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            danhSachLinhVuc.Add(new LinhVuc
                            {
                                IDLinhVuc = reader.GetInt32(0),
                                TenLinhVuc = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            return danhSachLinhVuc;
        }
    }


}
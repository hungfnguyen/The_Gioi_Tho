using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGioiTho.Config;
using TheGioiTho.Model;

namespace TheGioiTho.Dao
{
   
    public class ThongKeDao
    {

        public DataTable LayDanhSachCongViecDaHoanThanh(int IDTho)
        {
            DataTable dt = new DataTable();

            string query = @"
        SELECT 
            cv.IDCongViec,
            bdt.GiaTien,
            cv.TrangThaiCongViecTho,
            cv.ThoiGianBatDau,
            cv.ThoiGianHoanThanh
        FROM 
            CongViec cv
        JOIN 
            BaiDangTho bdt ON cv.IDBaiDang = bdt.IDBaiDang
        WHERE 
            cv.TrangThaiCongViecTho = 'Đã hoàn thành' 
            AND bdt.IDTho = @IDTho";

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@IDTho", IDTho);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
        }

        // Tính doanh thu của thợ trong 3 tháng gần nhất
        public decimal LayDoanhThuTheoThang(int month, int year, int IDTho)
        {
            decimal doanhThu = 0;

            // Truy vấn cơ sở dữ liệu
            string query = @"
        SELECT SUM(bdt.GiaTien) AS DoanhThu
        FROM CongViec cv
        JOIN BaiDangTho bdt ON cv.IDBaiDang = bdt.IDBaiDang
        WHERE MONTH(cv.ThoiGianBatDau) = @Thang 
          AND YEAR(cv.ThoiGianBatDau) = @Nam 
          AND cv.TrangThaiCongViecTho = 'Đã hoàn thành' 
          AND bdt.IDTho = @idTho";

            // Thực hiện truy vấn và xử lý dữ liệu
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@Thang", month);
                    command.Parameters.AddWithValue("@Nam", year);
                    
                    command.Parameters.AddWithValue("@idTho", IDTho);

                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        doanhThu = Convert.ToDecimal(result);
                    }
                }
            }

            return doanhThu;
        }

        public Dictionary<string, decimal> LayDoanhThuCacThangGanDay(int soThang,int IDTho)
        {
            Dictionary<string, decimal> doanhThuTheoThang = new Dictionary<string, decimal>();

            // Lấy tháng và năm hiện tại
            int thangHienTai = DateTime.Now.Month;
            int namHienTai = DateTime.Now.Year;

            // Bắt đầu từ ba tháng trước
            DateTime ngayBatDau = DateTime.Now.AddMonths(-soThang + 1); // Điều chỉnh để đếm bao gồm cả tháng hiện tại

            // Lặp qua số tháng yêu cầu
            for (int i = 0; i < soThang; i++)
            {
                // Lấy tháng và năm của vòng lặp hiện tại
                int thang = ngayBatDau.Month;
                int nam = ngayBatDau.Year;

                // Tạo chuỗi khóa cho kết hợp tháng-năm
                string khoa = $"{thang}/{nam}";

                // Lấy doanh thu cho tháng hiện tại
                decimal doanhThu = LayDoanhThuTheoThang(thang, nam, IDTho);

                // Thêm doanh thu vào từ điển
                doanhThuTheoThang[khoa] = doanhThu;

                // Chuyển sang tháng tiếp theo (tăng thêm một tháng)
                ngayBatDau = ngayBatDau.AddMonths(1);
            }

            return doanhThuTheoThang;
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

        // Đếm số lượng đánh giá cho từng số sao
        public int DemSoSao(int idTho, int soSao)
        {
            int count = 0;
            string query = @"
            SELECT COUNT(*)
            FROM DanhGia dg
            JOIN CongViec cv ON dg.IDCongViec = cv.IDCongViec
            JOIN BaiDang bd ON cv.IDBaiDang = bd.IDBaiDang
            JOIN BaiDangTho bdt ON bd.IDBaiDang = bdt.IDBaiDang
            WHERE bdt.IDTho = @IDTho AND dg.SoSao = @SoSao";

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IDTho", idTho);
                    cmd.Parameters.AddWithValue("@SoSao", soSao);

                    try
                    {
                        conn.Open();
                        count = (int)cmd.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in DemSoSao: {ex.Message}");
                        throw;
                    }
                }
            }
            return count;
        }

        public int DemSoLuongDanhGia(int idTho)
        {
            int reviewCount = 0;
            string query = @"
    SELECT COUNT(*)
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
                        reviewCount = (int)cmd.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in DemSoLuongDanhGia: {ex.Message}");
                        throw;
                    }
                }
            }
            return reviewCount;
        }


    }
}

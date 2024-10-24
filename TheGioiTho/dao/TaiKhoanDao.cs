using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGioiTho.Model;
using TheGioiTho.Config;
using GUI.All_Calendar_Control;
using System.Windows.Forms;

namespace TheGioiTho.Dao
{
    public class TaiKhoanDao
    {
        public bool DoiMatKhau(int idTho, string matKhauCu, string matKhauMoi)
        {
            // Kiểm tra mật khẩu cũ
            string queryCheck = "SELECT COUNT(*) FROM Tho WHERE IDTho = @IDTho AND MatKhau = @MatKhauCu";
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(queryCheck, conn))
                {
                    cmd.Parameters.AddWithValue("@IDTho", idTho);
                    cmd.Parameters.AddWithValue("@MatKhauCu", matKhauCu);

                    int count = (int)cmd.ExecuteScalar();
                    if (count == 0) // Mật khẩu cũ không chính xác
                    {
                        return false;
                    }
                }
            }

            // Cập nhật mật khẩu mới
            string queryUpdate = "UPDATE Tho SET MatKhau = @MatKhauMoi WHERE IDTho = @IDTho";
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(queryUpdate, conn))
                {
                    cmd.Parameters.AddWithValue("@MatKhauMoi", matKhauMoi);
                    cmd.Parameters.AddWithValue("@IDTho", idTho);
                    return cmd.ExecuteNonQuery() > 0; // Trả về true nếu có dòng được cập nhật
                }
            }
        }

        public bool CapNhatTho(Tho tho)
        {
            string query = @"
        UPDATE Tho 
        SET TaiKhoan = @TaiKhoan, MatKhau = @MatKhau, HoTen = @HoTen, 
            SoDienThoai = @SoDienThoai, GioiTinh = @GioiTinh, DiaChi = @DiaChi, 
            SoNamKinhNghiem = @SoNamKinhNghiem 
        WHERE IDTho = @IDTho";

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TaiKhoan", tho.TaiKhoan);
                    cmd.Parameters.AddWithValue("@MatKhau", tho.MatKhau); // Nếu cần cập nhật mật khẩu
                    cmd.Parameters.AddWithValue("@HoTen", tho.HoTen);
                    cmd.Parameters.AddWithValue("@SoDienThoai", tho.SoDienThoai);
                    cmd.Parameters.AddWithValue("@GioiTinh", tho.GioiTinh);
                    cmd.Parameters.AddWithValue("@DiaChi", tho.DiaChi);
                    cmd.Parameters.AddWithValue("@SoNamKinhNghiem", tho.SoNamKinhNghiem);
                    cmd.Parameters.AddWithValue("@IDTho", tho.IDTho);

                    return cmd.ExecuteNonQuery() > 0; // Trả về true nếu có dòng được cập nhật
                }
            }
        }

        // Phương thức lấy thông tin thợ qua IDTho
        public Tho LayThongTinTho(int idTho)
        {
            Tho tho = null;
            string query = @"
                SELECT IDTho, TaiKhoan, MatKhau, HoTen, SoDienThoai, GioiTinh, DiaChi, SoNamKinhNghiem 
                FROM Tho 
                WHERE IDTho = @IDTho";

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IDTho", idTho);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            tho = new Tho
                            {
                                IDTho = reader.GetInt32(0),
                                TaiKhoan = reader.GetString(1),
                                MatKhau = reader.GetString(2),
                                HoTen = reader.GetString(3),
                                SoDienThoai = reader.GetString(4),
                                GioiTinh = reader.GetString(5),
                                DiaChi = reader.GetString(6),
                                SoNamKinhNghiem = reader.GetInt32(7)
                            };
                        }
                    }
                }
            }

            return tho;
        }

        public void MarkOffDays(int idTho, FlowLayoutPanel panel, int month, int year)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Ngay FROM NgayNghi WHERE IDTho = @IDTho";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@IDTho", idTho);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        DateTime offDay = reader.GetDateTime(0);
                        foreach (UC_Day ucDay in panel.Controls.OfType<UC_Day>())
                        {
                            if (ucDay.DayText == offDay.Day.ToString() && month == offDay.Month && year == offDay.Year)
                            {
                                ucDay.MarkAsOffDay();
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể chọn ngày trống. Vui lòng chọn lại! " + ex.Message);
                }
            }
        }

        public void DeletePastOffDays(int idTho)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM NgayNghi WHERE IDTho = @IDTho AND Ngay < @CurrentDate";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@IDTho", idTho);
                    command.Parameters.AddWithValue("@CurrentDate", DateTime.Today);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa các ngày nghỉ trong quá khứ: " + ex.Message);
                }
            }
        }

        public void UpdateOffDaysToDatabase(int idTho, FlowLayoutPanel panel, int year, int month)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                try
                {
                    connection.Open();

                    // Xóa các ngày nghỉ hiện tại trong tháng
                    string deleteAllQuery = "DELETE FROM NgayNghi WHERE IDTho = @IDTho AND YEAR(Ngay) = @Year AND MONTH(Ngay) = @Month";
                    SqlCommand deleteAllCommand = new SqlCommand(deleteAllQuery, connection);
                    deleteAllCommand.Parameters.AddWithValue("@IDTho", idTho);
                    deleteAllCommand.Parameters.AddWithValue("@Year", year);
                    deleteAllCommand.Parameters.AddWithValue("@Month", month);
                    deleteAllCommand.ExecuteNonQuery();

                    // Thêm các ngày nghỉ
                    foreach (UC_Day dayControl in panel.Controls.OfType<UC_Day>().Where(uc => uc.IsOffDay))
                    {
                        DateTime offDay = new DateTime(year, month, int.Parse(dayControl.DayText));
                        string insertQuery = "INSERT INTO NgayNghi (IDTho, Ngay) VALUES (@IDTho, @Ngay)";
                        SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                        insertCommand.Parameters.AddWithValue("@IDTho", idTho);
                        insertCommand.Parameters.AddWithValue("@Ngay", offDay);
                        insertCommand.ExecuteNonQuery();
                    }

                    MessageBox.Show("Cập nhật lịch nghỉ thành công!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật lịch nghỉ: " + ex.Message);
                }
            }
        }
    }

}

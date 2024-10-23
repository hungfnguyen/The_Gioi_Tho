using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TheGioiTho.Controller.Tho
{
    public partial class UC_DangBai : UserControl
    {

        private void btnDangBai_Click(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ các trường nhập liệu
            string tieuDe = ""; // Giả sử bạn có txtTieuDe
            string moTa = txtMoTa.Text.Trim();
            string hinhAnh = ""; // Giả sử bạn có txtHinhAnh
            int idLinhVuc = (int)cbChonCongViec.SelectedValue; // Lấy ID lĩnh vực đã chọn
            //int idTho = GetIDTho(); // Hàm lấy ID của thợ (nếu cần)
            string thoiGianThucHien = txtThoiGianThucHien.Text.Trim();
            decimal giaTien;

            // Kiểm tra và chuyển đổi giá tiền
            if (!decimal.TryParse(txtGiaTien.Text.Trim(), out giaTien))
            {
                MessageBox.Show("Vui lòng nhập giá tiền hợp lệ.");
                return;
            }

            try
            {
                // Kết nối đến cơ sở dữ liệu
                conn.Open();

                // Bắt đầu giao dịch
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Thêm vào bảng BaiDang
                        string queryBaiDang = "INSERT INTO BaiDang (IDLinhVuc, TieuDe, MoTa, HinhAnh) OUTPUT INSERTED.IDBaiDang VALUES (@IDLinhVuc, @TieuDe, @MoTa, @HinhAnh)";
                        SqlCommand cmdBaiDang = new SqlCommand(queryBaiDang, conn, transaction);
                        cmdBaiDang.Parameters.AddWithValue("@IDLinhVuc", idLinhVuc);
                        cmdBaiDang.Parameters.AddWithValue("@TieuDe", tieuDe);
                        cmdBaiDang.Parameters.AddWithValue("@MoTa", moTa);
                        cmdBaiDang.Parameters.AddWithValue("@HinhAnh", hinhAnh);

                        int idBaiDang = (int)cmdBaiDang.ExecuteScalar(); // Lấy ID của bài đăng mới tạo

                        // Thêm vào bảng BaiDangTho
                        string queryBaiDangTho = "INSERT INTO BaiDangTho (IDBaiDang, IDTho, GiaTien, ThoiGianThucHien) VALUES (@IDBaiDang, @IDTho, @GiaTien, @ThoiGianThucHien)";
                        SqlCommand cmdBaiDangTho = new SqlCommand(queryBaiDangTho, conn, transaction);
                        cmdBaiDangTho.Parameters.AddWithValue("@IDBaiDang", idBaiDang);
                        cmdBaiDangTho.Parameters.AddWithValue("@IDTho", 1); // Thay thế bằng ID thợ hợp lệ
                        cmdBaiDangTho.Parameters.AddWithValue("@GiaTien", giaTien);
                        cmdBaiDangTho.Parameters.AddWithValue("@ThoiGianThucHien", thoiGianThucHien);

                        cmdBaiDangTho.ExecuteNonQuery(); // Thực thi truy vấn thêm vào bảng BaiDangTho

                        // Xác nhận giao dịch
                        transaction.Commit();
                        MessageBox.Show("Đăng bài thành công!");
                    }
                    catch (Exception ex)
                    {
                        // Nếu có lỗi, hoàn tác giao dịch
                        transaction.Rollback();
                        MessageBox.Show("Lỗi khi đăng bài: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối đến cơ sở dữ liệu: " + ex.Message);
            }
            finally
            {
                conn.Close(); // Đóng kết nối
            }
        }

        private void UC_DangBai_Load(object sender, EventArgs e)
        {

        }

        private SqlConnection conn = new SqlConnection("Data Source=LAPTOP-DTKDJMOS\\SQLEXPRESS;Initial Catalog=TheGioiTho1;Integrated Security=True");

        public UC_DangBai()
        {
            InitializeComponent();
            LoadThongTinTho(); // Gọi hàm khi UC được khởi tạo
            // Gọi hàm để tải lĩnh vực tương ứng khi công việc được chọn
            LoadLinhVuc();
        }

        private void LoadThongTinTho()
        {
            try
            {
                conn.Open();

                // Câu lệnh SQL để lấy thông tin thợ
                string query = "SELECT HoTen, SoDienThoai, DiaChi FROM Tho WHERE IDTho = @IDTho"; // Giả định bạn có IDTho để lấy thông tin

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IDTho", 1); // Thay IDTho bằng ID thợ thực tế cần lấy thông tin
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read()) // Nếu có kết quả
                    {
                        txtTenTho.Text = reader["HoTen"].ToString();
                        txtSoDienThoai.Text = reader["SoDienThoai"].ToString();
                        txtDiaChi.Text = reader["DiaChi"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thông tin thợ: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void cbChonCongViec_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void LoadLinhVuc()
        {
            try
            {
                conn.Open();
                string query = "SELECT IDLinhVuc, TenLinhVuc FROM LinhVuc"; // Truy vấn để lấy tất cả các lĩnh vực
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                cbChonCongViec.DataSource = dt; // Gán DataSource cho ComboBox
                cbChonCongViec.DisplayMember = "TenLinhVuc"; // Hiển thị tên lĩnh vực
                cbChonCongViec.ValueMember = "IDLinhVuc"; // Lưu ID lĩnh vực
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu lĩnh vực: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnQuanLyBaiDang_Click(object sender, EventArgs e)
        {
            // Tạo một thể hiện mới của Form_QuanLyBaiDang
            Form_QuanLyBaiDang formQuanLyBaiDang = new Form_QuanLyBaiDang();

            // Hiển thị form
            formQuanLyBaiDang.Show(); // Sử dụng Show() nếu bạn muốn mở form không đồng bộ
        }
    }
}

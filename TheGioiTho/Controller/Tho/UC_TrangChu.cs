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
    public partial class UC_TrangChu : UserControl
    {
        private SqlConnection conn = Config.DBConnection.GetConnection();
        private int idNguoiDung; // Biến để lưu ID người dùng



        public UC_TrangChu()
        {
            InitializeComponent();
            LoadBaiDangNguoiDung(); // Gọi hàm khi UC được khởi tạo
            dgvBaiDangNguoiDung.CellClick += dgvBaiDangNguoiDung_CellClick; // Đăng ký sự kiện CellClick
        }

   

        private void dgvBaiDangNguoiDung_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void LoadBaiDangNguoiDung()
        {
            try
            {
                string query = "SELECT * FROM vw_BaiDangNguoiDung";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvBaiDangNguoiDung.DataSource = dt;


                // Cập nhật cột HinhAnh để hiển thị hình ảnh
                foreach (DataGridViewRow row in dgvBaiDangNguoiDung.Rows)
                {
                    string imagePath = row.Cells["HinhAnh"].Value?.ToString(); // Lấy giá trị từ cột HinhAnh
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        try
                        {
                            // Đảm bảo đường dẫn hình ảnh chính xác và chuyển đổi thành Image
                            row.Cells["HinhAnh"].Value = Image.FromFile(imagePath); // Chuyển đổi đường dẫn thành hình ảnh
                        }
                        catch (Exception)
                        {
                            row.Cells["HinhAnh"].Value = null; // Nếu không thể load hình, gán giá trị null
                        }
                    }
                }


                // Ẩn cột IDNguoiDung
                dgvBaiDangNguoiDung.Columns["IDNguoiDung"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void dgvBaiDangNguoiDung_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Kiểm tra không phải tiêu đề cột
            {
                DataGridViewRow selectedRow = dgvBaiDangNguoiDung.Rows[e.RowIndex];

                // Gán dữ liệu vào các TextBox
                txtTenKhachHang.Text = selectedRow.Cells["HoTen"].Value?.ToString() ?? "";
                txtSoDienThoai.Text = selectedRow.Cells["SoDienThoai"].Value?.ToString() ?? "";
                txtDiaDiemLamViec.Text = selectedRow.Cells["DiaChiKhachHang"].Value?.ToString() ?? "";
                txtThoiGianLamViec.Text = $"{selectedRow.Cells["NgayThoDen"].Value?.ToString() ?? ""} {selectedRow.Cells["GioThoDen"].Value?.ToString() ?? ""}";
                txtGhiChu.Text = selectedRow.Cells["GhiChu"].Value?.ToString() ?? "";

                // Lấy ID người dùng từ hàng đã chọn
                idNguoiDung = Convert.ToInt32(selectedRow.Cells["IDNguoiDung"].Value); // ID người dùng đã được thêm vào nhưng ẩn
            }
        }

        private void btnDatLich_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem người dùng đã nhập đầy đủ thông tin chưa
            if (string.IsNullOrWhiteSpace(txtThoiGianThucHien.Text) || string.IsNullOrWhiteSpace(txtGiaTien.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin thời gian thực hiện và giá tiền.");
                return;
            }

            // Kiểm tra xem có hàng nào được chọn không
            if (dgvBaiDangNguoiDung.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một bài đăng trước khi đặt lịch.");
                return;
            }

            // Lấy thông tin từ các TextBox
            string giaTien = txtGiaTien.Text;

            // Chuyển đổi chuỗi thoiGianThucHien thành DateTime
            DateTime thoiGianThucHien;
            if (!DateTime.TryParse(txtThoiGianThucHien.Text, out thoiGianThucHien))
            {
                MessageBox.Show("Vui lòng nhập thời gian thực hiện đúng định dạng.");
                return;
            }

            string idBaiDang = dgvBaiDangNguoiDung.SelectedRows[0].Cells["IDBaiDang"].Value.ToString(); // Lấy ID bài đăng

            // Gọi hàm lưu thông tin đặt lịch
            DatLich(idBaiDang, thoiGianThucHien, giaTien);
        }


        private void DatLich(string idBaiDang, DateTime thoiGianThucHien, string giaTien)
        {
            try
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                DataGridViewRow selectedRow = dgvBaiDangNguoiDung.SelectedRows[0];
                DateTime gioThoDen;
                if (!DateTime.TryParse(selectedRow.Cells["GioThoDen"].Value.ToString(), out gioThoDen))
                {
                    MessageBox.Show("Giờ thợ đến không hợp lệ.");
                    return;
                }

                using (SqlCommand cmd = new SqlCommand("sp_DatLich", conn, transaction))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Truyền tham số vào stored procedure
                    cmd.Parameters.AddWithValue("@IDTho", 1); // Giả định ID thợ là 1
                    cmd.Parameters.AddWithValue("@IDBaiDang", idBaiDang);
                    cmd.Parameters.AddWithValue("@GioThoDen", gioThoDen);
                    cmd.Parameters.AddWithValue("@ThoiGianThucHien", thoiGianThucHien.TimeOfDay);
                    cmd.Parameters.AddWithValue("@TrangThaiCongViecTho", "Đã Chấp Nhận");
                    cmd.Parameters.AddWithValue("@TrangThaiCongViecNguoiDung", "Đã Chấp Nhận");

                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();
                MessageBox.Show("Đặt lịch thành công!");

                LoadBaiDangNguoiDung();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đặt lịch: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }



    }
}

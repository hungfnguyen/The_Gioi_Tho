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
using TheGioiTho.Config;

namespace TheGioiTho.Controller.Tho
{
    public partial class UC_TrangChu : UserControl
    {
       
        private int idNguoiDung; // Biến để lưu ID người dùng
        private SqlConnection conn;


        public UC_TrangChu()
        {
            InitializeComponent();
            conn = DBConnection.GetConnection(); // Lấy kết nối từ cấu hình
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
                string query = @"
        SELECT 
             BaiDang.IDBaiDang,
            LinhVuc.TenLinhVuc,
            NguoiDung.HoTen,
            NguoiDung.SoDienThoai,
            NguoiDung.DiaChi AS DiaChiKhachHang,
            BaiDangNguoiDung.NgayThoDen,
            BaiDangNguoiDung.GioThoDen,
            BaiDang.MoTa AS GhiChu,
            BaiDang.HinhAnh,
            NguoiDung.IDNguoiDung -- Thêm ID người dùng vào truy vấn
        FROM 
            BaiDangNguoiDung
        JOIN 
            BaiDang ON BaiDangNguoiDung.IDBaiDang = BaiDang.IDBaiDang
        JOIN 
            LinhVuc ON BaiDang.IDLinhVuc = LinhVuc.IDLinhVuc
        JOIN 
            NguoiDung ON BaiDangNguoiDung.IDNguoiDung = NguoiDung.IDNguoiDung";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvBaiDangNguoiDung.DataSource = dt;

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

                // Lấy giờ thợ đến từ hàng đã chọn
                DataGridViewRow selectedRow = dgvBaiDangNguoiDung.SelectedRows[0];
                // Sử dụng DateTime.TryParse để đảm bảo chuyển đổi an toàn
                DateTime gioThoDen;
                if (!DateTime.TryParse(selectedRow.Cells["GioThoDen"].Value.ToString(), out gioThoDen))
                {
                    MessageBox.Show("Giờ thợ đến không hợp lệ.");
                    return;
                }

                // Cộng thêm thời gian thực hiện
                DateTime thoiGianHoanThanh = gioThoDen.Add(thoiGianThucHien.TimeOfDay); // Tính thời gian kết thúc

                // Câu lệnh SQL để chèn vào bảng NhanViec
                string queryNhanViec = @"
        INSERT INTO NhanViec (IDTho, IDBaiDang)
        VALUES (@IDTho, @IDBaiDang)";

                using (SqlCommand cmdNhanViec = new SqlCommand(queryNhanViec, conn, transaction))
                {
                    cmdNhanViec.Parameters.AddWithValue("@IDTho", 1); // Giả định ID thợ là 1
                    cmdNhanViec.Parameters.AddWithValue("@IDBaiDang", idBaiDang);

                    cmdNhanViec.ExecuteNonQuery();
                }

                // Câu lệnh SQL để chèn vào bảng CongViec
                string queryCongViec = @"
        INSERT INTO CongViec (IDBaiDang, ThoiGianBatDau, ThoiGianHoanThanh, TrangThaiCongViecTho, TrangThaiCongViecNguoiDung)
        VALUES (@IDBaiDang, @ThoiGianBatDau, @ThoiGianHoanThanh, @TrangThaiCongViecTho, @TrangThaiCongViecNguoiDung)";

                using (SqlCommand cmdCongViec = new SqlCommand(queryCongViec, conn, transaction))
                {
                    cmdCongViec.Parameters.AddWithValue("@IDBaiDang", idBaiDang);
                    cmdCongViec.Parameters.AddWithValue("@ThoiGianBatDau", gioThoDen); // Truyền giờ thợ đến
                    cmdCongViec.Parameters.AddWithValue("@ThoiGianHoanThanh", thoiGianHoanThanh); // Truyền thời gian hoàn thành
                    cmdCongViec.Parameters.AddWithValue("@TrangThaiCongViecTho", "Chờ xác nhận"); // Trạng thái cho thợ
                    cmdCongViec.Parameters.AddWithValue("@TrangThaiCongViecNguoiDung", "Chờ xác nhận"); // Trạng thái cho người dùng

                    cmdCongViec.ExecuteNonQuery();
                }

                // Cam kết giao dịch
                transaction.Commit();
                MessageBox.Show("Đặt lịch thành công!");

                // Cập nhật lại giao diện
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

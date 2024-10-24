using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheGioiTho.DAO;

namespace TheGioiTho.Controller.Tho
{
    public partial class UC_XemDanhGia : UserControl
    {
        private DanhGiaDAO danhGiaDAO;
        private int reviewCount = 0;
        public UC_XemDanhGia()
        {
            InitializeComponent();
            this.Load += new EventHandler(UC_XemDanhGia_Load);
            danhGiaDAO = new DanhGiaDAO(); // Khởi tạo đối tượng DanhGiaDAO
            InitializeDataGridView(); // Khởi tạo các cột cho DataGridView
            dgvBaiDanhGia.CellClick += new DataGridViewCellEventHandler(dgvBaiDanhGia_CellClick); // Thêm sự kiện CellClick

        }

        private void dgvBaiDanhGia_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Kiểm tra nếu chỉ số dòng hợp lệ
            {
                // Lấy dòng hiện tại mà người dùng click
                DataGridViewRow selectedRow = dgvBaiDanhGia.Rows[e.RowIndex];

                // Gán giá trị từ dòng được chọn vào các điều khiển tương ứng
                txtTenKhachHang.Text = selectedRow.Cells["HoTen"].Value.ToString(); // Lấy Họ Tên
                lblsao.Text = selectedRow.Cells["SoSao"].Value.ToString() + " Sao"; // Lấy Số Sao
                txtNhanXet.Text = selectedRow.Cells["NhanXet"].Value.ToString(); // Lấy Nhận Xét

                // Kiểm tra và gán hình ảnh nếu có
                var hinhAnh = selectedRow.Cells["HinhAnh"].Value;
                if (hinhAnh != null && !string.IsNullOrEmpty(hinhAnh.ToString()))
                {
                    try
                    {
                        ptbHinhAnh.Image = Image.FromFile(hinhAnh.ToString()); // Đọc hình ảnh từ đường dẫn
                        ptbHinhAnh.SizeMode = PictureBoxSizeMode.Zoom; // Tùy chọn để hình ảnh hiển thị đúng kích thước
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể tải hình ảnh: " + ex.Message);
                        ptbHinhAnh.Image = null; // Nếu không thể tải hình ảnh, để trống PictureBox
                    }
                }
                else
                {
                    ptbHinhAnh.Image = null; // Không có hình ảnh thì để trống PictureBox
                }
            }
        }
        private void LoadDanhGia(int IDTho)
        {
            // Lấy danh sách đánh giá từ cơ sở dữ liệu
            var danhSachDanhGia = danhGiaDAO.GetDanhGiaByIDTho(IDTho);

            // Xóa các hàng hiện có
            dgvBaiDanhGia.Rows.Clear();



            reviewCount = danhSachDanhGia.Count;

            // Đổ dữ liệu vào DataGridView
            foreach (var danhGia in danhSachDanhGia)
            {
                int rowIndex = dgvBaiDanhGia.Rows.Add();
                dgvBaiDanhGia.Rows[rowIndex].Cells["HoTen"].Value = danhGia.HoTen; // Họ tên người dùng
                dgvBaiDanhGia.Rows[rowIndex].Cells["SoSao"].Value = danhGia.SoSao; // Số sao
                dgvBaiDanhGia.Rows[rowIndex].Cells["NhanXet"].Value = danhGia.NhanXet; // Nhận xét
                dgvBaiDanhGia.Rows[rowIndex].Cells["HinhAnh"].Value = danhGia.HinhAnh;                                   // Hiển thị hình ảnh
                                                                                                                       // Hiển thị hình ảnh
                /*if (!string.IsNullOrEmpty(danhGia.HinhAnh))
                {
                    try
                    {
                        dtgvDanhGia.Rows[rowIndex].Cells["HinhAnh"].Value = Image.FromFile(danhGia.HinhAnh); // Đọc hình ảnh từ đường dẫn
                        dtgvDanhGia.Rows[rowIndex].Height = 100; // Tùy chỉnh chiều cao hàng để hình ảnh hiển thị tốt hơn
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error loading image: {ex.Message}"); // Ghi lại lỗi nếu có
                        dtgvDanhGia.Rows[rowIndex].Cells["HinhAnh"].Value = null; // Đặt giá trị null nếu không thể đọc hình ảnh
                    }
                }*/
            }
        }
            private void InitializeDataGridView()
            {
                dgvBaiDanhGia.Columns.Clear(); // Xóa cột nếu có

                // Tạo và thêm cột Họ Tên
                dgvBaiDanhGia.Columns.Add("HoTen", "Họ Tên");

                // Tạo và thêm cột Số Sao
                dgvBaiDanhGia.Columns.Add("SoSao", "Số Sao");

                // Tạo và thêm cột Nhận Xét
                dgvBaiDanhGia.Columns.Add("NhanXet", "Nhận Xét");
                // Tạo và thêm cột Nhận Xét
                dgvBaiDanhGia.Columns.Add("HinhAnh", "Hình Ảnh");
                /* // Tạo và thêm cột Hình ảnh
                 var hinhAnhColumn = new DataGridViewImageColumn();
                 hinhAnhColumn.Name = "HinhAnh";
                 hinhAnhColumn.HeaderText = "Hình ảnh";
                 hinhAnhColumn.Width = 100; // Đặt chiều rộng cho cột hình ảnh
                 hinhAnhColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // Căn giữa hình ảnh
                 dtgvDanhGia.Columns.Add(hinhAnhColumn);*/
            }
            private void UC_XemDanhGia_Load(object sender, EventArgs e)
            {
                int IDTho = 1;  // Thay ID tho thực tế
                LoadDanhGia(IDTho);

               
            

             }
    }
}

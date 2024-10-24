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
       
        public UC_XemDanhGia()
        {
            InitializeComponent();
            /*ptbHinhAnh.Image = Image.FromFile(@"C:\Users\maiho\OneDrive\Hình ảnh\hehe.jpg");
            ptbHinhAnh.SizeMode = PictureBoxSizeMode.Zoom;*/
            this.Load += new EventHandler(UC_XemDanhGia_Load);
            danhGiaDAO = new DanhGiaDAO(); // Khởi tạo đối tượng DanhGiaDAO
            //InitializeDataGridView(); // Khởi tạo các cột cho DataGridView
            dgvBaiDanhGia.CellClick += new DataGridViewCellEventHandler(dgvBaiDanhGia_CellClick); // Thêm sự kiện CellClick

        }

        private void dgvBaiDanhGia_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Kiểm tra nếu chỉ số dòng hợp lệ
            {
                // Lấy dòng hiện tại mà người dùng click
                DataGridViewRow selectedRow = dgvBaiDanhGia.Rows[e.RowIndex];

                // Gán giá trị từ dòng được chọn vào các điều khiển tương ứng
                txtTenKhachHang.Text = selectedRow.Cells["HoTen"].Value?.ToString() ?? ""; // Lấy Họ Tên
                lblsao.Text = selectedRow.Cells["SoSao"].Value?.ToString() + " Sao"; // Lấy Số Sao
                txtNhanXet.Text = selectedRow.Cells["NhanXet"].Value?.ToString() ?? ""; // Lấy Nhận Xét

                // Lấy số sao từ cell "SoSao" và chuyển về kiểu số nguyên
                if (int.TryParse(selectedRow.Cells["SoSao"].Value?.ToString(), out int soSao))
                {
                    lblssao.Text = new string('★', soSao) + new string('☆', 5 - soSao);
                }

                // Kiểm tra và gán hình ảnh từ cột HinhAnhDisplay
                if (selectedRow.Cells["HinhAnhDisplay"].Value is Image img) // Kiểm tra xem giá trị có phải là Image không
                {
                    ptbHinhAnh.Image = img; // Gán hình ảnh vào PictureBox
                    ptbHinhAnh.SizeMode = PictureBoxSizeMode.Zoom; // Đặt chế độ hiển thị
                }
                else
                {
                    ptbHinhAnh.Image = Icon.ExtractAssociatedIcon(Application.ExecutablePath).ToBitmap(); // Hiển thị biểu tượng mặc định
                }
               
            }
        }
        private void LoadDanhGia(int IDTho)
        {
            // Lấy danh sách đánh giá từ cơ sở dữ liệu dưới dạng DataTable
            DataTable danhSachDanhGia = danhGiaDAO.GetDanhGiaByIDTho(IDTho);
            dgvBaiDanhGia.DataSource = danhSachDanhGia;

            // Kiểm tra và thêm cột hình ảnh chỉ nếu nó chưa tồn tại
            if (!dgvBaiDanhGia.Columns.Contains("HinhAnhDisplay"))
            {
                DataGridViewImageColumn imageColumn = new DataGridViewImageColumn
                {
                    Name = "HinhAnhDisplay", // Tên cột mới để hiển thị hình ảnh
                    HeaderText = "Hình Ảnh", // Tiêu đề cột
                    ImageLayout = DataGridViewImageCellLayout.Stretch // Đặt chế độ hiển thị hình ảnh to hơn
                };
                dgvBaiDanhGia.Columns.Add(imageColumn);
            }

            // Tăng chiều cao của các hàng để hiển thị hình ảnh lớn hơn
            dgvBaiDanhGia.RowTemplate.Height = 150; // Chiều cao của hàng, tùy chỉnh theo nhu cầu

            // Hiển thị hình ảnh trong DataGridView
            foreach (DataGridViewRow row in dgvBaiDanhGia.Rows)
            {
                // Lấy đường dẫn hình ảnh từ DataTable
                string imagePath = row.Cells["HinhAnh"].Value?.ToString(); // Lấy đường dẫn hình ảnh

                if (!string.IsNullOrEmpty(imagePath) && System.IO.File.Exists(imagePath)) // Kiểm tra đường dẫn
                {
                    try
                    {
                        // Đọc hình ảnh từ đường dẫn và gán vào cột hình ảnh
                        Image img = Image.FromFile(imagePath);
                        row.Cells["HinhAnhDisplay"].Value = img;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Không thể tải hình ảnh: {ex.Message}");
                        row.Cells["HinhAnhDisplay"].Value = null; // Nếu có lỗi, gán giá trị null
                    }
                }
                else
                {
                    row.Cells["HinhAnhDisplay"].Value = null; // Nếu không có hình ảnh, để null
                }
            }

        }
           
            private void UC_XemDanhGia_Load(object sender, EventArgs e)
            {
                int IDTho = 1;  // Thay ID tho thực tế
                LoadDanhGia(IDTho);

             }

        private void UC_XemDanhGia_Load_1(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheGioiTho.Dao;
using TheGioiTho.Model;

namespace TheGioiTho.Controller
{
    public partial class UC_QuanLyLich : UserControl
    {
        private LichHenDAO lichHenDAO;

        public UC_QuanLyLich()
        {
            InitializeComponent();
            lichHenDAO = new LichHenDAO();
        }

        private void UC_QuanLyLich_Load(object sender, EventArgs e)
        {
            // Mặc định hiển thị danh sách lịch hẹn đang chờ xác nhận khi tải UserControl
            HienThiDanhSachLichHen("Đang chờ xác nhận");
        }

        private void btnDangChoXacNhan_Click(object sender, EventArgs e)
        {
            HienThiDanhSachLichHen("Đang chờ xác nhận");
        }

        private void btnDaXacNhan_Click(object sender, EventArgs e)
        {
            HienThiDanhSachLichHen("Đã xác nhận");
        }

        private void btnHoanTat_Click(object sender, EventArgs e)
        {
            HienThiDanhSachLichHen("Hoàn tất");
        }

        private void btnDaHuy_Click(object sender, EventArgs e)
        {
            HienThiDanhSachLichHen("Đã hủy");
        }

        private void HienThiDanhSachLichHen(string trangThai)
        {
            // Lấy ID người dùng (ví dụ: từ thông tin đăng nhập)
            int idNguoiDung = 1; // Thay đổi giá trị này tùy theo ID người dùng đang đăng nhập

            // Lấy danh sách lịch hẹn theo trạng thái tương ứng
            List<LichHen> danhSachLichHen = lichHenDAO.GetLichHen(idNguoiDung, trangThai);

            // Xóa tất cả các lịch hẹn đang hiển thị trên panel
            flpLichHen.Controls.Clear();

            // Kiểm tra trạng thái trước khi duyệt qua danh sách lịch hẹn
            if (trangThai == "Đã hủy")
            {
                foreach (LichHen lichHen in danhSachLichHen)
                {
                    ChiTietLich chiTietLich = new ChiTietLich(lichHen);
                    chiTietLich.ConfigureButtons(false, false, false, false, false, true);
                    // Thiết lập các thông tin khác của lịch hẹn vào các control tương ứng trong ChiTietLich
                    // ...
                    chiTietLich.Dock = DockStyle.Top;
                    flpLichHen.Controls.Add(chiTietLich);
                }
            }
            else if (trangThai == "Hoàn tất")
            {
                foreach (LichHen lichHen in danhSachLichHen)
                {
                    ChiTietLich chiTietLich = new ChiTietLich(lichHen);
                    bool daDanhGia = false;
                    bool daYeuThich = lichHenDAO.DaYeuThich(idNguoiDung, lichHen.IDLichHen);
                    chiTietLich.ConfigureButtons(false, false, !daDanhGia, !daYeuThich, daYeuThich, false);
                    // Thiết lập các thông tin khác của lịch hẹn vào các control tương ứng trong ChiTietLich
                    // ...
                    chiTietLich.Dock = DockStyle.Top;
                    flpLichHen.Controls.Add(chiTietLich);
                }
            }
            else if (trangThai == "Đã xác nhận")
            {
                foreach (LichHen lichHen in danhSachLichHen)
                {
                    ChiTietLich chiTietLich = new ChiTietLich(lichHen);
                    chiTietLich.ConfigureButtons(true, false, false, false, false, false);
                    // Thiết lập các thông tin khác của lịch hẹn vào các control tương ứng trong ChiTietLich
                    // ...
                    chiTietLich.Dock = DockStyle.Top;
                    flpLichHen.Controls.Add(chiTietLich);
                }
            }
            else
            {
                foreach (LichHen lichHen in danhSachLichHen)
                {
                    ChiTietLich chiTietLich = new ChiTietLich(lichHen);
                    chiTietLich.ConfigureButtons(true, false, false, false, false, false);
                    // Thiết lập các thông tin khác của lịch hẹn vào các control tương ứng trong ChiTietLich
                    // ...
                    chiTietLich.Dock = DockStyle.Top;
                    flpLichHen.Controls.Add(chiTietLich);
                }
            }
        }
    }
}
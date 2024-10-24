using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TheGioiTho.Controller.Tho
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void btnDanhGia_Click(object sender, EventArgs e)
        {
            UC_XemDanhGia ucDanhGia = new UC_XemDanhGia();
            ucDanhGia.Dock = DockStyle.Fill; // Để UC phủ toàn bộ Form
            panel1.Controls.Clear(); // Xóa các control trước đó
            panel1.Controls.Add(ucDanhGia);

        }

        private void btnDangBai_Click(object sender, EventArgs e)
        {
            UC_DangBai ucDangBai = new UC_DangBai(); // Tạo instance của UC_DangBai
            ucDangBai.Dock = DockStyle.Fill; // Để UC phủ toàn bộ Form
            panel1.Controls.Clear(); // Xóa các control trước đó
            panel1.Controls.Add(ucDangBai); // Thêm UC vào panel
        }

        private void btnTrangChu_Click(object sender, EventArgs e)
        {
            UC_TrangChu ucTrangChu = new UC_TrangChu();
            ucTrangChu.Dock = DockStyle.Fill; // Để UC phủ toàn bộ Form
            panel1.Controls.Clear(); // Xóa các control trước đó
            panel1.Controls.Add(ucTrangChu); // Thêm UC vào panel
        }

        private void btnLichHen_Click(object sender, EventArgs e)
        {
            UC_LichHen ucLichHen = new UC_LichHen();
            ucLichHen.Dock = DockStyle.Fill; // Để UC phủ toàn bộ Form
            panel1.Controls.Clear(); // Xóa các control trước đó
            panel1.Controls.Add(ucLichHen); // Thêm UC vào panel
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            UC_ThongKe ucThongKe = new UC_ThongKe();
            ucThongKe.Dock = DockStyle.Fill; // Để UC phủ toàn bộ Form
            panel1.Controls.Clear(); // Xóa các control trước đó
            panel1.Controls.Add(ucThongKe); // Thêm UC vào panel
        }

        private void btnTaiKhoan_Click(object sender, EventArgs e)
        {
            UC_TaiKhoan ucTaiKhoan = new UC_TaiKhoan();
            ucTaiKhoan.Dock = DockStyle.Fill; // Để UC phủ toàn bộ Form
            panel1.Controls.Clear(); // Xóa các control trước đó
            panel1.Controls.Add(ucTaiKhoan); // Thêm UC vào panel
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnTrangChu.PerformClick();
        }
    }
}

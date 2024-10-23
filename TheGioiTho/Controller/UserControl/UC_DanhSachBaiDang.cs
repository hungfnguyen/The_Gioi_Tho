using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TheGioiTho.DAO;
using TheGioiTho.Model;

namespace TheGioiTho.Controller
{
    public partial class UC_DanhSachBaiDang : UserControl
    {
        private BaiDangDAO baiDangDAO;
        

        public UC_DanhSachBaiDang()
        {
            InitializeComponent();
            baiDangDAO = new BaiDangDAO();
            
        }


        private void UC_DanhSachBaiDang_Load(object sender, EventArgs e)
        {
            LoadDanhSachBaiDang();
        }

        private void LoadDanhSachBaiDang()
        {
            flpDanhSachBaiDang.Controls.Clear();
            List<BaiDang> danhSachBaiDang = baiDangDAO.GetAllBaiDang();

            foreach (BaiDang baiDang in danhSachBaiDang)
            {
                UC_NoiDungBaiDang ucNoiDungBaiDang = new UC_NoiDungBaiDang(baiDang.IDBaiDang);
                ucNoiDungBaiDang.BaiDangDeleted += UcNoiDungBaiDang_BaiDangDeleted;
                flpDanhSachBaiDang.Controls.Add(ucNoiDungBaiDang);
            }
        }

        private void UcNoiDungBaiDang_BaiDangDeleted(object sender, EventArgs e)
        {
            LoadDanhSachBaiDang(); // Tải lại danh sách sau khi xóa
        }
    }
}
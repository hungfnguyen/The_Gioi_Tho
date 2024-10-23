using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheGioiTho.DAO;
using TheGioiTho.Model;

namespace TheGioiTho.Controller
{
    public partial class UC_DangBaiTimTho : UserControl
    {
        private BaiDangDAO baiDangDAO;
        private BaiDangNguoiDungDAO baiDangNguoiDungDAO;
        private int idNguoiDung;
        private string imagePath;
        private static readonly string IMAGE_FOLDER = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

        public UC_DangBaiTimTho(int idNguoiDung)
        {
            InitializeComponent();
            baiDangDAO = new BaiDangDAO();
            baiDangNguoiDungDAO = new BaiDangNguoiDungDAO();
            this.idNguoiDung = idNguoiDung;

            if (!Directory.Exists(IMAGE_FOLDER))
            {
                Directory.CreateDirectory(IMAGE_FOLDER);
            }
        }

        private void UC_DangBaiTimTho_Load(object sender, EventArgs e)
        {
            LoadLinhVuc();
            LoadKhungGio();
            dtpLichThoDen.MinDate = DateTime.Today;
        }

        private void LoadLinhVuc()
        {
            cmbCongViec.DataSource = baiDangDAO.GetDanhSachLinhVuc();
            cmbCongViec.DisplayMember = "TenLinhVuc";
            cmbCongViec.ValueMember = "IDLinhVuc";
        }

        private void LoadKhungGio()
        {
            for (int i = 7; i <= 17; i++)
            {
                cmbChonGio.Items.Add($"{i:D2}:00");
            }
            if (cmbChonGio.Items.Count > 0)
            {
                cmbChonGio.SelectedIndex = 0;
            }
        }

        private void btnThemHinhAnh_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string sourceFile = openFileDialog.FileName;
                    string fileName = Path.GetFileName(sourceFile);
                    string destFile = Path.Combine(IMAGE_FOLDER, fileName);

                    if (File.Exists(destFile))
                    {
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                        string fileExtension = Path.GetExtension(fileName);
                        fileName = $"{fileNameWithoutExtension}_{DateTime.Now:yyyyMMddHHmmss}{fileExtension}";
                        destFile = Path.Combine(IMAGE_FOLDER, fileName);
                    }

                    File.Copy(sourceFile, destFile, true);
                    imagePath = destFile;
                    picHinhAnh.Image = new Bitmap(destFile);
                    picHinhAnh.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
        }

        private void btnDangBai_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                BaiDang baiDang = new BaiDang
                {
                    IDLinhVuc = (int)cmbCongViec.SelectedValue,
                    TieuDe = txtTieuDe.Text,
                    MoTa = txtMoTa.Text,
                    HinhAnh = imagePath
                };

                int newBaiDangId = baiDangDAO.ThemBaiDang(baiDang);

                if (newBaiDangId > 0)
                {
                    BaiDangNguoiDung baiDangNguoiDung = new BaiDangNguoiDung
                    {
                        IDBaiDang = newBaiDangId,
                        IDNguoiDung = this.idNguoiDung,
                        NgayThoDen = dtpLichThoDen.Value.Date,
                        GioThoDen = TimeSpan.Parse(cmbChonGio.SelectedItem.ToString())
                    };

                    if (baiDangNguoiDungDAO.ThemBaiDangNguoiDung(baiDangNguoiDung))
                    {
                        MessageBox.Show("Đăng bài thành công!");
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Có lỗi xảy ra khi đăng bài. Vui lòng thử lại.");
                        baiDangDAO.XoaBaiDang(newBaiDangId);
                    }
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra khi đăng bài. Vui lòng thử lại.");
                }
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTieuDe.Text))
            {
                MessageBox.Show("Vui lòng nhập tiêu đề.");
                return false;
            }
            if (cmbCongViec.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn lĩnh vực công việc.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtMoTa.Text))
            {
                MessageBox.Show("Vui lòng nhập mô tả chi tiết.");
                return false;
            }
            if (string.IsNullOrEmpty(imagePath))
            {
                MessageBox.Show("Vui lòng thêm hình ảnh mô tả.");
                return false;
            }
            return true;
        }

        private void ClearForm()
        {
            txtTieuDe.Clear();
            cmbCongViec.SelectedIndex = -1;
            dtpLichThoDen.Value = DateTime.Today;
            cmbChonGio.SelectedIndex = -1;
            txtMoTa.Clear();
            picHinhAnh.Image = null;
            imagePath = null;
        }
    }
}
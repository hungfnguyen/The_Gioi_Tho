using System;
using System.Windows.Forms;
using TheGioiTho.Dao;
using TheGioiTho.Model;

namespace TheGioiTho.Controller
{
    public partial class ChiTietLich : UserControl
    {
        private LichHen _lichHen;
        private LichHenDAO _lichHenDao;
        private int IDNguoiDung = 1;

        // Các phương thức để truy cập các button
        public Button GetBtnHuyLichHen() { return btnHuyLichHen; }
        public Button GetBtnChapNhan() { return btnChapNhan; }
        public Button GetBtnDanhGia() { return btnDanhGia; }
        public Button GetBtnYeuThich() { return btnYeuThich; }
        public Button GetBtnDaYeuThich() { return btnDaYeuThich; }
        public Button GetBtnLyDoHuy() { return btnLyDoHuy; }
        public ChiTietLich(LichHen lichHen)
        {
            InitializeComponent();
            _lichHen = lichHen;
            _lichHenDao = new LichHenDAO(); // Khởi tạo DAO
            LoadLichHenInfo();
        }

        private void ChiTietLich_Load(object sender, EventArgs e)
        {
            // Xử lý khi UserControl được load
        }

        private void LoadLichHenInfo()
        {
            if (_lichHen != null)
            {
                // Hiển thị thông tin lịch hẹn
                txtLinhVuc.Text = _lichHen.LinhVuc;
                txtTenKhachHang.Text = _lichHen.Ten;
                txtSDT.Text = _lichHen.SDT;
                txtLichThoDen.Text = _lichHen.LichHenDen.ToShortDateString();
                txtGio.Text = _lichHen.Gio;
                txtGhiChu.Text = _lichHen.GhiChu;
                txtGiaTien.Text = _lichHen.GiaTien.ToString("N0") + " VNĐ";
            }
        }
        

        // Phương thức để ẩn/hiện các button tùy theo trường hợp
        public void ConfigureButtons(bool showHuyLichHen, bool showChapNhan,
                                     bool showDanhGia, bool showYeuThich,
                                     bool showDaYeuThich, bool showLyDoHuy)
        {
            btnHuyLichHen.Visible = showHuyLichHen;
            btnChapNhan.Visible = showChapNhan;
            btnDanhGia.Visible = showDanhGia;
            btnYeuThich.Visible = showYeuThich;
            btnDaYeuThich.Visible = showDaYeuThich;
            btnLyDoHuy.Visible = showLyDoHuy;
        }

        // Các phương thức xử lý sự kiện click cho mỗi button (nếu cần)
        private void btnHuyLichHen_Click(object sender, EventArgs e)
        {
            // Lấy thông tin cần thiết từ đối tượng LichHen
            int idCongViec = _lichHen.IDLichHen;
            int idNguoiDung = IDNguoiDung;
            int idTho = _lichHen.IDTho;
            string nguoiHuy = "Người dùng"; // Thay đổi nếu cần

            // Mở form lý do hủy
            LiDoHuy formHuy = new LiDoHuy(idCongViec, idNguoiDung, idTho, nguoiHuy);
            formHuy.ShowDialog(); // Mở form ở chế độ dialog
        }

       

        private void btnChapNhan_Click(object sender, EventArgs e)
        {
            // Xử lý khi nút Chấp nhận được click
        }

        private void btnDanhGia_Click(object sender, EventArgs e)
        {
            // Lấy thông tin cần thiết từ đối tượng LichHen
            int idCongViec = _lichHen.IDLichHen;
            int idNguoiDung = IDNguoiDung;

            // Tạo và hiển thị form đánh giá
            DanhGia formDanhGia = new DanhGia(idNguoiDung,idCongViec);
            formDanhGia.ShowDialog(); // Mở form ở chế độ dialog

        }

        private void btnYeuThich_Click(object sender, EventArgs e)
        {
            if (_lichHen == null)
            {
                MessageBox.Show("Không có thông tin lịch hẹn để yêu thích.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int idLichHen = _lichHen.IDLichHen;  // ID lịch hẹn (trước là ID công việc)
            int idNguoiDung = IDNguoiDung;       // ID người dùng (có thể lấy từ session hoặc đăng nhập)

            try
            {
                // Kiểm tra nếu đã yêu thích
                if (!_lichHenDao.DaYeuThich(idNguoiDung, idLichHen))
                {
                    // Thêm vào danh sách yêu thích
                    if (_lichHenDao.ThemYeuThich(idNguoiDung, idLichHen))
                    {
                        ConfigureButtons(false, false, false, false, true, false); // Hiện nút Đã Yêu Thích
                        MessageBox.Show("Đã thêm vào yêu thích!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không thể thêm vào yêu thích.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Lịch hẹn này đã được yêu thích trước đó.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnDaYeuThich_Click(object sender, EventArgs e)
        {
            if (_lichHen == null)
            {
                MessageBox.Show("Không có thông tin lịch hẹn.",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // Hiển thị hộp thoại xác nhận
            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn hủy yêu thích lịch hẹn này không?",
                "Xác nhận hủy yêu thích",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            // Nếu người dùng chọn Yes
            if (result == DialogResult.Yes)
            {
                try
                {
                    int idLichHen = _lichHen.IDLichHen;
                    int idNguoiDung = IDNguoiDung;

                    // Thực hiện xóa yêu thích
                    if (_lichHenDao.XoaYeuThich(idNguoiDung, idLichHen))
                    {
                        // Cập nhật hiển thị nút
                        ConfigureButtons(false, false, false, true, false, false); // Hiện lại nút Yêu Thích
                        MessageBox.Show("Đã hủy yêu thích thành công!",
                            "Thông báo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không thể hủy yêu thích. Vui lòng thử lại sau.",
                            "Lỗi",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}",
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            // Nếu người dùng chọn No thì không làm gì cả
        }

        private void btnLyDoHuy_Click(object sender, EventArgs e)
        {
            try
            {
                LyDoHuy lyDoHuy = _lichHenDao.GetLyDoHuy(_lichHen.IDLichHen);

                if (lyDoHuy != null)
                {
                    string thongTinHuy = $"Thông tin hủy lịch hẹn:\n\n" +
                                        $"Người hủy: {lyDoHuy.NguoiHuy}\n" +
                                        $"Thời gian hủy: {lyDoHuy.NgayHuy:dd/MM/yyyy HH:mm}\n" +
                                        $"Lý do: {lyDoHuy.LyDo}";

                    MessageBox.Show(thongTinHuy, "Lý do hủy", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin hủy cho lịch hẹn này.",
                                  "Thông báo",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi lấy lý do hủy: {ex.Message}",
                               "Lỗi",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
        }
    }
}

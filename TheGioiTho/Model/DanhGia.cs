using System;

namespace TheGioiTho.Model
{
    public class DanhGia
    {
        public int IDNguoiDung { get; set; }      // ID người dùng thực hiện đánh giá
        public int IDLichHen { get; set; }        // ID lịch hẹn được đánh giá
        public int SoSao { get; set; }            // Số sao đánh giá (nguyên)
        public string NhanXet { get; set; }       // Nhận xét
        public string HinhAnh { get; set; }       // Đường dẫn hình ảnh hoặc tên file

        // Constructor không tham số
        public DanhGia() { }

        // Constructor có tham số
        public DanhGia(int idNguoiDung, int idLichHen, int soSao, string nhanXet, string hinhAnh)
        {
            IDNguoiDung = idNguoiDung;
            IDLichHen = idLichHen;
            SoSao = soSao;
            NhanXet = nhanXet;
            HinhAnh = hinhAnh;
        }

        // Phương thức ToString để hiển thị thông tin (tùy chọn)
        public override string ToString()
        {
            return $"ID Người Dùng: {IDNguoiDung}, ID Lịch Hẹn: {IDLichHen}, Số Sao: {SoSao}, Nhận Xét: {NhanXet}, Hình Ảnh: {HinhAnh}";
        }

        // Phương thức kiểm tra tính hợp lệ của dữ liệu
        public bool IsValid()
        {
            return IDNguoiDung > 0 &&
                   IDLichHen > 0 &&
                   SoSao >= 1 &&
                   SoSao <= 5 &&
                   (NhanXet == null || NhanXet.Length <= 500) &&
                   (HinhAnh == null || HinhAnh.Length <= 255); // Kiểm tra độ dài tên file hình ảnh
        }
    }
}

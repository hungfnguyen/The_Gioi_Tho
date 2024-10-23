using System;

namespace TheGioiTho.Model
{
    public class DanhGia
    {
        public int IDNguoiDung { get; set; }
        public int IDCongViec { get; set; }
        public decimal SoSao { get; set; }
        public string NhanXet { get; set; }

        // Constructor không tham số
        public DanhGia() { }

        // Constructor có tham số
        public DanhGia(int idNguoiDung, int idCongViec, decimal soSao, string nhanXet)
        {
            IDNguoiDung = idNguoiDung;
            IDCongViec = idCongViec;
            SoSao = soSao;
            NhanXet = nhanXet;
        }

        // Phương thức ToString để hiển thị thông tin (tùy chọn)
        public override string ToString()
        {
            return $"ID Người Dùng: {IDNguoiDung}, ID Công Việc: {IDCongViec}, Số Sao: {SoSao}, Nhận Xét: {NhanXet}";
        }

        // Phương thức kiểm tra tính hợp lệ của dữ liệu
        public bool IsValid()
        {
            return IDNguoiDung > 0 &&
                   IDCongViec > 0 &&
                   SoSao > 0 &&
                   SoSao <= 5 &&
                   (NhanXet == null || NhanXet.Length <= 500);
        }
    }
}
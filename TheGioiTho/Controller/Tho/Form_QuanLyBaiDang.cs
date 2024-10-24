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
    public partial class Form_QuanLyBaiDang : Form
    {
        public Form_QuanLyBaiDang()
        {
            InitializeComponent();
        }

        private void dgvQuanLyBaiDang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form_QuanLyBaiDang_Load(object sender, EventArgs e)
        {
            dgvQuanLyBaiDang.DataSource = LayDanhSachBaiDang(); // Gán DataTable vào DataGridView
        }

        private DataTable LayDanhSachBaiDang()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT BaiDangTho.IDBaiDang, BaiDang.TieuDe, BaiDang.MoTa, BaiDangTho.GiaTien, BaiDangTho.ThoiGianThucHien " +
                                   "FROM BaiDangTho " +
                                   "INNER JOIN BaiDang ON BaiDangTho.IDBaiDang = BaiDang.IDBaiDang " +
                                   "WHERE BaiDangTho.IDTho = @IDTho";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IDTho", 1); // Thay IDTho bằng ID thợ thực tế cần lấy thông tin
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu bài đăng: " + ex.Message);
                }
            }
            return dt;
        }


        private int GetSelectedBaiDangId()
        {
            if (dgvQuanLyBaiDang.SelectedRows.Count > 0)
            {
                return Convert.ToInt32(dgvQuanLyBaiDang.SelectedRows[0].Cells["IDBaiDang"].Value);
            }
            return -1; // Trả về -1 nếu không có hàng nào được chọn
        }

        private void XoaBaiDang(int idBaiDang)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM BaiDangTho WHERE IDBaiDang = @IDBaiDang";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IDBaiDang", idBaiDang);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Xóa bài đăng thành công!");
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy bài đăng để xóa.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa bài đăng: " + ex.Message);
                }
            }
        }

        private void btnXoaBaiDang_Click(object sender, EventArgs e)
        {
            int idBaiDang = GetSelectedBaiDangId();
            if (idBaiDang != -1)
            {
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa bài đăng này?", "Xác nhận xóa", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    XoaBaiDang(idBaiDang);
                    dgvQuanLyBaiDang.DataSource = LayDanhSachBaiDang(); // Cập nhật lại DataGridView
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một bài đăng để xóa.");
            }
        }
    }
}

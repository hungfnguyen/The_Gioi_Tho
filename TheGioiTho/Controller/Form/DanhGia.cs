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
using System.Xml.Linq;
using TheGioiTho.Config;

namespace TheGioiTho
{
    public partial class DanhGia : Form
    {
        public DanhGia()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String text1 = txtNhanXet1.Text;
            String text2 = txtNhanXet2.Text;
            DanhGiaTho(2,4,3, "Bình Thường");
        }
        private void DanhGiaTho(int ID_NguoiDung, int ID_CongViec, decimal SoSao, string NhanXet)
        {
            
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "EXEC DanhGiaTho " + ID_NguoiDung + ", " + ID_CongViec + ", " + SoSao + ", " + "'" + NhanXet + "'" + ";";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Thực thi câu lệnh SQL
                        SqlDataReader reader = command.ExecuteReader();
                        MessageBox.Show("Đánh Giá Thành Công");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }
    }
}

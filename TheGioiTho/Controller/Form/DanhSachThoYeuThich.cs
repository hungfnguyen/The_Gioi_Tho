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

namespace TheGioiTho
{
    public partial class DanhSachThoYeuThich : Form
    {
        public DanhSachThoYeuThich()
        {
            InitializeComponent();
        }

        private void DanhSachThoYeuThich_Load(object sender, EventArgs e)
        {
            XemDanhSachThoYeuThich("Nguyen Van D");
        }
        private void XemDanhSachThoYeuThich(String name)
        {
            using(SqlConnection connection = DBConnection.GetConnection())
            {
                try 
                {
                    connection.Open();
                    string query = "SELECT * FROM XemDanhSachThoYeuThich('"+name+"')";
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable; // Giả sử bạn đã đặt tên DataGridView là dataGridView1
                } 
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DanhGia danhGia = new DanhGia();
            danhGia.Show();
        }
    }
}

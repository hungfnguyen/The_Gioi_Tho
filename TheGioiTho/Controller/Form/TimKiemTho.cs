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
    public partial class TimKiemTho : Form
    {
        public TimKiemTho()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String text =txtTimKiem.Text;
            if(text=="")
                XemTatCaBaiDangTho();
            TimKiemThoTheoLinhVuc(text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            XemTatCaBaiDangTho();
        }
        private void TimKiemThoTheoLinhVuc(String timkiem)
        {
            using (SqlConnection connection = DBConnection.GetConnection()) 
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM TimThoTheoLinhVuc('"+timkiem+"')";
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataGridViewTimKiem.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }
        private void XemTatCaBaiDangTho()
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM XemTatCaBaiDangTho";
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataGridViewTimKiem.DataSource = dataTable; // Giả sử bạn đã đặt tên DataGridView là dataGridView1
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            XemTopTho xemTopTho = new XemTopTho();
            xemTopTho.Show();
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DanhSachThoYeuThich danhSachThoYeuThich = new DanhSachThoYeuThich();
            danhSachThoYeuThich.Show();
        }
    }
}

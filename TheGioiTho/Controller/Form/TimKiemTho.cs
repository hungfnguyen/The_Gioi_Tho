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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void dataGridViewTimKiem_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra nếu cột đầu tiên (index 0) được click
            if (dataGridViewTimKiem.Columns[e.ColumnIndex].Name=="XemChiTiet")
            {
                // Lấy hàng hiện tại từ vị trí click
                DataGridViewRow row = dataGridViewTimKiem.Rows[e.RowIndex];
                {
                    // Lấy dữ liệu từ các cột khác trong hàng đó              
                    textBox1.Text = row.Cells[2].Value?.ToString(); // Cột thứ 2 (index 1)
                    textBox2.Text = row.Cells[3].Value?.ToString(); // Cột thứ 3 (index 2)
                    textBox3.Text = row.Cells[4].Value?.ToString(); // Cột thứ 4 (index 3)
                    textBox4.Text = row.Cells[5].Value?.ToString();
                    textBox5.Text = row.Cells[6].Value?.ToString();
                    textBox6.Text = row.Cells[7].Value?.ToString();
                    textBox7.Text = row.Cells[8].Value?.ToString();
                    // Thực hiện logic của bạn với dữ liệu đã lấy
                    
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}

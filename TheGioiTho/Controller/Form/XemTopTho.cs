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
    public partial class XemTopTho : Form
    {
        public XemTopTho()
        {
            InitializeComponent();
        }

        private void XemTopTho_Load(object sender, EventArgs e)
        {
            GetTop3ThoYeuThichNhat();
            GetTop3ThoSoSaoCaoNhat();
            GetTop3ThoBiHuyNhieuNhat();
            GetTop3ThoBiHuyNhieuNhat();
        }
        private void GetTop3ThoYeuThichNhat()
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM GetTop3ThoYeuThichNhat()";
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataGridView3.DataSource = dataTable; // Giả sử bạn đã đặt tên DataGridView là dataGridView1
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }
        private void GetTop3ThoSoSaoCaoNhat()
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM GetTop3ThoSoSaoCaoNhat()";
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
        private void GetTop3ThoBiHuyNhieuNhat()
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM GetTop3ThoBiHuyNhieuNhat()";
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataGridView2.DataSource = dataTable; // Giả sử bạn đã đặt tên DataGridView là dataGridView1
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }
    }
}

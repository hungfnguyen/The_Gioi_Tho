namespace TheGioiTho.Controller.Tho
{
    partial class Form_QuanLyBaiDang
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvQuanLyBaiDang = new System.Windows.Forms.DataGridView();
            this.btnXoaBaiDang = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQuanLyBaiDang)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvQuanLyBaiDang
            // 
            this.dgvQuanLyBaiDang.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvQuanLyBaiDang.Location = new System.Drawing.Point(24, 36);
            this.dgvQuanLyBaiDang.Name = "dgvQuanLyBaiDang";
            this.dgvQuanLyBaiDang.RowHeadersWidth = 51;
            this.dgvQuanLyBaiDang.RowTemplate.Height = 24;
            this.dgvQuanLyBaiDang.Size = new System.Drawing.Size(698, 356);
            this.dgvQuanLyBaiDang.TabIndex = 0;
            this.dgvQuanLyBaiDang.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvQuanLyBaiDang_CellContentClick);
            // 
            // btnXoaBaiDang
            // 
            this.btnXoaBaiDang.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnXoaBaiDang.Location = new System.Drawing.Point(750, 352);
            this.btnXoaBaiDang.Name = "btnXoaBaiDang";
            this.btnXoaBaiDang.Size = new System.Drawing.Size(150, 40);
            this.btnXoaBaiDang.TabIndex = 1;
            this.btnXoaBaiDang.Text = "Xóa Bài Đăng";
            this.btnXoaBaiDang.UseVisualStyleBackColor = false;
            this.btnXoaBaiDang.Click += new System.EventHandler(this.btnXoaBaiDang_Click);
            // 
            // Form_QuanLyBaiDang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 492);
            this.Controls.Add(this.btnXoaBaiDang);
            this.Controls.Add(this.dgvQuanLyBaiDang);
            this.Name = "Form_QuanLyBaiDang";
            this.Text = "Form_QuanLyBaiDang";
            this.Load += new System.EventHandler(this.Form_QuanLyBaiDang_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvQuanLyBaiDang)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvQuanLyBaiDang;
        private System.Windows.Forms.Button btnXoaBaiDang;
    }
}
namespace TheGioiTho
{
    partial class DanhGia
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNhanXet1 = new System.Windows.Forms.RichTextBox();
            this.txtNhanXet2 = new System.Windows.Forms.RichTextBox();
            this.btnDanhGia = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(290, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 39);
            this.label1.TabIndex = 0;
            this.label1.Text = "Đánh Giá";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(51, 163);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Số Sao";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(51, 290);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Nhận Xét";
            // 
            // txtNhanXet1
            // 
            this.txtNhanXet1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNhanXet1.Location = new System.Drawing.Point(203, 160);
            this.txtNhanXet1.Name = "txtNhanXet1";
            this.txtNhanXet1.Size = new System.Drawing.Size(306, 37);
            this.txtNhanXet1.TabIndex = 9;
            this.txtNhanXet1.Text = "";
            // 
            // txtNhanXet2
            // 
            this.txtNhanXet2.Location = new System.Drawing.Point(165, 270);
            this.txtNhanXet2.Name = "txtNhanXet2";
            this.txtNhanXet2.Size = new System.Drawing.Size(468, 126);
            this.txtNhanXet2.TabIndex = 10;
            this.txtNhanXet2.Text = "";
            // 
            // btnDanhGia
            // 
            this.btnDanhGia.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDanhGia.Location = new System.Drawing.Point(297, 461);
            this.btnDanhGia.Name = "btnDanhGia";
            this.btnDanhGia.Size = new System.Drawing.Size(151, 37);
            this.btnDanhGia.TabIndex = 11;
            this.btnDanhGia.Text = "Xác Nhận";
            this.btnDanhGia.UseVisualStyleBackColor = true;
            this.btnDanhGia.Click += new System.EventHandler(this.button1_Click);
            // 
            // DatLich
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 510);
            this.Controls.Add(this.btnDanhGia);
            this.Controls.Add(this.txtNhanXet2);
            this.Controls.Add(this.txtNhanXet1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "DatLich";
            this.Text = "DatLich";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox txtNhanXet1;
        private System.Windows.Forms.RichTextBox txtNhanXet2;
        private System.Windows.Forms.Button btnDanhGia;
    }
}
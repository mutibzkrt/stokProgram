namespace CokEglencez
{
    partial class GirisFormu
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GirisFormu));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnUye = new System.Windows.Forms.Button();
            this.btnSatinAl = new System.Windows.Forms.Button();
            this.btnAltay = new System.Windows.Forms.Button();
            this.btnGiris = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnYardım = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBoxKullanıcıGiris = new System.Windows.Forms.TextBox();
            this.txtBoxSifre = new System.Windows.Forms.TextBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(2, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(107, 128);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.btnUye);
            this.panel1.Controls.Add(this.btnSatinAl);
            this.panel1.Controls.Add(this.btnAltay);
            this.panel1.Controls.Add(this.btnGiris);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnYardım);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtBoxKullanıcıGiris);
            this.panel1.Controls.Add(this.txtBoxSifre);
            this.panel1.Location = new System.Drawing.Point(116, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(297, 241);
            this.panel1.TabIndex = 3;
            // 
            // btnUye
            // 
            this.btnUye.Location = new System.Drawing.Point(88, 124);
            this.btnUye.Name = "btnUye";
            this.btnUye.Size = new System.Drawing.Size(198, 28);
            this.btnUye.TabIndex = 4;
            this.btnUye.Text = "Yeni Üye Kayıt Oluştur";
            this.btnUye.UseVisualStyleBackColor = true;
            this.btnUye.Click += new System.EventHandler(this.btnUye_Click);
            // 
            // btnSatinAl
            // 
            this.btnSatinAl.Location = new System.Drawing.Point(88, 88);
            this.btnSatinAl.Name = "btnSatinAl";
            this.btnSatinAl.Size = new System.Drawing.Size(198, 28);
            this.btnSatinAl.TabIndex = 3;
            this.btnSatinAl.Text = "Satın Alma Giriş Yap";
            this.btnSatinAl.UseVisualStyleBackColor = true;
            this.btnSatinAl.Click += new System.EventHandler(this.btnSatinAl_Click);
            // 
            // btnAltay
            // 
            this.btnAltay.Location = new System.Drawing.Point(88, 193);
            this.btnAltay.Name = "btnAltay";
            this.btnAltay.Size = new System.Drawing.Size(198, 28);
            this.btnAltay.TabIndex = 6;
            this.btnAltay.Text = "Hakkımızda";
            this.btnAltay.UseVisualStyleBackColor = true;
            this.btnAltay.Click += new System.EventHandler(this.btnAltay_Click_1);
            // 
            // btnGiris
            // 
            this.btnGiris.Location = new System.Drawing.Point(88, 54);
            this.btnGiris.Name = "btnGiris";
            this.btnGiris.Size = new System.Drawing.Size(198, 28);
            this.btnGiris.TabIndex = 2;
            this.btnGiris.Text = "EO Stoğa Giriş Yap";
            this.btnGiris.UseVisualStyleBackColor = true;
            this.btnGiris.Click += new System.EventHandler(this.btnGiris_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(57, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Şifre:";
            // 
            // btnYardım
            // 
            this.btnYardım.Location = new System.Drawing.Point(88, 159);
            this.btnYardım.Name = "btnYardım";
            this.btnYardım.Size = new System.Drawing.Size(198, 28);
            this.btnYardım.TabIndex = 5;
            this.btnYardım.Text = "Yardım";
            this.btnYardım.UseVisualStyleBackColor = true;
            this.btnYardım.Click += new System.EventHandler(this.btnYardım_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Kulanıcı Giriş :";
            // 
            // txtBoxKullanıcıGiris
            // 
            this.txtBoxKullanıcıGiris.Location = new System.Drawing.Point(88, 7);
            this.txtBoxKullanıcıGiris.Name = "txtBoxKullanıcıGiris";
            this.txtBoxKullanıcıGiris.Size = new System.Drawing.Size(198, 20);
            this.txtBoxKullanıcıGiris.TabIndex = 0;
            // 
            // txtBoxSifre
            // 
            this.txtBoxSifre.Location = new System.Drawing.Point(88, 32);
            this.txtBoxSifre.Name = "txtBoxSifre";
            this.txtBoxSifre.PasswordChar = '*';
            this.txtBoxSifre.Size = new System.Drawing.Size(198, 20);
            this.txtBoxSifre.TabIndex = 1;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(2, 136);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(107, 110);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // GirisFormu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(416, 244);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "GirisFormu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kullanıcı Giriş";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnYardım;
        private System.Windows.Forms.Button btnGiris;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBoxKullanıcıGiris;
        private System.Windows.Forms.TextBox txtBoxSifre;
        private System.Windows.Forms.Button btnAltay;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnSatinAl;
        private System.Windows.Forms.Button btnUye;
    }
}


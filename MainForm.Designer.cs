namespace video_processing
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.FaceCountTimer = new System.Windows.Forms.Timer(this.components);
            this.VideoRecordTimer = new System.Windows.Forms.Timer(this.components);
            this.QR_PB = new System.Windows.Forms.PictureBox();
            this.info_lbl = new MetroFramework.Controls.MetroLabel();
            this.TxtReadTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.QR_PB)).BeginInit();
            this.SuspendLayout();
            // 
            // FaceCountTimer
            // 
            this.FaceCountTimer.Interval = 2000;
            this.FaceCountTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // VideoRecordTimer
            // 
            this.VideoRecordTimer.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // QR_PB
            // 
            this.QR_PB.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.QR_PB.Location = new System.Drawing.Point(69, 96);
            this.QR_PB.Name = "QR_PB";
            this.QR_PB.Size = new System.Drawing.Size(400, 320);
            this.QR_PB.TabIndex = 0;
            this.QR_PB.TabStop = false;
            // 
            // info_lbl
            // 
            this.info_lbl.AutoSize = true;
            this.info_lbl.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.info_lbl.Location = new System.Drawing.Point(69, 441);
            this.info_lbl.Name = "info_lbl";
            this.info_lbl.Size = new System.Drawing.Size(118, 19);
            this.info_lbl.TabIndex = 1;
            this.info_lbl.Text = "Kayıt Yapılıyor...";
            // 
            // TxtReadTimer
            // 
            this.TxtReadTimer.Tick += new System.EventHandler(this.TxtReadTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 480);
            this.Controls.Add(this.info_lbl);
            this.Controls.Add(this.QR_PB);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Resizable = false;
            this.Text = "Videonuzu İndirmek İçin QR Kodu Taratabilirsiniz";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.QR_PB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer FaceCountTimer;
        private System.Windows.Forms.Timer VideoRecordTimer;
        private System.Windows.Forms.PictureBox QR_PB;
        private MetroFramework.Controls.MetroLabel info_lbl;
        private System.Windows.Forms.Timer TxtReadTimer;
    }
}


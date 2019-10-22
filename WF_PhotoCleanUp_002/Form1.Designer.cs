namespace WF_PhotoCleanUp_002
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.textBox_src_path = new System.Windows.Forms.TextBox();
            this.textBox_dest_path = new System.Windows.Forms.TextBox();
            this.btn_sel_src_folder = new System.Windows.Forms.Button();
            this.btn_sel_dest_folder = new System.Windows.Forms.Button();
            this.label_src_path = new System.Windows.Forms.Label();
            this.label_dest_path = new System.Windows.Forms.Label();
            this.textBox_log_1 = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btn_reserch = new System.Windows.Forms.Button();
            this.btn_clean = new System.Windows.Forms.Button();
            this.progressBar_clean = new System.Windows.Forms.ProgressBar();
            this.tb_cnt1 = new System.Windows.Forms.TextBox();
            this.tb_cnt2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox_src_path
            // 
            this.textBox_src_path.Location = new System.Drawing.Point(72, 12);
            this.textBox_src_path.Name = "textBox_src_path";
            this.textBox_src_path.Size = new System.Drawing.Size(272, 21);
            this.textBox_src_path.TabIndex = 0;
            // 
            // textBox_dest_path
            // 
            this.textBox_dest_path.Location = new System.Drawing.Point(72, 47);
            this.textBox_dest_path.Name = "textBox_dest_path";
            this.textBox_dest_path.Size = new System.Drawing.Size(272, 21);
            this.textBox_dest_path.TabIndex = 1;
            // 
            // btn_sel_src_folder
            // 
            this.btn_sel_src_folder.Location = new System.Drawing.Point(350, 9);
            this.btn_sel_src_folder.Name = "btn_sel_src_folder";
            this.btn_sel_src_folder.Size = new System.Drawing.Size(75, 23);
            this.btn_sel_src_folder.TabIndex = 2;
            this.btn_sel_src_folder.Text = "폴더선택";
            this.btn_sel_src_folder.UseVisualStyleBackColor = true;
            this.btn_sel_src_folder.Click += new System.EventHandler(this.btn_sel_src_folder_Click);
            // 
            // btn_sel_dest_folder
            // 
            this.btn_sel_dest_folder.Location = new System.Drawing.Point(350, 47);
            this.btn_sel_dest_folder.Name = "btn_sel_dest_folder";
            this.btn_sel_dest_folder.Size = new System.Drawing.Size(75, 23);
            this.btn_sel_dest_folder.TabIndex = 3;
            this.btn_sel_dest_folder.Text = "폴더선택";
            this.btn_sel_dest_folder.UseVisualStyleBackColor = true;
            this.btn_sel_dest_folder.Click += new System.EventHandler(this.btn_sel_dest_folder_Click);
            // 
            // label_src_path
            // 
            this.label_src_path.AutoSize = true;
            this.label_src_path.Location = new System.Drawing.Point(12, 17);
            this.label_src_path.Name = "label_src_path";
            this.label_src_path.Size = new System.Drawing.Size(53, 12);
            this.label_src_path.TabIndex = 4;
            this.label_src_path.Text = "소스폴더";
            // 
            // label_dest_path
            // 
            this.label_dest_path.AutoSize = true;
            this.label_dest_path.Location = new System.Drawing.Point(12, 52);
            this.label_dest_path.Name = "label_dest_path";
            this.label_dest_path.Size = new System.Drawing.Size(53, 12);
            this.label_dest_path.TabIndex = 5;
            this.label_dest_path.Text = "대상폴더";
            // 
            // textBox_log_1
            // 
            this.textBox_log_1.Location = new System.Drawing.Point(14, 179);
            this.textBox_log_1.Multiline = true;
            this.textBox_log_1.Name = "textBox_log_1";
            this.textBox_log_1.Size = new System.Drawing.Size(411, 261);
            this.textBox_log_1.TabIndex = 6;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btn_reserch
            // 
            this.btn_reserch.Location = new System.Drawing.Point(350, 86);
            this.btn_reserch.Name = "btn_reserch";
            this.btn_reserch.Size = new System.Drawing.Size(75, 33);
            this.btn_reserch.TabIndex = 7;
            this.btn_reserch.Text = "탐색";
            this.btn_reserch.UseVisualStyleBackColor = true;
            this.btn_reserch.Click += new System.EventHandler(this.btn_reserch_Click);
            // 
            // btn_clean
            // 
            this.btn_clean.Enabled = false;
            this.btn_clean.Location = new System.Drawing.Point(350, 132);
            this.btn_clean.Name = "btn_clean";
            this.btn_clean.Size = new System.Drawing.Size(75, 33);
            this.btn_clean.TabIndex = 8;
            this.btn_clean.Text = "정리";
            this.btn_clean.UseVisualStyleBackColor = true;
            this.btn_clean.Click += new System.EventHandler(this.btn_clean_Click);
            // 
            // progressBar_clean
            // 
            this.progressBar_clean.Location = new System.Drawing.Point(12, 149);
            this.progressBar_clean.Name = "progressBar_clean";
            this.progressBar_clean.Size = new System.Drawing.Size(319, 16);
            this.progressBar_clean.TabIndex = 9;
            // 
            // tb_cnt1
            // 
            this.tb_cnt1.Location = new System.Drawing.Point(15, 98);
            this.tb_cnt1.Name = "tb_cnt1";
            this.tb_cnt1.ReadOnly = true;
            this.tb_cnt1.Size = new System.Drawing.Size(75, 21);
            this.tb_cnt1.TabIndex = 10;
            // 
            // tb_cnt2
            // 
            this.tb_cnt2.Location = new System.Drawing.Point(113, 98);
            this.tb_cnt2.Name = "tb_cnt2";
            this.tb_cnt2.ReadOnly = true;
            this.tb_cnt2.Size = new System.Drawing.Size(75, 21);
            this.tb_cnt2.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "처리파일";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(123, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "찾은파일";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(96, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "/";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(459, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 12);
            this.label4.TabIndex = 15;
            this.label4.Text = "<사용법>";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(459, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(241, 12);
            this.label5.TabIndex = 16;
            this.label5.Text = "1. 정리할 폴더를 선택합니다.(사진, 동영상)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(459, 67);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(163, 12);
            this.label6.TabIndex = 17;
            this.label6.Text = "2. 이동될 폴더를 선택합니다.";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(459, 96);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(303, 12);
            this.label7.TabIndex = 18;
            this.label7.Text = "3. \"탐색\" 버튼을 누르면 폴더안에 파일들을 로드합니다.";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(459, 124);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(239, 12);
            this.label8.TabIndex = 19;
            this.label8.Text = "4. \"정리\" 버튼을 누르면 정리를 시작합니다.";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(459, 179);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(106, 12);
            this.label9.TabIndex = 20;
            this.label9.Text = "<지원파일 형식>";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(459, 208);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(318, 24);
            this.label10.TabIndex = 21;
            this.label10.Text = "사   진 : JPEG, PNG, WebP, GIF, ICO, BMP, TIFF, PSD,\r\n            Camera Raw(NEF, C" +
    "R2, ORF, ...)";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(459, 256);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(114, 12);
            this.label11.TabIndex = 22;
            this.label11.Text = "동영상 : MP4, MOV";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.Location = new System.Drawing.Point(459, 302);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(154, 12);
            this.label12.TabIndex = 23;
            this.label12.Text = "<Exif 정보가 없는 경우>";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(459, 332);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(381, 12);
            this.label13.TabIndex = 24;
            this.label13.Text = "찍은 날짜 정보가 없는 경우는 사진이 원본이 아닐 가능성이 높습니다.";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(459, 354);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(310, 12);
            this.label14.TabIndex = 25;
            this.label14.Text = "위 해당하는 파일은 대상폴더\\None 폴더에 들어갑니다.";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(459, 427);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(200, 12);
            this.label15.TabIndex = 26;
            this.label15.Text = "개발자 주소 : leejeeh7@gmail.com";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(459, 404);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(313, 12);
            this.label16.TabIndex = 27;
            this.label16.Text = "기능개선이나 요청사항 있으시면 아래 메일로 연락주세요";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(13, 134);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(41, 12);
            this.label17.TabIndex = 28;
            this.label17.Text = "상태바";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 452);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_cnt2);
            this.Controls.Add(this.tb_cnt1);
            this.Controls.Add(this.progressBar_clean);
            this.Controls.Add(this.btn_clean);
            this.Controls.Add(this.btn_reserch);
            this.Controls.Add(this.textBox_log_1);
            this.Controls.Add(this.label_dest_path);
            this.Controls.Add(this.label_src_path);
            this.Controls.Add(this.btn_sel_dest_folder);
            this.Controls.Add(this.btn_sel_src_folder);
            this.Controls.Add(this.textBox_dest_path);
            this.Controls.Add(this.textBox_src_path);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "포토클린업";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_src_path;
        private System.Windows.Forms.TextBox textBox_dest_path;
        private System.Windows.Forms.Button btn_sel_src_folder;
        private System.Windows.Forms.Button btn_sel_dest_folder;
        private System.Windows.Forms.Label label_src_path;
        private System.Windows.Forms.Label label_dest_path;
        private System.Windows.Forms.TextBox textBox_log_1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btn_reserch;
        private System.Windows.Forms.Button btn_clean;
        private System.Windows.Forms.ProgressBar progressBar_clean;
        private System.Windows.Forms.TextBox tb_cnt1;
        private System.Windows.Forms.TextBox tb_cnt2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
    }
}


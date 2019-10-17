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
            this.SuspendLayout();
            // 
            // textBox_src_path
            // 
            this.textBox_src_path.Location = new System.Drawing.Point(72, 12);
            this.textBox_src_path.Name = "textBox_src_path";
            this.textBox_src_path.Size = new System.Drawing.Size(236, 21);
            this.textBox_src_path.TabIndex = 0;
            // 
            // textBox_dest_path
            // 
            this.textBox_dest_path.Location = new System.Drawing.Point(72, 59);
            this.textBox_dest_path.Name = "textBox_dest_path";
            this.textBox_dest_path.Size = new System.Drawing.Size(236, 21);
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
            this.btn_sel_dest_folder.Location = new System.Drawing.Point(350, 59);
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
            this.label_src_path.Location = new System.Drawing.Point(12, 21);
            this.label_src_path.Name = "label_src_path";
            this.label_src_path.Size = new System.Drawing.Size(53, 12);
            this.label_src_path.TabIndex = 4;
            this.label_src_path.Text = "소스폴더";
            // 
            // label_dest_path
            // 
            this.label_dest_path.AutoSize = true;
            this.label_dest_path.Location = new System.Drawing.Point(12, 68);
            this.label_dest_path.Name = "label_dest_path";
            this.label_dest_path.Size = new System.Drawing.Size(53, 12);
            this.label_dest_path.TabIndex = 5;
            this.label_dest_path.Text = "대상폴더";
            // 
            // textBox_log_1
            // 
            this.textBox_log_1.Location = new System.Drawing.Point(14, 255);
            this.textBox_log_1.Multiline = true;
            this.textBox_log_1.Name = "textBox_log_1";
            this.textBox_log_1.Size = new System.Drawing.Size(438, 185);
            this.textBox_log_1.TabIndex = 6;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btn_reserch
            // 
            this.btn_reserch.Location = new System.Drawing.Point(14, 109);
            this.btn_reserch.Name = "btn_reserch";
            this.btn_reserch.Size = new System.Drawing.Size(75, 23);
            this.btn_reserch.TabIndex = 7;
            this.btn_reserch.Text = "탐색";
            this.btn_reserch.UseVisualStyleBackColor = true;
            this.btn_reserch.Click += new System.EventHandler(this.btn_reserch_Click);
            // 
            // btn_clean
            // 
            this.btn_clean.Enabled = false;
            this.btn_clean.Location = new System.Drawing.Point(111, 109);
            this.btn_clean.Name = "btn_clean";
            this.btn_clean.Size = new System.Drawing.Size(75, 23);
            this.btn_clean.TabIndex = 8;
            this.btn_clean.Text = "정리";
            this.btn_clean.UseVisualStyleBackColor = true;
            this.btn_clean.Click += new System.EventHandler(this.btn_clean_Click);
            // 
            // progressBar_clean
            // 
            this.progressBar_clean.Location = new System.Drawing.Point(12, 187);
            this.progressBar_clean.Name = "progressBar_clean";
            this.progressBar_clean.Size = new System.Drawing.Size(438, 23);
            this.progressBar_clean.TabIndex = 9;
            // 
            // tb_cnt1
            // 
            this.tb_cnt1.Location = new System.Drawing.Point(311, 111);
            this.tb_cnt1.Name = "tb_cnt1";
            this.tb_cnt1.ReadOnly = true;
            this.tb_cnt1.Size = new System.Drawing.Size(52, 21);
            this.tb_cnt1.TabIndex = 10;
            // 
            // tb_cnt2
            // 
            this.tb_cnt2.Location = new System.Drawing.Point(373, 111);
            this.tb_cnt2.Name = "tb_cnt2";
            this.tb_cnt2.ReadOnly = true;
            this.tb_cnt2.Size = new System.Drawing.Size(52, 21);
            this.tb_cnt2.TabIndex = 11;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 452);
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
            this.Name = "Form1";
            this.Text = "Form1";
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
    }
}


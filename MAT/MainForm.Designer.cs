namespace MAT
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.startTestBtn = new System.Windows.Forms.Button();
            this.usbConnectLabel = new System.Windows.Forms.Label();
            this.snEdit = new System.Windows.Forms.TextBox();
            this.snEditLabel = new System.Windows.Forms.Label();
            this.timeLabel = new System.Windows.Forms.Label();
            this.timeLabelName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.stationLabel = new System.Windows.Forms.Label();
            this.testResult = new System.Windows.Forms.Label();
            this.version_label = new System.Windows.Forms.Label();
            this.listView1 = new MAT.Dbj_ListView();
            this.SuspendLayout();
            // 
            // startTestBtn
            // 
            this.startTestBtn.Location = new System.Drawing.Point(822, 12);
            this.startTestBtn.Name = "startTestBtn";
            this.startTestBtn.Size = new System.Drawing.Size(65, 82);
            this.startTestBtn.TabIndex = 1;
            this.startTestBtn.Text = "开始";
            this.startTestBtn.UseVisualStyleBackColor = true;
            this.startTestBtn.Click += new System.EventHandler(this.startTestBtn_Click);
            // 
            // usbConnectLabel
            // 
            this.usbConnectLabel.BackColor = System.Drawing.Color.White;
            this.usbConnectLabel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.usbConnectLabel.Location = new System.Drawing.Point(9, 23);
            this.usbConnectLabel.Name = "usbConnectLabel";
            this.usbConnectLabel.Size = new System.Drawing.Size(88, 59);
            this.usbConnectLabel.TabIndex = 2;
            this.usbConnectLabel.Text = "测试等待开始...";
            this.usbConnectLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.usbConnectLabel.Click += new System.EventHandler(this.usbConnectLabel_Click);
            // 
            // snEdit
            // 
            this.snEdit.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.snEdit.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.snEdit.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.snEdit.Location = new System.Drawing.Point(630, 53);
            this.snEdit.Name = "snEdit";
            this.snEdit.Size = new System.Drawing.Size(176, 30);
            this.snEdit.TabIndex = 0;
            this.snEdit.Visible = false;
            this.snEdit.TextChanged += new System.EventHandler(this.snEdit_TextChanged);
            // 
            // snEditLabel
            // 
            this.snEditLabel.AutoSize = true;
            this.snEditLabel.Font = new System.Drawing.Font("宋体", 21.75F);
            this.snEditLabel.Location = new System.Drawing.Point(536, 53);
            this.snEditLabel.Name = "snEditLabel";
            this.snEditLabel.Size = new System.Drawing.Size(88, 29);
            this.snEditLabel.TabIndex = 4;
            this.snEditLabel.Text = "IMEI:";
            this.snEditLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.snEditLabel.Visible = false;
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.BackColor = System.Drawing.Color.White;
            this.timeLabel.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Bold);
            this.timeLabel.Location = new System.Drawing.Point(430, 46);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(93, 29);
            this.timeLabel.TabIndex = 5;
            this.timeLabel.Text = "00:00";
            // 
            // timeLabelName
            // 
            this.timeLabelName.AutoSize = true;
            this.timeLabelName.Location = new System.Drawing.Point(433, 27);
            this.timeLabelName.Name = "timeLabelName";
            this.timeLabelName.Size = new System.Drawing.Size(65, 12);
            this.timeLabelName.TabIndex = 6;
            this.timeLabelName.Text = "测试时间：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(344, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "终端版本号：";
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.BackColor = System.Drawing.Color.White;
            this.versionLabel.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.versionLabel.Location = new System.Drawing.Point(344, 46);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(64, 20);
            this.versionLabel.TabIndex = 8;
            this.versionLabel.Text = "-----";
            this.versionLabel.Click += new System.EventHandler(this.versionLabel_Click);
            // 
            // stationLabel
            // 
            this.stationLabel.BackColor = System.Drawing.Color.White;
            this.stationLabel.Location = new System.Drawing.Point(109, 23);
            this.stationLabel.Name = "stationLabel";
            this.stationLabel.Size = new System.Drawing.Size(88, 59);
            this.stationLabel.TabIndex = 9;
            this.stationLabel.Text = "站位信息:";
            this.stationLabel.Click += new System.EventHandler(this.stationLabel_Click);
            // 
            // testResult
            // 
            this.testResult.BackColor = System.Drawing.Color.White;
            this.testResult.Location = new System.Drawing.Point(210, 23);
            this.testResult.Name = "testResult";
            this.testResult.Size = new System.Drawing.Size(128, 59);
            this.testResult.TabIndex = 9;
            this.testResult.Text = "测试结果：";
            this.testResult.Click += new System.EventHandler(this.testResult_Click);
            // 
            // version_label
            // 
            this.version_label.AutoSize = true;
            this.version_label.Location = new System.Drawing.Point(10, 6);
            this.version_label.Name = "version_label";
            this.version_label.Size = new System.Drawing.Size(11, 12);
            this.version_label.TabIndex = 10;
            this.version_label.Text = "v";
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(3, 100);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(898, 635);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(902, 731);
            this.Controls.Add(this.version_label);
            this.Controls.Add(this.testResult);
            this.Controls.Add(this.stationLabel);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.timeLabelName);
            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.snEditLabel);
            this.Controls.Add(this.snEdit);
            this.Controls.Add(this.usbConnectLabel);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.startTestBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "MAT for ZJ210";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startTestBtn;
        private Dbj_ListView listView1;
        private System.Windows.Forms.Label usbConnectLabel;
        private System.Windows.Forms.TextBox snEdit;
        private System.Windows.Forms.Label snEditLabel;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.Label timeLabelName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label stationLabel;
        private System.Windows.Forms.Label testResult;
        private System.Windows.Forms.Label version_label;
    }
}


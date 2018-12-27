namespace MAT
{
    partial class PowerVoiceForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PowerVoiceForm));
            this.label_power = new System.Windows.Forms.Label();
            this.label_voice = new System.Windows.Forms.Label();
            this.button_no_light = new System.Windows.Forms.Button();
            this.button_no_voice = new System.Windows.Forms.Button();
            this.button_has_light = new System.Windows.Forms.Button();
            this.button_has_voice = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_power
            // 
            this.label_power.AutoSize = true;
            this.label_power.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_power.Location = new System.Drawing.Point(18, 138);
            this.label_power.Name = "label_power";
            this.label_power.Size = new System.Drawing.Size(191, 38);
            this.label_power.TabIndex = 6;
            this.label_power.Text = "红灯是否亮过";
            // 
            // label_voice
            // 
            this.label_voice.AutoSize = true;
            this.label_voice.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_voice.Location = new System.Drawing.Point(18, 32);
            this.label_voice.Name = "label_voice";
            this.label_voice.Size = new System.Drawing.Size(249, 38);
            this.label_voice.TabIndex = 7;
            this.label_voice.Text = "喇叭是否发出声音";
            // 
            // button_no_light
            // 
            this.button_no_light.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_no_light.Location = new System.Drawing.Point(161, 191);
            this.button_no_light.Name = "button_no_light";
            this.button_no_light.Size = new System.Drawing.Size(104, 39);
            this.button_no_light.TabIndex = 2;
            this.button_no_light.Text = "灯没灭";
            this.button_no_light.UseVisualStyleBackColor = true;
            this.button_no_light.Click += new System.EventHandler(this.button_no_light_Click);
            // 
            // button_no_voice
            // 
            this.button_no_voice.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_no_voice.Location = new System.Drawing.Point(161, 77);
            this.button_no_voice.Name = "button_no_voice";
            this.button_no_voice.Size = new System.Drawing.Size(104, 39);
            this.button_no_voice.TabIndex = 3;
            this.button_no_voice.Text = "无声音";
            this.button_no_voice.UseVisualStyleBackColor = true;
            this.button_no_voice.Click += new System.EventHandler(this.button_no_voice_Click);
            // 
            // button_has_light
            // 
            this.button_has_light.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_has_light.Location = new System.Drawing.Point(18, 191);
            this.button_has_light.Name = "button_has_light";
            this.button_has_light.Size = new System.Drawing.Size(104, 39);
            this.button_has_light.TabIndex = 4;
            this.button_has_light.Text = "灯灭了";
            this.button_has_light.UseVisualStyleBackColor = true;
            this.button_has_light.Click += new System.EventHandler(this.button_has_light_Click);
            // 
            // button_has_voice
            // 
            this.button_has_voice.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_has_voice.Location = new System.Drawing.Point(18, 77);
            this.button_has_voice.Name = "button_has_voice";
            this.button_has_voice.Size = new System.Drawing.Size(104, 39);
            this.button_has_voice.TabIndex = 5;
            this.button_has_voice.Text = "有声音";
            this.button_has_voice.UseVisualStyleBackColor = true;
            this.button_has_voice.Click += new System.EventHandler(this.button_has_voice_Click);
            // 
            // PowerVoiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.label_power);
            this.Controls.Add(this.label_voice);
            this.Controls.Add(this.button_no_light);
            this.Controls.Add(this.button_no_voice);
            this.Controls.Add(this.button_has_light);
            this.Controls.Add(this.button_has_voice);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PowerVoiceForm";
            this.Text = "熄火线与喇叭测试";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_power;
        private System.Windows.Forms.Label label_voice;
        private System.Windows.Forms.Button button_no_light;
        private System.Windows.Forms.Button button_no_voice;
        private System.Windows.Forms.Button button_has_light;
        private System.Windows.Forms.Button button_has_voice;
    }
}
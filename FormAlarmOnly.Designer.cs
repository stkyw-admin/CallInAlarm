
namespace StkywControlPanelCallInAlarm
{
    partial class FormAlarmOnly
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
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timerFlashing = new System.Windows.Forms.Timer(this.components);
            this.timerOthersAlert = new System.Windows.Forms.Timer(this.components);
            this.labelAlertOther = new System.Windows.Forms.Label();
            this.labelAlertOther2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(29, 32);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(29, 28);
            this.button1.TabIndex = 49;
            this.button1.Text = "buttonAlarm";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(13, 13);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(134, 124);
            this.pictureBox1.TabIndex = 48;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // timerFlashing
            // 
            this.timerFlashing.Interval = 500;
            this.timerFlashing.Tick += new System.EventHandler(this.timerFlashing_Tick);
            // 
            // timerOthersAlert
            // 
            this.timerOthersAlert.Interval = 10000;
            this.timerOthersAlert.Tick += new System.EventHandler(this.timerOthersAlert_Tick);
            // 
            // labelAlertOther
            // 
            this.labelAlertOther.AutoSize = true;
            this.labelAlertOther.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelAlertOther.Location = new System.Drawing.Point(0, 0);
            this.labelAlertOther.Name = "labelAlertOther";
            this.labelAlertOther.Size = new System.Drawing.Size(0, 17);
            this.labelAlertOther.TabIndex = 50;
            // 
            // labelAlertOther2
            // 
            this.labelAlertOther2.AutoSize = true;
            this.labelAlertOther2.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelAlertOther2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAlertOther2.Location = new System.Drawing.Point(0, 0);
            this.labelAlertOther2.Name = "labelAlertOther2";
            this.labelAlertOther2.Size = new System.Drawing.Size(66, 29);
            this.labelAlertOther2.TabIndex = 51;
            this.labelAlertOther2.Text = "label";
            // 
            // FormAlarmOnly
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(160, 149);
            this.Controls.Add(this.labelAlertOther2);
            this.Controls.Add(this.labelAlertOther);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "FormAlarmOnly";
            this.Text = "FormAlarmOnly";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAlarmOnly_FormClosing);
            this.Load += new System.EventHandler(this.FormAlarmOnly_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timerFlashing;
        private System.Windows.Forms.Timer timerOthersAlert;
        private System.Windows.Forms.Label labelAlertOther;
        private System.Windows.Forms.Label labelAlertOther2;
    }
}
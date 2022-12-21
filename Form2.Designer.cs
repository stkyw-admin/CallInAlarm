
using System.Timers;

namespace StkywControlPanelCallIn
{
    partial class FormStkywControlPanelCallInV2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormStkywControlPanelCallInV2));
            this.timerFlashing = new System.Windows.Forms.Timer(this.components);
            this.timerOthersAlert = new System.Windows.Forms.Timer(this.components);
            this.labelAlertOther = new System.Windows.Forms.Label();
            this.timerAutoDelay = new System.Windows.Forms.Timer(this.components);
            this.buttonCallInText = new System.Windows.Forms.Button();
            this.textBoxCallInNew = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelAlertOther
            // 
            this.labelAlertOther.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelAlertOther.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAlertOther.Location = new System.Drawing.Point(0, 0);
            this.labelAlertOther.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAlertOther.Name = "labelAlertOther";
            this.labelAlertOther.Size = new System.Drawing.Size(1, 88);
            this.labelAlertOther.TabIndex = 46;
            this.labelAlertOther.Text = "labelAlertOther";
            this.labelAlertOther.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelAlertOther.Visible = false;
            // 
            // buttonCallInText
            // 
            this.buttonCallInText.BackColor = System.Drawing.SystemColors.Control;
            this.buttonCallInText.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.buttonCallInText.FlatAppearance.BorderSize = 2;
            this.buttonCallInText.ForeColor = System.Drawing.Color.Black;
            this.buttonCallInText.Location = new System.Drawing.Point(161, 27);
            this.buttonCallInText.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCallInText.Name = "buttonCallInText";
            this.buttonCallInText.Size = new System.Drawing.Size(71, 28);
            this.buttonCallInText.TabIndex = 13;
            this.buttonCallInText.Text = "Kald";
            this.buttonCallInText.UseVisualStyleBackColor = false;
            this.buttonCallInText.Click += new System.EventHandler(this.buttonCallInText_Click_1);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(251, 31);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(29, 23);
            this.button1.TabIndex = 52;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxCallInNew
            // 
            this.textBoxCallInNew.Location = new System.Drawing.Point(13, 30);
            this.textBoxCallInNew.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxCallInNew.Name = "textBoxCallInNew";
            this.textBoxCallInNew.Size = new System.Drawing.Size(140, 22);
            this.textBoxCallInNew.TabIndex = 12;
            this.textBoxCallInNew.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxCallInNew_KeyDown);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.Image = Properties.Resources.Sorrytokeepyouwaiting_circle__62px;
            this.pictureBox1.Location = new System.Drawing.Point(239, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(62, 62);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 47;
            this.pictureBox1.TabStop = false;
            // 
            // FormStkywControlPanelCallInV2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(312, 88);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textBoxCallInNew);
            this.Controls.Add(this.buttonCallInText);
            this.Controls.Add(this.labelAlertOther);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormStkywControlPanelCallInV2";
            this.Text = "STKYW Control Panel";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormStkywControlPanelCallInV2_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timerFlashing;
        private System.Windows.Forms.Timer timerOthersAlert;
        private System.Windows.Forms.Label labelAlertOther;
        private System.Windows.Forms.Timer timerAutoDelay;
        private System.Windows.Forms.Button buttonCallInText;
        private System.Windows.Forms.TextBox textBoxCallInNew;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
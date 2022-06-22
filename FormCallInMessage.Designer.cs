namespace StkywControlPanelLight
{
    partial class FormCallInMessage
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
            this.textBoxCallIn = new System.Windows.Forms.TextBox();
            this.buttonCallInConfirm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxCallIn
            // 
            this.textBoxCallIn.Location = new System.Drawing.Point(16, 15);
            this.textBoxCallIn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxCallIn.Name = "textBoxCallIn";
            this.textBoxCallIn.Size = new System.Drawing.Size(159, 22);
            this.textBoxCallIn.TabIndex = 0;
            // 
            // buttonCallInConfirm
            // 
            this.buttonCallInConfirm.Location = new System.Drawing.Point(75, 45);
            this.buttonCallInConfirm.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonCallInConfirm.Name = "buttonCallInConfirm";
            this.buttonCallInConfirm.Size = new System.Drawing.Size(100, 28);
            this.buttonCallInConfirm.TabIndex = 1;
            this.buttonCallInConfirm.Text = "Kald";
            this.buttonCallInConfirm.UseVisualStyleBackColor = true;
            this.buttonCallInConfirm.Click += new System.EventHandler(this.buttonCallInConfirm_Click);
            // 
            // FormCallInMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(184, 85);
            this.Controls.Add(this.buttonCallInConfirm);
            this.Controls.Add(this.textBoxCallIn);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormCallInMessage";
            this.Text = "FormCallInMessage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxCallIn;
        private System.Windows.Forms.Button buttonCallInConfirm;
    }
}
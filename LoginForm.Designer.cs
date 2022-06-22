
namespace StkywControlPanelLight
{
    partial class LoginForm
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
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxCompany = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.checkBoxRememberMe = new System.Windows.Forms.CheckBox();
            this.textBoxWeekLogin = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBoxOnlyAlarm = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Brugernavn";
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Location = new System.Drawing.Point(121, 15);
            this.textBoxUsername.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(132, 22);
            this.textBoxUsername.TabIndex = 1;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(121, 47);
            this.textBoxPassword.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(132, 22);
            this.textBoxPassword.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 50);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Adgangskode";
            // 
            // textBoxCompany
            // 
            this.textBoxCompany.Location = new System.Drawing.Point(121, 79);
            this.textBoxCompany.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxCompany.Name = "textBoxCompany";
            this.textBoxCompany.Size = new System.Drawing.Size(132, 22);
            this.textBoxCompany.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 82);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Virksomhed";
            // 
            // buttonLogin
            // 
            this.buttonLogin.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonLogin.Location = new System.Drawing.Point(16, 197);
            this.buttonLogin.Margin = new System.Windows.Forms.Padding(4);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(113, 28);
            this.buttonLogin.TabIndex = 5;
            this.buttonLogin.Text = "Login";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(141, 197);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(113, 28);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // checkBoxRememberMe
            // 
            this.checkBoxRememberMe.AutoSize = true;
            this.checkBoxRememberMe.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxRememberMe.Location = new System.Drawing.Point(13, 109);
            this.checkBoxRememberMe.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxRememberMe.Name = "checkBoxRememberMe";
            this.checkBoxRememberMe.Size = new System.Drawing.Size(173, 21);
            this.checkBoxRememberMe.TabIndex = 4;
            this.checkBoxRememberMe.Text = "Husk mine oplysninger";
            this.checkBoxRememberMe.UseVisualStyleBackColor = true;
            // 
            // textBoxWeekLogin
            // 
            this.textBoxWeekLogin.Location = new System.Drawing.Point(141, 137);
            this.textBoxWeekLogin.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxWeekLogin.Name = "textBoxWeekLogin";
            this.textBoxWeekLogin.Size = new System.Drawing.Size(112, 22);
            this.textBoxWeekLogin.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 140);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Hent specifik uge";
            // 
            // checkBoxOnlyAlarm
            // 
            this.checkBoxOnlyAlarm.AutoSize = true;
            this.checkBoxOnlyAlarm.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxOnlyAlarm.Location = new System.Drawing.Point(13, 168);
            this.checkBoxOnlyAlarm.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxOnlyAlarm.Name = "checkBoxOnlyAlarm";
            this.checkBoxOnlyAlarm.Size = new System.Drawing.Size(127, 21);
            this.checkBoxOnlyAlarm.TabIndex = 9;
            this.checkBoxOnlyAlarm.Text = "Brug kun Alarm";
            this.checkBoxOnlyAlarm.UseVisualStyleBackColor = true;
            // 
            // LoginForm
            // 
            this.AcceptButton = this.buttonLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(267, 238);
            this.Controls.Add(this.checkBoxOnlyAlarm);
            this.Controls.Add(this.textBoxWeekLogin);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.checkBoxRememberMe);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonLogin);
            this.Controls.Add(this.textBoxCompany);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxUsername);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "LoginForm";
            this.Text = "LoginForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxCompany;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.CheckBox checkBoxRememberMe;
        private System.Windows.Forms.TextBox textBoxWeekLogin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBoxOnlyAlarm;
    }
}
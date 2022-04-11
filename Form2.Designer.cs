
namespace StkywControlPanelLight
{
    partial class FormStkywControlPanelLightV2
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
            this.buttonTimeslotMerge3 = new System.Windows.Forms.Button();
            this.buttonTimeslotMerge2 = new System.Windows.Forms.Button();
            this.buttonTimeslotMerge4 = new System.Windows.Forms.Button();
            this.buttonAwayPresent = new System.Windows.Forms.Button();
            this.buttonTimeslotMerge5 = new System.Windows.Forms.Button();
            this.buttonTimeslotResetToZero = new System.Windows.Forms.Button();
            this.buttonTimeslotNext = new System.Windows.Forms.Button();
            this.buttonTimeslotNextBreak = new System.Windows.Forms.Button();
            this.buttonTimeslotBestGuess = new System.Windows.Forms.Button();
            this.buttonTimeslotPrevious = new System.Windows.Forms.Button();
            this.labelTimeslotType = new System.Windows.Forms.Label();
            this.labelSlutTid = new System.Windows.Forms.Label();
            this.labelStartTid = new System.Windows.Forms.Label();
            this.labelCurrentDelay = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timerFlashing = new System.Windows.Forms.Timer(this.components);
            this.timerOthersAlert = new System.Windows.Forms.Timer(this.components);
            this.labelAlertOther = new System.Windows.Forms.Label();
            this.timerAutoDelay = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonTimeslotMerge3
            // 
            this.buttonTimeslotMerge3.Location = new System.Drawing.Point(212, 50);
            this.buttonTimeslotMerge3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonTimeslotMerge3.Name = "buttonTimeslotMerge3";
            this.buttonTimeslotMerge3.Size = new System.Drawing.Size(51, 28);
            this.buttonTimeslotMerge3.TabIndex = 7;
            this.buttonTimeslotMerge3.Text = ">> 3";
            this.buttonTimeslotMerge3.UseVisualStyleBackColor = true;
            this.buttonTimeslotMerge3.Click += new System.EventHandler(this.buttonTimeslotMerge3_Click);
            // 
            // buttonTimeslotMerge2
            // 
            this.buttonTimeslotMerge2.Location = new System.Drawing.Point(153, 50);
            this.buttonTimeslotMerge2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonTimeslotMerge2.Name = "buttonTimeslotMerge2";
            this.buttonTimeslotMerge2.Size = new System.Drawing.Size(51, 28);
            this.buttonTimeslotMerge2.TabIndex = 6;
            this.buttonTimeslotMerge2.Text = ">> 2";
            this.buttonTimeslotMerge2.UseVisualStyleBackColor = true;
            this.buttonTimeslotMerge2.Click += new System.EventHandler(this.buttonTimeslotMerge2_Click);
            // 
            // buttonTimeslotMerge4
            // 
            this.buttonTimeslotMerge4.Location = new System.Drawing.Point(271, 50);
            this.buttonTimeslotMerge4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonTimeslotMerge4.Name = "buttonTimeslotMerge4";
            this.buttonTimeslotMerge4.Size = new System.Drawing.Size(51, 28);
            this.buttonTimeslotMerge4.TabIndex = 8;
            this.buttonTimeslotMerge4.Text = ">> 4";
            this.buttonTimeslotMerge4.UseVisualStyleBackColor = true;
            this.buttonTimeslotMerge4.Click += new System.EventHandler(this.buttonTimeslotMerge4_Click);
            // 
            // buttonAwayPresent
            // 
            this.buttonAwayPresent.BackColor = System.Drawing.Color.PaleGreen;
            this.buttonAwayPresent.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.buttonAwayPresent.FlatAppearance.BorderSize = 2;
            this.buttonAwayPresent.ForeColor = System.Drawing.Color.DarkRed;
            this.buttonAwayPresent.Location = new System.Drawing.Point(388, 50);
            this.buttonAwayPresent.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonAwayPresent.Name = "buttonAwayPresent";
            this.buttonAwayPresent.Size = new System.Drawing.Size(51, 28);
            this.buttonAwayPresent.TabIndex = 10;
            this.buttonAwayPresent.Text = "Væk";
            this.buttonAwayPresent.UseVisualStyleBackColor = false;
            this.buttonAwayPresent.Click += new System.EventHandler(this.buttonAwayPresent_Click);
            // 
            // buttonTimeslotMerge5
            // 
            this.buttonTimeslotMerge5.Location = new System.Drawing.Point(329, 50);
            this.buttonTimeslotMerge5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonTimeslotMerge5.Name = "buttonTimeslotMerge5";
            this.buttonTimeslotMerge5.Size = new System.Drawing.Size(51, 28);
            this.buttonTimeslotMerge5.TabIndex = 9;
            this.buttonTimeslotMerge5.Text = ">> 5";
            this.buttonTimeslotMerge5.UseVisualStyleBackColor = true;
            this.buttonTimeslotMerge5.Click += new System.EventHandler(this.buttonTimeslotMerge5_Click);
            // 
            // buttonTimeslotResetToZero
            // 
            this.buttonTimeslotResetToZero.BackColor = System.Drawing.Color.DarkGray;
            this.buttonTimeslotResetToZero.Location = new System.Drawing.Point(388, 15);
            this.buttonTimeslotResetToZero.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonTimeslotResetToZero.Name = "buttonTimeslotResetToZero";
            this.buttonTimeslotResetToZero.Size = new System.Drawing.Size(51, 28);
            this.buttonTimeslotResetToZero.TabIndex = 5;
            this.buttonTimeslotResetToZero.Text = "0";
            this.buttonTimeslotResetToZero.UseVisualStyleBackColor = false;
            this.buttonTimeslotResetToZero.Click += new System.EventHandler(this.buttonTimeslotResetToZero_Click);
            // 
            // buttonTimeslotNext
            // 
            this.buttonTimeslotNext.BackColor = System.Drawing.Color.MediumSpringGreen;
            this.buttonTimeslotNext.Location = new System.Drawing.Point(153, 15);
            this.buttonTimeslotNext.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonTimeslotNext.Name = "buttonTimeslotNext";
            this.buttonTimeslotNext.Size = new System.Drawing.Size(51, 28);
            this.buttonTimeslotNext.TabIndex = 1;
            this.buttonTimeslotNext.Text = ">>";
            this.buttonTimeslotNext.UseVisualStyleBackColor = false;
            this.buttonTimeslotNext.Click += new System.EventHandler(this.buttonTimeslotNext_Click);
            // 
            // buttonTimeslotNextBreak
            // 
            this.buttonTimeslotNextBreak.BackColor = System.Drawing.Color.Khaki;
            this.buttonTimeslotNextBreak.Location = new System.Drawing.Point(329, 15);
            this.buttonTimeslotNextBreak.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonTimeslotNextBreak.Name = "buttonTimeslotNextBreak";
            this.buttonTimeslotNextBreak.Size = new System.Drawing.Size(51, 28);
            this.buttonTimeslotNextBreak.TabIndex = 4;
            this.buttonTimeslotNextBreak.Text = "| |";
            this.buttonTimeslotNextBreak.UseVisualStyleBackColor = false;
            this.buttonTimeslotNextBreak.Click += new System.EventHandler(this.buttonTimeslotNextBreak_Click);
            // 
            // buttonTimeslotBestGuess
            // 
            this.buttonTimeslotBestGuess.BackColor = System.Drawing.Color.PaleTurquoise;
            this.buttonTimeslotBestGuess.Location = new System.Drawing.Point(271, 15);
            this.buttonTimeslotBestGuess.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonTimeslotBestGuess.Name = "buttonTimeslotBestGuess";
            this.buttonTimeslotBestGuess.Size = new System.Drawing.Size(51, 28);
            this.buttonTimeslotBestGuess.TabIndex = 3;
            this.buttonTimeslotBestGuess.Text = "|>";
            this.buttonTimeslotBestGuess.UseVisualStyleBackColor = false;
            this.buttonTimeslotBestGuess.Click += new System.EventHandler(this.buttonTimeslotBestGuess_Click);
            // 
            // buttonTimeslotPrevious
            // 
            this.buttonTimeslotPrevious.BackColor = System.Drawing.Color.LightCoral;
            this.buttonTimeslotPrevious.Location = new System.Drawing.Point(212, 15);
            this.buttonTimeslotPrevious.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonTimeslotPrevious.Name = "buttonTimeslotPrevious";
            this.buttonTimeslotPrevious.Size = new System.Drawing.Size(51, 28);
            this.buttonTimeslotPrevious.TabIndex = 2;
            this.buttonTimeslotPrevious.Text = "<<";
            this.buttonTimeslotPrevious.UseVisualStyleBackColor = false;
            this.buttonTimeslotPrevious.Click += new System.EventHandler(this.buttonTimeslotPrevious_Click);
            // 
            // labelTimeslotType
            // 
            this.labelTimeslotType.AutoSize = true;
            this.labelTimeslotType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTimeslotType.Location = new System.Drawing.Point(21, 122);
            this.labelTimeslotType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTimeslotType.Name = "labelTimeslotType";
            this.labelTimeslotType.Size = new System.Drawing.Size(109, 20);
            this.labelTimeslotType.TabIndex = 0;
            this.labelTimeslotType.Text = "TimeslotType";
            // 
            // labelSlutTid
            // 
            this.labelSlutTid.AutoSize = true;
            this.labelSlutTid.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSlutTid.Location = new System.Drawing.Point(21, 91);
            this.labelSlutTid.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSlutTid.Name = "labelSlutTid";
            this.labelSlutTid.Size = new System.Drawing.Size(56, 20);
            this.labelSlutTid.TabIndex = 0;
            this.labelSlutTid.Text = "Sluttid";
            // 
            // labelStartTid
            // 
            this.labelStartTid.AutoSize = true;
            this.labelStartTid.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStartTid.Location = new System.Drawing.Point(21, 60);
            this.labelStartTid.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelStartTid.Name = "labelStartTid";
            this.labelStartTid.Size = new System.Drawing.Size(63, 20);
            this.labelStartTid.TabIndex = 0;
            this.labelStartTid.Text = "Starttid";
            // 
            // labelCurrentDelay
            // 
            this.labelCurrentDelay.AutoSize = true;
            this.labelCurrentDelay.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCurrentDelay.Location = new System.Drawing.Point(16, 11);
            this.labelCurrentDelay.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCurrentDelay.Name = "labelCurrentDelay";
            this.labelCurrentDelay.Size = new System.Drawing.Size(79, 29);
            this.labelCurrentDelay.TabIndex = 0;
            this.labelCurrentDelay.Text = "Delay";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownHeight = 78;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.IntegralHeight = false;
            this.comboBox1.Items.AddRange(new object[] {
            "",
            "5 minutter",
            "10 minutter",
            "15 minutter",
            "20 minutter",
            "25 minutter",
            "30 minutter",
            "35 minutter",
            "40 minutter",
            "45 minutter",
            "50 minutter",
            "55 minutter",
            "60 minutter"});
            this.comboBox1.Location = new System.Drawing.Point(153, 86);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(167, 24);
            this.comboBox1.TabIndex = 11;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::StkywControlPanelLight.Properties.Resources.Sorrytokeepyouwaiting_circle_Red_50px;
            this.pictureBox1.Location = new System.Drawing.Point(372, 86);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(67, 62);
            this.pictureBox1.TabIndex = 45;
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
            this.labelAlertOther.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelAlertOther.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAlertOther.Location = new System.Drawing.Point(0, 0);
            this.labelAlertOther.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAlertOther.Name = "labelAlertOther";
            this.labelAlertOther.Size = new System.Drawing.Size(1, 162);
            this.labelAlertOther.TabIndex = 46;
            this.labelAlertOther.Text = "labelAlertOther";
            this.labelAlertOther.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelAlertOther.Visible = false;
            // 
            // timerAutoDelay
            // 
            this.timerAutoDelay.Interval = 60000;
            this.timerAutoDelay.Tick += new System.EventHandler(this.timerAutoDelay_Tick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(388, 105);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(29, 28);
            this.button1.TabIndex = 47;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormStkywControlPanelLightV2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 162);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.labelAlertOther);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.buttonTimeslotMerge3);
            this.Controls.Add(this.buttonTimeslotMerge2);
            this.Controls.Add(this.buttonTimeslotMerge4);
            this.Controls.Add(this.buttonAwayPresent);
            this.Controls.Add(this.buttonTimeslotMerge5);
            this.Controls.Add(this.buttonTimeslotResetToZero);
            this.Controls.Add(this.buttonTimeslotNext);
            this.Controls.Add(this.buttonTimeslotNextBreak);
            this.Controls.Add(this.buttonTimeslotBestGuess);
            this.Controls.Add(this.buttonTimeslotPrevious);
            this.Controls.Add(this.labelTimeslotType);
            this.Controls.Add(this.labelSlutTid);
            this.Controls.Add(this.labelStartTid);
            this.Controls.Add(this.labelCurrentDelay);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormStkywControlPanelLightV2";
            this.Text = "STKYW Control Panel";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormStkywControlPanelLightV2_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonTimeslotMerge3;
        private System.Windows.Forms.Button buttonTimeslotMerge2;
        private System.Windows.Forms.Button buttonTimeslotMerge4;
        private System.Windows.Forms.Button buttonAwayPresent;
        private System.Windows.Forms.Button buttonTimeslotMerge5;
        private System.Windows.Forms.Button buttonTimeslotResetToZero;
        private System.Windows.Forms.Button buttonTimeslotNext;
        private System.Windows.Forms.Button buttonTimeslotNextBreak;
        private System.Windows.Forms.Button buttonTimeslotBestGuess;
        private System.Windows.Forms.Button buttonTimeslotPrevious;
        private System.Windows.Forms.Label labelTimeslotType;
        private System.Windows.Forms.Label labelSlutTid;
        private System.Windows.Forms.Label labelStartTid;
        private System.Windows.Forms.Label labelCurrentDelay;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timerFlashing;
        private System.Windows.Forms.Timer timerOthersAlert;
        private System.Windows.Forms.Label labelAlertOther;
        private System.Windows.Forms.Timer timerAutoDelay;
        private System.Windows.Forms.Button button1;
    }
}
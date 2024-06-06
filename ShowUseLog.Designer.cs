namespace StkywControlPanelCallInAlarm
{
    partial class ShowUseLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowUseLog));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Arbejdsdato = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartTid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SlutTid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TidArbejdet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonExtractToFile = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Arbejdsdato,
            this.StartTid,
            this.SlutTid,
            this.TidArbejdet});
            this.dataGridView1.Location = new System.Drawing.Point(13, 13);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(775, 246);
            this.dataGridView1.TabIndex = 0;
            // 
            // Arbejdsdato
            // 
            this.Arbejdsdato.HeaderText = "Arbejdsdato";
            this.Arbejdsdato.MinimumWidth = 6;
            this.Arbejdsdato.Name = "Arbejdsdato";
            this.Arbejdsdato.ReadOnly = true;
            this.Arbejdsdato.Width = 125;
            // 
            // StartTid
            // 
            this.StartTid.HeaderText = "Start tid";
            this.StartTid.MinimumWidth = 6;
            this.StartTid.Name = "StartTid";
            this.StartTid.ReadOnly = true;
            this.StartTid.Width = 125;
            // 
            // SlutTid
            // 
            this.SlutTid.HeaderText = "Slut tid";
            this.SlutTid.MinimumWidth = 6;
            this.SlutTid.Name = "SlutTid";
            this.SlutTid.ReadOnly = true;
            this.SlutTid.Width = 125;
            // 
            // TidArbejdet
            // 
            this.TidArbejdet.HeaderText = "Tid arbejdet";
            this.TidArbejdet.MinimumWidth = 6;
            this.TidArbejdet.Name = "TidArbejdet";
            this.TidArbejdet.ReadOnly = true;
            this.TidArbejdet.Width = 125;
            // 
            // buttonExtractToFile
            // 
            this.buttonExtractToFile.Location = new System.Drawing.Point(638, 266);
            this.buttonExtractToFile.Name = "buttonExtractToFile";
            this.buttonExtractToFile.Size = new System.Drawing.Size(149, 27);
            this.buttonExtractToFile.TabIndex = 1;
            this.buttonExtractToFile.Text = "Gem data";
            this.buttonExtractToFile.UseVisualStyleBackColor = true;
            this.buttonExtractToFile.Click += new System.EventHandler(this.buttonExtractToFile_Click);
            // 
            // ShowUseLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 299);
            this.Controls.Add(this.buttonExtractToFile);
            this.Controls.Add(this.dataGridView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ShowUseLog";
            this.Text = "Vis arbejdstid";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Arbejdsdato;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartTid;
        private System.Windows.Forms.DataGridViewTextBoxColumn SlutTid;
        private System.Windows.Forms.DataGridViewTextBoxColumn TidArbejdet;
        private System.Windows.Forms.Button buttonExtractToFile;

    }
}
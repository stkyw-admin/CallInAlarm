
using System.Timers;

namespace StkywControlPanelCallInAlarm
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
            this.comboBoxWadAlias = new System.Windows.Forms.ComboBox();
            this.buttonOpenCloseChat = new System.Windows.Forms.Button();
            this.panelChat = new System.Windows.Forms.Panel();
            this.buttonAssistNoTime = new System.Windows.Forms.Button();
            this.buttonAssistComingAsap = new System.Windows.Forms.Button();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.buttonSendChat = new System.Windows.Forms.Button();
            this.textBoxWriteChatMessage = new System.Windows.Forms.TextBox();
            this.textBoxMainChatWindow = new System.Windows.Forms.TextBox();
            this.comboBoxChatUsersList = new System.Windows.Forms.ComboBox();
            this.timerRefreshMessage = new System.Windows.Forms.Timer(this.components);
            this.timerRefreshMessageWhileClosed = new System.Windows.Forms.Timer(this.components);
            this.timerAssist = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelChat.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerFlashing
            // 
            this.timerFlashing.Tick += new System.EventHandler(this.timerFlashing_Tick);
            // 
            // timerOthersAlert
            // 
            this.timerOthersAlert.Tick += new System.EventHandler(this.timerOthersAlert_Tick);
            // 
            // labelAlertOther
            // 
            this.labelAlertOther.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelAlertOther.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAlertOther.Location = new System.Drawing.Point(0, 0);
            this.labelAlertOther.Name = "labelAlertOther";
            this.labelAlertOther.Size = new System.Drawing.Size(1, 254);
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
            this.buttonCallInText.Location = new System.Drawing.Point(121, 37);
            this.buttonCallInText.Name = "buttonCallInText";
            this.buttonCallInText.Size = new System.Drawing.Size(53, 23);
            this.buttonCallInText.TabIndex = 13;
            this.buttonCallInText.Text = "Kald";
            this.buttonCallInText.UseVisualStyleBackColor = false;
            this.buttonCallInText.Click += new System.EventHandler(this.buttonCallInText_Click);
            // 
            // textBoxCallInNew
            // 
            this.textBoxCallInNew.Location = new System.Drawing.Point(10, 40);
            this.textBoxCallInNew.Name = "textBoxCallInNew";
            this.textBoxCallInNew.Size = new System.Drawing.Size(106, 20);
            this.textBoxCallInNew.TabIndex = 12;
            this.textBoxCallInNew.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxCallInNew_KeyDown);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.Image = global::StkywControlPanelCallInAlarm.Properties.Resources._62Alarm;
            this.pictureBox1.Location = new System.Drawing.Point(179, 10);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(46, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 47;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // comboBoxWadAlias
            // 
            this.comboBoxWadAlias.FormattingEnabled = true;
            this.comboBoxWadAlias.Location = new System.Drawing.Point(10, 10);
            this.comboBoxWadAlias.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxWadAlias.Name = "comboBoxWadAlias";
            this.comboBoxWadAlias.Size = new System.Drawing.Size(92, 21);
            this.comboBoxWadAlias.TabIndex = 48;
            this.comboBoxWadAlias.SelectedIndexChanged += new System.EventHandler(this.comboBoxWadAlias_SelectedIndexChanged);
            // 
            // buttonOpenCloseChat
            // 
            this.buttonOpenCloseChat.BackColor = System.Drawing.SystemColors.Control;
            this.buttonOpenCloseChat.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.buttonOpenCloseChat.FlatAppearance.BorderSize = 2;
            this.buttonOpenCloseChat.ForeColor = System.Drawing.Color.Black;
            this.buttonOpenCloseChat.Location = new System.Drawing.Point(107, 8);
            this.buttonOpenCloseChat.Name = "buttonOpenCloseChat";
            this.buttonOpenCloseChat.Size = new System.Drawing.Size(67, 23);
            this.buttonOpenCloseChat.TabIndex = 49;
            this.buttonOpenCloseChat.Text = "Åbn chat";
            this.buttonOpenCloseChat.UseVisualStyleBackColor = false;
            this.buttonOpenCloseChat.Click += new System.EventHandler(this.buttonOpenCloseChat_Click);
            // 
            // panelChat
            // 
            this.panelChat.Controls.Add(this.buttonHelp);
            this.panelChat.Controls.Add(this.buttonSendChat);
            this.panelChat.Controls.Add(this.textBoxWriteChatMessage);
            this.panelChat.Controls.Add(this.textBoxMainChatWindow);
            this.panelChat.Controls.Add(this.comboBoxChatUsersList);
            this.panelChat.Location = new System.Drawing.Point(10, 66);
            this.panelChat.Margin = new System.Windows.Forms.Padding(2);
            this.panelChat.Name = "panelChat";
            this.panelChat.Size = new System.Drawing.Size(215, 179);
            this.panelChat.TabIndex = 51;
            // 
            // buttonAssistNoTime
            // 
            this.buttonAssistNoTime.Location = new System.Drawing.Point(59, 8);
            this.buttonAssistNoTime.Name = "buttonAssistNoTime";
            this.buttonAssistNoTime.Size = new System.Drawing.Size(115, 23);
            this.buttonAssistNoTime.TabIndex = 57;
            this.buttonAssistNoTime.Text = "Jeg har ikke tid";
            this.buttonAssistNoTime.UseVisualStyleBackColor = true;
            this.buttonAssistNoTime.Click += new System.EventHandler(this.buttonAssistNoTime_Click);
            // 
            // buttonAssistComingAsap
            // 
            this.buttonAssistComingAsap.Location = new System.Drawing.Point(59, 37);
            this.buttonAssistComingAsap.Name = "buttonAssistComingAsap";
            this.buttonAssistComingAsap.Size = new System.Drawing.Size(115, 23);
            this.buttonAssistComingAsap.TabIndex = 58;
            this.buttonAssistComingAsap.Text = "Kommer ASAP";
            this.buttonAssistComingAsap.UseVisualStyleBackColor = true;
            this.buttonAssistComingAsap.Click += new System.EventHandler(this.buttonAssistComingAsap_Click);
            // 
            // buttonHelp
            // 
            this.buttonHelp.BackColor = System.Drawing.SystemColors.Control;
            this.buttonHelp.Location = new System.Drawing.Point(156, 3);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(57, 21);
            this.buttonHelp.TabIndex = 5;
            this.buttonHelp.Text = "Tilkald";
            this.buttonHelp.UseVisualStyleBackColor = false;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // buttonSendChat
            // 
            this.buttonSendChat.BackColor = System.Drawing.SystemColors.Control;
            this.buttonSendChat.Location = new System.Drawing.Point(170, 131);
            this.buttonSendChat.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSendChat.Name = "buttonSendChat";
            this.buttonSendChat.Size = new System.Drawing.Size(43, 46);
            this.buttonSendChat.TabIndex = 3;
            this.buttonSendChat.Text = "Send Chat";
            this.buttonSendChat.UseVisualStyleBackColor = false;
            this.buttonSendChat.Click += new System.EventHandler(this.buttonSendChat_Click);
            // 
            // textBoxWriteChatMessage
            // 
            this.textBoxWriteChatMessage.Location = new System.Drawing.Point(0, 131);
            this.textBoxWriteChatMessage.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxWriteChatMessage.Multiline = true;
            this.textBoxWriteChatMessage.Name = "textBoxWriteChatMessage";
            this.textBoxWriteChatMessage.Size = new System.Drawing.Size(165, 46);
            this.textBoxWriteChatMessage.TabIndex = 2;
            this.textBoxWriteChatMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxWriteChatMessage_KeyDown);
            this.textBoxWriteChatMessage.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxWriteChatMessage_KeyUp);
            // 
            // textBoxMainChatWindow
            // 
            this.textBoxMainChatWindow.BackColor = System.Drawing.SystemColors.Menu;
            this.textBoxMainChatWindow.Location = new System.Drawing.Point(2, 24);
            this.textBoxMainChatWindow.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxMainChatWindow.Multiline = true;
            this.textBoxMainChatWindow.Name = "textBoxMainChatWindow";
            this.textBoxMainChatWindow.ReadOnly = true;
            this.textBoxMainChatWindow.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxMainChatWindow.ShortcutsEnabled = false;
            this.textBoxMainChatWindow.Size = new System.Drawing.Size(211, 102);
            this.textBoxMainChatWindow.TabIndex = 1;
            this.textBoxMainChatWindow.TabStop = false;
            // 
            // comboBoxChatUsersList
            // 
            this.comboBoxChatUsersList.FormattingEnabled = true;
            this.comboBoxChatUsersList.Location = new System.Drawing.Point(2, 2);
            this.comboBoxChatUsersList.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxChatUsersList.Name = "comboBoxChatUsersList";
            this.comboBoxChatUsersList.Size = new System.Drawing.Size(149, 21);
            this.comboBoxChatUsersList.TabIndex = 0;
            this.comboBoxChatUsersList.SelectedIndexChanged += new System.EventHandler(this.comboBoxChatUsersList_SelectedIndexChanged);
            // 
            // timerRefreshMessage
            // 
            this.timerRefreshMessage.Interval = 30000;
            this.timerRefreshMessage.Tick += new System.EventHandler(this.timerRefreshMessage_Tick);
            // 
            // timerRefreshMessageWhileClosed
            // 
            this.timerRefreshMessageWhileClosed.Enabled = true;
            this.timerRefreshMessageWhileClosed.Interval = 15000;
            this.timerRefreshMessageWhileClosed.Tick += new System.EventHandler(this.timerRefreshMessageWhileClosed_Tick);
            // 
            // timerAssist
            // 
            this.timerAssist.Interval = 200;
            this.timerAssist.Tick += new System.EventHandler(this.timerAssist_Tick);
            // 
            // FormStkywControlPanelCallInV2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(234, 254);
            this.Controls.Add(this.buttonAssistNoTime);
            this.Controls.Add(this.buttonAssistComingAsap);
            this.Controls.Add(this.panelChat);
            this.Controls.Add(this.buttonOpenCloseChat);
            this.Controls.Add(this.comboBoxWadAlias);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textBoxCallInNew);
            this.Controls.Add(this.buttonCallInText);
            this.Controls.Add(this.labelAlertOther);
            this.DataBindings.Add(new System.Windows.Forms.Binding("Location", global::StkywControlPanelCallInAlarm.Properties.Settings.Default, "Location", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = global::StkywControlPanelCallInAlarm.Properties.Settings.Default.Location;
            this.Name = "FormStkywControlPanelCallInV2";
            this.Text = "STKYW Control Panel";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormStkywControlPanelCallInV2_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormStkywControlPanelCallInV2_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelChat.ResumeLayout(false);
            this.panelChat.PerformLayout();
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
        private System.Windows.Forms.ComboBox comboBoxWadAlias;
        private System.Windows.Forms.Button buttonOpenCloseChat;
        private System.Windows.Forms.Panel panelChat;
        private System.Windows.Forms.Button buttonSendChat;
        private System.Windows.Forms.TextBox textBoxWriteChatMessage;
        private System.Windows.Forms.TextBox textBoxMainChatWindow;
        private System.Windows.Forms.ComboBox comboBoxChatUsersList;
        private System.Windows.Forms.Timer timerRefreshMessage;
        private System.Windows.Forms.Timer timerRefreshMessageWhileClosed;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.Timer timerAssist;
        private System.Windows.Forms.Button buttonAssistNoTime;
        private System.Windows.Forms.Button buttonAssistComingAsap;
    }
}
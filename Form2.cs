using Newtonsoft.Json;
using StkywControlPanelCallInAlarm.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static StkywControlPanelCallInAlarm.FormStkywControlPanelCallInV2;

namespace StkywControlPanelCallInAlarm
{
    public partial class FormStkywControlPanelCallInV2 : Form
    {
        public int panelCNRecipientID;
        public int timerOption;
        public string helpFromUser;
        public string chatOpenCloseStatus;
        public int userId;
        public string userName;
        public string nameOfUser;
        public int companyID;
        public string directions;
        public int sizeWidthBeforeAssist;
        public int sizeHeightBeforeAssist;
        public string callInLastName;
        public DateTime callInLastTime;
        static HttpClient client = new HttpClient();
        static List<Schedule> allSchedules = new List<Schedule>();
        static List<Schedule> schedules = new List<Schedule>();
        static List<vw_EmployeeLogin> allEmployees = new List<vw_EmployeeLogin>();
        static List<vw_EmployeeLogin> elComboBoxChatUsersList = new List<vw_EmployeeLogin>();
        static List<EmployeeCurrentUsage> ecuList = new List<EmployeeCurrentUsage>();
        static List<CallIn> callList = new List<CallIn>();
        static List<LatestRollingWeekSchedule_New> lrwsList = new List<LatestRollingWeekSchedule_New>();
        static List<ChatMessage> chatList = new List<ChatMessage>();
        static List<UseLog> useLogList = new List<UseLog>();
        static string apiPathSchedule = "http://www.api.sorrytokeepyouwaiting.com/api/schedule/";//"https://localhost:7017/api/schedule/";
        static string apiPathEmployee = "http://www.api.sorrytokeepyouwaiting.com/api/employees/";//"https://localhost:7017/api/employees/";
        static string apiPathEmployeeInCompany = "http://www.api.sorrytokeepyouwaiting.com/api/EmployeesInCompany/";
        static string apiPathlogin = "http://www.api.sorrytokeepyouwaiting.com/api/Login/";//"https://localhost:7017/api/Login/";
        static string apiPathloginInCompany = "http://www.api.sorrytokeepyouwaiting.com/api/LoginInCompany/";
        static string apiPathRollingWeek = "http://www.api.sorrytokeepyouwaiting.com/api/updRollingWeek/";//"https://localhost:7017/api/updRollingWeek";
        static string apiPathCallIn = "http://www.api.sorrytokeepyouwaiting.com/api/CallIn/";//"https://localhost:7017/api/CallIn/";
        static string apiPathChatMessage = "http://www.api.sorrytokeepyouwaiting.com/api/ChatMessage/";//"https://localhost:7017/api/ChatMessage";
        static string apiPathChatMessageInCompany = "http://www.api.sorrytokeepyouwaiting.com/api/ChatMessageInCompany/";//"https://localhost:7017/api/ChatMessage";
        static string apiPathUseLog = "http://www.api.sorrytokeepyouwaiting.com/api/UseLog/";//"https://localhost:7017/api/UseLog/";
        EmployeeCurrentUsage user = new EmployeeCurrentUsage();
        UseLog useLogEntity = new UseLog();
        Stopwatch sw = new Stopwatch();
        vw_EmployeeLogin helpUser;
        public FormStkywControlPanelCallInV2(int var1, string var2, int var3, string var4, string var5)
        {
            Cursor.Current = Cursors.WaitCursor;
            userId = var1;
            userName = var2;
            companyID = var3;
            directions = var4;
            this.Text = "STKYW Call In + Alarm - " + userName;

            apiPathEmployeeInCompany = apiPathEmployeeInCompany + companyID.ToString();
            apiPathloginInCompany = apiPathloginInCompany + companyID.ToString();
            apiPathChatMessageInCompany = apiPathChatMessageInCompany + companyID.ToString();
            /*int userId = 0;
            string userName = "";
            int companyID = 0;
            */
            /*using (LoginForm frmLogin = new LoginForm())
            {
                if (frmLogin.ShowDialog() == DialogResult.OK)
                {
                    userId = frmLogin.UserID;
                    userName = frmLogin.UserName;
                    companyID = frmLogin.CompanyID;
                }
                else
                {
                    Application.Exit();
                }
            }*/

            string dayOfWeek = DateTime.Now.DayOfWeek.ToString();

            PrepareVariables(userId, user, useLogEntity);
            System.Threading.Thread.Sleep(2000);

            //Initialize user //test values:
            user.ID = userId; // 8;
            user.CompanyID = companyID; // 1006;
            user.DelayInMinutes = 0;
            user.Alarm = false;
            user.Away = true;
            user.CpUsed = "CallIn Alarm Client";
            user.LastActive = DateTime.Now;
            user.ModifiedDate = DateTime.Now;
            user.Directions = directions;

            InitializeComponent();
            Cursor.Current = Cursors.Default;
            //timerAutoDelay.Start();
            timerOthersAlert.Start();
            labelAlertOther.Dock = DockStyle.Fill;
            Properties.Settings.Default.settingInitialDelay = 0;
            sw = Stopwatch.StartNew();
            FillComboBoxChatList();

            comboBoxWadAlias.Items.Add("Alle");
            string[] wanArray = var5.Split(';');
            for (int i = 0; i < wanArray.Length; i++)
            {
                comboBoxWadAlias.Items.Add(wanArray[i]);
            }

            if (Properties.Settings.Default.settingWadAlias != "")
                comboBoxWadAlias.SelectedItem = Properties.Settings.Default.settingWadAlias;
            else
                comboBoxWadAlias.SelectedIndex = 0;

            if (comboBoxWadAlias.SelectedItem == null)
                comboBoxWadAlias.SelectedIndex = 0;

            UpdateWadLayout(user, comboBoxWadAlias.SelectedItem.ToString());

            chatOpenCloseStatus = "Closed";
            this.Size = new Size(250, 107);
            //buttonOpenCloseChat.Text = "Åbn Chat";

            buttonAssistComingAsap.Visible = false;
            buttonAssistNoTime.Visible = false;
            buttonAssistRightAway.Visible = false;
            Properties.Settings.Default.settingRequestAidFrom = 0;
            buttonAssistReminder.BackColor = Color.Orange;
            textBoxAssistReminder.BackColor = Color.Orange;
            sizeHeightBeforeAssist = this.Size.Height;
            sizeWidthBeforeAssist = this.Size.Width;

            timerOffScreen.Start();
            textBoxCallInNew.Focus();
        }
        static void UpdateWadLayout(EmployeeCurrentUsage user, string comboChoice)
        {
            string ecuLayout = comboChoice;
            if (ecuLayout == "Alle")
                ecuLayout = null;
            user.ChosenLayout = ecuLayout;
            string finalPath = apiPathEmployee + user.ID;
            UpdateEmployee(user, finalPath);
        }
        static async Task PrepareVariables(int employeeID, EmployeeCurrentUsage user, UseLog useLogEntity)
        {
            ecuList = await GetECUList(apiPathEmployeeInCompany);

            foreach (EmployeeCurrentUsage ecuItem in ecuList)
            {
                if (ecuItem.ID == employeeID)
                    user = ecuItem;
            }

            //useListID
            if (Properties.Settings.Default.settingUseLog == true)
            {
                IEnumerable<UseLog> query;
                query = useLogList.Where(s => s.CompanyID == user.CompanyID && s.UserID == user.ID && s.LogonTime >= DateTime.Today);
                query = query.OrderBy(s => s.LogonTime);
                foreach (UseLog useItem in query)
                {
                    if (useItem.UserID == employeeID)
                    {
                        useLogEntity.UseLogID = useItem.UseLogID;
                        useLogEntity.UserID = employeeID;
                        useLogEntity.CompanyID = useItem.CompanyID;
                        useLogEntity.LogonTime = useItem.LogonTime;
                    }
                }
            }
        }
        public static int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
        #region API kald metoder
        static async Task<List<ChatMessage>> GetChatMessagesList(string path)
        {
            var response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var stringData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ChatMessage>>(stringData);
            }
            return null;
        }
        static async void PostChatMessage(ChatMessage chatMessage, string path)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(path, chatMessage);
            response.EnsureSuccessStatusCode();
        }
        static async Task<List<vw_EmployeeLogin>> GetEmployeeList(string path)
        {
            var response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var stringData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<vw_EmployeeLogin>>(stringData);
            }
            return null;
        }
        static async Task<List<EmployeeCurrentUsage>> GetECUList(string path)
        {
            var response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var stringData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<EmployeeCurrentUsage>>(stringData);
            }
            return null;
        }
        static async Task<List<Schedule>> GetScheduleList(string path)
        {
            var response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var stringData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Schedule>>(stringData);
            }
            return null;
        }
        static async Task<List<CallIn>> GetCallInList(string path)
        {
            var response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var stringData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<CallIn>>(stringData);
            }
            return null;
        }
        static async void UpdateSchedule(Schedule schedule, string path)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                path,
                schedule);
            response.EnsureSuccessStatusCode();
        }
        static async void PostCallIn(CallIn callMessage, string path)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(path, callMessage);
            response.EnsureSuccessStatusCode();
        }
        static async void UpdateEmployee(EmployeeCurrentUsage employee, string path)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                path, employee);
            response.EnsureSuccessStatusCode();
        }
        static async Task<List<LatestRollingWeekSchedule_New>> GetLRWSList(string path)
        {
            var response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var stringData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<LatestRollingWeekSchedule_New>>(stringData);
            }
            return null;
        }
        static async void UpdateLatestRolling(LatestRollingWeekSchedule_New lrws, string path)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                path, lrws);
            response.EnsureSuccessStatusCode();
        }
        static async void UpdateUseLog(UseLog useLog, string path)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(
                    path, useLog);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                string my = ex.Message;
                my += "";
            }
        }
        #endregion
        #region Klasser
        /// <summary>
        /// Schedule og Employee klasser til brug i APIet
        /// </summary>
        public class Schedule
        {
            public int UserID { get; set; }
            public string WeekDay { get; set; }
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }
            public DateTime? CreatedDate { get; set; }
            public DateTime? ModifiedDate { get; set; }
            public bool Active { get; set; }
            public int ScheduleID { get; set; }
            public short? DayOfWeek { get; set; }
            public string TsType { get; set; }
            public int? RollingWeekNr { get; set; }
        }
        public class EmployeeCurrentUsage
        {
            public int ID { get; set; }
            public int? DelayInMinutes { get; set; }
            public bool? Alarm { get; set; }
            public bool? Away { get; set; }
            public string CpUsed { get; set; }
            public DateTime? LastActive { get; set; }
            public DateTime? ModifiedDate { get; set; }
            public int CompanyID { get; set; }
            public string Directions { get; set; }
            public string ChosenLayout { get; set; }
            public int? RequestedAidFrom { get; set; }
        }
        public class vw_EmployeeLogin
        {
            public string Name { get; set; }
            public int ID { get; set; }
            public string Username { get; set; }
            public int CompanyID { get; set; }
            public byte[] PasswordEncrypted { get; set; }
            public string wadAliasName { get; set; }
        }
        public class Company
        {
            public string Name { get; set; }
            public int ID { get; set; }
            public string Street { get; set; }
            public string Zip { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public Nullable<System.DateTime> CreatedDate { get; set; }
            public Nullable<System.DateTime> ModifiedDate { get; set; }
            public Nullable<System.DateTime> LastPaymentDate { get; set; }
            public byte[] PasswordEncrypted { get; set; }
            public Nullable<System.DateTime> PaymentValidTo { get; set; }
            public string CountryCode { get; set; }
            public Nullable<System.DateTime> LastActive { get; set; }
            public Nullable<bool> EmailConfirmed { get; set; }
            public string CompanyType { get; set; }
            public string CurrentPlan { get; set; }
            public string SubscriptionID { get; set; }
        }
        public class LatestRollingWeekSchedule_New
        {
            public int UserID { get; set; }
            public int WeekRollFromLatestActive { get; set; }
            public System.DateTime SysRowTmModified { get; set; }
        }
        public class CallIn
        {
            public int CallInID { get; set; }
            public int EmployeeID { get; set; }
            public int CompanyID { get; set; }
            public string Besked { get; set; }
            public DateTime SysRowCreated { get; set; }
        }
        public class ChatMessage
        {
            public int ChatMessageID { get; set; }
            public int CompanyID { get; set; }
            public int FromEmployeeID { get; set; }
            public int ToEmployeeID { get; set; }
            public string Text { get; set; }
            public DateTime SysRowCreated { get; set; }
        }
        #endregion
        private void FormStkywControlPanelCallInV2_FormClosing(object sender, FormClosingEventArgs e)
        {
            string apiPathUserFinal = apiPathEmployee + user.ID;
            user.LastActive = DateTime.Now;
            user.ModifiedDate = DateTime.Now;
            user.Away = true;
            UpdateEmployee(user, apiPathUserFinal);

            if (Properties.Settings.Default.settingUseLog == true)
            {
                string apiPathUseLogFinal = apiPathUseLog + useLogEntity.UseLogID.ToString();
                useLogEntity.LogoffTime = DateTime.Now;
                UpdateUseLog(useLogEntity, apiPathUseLogFinal);
            }

            System.Windows.Forms.Application.Exit(); Application.Exit();
        }
        private void buttonCallInText_Click(object sender, EventArgs e)
        {
            //Puts the called in client on the WAD. If nextTimeAuto is set, also calls buttonTimeslotNext.
            if (callInLastName != null)
            {
                if (callInLastName == textBoxCallInNew.Text && DateTime.Now.CompareTo(callInLastTime.AddMinutes(3)) < 1)
                {
                    textBoxCallInNew.Text = textBoxCallInNew.Text + " ";
                }
            }

            string apiPathUserFinal = apiPathEmployee + user.ID;
            user.LastActive = DateTime.Now;
            user.ModifiedDate = DateTime.Now;
            user.ChosenLayout = Properties.Settings.Default.settingWadAlias;
            UpdateEmployee(user, apiPathUserFinal);

            CallIn callMsg = new CallIn();
            callMsg.EmployeeID = userId;
            callMsg.CompanyID = companyID;
            callMsg.SysRowCreated = DateTime.Now;
            callMsg.Besked = textBoxCallInNew.Text;

            PostCallIn(callMsg, apiPathCallIn);
            callInLastName = textBoxCallInNew.Text;
            callInLastTime = DateTime.Now;

            textBoxCallInNew.Text = "";
        }
        private void textBoxCallInNew_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                buttonCallInText.PerformClick();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (timerOthersAlert.Enabled == false)
            {
                timerOthersAlert.Start();
            }
            else
            {
                timerOthersAlert.Stop();
            }
            timerFlashing.Start();

            if (user.Alarm == false)
            {
                user.Alarm = true;
                timerFlashing.Start();
            }
            else if (user.Alarm == true)
            {
                user.Alarm = false;
                timerFlashing.Stop();
                this.BackColor = SystemColors.Control;
            }

            string apiPathUserFinal = apiPathEmployee + user.ID;
            user.LastActive = DateTime.Now;
            user.ModifiedDate = DateTime.Now;
            user.ChosenLayout = Properties.Settings.Default.settingLocation;
            UpdateEmployee(user, apiPathUserFinal);
            this.ActiveControl = null;
        }
        private void timerOthersAlert_Tick(object sender, EventArgs e)
        {
            performAlertCheck(companyID, labelAlertOther, user, timerFlashing, timerAssist);
            if (timerFlashing.Enabled == false && timerAssist.Enabled == false)
            {
                this.BackColor = ColorTranslator.FromHtml("#ffffff");//SystemColors.Control;
                                                                     //MyCurrentEdit2
                if (Settings.Default.settingUnreadMessage == false)
                {
                    this.Size = new Size(250, 107);
                }
            }
        }
        static async Task performAlertCheck(int companyID, System.Windows.Forms.Label label, EmployeeCurrentUsage user, System.Windows.Forms.Timer timer, System.Windows.Forms.Timer timerAssist)
        {
            allEmployees = await GetEmployeeList(apiPathloginInCompany);
            ecuList = await GetECUList(apiPathEmployeeInCompany);
            int foundAlertCollegue = 0;
            int foundHelpRequest = 0;

            IEnumerable<EmployeeCurrentUsage> ecuQuery = ecuList.Where(s => s.CompanyID == companyID && s.ID != user.ID);
            List<EmployeeCurrentUsage> OnlyCollegues = new List<EmployeeCurrentUsage>();
            foreach (EmployeeCurrentUsage qi in ecuQuery)
            {
                OnlyCollegues.Add(qi);
            }

            string alertLocation = "(ukendt)";
            foreach (EmployeeCurrentUsage queryItem in OnlyCollegues)
            {
                if (queryItem.Alarm == true)
                {
                    foundAlertCollegue = queryItem.ID;
                    if (queryItem.Directions != null)
                        alertLocation = queryItem.Directions.ToString();
                }
                else if (queryItem.RequestedAidFrom == user.ID)
                {
                    foundHelpRequest = queryItem.ID;
                    Properties.Settings.Default.settingRequestAidFrom = queryItem.ID;
                }
            }
            if (foundAlertCollegue > 0)
            {
                vw_EmployeeLogin alertUser = new vw_EmployeeLogin();
                for (int i = 0; i < allEmployees.Count; i++)
                {
                    if (allEmployees[i].ID == foundAlertCollegue)
                    {
                        alertUser = allEmployees[i];
                    }
                }
                timer.Start();
                label.Visible = true;
                label.Text = alertUser.Name + " har brug for hjælp!" + Environment.NewLine + "Gå til " + alertLocation;
                label.BringToFront();
            }
            else if (foundHelpRequest > 0)
            {
                timerAssist.Start();
            }
            else
            {
                timer.Stop();
                timerAssist.Stop();
                label.Visible = false;
                label.Text = "";
                label.SendToBack();
            }
        }
        private void timerFlashing_Tick(object sender, EventArgs e)
        {
            if (this.BackColor == Color.Red)
            {
                this.BackColor = ColorTranslator.FromHtml("#ffffff");//SystemColors.Control;
            }
            else
            {
                this.BackColor = Color.Red;
                SystemSounds.Exclamation.Play();
            }
            if (this.Height < 250)
                this.Size = new Size(250, 250);
        }
        private void FormStkywControlPanelCallInV2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.Save();
        }
        private void comboBoxWadAlias_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateWadLayout(user, comboBoxWadAlias.SelectedItem.ToString());
            Properties.Settings.Default.settingWadAlias = comboBoxWadAlias.SelectedItem.ToString();
        }
        private void buttonOpenCloseChat_Click(object sender, EventArgs e)
        {
            if (chatOpenCloseStatus == "Closed")
            {
                Settings.Default.settingUnreadMessage = true;
                //Stuff that happens when the chat window opens
                this.Size = new Size(250, 308);
                panelChat.Visible = true;
                //if (buttonOpenCloseChat.Text == "Åbn Chat")
                //FillComboBoxChatList();

                timerRefreshMessage.Enabled = true;
                SetTimerRefreshMessageInterval(100);
                //timerRefreshMessageWhileClosed.Enabled = false;
                SetTimerRefreshMessageWhileClosedInterval(60000);
                buttonOpenCloseChat.BackColor = SystemColors.Control;
                //buttonOpenCloseChat.Text = "Luk Chat";

                chatOpenCloseStatus = "Open";

                timerAssistFlashing.Stop();
                textBoxMainChatWindow.SelectionStart = textBoxMainChatWindow.TextLength;
                textBoxMainChatWindow.ScrollToCaret();

                menuStrip1.Visible = false;
            }
            else
            {
                Settings.Default.settingUnreadMessage = false;
                //Stuff that happens when the chat window closes
                this.Size = new Size(250, 107);
                panelChat.Visible = false;
                timerRefreshMessage.Enabled = false;
                timerRefreshMessageWhileClosed.Enabled = true;
                SetTimerRefreshMessageWhileClosedInterval(15000);
                buttonOpenCloseChat.BackColor = SystemColors.Control;
                //buttonOpenCloseChat.Text = "Åbn Chat";

                comboBoxChatUsersList.SelectedIndex = 0;

                chatOpenCloseStatus = "Closed";
                Properties.Settings.Default.settingLastCheckForMessages = System.DateTime.Now;
                
                menuStrip1.Visible = true;
            }
        }
        private void buttonSendChat_Click(object sender, EventArgs e)
        {
            textBoxMainChatWindow.Text += Environment.NewLine + userName + " (" + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + "): " + textBoxWriteChatMessage.Text;
            textBoxMainChatWindow.SelectionStart = textBoxMainChatWindow.TextLength;
            textBoxMainChatWindow.ScrollToCaret();

            string recipient = comboBoxChatUsersList.SelectedItem.ToString();
            int listID = comboBoxChatUsersList.SelectedIndex;
            //int startNo = elComboBoxChatUsersList[listID - 1]. .IndexOf("(");
            //int endNo = elComboBoxChatUsersList[listID - 1].ToString().IndexOf(")") - 1;
            //int length = endNo - startNo;
            string recipientID = "-1";
            if (recipient != "Alle")
            {
                if (elComboBoxChatUsersList[listID - 1].Name == recipient)
                    recipientID = elComboBoxChatUsersList[listID - 1].ID.ToString();
            }

            ChatMessage chatMessage = new ChatMessage();
            chatMessage.CompanyID = companyID;
            chatMessage.FromEmployeeID = userId;
            chatMessage.ToEmployeeID = Convert.ToInt32(recipientID);
            chatMessage.Text = textBoxWriteChatMessage.Text;
            chatMessage.SysRowCreated = DateTime.Now;

            PostChatMessage(chatMessage, apiPathChatMessage);
            //ReadChat(Convert.ToInt32(recipientID), user);

            textBoxWriteChatMessage.Text = "";
            Properties.Settings.Default.settingLastCheckForMessages = System.DateTime.Now;
        }
        private void textBoxWriteChatMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                buttonSendChat_Click(sender, e);
        }
        private async Task ReadChat(int chatRecipient, EmployeeCurrentUsage userTemp)
        {
            //Reads the chat every 4000 miliseconds.
            //Filters chat to only include those coming from you, to you or to everyone.
            chatList = await GetChatMessagesList(apiPathChatMessageInCompany);
            IEnumerable<ChatMessage> query;
            IEnumerable<ChatMessage> query2;
            IEnumerable<ChatMessage> filter;
            if (chatRecipient > -1)
            {
                query = chatList.Where(s => s.CompanyID == companyID && s.ToEmployeeID == chatRecipient && s.FromEmployeeID == userId);
                query2 = chatList.Where(s => s.CompanyID == companyID && s.FromEmployeeID == chatRecipient && s.ToEmployeeID == userId);
                filter = query.Union(query2);
            }
            else
            {
                query = chatList.Where(s => s.CompanyID == companyID && s.ToEmployeeID == chatRecipient);
                filter = query;
            }
            List<ChatMessage> correctMessages = new List<ChatMessage>();
            foreach (ChatMessage queryItem in filter)
            {
                correctMessages.Add(queryItem);
            }
            List<ChatMessage> orderedList = correctMessages.OrderByDescending(x => x.SysRowCreated).ToList();
            List<ChatMessage> latestTwentyChats = new List<ChatMessage>();
            //Show only the latest 20 chats in a channel
            for (int i = 19; i >= 0; i--)
            {
                if (orderedList.Count - 1 >= i)
                    latestTwentyChats.Add(orderedList[i]);
            }

            string[] repArr = new string[comboBoxChatUsersList.Items.Count];
            for (int i = 0; i < repArr.Length; i++)
            {
                repArr[i] = comboBoxChatUsersList.Items[i].ToString();
            }

            //Create the content for the chat window
            string chatBoxStringBuilder = "";
            foreach (ChatMessage ch in latestTwentyChats)
            {
                string sender = ch.FromEmployeeID.ToString();

                foreach (string chUser in repArr)
                {
                    if (sender == userId.ToString())
                    {
                        sender = userName;
                    }
                    else
                    {
                        foreach (vw_EmployeeLogin vel in elComboBoxChatUsersList)
                        {
                            if (vel.ID.ToString() == sender)
                            {
                                sender = vel.Name;
                                break;
                            }
                        }
                    }
                }
                if (ch.Text == "Jeg har ikke tid lige nu." || ch.Text == "Jeg kommer snarest muligt." || ch.Text == "Jeg er på vej." || ch.Text == "Tilkaldet er ikke besvaret.")
                {
                    userTemp.RequestedAidFrom = 0;
                    Properties.Settings.Default.settingRequestAidFrom = 0;
                }
                string whenHour = ch.SysRowCreated.Hour.ToString();
                string whenMinute = ch.SysRowCreated.Minute.ToString();
                if (whenHour.Length == 1)
                    whenHour = "0" + whenHour;
                if (whenMinute.Length == 1)
                    whenMinute = "0" + whenMinute;
                chatBoxStringBuilder += sender + " (" + whenHour + ":" + whenMinute + "): " + ch.Text + Environment.NewLine;
            }
            textBoxMainChatWindow.Text = "";
            textBoxMainChatWindow.Text = chatBoxStringBuilder;
            textBoxMainChatWindow.SelectionStart = textBoxMainChatWindow.TextLength;
            textBoxMainChatWindow.ScrollToCaret();
        }
        private async Task ReadChatWhileClosed()
        {
            //Read the chat-channels while the chat is closed or on others channel.
            chatList = await GetChatMessagesList(apiPathChatMessageInCompany);
            string recipient = comboBoxChatUsersList.SelectedItem.ToString();
            int listID = comboBoxChatUsersList.SelectedIndex;
            string recipientID = "-1";
            int allUsers = -1;
            bool otherChatThread = false;
            bool alleChatThread = false;
            IEnumerable<ChatMessage> query = chatList.Where(s => s.CompanyID == companyID && s.ToEmployeeID == userId && s.SysRowCreated > Properties.Settings.Default.settingLastCheckForMessages);
            IEnumerable<ChatMessage> query2 = chatList.Where(s => s.CompanyID == companyID && s.ToEmployeeID == allUsers && s.SysRowCreated > Properties.Settings.Default.settingLastCheckForMessages);
            List<ChatMessage> correctMessages = new List<ChatMessage>();
            IEnumerable<ChatMessage> filter = query.Union(query2);

            string latestMsg = "";
            bool onlyFromMyself = true;

            foreach (ChatMessage queryItem in filter)
            {
                correctMessages.Add(queryItem);

                if (latestMsg == "")
                {
                    latestMsg = queryItem.Text;
                    if (queryItem.FromEmployeeID != user.ID)
                    {
                        onlyFromMyself = false;
                    }
                }
                /*Hvis vi står i alle-tråden og den nye besked IKKE er til alle*/
                if (recipient == "Alle" && queryItem.ToEmployeeID != -1)
                {
                    otherChatThread = true;
                }
                /*Ellers hvis vi står i alle-tråden og den nye besked er til alle*/
                else if (recipient == "Alle" && queryItem.ToEmployeeID == -1)
                {
                    alleChatThread = true;
                }
                /*Ellers hvis den nye besked er til alle, og vi ikke står i Alle-tråden*/
                else if (recipient != "Alle" && queryItem.ToEmployeeID == -1)
                {
                    alleChatThread = true;
                }
                /*Ellers hvis vi ikke står i alle-tråden*/
                else if (recipient != "Alle")
                {
                    /*Hvis afsender ikke er den samme som den tråd vi står står i*/
                    if (queryItem.FromEmployeeID != elComboBoxChatUsersList[listID - 1].ID)
                    {
                        otherChatThread = true;
                    }
                }
            }

            if (chatOpenCloseStatus == "Open")
            {
                if (correctMessages.Count > 0 && (otherChatThread == true || alleChatThread == true)
                    && latestMsg != "Jeg har ikke tid lige nu." && latestMsg != "Jeg kommer snarest muligt."
                    && latestMsg != "Jeg er på vej." && latestMsg != "Tilkaldet er ikke besvaret." && latestMsg.Contains("har anmodet om hjælp fra") == false
                    && onlyFromMyself == false)
                {
                    buttonOpenCloseChat.BackColor = Color.Orange;
                    //buttonOpenCloseChat.Text = "Ny besked";
                    timerAssistFlashing.Start();
                }
            }
            else if (chatOpenCloseStatus == "Closed")
            {
                if (correctMessages.Count > 0
                    && latestMsg != "Jeg har ikke tid lige nu." && latestMsg != "Jeg kommer snarest muligt."
                    && latestMsg != "Jeg er på vej." && latestMsg != "Tilkaldet er ikke besvaret." && latestMsg.Contains("har anmodet om hjælp fra") == false
                    && onlyFromMyself == false)
                {
                    buttonOpenCloseChat.BackColor = Color.Orange;
                    //buttonOpenCloseChat.Text = "Ny besked";
                    timerAssistFlashing.Start();
                }
            }
            List<ChatMessage> orderedList = correctMessages.OrderByDescending(x => x.SysRowCreated).ToList();
            List<ChatMessage> latestTwentyChats = new List<ChatMessage>();
            for (int i = 19; i >= 0; i--)
            {
                if (orderedList.Count - 1 >= i)
                    latestTwentyChats.Add(orderedList[i]);
            }

            string[] repArr = new string[comboBoxChatUsersList.Items.Count - 1];
            for (int i = 0; i < repArr.Length; i++)
            {
                repArr[i] = elComboBoxChatUsersList[i].Name;
            }
            //Puts ** around the channel that wrote
            foreach (ChatMessage ch in latestTwentyChats)
            {
                if (ch.FromEmployeeID != userId && alleChatThread == true)
                {
                    buttonOpenCloseChat.BackColor = Color.Orange;
                    timerAssistFlashing.Start();
                }
                else if (ch.FromEmployeeID != userId && listID == 0)
                {
                    string fromUser = "";

                    foreach (vw_EmployeeLogin el in elComboBoxChatUsersList)
                    {
                        if (el.ID == ch.FromEmployeeID)
                        {
                            fromUser = el.Name;
                            break;
                        }
                    }
                    for (int i = 0; i < comboBoxChatUsersList.Items.Count; i++)
                    {
                        if (comboBoxChatUsersList.Items[i].ToString().Contains(fromUser) == true && comboBoxChatUsersList.Items[i].ToString().Contains("**") == false)
                        {
                            comboBoxChatUsersList.Items[i] = "**" + comboBoxChatUsersList.Items[i].ToString() + "**";
                        }
                    }
                }
                else if (ch.FromEmployeeID != userId && elComboBoxChatUsersList[listID - 1].Name == recipient)
                {
                    string fromUser = "";

                    foreach (vw_EmployeeLogin el in elComboBoxChatUsersList)
                    {
                        if (el.ID == ch.FromEmployeeID)
                        {
                            fromUser = el.Name;
                            break;
                        }
                    }
                    for (int i = 0; i < comboBoxChatUsersList.Items.Count; i++)
                    {
                        if (comboBoxChatUsersList.Items[i].ToString().Contains(fromUser) == true && comboBoxChatUsersList.Items[i].ToString().Contains("**") == false)
                        {
                            comboBoxChatUsersList.Items[i] = "**" + comboBoxChatUsersList.Items[i].ToString() + "**";
                        }
                    }
                }
            }

            //Build the chat window
            string chatBoxStringBuilder = "";
            int ltcCounter = 0;
            string cnOverlayText = "";
            foreach (ChatMessage ch in latestTwentyChats)
            {
                cnOverlayText = ch.Text;
                ltcCounter = ltcCounter + 1;
                string sender = ch.FromEmployeeID.ToString();
                bool privat = false;

                foreach (string chUser in repArr)
                {
                    if (ch.ToEmployeeID == userId)
                    {
                        privat = true;
                    }
                }
                if (privat == false)
                {
                }
                else if (latestMsg != "Jeg har ikke tid lige nu." && latestMsg != "Jeg kommer snarest muligt."
                      && latestMsg != "Jeg er på vej." && latestMsg != "Tilkaldet er ikke besvaret." && latestMsg.Contains("har anmodet om hjælp fra") == false)
                {
                    string fromUser = "";

                    foreach (vw_EmployeeLogin el in elComboBoxChatUsersList)
                    {
                        if (el.ID == ch.FromEmployeeID)
                        {
                            fromUser = el.Name;
                            break;
                        }
                    }
                    chatBoxStringBuilder += "Ny besked fra " + fromUser + "." + Environment.NewLine;//, husk at vælge chatkanalen før du svarer: " + ch.Text + Environment.NewLine;
                    if (chatOpenCloseStatus == "Closed")
                        SetTimerRefreshMessageInterval(60000);
                    SetTimerRefreshMessageWhileClosedInterval(60000);

                    //New chat overlay while closed
                    if (chatOpenCloseStatus == "Closed" && ltcCounter == latestTwentyChats.Count)
                    {
                        this.Size = new System.Drawing.Size(248, 165);
                        labelCNSender.Text = "Ny besked fra " + fromUser + ".";
                        textBoxCNMessage.Text = cnOverlayText;
                        panelCNRecipientID = ch.FromEmployeeID;

                        Settings.Default.settingUnreadMessage = true;

                        labelCNShowHideMessage.Text = "Vis besked";
                        textBoxCNMessage.Visible = false;
                        panelChatNotification.Visible = true;
                        panelChatNotification.BringToFront();
                    }
                }
            }
            if (chatBoxStringBuilder.Length > 0)
            {
                textBoxMainChatWindow.Text = "";
                textBoxMainChatWindow.Text = chatBoxStringBuilder;
            }

            Properties.Settings.Default.settingLastCheckForMessages = System.DateTime.Now;
        }
        private void comboBoxChatUsersList_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxMainChatWindow.Text = "";
            comboBoxChatUsersList.SelectedIndexChanged -= comboBoxChatUsersList_SelectedIndexChanged;
            string recipient = comboBoxChatUsersList.SelectedItem.ToString();
            int listID = comboBoxChatUsersList.SelectedIndex;
            //int startNo = elComboBoxChatUsersList[listID - 1]. .IndexOf("(");
            //int endNo = elComboBoxChatUsersList[listID - 1].ToString().IndexOf(")") - 1;
            //int length = endNo - startNo;
            string recipientID = "-1";
            comboBoxChatUsersList.Items[comboBoxChatUsersList.FindStringExact(recipient)] = recipient.Replace("*", "");
            recipient = recipient.Replace("*", "");
            if (recipient != "Alle")
            {
                buttonHelp.Enabled = true;
                if (elComboBoxChatUsersList[listID - 1].Name == recipient)
                    recipientID = elComboBoxChatUsersList[listID - 1].ID.ToString();
                //recipientID = elComboBoxChatUsersList[listID-1].ToString().Substring(elComboBoxChatUsersList[listID-1].ToString().IndexOf("(") + 1, length);
                //recipientID = recipient.Substring(recipient.IndexOf("(") + 1, length);
            }
            else
            {
                buttonHelp.Enabled = false;
            }

            bool check = true;
            buttonOpenCloseChat.BackColor = SystemColors.Control;
            if (chatOpenCloseStatus == "Open")
            {
                for (int i = 0; i < comboBoxChatUsersList.Items.Count; i++)
                {
                    if (comboBoxChatUsersList.Items[i].ToString().Contains("**"))
                    {
                        check = false;
                    }
                }
            }
            if (chatOpenCloseStatus == "Open" && check == true)
            {
                buttonOpenCloseChat.Text = "Udvid";
                timerAssistFlashing.Stop();
            }
            comboBoxChatUsersList.SelectedIndexChanged += comboBoxChatUsersList_SelectedIndexChanged;
            SetTimerRefreshMessageInterval(4000);
            SetTimerRefreshMessageWhileClosedInterval(15000);
            ReadChat(Convert.ToInt32(recipientID), user);
        }
        private void timerRefreshMessage_Tick(object sender, EventArgs e)
        {
            if (timerRefreshMessage.Interval != 4000)
                SetTimerRefreshMessageInterval(4000);

            string recipient = comboBoxChatUsersList.SelectedItem.ToString();
            int listID = comboBoxChatUsersList.SelectedIndex;
            //int startNo = recipient.IndexOf("(");
            //int endNo = recipient.IndexOf(")") - 1;
            //int length = endNo - startNo;
            string recipientID = "-1";
            if (recipient != "Alle")
            {
                if (elComboBoxChatUsersList[listID - 1].Name == recipient)
                    recipientID = elComboBoxChatUsersList[listID - 1].ID.ToString();

            }

            ReadChat(Convert.ToInt32(recipientID), user);
        }
        private void timerRefreshMessageWhileClosed_Tick(object sender, EventArgs e)
        {
            if (timerRefreshMessageWhileClosed.Interval != 15000 && chatOpenCloseStatus == "Closed")
                SetTimerRefreshMessageWhileClosedInterval(15000);
            else if (timerRefreshMessageWhileClosed.Interval != 60000 && chatOpenCloseStatus != "Closed")
                SetTimerRefreshMessageWhileClosedInterval(60000);
            ReadChatWhileClosed();
        }
        private async void FillComboBoxChatList()
        {
            elComboBoxChatUsersList.Clear();
            comboBoxChatUsersList.Items.Clear();
            comboBoxChatUsersList.Items.Add("Alle");
            /*Consider adding only the customers logged in, like on the WAD.*/

            allEmployees = await GetEmployeeList(apiPathloginInCompany);
            IEnumerable<vw_EmployeeLogin> query = allEmployees.Where(s => s.CompanyID == companyID);
            foreach (vw_EmployeeLogin queryItem in query)
            {
                if (queryItem.ID != userId)
                {
                    comboBoxChatUsersList.Items.Add(queryItem.Name);
                    elComboBoxChatUsersList.Add(queryItem);
                }
                else
                {
                    nameOfUser = queryItem.Name;
                }
            }
            comboBoxChatUsersList.SelectedIndex = 0;
        }
        private void buttonHelp_Click(object sender, EventArgs e)
        {
            try
            {
                string recipient = comboBoxChatUsersList.SelectedItem.ToString();
                int listID = comboBoxChatUsersList.SelectedIndex;
                //int startNo = elComboBoxChatUsersList[listID - 1]. .IndexOf("(");
                //int endNo = elComboBoxChatUsersList[listID - 1].ToString().IndexOf(")") - 1;
                //int length = endNo - startNo;
                string recipientID = "-1";
                if (recipient != "Alle")
                {
                    if (elComboBoxChatUsersList[listID - 1].Name == recipient)
                        recipientID = elComboBoxChatUsersList[listID - 1].ID.ToString();
                }
                helpFromUser = Properties.Settings.Default.settingRequestAidFrom.ToString();
                if (comboBoxChatUsersList.SelectedItem.ToString() != "Alle" && (helpFromUser == "" || helpFromUser == "0"))
                {
                    string apiPathUserFinal = apiPathEmployee + user.ID;
                    user.RequestedAidFrom = Convert.ToInt32(recipientID);
                    user.ModifiedDate = DateTime.Now;
                    UpdateEmployee(user, apiPathUserFinal);
                    helpFromUser = recipientID;

                    ChatMessage chatMessage = new ChatMessage();
                    chatMessage.CompanyID = companyID;
                    chatMessage.FromEmployeeID = userId;
                    chatMessage.ToEmployeeID = Convert.ToInt32(recipientID); ;
                    chatMessage.Text = nameOfUser + " har anmodet om hjælp fra " + recipient + ".";
                    chatMessage.SysRowCreated = DateTime.Now;

                    PostChatMessage(chatMessage, apiPathChatMessage);
                    ReadChat(Convert.ToInt32(recipientID), user);
                }
                //else if (comboBoxChatUsersList.SelectedItem.ToString() != "Alle" && helpFromUser != "")
                //{
                //    string apiPathUserFinal = apiPathEmployee + user.ID;
                //    user.RequestedAidFrom = 0;
                //    user.ModifiedDate = DateTime.Now;
                //    UpdateEmployee(user, apiPathUserFinal);
                //    helpFromUser = "";
                //}
                else
                {
                    helpFromUser = "";
                    Properties.Settings.Default.settingRequestAidFrom = 0;
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                err += "";
            }
        }
        private void timerAssist_Tick(object sender, EventArgs e)
        {
            //Dictates whether another user has requested help

            //If the stopwatch is not running, we reset it and start it over.
            if (sw.IsRunning == false)
            {
                sw.Reset();
                sw.Start();
                sizeHeightBeforeAssist = this.Size.Height;
                sizeWidthBeforeAssist = this.Size.Width;
            }
            //If the stopwatch has run for less than a minute, then we look into this.
            if (sw.ElapsedMilliseconds < 60000)
            {
                //If the green-flashing hasn't started yet, then we look into this.
                if (timerHelpFlash.Enabled == false)
                {
                    //Redundant as we can now sort this in the API, but I'm scared of breaking my code.
                    int foundHelpRequest = 0;
                    IEnumerable<vw_EmployeeLogin> query = allEmployees.Where(s => s.CompanyID == companyID);
                    List<vw_EmployeeLogin> CompanyEmployees = new List<vw_EmployeeLogin>();
                    foreach (vw_EmployeeLogin queryItem in query)
                    {
                        CompanyEmployees.Add(queryItem);
                    }
                    IEnumerable<EmployeeCurrentUsage> ecuQuery = ecuList.Where(s => s.CompanyID == companyID && s.ID != user.ID);
                    List<EmployeeCurrentUsage> OnlyCollegues = new List<EmployeeCurrentUsage>();
                    foreach (EmployeeCurrentUsage qi in ecuQuery)
                    {
                        OnlyCollegues.Add(qi);
                    }
                    foreach (EmployeeCurrentUsage queryItem in OnlyCollegues)
                    {
                        if (queryItem.RequestedAidFrom == user.ID)
                        {
                            foundHelpRequest = queryItem.ID;
                        }
                    }
                    //Find who asked for help.
                    helpUser = new vw_EmployeeLogin();
                    for (int i = 0; i < CompanyEmployees.Count; i++)
                    {
                        if (CompanyEmployees[i].ID == foundHelpRequest)
                        {
                            helpUser = CompanyEmployees[i];
                        }
                    }

                    //Now we know who asked for help, so show it and the buttons.
                    labelAlertOther.BringToFront();
                    labelAlertOther.Visible = true;
                    labelAlertOther.Text = helpUser.Name + " anmoder om din tilstedeværelse.";
                    labelAlertOther.TextAlign = ContentAlignment.TopCenter;

                    helpFromUser = helpUser.Name;

                    buttonAssistComingAsap.Visible = true;
                    buttonAssistNoTime.Visible = true;
                    buttonAssistRightAway.Visible = true;
                    buttonAssistComingAsap.BringToFront();
                    buttonAssistNoTime.BringToFront();
                    buttonAssistRightAway.BringToFront();

                    this.Size = new Size(250, 356);

                    buttonAssistComingAsap.Location = new Point(50, 134);
                    buttonAssistNoTime.Location = new Point(50, 104);
                    buttonAssistRightAway.Location = new Point(50, 164);

                    timerHelpFlash.Start();
                }
            }
            //If the stopwatch is running and has done so for 60 seconds or more
            else
            {
                //Revert the GUI to pre-helpRequest.
                this.Size = new Size(sizeWidthBeforeAssist, sizeHeightBeforeAssist);
                timerHelpFlash.Stop();
                this.BackColor = Color.White;
                labelAlertOther.SendToBack();
                labelAlertOther.Visible = false;
                buttonAssistComingAsap.Visible = false;
                buttonAssistNoTime.Visible = false;
                buttonAssistRightAway.Visible = false;

                //Stop stopwatch and timer
                sw.Stop();
                timerAssist.Stop();

                //Once again, find who had requested your aid
                EmployeeCurrentUsage ecu = new EmployeeCurrentUsage();
                IEnumerable<vw_EmployeeLogin> query = allEmployees.Where(s => s.CompanyID == companyID);
                List<vw_EmployeeLogin> CompanyEmployees = new List<vw_EmployeeLogin>();
                foreach (vw_EmployeeLogin queryItem in query)
                {
                    CompanyEmployees.Add(queryItem);
                }
                IEnumerable<EmployeeCurrentUsage> ecuQuery = ecuList.Where(s => s.CompanyID == companyID && s.ID == Properties.Settings.Default.settingRequestAidFrom);
                List<EmployeeCurrentUsage> OnlyCollegues = new List<EmployeeCurrentUsage>();
                foreach (EmployeeCurrentUsage qi in ecuQuery)
                {
                    OnlyCollegues.Add(qi);
                }
                foreach (EmployeeCurrentUsage queryItem in OnlyCollegues)
                {
                    if (queryItem.RequestedAidFrom == user.ID)
                    {
                        ecu = queryItem;
                    }
                }

                string apiPathUserFinal = apiPathEmployee + Properties.Settings.Default.settingRequestAidFrom;

                //Remove the RequestedAid marking
                ecu.RequestedAidFrom = 0;
                ecu.ModifiedDate = DateTime.Now;
                UpdateEmployee(ecu, apiPathUserFinal);

                //Send a chat, saying the request have not been answered.
                ChatMessage chatMessage = new ChatMessage();
                chatMessage.CompanyID = companyID;
                chatMessage.FromEmployeeID = userId;
                chatMessage.ToEmployeeID = Properties.Settings.Default.settingRequestAidFrom;
                chatMessage.Text = "Tilkaldet er ikke besvaret.";
                chatMessage.SysRowCreated = DateTime.Now;

                PostChatMessage(chatMessage, apiPathChatMessage);
                ReadChat(Properties.Settings.Default.settingRequestAidFrom, user);
            }
        }
        private void buttonAssistNoTime_Click(object sender, EventArgs e)
        {
            if (timerOthersAlert.Enabled == false)
            {
                timerOthersAlert.Start();
            }
            timerAssist.Stop();
            this.BackColor = ColorTranslator.FromHtml("#ffffff");

            buttonCalls("Jeg har ikke tid lige nu.");

            labelAlertOther.Visible = false;
            buttonAssistComingAsap.Visible = false;
            buttonAssistNoTime.Visible = false;
            buttonAssistRightAway.Visible = false;
            timerOthersAlert.Start();

            this.Size = new Size(sizeWidthBeforeAssist, sizeHeightBeforeAssist);
        }
        private void buttonAssistComingAsap_Click(object sender, EventArgs e)
        {
            if (timerOthersAlert.Enabled == false)
            {
                timerOthersAlert.Start();
            }
            timerAssist.Stop();
            this.BackColor = ColorTranslator.FromHtml("#ffffff"); ChatMessage cm = new ChatMessage();

            buttonCalls("Jeg kommer snarest muligt.");

            labelAlertOther.Visible = false;
            buttonAssistComingAsap.Visible = false;
            buttonAssistNoTime.Visible = false;
            buttonAssistRightAway.Visible = false;
            timerOthersAlert.Start();

            buttonAssistReminder.Visible = true;
            textBoxAssistReminder.Visible = true;
            buttonAssistReminder.Location = new Point(84, 41);
            textBoxAssistReminder.Location = new Point(84, 13);
            buttonAssistReminder.Size = new Size(170, 22);
            textBoxAssistReminder.Size = new Size(170, 22);

            textBoxAssistReminder.Text = "Husk at gå til " + helpFromUser;
            buttonAssistReminder.Text = "OK";

            this.Size = new Size(sizeWidthBeforeAssist, sizeHeightBeforeAssist);
        }
        private void buttonCalls(string messageTextFromBtn)
        {
            sw.Stop();
            timerHelpFlash.Stop();
            int foundHelpRequest = 0;
            EmployeeCurrentUsage ecu = new EmployeeCurrentUsage();
            IEnumerable<vw_EmployeeLogin> query = allEmployees.Where(s => s.CompanyID == companyID);
            List<vw_EmployeeLogin> CompanyEmployees = new List<vw_EmployeeLogin>();
            foreach (vw_EmployeeLogin queryItem in query)
            {
                CompanyEmployees.Add(queryItem);
            }
            IEnumerable<EmployeeCurrentUsage> ecuQuery = ecuList.Where(s => s.CompanyID == companyID && s.ID != user.ID);
            List<EmployeeCurrentUsage> OnlyCollegues = new List<EmployeeCurrentUsage>();
            foreach (EmployeeCurrentUsage qi in ecuQuery)
            {
                OnlyCollegues.Add(qi);
            }
            foreach (EmployeeCurrentUsage queryItem in OnlyCollegues)
            {
                if (queryItem.RequestedAidFrom == user.ID)
                {
                    foundHelpRequest = queryItem.ID;
                    ecu = queryItem;
                }
            }
            helpUser = new vw_EmployeeLogin();
            for (int i = 0; i < CompanyEmployees.Count; i++)
            {
                if (CompanyEmployees[i].ID == foundHelpRequest)
                {
                    helpUser = CompanyEmployees[i];
                }
            }

            ChatMessage cm = new ChatMessage();
            cm.Text = messageTextFromBtn;
            cm.FromEmployeeID = userId;
            cm.ToEmployeeID = helpUser.ID;
            cm.CompanyID = user.CompanyID;
            cm.SysRowCreated = DateTime.Now;
            PostChatMessage(cm, apiPathChatMessage);

            string apiPathUserFinal = apiPathEmployee + helpUser.ID;

            ecu.RequestedAidFrom = 0;
            ecu.ModifiedDate = DateTime.Now;
            UpdateEmployee(ecu, apiPathUserFinal);

            labelAlertOther.Visible = false;
            buttonAssistComingAsap.Visible = false;
            buttonAssistNoTime.Visible = false;
            buttonAssistRightAway.Visible = false;
        }
        private void textBoxWriteChatMessage_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBoxWriteChatMessage.Text = "";
            }
        }
        private void buttonAssistReminder_Click(object sender, EventArgs e)
        {
            buttonAssistReminder.Visible = false;
            textBoxAssistReminder.Visible = false;
        }
        private void timerAssistFlashing_Tick(object sender, EventArgs e)
        {
            if (buttonOpenCloseChat.BackColor == SystemColors.Control)
                buttonOpenCloseChat.BackColor = Color.Orange;
            else
                buttonOpenCloseChat.BackColor = SystemColors.Control;
        }
        private void SetTimerRefreshMessageInterval(int interval)
        {
            timerRefreshMessage.Stop();
            timerRefreshMessage.Interval = interval;
            timerRefreshMessage.Start();
        }
        private void SetTimerRefreshMessageWhileClosedInterval(int interval)
        {
            timerRefreshMessageWhileClosed.Stop();
            timerRefreshMessageWhileClosed.Interval = interval;
            timerRefreshMessageWhileClosed.Start();
        }
        private void buttonAssistRightAway_Click(object sender, EventArgs e)
        {
            if (timerOthersAlert.Enabled == false)
            {
                timerOthersAlert.Start();
            }
            timerAssist.Stop();
            this.BackColor = ColorTranslator.FromHtml("#ffffff"); ChatMessage cm = new ChatMessage();

            buttonCalls("Jeg er på vej.");

            labelAlertOther.Visible = false;
            buttonAssistComingAsap.Visible = false;
            buttonAssistNoTime.Visible = false;
            buttonAssistRightAway.Visible = false;
            timerOthersAlert.Start();
            this.Size = new Size(sizeWidthBeforeAssist, sizeHeightBeforeAssist);
        }
        private void timerCheckForBreakEnd_Tick(object sender, EventArgs e)
        {

        }
        private void timerHelpFlash_Tick(object sender, EventArgs e)
        {
            if (this.BackColor == Color.LightGreen)
            {
                this.BackColor = Color.White;
            }
            else
            {
                this.BackColor = Color.LightGreen;
                SystemSounds.Exclamation.Play();
            }
        }

        private void labelCNShowHideMessage_Click(object sender, EventArgs e)
        {
            if (labelCNShowHideMessage.Text == "Vis besked")
            {
                textBoxCNMessage.Visible = true;
                labelCNShowHideMessage.Text = "Skjul besked";
                textBoxCNReply.Focus();
            }
            else if (labelCNShowHideMessage.Text == "Skjul besked")
            {
                textBoxCNMessage.Visible = false;
                labelCNShowHideMessage.Text = "Vis besked";
            }
        }

        private void textBoxCNReply_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBoxCNReply.Text = "";
            }
        }

        private void textBoxCNReply_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ChatMessage chatMessage = new ChatMessage();
                chatMessage.CompanyID = companyID;
                chatMessage.FromEmployeeID = userId;
                chatMessage.ToEmployeeID = Convert.ToInt32(panelCNRecipientID);
                chatMessage.Text = textBoxCNReply.Text;
                chatMessage.SysRowCreated = DateTime.Now;

                PostChatMessage(chatMessage, apiPathChatMessage);
                buttonCNDismiss_Click(sender, e);

                timerAssistFlashing.Stop();
                buttonOpenCloseChat.BackColor = System.Drawing.SystemColors.Control;
            }
        }

        private void buttonCNDismiss_Click(object sender, EventArgs e)
        {
            Settings.Default.settingUnreadMessage = false;
            panelChatNotification.Visible = false;
            panelChatNotification.SendToBack();
            this.Size = new Size(250, 107);
        }

        public bool IsOnScreen(Form form)
        {
            Screen[] screens = Screen.AllScreens;
            foreach (Screen screen in screens)
            {
                int width = screen.WorkingArea.Width;
                int workWidth = width - 10;
                Point formTopLeft = new Point(form.Left, form.Top);

                if (workWidth > formTopLeft.X)//(screen.WorkingArea.Contains(formTopLeft))
                {
                    return true;
                }
            }

            return false;
        }

        private void timerOffScreen_Tick(object sender, EventArgs e)
        {
            bool reposition = IsOnScreen(this);
            if (reposition == false)
            {
                this.Location = new Point(100, 100);
            }
            timerOffScreen.Stop();
            timerOffScreen.Enabled = false;
        }

        private void buttonCNClose_Click(object sender, EventArgs e)
        {
            timerAssistFlashing.Stop();
            buttonOpenCloseChat.BackColor = System.Drawing.SystemColors.Control;
            panelChatNotification.Visible = false;
            panelChatNotification.SendToBack();
        }
        private void tilføjTidsregistreringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegisterMissingWork rmwForm = new RegisterMissingWork(userId, companyID);
            rmwForm.Show();
        }

        private void seTidsregistreringerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowUseLog sulForm = new ShowUseLog(userId, companyID);
            sulForm.Show();
        }
    }
}

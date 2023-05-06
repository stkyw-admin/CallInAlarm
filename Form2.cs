using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static StkywControlPanelCallInAlarm.FormStkywControlPanelCallInV2;

namespace StkywControlPanelCallInAlarm
{
    public partial class FormStkywControlPanelCallInV2 : Form
    {
        public int timerOption;
        public string helpFromUser;
        public string chatOpenCloseStatus;
        public int userId;
        public string userName;
        public int companyID;
        public string directions;
        static HttpClient client = new HttpClient();
        static List<Schedule> allSchedules = new List<Schedule>();
        static List<Schedule> schedules = new List<Schedule>();
        static List<vw_EmployeeLogin> allEmployees = new List<vw_EmployeeLogin>();
        static List<vw_EmployeeLogin> elComboBoxChatUsersList = new List<vw_EmployeeLogin>();
        static List<EmployeeCurrentUsage> ecuList = new List<EmployeeCurrentUsage>();
        static List<CallIn> callList = new List<CallIn>();
        static List<LatestRollingWeekSchedule_New> lrwsList = new List<LatestRollingWeekSchedule_New>();
        static List<ChatMessage> chatList = new List<ChatMessage>();
        static string apiPathSchedule = "http://www.api.sorrytokeepyouwaiting.com/api/schedule/";//"https://localhost:7017/api/schedule/";
        static string apiPathEmployee = "http://www.api.sorrytokeepyouwaiting.com/api/employees/";//"https://localhost:7017/api/employees/";
        static string apiPathlogin = "http://www.api.sorrytokeepyouwaiting.com/api/Login/";//"https://localhost:7017/api/Login/";
        static string apiPathRollingWeek = "http://www.api.sorrytokeepyouwaiting.com/api/updRollingWeek/";//"https://localhost:7017/api/updRollingWeek";
        static string apiPathCallIn = "http://www.api.sorrytokeepyouwaiting.com/api/CallIn/";//"https://localhost:7017/api/CallIn/";
        static string apiPathChatMessage = "http://www.api.sorrytokeepyouwaiting.com/api/ChatMessage/";//"https://localhost:7017/api/ChatMessage";
        EmployeeCurrentUsage user = new EmployeeCurrentUsage();
        Stopwatch sw = new Stopwatch();
        vw_EmployeeLogin helpUser;
        public FormStkywControlPanelCallInV2(int var1, string var2, int var3, string var4, string var5)
        {
            Cursor.Current = Cursors.WaitCursor;
            userId = var1;
            userName = var2;
            companyID = var3;
            directions = var4;
            this.Text = "STKYW Control Panel - " + userName;
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

            PrepareVariables(userId, user);
            System.Threading.Thread.Sleep(2000);

            //Initialize user //test values:
            user.ID = userId; // 8;
            user.CompanyID = companyID; // 1006;
            user.DelayInMinutes = 0;
            user.Alarm = false;
            user.Away = false;
            user.CpUsed = "CallIn Alarm Client";
            user.LastActive = DateTime.Now;
            user.ModifiedDate = DateTime.Now;
            user.Directions = directions;

            InitializeComponent();
            Cursor.Current = Cursors.Default;
            timerAutoDelay.Start();
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

            UpdateWadLayout(user, comboBoxWadAlias.SelectedItem.ToString());

            chatOpenCloseStatus = "Closed";
            this.Size = new Size(250, 107);
            buttonOpenCloseChat.Text = "Åbn Chat";

            buttonAssistComingAsap.Visible = false;
            buttonAssistNoTime.Visible = false;
            Properties.Settings.Default.settingRequestAidFrom = 0;
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
        static async Task PrepareVariables(int employeeID, EmployeeCurrentUsage user)
        {
            ecuList = await GetECUList(apiPathEmployee);

            foreach (EmployeeCurrentUsage ecuItem in ecuList)
            {
                if (ecuItem.ID == employeeID)
                    user = ecuItem;
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
            Application.Exit();
        }
        private void buttonCallInText_Click(object sender, EventArgs e)
        {
            //int l = this.Left;
            //int h = this.Top;
            //var formPopup = new FormCallInMessage(userId, companyID, userName);
            //formPopup.StartPosition = FormStartPosition.CenterParent;
            //formPopup.ShowDialog(this);
            //gem data i db

            //callList = await GetCallInList(apiPathCallIn);

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
            }
        }
        static async Task performAlertCheck(int companyID, System.Windows.Forms.Label label, EmployeeCurrentUsage user, System.Windows.Forms.Timer timer, System.Windows.Forms.Timer timerAssist)
        {
            allEmployees = await GetEmployeeList(apiPathlogin);
            ecuList = await GetECUList(apiPathEmployee);
            int foundAlertCollegue = 0;
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
                if (queryItem.Alarm == true)
                {
                    foundAlertCollegue = queryItem.ID;
                }
                else if (queryItem.RequestedAidFrom == user.ID)
                {
                    foundHelpRequest = queryItem.ID;
                }
            }
            if (foundAlertCollegue > 0)
            {
                vw_EmployeeLogin alertUser = new vw_EmployeeLogin();
                for (int i = 0; i < CompanyEmployees.Count; i++)
                {
                    if (CompanyEmployees[i].ID == foundAlertCollegue)
                    {
                        alertUser = CompanyEmployees[i];
                    }
                }
                timer.Start();
                label.Visible = true;
                label.Text = alertUser.Name + " har brug for hjælp!";
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
            if (this.BackColor != ColorTranslator.FromHtml("#ffffff"))
                this.BackColor = ColorTranslator.FromHtml("#ffffff");//SystemColors.Control;
            else
                this.BackColor = Color.Red;
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
                //Stuff that happens when the chat window opens
                this.Size = new Size(250, 293);
                panelChat.Visible = true;
                if (buttonOpenCloseChat.Text == "Åbn Chat")
                    FillComboBoxChatList();

                timerRefreshMessage.Enabled = true;
                timerRefreshMessage.Interval = 7000;
                //timerRefreshMessageWhileClosed.Enabled = false;
                timerRefreshMessageWhileClosed.Interval = 60000;
                buttonOpenCloseChat.BackColor = SystemColors.Control;
                buttonOpenCloseChat.Text = "Luk Chat";

                chatOpenCloseStatus = "Open";
            }
            else
            {
                //Stuff that happens when the chat window closes
                this.Size = new Size(250, 107);
                panelChat.Visible = false;
                timerRefreshMessage.Enabled = false;
                timerRefreshMessageWhileClosed.Enabled = true;
                timerRefreshMessageWhileClosed.Interval = 15000;
                buttonOpenCloseChat.BackColor = SystemColors.Control;
                buttonOpenCloseChat.Text = "Åbn Chat";

                comboBoxChatUsersList.SelectedIndex = 0;

                chatOpenCloseStatus = "Closed";
                Properties.Settings.Default.settingLastCheckForMessages = System.DateTime.Now;
            }
        }

        private void buttonSendChat_Click(object sender, EventArgs e)
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

            ChatMessage chatMessage = new ChatMessage();
            chatMessage.CompanyID = companyID;
            chatMessage.FromEmployeeID = userId;
            chatMessage.ToEmployeeID = Convert.ToInt32(recipientID);
            chatMessage.Text = textBoxWriteChatMessage.Text;
            chatMessage.SysRowCreated = DateTime.Now;

            PostChatMessage(chatMessage, apiPathChatMessage);
            ReadChat(Convert.ToInt32(recipientID), user);

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
            chatList = await GetChatMessagesList(apiPathChatMessage);
            IEnumerable<ChatMessage> query;
            IEnumerable<ChatMessage> query2;
            if (chatRecipient > -1)
            {
                query = chatList.Where(s => s.CompanyID == companyID && s.ToEmployeeID == chatRecipient && s.FromEmployeeID == userId);
                query2 = chatList.Where(s => s.CompanyID == companyID && s.FromEmployeeID == chatRecipient && s.ToEmployeeID == userId);
            }
            else
            {
                query = chatList.Where(s => s.CompanyID == companyID && s.ToEmployeeID == chatRecipient);
                query2 = chatList.Where(s => s.CompanyID == companyID && s.ToEmployeeID == chatRecipient);
            }
            List<ChatMessage> correctMessages = new List<ChatMessage>();
            IEnumerable<ChatMessage> filter = query.Union(query2);
            foreach (ChatMessage queryItem in filter)
            {
                correctMessages.Add(queryItem);
            }
            List<ChatMessage> orderedList = correctMessages.OrderByDescending(x => x.SysRowCreated).ToList();
            List<ChatMessage> latestTwentyChats = new List<ChatMessage>();
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
                    //else if (chUser.IndexOf(sender) >= 0)
                    //{
                    //    sender = chUser.Substring(0, chUser.IndexOf("(") - 1);
                    //}
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
                if (ch.Text == "Jeg har ikke tid lige nu." || ch.Text == "Jeg kommer snarest muligt.")
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
            chatList = await GetChatMessagesList(apiPathChatMessage);
            string recipient = comboBoxChatUsersList.SelectedItem.ToString();
            int listID = comboBoxChatUsersList.SelectedIndex;
            string recipientID = "-1";
            int allUsers = -1;
            bool otherChatThread = false;
            IEnumerable<ChatMessage> query = chatList.Where(s => s.CompanyID == companyID && s.ToEmployeeID == userId && s.SysRowCreated >= Properties.Settings.Default.settingLastCheckForMessages);
            IEnumerable<ChatMessage> query2 = chatList.Where(s => s.CompanyID == companyID && s.ToEmployeeID == allUsers && s.SysRowCreated >= Properties.Settings.Default.settingLastCheckForMessages);
            List<ChatMessage> correctMessages = new List<ChatMessage>();
            IEnumerable<ChatMessage> filter = query.Union(query2);
            foreach (ChatMessage queryItem in filter)
            {
                correctMessages.Add(queryItem);

                if (recipient == "Alle")
                {
                    otherChatThread = true;
                }
                else if (queryItem.FromEmployeeID != elComboBoxChatUsersList[listID - 1].ID && recipient != "Alle")
                {
                    otherChatThread = true;
                }
            }

            if (correctMessages.Count > 0 && otherChatThread == true)
            {
                buttonOpenCloseChat.BackColor = Color.Orange;
                buttonOpenCloseChat.Text = "Ny besked";
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
                //repArr[i] = comboBoxChatUsersList.Items[i].ToString();
                repArr[i] = elComboBoxChatUsersList[i].Name;// + " (" + elComboBoxChatUsersList[i].ID + ")";
            }

            foreach (ChatMessage ch in latestTwentyChats)
            {
                if (ch.FromEmployeeID != userId && recipient == "Alle")
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

            string chatBoxStringBuilder = "";
            foreach (ChatMessage ch in latestTwentyChats)
            {
                string sender = ch.FromEmployeeID.ToString();
                bool privat = false;

                foreach (string chUser in repArr)
                {
                    if (chUser.IndexOf(sender) >= 0)
                    {
                        sender = chUser.Substring(0, chUser.IndexOf("("));
                    }
                    if (ch.ToEmployeeID == userId)
                    {
                        privat = true;
                    }
                }
                if (privat == false)
                {
                    //chatBoxStringBuilder += sender + ": " + ch.Text + Environment.NewLine;
                }
                else
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
                    chatBoxStringBuilder += "Ny besked fra " + fromUser + ".";//, husk at vælge chatkanalen før du svarer: " + ch.Text + Environment.NewLine;
                }
            }
            if (chatBoxStringBuilder.Length > 0)
            {
                textBoxMainChatWindow.Text = "";
                textBoxMainChatWindow.Text = chatBoxStringBuilder;
            }
            //timerRefreshMessage.Interval = 15000;
            //timerRefreshMessage.Start();
            Properties.Settings.Default.settingLastCheckForMessages = System.DateTime.Now;
        }
        private void comboBoxChatUsersList_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxChatUsersList.SelectedIndexChanged -= comboBoxChatUsersList_SelectedIndexChanged;
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
                //recipientID = elComboBoxChatUsersList[listID-1].ToString().Substring(elComboBoxChatUsersList[listID-1].ToString().IndexOf("(") + 1, length);
                //recipientID = recipient.Substring(recipient.IndexOf("(") + 1, length);
            }

            bool check = true;
            comboBoxChatUsersList.Items[comboBoxChatUsersList.FindStringExact(recipient)] = recipient.Replace("*", "");
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
                buttonOpenCloseChat.Text = "Luk Chat";
            comboBoxChatUsersList.SelectedIndexChanged += comboBoxChatUsersList_SelectedIndexChanged;
            ReadChat(Convert.ToInt32(recipientID), user);
        }
        private void timerRefreshMessage_Tick(object sender, EventArgs e)
        {
            if (timerRefreshMessage.Interval > 7000)
                timerRefreshMessage.Interval = 7000;

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
            ReadChatWhileClosed();
        }
        private async void FillComboBoxChatList()
        {
            elComboBoxChatUsersList.Clear();
            comboBoxChatUsersList.Items.Clear();
            comboBoxChatUsersList.Items.Add("Alle");
            /*Consider adding only the customers logged in, like on the WAD.*/

            allEmployees = await GetEmployeeList(apiPathlogin);
            IEnumerable<vw_EmployeeLogin> query = allEmployees.Where(s => s.CompanyID == companyID);
            foreach (vw_EmployeeLogin queryItem in query)
            {
                if (queryItem.ID != userId)
                {
                    comboBoxChatUsersList.Items.Add(queryItem.Name);
                    elComboBoxChatUsersList.Add(queryItem);
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
            helpUser = new vw_EmployeeLogin();
            for (int i = 0; i < CompanyEmployees.Count; i++)
            {
                if (CompanyEmployees[i].ID == foundHelpRequest)
                {
                    helpUser = CompanyEmployees[i];
                }
            }

            this.Size = new Size(250, 293);
            labelAlertOther.BringToFront();
            labelAlertOther.Visible = true;
            labelAlertOther.Text = helpUser.Name + " anmoder om din tilstedeværelse.";
            labelAlertOther.TextAlign = ContentAlignment.TopCenter;

            this.BackColor = Color.LightGreen;
            buttonAssistComingAsap.Visible = true;
            buttonAssistNoTime.Visible = true;
            buttonAssistComingAsap.BringToFront();
            buttonAssistNoTime.BringToFront();
            buttonAssistComingAsap.Location = new Point(50, 134);
            buttonAssistNoTime.Location = new Point(50, 104);
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
            timerOthersAlert.Start();
            if (chatOpenCloseStatus == "Closed")
                this.Size = new Size(250, 107);
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
            timerOthersAlert.Start();
            if (chatOpenCloseStatus == "Closed")
                this.Size = new Size(250, 107);
        }
        private void buttonCalls(string messageTextFromBtn)
        {
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
        }

        private void textBoxWriteChatMessage_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBoxWriteChatMessage.Text = "";
            }
        }
    }
}

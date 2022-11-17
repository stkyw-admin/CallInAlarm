using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

namespace StkywControlPanelCallInAlarm
{
    public partial class FormStkywControlPanelCallInAlarm : Form
    {
        public int userId;
        public string userName;
        public int companyID;
        static HttpClient client = new HttpClient();
        static List<Schedule> allSchedules = new List<Schedule>();
        static List<Schedule> schedules = new List<Schedule>();
        static List<vw_EmployeeLogin> allEmployees = new List<vw_EmployeeLogin>();
        static List<EmployeeCurrentUsage> ecuList = new List<EmployeeCurrentUsage>();
        static List<LatestRollingWeekSchedule_New> lrwsList = new List<LatestRollingWeekSchedule_New>();
        static string apiPathSchedule = "http://www.api.sorrytokeepyouwaiting.com/api/schedule/";//"https://localhost:7017/api/schedule/";
        static string apiPathEmployee = "http://www.api.sorrytokeepyouwaiting.com/api/employees/";//"https://localhost:7017/api/employees/";
        static string apiPathlogin = "http://www.api.sorrytokeepyouwaiting.com/api/Login/";//"https://localhost:7017/api/Login/";
        static string apiPathRollingWeek = "http://www.api.sorrytokeepyouwaiting.com/api/updRollingWeek/";//"https://localhost:7017/api/updRollingWeek";
        static string apiPathCallIn = "http://www.api.sorrytokeepyouwaiting.com/api/CallIn/";//"https://localhost:7017/api/CallIn/";
        EmployeeCurrentUsage user = new EmployeeCurrentUsage();
        Stopwatch sw = new Stopwatch();
        public FormStkywControlPanelCallInAlarm(int var1, string var2, int var3)
        {
            Cursor.Current = Cursors.WaitCursor;
            userId = var1;
            userName = var2;
            companyID = var3;
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

            PrepareVariables(userId, dayOfWeek, user);
            System.Threading.Thread.Sleep(2000);

            //Initialize user //test values:
            user.ID = userId; // 8;
            user.CompanyID = companyID; // 1006;
            user.DelayInMinutes = 0;
            user.Alarm = false;
            user.Away = false;
            user.CpUsed = "PC CallInAlarm";
            user.LastActive = DateTime.Now;
            user.ModifiedDate = DateTime.Now;

            InitializeComponent();
            Cursor.Current = Cursors.Default;
            button1.SendToBack();
            timerOthersAlert.Start();
            labelAlertOther.Dock = DockStyle.Fill;
            StkywControlPanelCallInAlarm.Properties.Settings.Default.settingInitialDelay = 0;
            sw = Stopwatch.StartNew();
        }
        static async Task PrepareVariables(int employeeID, string weekday, EmployeeCurrentUsage user)
        {
            allSchedules = await GetScheduleList(apiPathSchedule);
            ecuList = await GetECUList(apiPathEmployee);
            lrwsList = await GetLRWSList(apiPathRollingWeek);

            int lrwsListWeek = 0;
            int nextWeek = 0;
            DateTime lrwsListDate = new DateTime();

            foreach (LatestRollingWeekSchedule_New item in lrwsList)
            {
                if (item.UserID == employeeID)
                {
                    lrwsListWeek = item.WeekRollFromLatestActive;
                    lrwsListDate = item.SysRowTmModified;
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
        private void timerFlashing_Tick(object sender, EventArgs e)
        {
            if (this.BackColor == Color.Red)
                this.BackColor = SystemColors.Control;
            else
                this.BackColor = Color.Red;
        }
        private void timerOthersAlert_Tick(object sender, EventArgs e)
        {
            performAlertCheck(companyID, labelAlertOther, user, timerFlashing);
            if (timerFlashing.Enabled == false)
            {
                this.BackColor = SystemColors.Control;
            }
        }
        static async Task performAlertCheck(int companyID, Label label, EmployeeCurrentUsage user, Timer timer)
        {
            allEmployees = await GetEmployeeList(apiPathlogin);
            ecuList = await GetECUList(apiPathEmployee);
            int foundAlertCollegue = 0;

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
            }
            else
            {
                timer.Stop();
                label.Visible = false;
                label.Text = ""; 
            }
        }
        #region API kald metoder
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
        #region Knapper
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (timerOthersAlert.Enabled == false)
            {
                timerOthersAlert.Start();
            }
            this.BackColor = SystemColors.Control;

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
            UpdateEmployee(user, apiPathUserFinal);
            this.ActiveControl = null;
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
        }
        public class vw_EmployeeLogin
        {
            public string Name { get; set; }
            public int ID { get; set; }
            public string Username { get; set; }
            public int CompanyID { get; set; }
            public byte[] PasswordEncrypted { get; set; }
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
            public int EmployeeID { get; set; }
            public int CompanyID { get; set; }
            public string Besked { get; set; }
            public DateTime SysRowCreated { get; set; }
        }
        #endregion
        private void FormStkywControlPanelLightV2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1_Click(sender, e);
        }
        private void buttonCallInText_Click_1(object sender, EventArgs e)
        {
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
    }
}

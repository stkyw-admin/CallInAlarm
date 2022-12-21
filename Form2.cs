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
using static StkywControlPanelCallIn.FormStkywControlPanelCallInV2;

namespace StkywControlPanelCallIn
{
    public partial class FormStkywControlPanelCallInV2 : Form
    {
        public int userId;
        public string userName;
        public int companyID;
        public string directions;
        static HttpClient client = new HttpClient();
        static List<Schedule> allSchedules = new List<Schedule>();
        static List<Schedule> schedules = new List<Schedule>();
        static List<vw_EmployeeLogin> allEmployees = new List<vw_EmployeeLogin>();
        static List<EmployeeCurrentUsage> ecuList = new List<EmployeeCurrentUsage>();
        static List<CallIn> callList = new List<CallIn>();
        static List<LatestRollingWeekSchedule_New> lrwsList = new List<LatestRollingWeekSchedule_New>();
        static string apiPathSchedule = "http://www.api.sorrytokeepyouwaiting.com/api/schedule/";//"https://localhost:7017/api/schedule/";
        static string apiPathEmployee = "http://www.api.sorrytokeepyouwaiting.com/api/employees/";//"https://localhost:7017/api/employees/";
        static string apiPathlogin = "http://www.api.sorrytokeepyouwaiting.com/api/Login/";//"https://localhost:7017/api/Login/";
        static string apiPathRollingWeek = "http://www.api.sorrytokeepyouwaiting.com/api/updRollingWeek/";//"https://localhost:7017/api/updRollingWeek";
        static string apiPathCallIn = "http://www.api.sorrytokeepyouwaiting.com/api/CallIn/";//"https://localhost:7017/api/CallIn/";
        EmployeeCurrentUsage user = new EmployeeCurrentUsage();
        Stopwatch sw = new Stopwatch();
        public FormStkywControlPanelCallInV2(int var1, string var2, int var3, string var4)
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
            user.CpUsed = "CallIn Client";
            user.LastActive = DateTime.Now;
            user.ModifiedDate = DateTime.Now;
            user.Directions = directions;

            InitializeComponent();
            Cursor.Current = Cursors.Default;
            timerAutoDelay.Start();
            timerOthersAlert.Start();
            labelAlertOther.Dock = DockStyle.Fill;
            StkywControlPanelCallIn.Properties.Settings.Default.settingInitialDelay = 0;
            sw = Stopwatch.StartNew();

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
            user.ChosenLayout = Properties.Settings.Default.settingLocation;
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
    }
}

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

namespace StkywControlPanelLight
{
    public partial class FormStkywControlPanelLightV2 : Form
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
        EmployeeCurrentUsage user = new EmployeeCurrentUsage();
        Stopwatch sw = new Stopwatch();
        public FormStkywControlPanelLightV2(int var1, string var2, int var3)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.Icon = StkywControlPanelLight.Properties.Resources.icon;
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
            user.CpUsed = "PC Client";
            user.LastActive = DateTime.Now;
            user.ModifiedDate = DateTime.Now;

            InitializeComponent();
            Cursor.Current = Cursors.Default;
            button1.SendToBack();
            timerAutoDelay.Start();
            timerOthersAlert.Start();
            labelAlertOther.Dock = DockStyle.Fill;
            StkywControlPanelLight.Properties.Settings.Default.settingInitialDelay = 0;
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

            DateTime today = DateTime.Now;
            int isoWeekFromList = GetIso8601WeekOfYear(lrwsListDate);
            int isoWeekToday = GetIso8601WeekOfYear(today);

            if (isoWeekFromList < isoWeekToday) /*Hvis vi er gået ind i en højere uge, fx fra uge 4 til 5*/
            {
                nextWeek = lrwsListWeek + 1;
            }
            else if (isoWeekFromList == isoWeekToday) /*Hvis vi er i samme uge*/
            {
                nextWeek = lrwsListWeek;
            }
            else if (isoWeekFromList > isoWeekToday) /*Hvis vi er gået ind i en lavere uge, fx 52 til 1*/
            {
                nextWeek = lrwsListWeek + 1;
            }

            int maxWeekRoll = 0;
            foreach (Schedule item in allSchedules)
            {
                if (item.UserID == employeeID && item.RollingWeekNr > maxWeekRoll)
                {
                    maxWeekRoll = (int)item.RollingWeekNr;
                }
            }

            if (maxWeekRoll < nextWeek)
            {
                nextWeek = 1;
            }

            //Hvis man har skrevet et ugetal i login-formen, så overskriver den vores udregninger.
            if (StkywControlPanelLight.Properties.Settings.Default.settingLoginWeek > 0)
            {
                nextWeek = StkywControlPanelLight.Properties.Settings.Default.settingLoginWeek;
            }

            for (int i = 0; i < allSchedules.Count; i++)
            {
                if (allSchedules[i].UserID == employeeID && allSchedules[i].WeekDay == weekday 
                    && allSchedules[i].RollingWeekNr == nextWeek) 
                {
                    schedules.Add(allSchedules[i]);
                }
            }
            schedules = schedules.OrderBy(s => s.StartTime).ToList();

            //Nulstil aktive timeslots, så vi starter fra bunden af.
            foreach (Schedule queryItem in schedules)
            {
                if (queryItem.Active == true)
                {
                    string apiPathFinal = apiPathSchedule + queryItem.ScheduleID;
                    queryItem.Active = false;
                    UpdateSchedule(queryItem, apiPathFinal);
                }
            }
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
        public int CalculateDelay(Schedule schedule)
        {
            int delayInt;
            double delay;

            DateTime curDateTime = new DateTime();
            curDateTime = DateTime.Now;

            double hours = Convert.ToDouble(schedule.StartTime.ToString().Substring(0, 2));
            double minutes = Convert.ToDouble(schedule.StartTime.ToString().Substring(3, 2));
            DateTime tsSt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(hours), Convert.ToInt32(minutes), 0);
            
            if ((curDateTime.CompareTo(tsSt) <= 0) || (schedule.TsType == "Break"))
            {
                delay = 0;
            }
            else
            {
                System.TimeSpan diff1 = (curDateTime.Subtract(tsSt));
                delay = diff1.TotalMinutes;
            }
            delayInt = Convert.ToInt32(Math.Floor(delay));
            return delayInt;
        }
        public int CalculateDelayAutoIncrease(Schedule schedule)
        {
            int delayInt;
            double delay;

            DateTime curDateTime = new DateTime();
            curDateTime = DateTime.Now;

            double hours = Convert.ToDouble(schedule.EndTime.ToString().Substring(0, 2));
            double minutes = Convert.ToDouble(schedule.EndTime.ToString().Substring(3, 2));
            DateTime tsSt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(hours), Convert.ToInt32(minutes), 0);
            tsSt = tsSt.AddMinutes(StkywControlPanelLight.Properties.Settings.Default.settingInitialDelay);
            delay = StkywControlPanelLight.Properties.Settings.Default.settingInitialDelay;
            if (curDateTime.CompareTo(tsSt) > 0)
            {
                System.TimeSpan diff1 = (curDateTime.Subtract(tsSt));
                delay = StkywControlPanelLight.Properties.Settings.Default.settingInitialDelay + diff1.TotalMinutes; 
            }
            delayInt = Convert.ToInt32(Math.Floor(delay));
            return delayInt;
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
            if (sw.ElapsedMilliseconds < 120000)
            {
                timerAutoDelay.Enabled = true;
                timerAutoDelay.Start();
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
        static async void UpdateSchedule(Schedule schedule, string path)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                path,
                schedule);
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
        private void buttonAwayPresent_Click(object sender, EventArgs e)
        {
            if (timerOthersAlert.Enabled == false)
            {
                timerOthersAlert.Start();
            }
            this.BackColor = SystemColors.Control;

            comboBox1.SelectedIndex = 0;
            if (buttonAwayPresent.BackColor.Name == "PaleGreen")
            {
                buttonAwayPresent.BackColor = Color.DarkRed;
                buttonAwayPresent.ForeColor = Color.White;
                buttonAwayPresent.Text = "Væk";
                user.Away = true;
                if (timerAutoDelay.Enabled == true)
                {
                    timerAutoDelay.Stop();
                }
            }
            else if (buttonAwayPresent.BackColor.Name == "DarkRed")
            {
                buttonAwayPresent.BackColor = Color.PaleGreen;
                buttonAwayPresent.ForeColor = Color.Black;
                buttonAwayPresent.Text = "Her";
                user.Away = false;
                if (timerAutoDelay.Enabled == false)
                {
                    timerAutoDelay.Start();
                }
                //user.DelayInMinutes = 0;
            }
            string apiPathUserFinal = apiPathEmployee + user.ID;
            user.LastActive = DateTime.Now;
            user.ModifiedDate = DateTime.Now;
            UpdateEmployee(user, apiPathUserFinal);

            labelCurrentDelay.Text = user.DelayInMinutes.ToString();
            this.ActiveControl = null;
        }
        private void buttonTimeslotNext_Click(object sender, EventArgs e)
        {
            timerAutoDelay.Start();
            if (timerOthersAlert.Enabled == false)
            {
                timerOthersAlert.Start();
            }
            if (timerAutoDelay.Enabled == false)
            {
                timerAutoDelay.Start();
            }
            this.BackColor = SystemColors.Control;

            comboBox1.SelectedIndex = 0;
            int checkForLastTs = 0;

            for (int j = 0; j < schedules.Count; j++)
            {
                if (schedules[j].Active == true)
                {
                    checkForLastTs = j + 1;
                }
            }
            if (checkForLastTs < schedules.Count)
            {

                int currentTimeslot = 0;
                int nextTimeslot = 0;
                Schedule schedule = new Schedule();
                Schedule oldSchedule = new Schedule();

                IEnumerable<Schedule> query = schedules.Where(s => s.Active == true);
                List<Schedule> activeTimeslots = new List<Schedule>();
                foreach (Schedule queryItem in query)
                {
                    activeTimeslots.Add(queryItem);
                }
                if (activeTimeslots.Count == 0)
                {
                    //Ingen aktive timeslots, find det første
                    nextTimeslot = schedules[0].ScheduleID;
                    schedule = schedules[0];
                    string apiPathFinal = apiPathSchedule + nextTimeslot;
                    schedule.Active = true;
                    UpdateSchedule(schedule, apiPathFinal);
                }
                else if (activeTimeslots.Count == 1)
                {
                    //Der er ét aktivt timeslot, så find det næste på listen
                    for (int i = 0; i < schedules.Count; i++)
                    {
                        if (schedules[i].Active == true)
                        {
                            currentTimeslot = schedules[i].ScheduleID;
                            oldSchedule = schedules[i];
                            nextTimeslot = schedules[i + 1].ScheduleID;
                            schedule = schedules[i + 1];
                        }
                    }
                    oldSchedule.Active = false;
                    schedule.Active = true;
                    string apiPathFinal = apiPathSchedule + currentTimeslot;
                    UpdateSchedule(oldSchedule, apiPathFinal);
                    apiPathFinal = apiPathSchedule + nextTimeslot;
                    UpdateSchedule(schedule, apiPathFinal);
                }
                else if (activeTimeslots.Count >= 2)
                {
                    //Der er flere aktive timeslots, så find det næste på listen
                    int actTsCount = activeTimeslots.Count - 1;
                    if (actTsCount < 0)
                        actTsCount = 0;
                    int maxActiveTimeslot = activeTimeslots[actTsCount].ScheduleID;
                    for (int i = 0; i < activeTimeslots.Count; i++)
                    {
                        Schedule actSchedule = activeTimeslots[i];
                        actSchedule.Active = false;
                        string apiPathActSch = apiPathSchedule + actSchedule.ScheduleID;
                        UpdateSchedule(actSchedule, apiPathActSch);
                    }
                    for (int i = 0; i < schedules.Count; i++)
                    {
                        if (schedules[i].ScheduleID == maxActiveTimeslot)
                        {
                            nextTimeslot = schedules[i + 1].ScheduleID;
                            schedule = schedules[i + 1];
                        }
                    }
                    schedule.Active = true;
                    string apiPathFinal = apiPathSchedule + nextTimeslot;
                    UpdateSchedule(schedule, apiPathFinal);
                }
                else
                {
                    //Unhandled exception...
                }

                user.DelayInMinutes = CalculateDelay(schedule);
                string apiPathUserFinal = apiPathEmployee + user.ID;
                user.LastActive = DateTime.Now;
                user.ModifiedDate = DateTime.Now;
                UpdateEmployee(user, apiPathUserFinal);

                DisplayValues(schedule, user);
                StkywControlPanelLight.Properties.Settings.Default.settingInitialDelay = (int)user.DelayInMinutes;
            }
            this.ActiveControl = null;
        }
        private void buttonTimeslotPrevious_Click(object sender, EventArgs e)
        {
            timerAutoDelay.Start();
            if (timerOthersAlert.Enabled == false)
            {
                timerOthersAlert.Start();
            }
            if (timerAutoDelay.Enabled == false)
            {
                timerAutoDelay.Start();
            }
            this.BackColor = SystemColors.Control;

            comboBox1.SelectedIndex = 0;
            int checkForFirstTs = 0;

            for (int j = 0; j < schedules.Count; j++)
            {
                if (schedules[j].Active == true)
                {
                    checkForFirstTs = j;
                }
            }
            if (checkForFirstTs > 0)
            {
                int currentTimeslot = 0;
                int nextTimeslot = 0;
                Schedule schedule = new Schedule();
                Schedule oldSchedule = new Schedule();

                IEnumerable<Schedule> query = schedules.Where(s => s.Active == true);
                List<Schedule> activeTimeslots = new List<Schedule>();
                foreach (Schedule queryItem in query)
                {
                    activeTimeslots.Add(queryItem);
                }
                if (activeTimeslots.Count == 0)
                {
                    //Ingen aktive timeslots, find det første
                    nextTimeslot = schedules[0].ScheduleID;
                    schedule = schedules[0];
                    string apiPathFinal = apiPathSchedule + nextTimeslot;
                    schedule.Active = true;
                    UpdateSchedule(schedule, apiPathFinal);
                }
                else if (activeTimeslots.Count == 1)
                {
                    //Der er ét aktivt timeslot, så find det forrige på listen
                    for (int i = 0; i < schedules.Count; i++)
                    {
                        if (schedules[i].Active == true)
                        {
                            currentTimeslot = schedules[i].ScheduleID;
                            oldSchedule = schedules[i];
                            nextTimeslot = schedules[i - 1].ScheduleID;
                            schedule = schedules[i - 1];
                        }
                    }
                    oldSchedule.Active = false;
                    schedule.Active = true;
                    string apiPathFinal = apiPathSchedule + currentTimeslot;
                    UpdateSchedule(oldSchedule, apiPathFinal);
                    apiPathFinal = apiPathSchedule + nextTimeslot;
                    UpdateSchedule(schedule, apiPathFinal);
                }
                else if (activeTimeslots.Count >= 2)
                {
                    //Der er flere aktive timeslots, så find det forrige på listen
                    int minActiveTimeslot = activeTimeslots[0].ScheduleID;
                    for (int i = 0; i < activeTimeslots.Count; i++)
                    {
                        Schedule actSchedule = activeTimeslots[i];
                        actSchedule.Active = false;
                        string apiPathActSch = apiPathSchedule + actSchedule.ScheduleID;
                        UpdateSchedule(actSchedule, apiPathActSch);
                    }
                    for (int i = 0; i < schedules.Count; i++)
                    {
                        if (schedules[i].ScheduleID == minActiveTimeslot)
                        {
                            nextTimeslot = schedules[i - 1].ScheduleID;
                            schedule = schedules[i - 1];
                        }
                    }
                    schedule.Active = true;
                    string apiPathFinal = apiPathSchedule + nextTimeslot;
                    UpdateSchedule(schedule, apiPathFinal);
                }
                else
                {
                    //Unhandled exception...
                }

                user.DelayInMinutes = CalculateDelay(schedule);
                string apiPathUserFinal = apiPathEmployee + user.ID;
                user.LastActive = DateTime.Now;
                user.ModifiedDate = DateTime.Now;
                UpdateEmployee(user, apiPathUserFinal);

                DisplayValues(schedule, user);
                StkywControlPanelLight.Properties.Settings.Default.settingInitialDelay = (int)user.DelayInMinutes;
            }
            this.ActiveControl = null;
        }
        private void buttonTimeslotBestGuess_Click(object sender, EventArgs e)
        {
            timerAutoDelay.Start();
            if (timerOthersAlert.Enabled == false)
            {
                timerOthersAlert.Start();
            }
            if (timerAutoDelay.Enabled == false)
            {
                timerAutoDelay.Start();
            }
            this.BackColor = SystemColors.Control;

            comboBox1.SelectedIndex = 0;
            int nextTimeslot = 0;
            Schedule schedule = new Schedule();
            TimeSpan currentTime = new TimeSpan();
            currentTime = DateTime.Now.TimeOfDay;

            IEnumerable<Schedule> query = schedules.Where(s => s.Active == true);
            List<Schedule> activeTimeslots = new List<Schedule>();
            foreach (Schedule queryItem in query)
            {
                activeTimeslots.Add(queryItem);
            }

            //Tjek for aktive timeslots og gør dem ikke-aktive
            for (int i = 0; i < activeTimeslots.Count; i++)
            {
                Schedule actSchedule = activeTimeslots[i];
                actSchedule.Active = false;
                string apiPathActSch = apiPathSchedule + actSchedule.ScheduleID;
                UpdateSchedule(actSchedule, apiPathActSch);
            }

            for (int j = 0; j < schedules.Count; j++)
            {
                if (schedules[j].StartTime <= currentTime)
                {
                    if ((schedules[j].StartTime <= currentTime && schedules[j].EndTime > currentTime) || (j == schedules.Count - 1))
                    {
                        nextTimeslot = schedules[j].ScheduleID;
                        schedule = schedules[j];
                        break;
                    }
                }
                else if (schedules[j].StartTime > currentTime)
                {
                    nextTimeslot = schedules[0].ScheduleID;
                    schedule = schedules[0];
                    break;
                }
            }
            if (schedules.Count > 0)
            {
                schedule.Active = true;
                string apiPathFinal = apiPathSchedule + nextTimeslot;
                UpdateSchedule(schedule, apiPathFinal);

                user.DelayInMinutes = CalculateDelay(schedule);
                string apiPathUserFinal = apiPathEmployee + user.ID;
                user.LastActive = DateTime.Now;
                user.ModifiedDate = DateTime.Now;
                UpdateEmployee(user, apiPathUserFinal);

                DisplayValues(schedule, user);
                StkywControlPanelLight.Properties.Settings.Default.settingInitialDelay = (int)user.DelayInMinutes;
            }
            this.ActiveControl = null;
        }
        private void buttonTimeslotNextBreak_Click(object sender, EventArgs e)
        {
            timerAutoDelay.Start();
            if (timerOthersAlert.Enabled == false)
            {
                timerOthersAlert.Start();
            }
            if (timerAutoDelay.Enabled == false)
            {
                timerAutoDelay.Start();
            }
            this.BackColor = SystemColors.Control;

            comboBox1.SelectedIndex = 0;
            int nextTimeslot = 0;
            Schedule schedule = new Schedule();
            TimeSpan currentTime = new TimeSpan();
            currentTime = DateTime.Now.TimeOfDay;

            int lastCurrentSchedule = 0;
            IEnumerable<Schedule> query = schedules.Where(s => s.Active == true);
            List<Schedule> activeTimeslots = new List<Schedule>();
            foreach (Schedule queryItem in query)
            {
                activeTimeslots.Add(queryItem);
                lastCurrentSchedule = queryItem.ScheduleID;
            }

            bool foundABreak = false;
            int currentSlot = 0;
            for (int j = 0; j < schedules.Count; j++)
            {
                if (schedules[j].ScheduleID == lastCurrentSchedule)
                {
                    currentSlot = j;
                }
            }
            for (int k = currentSlot+1; k < schedules.Count; k++)
            {
                if (schedules[k].TsType == "Break")
                {
                    nextTimeslot = schedules[k].ScheduleID;
                    schedule = schedules[k];
                    foundABreak = true;
                    break;
                }
            }

            if (foundABreak == true)
            {
                //Tjek for aktive timeslots og gør dem ikke-aktive
                for (int i = 0; i < activeTimeslots.Count; i++)
                {
                    Schedule actSchedule = activeTimeslots[i];
                    string apiPathActSch = apiPathSchedule + actSchedule.ScheduleID;
                    actSchedule.Active = false;
                    UpdateSchedule(actSchedule, apiPathActSch);
                }

                schedule.Active = true;
                string apiPathFinal = apiPathSchedule + nextTimeslot;
                UpdateSchedule(schedule, apiPathFinal);

                user.DelayInMinutes = CalculateDelay(schedule);
                string apiPathUserFinal = apiPathEmployee + user.ID;
                user.LastActive = DateTime.Now;
                user.ModifiedDate = DateTime.Now;
                UpdateEmployee(user, apiPathUserFinal);

                DisplayValues(schedule, user);
                StkywControlPanelLight.Properties.Settings.Default.settingInitialDelay = (int)user.DelayInMinutes;
            }
            this.ActiveControl = null;
        }
        private void buttonTimeslotResetToZero_Click(object sender, EventArgs e)
        {
            if (timerOthersAlert.Enabled == false)
            {
                timerOthersAlert.Start();
            }
            if (timerAutoDelay.Enabled == true)
            {
                timerAutoDelay.Stop();
            }
            this.BackColor = SystemColors.Control;

            comboBox1.SelectedIndex = 0;
            user.DelayInMinutes = 0;
            string apiPathUserFinal = apiPathEmployee + user.ID;
            user.LastActive = DateTime.Now;
            user.ModifiedDate = DateTime.Now;
            UpdateEmployee(user, apiPathUserFinal);

            labelCurrentDelay.Text = user.DelayInMinutes.ToString();
            StkywControlPanelLight.Properties.Settings.Default.settingInitialDelay = (int)user.DelayInMinutes;
            this.ActiveControl = null;
        }
        private void buttonTimeslotMerge2_Click(object sender, EventArgs e)
        {
            mergeTimeslots(2);
        }
        private void buttonTimeslotMerge3_Click(object sender, EventArgs e)
        {
            mergeTimeslots(3);
        }
        private void buttonTimeslotMerge4_Click(object sender, EventArgs e)
        {
            mergeTimeslots(4);
        }
        private void buttonTimeslotMerge5_Click(object sender, EventArgs e)
        {
            mergeTimeslots(5);
        }
        private void mergeTimeslots(int amtToMerge)
        {
            if (timerOthersAlert.Enabled == false)
            {
                timerOthersAlert.Start();
            }
            if (timerAutoDelay.Enabled == false)
            {
                timerAutoDelay.Start();
            }
            this.BackColor = SystemColors.Control;

            comboBox1.SelectedIndex = 0;
            int checkForLastTs = 0;

            for (int j = 0; j < schedules.Count; j++)
            {
                if (schedules[j].Active == true)
                {
                    checkForLastTs = j + 1;
                }
            }
            if (checkForLastTs < schedules.Count)
            {
                int nextTimeslot = 0;
                Schedule schedule = new Schedule();
                TimeSpan currentTime = new TimeSpan();
                currentTime = DateTime.Now.TimeOfDay;

                IEnumerable<Schedule> query = schedules.Where(s => s.Active == true);
                List<Schedule> activeTimeslots = new List<Schedule>();
                foreach (Schedule queryItem in query)
                {
                    activeTimeslots.Add(queryItem);
                }

                //Tjek for aktive timeslots, så find det næste på listen
                int actTsCount = activeTimeslots.Count;
                if (actTsCount < 0)
                    actTsCount = 0;

                int maxActiveTimeslot = 0;

                if (actTsCount > 0)
                    maxActiveTimeslot = activeTimeslots[actTsCount - 1].ScheduleID;

                if (activeTimeslots.Count == 0)
                {
                    //Ingen aktive timeslots, start fra begyndelsen
                    for (int i = 0; i < amtToMerge; i++)
                    {
                        nextTimeslot = schedules[i].ScheduleID;
                        schedule = schedules[i];
                        string apiPathFinal = apiPathSchedule + nextTimeslot;
                        schedule.Active = true;
                        UpdateSchedule(schedule, apiPathFinal);
                    }
                }
                else
                {
                    for (int i = 0; i < activeTimeslots.Count; i++)
                    {
                        Schedule actSchedule = activeTimeslots[i];
                        actSchedule.Active = false;
                        string apiPathActSch = apiPathSchedule + actSchedule.ScheduleID;
                        UpdateSchedule(actSchedule, apiPathActSch);
                    }
                    for (int i = 0; i < schedules.Count; i++)
                    {
                        if (schedules[i].ScheduleID == maxActiveTimeslot)
                        {
                            int diff = schedules.Count - (i + 1);
                            if (amtToMerge > diff)
                                amtToMerge = diff;
                            for (int j = 1; j <= amtToMerge; j++)
                            {
                                nextTimeslot = schedules[i + j].ScheduleID;
                                schedule = schedules[i + j];
                                schedule.Active = true;
                                string apiPathActSch = apiPathSchedule + nextTimeslot;
                                UpdateSchedule(schedule, apiPathActSch);
                            }
                        }
                    }
                }
                //Loop igennem de aktive Ts'er igen og find første start og sidste slut tid.
                //Beregn forsinkelse baseret på den første
                Schedule firstSchedule = new Schedule();
                Schedule lastSchedule = new Schedule();
                IEnumerable<Schedule> query2 = schedules.Where(s => s.Active == true);
                List<Schedule> activeTimeslots2 = new List<Schedule>();
                foreach (Schedule queryItem in query2)
                {
                    activeTimeslots2.Add(queryItem);
                }

                for (int i = 0; i < activeTimeslots2.Count; i++)
                {
                    if (i == 0)
                        firstSchedule = activeTimeslots2[0];
                    if (i == activeTimeslots2.Count - 1)
                        lastSchedule = activeTimeslots2[i];
                }
                user.DelayInMinutes = CalculateDelay(firstSchedule);
                string apiPathUserFinal = apiPathEmployee + user.ID;
                user.LastActive = DateTime.Now;
                user.ModifiedDate = DateTime.Now;
                UpdateEmployee(user, apiPathUserFinal);

                //Sæt viste værdier
                if (schedule.TsType == null || schedule.TsType == "")
                {
                    schedule.TsType = "Normal";
                }
                labelStartTid.Text = firstSchedule.StartTime.ToString();
                labelSlutTid.Text = lastSchedule.EndTime.ToString();
                labelTimeslotType.Text = firstSchedule.TsType;
                labelCurrentDelay.Text = user.DelayInMinutes.ToString();
                StkywControlPanelLight.Properties.Settings.Default.settingInitialDelay = (int)user.DelayInMinutes;
            }
            this.ActiveControl = null;
        }
        private void DisplayValues(Schedule schedule, EmployeeCurrentUsage user)
        {
            //Sæt viste værdier
            if (schedule.TsType == null || schedule.TsType == "")
            {
                schedule.TsType = "Normal";
            }
            labelStartTid.Text = schedule.StartTime.ToString();
            labelSlutTid.Text = schedule.EndTime.ToString();
            labelTimeslotType.Text = schedule.TsType;
            labelCurrentDelay.Text = user.DelayInMinutes.ToString();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (timerOthersAlert.Enabled == false)
            {
                timerOthersAlert.Start();
            }
            if (timerAutoDelay.Enabled == true)
            {
                timerAutoDelay.Stop();
            }

            string txt = comboBox1.SelectedItem.ToString();
            if (txt.Length > 0)
                txt = txt.Substring(0, 2);
            else
                txt = "0";
            labelCurrentDelay.Text = txt;
            user.DelayInMinutes = Convert.ToInt32(txt);
            string apiPathUserFinal = apiPathEmployee + user.ID;
            user.LastActive = DateTime.Now;
            user.ModifiedDate = DateTime.Now;
            UpdateEmployee(user, apiPathUserFinal);
            StkywControlPanelLight.Properties.Settings.Default.settingInitialDelay = (int)user.DelayInMinutes;
        }
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
        private void timerAutoDelay_Tick(object sender, EventArgs e)
        {
             IEnumerable<Schedule> query = schedules.Where(s => s.Active == true);
            List<Schedule> activeTimeslots = new List<Schedule>();
            foreach (Schedule queryItem in query)
            {
                activeTimeslots.Add(queryItem);
            }
            for (int i = 0; i < activeTimeslots.Count; i++)
            {
                if (i == activeTimeslots.Count-1)
                {
                    Schedule actSchedule = activeTimeslots[i];
                    user.DelayInMinutes = CalculateDelayAutoIncrease(actSchedule);

                    string apiPathUserFinal = apiPathEmployee + user.ID;
                    user.LastActive = DateTime.Now;
                    user.ModifiedDate = DateTime.Now;
                    UpdateEmployee(user, apiPathUserFinal);

                    DisplayValues(actSchedule, user);
                    break;
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1_Click(sender, e);
        }

        private void buttonCallInText_Click(object sender, EventArgs e)
        {
            var formPopup = new FormCallInMessage(userId, companyID, userName);
            formPopup.ShowDialog(this);
        }
    }
}

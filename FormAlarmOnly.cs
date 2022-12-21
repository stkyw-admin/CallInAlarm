using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StkywControlPanelCallIn
{
    public partial class FormAlarmOnly : Form
    {
        public int userId;
        public string userName;
        public int companyID;
        static HttpClient client = new HttpClient(); 
        static List<vw_EmployeeLogin> allEmployees = new List<vw_EmployeeLogin>();
        static List<EmployeeCurrentUsage> ecuList = new List<EmployeeCurrentUsage>();
        static string apiPathlogin = "http://www.api.sorrytokeepyouwaiting.com/api/Login/";//"https://localhost:7017/api/Login/";
        static string apiPathEmployee = "http://www.api.sorrytokeepyouwaiting.com/api/employees/";//"https://localhost:7017/api/employees/";
        EmployeeCurrentUsage user = new EmployeeCurrentUsage();
        public FormAlarmOnly(int var1, string var2, int var3)
        {
            Cursor.Current = Cursors.WaitCursor;
            userId = var1;
            userName = var2;
            companyID = var3;
            this.Text = "STKYW Control Panel - " + userName;

            string dayOfWeek = DateTime.Now.DayOfWeek.ToString();

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
            timerOthersAlert.Start();
            labelAlertOther.Dock = DockStyle.Fill;
            labelAlertOther2.Dock = DockStyle.Fill;
            labelAlertOther2.Text = "";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
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

        private void timerFlashing_Tick(object sender, EventArgs e)
        {
            if (this.BackColor == Color.Red)
                this.BackColor = SystemColors.Control;
            else
                this.BackColor = Color.Red;
        }

        private void timerOthersAlert_Tick(object sender, EventArgs e)
        {
            performAlertCheck(companyID, labelAlertOther2, user, timerFlashing);
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
                label.Text = alertUser.Name + Environment.NewLine + " har brug for hjælp!";
            }
            else
            {
                timer.Stop();
                label.Visible = false;
                label.Text = "";
            }
        }
        static async void UpdateEmployee(EmployeeCurrentUsage employee, string path)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                path, employee);
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
        private void FormAlarmOnly_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
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

        private void FormAlarmOnly_Load(object sender, EventArgs e)
        {

        }
    }
}

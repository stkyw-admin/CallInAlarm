using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StkywControlPanelCallInAlarm
{
    public partial class LoginForm : Form
    {
        static HttpClient client = new HttpClient();
        static List<vw_EmployeeLogin> allEmployees = new List<vw_EmployeeLogin>();
        static List<Company> allCompanies = new List<Company>();
        static List<EmployeeCurrentUsage> ecuList = new List<EmployeeCurrentUsage>();
        static vw_EmployeeLogin loginUser = new vw_EmployeeLogin();
        static EmployeeCurrentUsage loggedInUser = new EmployeeCurrentUsage();
        static Company loginCompany = new Company();
        static bool verified = false;
        static string apiPathEcu = "http://www.api.sorrytokeepyouwaiting.com/api/Employees/";//"https://localhost:7017/api/Employees/";
        static string apiPathCompany = "http://www.api.sorrytokeepyouwaiting.com/api/Companies/";//"https://localhost:7017/api/Companies/";
        static string apiPathlogin = "http://www.api.sorrytokeepyouwaiting.com/api/Login/";//"https://localhost:7017/api/Login/";
        static string apiPathUseLog = "http://www.api.sorrytokeepyouwaiting.com/api/UseLog/";//"https://localhost:7017/api/UseLog/";
        public LoginForm()
        {
            InitializeComponent();
            if (Properties.Settings.Default.settingUsername != null)
            {
                textBoxUsername.Text = Properties.Settings.Default.settingUsername;
                textBoxPassword.Text = Properties.Settings.Default.settingPassword;
                textBoxCompany.Text = Properties.Settings.Default.settingCompany;
                textBoxLoginDirections.Text = Properties.Settings.Default.settingDirections;
                checkBoxRememberMe.Checked = true;
            }
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            timerLoginTime.Start();
            string username = textBoxUsername.Text;
            string password = textBoxPassword.Text;
            string company = textBoxCompany.Text;
            string directions = textBoxLoginDirections.Text;
            Form f = this;
            Properties.Settings.Default.settingLoginWeek = 0;
            
            if (checkBoxRememberMe.Checked == true)
            {
                Properties.Settings.Default.settingUsername = username;
                Properties.Settings.Default.settingPassword = password;
                Properties.Settings.Default.settingCompany = company;
                Properties.Settings.Default.settingDirections = directions;
                Properties.Settings.Default.Save();
            }
            
            PerformLogin(sender, e, username, password, company, f, directions, timerLoginTime);

            //Test
            //UserID = 8;
        }
        static async Task PerformLogin(object sender, EventArgs e, string username, string password, string company, Form logForm, string directions, Timer timerLoginTime)
        {
            allEmployees = await GetEmployeeList(apiPathlogin);
            allCompanies = await GetCompanyList(apiPathCompany);
            ecuList = await GetECUList(apiPathEcu);
            username = username.ToUpper();

            for (int i = 0; i < allCompanies.Count; i++)
            {
                if (allCompanies[i].Name == company)
                {
                    loginCompany = allCompanies[i];
                }
            }

            for (int i = 0; i < allEmployees.Count; i++)
            {
                if (allEmployees[i].Username.ToUpper() == username && allEmployees[i].CompanyID == loginCompany.ID)
                {
                    loginUser = allEmployees[i];
                }
            }
            foreach (EmployeeCurrentUsage ecuItem in ecuList)
            {
                if (ecuItem.ID == loginUser.ID)
                    loggedInUser = ecuItem;
            }

            
            SHA256 sha256 = SHA256Managed.Create();
            byte[] hashValue;
            UTF8Encoding objUtf8 = new UTF8Encoding();
            hashValue = sha256.ComputeHash(objUtf8.GetBytes(password));
            bool passMatch = ByteArrayCompare(hashValue, loginUser.PasswordEncrypted);

            if (passMatch == true && loginUser.Username.ToUpper() == username && loginUser.CompanyID == loginCompany.ID)
            {
                verified = true;
            }
            else
            {
                verified = false;
            }

            if (verified == true)
            {
                Properties.Settings.Default.settingOnlyAlert = 0;
                FormStkywControlPanelCallInV2 main = new FormStkywControlPanelCallInV2(loginUser.ID, loginUser.Name, loginCompany.ID, directions, loginUser.wadAliasName);
                //main.userId = loginUser.ID;
                //main.userName = loginUser.Name;
                //main.companyID = loginCompany.ID;
                RegisterLogin(loginUser.ID, loginCompany.ID);
                timerLoginTime.Stop();
                logForm.Hide();
                main.Show();

                //wadAliasSelector was = new wadAliasSelector(loginUser.ID, loginUser);
                //was.Show();
            }
            else
            {
                MessageBox.Show("Dine login-info stemmer ikke overens med databasen.");
            }
        }
        static void RegisterLogin(int uID, int cID)
        {
            UseLog useLog = new UseLog();
            useLog.UserID = uID;
            useLog.CompanyID = cID;
            useLog.LogonTime = DateTime.Now;
            useLog.LogoffTime = DateTime.MaxValue;
            UpdateUseLog(useLog, apiPathUseLog);
        }
        static bool ByteArrayCompare(byte[] a1, byte[] a2)
        {
            return StructuralComparisons.StructuralEqualityComparer.Equals(a1, a2);
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
        static async Task<List<Company>> GetCompanyList(string path)
        {
            var response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var stringData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Company>>(stringData);
            }
            return null;
        }
        static async void UpdateUseLog(UseLog useLog, string path)
        {
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(path, useLog); //PostAsJsonAsync
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                string my = ex.Message;
                my += "";
            }
        }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int CompanyID { get; set; }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void timerLoginTime_Tick(object sender, EventArgs e)
        {
            MessageBox.Show("Vi kunne ikke logge dig på lige nu, prøv igen senere.");
            this.Enabled = true;
        }
    }


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
    public class UseLog
    {
        public Int64 UseLogID { get; set; }
        public int UserID { get; set; }
        public int CompanyID { get; set; }
        public DateTime LogonTime { get; set; }
        public DateTime LogoffTime { get; set; }
    }
}

﻿using Newtonsoft.Json;
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

namespace WindowsFormsApp1_StkywControlPanelLight
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
        static string apiPathEcu = "https://localhost:7017/api/Employees/";
        static string apiPathCompany = "https://localhost:7017/api/Companies/";
        static string apiPathlogin = "https://localhost:7017/api/Login/";
        public LoginForm()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text;
            string password = textBoxPassword.Text;
            string company = textBoxCompany.Text;
            Form f = this;
            PerformLogin(sender, e, username, password, company, f);

            //Test
            //UserID = 8;
        }
        static async Task PerformLogin(object sender, EventArgs e, string username, string password, string company, Form logForm)
        {
            allEmployees = await GetEmployeeList(apiPathlogin);
            allCompanies = await GetCompanyList(apiPathCompany);
            ecuList = await GetECUList(apiPathEcu);
            username = username.ToUpper();

            for (int i = 0; i < allEmployees.Count; i++)
            {
                if (allEmployees[i].Username.ToUpper() == username.ToUpper())
                {
                    loginUser = allEmployees[i];
                }
            }
            foreach (EmployeeCurrentUsage ecuItem in ecuList)
            {
                if (ecuItem.ID == loginUser.ID)
                    loggedInUser = ecuItem;
            }

            for (int i = 0; i < allCompanies.Count; i++)
            {
                if (allCompanies[i].Name == company)
                {
                    loginCompany = allCompanies[i];
                }
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
                FormStkywControlPanelLightV2 main = new FormStkywControlPanelLightV2(loginUser.ID, loginUser.Name, loginCompany.ID);
                //main.userId = loginUser.ID;
                //main.userName = loginUser.Name;
                //main.companyID = loginCompany.ID;
                logForm.Hide();
                main.Show();
            }
            else
            {
                MessageBox.Show("Dine login-info stemmer ikke overens med databasen.");
            }
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
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int CompanyID { get; set; }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
}
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
    public partial class wadAliasSelector : Form
    {
        public int userId;
        static HttpClient client = new HttpClient();
        static string path = "http://www.api.sorrytokeepyouwaiting.com/api/wadAlias/";//"https://localhost:7017/api/wadAlias/";
        static string apiPathEmployee = "http://www.api.sorrytokeepyouwaiting.com/api/employees/";//"https://localhost:7017/api/employees/";
        EmployeeCurrentUsage user = new EmployeeCurrentUsage();
        vw_EmployeeLogin loggedInUser = new vw_EmployeeLogin();
        public wadAliasSelector(int var1, vw_EmployeeLogin var2)
        {
            userId = var1;
            user.ID = userId; // 8;
            loggedInUser = var2;
            InitializeComponent();

            string myVar = loggedInUser.wadAliasName;
            string[] wanArray = myVar.Split(';');
            for (int i = 0; i < wanArray.Length; i++)
            {
                comboBox1.Items.Add(wanArray[i]);
            }
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ecuLayout = comboBox1.SelectedItem.ToString();
            user.ChosenLayout = ecuLayout;
            string finalPath = apiPathEmployee + user.ID;
            UpdateEmployee(user, finalPath);
            Properties.Settings.Default.settingLocation = ecuLayout;
            this.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            user.ChosenLayout = null;
            string finalPath = apiPathEmployee + user.ID;
            UpdateEmployee(user, finalPath);
            Properties.Settings.Default.settingLocation = null;
            this.Close();
        }
        static async void UpdateEmployee(EmployeeCurrentUsage employee, string path)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                path, employee);
            response.EnsureSuccessStatusCode();
        }
    }
}

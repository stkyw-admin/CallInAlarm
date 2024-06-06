using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StkywControlPanelCallInAlarm
{
    public partial class ShowUseLog : Form
    {
        static HttpClient client = new HttpClient();
        int uID;
        int cID;
        static List<UseLog> useLogList = new List<UseLog>();
        static string apiPathUseLog = "http://www.api.sorrytokeepyouwaiting.com/api/UseLog/";//"https://localhost:7017/api/UseLog/";
        UseLog useLogEntity = new UseLog();
        public ShowUseLog(int userID, int companyID)
        {
            InitializeComponent();
            uID = userID;
            cID = companyID;
            PrepareVariables(uID, companyID, dataGridView1);
        }
        static async Task PrepareVariables(int employeeID, int companyID, DataGridView dgv)
        {
            useLogList = await GetUseLogList(apiPathUseLog);

            //useListID
            if (Properties.Settings.Default.settingUseLog == true)
            {
                DateTime reasonableDate = DateTime.MaxValue.AddDays(-1);
                IEnumerable<UseLog> query;
                query = useLogList.Where(s => s.CompanyID == companyID
                                           && s.UserID == employeeID
                                           && s.LogonTime >= DateTime.Today
                                           && s.LogoffTime < reasonableDate);
                query = query.OrderBy(s => s.LogonTime);
                useLogList = query.ToList();

                foreach (UseLog item in useLogList)
                {
                    int curEmployeeMinutesWorkedTotal = 0;
                    int curEmployeeHoursWorkedTotal = 0;

                    TimeSpan span = item.LogoffTime.Subtract(item.LogonTime);
                    curEmployeeMinutesWorkedTotal = Convert.ToInt32(span.TotalMinutes);

                    while (curEmployeeMinutesWorkedTotal >= 60)
                    {
                        curEmployeeHoursWorkedTotal++;
                        curEmployeeMinutesWorkedTotal -= 60;
                    }

                    dgv.Rows.Add(item.LogonTime.ToShortDateString(),
                                 item.LogonTime.ToShortTimeString(),
                                 item.LogoffTime.ToShortTimeString(),
                                 curEmployeeHoursWorkedTotal.ToString() + " timer " + curEmployeeMinutesWorkedTotal.ToString() + " minutter");
                }

            }
            dgv.Refresh();
        }
        static async Task<List<UseLog>> GetUseLogList(string path)
        {
            try
            {
                var response = await client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    var stringData = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<UseLog>>(stringData);
                }
            }
            catch (Exception ex)
            {
                string myEx = ex.Message;
                myEx = "";
            }
            return null;
        }

        private void buttonExtractToFile_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "Excel files (*.csv)|*.csv|txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    string header = "Arbejdsdato,Første Logon,Sidste Logout, Tid arbejdet\r\n";
                    myStream.Write(Encoding.UTF8.GetBytes(header), 0, header.Length);
                    // Code to write the stream goes here.
                    for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    {
                        var row = dataGridView1.Rows[i];
                        string rowData = row.Cells[0].Value.ToString();
                        rowData += "," + row.Cells[1].Value.ToString();
                        rowData += "," + row.Cells[2].Value.ToString();
                        rowData += "," + row.Cells[3].Value.ToString();
                        myStream.Write(Encoding.UTF8.GetBytes(rowData), 0, rowData.Length);
                    }
                    myStream.Close();
                }
            }

        }
    }
}

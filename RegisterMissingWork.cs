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

namespace StkywControlPanelCallInAlarm
{
    public partial class RegisterMissingWork : Form
    {
        static HttpClient client = new HttpClient();
        int uID;
        int cID;
        static string apiPathUseLog = "http://www.api.sorrytokeepyouwaiting.com/api/UseLog/";//"https://localhost:7017/api/UseLog/";
        public RegisterMissingWork(int userID, int companyId)
        {
            InitializeComponent();
            labelError.Visible = false;
            uID = userID;
            cID = companyId;
        }

        private void buttonSaveAndContinue_Click(object sender, EventArgs e)
        {
            DateTime startTid = new DateTime(dateTimePickerStartDate.Value.Year, dateTimePickerStartDate.Value.Month, dateTimePickerStartDate.Value.Day
                                            , dateTimePickerStartTime.Value.Hour, dateTimePickerStartTime.Value.Minute, dateTimePickerStartTime.Value.Second);
            DateTime slutTid = new DateTime(dateTimePickerEndDate.Value.Year, dateTimePickerEndDate.Value.Month, dateTimePickerEndDate.Value.Day
                                            , dateTimePickerEndTime.Value.Hour, dateTimePickerEndTime.Value.Minute, dateTimePickerEndTime.Value.Second);
            string p = "";
            if (DateTime.Compare(startTid, slutTid) == -1)
            {
                //Lav tidsreg.
                UseLog useLog = new UseLog();
                useLog.UserID = uID;
                useLog.CompanyID = cID;
                useLog.LogonTime = startTid;
                useLog.LogoffTime = slutTid;
                UpdateUseLog(useLog, apiPathUseLog);
                labelError.Text = "Tid (" + startTid.ToString() + " til " + slutTid.ToString() + ") registreret";
                labelError.Visible = true;
            }
            else
            {
                labelError.Text = "Start tidspunktet skal være før slut tidspunktet.";
                labelError.Visible = true;
            }

        }

        private void buttonSaveAndClose_Click(object sender, EventArgs e)
        {
            buttonSaveAndContinue_Click(sender, e);
            this.Close();
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
    }
}

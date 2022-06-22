using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StkywControlPanelLight
{
    public partial class FormCallInMessage : Form
    {
        public int userId;
        public string userName;
        public int companyID;
        public FormCallInMessage(int var1, int var2, string var3)
        {
            userId = var1;
            companyID = var2;
            userName = var3;
            InitializeComponent();
        }

        private void buttonCallInConfirm_Click(object sender, EventArgs e)
        {
            //gem data i db
            SqlConnection con = new SqlConnection("Data Source=mssql2.dandomain.dk;Initial Catalog=sorrytokeepyouwaiting_com;User Id=sorrytokeepyouwaiting;Password=5dXkHz_=A2;");
            con.Open();
            string updateQuery = "udsp_Insert_CallInMessage";
            SqlCommand updCmd = new SqlCommand(updateQuery, con);
            updCmd.CommandType = CommandType.StoredProcedure;
            updCmd.Parameters.AddWithValue("@employeeID", userId);
            updCmd.Parameters.AddWithValue("@companyID", companyID);
            updCmd.Parameters.AddWithValue("@besked", textBoxCallIn.Text);
            updCmd.ExecuteNonQuery();
            con.Close();

            this.Close();
        }
    }
}

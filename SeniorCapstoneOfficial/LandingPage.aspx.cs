using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.DataVisualization.Charting;
using System.Threading.Tasks;
using Box.V2.Config;
using Box.V2.JWTAuth;
using Box.V2.Models;
using Box.V2;

namespace SeniorCapstoneOfficial
{
    public partial class LandingPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            DBConnector db = new DBConnector();
            bool admin = db.AdminCheck(Session["UserName"].ToString());
            if (admin == false)
            {
                StudentSearchButton.Attributes.Add("style", "right:15%");
                AdminPageButton.Visible = false;
            }

            RegisterAsyncTask(new PageAsyncTask(showLogins));
        }

        protected void AdminPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminMenu.aspx");
        }

        protected void LogoutBtn_Click(object sender, EventArgs e)
        {

            DBConnector dbconnector = new DBConnector();
            dbconnector.saveLogout(Session["UserName"].ToString(), DateTime.Now);
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }

        protected void StudentSearchBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("NormalUserMenu.aspx");
        }


        private async Task showLogins()
        {
            DBConnector db = new DBConnector();
            BoxAuthTest box = new BoxAuthTest();
            IEnumerable<BoxUser> allUsers = await box.GetallUsers();

            Chart lastLogins = new Chart();

            ChartArea OneDay = new ChartArea("oneDay");

            lastLogins.ChartAreas.Add(OneDay);

            Series day1 = new Series();
            Series week1 = new Series();
            Series month1 = new Series();

            lastLogins.Series.Add(day1);
            lastLogins.Series.Add(week1);
            lastLogins.Series.Add(month1);

            day1.ChartArea = "oneDay";

            DataTable table1 = new DataTable("logins");
            table1.Columns.Add("loginCountDay");
            int dayCount = 0;

            DateTime startDate = DateTime.Today;

            DateTime endDate = new DateTime();
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

            foreach (BoxUser user in allUsers)
            {
                if(user.ModifiedAt >= startDate && user.ModifiedAt < endDate)
                {
                    dayCount++;
                }
            }
            DataRow loginRow = table1.NewRow();
            loginRow["loginCountDay"] = dayCount.ToString();
            table1.Rows.Add(loginRow);

            table1.Columns.Add("loginCountWeek");
            int weekCount = 0;

            startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);

            endDate = startDate.AddDays(DayOfWeek.Friday - startDate.DayOfWeek).Date;

            foreach (BoxUser user in allUsers)
            {
                if (user.ModifiedAt >= startDate && user.ModifiedAt < endDate)
                {
                    weekCount++;
                }
            }
            loginRow["loginCountWeek"] = weekCount.ToString();

            table1.Columns.Add("loginCountMonth");
            int monthCount = 0;
            DateTime now = DateTime.Now;
            startDate = new DateTime(now.Year, now.Month, 1);
            endDate = startDate.AddMonths(1).AddDays(-1);

            foreach (BoxUser user in allUsers)
            {
                if (user.ModifiedAt >= startDate && user.ModifiedAt < endDate)
                {
                    monthCount++;
                }
            }
            loginRow["loginCountMonth"] = monthCount.ToString();

           // lastLogins.Series[0].YValueMembers = dayCount.ToString() ;
            day1.Points.AddY(dayCount);
           // lastLogins.Series[1].YValueMembers = weekCount.ToString();
            day1.Points.AddY(weekCount);
           // lastLogins.Series[2].YValueMembers = monthCount.ToString();
            day1.Points.AddY(monthCount);

            //lastLogins.DataSource = table1;
            //lastLogins.DataBind();
            lastLogins.Series[0].Points[0].AxisLabel = "1 Day";
            lastLogins.Series[0].Points[1].AxisLabel = "1 Week";
            lastLogins.Series[0].Points[2].AxisLabel = "1 Month";
            //lastLogins.ChartAreas[0].AxisX.CustomLabels;
            //lastLogins.Series[1].AxisLabel = "1 Week";
            //lastLogins.Series[2].AxisLabel = "1 Month";

            LoginChart.Controls.Add(lastLogins);       
        }


    }
}
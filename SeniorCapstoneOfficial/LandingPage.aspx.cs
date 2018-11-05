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

            //RegisterAsyncTask(new PageAsyncTask(showLogins));
            RegisterAsyncTask(new PageAsyncTask(GetLoginsss));

        }

        protected void AdminPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminMenu.aspx");
        }

        private async Task GetLoginsss()
        {
            BoxAuthTest box = new BoxAuthTest();
            string a = await box.GetLogins();

       

            fillTopFiveUserChart(TopUsersLogin(LoginList(a)));

            //foreach (string y in LoginList(a))
            //{
            //    Label aaa = new Label();
            //    aaa.Text = y;
            //    topChart.Controls.Add(aaa);
            //    topChart.Controls.Add(new LiteralControl("<br />"));
            //    topChart.Controls.Add(new LiteralControl("<br />"));

            //}

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

        private void fillTopFiveUserChart(List<Tuple<int,string>> topFive)
        {
            Chart topFiveUsersChart = new Chart();

            ChartArea logins = new ChartArea("logins");

            topFiveUsersChart.ChartAreas.Add(logins);

            topFiveUsersChart.ChartAreas["logins"].AxisX.MajorGrid.Enabled = false;
            topFiveUsersChart.ChartAreas["logins"].AxisY.MajorGrid.Enabled = false;

            Series user1 = new Series();

            topFiveUsersChart.Series.Add(user1);

            topFiveUsersChart.Series[0].ChartType = SeriesChartType.Bar;
            topFiveUsersChart.Series[0].IsValueShownAsLabel = true;

            user1.Points.AddY(topFive[4].Item1);
            user1.Points.AddY(topFive[3].Item1);
            user1.Points.AddY(topFive[2].Item1);
            user1.Points.AddY(topFive[1].Item1);
            user1.Points.AddY(topFive[0].Item1);

            topFiveUsersChart.Series[0].Points[0].AxisLabel = topFive[4].Item2;
            topFiveUsersChart.Series[0].Points[1].AxisLabel = topFive[3].Item2;
            topFiveUsersChart.Series[0].Points[2].AxisLabel = topFive[2].Item2;
            topFiveUsersChart.Series[0].Points[3].AxisLabel = topFive[1].Item2;
            topFiveUsersChart.Series[0].Points[4].AxisLabel = topFive[0].Item2;

            topChart.Controls.Add(topFiveUsersChart);
        }

        private async Task showLogins()
        {
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
                if (user.ModifiedAt >= startDate && user.ModifiedAt < endDate)
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

        private int NumOfLogins(string listOfLogins)
        {
            string tester = "";
            string tester1 = "";
            int index = listOfLogins.IndexOf(',');
            if (index > 0)
            {
                tester = listOfLogins.Substring(0, index);
            }

            int result = tester.IndexOf(':');
            if (index > 0)
            {
                tester1 = tester.Substring(result + 1);
            }

            int myInt = int.Parse(tester1);

            return myInt;
        }

        private string NextStreamPos(string listOfLogins)
        {
            string tester = "";
            string tester1 = "";
            int index = listOfLogins.IndexOf("\",\"entries");
            if (index > 0)
            {
                tester = listOfLogins.Substring(0, index);
            }

            int result = tester.IndexOf("\":\"");
            if (index > 0)
            {
                tester1 = tester.Substring(result + 3);
            }

            return tester1;
        }

        private List<string> LoginList(string listOfLogins)
        {
            string[] latest = listOfLogins.Split(new string[] { "\"source\"" }, StringSplitOptions.RemoveEmptyEntries);
            latest.Reverse();
            List<string> alist = new List<string>();
            for (int i = latest.Length - 1; i >= 0; i--)
            {
                if (latest[i] != null && alist.Count <= 500)
                {
                    alist.Add(latest[i]);
                }

                else
                {
                    break;
                }
            }

            return alist;
        }

        private List<Tuple<int, string>> TopUsersLogin(List<string> loginlist)
        {
            List<Tuple<int, string>> topusers = new List<Tuple<int, string>>();

            string tester = "";
            string tester1 = "";

            List<string> userlist = new List<string>();

            foreach (string str in loginlist)
            {
                int index = str.IndexOf("name");
                if (index > 0)
                {
                    tester = str.Substring(index + 7);
                }

                int result = tester.IndexOf("\",\"");
                if (index > 0)
                {
                    tester1 = tester.Substring(0, result);
                }

                userlist.Add(tester1);
            }

            var q = userlist.GroupBy(x => x)
                        .Select(g => new { Value = g.Key, Count = g.Count() })
                        .OrderByDescending(x => x.Count);
            int c = 0;
            string v = "";

            foreach (var x in q)
            {
                c = x.Count;
                v = x.Value;

                var user = Tuple.Create(c, v);
                topusers.Add(user);
            }

            List<Tuple<int,string>> topFive = topusers.GetRange(0, 5);
            return topFive;
        }

    }
}
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
            RegisterAsyncTask(new PageAsyncTask(getUsersss));


        }

        protected void AdminPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminMenu.aspx");
        }

        private async Task LastLogins()
        {
            BoxAuthTest box = new BoxAuthTest();
            string a = await box.GetLogins();
        }

        private async Task GetLoginsss()
        {
            BoxAuthTest box = new BoxAuthTest();
            string a = await box.GetLogins();

            fillTopFiveUserChart(TopUsersLogin(LoginList(a)));
            fillLastLoginCharts(lastLoginsByTime(LoginList(a)));


            //foreach (string y in LoginList(a))
            //{
            //    Label aaa = new Label();
            //    aaa.Text = y;
            //    topChart.Controls.Add(aaa);
            //    topChart.Controls.Add(new LiteralControl("<br />"));
            //    topChart.Controls.Add(new LiteralControl("<br />"));

            //}

        }

        private async Task getUsersss()
        {
            BoxAuthTest box = new BoxAuthTest();
            List<BoxUser> users = await box.GetallUsers();
            List<Tuple<int, string>> storage = new List<Tuple<int, string>>();

            foreach (BoxUser u in users)
            {
                int mb = (Convert.ToInt32(u.SpaceUsed) / 1024) / 1024;
                var v = Tuple.Create(mb, u.Name);
                storage.Add(v);
            }

            var orderedStorage = storage.OrderByDescending(t => t.Item1).ToList();

            List<Tuple<int, string>> topFiveStorage = orderedStorage.GetRange(0, 5);

            Chart topFiveUsersChart = new Chart();

            ChartArea logins = new ChartArea("logins");

            topFiveUsersChart.ChartAreas.Add(logins);

            topFiveUsersChart.ChartAreas["logins"].AxisX.MajorGrid.Enabled = false;
            topFiveUsersChart.ChartAreas["logins"].AxisY.MajorGrid.Enabled = false;

            Series user1 = new Series();

            topFiveUsersChart.Series.Add(user1);

            topFiveUsersChart.Series[0].ChartType = SeriesChartType.Bar;
            topFiveUsersChart.Series[0].IsValueShownAsLabel = true;

            user1.Points.AddY(topFiveStorage[4].Item1);
            user1.Points.AddY(topFiveStorage[3].Item1);
            user1.Points.AddY(topFiveStorage[2].Item1);
            user1.Points.AddY(topFiveStorage[1].Item1);
            user1.Points.AddY(topFiveStorage[0].Item1);

            topFiveUsersChart.Series[0].Points[0].AxisLabel = topFiveStorage[4].Item2;
            topFiveUsersChart.Series[0].Points[1].AxisLabel = topFiveStorage[3].Item2;
            topFiveUsersChart.Series[0].Points[2].AxisLabel = topFiveStorage[2].Item2;
            topFiveUsersChart.Series[0].Points[3].AxisLabel = topFiveStorage[1].Item2;
            topFiveUsersChart.Series[0].Points[4].AxisLabel = topFiveStorage[0].Item2;

            topStorageChart.Controls.Add(topFiveUsersChart);
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

        private void fillLastLoginCharts(List<string> lastlogins)
        {
          

            Chart lastLoginsChart = new Chart();

            ChartArea OneDay = new ChartArea("time");

            lastLoginsChart.ChartAreas.Add(OneDay);

            Series time = new Series();

            lastLoginsChart.Series.Add(time);

            int dayCount = 0;

            DateTime startDate = DateTime.Today;

            DateTime endDate = new DateTime();
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

        
            foreach (string date in lastlogins)
            {              
                if (DateTime.Parse(date) >= startDate)
                {
                    dayCount++;
                }

            }
            
            int weekCount = 0;

            //startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);

            //endDate = startDate.AddDays(DayOfWeek.Friday).Date;
            DateTime today = DateTime.Today;
            int offset = today.DayOfWeek - DayOfWeek.Monday;
            startDate = today.AddDays(-offset);
            endDate = startDate.AddDays(6);

            foreach (string date in lastlogins)
            {
                if (DateTime.Parse(date) >= startDate && DateTime.Parse(date) < endDate)
                {
                    weekCount++;
                }
            }

            int monthCount = 0;
            DateTime now = DateTime.Now;
            startDate = new DateTime(now.Year, now.Month, 1);
            endDate = startDate.AddMonths(1).AddDays(-1);

            foreach (string date in lastlogins)
            {
                if (DateTime.Parse(date) >= startDate && DateTime.Parse(date) < endDate)
                {
                    monthCount++;
                }
            }
            


            time.Points.AddY(dayCount);
            time.Points.AddY(weekCount);
            time.Points.AddY(monthCount);

            lastLoginsChart.Series[0].Points[0].AxisLabel = "1 Day";
            lastLoginsChart.Series[0].Points[1].AxisLabel = "1 Week";
            lastLoginsChart.Series[0].Points[2].AxisLabel = "1 Month";

            lastLoginsChart.Series[0].IsValueShownAsLabel = true;
            lastLoginsChart.ChartAreas["time"].AxisX.MajorGrid.Enabled = false;
            lastLoginsChart.ChartAreas["time"].AxisY.MajorGrid.Enabled = false;

            LoginChart.Controls.Add(lastLoginsChart);
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

        private List<string> lastLoginsByTime(List<string> loginlist)
        {
            List<string> lastLogins = new List<string>();
            string tester = "";
            string tester1 = "";

            foreach (string str in loginlist)
            {
                int index = str.IndexOf("created_at");
                if (index > 0)
                {
                    tester = str.Substring(index + 13);
                }

                int result = tester.IndexOf("T");
                if (index > 0)
                {
                    tester1 = tester.Substring(0, result);
                }

                lastLogins.Add(tester1);
            }

            return lastLogins;
        }

    }
}
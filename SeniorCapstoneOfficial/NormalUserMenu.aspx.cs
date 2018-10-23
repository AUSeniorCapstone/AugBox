using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using Box.V2.Config;
using Box.V2.JWTAuth;
using Box.V2.Models;
using Box.V2;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Data;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using Excel = Microsoft.Office.Interop.Excel;

namespace SeniorCapstoneOfficial
{
    public partial class NormalUserMenu : System.Web.UI.Page
    {
        private static List<Button> btn = new List<Button>();
        private static List<Label> lbl = new List<Label>();
        private static List<Button> DeleteButtonList = new List<Button>();
        private static BoxUser foundUser = new BoxUser();

        List<BoxItem> FolderList = new List<BoxItem>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            else
            {
                RegisterAsyncTask(new PageAsyncTask(ListofEmailToJavaScript));

                DBConnector db = new DBConnector();
                bool admin = db.AdminCheck(Session["UserName"].ToString());
                if (admin == false)
                {
                    StudentSearchButton.Attributes.Add("style", "right:15%");
                    AdminPageButton.Visible = false;
                }
                RecentEvents.Visible = false;
                InvalidEmailLabel.Visible = false;
                Exportbtn.Visible = true;
            }
        }

        protected void SearchForStudent_Click(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(GetUsersbyEmail));
        }

        protected void Exportbtn_Click(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(ExportEverything));

        }


        public void WriteTsv<T>(IEnumerable<T> data, TextWriter output)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor prop in props)
            {
                output.Write(prop.DisplayName); // header
                output.Write("\t");
            }
            output.WriteLine();
            foreach (T item in data)
            {
                foreach (PropertyDescriptor prop in props)
                {
                    output.Write(prop.Converter.ConvertToString(
                         prop.GetValue(item)));
                    output.Write("\t");
                }
                output.WriteLine();
            }
        }

        private async Task ExportEverything()
        {

            /*
           Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            var excel = new Excel.Application();
            var workBooks = excel.Workbooks;
            var workBook = workBooks.Add();
            
            var workSheet = (Excel.Worksheet)excel.ActiveSheet;
            MemoryStream st = new MemoryStream();
            workSheet.Cells[1, "A"] = "Name";
            workSheet.Cells[1, "B"] = "Email";
            workSheet.Cells[1, "C"] = "SpaceUsed";
            workSheet.Cells[1, "D"] = "Status";
            workSheet.Cells[1, "E"] = "LastLogin";
            workSheet.Columns.AutoFit();
            workSheet.Rows.AutoFit();

            */



            Response.Clear();
            Response.Charset = "";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            string filename = "user";
            DataTable data = new DataTable();
            BoxAuthTest box = new BoxAuthTest();
            IEnumerable<BoxUser> allUsers = await box.GetallUsers();

            string string1 = "Name";
            string string5 = "Email";
            string string2 = "SpaceUsed";
            string string3 = "Status";
            string string4 = "LastLogin";
            //string string5 = "EmailAlias1";
            //string string6 = "EmailAlias2";

            List<string> columnHead = new List<string>();
            List<string> row = new List<string>();
            columnHead.Add(string1);
            columnHead.Add(string5);
            columnHead.Add(string2);
            columnHead.Add(string3);
            columnHead.Add(string4);
            //columnHead.Add(string6);
            string tab = "";
            foreach (string s in columnHead)
            {
                Response.Write(tab + s);
                tab = "\t";
            }

            string1 = "";
            string2 = "";
            string3 = "";
            string4 = "";
            string5 = "";
            //string6 = "";

            Response.Write("\n");
            foreach (BoxUser user in allUsers)
            {
                //try
                //{

                //var emailAliases = await box.GetEmailAlias(user.Id);
                //for (int i = 0; i < emailAliases.TotalCount; i++)
                //{
                //    if (emailAliases.TotalCount > 1)
                //        string6 = string6 + ", " + emailAliases.Entries[i].Email;
                //    if (emailAliases.TotalCount == 1)
                //        string6 = emailAliases.Entries[i].Email;
                //    else
                //        string6 = "";
                //}

                string1 = user.Name;
                row.Add(string1);
                string5 = user.Login.ToString();
                string5 = string5.Replace(" ", "");
                row.Add(string5);
                string2 = user.SpaceUsed.ToString();
                string2 = string2.Replace(" ", "");
                row.Add(string2);
                string3 = user.Status.ToUpper();
                string3 = string3.Replace(" ", "");
                row.Add(string3);
                string4 = user.ModifiedAt.ToString();
                int a = string4.IndexOf(' ');   //adding "-" between date and time
                string astring = string4.Substring(0, a);
                string4 = string4.Substring(a);
                string finalstring = astring + " -" + string4;
                finalstring.Replace(" ", "");
                row.Add(finalstring);


                //if (emailAliases.Entries[0] == null)
                //    string5 = "";
                //else
                //    string5 = emailAliases.Entries[0].Email;
                //string5 = string5.Replace(" ", "");
                //if (emailAliases.Entries[1] == null)
                //    string6 = "";
                //else
                //    string6 = emailAliases.Entries[1].Email;
                //string6 = string5.Replace(" ", "");
                //row.Add(string5);
                //row.Add(string6);
                tab = "";
                foreach (string s in row)
                {
                    Response.Write(tab + s);
                    tab = "\t";
                }
                Response.Write("\n");
                row.Clear();
            }
            //catch (Exception ex)
            //{
            //    Response.Write("Box user: " + user.Name + " has not verified email address.\n\n");
            //}
            //}


            filename = filename.Replace(" ", "");
            Response.AddHeader("content-disposition", "attachment;filename=" + "AllUsers.xls");
            Response.Flush();
            Response.End();

        }

        private async Task GetUsersbyEmail()
        {
            BoxAuthTest box = new BoxAuthTest();
            bool found = false;
            List<BoxUser> users = await box.GetallUsers();

            foundUser = users.Find(u => u.Login.Equals(EmailAddress.Text.Trim()));
            String emails = "";

            if (foundUser != null)
            {
                rightdiv.Style.Add("border-left", "dashed 1pt");
                rightdiv.Style.Add("border-left-color", "#033459");
                //string[] recent = await box.GetRecentEvents(foundUser.Id);
                RecentEvents.Visible = true;
                List<string> recent = await box.GetRecentEvents(foundUser.Id);
                Label8.Text = string.Join("}},", recent);

                var emailAliases = await box.GetEmailAlias(foundUser.Id);
                //var teste = box.GetRecentEvents(foundUser.Id);                

                TextBox1.Visible = true;
                Button7.Visible = true;


                Label1.Text = "<b>" + "Name: " + "</b>" + foundUser.Name;
                Label2.Text = "<b>" + "Space Used: " + "</b>" + foundUser.SpaceUsed.ToString() + " bytes";
                Label3.Text = "<b>" + "Status: " + "</b>" + foundUser.Status.ToUpper();
                Label4.Text = "<b>" + "Last Login: " + "</b>" + foundUser.ModifiedAt.ToString();
                if (emailAliases.TotalCount == 0)
                {
                    Label5.Visible = true;
                    Label6.Visible = false;
                    Label5.Text = "<b>" + "Email Alias #1: " + "</b>" + "No email aliases exist for this user.";
                }
                else if (emailAliases.TotalCount == 1)
                {
                    Label5.Visible = true;
                    Label6.Visible = false;
                    Label5.Text = "<b>" + "Email Alias #1: " + "</b>" + emailAliases.Entries[0].Email;
                    Button5.Visible = true;
                    Button6.Visible = false;
                }
                else for (int i = 0; i < 1; i++)
                    {
                        Label5.Visible = true;
                        Button5.Visible = true;
                        Label6.Visible = true;
                        Button6.Visible = true;
                        Label5.Text = "<b>" + "Email Alias #0: </b>" + emailAliases.Entries[0].Email;
                        Label6.Text = "<b>" + "Email Alias #1: </b>" + emailAliases.Entries[1].Email;
                    }
                lbl.Add(Label5);
                lbl.Add(Label6);
                Label7.Text = "<b>" + "Top Folders" + "</b>";
                Label7.Visible = true;
                Exportbtn.Visible = true;
                found = true;

                FolderList = await box.GetFolder(foundUser.Id);
                int foldercount = FolderList.Count;


                for (int i = 0; i < foldercount; i++)
                {
                    Label folder = new Label();
                    folder.ID = "folder" + i;
                    folder.Text = FolderList[i].Name;

                    FolderPH.Controls.Add(folder);
                    FolderPH.Controls.Add(new LiteralControl("<br />"));
                    FolderPH.Controls.Add(new LiteralControl("<br />"));
                }
            }



            else if (found == false)
            {
                rightdiv.Style.Remove("border-left");
                rightdiv.Style.Remove("border-left-color");
                RecentEvents.Visible = false;
                Exportbtn.Visible = true;
                InvalidEmailLabel.Visible = true;
                Label1.Text = "";
                Label2.Text = "";
                Label3.Text = "";
                Label4.Text = "";
                Label7.Text = "";
                Label8.Text = "";
            }
        }

        private async Task ListofEmailToJavaScript()
        {
            List<string> listemails = new List<string>();
            BoxAuthTest box = new BoxAuthTest();
            List<BoxUser> listusers = await box.GetallUsers();

            foreach (BoxUser user in listusers)
            {
                listemails.Add(user.Login);
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<script>");
            sb.Append("var testArray = new Array;");

            foreach (string email in listemails)
            {
                sb.Append("testArray.push('" + email + "');");
            }
            sb.Append("</script>");
            ClientScript.RegisterStartupScript(this.GetType(), "TestArrayScript", sb.ToString());
        }

        protected void LogoutBtn_Click(object sender, EventArgs e)
        {

            DBConnector dbconnector = new DBConnector();
            dbconnector.saveLogout(Session["UserName"].ToString(), DateTime.Now);
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }

        protected async void DeleteButton_Click(object sender, EventArgs e)
        {
            Button bttn = (Button)sender;
            string button = bttn.ID.ToString();
            button = button.Substring(button.IndexOf("n") + 1);
            var labelIndex = 0;
            Int32.TryParse(button, out labelIndex);

            string last = lbl[labelIndex - 5].Text;
            last = last.Substring(last.IndexOf(": ") + 2);


            BoxAuthTest box = new BoxAuthTest();
            BoxCollection<BoxEmailAlias> alia = await box.GetEmailAlias(foundUser.Id);
            string emailAliasID = alia.Entries[labelIndex - 5].Id;

            await box.DeleteEmailAlias(foundUser.Id, emailAliasID);
            RegisterAsyncTask(new PageAsyncTask(GetUsersbyEmail));
        }

        protected async void AddButton_Click(object sender, EventArgs e)
        {
            Button bttn = (Button)sender;
            string button = bttn.ID.ToString();
            button = button.Substring(button.IndexOf("n") + 1);
            var labelIndex = 0;
            Int32.TryParse(button, out labelIndex);

            BoxAuthTest box = new BoxAuthTest();
            string email = "";

            await box.CreateEmailAlias(foundUser.Id, email);
        }

        protected void AdminPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminMenu.aspx");
        }
    }
}
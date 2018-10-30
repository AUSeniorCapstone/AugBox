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
            int eventnumber = 0;
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
                List<string> logins = EventNameParser(recent);
                List<string> types = EventTypeParser(recent);
                List<string> create = EventCreationParser(recent);
                List<string> sourcetype = EventSourceTypeParser(recent);
                List<string> sourceid = EventSourceIdParser(recent);
                List<string> sourcenames = EventSourceNameParser(recent);
                if (logins[0] == "" || types[0] == "" || create[0] == "")
                {
                    Label Error = new Label();
                    Error.Text = "No recent events from this user";
                    EventHolder.Controls.Add(Error);
                }
                else
                {
                    for (int i = 0; i < recent.Count; i++)
                    {
                        eventnumber = eventnumber + 1;
                        Label temp = new Label();
                        Label temp2 = new Label();
                        temp.Text = "<b>" + eventnumber.ToString() + "</b>" + "." + " <b>Login:</b> " + logins[i] + " <b>Type:</b> " + types[i] + " <b>Created at:</b> " 
                            + create[i];
                        temp2.Text = " <b>Source</b> " + "(Type: " + sourcetype[i] + ", Id: " + sourceid[i] + ", Name: " + sourcenames[i] + ")";
                        temp.Attributes.CssStyle.Add("display", "block");
                        temp.Attributes.CssStyle.Add("clear", "right");
                        temp.Attributes.CssStyle.Add("margin-top", "25px");
                        EventHolder.Controls.Add(temp);
                        EventHolder.Controls.Add(temp2);

                    }
                }


                var emailAliases = await box.GetEmailAlias(foundUser.Id);
                //var teste = box.GetRecentEvents(foundUser.Id);                

                TextBox1.Visible = true;
                Button7.Visible = true;


                Label1.Text = "<b>" + "Name: " + "</b>" + foundUser.Name;
                Label2.Text = "<b>" + "Space Used: " + "</b>" + foundUser.SpaceUsed.ToString() + " bytes";
                Label3.Text = "<b>" + "Status: " + "</b>" + foundUser.Status.ToUpper();
                Label4.Text = "<b>" + "Last Login: " + "</b>" + foundUser.ModifiedAt.ToString();

                for (int i = 0; i < emailAliases.TotalCount; i++)
                {
                    Label label1 = new Label();
                    label1.Text = emailAliases.Entries[i].Email + " ";
                    label1.ID = "label" + (8 + i);
                    Button button = new Button();
                    button.ID = "button" + (8 + i);
                    button.Text = "Delete";
                    button.CausesValidation = true;
                    
                    button.Enabled = true;
                    button.UseSubmitBehavior = true;
                    button.CssClass += "DeleteUserButton";
                    Panel1.Controls.Add(label1);
                    Panel1.Controls.Add(button);
                    button.OnClientClick = "return Validate();";
                    button.Click += new EventHandler(DeleteButton_Click);
                    Panel1.Controls.Add(new LiteralControl("<br />"));
                }

                /*if (emailAliases.TotalCount == 0)
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
                lbl.Add(Label6); */
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

            string last = lbl[labelIndex - 8].Text;
            last = last.Substring(last.IndexOf(": ") + 2);


            BoxAuthTest box = new BoxAuthTest();
            BoxCollection<BoxEmailAlias> alia = await box.GetEmailAlias(foundUser.Id);
            string emailAliasID = alia.Entries[labelIndex - 8].Id;

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
            email = TextBox1.Text;
            VerifyEmail ve = new VerifyEmail();
            if (ve.IsValidEmail(email))
            {
                try
                {
                    await box.CreateEmailAlias(foundUser.Id, email);
                    RegisterAsyncTask(new PageAsyncTask(GetUsersbyEmail));
                    TextBox1.Text = "Enter Email Alias";
                }
                catch
                {
                    TextBox1.Text = "Invalid email";
                }
            }
            else
                ;
            //Error

        }
        private List<string> EventNameParser(List<string> listofevents)
        {
            List<string> listofnames = new List<string>();
            string a = "";
            foreach (string str in listofevents)
            {

                int index = str.IndexOf("login");
                index = index + 8;
                if (index == -1 || index < 20)
                {
                    listofnames.Add(a);
                    break;
                }
                else
                {
                    a = str.Substring(index);
                    int index2 = a.IndexOf("},");
                    a = a.Substring(0, index2 - 1);
                    listofnames.Add(a);
                }

            }
            return listofnames;
        }

        private List<string> EventTypeParser(List<string> listofevents)
        {
            List<string> listoftypes = new List<string>();
            string a = "";
            foreach (string str in listofevents)
            {
                try
                {
                    int index = str.IndexOf("event_type");
                    index = index + 13;
                    if (index == -1)
                    {
                        listoftypes.Add(a);
                        break;
                    }

                    else
                    {
                        a = str.Substring(index);
                        int index2 = a.IndexOf(",");
                        a = a.Substring(0, index2 - 1);
                        listoftypes.Add(a);
                    }
                }
                catch
                {
                    Server.Transfer("NormalUserMenu.aspx", true);
                }
            }
            return listoftypes;
        }

        private List<string> EventCreationParser(List<string> listofevents)
        {
            List<string> listofcreations = new List<string>();
            string a = "";
            foreach (string str in listofevents)
            {

                int index = str.IndexOf("created_at");
                index = index + 13;
                if (index == -1)
                {
                    listofcreations.Add(a);
                    break;
                }
                else
                {
                    a = str.Substring(index);
                    int index2 = a.IndexOf(",");
                    a = a.Substring(0, index2 - 1);
                    listofcreations.Add(a);
                }
            }

            return listofcreations;
        }

        private List<string> EventSourceTypeParser(List<string> listofevents)
        {
            List<string> list = new List<string>();
            string a = "";
            foreach (string str in listofevents)
            {

                int index = str.IndexOf("source");
                index = index + 17;
                if (index == -1)
                {
                    list.Add(a);
                    break;
                }
                else
                {
                    a = str.Substring(index);
                    int index2 = a.IndexOf(",");
                    a = a.Substring(0, index2 - 1);
                    list.Add(a);
                }
            }

            return list;
        }

        private List<string> EventSourceIdParser(List<string> listofevents)
        {
            List<string> list = new List<string>();
            string a = "";
            foreach (string str in listofevents)
            {

                int index = str.IndexOf("source");
                if (index == -1)
                {
                    list.Add(a);
                    break;
                }
                else
                {
                    a = str.Substring(index);
                    int index2 = a.IndexOf("id");
                    a = a.Substring(index2);
                    int index3 = a.IndexOf(",");
                    a = a.Substring(5, index3 - 6);
                    list.Add(a);
                }
            }

            return list;
        }

        private List<string> EventSourceNameParser(List<string> listofevents)
        {
            List<string> list = new List<string>();
            string a = "";
            foreach (string str in listofevents)
            {

                int index = str.IndexOf("source");
                if (index == -1)
                {
                    list.Add(a);
                    break;
                }
                else
                {
                    a = str.Substring(index);
                    int index2 = a.IndexOf("etag");
                    a = a.Substring(index2);
                    int index3 = a.IndexOf("name", 0, 22);
                    if (index3 == -1)
                    {
                        list.Add("Unknown");
                    }
                    else
                    {
                        a = a.Substring(index3);
                        int index4 = a.IndexOf(",");
                        a = a.Substring(7, index4 - 8);
                        list.Add(a);
                    }
                }
            }

            return list;
        }

        protected void AdminPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminMenu.aspx");
        }
    }
}
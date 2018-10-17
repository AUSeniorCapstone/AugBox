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

namespace SeniorCapstoneOfficial
{
    public partial class NormalUserMenu : System.Web.UI.Page
    {
        List<Button> btn = new List<Button>();
        List<Label> lbl = new List<Label>();
        List<Button> DeleteButtonList = new List<Button>();
        string emailAlias1 = "";
        string emailAlias2 = "";

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
            //BoxAuthTest box = new BoxAuthTest();
            //IEnumerable<BoxUser> allUsers = await box.GetallUsers();

            //var data = new[]{
            //                   new{ Name="Ram", Email="ram@techbrij.com", Phone="111-222-3333" },
            //                   new{ Name="Shyam", Email="shyam@techbrij.com", Phone="159-222-1596" },
            //                   new{ Name="Mohan", Email="mohan@techbrij.com", Phone="456-222-4569" },
            //                   new{ Name="Sohan", Email="sohan@techbrij.com", Phone="789-456-3333" },
            //                   new{ Name="Karan", Email="karan@techbrij.com", Phone="111-222-1234" },
            //                   new{ Name="Brij", Email="brij@techbrij.com", Phone="111-222-3333" }
            //          };

            ////var data = new List<string>();

            ////string string1 = "";
            ////string string2 = "";
            ////string string3 = "";
            ////string string4 = "";

            ////foreach (BoxUser user in allUsers)
            ////{
            ////    try
            ////    {

            ////        string1 = "Name: " + user.Name + "\n";
            ////        data.Add(string1);
            ////        string2 = "Space Used: " + user.SpaceUsed.ToString() + " bytes \n";
            ////        data.Add(string2);
            ////        string3 = "Status: " + user.Status.ToUpper() + "\n";
            ////        data.Add(string3);
            ////        string4 = "Last Login: " + user.ModifiedAt.ToString() + "\n";
            ////        data.Add(string4);                                      
            ////    }
            ////    catch (Exception ex)
            ////    {
            ////        Response.Write("Box user: " + user.Name + " has not verified email address.\n\n");
            ////    }
            ////}

            //Response.ClearContent();
            //Response.AddHeader("content-disposition", "attachment;filename=AllUsers.xls");
            //Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            //WriteTsv(data, Response.Output);
            //Response.End();

            Response.Clear();
            Response.Charset = "";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            string filename = "user";

            BoxAuthTest box = new BoxAuthTest();
            IEnumerable<BoxUser> allUsers = await box.GetallUsers();

            String string1 = "";
            String string2 = "";
            String string3 = "";
            String string4 = "";
            String string5 = "";
            String string6 = "";

            Response.Write("Name" + " " + "SpaceUsed" + " " + "Status" + " " + "LastLogin" + " " + "Email Alias" + " " + "Email Alias" + "\n");

            foreach (BoxUser user in allUsers)
            {
                try
                {

                    var emailAliases = await box.GetEmailAlias(user.Id);
                    for (int i = 0; i < emailAliases.TotalCount; i++)
                    {
                        if (emailAliases.TotalCount > 1)
                            string6 = string6 + ", " + emailAliases.Entries[i].Email;
                        if (emailAliases.TotalCount == 1)
                            string6 = emailAliases.Entries[i].Email;
                        else
                            string6 = "";
                    }

                    string1 = user.Name;
                    string1 = string1.Replace(" ", "");
                    string2 = user.SpaceUsed.ToString();
                    string2 = string2.Replace(" ", "");
                    string3 = user.Status.ToUpper();
                    string3 = string3.Replace(" ", "");
                    string4 = user.ModifiedAt.ToString();
                    string4 = string4.Replace(" ", "");
                    if (emailAliases.Entries[0] == null)
                        string5 = "";
                    else
                        string5 = emailAliases.Entries[0].Email;
                    string5 = string5.Replace(" ", "");
                    if (emailAliases.Entries[1] == null)
                        string6 = "";
                    else
                        string6 = emailAliases.Entries[1].Email;
                    string6 = string5.Replace(" ", "");


                    Response.Write(string1 + " " + string2 + " " + string3 + " " + string4 + " " + string5 + " " + string6 + "\n");
                }
                catch (Exception ex)
                {
                    Response.Write("Box user: " + user.Name + " has not verified email address.\n\n");
                }
            }


            filename = filename.Replace(" ", "");
            Response.AddHeader("content-disposition", "attachment;filename=" + filename.ToLower() + "Info.doc");
            Response.Flush();
            Response.End();

        }

        private async Task GetUsersbyEmail()
        {
            BoxAuthTest box = new BoxAuthTest();
            bool found = false;
            List<BoxUser> users = await box.GetallUsers();

            BoxUser foundUser = users.Find(u => u.Login.Equals(EmailAddress.Text.Trim()));
            String emails = "";

            if (foundUser != null)
            {
                var emailAliases = await box.GetEmailAlias(foundUser.Id);

                if (emailAliases.TotalCount == 2)
                    emails = emailAliases.Entries[1].Email;



                Label1.Text = "<b>" + "Name: " + "</b>" + foundUser.Name;
                Label2.Text = "<b>" + "Space Used: " + "</b>" + foundUser.SpaceUsed.ToString() + " bytes";
                Label3.Text = "<b>" + "Status: " + "</b>" + foundUser.Status.ToUpper();
                Label4.Text = "<b>" + "Last Login: " + "</b>" + foundUser.ModifiedAt.ToString();
                Label6.Text = "<b>" + "Email Alias #1: " + "</b>" + emailAliases.Entries[0];
                Label7.Text = "<b>" + "Email Alias #2: " + "</b>" + emails;
                Label5.Text = "<b>" + "Top Folders" + "</b>";
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



            if (found == false)
            {
                Exportbtn.Visible = true;
                InvalidEmailLabel.Visible = true;
                Label1.Text = "";
                Label2.Text = "";
                Label3.Text = "";
                Label4.Text = "";
                Label5.Text = "";
            }

            /* This shows all the users
              GridView1.DataSource = users;
              GridView1.DataBind();*/
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

        /*private async Task CurlFindContents(Uri uri, string contents)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Host = "box.com";
            Task<string> task = client.GetStringAsync("https://api.box.com/2.0/events")
        } */

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
            string last = button[button.Length - 1].ToString();
            int i = Convert.ToInt32(last);
            string input = lbl[i].Text.Substring(lbl[i].Text.IndexOf(": ") + 2);


            BoxAuthTest box = new BoxAuthTest();
            List<BoxUser> users = await box.GetallUsers();
            BoxUser foundUser = users.Find(u => u.Login.Equals(EmailAddress.Text.Trim()));
            BoxCollection<BoxEmailAlias> alia = await box.GetEmailAlias(foundUser.Id);

            await box.DeleteEmailAlias(foundUser.Id, string: emailAliasID);
            for (int x = 0; x < 1; x++)
            {
                lbl[6 + x].Visible = true;
                btn[6 + x].Visible = true;
            }
        }

        protected void AdminPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminMenu.aspx");
        }
    }
}
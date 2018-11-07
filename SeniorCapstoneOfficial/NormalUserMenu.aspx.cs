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
                RecentEventsButton.Visible = false;
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

            Label8.Visible = false;
            Button8.Visible = false;
            Label9.Visible = false;
            Button9.Visible = false;
            Label10.Visible = false;
            Button10.Visible = false;
            Label11.Visible = false;
            Button11.Visible = false;
            Label12.Visible = false;
            Button12.Visible = false;
            Label13.Visible = false;
            Button13.Visible = false;
            Label14.Visible = false;
            Button14.Visible = false;
            Label15.Visible = false;
            Button15.Visible = false;
            Label16.Visible = false;
            Button16.Visible = false;
            Label17.Visible = false;
            Button17.Visible = false;
            Label18.Visible = false;
            Button18.Visible = false;
            Label19.Visible = false;
            Button19.Visible = false;
            Label20.Visible = false;
            Button20.Visible = false;
            Label21.Visible = false;
            Button21.Visible = false;
            Label22.Visible = false;
            Button22.Visible = false;
            Label23.Visible = false;
            Button23.Visible = false;
            Label24.Visible = false;
            Button24.Visible = false;
            Label25.Visible = false;
            Button25.Visible = false;
            Label26.Visible = false;
            Button26.Visible = false;
            Label27.Visible = false;
            Button27.Visible = false;
            Label28.Visible = false;
            Button28.Visible = false;

            if (foundUser != null)
            {
                rightdiv.Style.Add("border-left", "dashed 1pt");
                rightdiv.Style.Add("border-left-color", "#033459");
                //string[] recent = await box.GetRecentEvents(foundUser.Id);
                RecentEventsButton.Visible = true;
                List<string> recent = box.GetRecentEvents(foundUser.Id);
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
                        Label temp1 = new Label();
                        Label temp3 = new Label();
                        Label temp2 = new Label();
                        temp.Text = "<b>" + eventnumber.ToString() + "</b>" + "." + " <b>Login:</b> " + logins[i];
                        temp1.Text = " <b>Type:</b> " + types[i];
                        temp3.Text = " <b>Created at:</b> " + create[i];
                        if (sourcenames[i] =="" || sourcetype[i] =="" || sourceid[i]=="")
                        {
                            temp2.Text = " <b>Source:</b> " + "Not Found";
                        }
                        else
                        temp2.Text = " <b>Source:</b> " + "(Type: " + sourcetype[i] + ", Id: " + sourceid[i] + ", Name: " + sourcenames[i] + ")";
                        temp1.Attributes.CssStyle.Add("display", "block");
                        temp1.Attributes.CssStyle.Add("clear", "right");
                        temp2.Attributes.CssStyle.Add("display", "block");
                        temp2.Attributes.CssStyle.Add("clear", "right");
                        temp3.Attributes.CssStyle.Add("display", "block");
                        temp3.Attributes.CssStyle.Add("clear", "right");
                        temp.Attributes.CssStyle.Add("display", "block");
                        temp.Attributes.CssStyle.Add("clear", "right");
                        temp.Attributes.CssStyle.Add("margin-top", "25px");
                        temp.Attributes.CssStyle.Add("text-align", "left");
                        temp1.Attributes.CssStyle.Add("text-align", "left");
                        temp2.Attributes.CssStyle.Add("text-align", "left");
                        temp3.Attributes.CssStyle.Add("text-align", "left");
                        temp.Attributes.CssStyle.Add("margin-left", "240px");
                        temp2.Attributes.CssStyle.Add("margin-left", "257px");
                        temp1.Attributes.CssStyle.Add("margin-left", "257px");
                        temp3.Attributes.CssStyle.Add("margin-left", "257px");
                        EventHolder.Controls.Add(temp);
                        EventHolder.Controls.Add(temp1);
                        EventHolder.Controls.Add(temp3);
                        EventHolder.Controls.Add(temp2);
                    }
                }
                /* display all the events in a raw format
              foreach(string a in recent)
              {
                  Label aaa = new Label();
                  aaa.Text = a;
                  EventHolder.Controls.Add(aaa);

              }*/

                var emailAliases = await box.GetEmailAlias(foundUser.Id);
                //var teste = box.GetRecentEvents(foundUser.Id);                

                TextBox1.Visible = true;
                Button7.Visible = true;


                Label1.Text = "<b>" + "Name: " + "</b>" + foundUser.Name;
                Label2.Text = "<b>" + "Space Used: " + "</b>" + foundUser.SpaceUsed.ToString() + " bytes";
                Label3.Text = "<b>" + "Status: " + "</b>" + foundUser.Status.ToUpper();
                Label4.Text = "<b>" + "Last Login: " + "</b>" + foundUser.ModifiedAt.ToString();
                Panel1.Visible = true;

                for (int i = 0; i < emailAliases.TotalCount; i++)
                {
                    /*Label label = new Label();
                    label.Text = emailAliases.Entries[i].Email + " ";
                    label.ID = "label" + (8 + i);
                    Button button = new Button();
                    button.ID = "button" + (8 + i);
                    button.Text = "Delete";
                    button.CausesValidation = true;
                    
                    button.Enabled = true;
                    button.UseSubmitBehavior = true;
                    button.CssClass += "DeleteUserButton";
                    Panel1.Controls.Add(label);
                    Panel1.Controls.Add(button);
                    button.OnClientClick = "return Validate();";
                    button.Click += new EventHandler(DeleteButton_Click);
                    Panel1.Controls.Add(new LiteralControl("<br />"));
                    */

                }

                switch (emailAliases.TotalCount)
                {
                    case 1:
                        {
                            Label8.Visible = true;
                            Label8.Text = "<b>" + "Email Alias #1: " + "</b>" + emailAliases.Entries[0].Email;
                            Button8.Visible = true;
                            lbl.Add(Label8);
                            break;
                        }
                    case 2:
                        {
                            Label8.Visible = true;
                            Button8.Visible = true;
                            Label9.Visible = true;
                            Button9.Visible = true;
                            Label8.Text = "<b>" + "Email Alias #1: </b>" + emailAliases.Entries[0].Email;
                            Label9.Text = "<b>" + "Email Alias #2: </b>" + emailAliases.Entries[1].Email;
                            lbl.Add(Label8);
                            lbl.Add(Label9);
                            break;
                        }
                    case 3:
                        {
                            Label8.Visible = true;
                            Button8.Visible = true;
                            Label9.Visible = true;
                            Button9.Visible = true;
                            Label10.Visible = true;
                            Button10.Visible = true;
                            Label8.Text = "<b>" + "Email Alias #1: </b>" + emailAliases.Entries[0].Email;
                            Label9.Text = "<b>" + "Email Alias #2: </b>" + emailAliases.Entries[1].Email;
                            Label10.Text = "<b>" + "Email Alias #3: </b>" + emailAliases.Entries[2].Email;
                            lbl.Add(Label8);
                            lbl.Add(Label9);
                            lbl.Add(Label10);
                            break;
                        }
                    case 4:
                        {
                            Label8.Visible = true;
                            Button8.Visible = true;
                            Label9.Visible = true;
                            Button9.Visible = true;
                            Label10.Visible = true;
                            Button10.Visible = true;
                            Label11.Visible = true;
                            Button11.Visible = true;
                            Label8.Text = "<b>" + "Email Alias #1: </b>" + emailAliases.Entries[0].Email;
                            Label9.Text = "<b>" + "Email Alias #2: </b>" + emailAliases.Entries[1].Email;
                            Label10.Text = "<b>" + "Email Alias #3 </b>" + emailAliases.Entries[2].Email;
                            Label11.Text = "<b>" + "Email Alias #4: </b>" + emailAliases.Entries[3].Email;
                            lbl.Add(Label8);
                            lbl.Add(Label9);
                            lbl.Add(Label10);
                            lbl.Add(Label11);
                            break;
                        }
                    case 5:
                        {
                            Label8.Visible = true;
                            Button8.Visible = true;
                            Label9.Visible = true;
                            Button9.Visible = true;
                            Label10.Visible = true;
                            Button10.Visible = true;
                            Label11.Visible = true;
                            Button11.Visible = true;
                            Label12.Visible = true;
                            Button12.Visible = true;
                            Label8.Text = "<b>" + "Email Alias #1: </b>" + emailAliases.Entries[0].Email;
                            Label9.Text = "<b>" + "Email Alias #2: </b>" + emailAliases.Entries[1].Email;
                            Label10.Text = "<b>" + "Email Alias #3: </b>" + emailAliases.Entries[2].Email;
                            Label11.Text = "<b>" + "Email Alias #4: </b>" + emailAliases.Entries[3].Email;
                            Label12.Text = "<b>" + "Email Alias #5: </b>" + emailAliases.Entries[4].Email;
                            lbl.Add(Label8);
                            lbl.Add(Label9);
                            lbl.Add(Label10);
                            lbl.Add(Label11);
                            lbl.Add(Label12);
                            break;
                        }
                    case 6:
                        {
                            Label8.Visible = true;
                            Button8.Visible = true;
                            Label9.Visible = true;
                            Button9.Visible = true;
                            Label10.Visible = true;
                            Button10.Visible = true;
                            Label11.Visible = true;
                            Button11.Visible = true;
                            Label12.Visible = true;
                            Button12.Visible = true;
                            Label13.Visible = true;
                            Button13.Visible = true;
                            Label8.Text = "<b>" + "Email Alias #1: </b>" + emailAliases.Entries[0].Email;
                            Label9.Text = "<b>" + "Email Alias #2: </b>" + emailAliases.Entries[1].Email;
                            Label10.Text = "<b>" + "Email Alias #3: </b>" + emailAliases.Entries[2].Email;
                            Label11.Text = "<b>" + "Email Alias #4: </b>" + emailAliases.Entries[3].Email;
                            Label12.Text = "<b>" + "Email Alias #5: </b>" + emailAliases.Entries[4].Email;
                            Label13.Text = "<b>" + "Email Alias #6: </b>" + emailAliases.Entries[5].Email;
                            lbl.Add(Label8);
                            lbl.Add(Label9);
                            lbl.Add(Label10);
                            lbl.Add(Label11);
                            lbl.Add(Label12);
                            lbl.Add(Label13);
                            break;
                        }
                    case 7:
                        {
                            Label8.Visible = true;
                            Button8.Visible = true;
                            Label9.Visible = true;
                            Button9.Visible = true;
                            Label10.Visible = true;
                            Button10.Visible = true;
                            Label11.Visible = true;
                            Button11.Visible = true;
                            Label12.Visible = true;
                            Button12.Visible = true;
                            Label13.Visible = true;
                            Button13.Visible = true;
                            Label14.Visible = true;
                            Button14.Visible = true;
                            Label8.Text = "<b>" + "Email Alias #1: </b>" + emailAliases.Entries[0].Email;
                            Label9.Text = "<b>" + "Email Alias #2: </b>" + emailAliases.Entries[1].Email;
                            Label10.Text = "<b>" + "Email Alias #3: </b>" + emailAliases.Entries[2].Email;
                            Label11.Text = "<b>" + "Email Alias #4: </b>" + emailAliases.Entries[3].Email;
                            Label12.Text = "<b>" + "Email Alias #5: </b>" + emailAliases.Entries[4].Email;
                            Label13.Text = "<b>" + "Email Alias #6: </b>" + emailAliases.Entries[5].Email;
                            Label14.Text = "<b>" + "Email Alias #7: </b>" + emailAliases.Entries[6].Email;
                            lbl.Add(Label8);
                            lbl.Add(Label9);
                            lbl.Add(Label10);
                            lbl.Add(Label11);
                            lbl.Add(Label12);
                            lbl.Add(Label13);
                            lbl.Add(Label14);
                            break;
                        }
                    case 8:
                        {
                            Label8.Visible = true;
                            Button8.Visible = true;
                            Label9.Visible = true;
                            Button9.Visible = true;
                            Label10.Visible = true;
                            Button10.Visible = true;
                            Label11.Visible = true;
                            Button11.Visible = true;
                            Label12.Visible = true;
                            Button12.Visible = true;
                            Label13.Visible = true;
                            Button13.Visible = true;
                            Label14.Visible = true;
                            Button14.Visible = true;
                            Label15.Visible = true;
                            Button15.Visible = true;
                            Label8.Text = "<b>" + "Email Alias #1: </b>" + emailAliases.Entries[0].Email;
                            Label9.Text = "<b>" + "Email Alias #2: </b>" + emailAliases.Entries[1].Email;
                            Label10.Text = "<b>" + "Email Alias #3: </b>" + emailAliases.Entries[2].Email;
                            Label11.Text = "<b>" + "Email Alias #4: </b>" + emailAliases.Entries[3].Email;
                            Label12.Text = "<b>" + "Email Alias #5: </b>" + emailAliases.Entries[4].Email;
                            Label13.Text = "<b>" + "Email Alias #6: </b>" + emailAliases.Entries[5].Email;
                            Label14.Text = "<b>" + "Email Alias #7: </b>" + emailAliases.Entries[6].Email;
                            Label15.Text = "<b>" + "Email Alias #8: </b>" + emailAliases.Entries[7].Email;
                            lbl.Add(Label8);
                            lbl.Add(Label9);
                            lbl.Add(Label10);
                            lbl.Add(Label11);
                            lbl.Add(Label12);
                            lbl.Add(Label13);
                            lbl.Add(Label14);
                            lbl.Add(Label15);
                            break;
                        }
                    case 9:
                        {
                            Label8.Visible = true;
                            Button8.Visible = true;
                            Label9.Visible = true;
                            Button9.Visible = true;
                            Label10.Visible = true;
                            Button10.Visible = true;
                            Label11.Visible = true;
                            Button11.Visible = true;
                            Label12.Visible = true;
                            Button12.Visible = true;
                            Label13.Visible = true;
                            Button13.Visible = true;
                            Label14.Visible = true;
                            Button14.Visible = true;
                            Label15.Visible = true;
                            Button15.Visible = true;
                            Label16.Visible = true;
                            Button16.Visible = true;
                            Label8.Text = "<b>" + "Email Alias #1: </b>" + emailAliases.Entries[0].Email;
                            Label9.Text = "<b>" + "Email Alias #2: </b>" + emailAliases.Entries[1].Email;
                            Label10.Text = "<b>" + "Email Alias #3: </b>" + emailAliases.Entries[2].Email;
                            Label11.Text = "<b>" + "Email Alias #4: </b>" + emailAliases.Entries[3].Email;
                            Label12.Text = "<b>" + "Email Alias #5: </b>" + emailAliases.Entries[4].Email;
                            Label13.Text = "<b>" + "Email Alias #6: </b>" + emailAliases.Entries[5].Email;
                            Label14.Text = "<b>" + "Email Alias #7: </b>" + emailAliases.Entries[6].Email;
                            Label15.Text = "<b>" + "Email Alias #8: </b>" + emailAliases.Entries[7].Email;
                            Label16.Text = "<b>" + "Email Alias #9: </b>" + emailAliases.Entries[8].Email;
                            lbl.Add(Label8);
                            lbl.Add(Label9);
                            lbl.Add(Label10);
                            lbl.Add(Label11);
                            lbl.Add(Label12);
                            lbl.Add(Label13);
                            lbl.Add(Label14);
                            lbl.Add(Label15);
                            lbl.Add(Label16);
                            break;
                        }
                    case 10:
                        {
                            Label8.Visible = true;
                            Button8.Visible = true;
                            Label9.Visible = true;
                            Button9.Visible = true;
                            Label10.Visible = true;
                            Button10.Visible = true;
                            Label11.Visible = true;
                            Button11.Visible = true;
                            Label12.Visible = true;
                            Button12.Visible = true;
                            Label13.Visible = true;
                            Button13.Visible = true;
                            Label14.Visible = true;
                            Button14.Visible = true;
                            Label15.Visible = true;
                            Button15.Visible = true;
                            Label16.Visible = true;
                            Button16.Visible = true;
                            Label17.Visible = true;
                            Button17.Visible = true;
                            Label8.Text = "<b>" + "Email Alias #1: </b>" + emailAliases.Entries[0].Email;
                            Label9.Text = "<b>" + "Email Alias #2: </b>" + emailAliases.Entries[1].Email;
                            Label10.Text = "<b>" + "Email Alias #3: </b>" + emailAliases.Entries[2].Email;
                            Label11.Text = "<b>" + "Email Alias #4: </b>" + emailAliases.Entries[3].Email;
                            Label12.Text = "<b>" + "Email Alias #5: </b>" + emailAliases.Entries[4].Email;
                            Label13.Text = "<b>" + "Email Alias #6: </b>" + emailAliases.Entries[5].Email;
                            Label14.Text = "<b>" + "Email Alias #7: </b>" + emailAliases.Entries[6].Email;
                            Label15.Text = "<b>" + "Email Alias #8: </b>" + emailAliases.Entries[7].Email;
                            Label16.Text = "<b>" + "Email Alias #9: </b>" + emailAliases.Entries[8].Email;
                            Label17.Text = "<b>" + "Email Alias #10: </b>" + emailAliases.Entries[9].Email;
                            lbl.Add(Label8);
                            lbl.Add(Label9);
                            lbl.Add(Label10);
                            lbl.Add(Label11);
                            lbl.Add(Label12);
                            lbl.Add(Label13);
                            lbl.Add(Label14);
                            lbl.Add(Label15);
                            lbl.Add(Label16);
                            lbl.Add(Label17);
                            break;
                        }
                    case 11:
                        {
                            Label8.Visible = true;
                            Button8.Visible = true;
                            Label9.Visible = true;
                            Button9.Visible = true;
                            Label10.Visible = true;
                            Button10.Visible = true;
                            Label11.Visible = true;
                            Button11.Visible = true;
                            Label12.Visible = true;
                            Button12.Visible = true;
                            Label13.Visible = true;
                            Button13.Visible = true;
                            Label14.Visible = true;
                            Button14.Visible = true;
                            Label15.Visible = true;
                            Button15.Visible = true;
                            Label16.Visible = true;
                            Button16.Visible = true;
                            Label17.Visible = true;
                            Button17.Visible = true;
                            Label18.Visible = true;
                            Button18.Visible = true;
                            Label8.Text = "<b>" + "Email Alias #1: </b>" + emailAliases.Entries[0].Email;
                            Label9.Text = "<b>" + "Email Alias #2: </b>" + emailAliases.Entries[1].Email;
                            Label10.Text = "<b>" + "Email Alias #3: </b>" + emailAliases.Entries[2].Email;
                            Label11.Text = "<b>" + "Email Alias #4: </b>" + emailAliases.Entries[3].Email;
                            Label12.Text = "<b>" + "Email Alias #5: </b>" + emailAliases.Entries[4].Email;
                            Label13.Text = "<b>" + "Email Alias #6: </b>" + emailAliases.Entries[5].Email;
                            Label14.Text = "<b>" + "Email Alias #7: </b>" + emailAliases.Entries[6].Email;
                            Label15.Text = "<b>" + "Email Alias #7: </b>" + emailAliases.Entries[6].Email;
                            Label16.Text = "<b>" + "Email Alias #8: </b>" + emailAliases.Entries[7].Email;
                            Label17.Text = "<b>" + "Email Alias #9: </b>" + emailAliases.Entries[8].Email;
                            Label18.Text = "<b>" + "Email Alias #10: </b>" + emailAliases.Entries[9].Email;
                            lbl.Add(Label8);
                            lbl.Add(Label9);
                            lbl.Add(Label10);
                            lbl.Add(Label11);
                            lbl.Add(Label12);
                            lbl.Add(Label13);
                            lbl.Add(Label14);
                            lbl.Add(Label15);
                            lbl.Add(Label16);
                            lbl.Add(Label17);
                            lbl.Add(Label18);
                            break;
                        }
                    case 12:
                        {
                            Label8.Visible = true;
                            Button8.Visible = true;
                            Label9.Visible = true;
                            Button9.Visible = true;
                            Label10.Visible = true;
                            Button10.Visible = true;
                            Label11.Visible = true;
                            Button11.Visible = true;
                            Label12.Visible = true;
                            Button12.Visible = true;
                            Label13.Visible = true;
                            Button13.Visible = true;
                            Label14.Visible = true;
                            Button14.Visible = true;
                            Label15.Visible = true;
                            Button15.Visible = true;
                            Label16.Visible = true;
                            Button16.Visible = true;
                            Label17.Visible = true;
                            Button17.Visible = true;
                            Label18.Visible = true;
                            Button18.Visible = true;
                            Label19.Visible = true;
                            Button19.Visible = true;
                            Label8.Text = "<b>" + "Email Alias #1: </b>" + emailAliases.Entries[0].Email;
                            Label9.Text = "<b>" + "Email Alias #2: </b>" + emailAliases.Entries[1].Email;
                            Label10.Text = "<b>" + "Email Alias #3: </b>" + emailAliases.Entries[2].Email;
                            Label11.Text = "<b>" + "Email Alias #4: </b>" + emailAliases.Entries[3].Email;
                            Label12.Text = "<b>" + "Email Alias #5: </b>" + emailAliases.Entries[4].Email;
                            Label13.Text = "<b>" + "Email Alias #6: </b>" + emailAliases.Entries[5].Email;
                            Label14.Text = "<b>" + "Email Alias #7: </b>" + emailAliases.Entries[6].Email;
                            Label15.Text = "<b>" + "Email Alias #8: </b>" + emailAliases.Entries[7].Email;
                            Label16.Text = "<b>" + "Email Alias #9: </b>" + emailAliases.Entries[8].Email;
                            Label17.Text = "<b>" + "Email Alias #10: </b>" + emailAliases.Entries[9].Email;
                            Label18.Text = "<b>" + "Email Alias #11: </b>" + emailAliases.Entries[10].Email;
                            Label19.Text = "<b>" + "Email Alias #12: </b>" + emailAliases.Entries[11].Email;
                            lbl.Add(Label8);
                            lbl.Add(Label9);
                            lbl.Add(Label10);
                            lbl.Add(Label11);
                            lbl.Add(Label12);
                            lbl.Add(Label13);
                            lbl.Add(Label14);
                            lbl.Add(Label15);
                            lbl.Add(Label16);
                            lbl.Add(Label17);
                            lbl.Add(Label18);
                            lbl.Add(Label19);
                            break;
                        }
                    case 13:
                        {
                            Label8.Visible = true;
                            Button8.Visible = true;
                            Label9.Visible = true;
                            Button9.Visible = true;
                            Label10.Visible = true;
                            Button10.Visible = true;
                            Label11.Visible = true;
                            Button11.Visible = true;
                            Label12.Visible = true;
                            Button12.Visible = true;
                            Label13.Visible = true;
                            Button13.Visible = true;
                            Label14.Visible = true;
                            Button14.Visible = true;
                            Label15.Visible = true;
                            Button15.Visible = true;
                            Label16.Visible = true;
                            Button16.Visible = true;
                            Label17.Visible = true;
                            Button17.Visible = true;
                            Label18.Visible = true;
                            Button18.Visible = true;
                            Label19.Visible = true;
                            Button19.Visible = true;
                            Label20.Visible = true;
                            Button20.Visible = true;
                            Label8.Text = "<b>" + "Email Alias #1: </b>" + emailAliases.Entries[0].Email;
                            Label9.Text = "<b>" + "Email Alias #2: </b>" + emailAliases.Entries[1].Email;
                            Label10.Text = "<b>" + "Email Alias #3: </b>" + emailAliases.Entries[2].Email;
                            Label11.Text = "<b>" + "Email Alias #4: </b>" + emailAliases.Entries[3].Email;
                            Label12.Text = "<b>" + "Email Alias #5: </b>" + emailAliases.Entries[4].Email;
                            Label13.Text = "<b>" + "Email Alias #6: </b>" + emailAliases.Entries[5].Email;
                            Label14.Text = "<b>" + "Email Alias #7: </b>" + emailAliases.Entries[6].Email;
                            Label15.Text = "<b>" + "Email Alias #8: </b>" + emailAliases.Entries[7].Email;
                            Label16.Text = "<b>" + "Email Alias #9: </b>" + emailAliases.Entries[8].Email;
                            Label17.Text = "<b>" + "Email Alias #10: </b>" + emailAliases.Entries[9].Email;
                            Label18.Text = "<b>" + "Email Alias #11: </b>" + emailAliases.Entries[10].Email;
                            Label19.Text = "<b>" + "Email Alias #12: </b>" + emailAliases.Entries[11].Email;
                            Label20.Text = "<b>" + "Email Alias #13: </b>" + emailAliases.Entries[12].Email;
                            lbl.Add(Label8);
                            lbl.Add(Label9);
                            lbl.Add(Label10);
                            lbl.Add(Label11);
                            lbl.Add(Label12);
                            lbl.Add(Label13);
                            lbl.Add(Label14);
                            lbl.Add(Label15);
                            lbl.Add(Label16);
                            lbl.Add(Label17);
                            lbl.Add(Label18);
                            lbl.Add(Label19);
                            lbl.Add(Label20);
                            break;
                        }
                    case 14:
                        {
                            Label8.Visible = true;
                            Button8.Visible = true;
                            Label9.Visible = true;
                            Button9.Visible = true;
                            Label10.Visible = true;
                            Button10.Visible = true;
                            Label11.Visible = true;
                            Button11.Visible = true;
                            Label12.Visible = true;
                            Button12.Visible = true;
                            Label13.Visible = true;
                            Button13.Visible = true;
                            Label14.Visible = true;
                            Button14.Visible = true;
                            Label15.Visible = true;
                            Button15.Visible = true;
                            Label16.Visible = true;
                            Button16.Visible = true;
                            Label17.Visible = true;
                            Button17.Visible = true;
                            Label18.Visible = true;
                            Button18.Visible = true;
                            Label19.Visible = true;
                            Button19.Visible = true;
                            Label20.Visible = true;
                            Button20.Visible = true;
                            Label21.Visible = true;
                            Button21.Visible = true;
                            Label8.Text = "<b>" + "Email Alias #1: </b>" + emailAliases.Entries[0].Email;
                            Label9.Text = "<b>" + "Email Alias #2: </b>" + emailAliases.Entries[1].Email;
                            Label10.Text = "<b>" + "Email Alias #3: </b>" + emailAliases.Entries[2].Email;
                            Label11.Text = "<b>" + "Email Alias #4: </b>" + emailAliases.Entries[3].Email;
                            Label12.Text = "<b>" + "Email Alias #5: </b>" + emailAliases.Entries[4].Email;
                            Label13.Text = "<b>" + "Email Alias #6: </b>" + emailAliases.Entries[5].Email;
                            Label14.Text = "<b>" + "Email Alias #7: </b>" + emailAliases.Entries[6].Email;
                            Label15.Text = "<b>" + "Email Alias #8: </b>" + emailAliases.Entries[7].Email;
                            Label16.Text = "<b>" + "Email Alias #9: </b>" + emailAliases.Entries[8].Email;
                            Label17.Text = "<b>" + "Email Alias #10: </b>" + emailAliases.Entries[9].Email;
                            Label18.Text = "<b>" + "Email Alias #11: </b>" + emailAliases.Entries[10].Email;
                            Label19.Text = "<b>" + "Email Alias #12: </b>" + emailAliases.Entries[11].Email;
                            Label20.Text = "<b>" + "Email Alias #13: </b>" + emailAliases.Entries[12].Email;
                            Label21.Text = "<b>" + "Email Alias #14: </b>" + emailAliases.Entries[13].Email;
                            lbl.Add(Label8);
                            lbl.Add(Label9);
                            lbl.Add(Label10);
                            lbl.Add(Label11);
                            lbl.Add(Label12);
                            lbl.Add(Label13);
                            lbl.Add(Label14);
                            lbl.Add(Label15);
                            lbl.Add(Label16);
                            lbl.Add(Label17);
                            lbl.Add(Label18);
                            lbl.Add(Label19);
                            lbl.Add(Label20);
                            lbl.Add(Label21);
                            break;
                        }
                    case 15:
                        {
                            Label8.Visible = true;
                            Button8.Visible = true;
                            Label9.Visible = true;
                            Button9.Visible = true;
                            Label10.Visible = true;
                            Button10.Visible = true;
                            Label11.Visible = true;
                            Button11.Visible = true;
                            Label12.Visible = true;
                            Button12.Visible = true;
                            Label13.Visible = true;
                            Button13.Visible = true;
                            Label14.Visible = true;
                            Button14.Visible = true;
                            Label15.Visible = true;
                            Button15.Visible = true;
                            Label16.Visible = true;
                            Button16.Visible = true;
                            Label17.Visible = true;
                            Button17.Visible = true;
                            Label18.Visible = true;
                            Button18.Visible = true;
                            Label19.Visible = true;
                            Button19.Visible = true;
                            Label20.Visible = true;
                            Button20.Visible = true;
                            Label21.Visible = true;
                            Button21.Visible = true;
                            Label22.Visible = true;
                            Button22.Visible = true;
                            Label8.Text = "<b>" + "Email Alias #1: </b>" + emailAliases.Entries[0].Email;
                            Label9.Text = "<b>" + "Email Alias #2: </b>" + emailAliases.Entries[1].Email;
                            Label10.Text = "<b>" + "Email Alias #3: </b>" + emailAliases.Entries[2].Email;
                            Label11.Text = "<b>" + "Email Alias #4: </b>" + emailAliases.Entries[3].Email;
                            Label12.Text = "<b>" + "Email Alias #5: </b>" + emailAliases.Entries[4].Email;
                            Label13.Text = "<b>" + "Email Alias #6: </b>" + emailAliases.Entries[5].Email;
                            Label14.Text = "<b>" + "Email Alias #7: </b>" + emailAliases.Entries[6].Email;
                            Label15.Text = "<b>" + "Email Alias #8: </b>" + emailAliases.Entries[7].Email;
                            Label16.Text = "<b>" + "Email Alias #9: </b>" + emailAliases.Entries[8].Email;
                            Label17.Text = "<b>" + "Email Alias #10: </b>" + emailAliases.Entries[9].Email;
                            Label18.Text = "<b>" + "Email Alias #11: </b>" + emailAliases.Entries[10].Email;
                            Label19.Text = "<b>" + "Email Alias #12: </b>" + emailAliases.Entries[11].Email;
                            Label20.Text = "<b>" + "Email Alias #13: </b>" + emailAliases.Entries[12].Email;
                            Label21.Text = "<b>" + "Email Alias #14: </b>" + emailAliases.Entries[13].Email;
                            Label22.Text = "<b>" + "Email Alias #15: </b>" + emailAliases.Entries[14].Email;
                            lbl.Add(Label8);
                            lbl.Add(Label9);
                            lbl.Add(Label10);
                            lbl.Add(Label11);
                            lbl.Add(Label12);
                            lbl.Add(Label13);
                            lbl.Add(Label14);
                            lbl.Add(Label15);
                            lbl.Add(Label16);
                            lbl.Add(Label17);
                            lbl.Add(Label18);
                            lbl.Add(Label19);
                            lbl.Add(Label20);
                            lbl.Add(Label21);
                            lbl.Add(Label22);
                            break;
                        }
                    case 16:
                        {
                            Label8.Visible = true;
                            Button8.Visible = true;
                            Label9.Visible = true;
                            Button9.Visible = true;
                            Label10.Visible = true;
                            Button10.Visible = true;
                            Label11.Visible = true;
                            Button11.Visible = true;
                            Label12.Visible = true;
                            Button12.Visible = true;
                            Label13.Visible = true;
                            Button13.Visible = true;
                            Label14.Visible = true;
                            Button14.Visible = true;
                            Label15.Visible = true;
                            Button15.Visible = true;
                            Label16.Visible = true;
                            Button16.Visible = true;
                            Label17.Visible = true;
                            Button17.Visible = true;
                            Label18.Visible = true;
                            Button18.Visible = true;
                            Label19.Visible = true;
                            Button19.Visible = true;
                            Label20.Visible = true;
                            Button20.Visible = true;
                            Label21.Visible = true;
                            Button21.Visible = true;
                            Label22.Visible = true;
                            Button22.Visible = true;
                            Label23.Visible = true;
                            Button23.Visible = true;
                            Label8.Text = "<b>" + "Email Alias #1: </b>" + emailAliases.Entries[0].Email;
                            Label9.Text = "<b>" + "Email Alias #2: </b>" + emailAliases.Entries[1].Email;
                            Label10.Text = "<b>" + "Email Alias #3: </b>" + emailAliases.Entries[2].Email;
                            Label11.Text = "<b>" + "Email Alias #4: </b>" + emailAliases.Entries[3].Email;
                            Label12.Text = "<b>" + "Email Alias #5: </b>" + emailAliases.Entries[4].Email;
                            Label13.Text = "<b>" + "Email Alias #6: </b>" + emailAliases.Entries[5].Email;
                            Label14.Text = "<b>" + "Email Alias #7: </b>" + emailAliases.Entries[6].Email;
                            Label15.Text = "<b>" + "Email Alias #8: </b>" + emailAliases.Entries[7].Email;
                            Label16.Text = "<b>" + "Email Alias #9: </b>" + emailAliases.Entries[8].Email;
                            Label17.Text = "<b>" + "Email Alias #10: </b>" + emailAliases.Entries[9].Email;
                            Label18.Text = "<b>" + "Email Alias #11: </b>" + emailAliases.Entries[10].Email;
                            Label19.Text = "<b>" + "Email Alias #12: </b>" + emailAliases.Entries[11].Email;
                            Label20.Text = "<b>" + "Email Alias #13: </b>" + emailAliases.Entries[12].Email;
                            Label21.Text = "<b>" + "Email Alias #14: </b>" + emailAliases.Entries[13].Email;
                            Label22.Text = "<b>" + "Email Alias #15: </b>" + emailAliases.Entries[14].Email;
                            Label23.Text = "<b>" + "Email Alias #16: </b>" + emailAliases.Entries[15].Email;
                            lbl.Add(Label8);
                            lbl.Add(Label9);
                            lbl.Add(Label10);
                            lbl.Add(Label11);
                            lbl.Add(Label12);
                            lbl.Add(Label13);
                            lbl.Add(Label14);
                            lbl.Add(Label15);
                            lbl.Add(Label16);
                            lbl.Add(Label17);
                            lbl.Add(Label18);
                            lbl.Add(Label19);
                            lbl.Add(Label20);
                            lbl.Add(Label21);
                            lbl.Add(Label22);
                            lbl.Add(Label23);
                            break;
                        }
                    case 17:
                        {
                            Label8.Visible = true;
                            Button8.Visible = true;
                            Label9.Visible = true;
                            Button9.Visible = true;
                            Label10.Visible = true;
                            Button10.Visible = true;
                            Label11.Visible = true;
                            Button11.Visible = true;
                            Label12.Visible = true;
                            Button12.Visible = true;
                            Label13.Visible = true;
                            Button13.Visible = true;
                            Label14.Visible = true;
                            Button14.Visible = true;
                            Label15.Visible = true;
                            Button15.Visible = true;
                            Label16.Visible = true;
                            Button16.Visible = true;
                            Label17.Visible = true;
                            Button17.Visible = true;
                            Label18.Visible = true;
                            Button18.Visible = true;
                            Label19.Visible = true;
                            Button19.Visible = true;
                            Label20.Visible = true;
                            Button20.Visible = true;
                            Label21.Visible = true;
                            Button21.Visible = true;
                            Label22.Visible = true;
                            Button22.Visible = true;
                            Label23.Visible = true;
                            Button23.Visible = true;
                            Label24.Visible = true;
                            Button24.Visible = true;
                            Label8.Text = "<b>" + "Email Alias #1: </b>" + emailAliases.Entries[0].Email;
                            Label9.Text = "<b>" + "Email Alias #2: </b>" + emailAliases.Entries[1].Email;
                            Label10.Text = "<b>" + "Email Alias #3: </b>" + emailAliases.Entries[2].Email;
                            Label11.Text = "<b>" + "Email Alias #4: </b>" + emailAliases.Entries[3].Email;
                            Label12.Text = "<b>" + "Email Alias #5: </b>" + emailAliases.Entries[4].Email;
                            Label13.Text = "<b>" + "Email Alias #6: </b>" + emailAliases.Entries[5].Email;
                            Label14.Text = "<b>" + "Email Alias #7: </b>" + emailAliases.Entries[6].Email;
                            Label15.Text = "<b>" + "Email Alias #8: </b>" + emailAliases.Entries[7].Email;
                            Label16.Text = "<b>" + "Email Alias #9: </b>" + emailAliases.Entries[8].Email;
                            Label17.Text = "<b>" + "Email Alias #10: </b>" + emailAliases.Entries[9].Email;
                            Label18.Text = "<b>" + "Email Alias #11: </b>" + emailAliases.Entries[10].Email;
                            Label19.Text = "<b>" + "Email Alias #12: </b>" + emailAliases.Entries[11].Email;
                            Label20.Text = "<b>" + "Email Alias #13: </b>" + emailAliases.Entries[12].Email;
                            Label21.Text = "<b>" + "Email Alias #14: </b>" + emailAliases.Entries[13].Email;
                            Label22.Text = "<b>" + "Email Alias #15: </b>" + emailAliases.Entries[14].Email;
                            Label23.Text = "<b>" + "Email Alias #16: </b>" + emailAliases.Entries[15].Email;
                            Label24.Text = "<b>" + "Email Alias #17: </b>" + emailAliases.Entries[16].Email;
                            lbl.Add(Label8);
                            lbl.Add(Label9);
                            lbl.Add(Label10);
                            lbl.Add(Label11);
                            lbl.Add(Label12);
                            lbl.Add(Label13);
                            lbl.Add(Label14);
                            lbl.Add(Label15);
                            lbl.Add(Label16);
                            lbl.Add(Label17);
                            lbl.Add(Label18);
                            lbl.Add(Label19);
                            lbl.Add(Label20);
                            lbl.Add(Label21);
                            lbl.Add(Label22);
                            lbl.Add(Label23);
                            lbl.Add(Label24);
                            break;
                        }
                    case 18:
                        {
                            Label8.Visible = true;
                            Button8.Visible = true;
                            Label9.Visible = true;
                            Button9.Visible = true;
                            Label10.Visible = true;
                            Button10.Visible = true;
                            Label11.Visible = true;
                            Button11.Visible = true;
                            Label12.Visible = true;
                            Button12.Visible = true;
                            Label13.Visible = true;
                            Button13.Visible = true;
                            Label14.Visible = true;
                            Button14.Visible = true;
                            Label15.Visible = true;
                            Button15.Visible = true;
                            Label16.Visible = true;
                            Button16.Visible = true;
                            Label17.Visible = true;
                            Button17.Visible = true;
                            Label18.Visible = true;
                            Button18.Visible = true;
                            Label19.Visible = true;
                            Button19.Visible = true;
                            Label20.Visible = true;
                            Button20.Visible = true;
                            Label21.Visible = true;
                            Button21.Visible = true;
                            Label22.Visible = true;
                            Button22.Visible = true;
                            Label23.Visible = true;
                            Button23.Visible = true;
                            Label24.Visible = true;
                            Button24.Visible = true;
                            Label25.Visible = true;
                            Button25.Visible = true;
                            Label8.Text = "<b>" + "Email Alias #1: </b>" + emailAliases.Entries[0].Email;
                            Label9.Text = "<b>" + "Email Alias #2: </b>" + emailAliases.Entries[1].Email;
                            Label10.Text = "<b>" + "Email Alias #3: </b>" + emailAliases.Entries[2].Email;
                            Label11.Text = "<b>" + "Email Alias #4: </b>" + emailAliases.Entries[3].Email;
                            Label12.Text = "<b>" + "Email Alias #5: </b>" + emailAliases.Entries[4].Email;
                            Label13.Text = "<b>" + "Email Alias #6: </b>" + emailAliases.Entries[5].Email;
                            Label14.Text = "<b>" + "Email Alias #7: </b>" + emailAliases.Entries[6].Email;
                            Label15.Text = "<b>" + "Email Alias #8: </b>" + emailAliases.Entries[7].Email;
                            Label16.Text = "<b>" + "Email Alias #9: </b>" + emailAliases.Entries[8].Email;
                            Label17.Text = "<b>" + "Email Alias #10: </b>" + emailAliases.Entries[9].Email;
                            Label18.Text = "<b>" + "Email Alias #11: </b>" + emailAliases.Entries[10].Email;
                            Label19.Text = "<b>" + "Email Alias #12: </b>" + emailAliases.Entries[11].Email;
                            Label20.Text = "<b>" + "Email Alias #13: </b>" + emailAliases.Entries[12].Email;
                            Label21.Text = "<b>" + "Email Alias #14: </b>" + emailAliases.Entries[13].Email;
                            Label22.Text = "<b>" + "Email Alias #15: </b>" + emailAliases.Entries[14].Email;
                            Label23.Text = "<b>" + "Email Alias #16: </b>" + emailAliases.Entries[15].Email;
                            Label24.Text = "<b>" + "Email Alias #17: </b>" + emailAliases.Entries[16].Email;
                            Label25.Text = "<b>" + "Email Alias #18: </b>" + emailAliases.Entries[17].Email;
                            lbl.Add(Label8);
                            lbl.Add(Label9);
                            lbl.Add(Label10);
                            lbl.Add(Label11);
                            lbl.Add(Label12);
                            lbl.Add(Label13);
                            lbl.Add(Label14);
                            lbl.Add(Label15);
                            lbl.Add(Label16);
                            lbl.Add(Label17);
                            lbl.Add(Label18);
                            lbl.Add(Label19);
                            lbl.Add(Label20);
                            lbl.Add(Label21);
                            lbl.Add(Label22);
                            lbl.Add(Label23);
                            lbl.Add(Label24);
                            lbl.Add(Label25);
                            break;
                        }
                    case 19:
                        {
                            Label8.Visible = true;
                            Button8.Visible = true;
                            Label9.Visible = true;
                            Button9.Visible = true;
                            Label10.Visible = true;
                            Button10.Visible = true;
                            Label11.Visible = true;
                            Button11.Visible = true;
                            Label12.Visible = true;
                            Button12.Visible = true;
                            Label13.Visible = true;
                            Button13.Visible = true;
                            Label14.Visible = true;
                            Button14.Visible = true;
                            Label15.Visible = true;
                            Button15.Visible = true;
                            Label16.Visible = true;
                            Button16.Visible = true;
                            Label17.Visible = true;
                            Button17.Visible = true;
                            Label18.Visible = true;
                            Button18.Visible = true;
                            Label19.Visible = true;
                            Button19.Visible = true;
                            Label20.Visible = true;
                            Button20.Visible = true;
                            Label21.Visible = true;
                            Button21.Visible = true;
                            Label22.Visible = true;
                            Button22.Visible = true;
                            Label23.Visible = true;
                            Button23.Visible = true;
                            Label24.Visible = true;
                            Button24.Visible = true;
                            Label25.Visible = true;
                            Button25.Visible = true;
                            Label26.Visible = true;
                            Button26.Visible = true;
                            Label8.Text = "<b>" + "Email Alias #1: </b>" + emailAliases.Entries[0].Email;
                            Label9.Text = "<b>" + "Email Alias #2: </b>" + emailAliases.Entries[1].Email;
                            Label10.Text = "<b>" + "Email Alias #3: </b>" + emailAliases.Entries[2].Email;
                            Label11.Text = "<b>" + "Email Alias #4: </b>" + emailAliases.Entries[3].Email;
                            Label12.Text = "<b>" + "Email Alias #5: </b>" + emailAliases.Entries[4].Email;
                            Label13.Text = "<b>" + "Email Alias #6: </b>" + emailAliases.Entries[5].Email;
                            Label14.Text = "<b>" + "Email Alias #7: </b>" + emailAliases.Entries[6].Email;
                            Label15.Text = "<b>" + "Email Alias #8: </b>" + emailAliases.Entries[7].Email;
                            Label16.Text = "<b>" + "Email Alias #9: </b>" + emailAliases.Entries[8].Email;
                            Label17.Text = "<b>" + "Email Alias #10: </b>" + emailAliases.Entries[9].Email;
                            Label18.Text = "<b>" + "Email Alias #11: </b>" + emailAliases.Entries[10].Email;
                            Label19.Text = "<b>" + "Email Alias #12: </b>" + emailAliases.Entries[11].Email;
                            Label20.Text = "<b>" + "Email Alias #13: </b>" + emailAliases.Entries[12].Email;
                            Label21.Text = "<b>" + "Email Alias #14: </b>" + emailAliases.Entries[13].Email;
                            Label22.Text = "<b>" + "Email Alias #15: </b>" + emailAliases.Entries[14].Email;
                            Label23.Text = "<b>" + "Email Alias #16: </b>" + emailAliases.Entries[15].Email;
                            Label24.Text = "<b>" + "Email Alias #17: </b>" + emailAliases.Entries[16].Email;
                            Label25.Text = "<b>" + "Email Alias #18: </b>" + emailAliases.Entries[17].Email;
                            Label26.Text = "<b>" + "Email Alias #19: </b>" + emailAliases.Entries[18].Email;
                            lbl.Add(Label8);
                            lbl.Add(Label9);
                            lbl.Add(Label10);
                            lbl.Add(Label11);
                            lbl.Add(Label12);
                            lbl.Add(Label13);
                            lbl.Add(Label14);
                            lbl.Add(Label15);
                            lbl.Add(Label16);
                            lbl.Add(Label17);
                            lbl.Add(Label18);
                            lbl.Add(Label19);
                            lbl.Add(Label20);
                            lbl.Add(Label21);
                            lbl.Add(Label22);
                            lbl.Add(Label23);
                            lbl.Add(Label24);
                            lbl.Add(Label25);
                            lbl.Add(Label26);
                            break;
                        }
                    case 20:
                        {
                            Label8.Visible = true;
                            Button8.Visible = true;
                            Label9.Visible = true;
                            Button9.Visible = true;
                            Label10.Visible = true;
                            Button10.Visible = true;
                            Label11.Visible = true;
                            Button11.Visible = true;
                            Label12.Visible = true;
                            Button12.Visible = true;
                            Label13.Visible = true;
                            Button13.Visible = true;
                            Label14.Visible = true;
                            Button14.Visible = true;
                            Label15.Visible = true;
                            Button15.Visible = true;
                            Label16.Visible = true;
                            Button16.Visible = true;
                            Label17.Visible = true;
                            Button17.Visible = true;
                            Label18.Visible = true;
                            Button18.Visible = true;
                            Label19.Visible = true;
                            Button19.Visible = true;
                            Label20.Visible = true;
                            Button20.Visible = true;
                            Label21.Visible = true;
                            Button21.Visible = true;
                            Label22.Visible = true;
                            Button22.Visible = true;
                            Label23.Visible = true;
                            Button23.Visible = true;
                            Label24.Visible = true;
                            Button24.Visible = true;
                            Label25.Visible = true;
                            Button25.Visible = true;
                            Label26.Visible = true;
                            Button26.Visible = true;
                            Label27.Visible = true;
                            Button27.Visible = true;
                            Label8.Text = "<b>" + "Email Alias #1: </b>" + emailAliases.Entries[0].Email;
                            Label9.Text = "<b>" + "Email Alias #2: </b>" + emailAliases.Entries[1].Email;
                            Label10.Text = "<b>" + "Email Alias #3: </b>" + emailAliases.Entries[2].Email;
                            Label11.Text = "<b>" + "Email Alias #4: </b>" + emailAliases.Entries[3].Email;
                            Label12.Text = "<b>" + "Email Alias #5: </b>" + emailAliases.Entries[4].Email;
                            Label13.Text = "<b>" + "Email Alias #6: </b>" + emailAliases.Entries[5].Email;
                            Label14.Text = "<b>" + "Email Alias #7: </b>" + emailAliases.Entries[6].Email;
                            Label15.Text = "<b>" + "Email Alias #8: </b>" + emailAliases.Entries[7].Email;
                            Label16.Text = "<b>" + "Email Alias #9: </b>" + emailAliases.Entries[8].Email;
                            Label17.Text = "<b>" + "Email Alias #10: </b>" + emailAliases.Entries[9].Email;
                            Label18.Text = "<b>" + "Email Alias #11: </b>" + emailAliases.Entries[10].Email;
                            Label19.Text = "<b>" + "Email Alias #12: </b>" + emailAliases.Entries[11].Email;
                            Label20.Text = "<b>" + "Email Alias #13: </b>" + emailAliases.Entries[12].Email;
                            Label21.Text = "<b>" + "Email Alias #14: </b>" + emailAliases.Entries[13].Email;
                            Label22.Text = "<b>" + "Email Alias #15: </b>" + emailAliases.Entries[14].Email;
                            Label23.Text = "<b>" + "Email Alias #16: </b>" + emailAliases.Entries[15].Email;
                            Label24.Text = "<b>" + "Email Alias #17: </b>" + emailAliases.Entries[16].Email;
                            Label25.Text = "<b>" + "Email Alias #18: </b>" + emailAliases.Entries[17].Email;
                            Label26.Text = "<b>" + "Email Alias #19: </b>" + emailAliases.Entries[18].Email;
                            Label27.Text = "<b>" + "Email Alias #20: </b>" + emailAliases.Entries[19].Email;
                            lbl.Add(Label8);
                            lbl.Add(Label9);
                            lbl.Add(Label10);
                            lbl.Add(Label11);
                            lbl.Add(Label12);
                            lbl.Add(Label13);
                            lbl.Add(Label14);
                            lbl.Add(Label15);
                            lbl.Add(Label16);
                            lbl.Add(Label17);
                            lbl.Add(Label18);
                            lbl.Add(Label19);
                            lbl.Add(Label20);
                            lbl.Add(Label21);
                            lbl.Add(Label22);
                            lbl.Add(Label23);
                            lbl.Add(Label24);
                            lbl.Add(Label25);
                            lbl.Add(Label26);
                            lbl.Add(Label27);
                            break;
                        }
                    default:
                        break;
                }
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
                RecentEventsButton.Visible = false;
                Exportbtn.Visible = true;
                InvalidEmailLabel.Visible = true;
                Label1.Text = "";
                Label2.Text = "";
                Label3.Text = "";
                Label4.Text = "";
                Label7.Text = "";
                Button7.Visible = false;
                TextBox1.Visible = false;

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
            last = last.Substring(last.IndexOf(": ") + 6);


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
                RegisterAsyncTask(new PageAsyncTask(GetUsersbyEmail));;
            //Error

        }
        private List<string> EventNameParser(List<string> listofevents)
        {
            List<string> listofnames = new List<string>();
            string a = "";
            foreach (string str in listofevents)
            {

                int index = str.IndexOf("login");
                
                if (index == -1 || index < 20)
                {
                    listofnames.Add("");
                   
                }
                else
                {
                    index = index + 8;
                    a = str.Substring(index);
                    int index2 = a.IndexOf("},");
                    if (index2 == -1)
                    {
                        listofnames.Add("");
                    }
                    else
                    {
                        a = a.Substring(0, index2 - 1);
                        listofnames.Add(a);
                    }
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
               
                    int index = str.IndexOf("event_type");
                    if (index == -1)
                    {
                        listoftypes.Add("");
                      
                    }

                    else
                    {
                        index = index + 13;
                        a = str.Substring(index);
                        int index2 = a.IndexOf(",");
                        if (index2 == -1)
                        {
                            listofevents.Add("");
                        }
                        else
                        {
                            a = a.Substring(0, index2 - 1);
                            listoftypes.Add(a);
                        }
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
                if (index == -1)
                {
                    listofcreations.Add("");
                   
                }
                else
                {
                    index = index + 13;
                    a = str.Substring(index);
                    int index2 = a.IndexOf(",");
                    if (index2 == -1)
                    {
                        listofcreations.Add("");
                    }
                    else
                    {
                        a = a.Substring(0, index2 - 1);
                        listofcreations.Add(a);
                    }
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
                if (index == -1)
                {
                    list.Add("");
                   
                }
                else
                {
                    index = index + 17;
                    a = str.Substring(index);
                    int index2 = a.IndexOf(",");
                    if (index2 == -1)
                    {
                        list.Add("");
                    }
                    else
                    {
                        a = a.Substring(0, index2 - 1);
                        list.Add(a);
                    }
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
                    list.Add("");
                 
                }
                else
                {
                    a = str.Substring(index);
                    int index2 = a.IndexOf("id");
                    if (index2 == -1)
                    {
                        list.Add("");
                       
                    }
                    else
                    {
                        a = a.Substring(index2);
                        int index3 = a.IndexOf(",");
                        if (index3 == -1)
                        {
                            list.Add("");
                          
                        }
                        else
                        {
                            a = a.Substring(5, index3 - 6);
                            list.Add(a);
                        }
                    }
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
                    list.Add("");
                }
                else
                {
                    a = str.Substring(index);
                    int index2 = a.IndexOf("parent");
                    if (index2 == -1)
                    {
                        list.Add("");
                       
                    }
                    else
                    {
                        a = a.Substring(index2);
                        int index3 = a.IndexOf("name");
                        if (index3 == -1)
                        {
                            list.Add("");
                            
                        }

                        else
                        {
                            a = a.Substring(index3);
                            int index4 = a.IndexOf(",");
                            if (index4 == -1)
                            {
                                list.Add("");
                             
                            }
                            else
                            {
                                a = a.Substring(7, index4 - 9);
                                list.Add(a);
                            }
                        }
                    }

                }
            }

            return list;
        }
        protected void Recent_Click(object sender, EventArgs e)
        {

        }
        protected void StudentSearchButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("NormalUserMenu.aspx");
        }
        protected void AdminPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminMenu.aspx");
        }
    }
}
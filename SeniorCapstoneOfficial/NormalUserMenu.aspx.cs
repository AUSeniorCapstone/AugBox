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

namespace SeniorCapstoneOfficial
{
    public partial class NormalUserMenu : System.Web.UI.Page
    {

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

        private async Task ExportEverything()
        {

            Response.Clear();
            Response.Charset = "";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/doc";
            String filename = "user";

            BoxAuthTest box = new BoxAuthTest();
            IEnumerable<BoxUser> allUsers = await box.GetallUsers();

            String string1 = "";
            String string2 = "";
            String string3 = "";
            String string4 = "";
            String string5 = "";

            foreach (BoxUser user in allUsers)
            {
                try
                {

                    string1 = "Name: " + user.Name + "\n";
                    string2 = "Space Used: " + user.SpaceUsed.ToString() + " bytes \n";
                    string3 = "Status: " + user.Status.ToUpper() + "\n";
                    string4 = "Last Login: " + user.ModifiedAt.ToString() + "\n";

                    IEnumerable<BoxItem> boxFolder = await box.GetFolder(user.Id);

                    foreach (BoxItem item in boxFolder)
                    {
                        if (!item.Name.EndsWith(".txt"))
                            string5 = string5 + "Dir: " + item.Name + "\n";
                        else
                            string5 = string5 + item.Name + "\n";
                    }
                    Response.Write(string1 + string2 + string3 + string4 + string5 + "\n");
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

            if (foundUser != null)
            {
                Label1.Text = "<b>" + "Name: " + "</b>" + foundUser.Name;
                Label2.Text = "<b>" + "Space Used: " + "</b>" + foundUser.SpaceUsed.ToString() + " bytes";
                Label3.Text = "<b>" + "Status: " + "</b>" + foundUser.Status.ToUpper();
                Label4.Text = "<b>" + "Last Login: " + "</b>" + foundUser.ModifiedAt.ToString();
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

        protected void LogoutBtn_Click(object sender, EventArgs e)
        {

            DBConnector dbconnector = new DBConnector();
            dbconnector.saveLogout(Session["UserName"].ToString(), DateTime.Now);
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }

        protected void AdminPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminMenu.aspx");
        }
    }
}
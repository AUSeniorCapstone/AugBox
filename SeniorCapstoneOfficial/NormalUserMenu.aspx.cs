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

namespace SeniorCapstoneOfficial
{
    public partial class NormalUserMenu : System.Web.UI.Page
    {
       
        List<BoxItem> FolderList = new List<BoxItem>();
        protected void Page_Load(object sender, EventArgs e)
        {
            
            DBConnector db = new DBConnector();
            bool admin = db.AdminCheck(Session["UserName"].ToString());
           if(admin == false)
            {
                StudentSearchButton.Attributes.Add("style", "right:15%");
                AdminPageButton.Visible = false;
            }          
            InvalidEmailLabel.Visible = false;
            Exportbtn.Visible = true;
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
            List<BoxUser> allUsers = await box.GetallUsers();

            String string1 = "";
            String string2 = "";
            String string3 = "";
            String string4 = "";
            String string5 = "";

            for (int i = 0; i < allUsers.Count - 1; i++)
            {

                //BoxAuthTest boxLoop = new BoxAuthTest();
                //List<BoxUser> allUsers = await boxLoop.GetallUsers();

                string1 = "Name: " + allUsers[i].Name + "\n";
                string2 = "Space Used: " + allUsers[i].SpaceUsed.ToString() + " bytes \n";
                string3 = "Status: " + allUsers[i].Status.ToUpper() + "\n";
                string4 = "Last Modified: " + allUsers[i].ModifiedAt.ToString() + "\n";

                /*FolderList = await box.GetFolder(allUsers[i].Id); Uncomment from here down to that area to see how long it'll take.
                int foldercount = FolderList.Count; It probably won't finish because the client will timeout. API calls for box are SLOOOOOW


                for (int j = 0; j < foldercount; j++)
                {
                    if(FolderList[j].Name.EndsWith(".txt"))
                        string5 = "Dir: " + FolderList[j].Name + "\n";
                    else
                    {
                        string5 = FolderList[j].Name + "\n";
                    }
                }
                */

                Response.Write(string1 + string2 + string3 + string4 + string5 + "\n");
            }

            filename = filename.Replace(" ", "");
            Response.AddHeader("content-disposition", "attachment;filename=" + filename.ToLower() + "Info.doc");
            Response.Flush();
            Response.End();

        }

        private async Task GetUsersbyEmail()
        {
            BoxAuthTest box = new BoxAuthTest();
            bool found;
            List<BoxUser> users = await box.GetallUsers();

            BoxUser foundUser = users.Find(u => u.Login.Equals(EmailAddress.Text.Trim()));
            if(!foundUser.Name.Equals(EmailAddress.Text.Trim()))
            Label1.Text = "<b>" + "Name: " + "</b>" + foundUser.Name;
            Label2.Text = "<b>" + "Space Used: " + "</b>" + foundUser.SpaceUsed.ToString() + " bytes";
            Label3.Text = "<b>" + "Status: " + "</b>" + foundUser.Status.ToUpper();
            Label4.Text = "<b>" + "Last Modified: " + "</b>" + foundUser.ModifiedAt.ToString();
            Label5.Text = "<b>" + "Top Folders" + "</b>";


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

                    Exportbtn.Visible = true;

                    found = true;               
                    

            if (found == false)
            {
                Exportbtn.Visible = true;
                InvalidEmailLabel.Visible = true;
                Label1.Text = "";
                Label2.Text = "";
                Label3.Text = "";
                Label4.Text = "";
            }

            /* This shows all the users
              GridView1.DataSource = users;
              GridView1.DataBind();*/
        }

        protected void LogoutBtn_Click(object sender, EventArgs e)
        {

            DBConnector dbconnector = new DBConnector();
            dbconnector.saveLogout(Session["UserName"].ToString(), DateTime.Now);
            Response.Redirect("Login.aspx");
        }

        protected void AdminPage_Click(object sender, EventArgs e)
        {          
            Response.Redirect("AdminMenu.aspx");
        }
    }
}
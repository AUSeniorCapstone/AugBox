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
            Exportbtn.Visible = false;
        }

        protected void SearchForStudent_Click(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(GetUsersbyEmail));

        }

        protected void Exportbtn_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Charset = "";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/doc";
            string filename = Label1.Text.Substring(5);
            filename = filename.Replace(" ", "");
            Response.AddHeader("content-disposition", "attachment;filename=" + filename.ToLower()+ "info.doc");
            Response.Write(Label1.Text + "\n" + Label2.Text + "\n" + Label3.Text + "\n" + Label4.Text);
            Response.Flush();
            Response.End();

        }

        private async Task GetUsersbyEmail()
        {
            BoxAuthTest box = new BoxAuthTest();
            bool found = false;
            List<BoxUser> users = await box.GetallUsers();

            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Login.Equals(EmailAddress.Text.Trim()))
                {
                    Label1.Text = "<b>" + "Name: " + "</b>" + users[i].Name;
                    Label2.Text = "<b>" + "Space Used: " + "</b>" + users[i].SpaceUsed.ToString() + " bytes";
                    Label3.Text = "<b>" + "Status: " + "</b>" + users[i].Status.ToUpper();
                    Label4.Text = "<b>" + "Last Modified: " + "</b>" + users[i].ModifiedAt.ToString();
                    Label5.Text = "<b>" + "Top Folders" + "</b>";


                    FolderList = await box.GetFolder(users[i].Id);
                    int foldercount = FolderList.Count;


                    for (int j = 0; j < foldercount; j++)
                    {
                        Label folder = new Label();
                        folder.ID = "folder" + j;
                        folder.Text = FolderList[j].Name;

                        FolderPH.Controls.Add(folder);
                        FolderPH.Controls.Add(new LiteralControl("<br />"));
                        FolderPH.Controls.Add(new LiteralControl("<br />"));
                    }

                    Exportbtn.Visible = true;

                    found = true;               
                }
                    
             }

            if (found == false)
            {
                Exportbtn.Visible = false;
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
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using Box.V2.Config;
using Box.V2.JWTAuth;
using Box.V2.Models;

namespace SeniorCapstoneOfficial
{
    public partial class NormalUserMenu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SearchForStudent.Click += SearchForStudent_Click;
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
                    Label1.Text = "Name: " + users[i].Name;
                    Label2.Text = "Space Used: " + users[i].SpaceUsed.ToString() + " bytes";
                    Label3.Text = "Status: " + users[i].Status.ToUpper();
                    Label4.Text = "Last Modified: " + users[i].ModifiedAt.ToString();


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
            dbconnector.saveLogout(HttpContext.Current.Request.Params["username"], DateTime.Now);
            Response.Redirect("Login.aspx");
        }

        protected void AdminPage_Click(object sender, EventArgs e)
        {
            string uname = HttpContext.Current.Request.Params["username"];
            Response.Redirect("AdminMenu.aspx?username=" + uname);
        }
    }
}
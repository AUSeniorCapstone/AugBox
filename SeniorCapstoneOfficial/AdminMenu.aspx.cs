﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SeniorCapstoneOfficial
{
    public partial class AdminMenu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PasswordTextBox.Attributes["type"] = "password";
        }

        protected void AddUserButton_Click(object sender, EventArgs e)
        {
            DBConnector db = new DBConnector();
            if (db.getUser(UserNameTextBox.Text).uname != null)
            {
                PH.Controls.Clear();
                Label ErrorMessage = new Label();
                ErrorMessage.Text = "Username Not Available";
                ErrorMessage.Attributes.Add("style", "color:Red;");
                PH.Controls.Add(ErrorMessage);
                
            }
            else
            {
                PH.Controls.Clear();
                db.createUsers(FirstNameTextbox.Text, LastNameTextbox.Text, UserNameTextBox.Text, PasswordTextBox.Text, AdminCheckbox.Checked);
                Label Confirmation = new Label();
                Confirmation.Text = "User Successfully Added";
                Confirmation.Attributes.Add("style", "color:green;");
                PH.Controls.Add(Confirmation);
                FirstNameTextbox.Text = "";
                LastNameTextbox.Text = "";
                UserNameTextBox.Text = "";
                PasswordTextBox.Text = "";
                AdminCheckbox.Checked = false;
            }
        }

        protected void SearchUserButton_Click(object sender, EventArgs e)
        {
            DBConnector db = new DBConnector();
            int i = 0;
            List<Button> DeleteButtonList = new List<Button>();

            foreach (User u in db.userSearch(SearchUserTextBox.Text))
            {
                Label UserLabel = new Label();
                Button DeleteButton = new Button();

                DeleteButton.ID = "DeleteButton" + i;
                DeleteButtonList.Add(DeleteButton);
                DeleteButton.Text = "Delete";
                DeleteButton.Click += new EventHandler(DeleteButton_Click);
                DeleteButton.OnClientClick = "return confirm('Are you sure you want to delete selected user?');";

                UserLabel.Text = u.fName.ToString() + " " + u.lName.ToString() + " " + u.uname.ToString() + " ";

                SearchUserPlaceHolder.Controls.Add(UserLabel);
                SearchUserPlaceHolder.Controls.Add(DeleteButton);

                i++;

                SearchUserPlaceHolder.Controls.Add(new LiteralControl("<br />"));
                SearchUserPlaceHolder.Controls.Add(new LiteralControl("<br />"));
            }

        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            DBConnector db = new DBConnector();
           // db.DeleteUser()
        }

        protected void LogoutBtn_Click(object sender, EventArgs e)
        {
            
            DBConnector dbconnector = new DBConnector();
            dbconnector.saveLogout(HttpContext.Current.Request.Params["username"], DateTime.Now);
            Response.Redirect("Login.aspx");
        }

        protected void StudentSearchBtn_Click(object sender, EventArgs e)
        {
             string uname = HttpContext.Current.Request.Params["username"];
             Response.Redirect("NormalUserMenu.aspx?username=" + uname);
        }
    }
}

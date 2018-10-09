using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SeniorCapstoneOfficial
{
    public partial class AdminMenu : System.Web.UI.Page
    {
        List<Button> DeleteButtonList = new List<Button>();
        protected void Page_Load(object sender, EventArgs e)
        {
            PasswordTextBox.Attributes["type"] = "password";
            if (Session["UserName"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            else
            {
                DBConnector db1 = new DBConnector();
                bool admin = db1.AdminCheck(Session["UserName"].ToString());
                if (admin == true)
                {
                    if (Page.IsPostBack && ViewState["buttonSearch"] != null)
                    {
                        DBConnector db = new DBConnector();
                        int i = 0;

                        foreach (User u in db.userSearch(SearchUserTextBox.Text))
                        {

                            Button DeleteButton = new Button();

                            DeleteButton.ID = u.uname;
                            DeleteButtonList.Add(DeleteButton);
                            DeleteButton.Text = "Delete";

                            DeleteButton.Click += new EventHandler(DeleteButton_Click);
                            DeleteButton.OnClientClick = "return confirm('Are you sure you want to delete selected user?');";


                            DeleteButton.Visible = false;
                            SearchUserPlaceHolder.Controls.Add(DeleteButton);

                            i++;

                            SearchUserPlaceHolder.Controls.Add(new LiteralControl("<br />"));
                            SearchUserPlaceHolder.Controls.Add(new LiteralControl("<br />"));
                        }
                    }
                }
                else
                    Response.Redirect("NormalUserMenu.aspx");
            }
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

            foreach (User u in db.userSearch(SearchUserTextBox.Text))
            {
                List<Button> DeleteButtonList = new List<Button>();
                Label UserLabel = new Label();
                Button DeleteButton = new Button();

                DeleteButton.ID = u.uname;
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
            ViewState["buttonSearch"] = true;
        }


        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            DBConnector db = new DBConnector();
            db.DeleteUser(btn.ID);
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
    }
}
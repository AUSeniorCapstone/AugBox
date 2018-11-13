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
        List<Button> btn = new List<Button>();
        List<Label> lbl = new List<Label>();
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
                bool comp = db1.ComplianceCheck(Session["UserName"].ToString());
                if (admin == true)
                {
                    string userlastname = "";
                    if (Request.QueryString["Parameter"] != null)
                    {
                        userlastname = Request.QueryString["Parameter"].ToString();
                    }
                    DBConnector db = new DBConnector();
                    SearchUserFailed.Controls.Clear();
                    for (int x = 0; x < btn.Count; x++)
                    {
                        btn[x].Visible = false;
                        lbl[x].Visible = false;
                    }

                    int i = 0;
                    if (db.userSearch(userlastname).Count != 0)
                    {
                        foreach (User u in db.userSearch(userlastname))
                        {
                            Button button = new Button();
                            Label newlabel = new Label();
                            newlabel.Text = u.fName.ToString() + " " + u.lName.ToString() + ": " + u.uname.ToString();
                            button.ID = i.ToString();
                            button.Click += new EventHandler(DeleteButton_Click);
                            button.CssClass = "DeleteUserButton";
                            button.OnClientClick = "return Validate();";
                            button.Text = "Delete";
                            newlabel.Attributes.CssStyle.Add("margin-bottom", "5px");
                            button.Attributes.CssStyle.Add("margin-bottom", "5px");
                            button.Attributes.CssStyle.Add("margin-left", "5px");
                            btn.Add(button);
                            lbl.Add(newlabel);
                            SearchUserFailed.Controls.Add(newlabel);
                            SearchUserFailed.Controls.Add(button);
                            SearchUserFailed.Controls.Add(new LiteralControl("<br />"));
                            i++;
                        }
                    }

                    else
                    {
                        Label failed = new Label();

                        if (userlastname == "")
                            failed.Text = "";
                        else
                        {
                            failed.Text = "No User Found";
                            failed.ForeColor = System.Drawing.Color.Red;
                            SearchUserFailed.Controls.Add(failed);
                        }
                    }


                }
                else
                    Response.Redirect("NormalUserMenu.aspx");
            }
        }

        protected void ImageButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("LandingPage.aspx");
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
                int role = 0;
                if(AdminCheckbox.Checked)
                {
                    role = 1;
                }
                else if (ComplianceCheckbox.Checked)
                {
                    role = 4;
                }
                else if (NormaluserCheckbox.Checked)
                {
                    role = 0;
                }

                if (AdminCheckbox.Checked || ComplianceCheckbox.Checked || NormaluserCheckbox.Checked)
                {
                    PH.Controls.Clear();
                    db.createUsers(FirstNameTextbox.Text, LastNameTextbox.Text, UserNameTextBox.Text, PasswordTextBox.Text, role);
                    Label Confirmation = new Label();
                    Confirmation.Text = "User Successfully Added";
                    Confirmation.Attributes.Add("style", "color:green;");
                    PH.Controls.Add(Confirmation);
                    FirstNameTextbox.Text = "";
                    LastNameTextbox.Text = "";
                    UserNameTextBox.Text = "";
                    PasswordTextBox.Text = "";
                    AdminCheckbox.Checked = false;
                    ComplianceCheckbox.Checked = false;
                    SearchUserFailed.Controls.Clear();
                }
                else
                {
                    PH.Controls.Clear();
                    Label checkboxerror = new Label();
                    checkboxerror.Text = "Select a User Type";
                    PH.Controls.Add(checkboxerror);
                }
            }
        }

        /*protected void SearchUserButton_Click(object sender, EventArgs e)
        {
            DBConnector db = new DBConnector();
            SearchUserFailed.Controls.Clear();
            for (int x = 0; x < btn.Count; x++)
            {
                btn[x].Visible = false;
                lbl[x].Visible = false;
            }

            int i = 0;
            if (db.userSearch(SearchUserTextBox.Text).Count != 0)
            {
                foreach (User u in db.userSearch(SearchUserTextBox.Text))
                {
                    btn[i].Visible = true;
                    lbl[i].Visible = true;
                    lbl[i].Text = u.fName.ToString() + " " + u.lName.ToString() + ": " + u.uname.ToString();
                    i++;                    
                }
            }
            else
            {
                Label failed = new Label();
                failed.Text = "No User Found";
                failed.ForeColor = System.Drawing.Color.Red;
                SearchUserFailed.Controls.Add(failed);
            }         
        }

    */

        protected void SearchUserButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminMenu.aspx?Parameter=" + SearchUserTextBox.Text);
        }
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            Button bttn = (Button)sender;
            string button = bttn.ID.ToString();
            string last = button[button.Length - 1].ToString();
            int i = Convert.ToInt32(last);
            DBConnector db = new DBConnector();
            string input = lbl[i].Text.Substring(lbl[i].Text.IndexOf(": ") + 2);
            db.DeleteUser(input);
            SearchUserFailed.Controls.Clear();
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
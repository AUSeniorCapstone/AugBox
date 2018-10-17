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
                if (admin == true)
                {

                    lbl.Add(Label0);
                    lbl.Add(Label1);
                    lbl.Add(Label2);
                    lbl.Add(Label3);
                    lbl.Add(Label4);
                    lbl.Add(Label5);
                    lbl.Add(Label6);
                    lbl.Add(Label7);
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

                    btn.Add(Button0);
                    btn.Add(Button1);
                    btn.Add(Button2);
                    btn.Add(Button3);
                    btn.Add(Button4);
                    btn.Add(Button5);
                    btn.Add(Button6);
                    btn.Add(Button7);
                    btn.Add(Button8);
                    btn.Add(Button9);
                    btn.Add(Button10);
                    btn.Add(Button11);
                    btn.Add(Button12);
                    btn.Add(Button13);
                    btn.Add(Button14);
                    btn.Add(Button15);
                    btn.Add(Button16);
                    btn.Add(Button17);
                    btn.Add(Button18);
                    btn.Add(Button19);
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


        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            Button bttn = (Button)sender;
            string button = bttn.ID.ToString();
            string last = button[button.Length - 1].ToString();
            int i = Convert.ToInt32(last);
            DBConnector db = new DBConnector();
            string input = lbl[i].Text.Substring(lbl[i].Text.IndexOf(": ") + 2);
            db.DeleteUser(input);
            for(int x = 0; x<lbl.Count;x++)
            {
                lbl[x].Visible = false;
                btn[x].Visible = false;
            }
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SQLite;
using System.IO;
using System.Data;
using System.Security.Cryptography;

namespace SeniorCapstoneOfficial
{
    public partial class Login : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null)
            {                                            
                    Response.Redirect("LandingPage.aspx");            
            }


            lblErrorMessage.Visible = false;
            DBConnector databaseObject = new DBConnector();
            databaseObject.OpenConnection();
            databaseObject.createTables();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            LoginControl lc = new LoginControl();

            if (lc.submit(txturname.Text.Trim(), txtpassword.Text.Trim()) == 1)
            {

                Session["UserName"] = txturname.Text.Trim();
                Response.Redirect("LandingPage.aspx");

            }
            else if (lc.submit(txturname.Text.Trim(), txtpassword.Text.Trim()) == 2)
            {
                Session["UserName"] = txturname.Text.Trim();
                Response.Redirect("LandingPage.aspx");
            }
            else if (lc.submit(txturname.Text.Trim(), txtpassword.Text.Trim()) == 4)
            {
                Session["UserName"] = txturname.Text.Trim();
                Response.Redirect("LandingPage.aspx");
            }
            else if (lc.submit(txturname.Text.Trim(), txtpassword.Text.Trim()) == 3)
            {
                lblErrorMessage.Visible = true;
                txtpassword.Text= "";
                txturname.Text = "";
            }
        }
    }
}
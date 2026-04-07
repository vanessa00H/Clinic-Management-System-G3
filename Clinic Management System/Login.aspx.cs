using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Clinic_Management_System
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
             string connStr = ConfigurationManager.ConnectionStrings["ClinicDBConnection"].ConnectionString;

             using (SqlConnection conn = new SqlConnection(connStr))
             {
                string query = "SELECT * FROM Users WHERE Username=@Username AND Password=@Password";

              SqlCommand cmd = new SqlCommand(query, conn);
              cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
              cmd.Parameters.AddWithValue("@Password", txtPassword.Text);

              conn.Open();
              SqlDataReader reader = cmd.ExecuteReader();

              if (reader.Read())
              {
                Response.Redirect("Default.aspx");
              }
              else
              {
                lblMessage.Text = "Invalid username or password";
              }
        }
    }
}
}
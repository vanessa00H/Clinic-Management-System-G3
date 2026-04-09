using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Clinic_Management_System
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtNewUsername.Text.Trim();
            string password = txtNewPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                lblRegisterMessage.ForeColor = Color.Red;
                lblRegisterMessage.Text = "Username and password are required.";
                return;
            }

            // Username: letters only
            if (!Regex.IsMatch(username, @"^[A-Za-z]+$"))
            {
                lblRegisterMessage.ForeColor = Color.Red;
                lblRegisterMessage.Text = "Username must contain letters only.";
                return;
            }

            // Password: at least 6 characters
            if (password.Length < 6)
            {
                lblRegisterMessage.ForeColor = Color.Red;
                lblRegisterMessage.Text = "Password must be at least 6 characters.";
                return;
            }

            bool hasLetter = Regex.IsMatch(password, @"[A-Za-z]");
            bool hasNumber = Regex.IsMatch(password, @"\d");
            bool validPasswordChars = Regex.IsMatch(password, @"^[A-Za-z0-9_]+$");

            if (!validPasswordChars)
            {
                lblRegisterMessage.ForeColor = Color.Red;
                lblRegisterMessage.Text = "Password can contain letters, numbers, and underscore (_) only.";
                return;
            }

            if (!hasLetter || !hasNumber)
            {
                lblRegisterMessage.ForeColor = Color.Red;
                lblRegisterMessage.Text = "Password must contain letters and numbers.";
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["ClinicDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string checkQuery = "SELECT COUNT(*) FROM dbo.Users WHERE Username = @Username";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@Username", username);

                int userExists = (int)checkCmd.ExecuteScalar();

                if (userExists > 0)
                {
                    lblRegisterMessage.ForeColor = Color.Red;
                    lblRegisterMessage.Text = "Username already exists.";
                    return;
                }

                string insertQuery = "INSERT INTO dbo.Users (Username, Password,Role) VALUES (@Username, @Password,@Role)";
                SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("@Username", username);
                insertCmd.Parameters.AddWithValue("@Password", password);
                insertCmd.Parameters.AddWithValue("@Role", "Patient");

                int rows = insertCmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    lblRegisterMessage.ForeColor = Color.Green;
                    lblRegisterMessage.Text = "User registered successfully!";
                    txtNewUsername.Text = "";
                    txtNewPassword.Text = "";
                }
                else
                {
                    lblRegisterMessage.ForeColor = Color.Red;
                    lblRegisterMessage.Text = "Registration failed.";
                }
            }
        }
    }
}
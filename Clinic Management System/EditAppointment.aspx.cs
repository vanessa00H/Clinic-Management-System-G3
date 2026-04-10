using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Clinic_Management_System
{
    public partial class EditAppointment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
                Response.Redirect("Login.aspx");

            if (!IsPostBack)
                LoadAppointment();
        }

        private void LoadAppointment()
        {
            string id = Request.QueryString["id"];
            if (string.IsNullOrEmpty(id)) return;

            string connStr = ConfigurationManager
                .ConnectionStrings["ClinicDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM Appointments WHERE AppointmentID=@ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    ((System.Web.UI.WebControls.TextBox)FindControl("txtPatientName")).Text = reader["PatientName"].ToString();
                    ((System.Web.UI.WebControls.TextBox)FindControl("txtEmail")).Text = reader["Email"].ToString();
                    ((System.Web.UI.WebControls.TextBox)FindControl("txtPhone")).Text = reader["Phone"].ToString();
                    ((System.Web.UI.WebControls.TextBox)FindControl("txtDate")).Text = Convert.ToDateTime(reader["AppointmentDate"]).ToString("yyyy-MM-dd");
                    ((System.Web.UI.WebControls.TextBox)FindControl("txtTime")).Text = reader["AppointmentTime"].ToString();
                    ((System.Web.UI.WebControls.DropDownList)FindControl("ddlStatus")).SelectedValue = reader["Status"].ToString();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var txtPatientName = (System.Web.UI.WebControls.TextBox)FindControl("txtPatientName");
            var txtEmail = (System.Web.UI.WebControls.TextBox)FindControl("txtEmail");
            var txtPhone = (System.Web.UI.WebControls.TextBox)FindControl("txtPhone");
            var txtDate = (System.Web.UI.WebControls.TextBox)FindControl("txtDate");
            var txtTime = (System.Web.UI.WebControls.TextBox)FindControl("txtTime");
            var ddlStatus = (System.Web.UI.WebControls.DropDownList)FindControl("ddlStatus");
            var lblMessage = (System.Web.UI.WebControls.Label)FindControl("lblMessage");

            bool isValid = true;

            // Patient Name
            if (string.IsNullOrWhiteSpace(txtPatientName.Text))
            {
                lblMessage.Text = "Patient name is required.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                isValid = false;
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(txtPatientName.Text, @"^[A-Za-z\s]+$"))
            {
                lblMessage.Text = "Name must contain letters only.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                isValid = false;
            }

            // Email
            if (isValid && !System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                lblMessage.Text = "Enter a valid email.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                isValid = false;
            }

            // Phone
            if (isValid && !System.Text.RegularExpressions.Regex.IsMatch(txtPhone.Text, @"^\d{10}$"))
            {
                lblMessage.Text = "Phone must be 10 digits.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                isValid = false;
            }

            // Date
            DateTime parsedDate;
            if (isValid && (!DateTime.TryParse(txtDate.Text, out parsedDate) || parsedDate.Date < DateTime.Today))
            {
                lblMessage.Text = "Enter a valid future date.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                isValid = false;
            }

            // Time
            TimeSpan parsedTime;
            if (isValid && !TimeSpan.TryParse(txtTime.Text, out parsedTime))
            {
                lblMessage.Text = "Enter a valid time.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                isValid = false;
            }

            if (!isValid) return;

        }

        }
    }
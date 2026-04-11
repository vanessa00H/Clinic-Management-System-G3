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
            {
                LoadAppointment();
            }
            else
            {
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                Response.Cache.SetNoStore();
            }
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
            var lblPatientNameError = (System.Web.UI.WebControls.Label)FindControl("lblPatientNameError");
            var lblEmailError = (System.Web.UI.WebControls.Label)FindControl("lblEmailError");
            var lblPhoneError = (System.Web.UI.WebControls.Label)FindControl("lblPhoneError");
            var lblDateError = (System.Web.UI.WebControls.Label)FindControl("lblDateError");
            var lblTimeError = (System.Web.UI.WebControls.Label)FindControl("lblTimeError");

            // Clear all errors
            lblPatientNameError.Text = "";
            lblEmailError.Text = "";
            lblPhoneError.Text = "";
            lblDateError.Text = "";
            lblTimeError.Text = "";
            lblMessage.Text = "";

            bool isValid = true;

            // Patient Name
            if (string.IsNullOrWhiteSpace(txtPatientName.Text))
            { lblPatientNameError.Text = "Patient name is required."; isValid = false; }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(txtPatientName.Text, @"^[A-Za-z\s]+$"))
            { lblPatientNameError.Text = "Letters only."; isValid = false; }

            // Email
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            { lblEmailError.Text = "Enter a valid email."; isValid = false; }

            // Phone
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtPhone.Text, @"^\d{10}$"))
            { lblPhoneError.Text = "Phone must be 10 digits."; isValid = false; }

            // Date
            DateTime parsedDate = DateTime.MinValue;
            if (!DateTime.TryParse(txtDate.Text, out parsedDate) || parsedDate.Date < DateTime.Today)
            { lblDateError.Text = "Enter a valid future date."; isValid = false; }

            // Time
            TimeSpan parsedTime = TimeSpan.Zero;
            if (!TimeSpan.TryParse(txtTime.Text, out parsedTime))
            { lblTimeError.Text = "Enter a valid time."; isValid = false; }

            if (!isValid) return;

            string id = Request.QueryString["id"];
            string connStr = ConfigurationManager
                .ConnectionStrings["ClinicDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"UPDATE Appointments SET 
            PatientName=@PatientName, Email=@Email, Phone=@Phone,
            AppointmentDate=@Date, AppointmentTime=@Time, Status=@Status
            WHERE AppointmentID=@ID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PatientName", txtPatientName.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                cmd.Parameters.AddWithValue("@Date", parsedDate.Date);
                cmd.Parameters.AddWithValue("@Time", parsedTime);
                cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);
                cmd.Parameters.AddWithValue("@ID", id);

                conn.Open();
                cmd.ExecuteNonQuery();

                lblMessage.Text = "Appointment updated successfully!";
                lblMessage.ForeColor = System.Drawing.Color.LightGreen;
            }
        }
    }
    }
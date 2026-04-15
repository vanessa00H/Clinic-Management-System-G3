using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Data;

namespace Clinic_Management_System
{
    public partial class BookAppointment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            if(!IsPostBack)
            {
                LoadDoctors();
        
            }
        }

        protected void btnBook_Click(object sender, EventArgs e)
        {
            ClearFieldErrors();
            lblMessage.Text = "";

            string patientName = txtPatientName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string appointmentDate = txtDate.Text.Trim();
            string appointmentTime = txtTime.Text.Trim();
            string doctor = ddlDoctor.SelectedItem.Text;
            string department = txtDepartment.Text.Trim();
            string appointmentType = ddlAppointmentType.SelectedValue;
            string service = ddlService.SelectedValue;

            bool isValid = true;

            // Patient Name
            if (string.IsNullOrWhiteSpace(patientName))
            {
                lblPatientNameError.Text = "Patient name is required.";
                isValid = false;
            }
            else if (!Regex.IsMatch(patientName, @"^[A-Za-z\s]+$"))
            {
                lblPatientNameError.Text = "Name must contain letters and spaces only.";
                isValid = false;
            }

            // Email
            if (string.IsNullOrWhiteSpace(email))
            {
                lblEmailError.Text = "Email is required.";
                isValid = false;
            }
            else if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                lblEmailError.Text = "Enter a valid email address.";
                isValid = false;
            }

            // Phone
            if (string.IsNullOrWhiteSpace(phone))
            {
                lblPhoneError.Text = "Phone number is required.";
                isValid = false;
            }
            else if (!Regex.IsMatch(phone, @"^\d{10}$"))
            {
                lblPhoneError.Text = "Phone number must be exactly 10 digits.";
                isValid = false;
            }

            // Date
            DateTime parsedDate=DateTime.MinValue;
            if (string.IsNullOrWhiteSpace(appointmentDate))
            {
                lblDateError.Text = "Appointment date is required.";
                isValid = false;
            }
            else if (!DateTime.TryParse(appointmentDate, out parsedDate))
            {
                lblDateError.Text = "Enter a valid appointment date.";
                isValid = false;
            }
            else if (parsedDate.Date < DateTime.Today)
            {
                lblDateError.Text = "Appointment date cannot be in the past.";
                isValid = false;
            }

            // Time
            TimeSpan parsedTime=TimeSpan.Zero;
            if (string.IsNullOrWhiteSpace(appointmentTime))
            {
                lblTimeError.Text = "Appointment time is required.";
                isValid = false;
            }
            else if (!TimeSpan.TryParse(appointmentTime, out parsedTime))
            {
                lblTimeError.Text = "Enter a valid appointment time.";
                isValid = false;
            }

            // Doctor
            if (string.IsNullOrWhiteSpace(doctor))
            {
                lblDoctorError.Text = "Please select a doctor.";
                isValid = false;
            }

          

            // Appointment Type
            if (string.IsNullOrWhiteSpace(appointmentType))
            {
                lblTypeError.Text = "Please select an appointment type.";
                isValid = false;
            }

            // Additional Service (optional, no hard validation)
            lblServiceError.Text = "";

            if (!isValid)
            {
                lblMessage.ForeColor = Color.Red;
                lblMessage.Text = "Please correct the highlighted fields.";
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["ClinicDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Prevent double booking
                string checkQuery = @"SELECT COUNT(*) FROM Appointments
                                      WHERE DoctorName = @DoctorName
                                      AND AppointmentDate = @AppointmentDate
                                      AND AppointmentTime = @AppointmentTime";

                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@DoctorName", doctor);
                checkCmd.Parameters.AddWithValue("@AppointmentDate", parsedDate.Date);
                checkCmd.Parameters.AddWithValue("@AppointmentTime", parsedTime);

                int existingAppointments = (int)checkCmd.ExecuteScalar();

                if (existingAppointments > 0)
                {
                    lblMessage.ForeColor = Color.Red;
                    lblMessage.Text = "This doctor already has an appointment at the selected date and time.";
                    return;
                }

                string insertQuery = @"INSERT INTO Appointments
                    (PatientName, Email, Phone, AppointmentDate, AppointmentTime, DoctorName, Department, AppointmentType, AdditionalService, Status)
                    VALUES
                    (@PatientName, @Email, @Phone, @AppointmentDate, @AppointmentTime, @DoctorName, @Department, @AppointmentType, @AdditionalService, @Status)";

                SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("@PatientName", patientName);
                insertCmd.Parameters.AddWithValue("@Email", email);
                insertCmd.Parameters.AddWithValue("@Phone", phone);
                insertCmd.Parameters.AddWithValue("@AppointmentDate", parsedDate.Date);
                insertCmd.Parameters.AddWithValue("@AppointmentTime", parsedTime);
                insertCmd.Parameters.AddWithValue("@DoctorName", doctor);
                insertCmd.Parameters.AddWithValue("@Department", department);
                insertCmd.Parameters.AddWithValue("@AppointmentType", appointmentType);
                insertCmd.Parameters.AddWithValue("@AdditionalService", string.IsNullOrWhiteSpace(service) ? (object)DBNull.Value : service);
                insertCmd.Parameters.AddWithValue("@Status", "Pending");

                int rows = insertCmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    lblMessage.ForeColor = Color.LightGreen;
                    lblMessage.Text = "Appointment booked successfully!";
                    ClearForm();
                }
                else
                {
                    lblMessage.ForeColor = Color.Red;
                    lblMessage.Text = "Failed to book appointment.";
                }
            }
        }

        private void ClearFieldErrors()
        {
            lblPatientNameError.Text = "";
            lblEmailError.Text = "";
            lblPhoneError.Text = "";
            lblDateError.Text = "";
            lblTimeError.Text = "";
            lblDoctorError.Text = "";
            lblDepartmentError.Text = "";
            lblTypeError.Text = "";
            lblServiceError.Text = "";
        }

        private void ClearForm()
        {
            txtPatientName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            txtDate.Text = "";
            txtTime.Text = "";
            ddlDoctor.SelectedIndex = 0;
          txtDepartment .Text = "";
            txtConsultationFee.Text = "";
            ddlAppointmentType.SelectedIndex = 0;
            ddlService.SelectedIndex = 0;
        }
        private void LoadDoctors()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ClinicDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT DoctorID, DoctorName FROM Doctors WHERE IsAvailable = 1";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlDoctor.DataSource = dt;
                ddlDoctor.DataTextField = "DoctorName";
                ddlDoctor.DataValueField = "DoctorID";
                ddlDoctor.DataBind();

                ddlDoctor.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Doctor", ""));
            }
        }
        protected void ddlDoctor_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDepartment.Text = "";
            txtConsultationFee.Text = "";

            if (string.IsNullOrWhiteSpace(ddlDoctor.SelectedValue))
                return;

            string connStr = ConfigurationManager.ConnectionStrings["ClinicDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT Department, ConsultationFee FROM Doctors WHERE DoctorID = @DoctorID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DoctorID", ddlDoctor.SelectedValue);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtDepartment.Text = reader["Department"].ToString();
                    txtConsultationFee.Text = reader["ConsultationFee"].ToString();
                }
            }
        }
    }
}
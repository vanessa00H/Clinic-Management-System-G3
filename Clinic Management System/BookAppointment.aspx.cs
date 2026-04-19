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
            if (Session["Username"] == null || Session["Role"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            string role = Session["Role"].ToString().ToLower();

            
            if (role == "doctor")
            {
                Response.Redirect("Dashboard.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadDoctors();

                
                if (role == "patient")
                {
                    txtPatientName.Text = Session["Username"].ToString();
                    txtPatientName.ReadOnly = true;
                    txtPatientName.Style["background-color"] = "#e2e8f0"; 
                }
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
            string doctorID = ddlDoctor.SelectedValue;
            string department = txtDepartment.Text.Trim();
            string appointmentType = ddlAppointmentType.SelectedValue;
            string service = ddlService.SelectedValue;
            string feeText = txtConsultationFee.Text.Trim();

            bool isValid = true;

            if (string.IsNullOrWhiteSpace(patientName)) { lblPatientNameError.Text = "Patient name is required."; isValid = false; }
            else if (!Regex.IsMatch(patientName, @"^[A-Za-z\s]+$")) { lblPatientNameError.Text = "Name must contain letters and spaces only."; isValid = false; }

            if (string.IsNullOrWhiteSpace(email)) { lblEmailError.Text = "Email is required."; isValid = false; }
            else if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) { lblEmailError.Text = "Enter a valid email address."; isValid = false; }

            if (string.IsNullOrWhiteSpace(phone)) { lblPhoneError.Text = "Phone number is required."; isValid = false; }
            else if (!Regex.IsMatch(phone, @"^\d{8}$")) { lblPhoneError.Text = "Please enter the remaining 8 digits."; isValid = false; }

            DateTime parsedDate = DateTime.MinValue;
            if (string.IsNullOrWhiteSpace(appointmentDate)) { lblDateError.Text = "Appointment date is required."; isValid = false; }
            else if (!DateTime.TryParse(appointmentDate, out parsedDate)) { lblDateError.Text = "Enter a valid appointment date."; isValid = false; }
            else if (parsedDate.Date < DateTime.Today) { lblDateError.Text = "Appointment date cannot be in the past."; isValid = false; }

            TimeSpan parsedTime = TimeSpan.Zero;
            if (string.IsNullOrWhiteSpace(appointmentTime)) { lblTimeError.Text = "Appointment time is required."; isValid = false; }
            else if (!TimeSpan.TryParse(appointmentTime, out parsedTime)) { lblTimeError.Text = "Enter a valid appointment time."; isValid = false; }
            else { if (parsedTime.Hours < 8 || parsedTime.Hours > 22) { lblTimeError.Text = "Appointments: 08:00 AM to 10:00 PM."; isValid = false; } }

            if (string.IsNullOrWhiteSpace(ddlDoctor.SelectedValue)) { lblDoctorError.Text = "Please select a doctor."; isValid = false; }
            if (string.IsNullOrWhiteSpace(appointmentType)) { lblTypeError.Text = "Please select an appointment type."; isValid = false; }

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
                    lblMessage.Text = "This doctor already has an appointment at this time.";
                    return;
                }

               
               
                string insertQuery = @"INSERT INTO Appointments (PatientName, Email, Phone, AppointmentDate, AppointmentTime, DoctorName, Department, AppointmentType, AdditionalService, ConsultationFee, Status, PaymentStatus) 
VALUES (@PatientName, @Email, @Phone, @AppointmentDate, @AppointmentTime, @DoctorName, @Department, @AppointmentType, @AdditionalService, @ConsultationFee, @Status, @PaymentStatus)";

                SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("@PatientName", patientName);
                insertCmd.Parameters.AddWithValue("@Email", email);

                string fullPhoneNumber = "05" + phone;
                insertCmd.Parameters.AddWithValue("@Phone", fullPhoneNumber);

                insertCmd.Parameters.AddWithValue("@AppointmentDate", parsedDate.Date);
                insertCmd.Parameters.AddWithValue("@AppointmentTime", parsedTime);
                insertCmd.Parameters.AddWithValue("@DoctorName", doctor);
                insertCmd.Parameters.AddWithValue("@Department", department);
                insertCmd.Parameters.AddWithValue("@AppointmentType", appointmentType);
                insertCmd.Parameters.AddWithValue("@AdditionalService", string.IsNullOrWhiteSpace(service) ? (object)DBNull.Value : service);

                decimal finalFee = 0;
                decimal.TryParse(feeText, out finalFee);
                insertCmd.Parameters.AddWithValue("@ConsultationFee", finalFee);

                insertCmd.Parameters.AddWithValue("@Status", "Pending");

                // 👇 السطر الجديد اللي ضفناه عشان حالة الدفع
                insertCmd.Parameters.AddWithValue("@PaymentStatus", "Unpaid");

                int rows = insertCmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    lblMessage.ForeColor = Color.LimeGreen;
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
            string role = Session["Role"]?.ToString().ToLower() ?? "";

            
            if (role != "patient")
            {
                txtPatientName.Text = "";
            }

            txtEmail.Text = "";
            txtPhone.Text = "";
            txtDate.Text = "";
            txtTime.Text = "";
            ddlDoctor.SelectedIndex = 0;
            txtDepartment.Text = "";
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
            if (string.IsNullOrWhiteSpace(ddlDoctor.SelectedValue))
            {
                txtDepartment.Text = "";
                txtConsultationFee.Text = "";
                return;
            }

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
                    ViewState["BaseFee"] = reader["ConsultationFee"].ToString();
                    CalculateTotalFee(null, null);
                }
            }
        }

        protected void CalculateTotalFee(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlDoctor.SelectedValue)) return;

            decimal baseFee = 0;
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["ClinicDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                
                string specialQuery = "SELECT FeeAmount FROM SpecialFees WHERE TRIM(Department) = @Dept AND CAST(SpecialDate AS DATE) = @Date";
                SqlCommand cmdSpecial = new SqlCommand(specialQuery, conn);
                cmdSpecial.Parameters.AddWithValue("@Dept", txtDepartment.Text.Trim());

                DateTime selectedDate;
                if (DateTime.TryParse(txtDate.Text, out selectedDate))
                {
                    cmdSpecial.Parameters.AddWithValue("@Date", selectedDate.Date);
                    object specialResult = cmdSpecial.ExecuteScalar();

                    if (specialResult != null)
                    {
                        baseFee = Convert.ToDecimal(specialResult);
                        lblFeeError.Text = "✨ Special Promotional Fee Applied!";
                        lblFeeError.ForeColor = System.Drawing.Color.SpringGreen;
                    }
                }

              
                if (baseFee == 0)
                {
                    string doctorQuery = "SELECT ConsultationFee FROM Doctors WHERE DoctorName = @DocName";
                    SqlCommand cmdDoc = new SqlCommand(doctorQuery, conn);
                    cmdDoc.Parameters.AddWithValue("@DocName", ddlDoctor.SelectedItem.Text);
                    object docResult = cmdDoc.ExecuteScalar();
                    if (docResult != null) baseFee = Convert.ToDecimal(docResult);
                    lblFeeError.Text = "";
                }
            }

            
            decimal total = baseFee;
            if (ddlAppointmentType.SelectedValue == "Urgent Consultation") total += 50; 
            if (ddlService.SelectedValue == "Lab Test") total += 100; 
            
            txtConsultationFee.Text = total.ToString("0.00");
        }
        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDate.Text) || string.IsNullOrEmpty(txtDepartment.Text))
            {
                return;
            }

            try
            {
                
                string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["ClinicDBConnection"].ConnectionString;

                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connStr))
                {
                    string query = "SELECT FeeAmount FROM SpecialFees WHERE TRIM(Department) = @Dept AND CAST(SpecialDate AS DATE) = @Date";
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@Dept", txtDepartment.Text.Trim());

                    DateTime parsedDate = DateTime.Parse(txtDate.Text);
                    cmd.Parameters.AddWithValue("@Date", parsedDate.Date);

                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        txtConsultationFee.Text = Convert.ToDecimal(result).ToString("0.00");
                        lblFeeError.Text = "✨ Special Promotional Fee Applied!";
                        lblFeeError.ForeColor = System.Drawing.Color.SpringGreen;
                        lblMessage.Text = "";
                    }
                    else
                    {
                        lblFeeError.Text = "";

                        string normalPriceQuery = "SELECT ConsultationFee FROM Doctors WHERE DoctorName = @DocName";
                        System.Data.SqlClient.SqlCommand cmdNormal = new System.Data.SqlClient.SqlCommand(normalPriceQuery, conn);
                        cmdNormal.Parameters.AddWithValue("@DocName", ddlDoctor.SelectedItem.Text);

                        object normalPrice = cmdNormal.ExecuteScalar();
                        if (normalPrice != null)
                        {
                            txtConsultationFee.Text = Convert.ToDecimal(normalPrice).ToString("0.00");
                        }
                    }
                }
                CalculateTotalFee(null, null);
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}  

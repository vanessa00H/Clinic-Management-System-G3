using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;

namespace Clinic_Management_System
{
    public partial class EditAppointment : System.Web.UI.Page
    {
        protected global::System.Web.UI.WebControls.TextBox txtPatientName;
        protected global::System.Web.UI.WebControls.Label lblPatientNameError;
        protected global::System.Web.UI.WebControls.TextBox txtEmail;
        protected global::System.Web.UI.WebControls.Label lblEmailError;
        protected global::System.Web.UI.WebControls.TextBox txtPhone;
        protected global::System.Web.UI.WebControls.Label lblPhoneError;
        protected global::System.Web.UI.WebControls.TextBox txtDate;
        protected global::System.Web.UI.WebControls.Label lblDateError;
        protected global::System.Web.UI.WebControls.TextBox txtTime;
        protected global::System.Web.UI.WebControls.Label lblTimeError;
        protected global::System.Web.UI.WebControls.DropDownList ddlDoctor;
        protected global::System.Web.UI.WebControls.TextBox txtDepartment;
        protected global::System.Web.UI.WebControls.DropDownList ddlService;
        protected global::System.Web.UI.WebControls.PlaceHolder phStatus;
        protected global::System.Web.UI.WebControls.DropDownList ddlStatus;
        protected global::System.Web.UI.WebControls.Button btnSave;
        protected global::System.Web.UI.WebControls.Label lblMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDoctors();
                LoadAppointment();
            }
            else
            {
                
                if (Request["__EVENTTARGET"] == "ddlDoctor")
                {
                    ddlDoctor_SelectedIndexChanged(null, null);
                }
            }
        }

        private void LoadDoctors()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ClinicDBConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                
                string query = "SELECT DoctorName, Department FROM Doctors WHERE IsAvailable = 1";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlDoctor.DataSource = dt;
                ddlDoctor.DataTextField = "DoctorName";
                ddlDoctor.DataValueField = "DoctorName"; 
                ddlDoctor.DataBind();
                ddlDoctor.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Doctor", ""));
            }
        }

        protected void ddlDoctor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ddlDoctor.SelectedValue))
            {
                txtDepartment.Text = "";
                return;
            }

            
            string connStr = ConfigurationManager.ConnectionStrings["ClinicDBConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT Department FROM Doctors WHERE DoctorName = @DoctorName";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DoctorName", ddlDoctor.SelectedValue);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtDepartment.Text = reader["Department"].ToString();
                }
            }
        }

        private void LoadAppointment()
        {
            string id = Request.QueryString["id"];
            if (string.IsNullOrEmpty(id)) return;

            string connStr = ConfigurationManager.ConnectionStrings["ClinicDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM Appointments WHERE AppointmentID=@ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtPatientName.Text = reader["PatientName"].ToString();
                    txtEmail.Text = reader["Email"].ToString();
                    txtPhone.Text = reader["Phone"].ToString();
                    txtDate.Text = Convert.ToDateTime(reader["AppointmentDate"]).ToString("yyyy-MM-dd");
                    txtTime.Text = reader["AppointmentTime"].ToString();
                    ddlStatus.SelectedValue = reader["Status"].ToString();

                    
                    string docName = reader["DoctorName"].ToString();
                    if (ddlDoctor.Items.FindByValue(docName) != null)
                    {
                        ddlDoctor.SelectedValue = docName;
                    }
                    txtDepartment.Text = reader["Department"].ToString();

                    string service = reader["AdditionalService"].ToString();
                    if (ddlService.Items.FindByValue(service) != null)
                    {
                        ddlService.SelectedValue = service;
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblPatientNameError.Text = "";
            lblEmailError.Text = "";
            lblPhoneError.Text = "";
            lblDateError.Text = "";
            lblTimeError.Text = "";
            lblMessage.Text = "";

            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtPatientName.Text))
            { lblPatientNameError.Text = "Patient name is required."; isValid = false; }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(txtPatientName.Text, @"^[A-Za-z\s]+$"))
            { lblPatientNameError.Text = "Letters only."; isValid = false; }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            { lblEmailError.Text = "Enter a valid email."; isValid = false; }

            
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            { lblPhoneError.Text = "Phone is required."; isValid = false; }

            DateTime parsedDate = DateTime.MinValue;
            if (!DateTime.TryParse(txtDate.Text, out parsedDate) || parsedDate.Date < DateTime.Today)
            { lblDateError.Text = "Enter a valid future date."; isValid = false; }

            TimeSpan parsedTime = TimeSpan.Zero;
            if (!TimeSpan.TryParse(txtTime.Text, out parsedTime))
            { lblTimeError.Text = "Enter a valid time."; isValid = false; }

            if (string.IsNullOrWhiteSpace(ddlDoctor.SelectedValue))
            {
                lblMessage.Text = "Please select a doctor.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                isValid = false;
            }

            if (!isValid) return;

            string id = Request.QueryString["id"];
            string connStr = ConfigurationManager.ConnectionStrings["ClinicDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

               
                if (phStatus.Visible == false)
                {
                    query = @"UPDATE Appointments 
                      SET PatientName=@PatientName, Email=@Email, Phone=@Phone,    
                          AppointmentDate=@Date, AppointmentTime=@Time,
                          DoctorName=@DoctorName, Department=@Department, AdditionalService=@AdditionalService
                      WHERE AppointmentID=@ID";
                }
                else
                {
                    query = @"UPDATE Appointments 
                      SET PatientName=@PatientName, Email=@Email, Phone=@Phone,    
                          AppointmentDate=@Date, AppointmentTime=@Time, Status=@Status,
                          DoctorName=@DoctorName, Department=@Department, AdditionalService=@AdditionalService
                      WHERE AppointmentID=@ID";

                    cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);
                }

                cmd.CommandText = query;

                cmd.Parameters.AddWithValue("@PatientName", txtPatientName.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                cmd.Parameters.AddWithValue("@Date", parsedDate.Date);
                cmd.Parameters.AddWithValue("@Time", parsedTime);
                cmd.Parameters.AddWithValue("@DoctorName", ddlDoctor.SelectedValue);
                cmd.Parameters.AddWithValue("@Department", txtDepartment.Text);
                cmd.Parameters.AddWithValue("@AdditionalService", string.IsNullOrWhiteSpace(ddlService.SelectedValue) ? (object)DBNull.Value : ddlService.SelectedValue);
                cmd.Parameters.AddWithValue("@ID", id);

                conn.Open();
                cmd.ExecuteNonQuery();

                lblMessage.Text = "Appointment updated successfully!";
                lblMessage.ForeColor = System.Drawing.Color.LightGreen;
            }
        }
    }
}
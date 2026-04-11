using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;

namespace Clinic_Management_System
{
    public partial class Notifications : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
                Response.Redirect("Login.aspx");

            if (!IsPostBack)
                LoadAppointments();
        }

        private void LoadAppointments()
        {
            string connStr = ConfigurationManager
                .ConnectionStrings["ClinicDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT AppointmentID, 
                    PatientName + ' - ' + DoctorName + ' (' + CONVERT(VARCHAR, AppointmentDate, 103) + ')' AS Display 
                    FROM Appointments";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                var ddl = (System.Web.UI.WebControls.DropDownList)FindControl("ddlAppointment");
                ddl.DataSource = dt;
                ddl.DataTextField = "Display";
                ddl.DataValueField = "AppointmentID";
                ddl.DataBind();
                ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Appointment", ""));
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            var ddl = (System.Web.UI.WebControls.DropDownList)FindControl("ddlAppointment");
            var ddlType = (System.Web.UI.WebControls.DropDownList)FindControl("ddlType");
            var lblMessage = (System.Web.UI.WebControls.Label)FindControl("lblMessage");
            var lblAppointmentError = (System.Web.UI.WebControls.Label)FindControl("lblAppointmentError");
            var lblTypeError = (System.Web.UI.WebControls.Label)FindControl("lblTypeError");

            lblAppointmentError.Text = "";
            lblTypeError.Text = "";
            lblMessage.Text = "";

            bool isValid = true;

            if (string.IsNullOrEmpty(ddl.SelectedValue))
            { lblAppointmentError.Text = "Please select an appointment."; isValid = false; }

            if (string.IsNullOrEmpty(ddlType.SelectedValue))
            { lblTypeError.Text = "Please select notification type."; isValid = false; }

            if (!isValid) return;

            // جيب بيانات الموعد
            string connStr = ConfigurationManager
                .ConnectionStrings["ClinicDBConnection"].ConnectionString;

            string patientName = "", email = "", doctorName = "", date = "";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT PatientName, Email, DoctorName, AppointmentDate FROM Appointments WHERE AppointmentID=@ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", ddl.SelectedValue);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    patientName = reader["PatientName"].ToString();
                    email = reader["Email"].ToString();
                    doctorName = reader["DoctorName"].ToString();
                    date = Convert.ToDateTime(reader["AppointmentDate"]).ToString("dd/MM/yyyy");
                }
            }

            
            try
            {
                string subject = "";
                string body = "";
                string type = ddlType.SelectedValue;

                if (type == "Confirmed")
                {
                    subject = "Appointment Confirmed";
                    body = $"Dear {patientName},\n\nYour appointment with {doctorName} on {date} has been confirmed.\n\nThank you.";
                }
                else if (type == "Reminder")
                {
                    subject = "Appointment Reminder";
                    body = $"Dear {patientName},\n\nThis is a reminder for your appointment with {doctorName} on {date}.\n\nThank you.";
                }
                else if (type == "Rescheduled")
                {
                    subject = "Appointment Rescheduled";
                    body = $"Dear {patientName},\n\nYour appointment with {doctorName} has been rescheduled to {date}.\n\nThank you.";
                }
                else if (type == "Completed")
                {
                    subject = "Appointment Completed";
                    body = $"Dear {patientName},\n\nYour appointment with {doctorName} on {date} has been completed.\n\nThank you.";
                }
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("ghaidasalem71@gmail.com", "qxbw amwe zzvu fnze");
                smtp.EnableSsl = true;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("ghaidasalem71@gmail.com");
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = body;

                smtp.Send(mail);

                lblMessage.ForeColor = System.Drawing.Color.LightGreen;
                lblMessage.Text = "Notification sent successfully!";
            }
            catch (Exception ex)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Failed to send: " + ex.Message;
            }
        }
    }
}
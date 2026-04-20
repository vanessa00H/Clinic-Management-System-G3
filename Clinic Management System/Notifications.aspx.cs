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
        protected global::System.Web.UI.WebControls.DropDownList ddlAppointment;
        protected global::System.Web.UI.WebControls.DropDownList ddlType;
        protected global::System.Web.UI.WebControls.Label lblMessage;
        protected global::System.Web.UI.WebControls.Label lblAppointmentError;
        protected global::System.Web.UI.WebControls.Label lblTypeError;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
                Response.Redirect("Login.aspx");

            if (!IsPostBack)
                LoadAppointments();
        }

        private void LoadAppointments()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ClinicDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT AppointmentID, 
                    PatientName + ' - ' + DoctorName + ' (' + CONVERT(VARCHAR, AppointmentDate, 103) + ')' AS Display 
                    FROM Appointments WHERE Email IS NOT NULL AND Email != ''";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlAppointment.DataSource = dt;
                ddlAppointment.DataTextField = "Display";
                ddlAppointment.DataValueField = "AppointmentID";
                ddlAppointment.DataBind();
                ddlAppointment.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Appointment", ""));
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            lblAppointmentError.Text = "";
            lblTypeError.Text = "";
            lblMessage.Text = "";

            if (string.IsNullOrEmpty(ddlAppointment.SelectedValue))
            {
                lblAppointmentError.Text = "Please select an appointment.";
                return;
            }

            if (string.IsNullOrEmpty(ddlType.SelectedValue))
            {
                lblTypeError.Text = "Please select notification type.";
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["ClinicDBConnection"].ConnectionString;
            string patientName = "", email = "", doctorName = "", date = "";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT PatientName, Email, DoctorName, AppointmentDate FROM Appointments WHERE AppointmentID=@ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", ddlAppointment.SelectedValue);
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
            // check if appointment is cancelled
            string status = "";
            using (SqlConnection conn2 = new SqlConnection(connStr))
            {
                SqlCommand cmd2 = new SqlCommand("SELECT Status FROM Appointments WHERE AppointmentID=@ID", conn2);
                cmd2.Parameters.AddWithValue("@ID", ddlAppointment.SelectedValue);
                conn2.Open();
                status = cmd2.ExecuteScalar()?.ToString() ?? "";
            }

            if (status == "Cancelled" && ddlType.SelectedValue != "Cancelled")
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "❌ Cannot send this notification. Appointment is Cancelled.";
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Error: This patient does not have a registered email.";
                return;
            }

            try
            {
                string subject = "";
                string body = "";
                string type = ddlType.SelectedValue;

                if (type == "Confirmed")
                {
                    subject = "Appointment Confirmed - Clinic System";
                    body = $"Dear {patientName},\n\nGood news! Your appointment with Dr. {doctorName} on {date} has been confirmed.\n\nWe look forward to seeing you.";
                }
                else if (type == "Reminder")
                {
                    subject = "Appointment Reminder";
                    body = $"Dear {patientName},\n\nThis is a friendly reminder for your upcoming appointment with Dr. {doctorName} scheduled for {date}.\n\nPlease arrive 10 minutes early.";
                }
                else if (type == "Rescheduled")
                {
                    subject = "Appointment Rescheduled";
                    body = $"Dear {patientName},\n\nYour appointment with Dr. {doctorName} has been rescheduled to {date}.\n\nIf this time doesn't work for you, please contact us.";
                }
                else if (type == "Completed")
                {
                    subject = "Health Check-up Completed";
                    body = $"Dear {patientName},\n\nYour visit with Dr. {doctorName} is now complete. We hope you had a good experience.\n\nStay healthy!";
                }

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("ghaidasalem71@gmail.com", "qxbw amwe zzvu fnze");
                smtp.EnableSsl = true;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("ghaidasalem71@gmail.com", "Clinic Management System");
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = body;

                smtp.Send(mail);

                lblMessage.ForeColor = System.Drawing.Color.SpringGreen;
                lblMessage.Text = "✅ Notification sent successfully to " + email;
            }
            catch (Exception)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "❌ Failed to send email. Check your internet or App Password.";

            }

        }
    }
}
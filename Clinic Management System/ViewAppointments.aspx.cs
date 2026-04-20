using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;

namespace Clinic_Management_System
{
    public partial class ViewAppointments : System.Web.UI.Page
    {

        private System.Web.UI.WebControls.Label GetMessageLabel()
        {
            return (System.Web.UI.WebControls.Label)FindControl("lblMessage");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadDoctors();
                LoadDepartments();
                LoadAppointments();
            }
        }


        private void LoadDoctors()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ClinicDBConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT DISTINCT DoctorName FROM Appointments WHERE DoctorName IS NOT NULL";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                var ddl = (System.Web.UI.WebControls.DropDownList)FindControl("ddlFilterDoctor");
                if (ddl != null) { ddl.DataSource = dt; ddl.DataTextField = "DoctorName"; ddl.DataValueField = "DoctorName"; ddl.DataBind(); ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All Doctors", "")); }
            }
        }

        private void LoadDepartments()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ClinicDBConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT DISTINCT Department FROM Appointments WHERE Department IS NOT NULL AND Department != ''";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                var ddl = (System.Web.UI.WebControls.DropDownList)FindControl("ddlFilterDepartment");
                if (ddl != null) { ddl.DataSource = dt; ddl.DataTextField = "Department"; ddl.DataValueField = "Department"; ddl.DataBind(); ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All Departments", "")); }
            }
        }

        private void LoadAppointments(string status = "", string doctor = "", string department = "", string fromDate = "", string toDate = "", string sortBy = "AppointmentDate")
        {
            string connStr = ConfigurationManager.ConnectionStrings["ClinicDBConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM Appointments WHERE 1=1";
                string role = Session["Role"]?.ToString().ToLower() ?? "";
                string currentUser = Session["Username"]?.ToString() ?? "";

                if (role == "doctor") query += " AND DoctorName = @CurrentUser";
                else if (role == "patient") query += " AND PatientName = @CurrentUser";

                if (!string.IsNullOrEmpty(status)) query += " AND Status=@Status";
                if (!string.IsNullOrEmpty(doctor)) query += " AND DoctorName=@Doctor";
                if (!string.IsNullOrEmpty(department)) query += " AND Department=@Dept";
                if (!string.IsNullOrEmpty(fromDate)) query += " AND AppointmentDate >= @FromDate";
                if (!string.IsNullOrEmpty(toDate)) query += " AND AppointmentDate <= @ToDate";

                query += " ORDER BY " + sortBy;

                SqlCommand cmd = new SqlCommand(query, conn);
                if (role == "doctor" || role == "patient") cmd.Parameters.AddWithValue("@CurrentUser", currentUser);
                if (!string.IsNullOrEmpty(status)) cmd.Parameters.AddWithValue("@Status", status);
                if (!string.IsNullOrEmpty(doctor)) cmd.Parameters.AddWithValue("@Doctor", doctor);
                if (!string.IsNullOrEmpty(department)) cmd.Parameters.AddWithValue("@Dept", department);
                if (!string.IsNullOrEmpty(fromDate)) cmd.Parameters.AddWithValue("@FromDate", fromDate);
                if (!string.IsNullOrEmpty(toDate)) cmd.Parameters.AddWithValue("@ToDate", toDate);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                var gv = (System.Web.UI.WebControls.GridView)FindControl("gvAppointments");
                if (gv != null) { gv.DataSource = dt; gv.DataBind(); }
            }
        }


        protected void gvAppointments_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {

            string id = e.CommandArgument.ToString();

            if (e.CommandName == "EditApp")
            {
                Response.Redirect("EditAppointment.aspx?id=" + id);
            }
            else if (e.CommandName == "CancelApp")
            {
                // check appointment date before allowing cancellation
                string connStr = ConfigurationManager.ConnectionStrings["ClinicDBConnection"].ConnectionString;
                DateTime appointmentDate = DateTime.MinValue;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand cmd = new SqlCommand("SELECT AppointmentDate FROM Appointments WHERE AppointmentID=@ID", conn);
                    cmd.Parameters.AddWithValue("@ID", id);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                        appointmentDate = Convert.ToDateTime(result);
                }

                // Allow cancellation only if appointment is more than 24 hours away
                // This is a common policy to prevent last-minute cancellations and allow the clinic to manage schedules effectively.
                if (appointmentDate <= DateTime.Now || (appointmentDate - DateTime.Now).TotalHours < 24)
                {
                    var lbl = GetMessageLabel();
                    if (lbl != null)
                    {
                        lbl.Text = "❌ Cannot cancel! Appointment is within 24 hours. Please contact the clinic directly.";
                        lbl.ForeColor = System.Drawing.Color.Red;
                    }
                }
                else
                {
                    UpdateAppointmentStatus(id, "Status", "Cancelled");
                    SendAutomaticEmail(id, "Cancelled");
                    var lbl = GetMessageLabel();
                    if (lbl != null)
                    {
                        lbl.Text = "✅ Appointment cancelled successfully.";
                        lbl.ForeColor = System.Drawing.Color.SpringGreen;
                    }
                }
            }
            else if (e.CommandName == "PayApp")
            {
                // check current status before marking as paid
                string connStr = ConfigurationManager.ConnectionStrings["ClinicDBConnection"].ConnectionString;
                string currentStatus = "";
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand cmd = new SqlCommand("SELECT Status FROM Appointments WHERE AppointmentID=@ID", conn);
                    cmd.Parameters.AddWithValue("@ID", id);
                    conn.Open();
                    currentStatus = cmd.ExecuteScalar()?.ToString() ?? "";
                }

                UpdateAppointmentStatus(id, "PaymentStatus", "Paid");

                if (currentStatus != "Cancelled")
                    SendAutomaticEmail(id, "Paid (Payment Received)");
            }
            else if (e.CommandName == "RemindApp")
            {
                SendAutomaticEmail(id, "Reminder: Your appointment is approaching!");
            }
            else if (e.CommandName == "DeleteApp")
            {
                string connStr = ConfigurationManager.ConnectionStrings["ClinicDBConnection"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = "DELETE FROM Appointments WHERE AppointmentID=@ID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                LoadAppointments();
            }
        }

        private void UpdateAppointmentStatus(string id, string column, string value)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ClinicDBConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = $"UPDATE Appointments SET {column}=@Value WHERE AppointmentID=@ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Value", value);
                cmd.Parameters.AddWithValue("@ID", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            LoadAppointments();
        }

        private void SendAutomaticEmail(string appointmentID, string type)
        {
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["ClinicDBConnection"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = "SELECT PatientName, Email, DoctorName, AppointmentDate FROM Appointments WHERE AppointmentID=@ID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", appointmentID);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string email = reader["Email"].ToString();
                        if (string.IsNullOrEmpty(email)) return;

                        string pName = reader["PatientName"].ToString();
                        string dName = reader["DoctorName"].ToString();
                        string date = Convert.ToDateTime(reader["AppointmentDate"]).ToString("dd/MM/yyyy");

                        string subject = "Clinic Notification: " + type;
                        string body = $"Dear {pName},\n\nYour appointment with Dr. {dName} on {date} has a new update: {type}.\n\nThank you.";

                        SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                        smtp.Credentials = new NetworkCredential("ghaidasalem71@gmail.com", "qxbw amwe zzvu fnze");
                        smtp.EnableSsl = true;

                        MailMessage mail = new MailMessage("ghaidasalem71@gmail.com", email, subject, body);
                        smtp.Send(mail);


                        var lbl = GetMessageLabel();
                        if (lbl != null)
                        {
                            lbl.Text = "✅ Notification sent to " + email;
                            lbl.ForeColor = System.Drawing.Color.SpringGreen;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var lbl = GetMessageLabel();
                if (lbl != null)
                {
                    lbl.Text = "❌ Mail Error: " + ex.Message;
                    lbl.ForeColor = System.Drawing.Color.Yellow;
                }
            }
        }


        protected void btnFilter_Click(object sender, EventArgs e)
        {
            var ddlS = (System.Web.UI.WebControls.DropDownList)FindControl("ddlFilterStatus");
            var ddlD = (System.Web.UI.WebControls.DropDownList)FindControl("ddlFilterDoctor");
            var ddlDep = (System.Web.UI.WebControls.DropDownList)FindControl("ddlFilterDepartment");
            var tF = (System.Web.UI.WebControls.TextBox)FindControl("txtFromDate");
            var tT = (System.Web.UI.WebControls.TextBox)FindControl("txtToDate");
            var ddlSort = (System.Web.UI.WebControls.DropDownList)FindControl("ddlSort");

            LoadAppointments(ddlS?.SelectedValue ?? "", ddlD?.SelectedValue ?? "", ddlDep?.SelectedValue ?? "", tF?.Text ?? "", tT?.Text ?? "", ddlSort?.SelectedValue ?? "AppointmentDate");
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            LoadAppointments();
        }


    }
}
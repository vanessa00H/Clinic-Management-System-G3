using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Clinic_Management_System
{
    public partial class AppointmentNotes : System.Web.UI.Page
    {
        protected global::System.Web.UI.WebControls.DropDownList ddlAppointment;
        protected global::System.Web.UI.WebControls.Label lblAppointmentError;
        protected global::System.Web.UI.WebControls.TextBox txtNote;
        protected global::System.Web.UI.WebControls.Label lblNoteError;
        protected global::System.Web.UI.WebControls.Button btnSave;
        protected global::System.Web.UI.WebControls.Label lblMessage;
        protected global::System.Web.UI.WebControls.GridView gvNotes;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null || Session["Role"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            // 🛡️ تأمين الأدوار (نحولها لحروف صغيرة عشان نتجنب مشاكل الـ Capitalization)
            string role = Session["Role"].ToString().ToLower();
            if (role != "admin" && role != "doctor")
            {
                Response.Redirect("Dashboard.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadAppointments();
                LoadNotes();
            }
        }

        private void LoadAppointments()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ClinicDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // جملة استعلام ذكية تدمج البيانات للعرض
                string query = "SELECT AppointmentID, PatientName + ' - ' + DoctorName + ' (' + CONVERT(VARCHAR, AppointmentDate, 103) + ')' AS Display FROM Appointments";

                // 🌟 إضافة: إذا كان دكتور، يشوف بس مرضاه
                if (Session["Role"].ToString().ToLower() == "doctor")
                {
                    query += " WHERE DoctorName = @DocName";
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                if (Session["Role"].ToString().ToLower() == "doctor")
                {
                    cmd.Parameters.AddWithValue("@DocName", Session["Username"].ToString());
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlAppointment.DataSource = dt;
                ddlAppointment.DataTextField = "Display";
                ddlAppointment.DataValueField = "AppointmentID";
                ddlAppointment.DataBind();
                ddlAppointment.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Appointment", ""));
            }
        }

        private void LoadNotes()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ClinicDBConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT PatientName, DoctorName, AppointmentDate, Notes FROM Appointments WHERE Notes IS NOT NULL AND Notes != ''";

                // 🌟 إضافة: الدكتور يشوف ملاحظاته هو بس
                if (Session["Role"].ToString().ToLower() == "doctor")
                {
                    query += " AND DoctorName = @DocName";
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                if (Session["Role"].ToString().ToLower() == "doctor")
                {
                    cmd.Parameters.AddWithValue("@DocName", Session["Username"].ToString());
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvNotes.DataSource = dt;
                gvNotes.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblAppointmentError.Text = "";
            lblNoteError.Text = "";
            lblMessage.Text = "";

            bool isValid = true;

            if (string.IsNullOrEmpty(ddlAppointment.SelectedValue))
            {
                lblAppointmentError.Text = "Please select an appointment.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtNote.Text))
            {
                lblNoteError.Text = "Note cannot be empty.";
                isValid = false;
            }

            if (!isValid) return;

            string connStr = ConfigurationManager.ConnectionStrings["ClinicDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "UPDATE Appointments SET Notes=@Notes WHERE AppointmentID=@ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Notes", txtNote.Text.Trim());
                cmd.Parameters.AddWithValue("@ID", ddlAppointment.SelectedValue);

                conn.Open();
                cmd.ExecuteNonQuery();

                lblMessage.Text = "Note saved successfully!";
                lblMessage.ForeColor = System.Drawing.Color.SpringGreen;
                txtNote.Text = "";
            }

            LoadNotes();
        }
    }
}
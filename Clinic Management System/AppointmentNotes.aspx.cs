using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Clinic_Management_System
{
    public partial class AppointmentNotes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
                Response.Redirect("Login.aspx");

            string role = Session["Role"].ToString();
            if (role != "Admin" && role != "Doctor")
                Response.Redirect("Dashboard.aspx");

            if (!IsPostBack)
            {
                LoadAppointments();
                LoadNotes();
            }
        }

        private void LoadAppointments()
        {
            string connStr = ConfigurationManager
                .ConnectionStrings["ClinicDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT AppointmentID, PatientName + ' - ' + DoctorName + ' (' + CONVERT(VARCHAR, AppointmentDate, 103) + ')' AS Display FROM Appointments";
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

        private void LoadNotes()
        {
            string connStr = ConfigurationManager
                .ConnectionStrings["ClinicDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT PatientName, DoctorName, AppointmentDate, Notes FROM Appointments WHERE Notes IS NOT NULL AND Notes != ''";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                var gv = (System.Web.UI.WebControls.GridView)FindControl("gvNotes");
                gv.DataSource = dt;
                gv.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var ddl = (System.Web.UI.WebControls.DropDownList)FindControl("ddlAppointment");
            var txtNote = (System.Web.UI.WebControls.TextBox)FindControl("txtNote");
            var lblMessage = (System.Web.UI.WebControls.Label)FindControl("lblMessage");
            var lblAppointmentError = (System.Web.UI.WebControls.Label)FindControl("lblAppointmentError");
            var lblNoteError = (System.Web.UI.WebControls.Label)FindControl("lblNoteError");

            lblAppointmentError.Text = "";
            lblNoteError.Text = "";
            lblMessage.Text = "";

            bool isValid = true;

            if (string.IsNullOrEmpty(ddl.SelectedValue))
            { lblAppointmentError.Text = "Please select an appointment."; isValid = false; }

            if (string.IsNullOrWhiteSpace(txtNote.Text))
            { lblNoteError.Text = "Note cannot be empty."; isValid = false; }

            if (!isValid) return;

            string connStr = ConfigurationManager
                .ConnectionStrings["ClinicDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "UPDATE Appointments SET Notes=@Notes WHERE AppointmentID=@ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Notes", txtNote.Text.Trim());
                cmd.Parameters.AddWithValue("@ID", ddl.SelectedValue);

                conn.Open();
                cmd.ExecuteNonQuery();

                lblMessage.Text = "Note saved successfully!";
                lblMessage.ForeColor = System.Drawing.Color.LightGreen;
                txtNote.Text = "";
            }

            LoadNotes();
        }
    }
}
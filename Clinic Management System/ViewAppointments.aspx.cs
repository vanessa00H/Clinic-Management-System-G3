using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Clinic_Management_System
{
    public partial class ViewAppointments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
                Response.Redirect("Login.aspx");

            if (!IsPostBack)
            {
                LoadDoctors();
                LoadAppointments();
            }
        }

        private void LoadDoctors()
        {
            string connStr = ConfigurationManager
                .ConnectionStrings["ClinicDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT DISTINCT DoctorName FROM Appointments";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                var ddlFilterDoctor = (System.Web.UI.WebControls.DropDownList)FindControl("ddlFilterDoctor");
                ddlFilterDoctor.DataSource = dt;
                ddlFilterDoctor.DataTextField = "DoctorName";
                ddlFilterDoctor.DataValueField = "DoctorName";
                ddlFilterDoctor.DataBind();
                ddlFilterDoctor.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All Doctors", ""));
            }
        }

        private void LoadAppointments(string status = "", string doctor = "",
            string fromDate = "", string toDate = "", string sortBy = "AppointmentDate")
        {
            string connStr = ConfigurationManager
                .ConnectionStrings["ClinicDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM Appointments WHERE 1=1";

                if (!string.IsNullOrEmpty(status))
                    query += " AND Status=@Status";
                if (!string.IsNullOrEmpty(doctor))
                    query += " AND DoctorName=@Doctor";
                if (!string.IsNullOrEmpty(fromDate))
                    query += " AND AppointmentDate >= @FromDate";
                if (!string.IsNullOrEmpty(toDate))
                    query += " AND AppointmentDate <= @ToDate";

                query += " ORDER BY " + sortBy;

                SqlCommand cmd = new SqlCommand(query, conn);

                if (!string.IsNullOrEmpty(status))
                    cmd.Parameters.AddWithValue("@Status", status);
                if (!string.IsNullOrEmpty(doctor))
                    cmd.Parameters.AddWithValue("@Doctor", doctor);
                if (!string.IsNullOrEmpty(fromDate))
                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                if (!string.IsNullOrEmpty(toDate))
                    cmd.Parameters.AddWithValue("@ToDate", toDate);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                var gv = (System.Web.UI.WebControls.GridView)FindControl("gvAppointments");
                if (gv != null)
                {
                    gv.DataSource = dt;
                    gv.DataBind();
                }
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            var ddlStatus = (System.Web.UI.WebControls.DropDownList)FindControl("ddlFilterStatus");
            var ddlDoctor = (System.Web.UI.WebControls.DropDownList)FindControl("ddlFilterDoctor");
            var txtFrom = (System.Web.UI.WebControls.TextBox)FindControl("txtFromDate");
            var ddlSort = (System.Web.UI.WebControls.DropDownList)FindControl("ddlSort");

            LoadAppointments(
                ddlStatus.SelectedValue,
                ddlDoctor.SelectedValue,
                txtFrom.Text,
                "",
                ddlSort.SelectedValue
            );
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            var ddlStatus = (System.Web.UI.WebControls.DropDownList)FindControl("ddlFilterStatus");
            var ddlDoctor = (System.Web.UI.WebControls.DropDownList)FindControl("ddlFilterDoctor");
            var txtFrom = (System.Web.UI.WebControls.TextBox)FindControl("txtFromDate");
            var txtTo = (System.Web.UI.WebControls.TextBox)FindControl("txtToDate");

            ddlStatus.SelectedIndex = 0;
            ddlDoctor.SelectedIndex = 0;
            txtFrom.Text = "";
            txtTo.Text = "";

            LoadAppointments();
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
                string connStr = ConfigurationManager
                    .ConnectionStrings["ClinicDBConnection"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = "UPDATE Appointments SET Status='Cancelled' WHERE AppointmentID=@ID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                LoadAppointments();
            }
        }
    }
    }
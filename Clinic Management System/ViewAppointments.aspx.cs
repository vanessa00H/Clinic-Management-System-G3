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
                LoadAppointments();
        }

        private void LoadAppointments()
        {
            string connStr = ConfigurationManager
                .ConnectionStrings["ClinicDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM Appointments";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
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
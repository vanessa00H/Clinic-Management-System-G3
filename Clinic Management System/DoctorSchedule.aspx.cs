using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Clinic_Management_System
{
    public partial class DoctorSchedule : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
                Response.Redirect("Login.aspx");
            string role = Session["Role"]?.ToString() ?? "";

            // الآدمن والدكتور يقدرون يشوفون هذي الصفحة
            if (role != "Admin" && role.ToLower() != "doctor")
            {
                Response.Redirect("Dashboard.aspx");
            }

            if (!IsPostBack)
                LoadDoctors();
        }

        private void LoadDoctors()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ClinicDBConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM Doctors";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                var gv = (System.Web.UI.WebControls.GridView)FindControl("gvDoctors");
                if (gv != null)
                {
                    gv.DataSource = dt;
                    gv.DataBind();
                }
            }
        }

        // كود تغيير حالة التوفر فقط (Toggle)
        protected void gvDoctors_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ToggleDoc")
            {
                string id = e.CommandArgument.ToString();
                string connStr = ConfigurationManager.ConnectionStrings["ClinicDBConnection"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE Doctors SET IsAvailable = CASE WHEN IsAvailable=1 THEN 0 ELSE 1 END WHERE DoctorID=@ID", conn);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();
                }
                LoadDoctors();
            }
        }
    }
}
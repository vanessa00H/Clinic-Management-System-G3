using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Clinic_Management_System
{
    public partial class DoctorSchedule : System.Web.UI.Page
    {
        protected global::System.Web.UI.WebControls.GridView gvDoctors;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
             
                if (Session["Role"] != null && Session["Role"].ToString() != "Admin")
                {
                 
                    gvDoctors.Columns[5].Visible = false;
                }

        
                LoadDoctors();
            }
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
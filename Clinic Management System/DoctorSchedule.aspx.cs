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

         
            if (role != "Admin" && role.ToLower() != "doctor")
            {
                Response.Redirect("Dashboard.aspx");
            }

            if (!IsPostBack)
                LoadDoctors();
        }

        private void LoadDoctors()
        {
            string connStr = ConfigurationManager
                .ConnectionStrings["ClinicDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM Doctors";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                var gv = (System.Web.UI.WebControls.GridView)FindControl("gvDoctors");
                gv.DataSource = dt;
                gv.DataBind();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            var txtDoctorName = (System.Web.UI.WebControls.TextBox)FindControl("txtDoctorName");
            var txtSpecialization = (System.Web.UI.WebControls.TextBox)FindControl("txtSpecialization");
            var txtDepartment = (System.Web.UI.WebControls.TextBox)FindControl("txtDepartment");
            var txtFee = (System.Web.UI.WebControls.TextBox)FindControl("txtFee");
            var ddlAvailable = (System.Web.UI.WebControls.DropDownList)FindControl("ddlAvailable");
            var lblMessage = (System.Web.UI.WebControls.Label)FindControl("lblMessage");
            var lblDoctorNameError = (System.Web.UI.WebControls.Label)FindControl("lblDoctorNameError");
            var lblSpecializationError = (System.Web.UI.WebControls.Label)FindControl("lblSpecializationError");
            var lblDepartmentError = (System.Web.UI.WebControls.Label)FindControl("lblDepartmentError");
            var lblFeeError = (System.Web.UI.WebControls.Label)FindControl("lblFeeError");

            // Clear errors
            lblDoctorNameError.Text = "";
            lblSpecializationError.Text = "";
            lblDepartmentError.Text = "";
            lblFeeError.Text = "";
            lblMessage.Text = "";

            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtDoctorName.Text))
            { lblDoctorNameError.Text = "Doctor name is required."; isValid = false; }

            if (string.IsNullOrWhiteSpace(txtSpecialization.Text))
            { lblSpecializationError.Text = "Specialization is required."; isValid = false; }

            if (string.IsNullOrWhiteSpace(txtDepartment.Text))
            { lblDepartmentError.Text = "Department is required."; isValid = false; }

            decimal fee;
            if (!decimal.TryParse(txtFee.Text, out fee) || fee < 0)
            { lblFeeError.Text = "Enter a valid fee."; isValid = false; }

            if (!isValid) return;

            string connStr = ConfigurationManager
                .ConnectionStrings["ClinicDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"INSERT INTO Doctors 
                    (DoctorName, Specialization, Department, ConsultationFee, IsAvailable)
                    VALUES (@Name, @Spec, @Dept, @Fee, @Available)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", txtDoctorName.Text);
                cmd.Parameters.AddWithValue("@Spec", txtSpecialization.Text);
                cmd.Parameters.AddWithValue("@Dept", txtDepartment.Text);
                cmd.Parameters.AddWithValue("@Fee", fee);
                cmd.Parameters.AddWithValue("@Available", ddlAvailable.SelectedValue);

                conn.Open();
                cmd.ExecuteNonQuery();

                lblMessage.Text = "Doctor added successfully!";
                lblMessage.ForeColor = System.Drawing.Color.LightGreen;

                txtDoctorName.Text = "";
                txtSpecialization.Text = "";
                txtDepartment.Text = "";
                txtFee.Text = "";
            }

            LoadDoctors();
        }

        protected void gvDoctors_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();
            string connStr = ConfigurationManager
                .ConnectionStrings["ClinicDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                if (e.CommandName == "DeleteDoc")
                {
                    SqlCommand cmd = new SqlCommand(
                        "DELETE FROM Doctors WHERE DoctorID=@ID", conn);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();
                }
                else if (e.CommandName == "ToggleDoc")
                {
                    SqlCommand cmd = new SqlCommand(
                        "UPDATE Doctors SET IsAvailable = CASE WHEN IsAvailable=1 THEN 0 ELSE 1 END WHERE DoctorID=@ID", conn);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();
                }
            }

            LoadDoctors();
        }
    }
}
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Clinic_Management_System
{
    public partial class ManageDoctors : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["ClinicDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"]?.ToString() != "Admin") Response.Redirect("Dashboard.aspx");
        }

        // ميزة التعبئة التلقائية عند كتابة الاسم
        protected void txtDoctorName_TextChanged(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM Doctors WHERE DoctorName = @Name";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", txtDoctorName.Text.Trim());
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtSpecialization.Text = reader["Specialization"].ToString();
                    txtDepartment.Text = reader["Department"].ToString();
                    txtFee.Text = reader["ConsultationFee"].ToString();
                    ddlAvailable.SelectedValue = Convert.ToBoolean(reader["IsAvailable"]) ? "1" : "0";
                    lblMessage.Text = "Doctor found! You can Update or Delete.";
                    lblMessage.ForeColor = System.Drawing.Color.SpringGreen;
                }
                else
                {
                    // إذا لم يجد الاسم، يمسح الحقول للاستعداد للإضافة
                    txtSpecialization.Text = "";
                    txtDepartment.Text = "";
                    txtFee.Text = "";
                    lblMessage.Text = "New Doctor Name. Ready to Add.";
                    lblMessage.ForeColor = System.Drawing.Color.White;
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ExecuteQuery("INSERT INTO Doctors (DoctorName, Specialization, Department, ConsultationFee, IsAvailable) VALUES (@Name, @Spec, @Dept, @Fee, @Avail)", "Added Successfully!");
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            ExecuteQuery("UPDATE Doctors SET Specialization=@Spec, Department=@Dept, ConsultationFee=@Fee, IsAvailable=@Avail WHERE DoctorName=@Name", "Updated Successfully!");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ExecuteQuery("DELETE FROM Doctors WHERE DoctorName=@Name", "Deleted Successfully!");
        }

        private void ExecuteQuery(string query, string successMsg)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", txtDoctorName.Text.Trim());
                cmd.Parameters.AddWithValue("@Spec", txtSpecialization.Text.Trim());
                cmd.Parameters.AddWithValue("@Dept", txtDepartment.Text.Trim());
                cmd.Parameters.AddWithValue("@Fee", txtFee.Text.Trim());
                cmd.Parameters.AddWithValue("@Avail", ddlAvailable.SelectedValue);
                conn.Open();
                cmd.ExecuteNonQuery();
                lblMessage.Text = successMsg;
                lblMessage.ForeColor = System.Drawing.Color.Yellow;
            }
        }
    }
}
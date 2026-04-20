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
           
            if (Session["Role"]?.ToString().ToLower() != "admin")
            {
                Response.Redirect("Dashboard.aspx");
            }
        }

       
        protected void txtDoctorName_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDoctorName.Text)) return;
            
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM Doctors WHERE DoctorName = @Name";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", txtDoctorName.Text.Trim());
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    ViewState["OldName"] = txtDoctorName.Text.Trim();
                    txtSpecialization.Text = reader["Specialization"].ToString();
                    txtDepartment.Text = reader["Department"].ToString();
                    txtFee.Text = reader["ConsultationFee"].ToString();
                    ddlAvailable.SelectedValue = Convert.ToBoolean(reader["IsAvailable"]) ? "1" : "0";
                    lblMessage.Text = "Doctor found! You can Update or Delete.";
                    lblMessage.ForeColor = System.Drawing.Color.SpringGreen;
                }
                else
                {
                    
                    if (string.IsNullOrWhiteSpace(txtSpecialization.Text) &&
                        string.IsNullOrWhiteSpace(txtDepartment.Text))
                    {
                        lblMessage.Text = "New Doctor Name. Ready to Add.";
                        lblMessage.ForeColor = System.Drawing.Color.White;
                    }
                    else
                    {
                        lblMessage.Text = "Editing doctor name. Click Update to save.";
                        lblMessage.ForeColor = System.Drawing.Color.Yellow;
                    }
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ExecuteQuery("INSERT INTO Doctors (DoctorName, Specialization, Department, ConsultationFee, IsAvailable) VALUES (@Name, @Spec, @Dept, @Fee, @Avail)", "Added Successfully!");
        }
        
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string oldName = ViewState["OldName"]?.ToString() ?? txtDoctorName.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "UPDATE Doctors SET DoctorName=@NewName, Specialization=@Spec, Department=@Dept, ConsultationFee=@Fee, IsAvailable=@Avail WHERE DoctorName=@OldName";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OldName", oldName);
                cmd.Parameters.AddWithValue("@NewName", txtDoctorName.Text.Trim());
                cmd.Parameters.AddWithValue("@Spec", txtSpecialization.Text.Trim());
                cmd.Parameters.AddWithValue("@Dept", txtDepartment.Text.Trim());
                cmd.Parameters.AddWithValue("@Fee", decimal.Parse(txtFee.Text.Trim()));
                cmd.Parameters.AddWithValue("@Avail", ddlAvailable.SelectedValue);
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    lblMessage.Text = "Updated Successfully!";
                    lblMessage.ForeColor = System.Drawing.Color.SpringGreen;
                    ViewState["OldName"] = txtDoctorName.Text.Trim();
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDoctorName.Text))
            {
                lblMessage.Text = "Please enter a Doctor Name.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            string docName = txtDoctorName.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                
                string checkQuery = "SELECT COUNT(*) FROM Appointments WHERE DoctorName = @Name";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@Name", docName);

                conn.Open();
                int appCount = (int)checkCmd.ExecuteScalar();

               
                if (appCount > 0)
                {
                    lblMessage.Text = $"Cannot delete! This doctor has {appCount} booked appointment(s). Please set to 'Unavailable' instead.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    return; 
                }

                
                string deleteQuery = "DELETE FROM Doctors WHERE DoctorName = @Name";
                SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn);
                deleteCmd.Parameters.AddWithValue("@Name", docName);

                int rows = deleteCmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    lblMessage.Text = "Deleted Successfully!";
                    lblMessage.ForeColor = System.Drawing.Color.SpringGreen;

                    
                    txtDoctorName.Text = "";
                    txtSpecialization.Text = "";
                    txtDepartment.Text = "";
                    txtFee.Text = "";
                }
            }
        }

        private void ExecuteQuery(string query, string successMsg)
        {
            
            if (string.IsNullOrWhiteSpace(txtDoctorName.Text))
            {
                lblMessage.Text = "Please enter a Doctor Name.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            
            decimal fee = 0;
            if (!query.Contains("DELETE") && !decimal.TryParse(txtFee.Text.Trim(), out fee))
            {
                lblMessage.Text = "Please enter a valid number for the fee.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", txtDoctorName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Spec", txtSpecialization.Text.Trim());
                    cmd.Parameters.AddWithValue("@Dept", txtDepartment.Text.Trim());
                    cmd.Parameters.AddWithValue("@Fee", fee);
                    cmd.Parameters.AddWithValue("@Avail", ddlAvailable.SelectedValue);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        lblMessage.Text = successMsg;
                        lblMessage.ForeColor = System.Drawing.Color.SpringGreen;
                    }
                    else
                    {
                        lblMessage.Text = "No changes were made. Make sure the name is correct.";
                        lblMessage.ForeColor = System.Drawing.Color.Orange;
                    }
                }
            }
            catch (SqlException ex)
            {
               
                if (ex.Number == 547) 
                {
                    lblMessage.Text = "Cannot delete this doctor! They have booked appointments. Please set them as 'Unavailable' instead.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    lblMessage.Text = "Database Error: " + ex.Message;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }
}
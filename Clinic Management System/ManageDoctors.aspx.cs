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
            // تأكدنا إن الحرف الأول كابيتال أو سمول يمشي
            if (Session["Role"]?.ToString().ToLower() != "admin")
            {
                Response.Redirect("Dashboard.aspx");
            }
        }

        // ميزة التعبئة التلقائية عند كتابة الاسم
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
            if (string.IsNullOrWhiteSpace(txtDoctorName.Text))
            {
                lblMessage.Text = "Please enter a Doctor Name.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            string docName = txtDoctorName.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // 🌟 1. التفتيش اليدوي: هل الدكتور عنده مواعيد؟
                string checkQuery = "SELECT COUNT(*) FROM Appointments WHERE DoctorName = @Name";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@Name", docName);

                conn.Open();
                int appCount = (int)checkCmd.ExecuteScalar();

                // إذا عنده مواعيد (أكثر من صفر)، نوقف الحذف فوراً!
                if (appCount > 0)
                {
                    lblMessage.Text = $"Cannot delete! This doctor has {appCount} booked appointment(s). Please set to 'Unavailable' instead.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    return; // هذي توقف الكود وما تخليه يكمل للحذف
                }

                // 🌟 2. إذا طلع نظيف وما عنده مواعيد، نحذفه بأمان
                string deleteQuery = "DELETE FROM Doctors WHERE DoctorName = @Name";
                SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn);
                deleteCmd.Parameters.AddWithValue("@Name", docName);

                int rows = deleteCmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    lblMessage.Text = "Deleted Successfully!";
                    lblMessage.ForeColor = System.Drawing.Color.SpringGreen;

                    // تنظيف الخانات بعد الحذف
                    txtDoctorName.Text = "";
                    txtSpecialization.Text = "";
                    txtDepartment.Text = "";
                    txtFee.Text = "";
                }
            }
        }

        private void ExecuteQuery(string query, string successMsg)
        {
            // 🛡️ حماية 1: التأكد من كتابة الاسم
            if (string.IsNullOrWhiteSpace(txtDoctorName.Text))
            {
                lblMessage.Text = "Please enter a Doctor Name.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // 🛡️ حماية 2: التأكد إن السعر رقم صحيح (فقط في حال الإضافة والتعديل)
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
                // 🛡️ حماية 3: منع انهيار النظام إذا حاولنا نحذف دكتور عنده مواعيد
                if (ex.Number == 547) // 547 هو كود الخطأ حق الـ Foreign Key
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
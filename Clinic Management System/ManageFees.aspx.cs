using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace Clinic_Management_System
{
    public partial class ManageFees : System.Web.UI.Page
    {
        protected global::System.Web.UI.WebControls.DropDownList ddlDepartment;
        protected global::System.Web.UI.WebControls.TextBox txtDate;
        protected global::System.Web.UI.WebControls.TextBox txtFee;
        protected global::System.Web.UI.WebControls.Label lblMessage;
        protected global::System.Web.UI.WebControls.GridView gvSpecialFees;

        string connStr = ConfigurationManager.ConnectionStrings["ClinicDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Role"].ToString().ToLower() == "patient")
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadSpecialFees();
            }
        }

        private void LoadSpecialFees()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT FeeID, Department, SpecialDate, FeeAmount FROM SpecialFees ORDER BY SpecialDate DESC";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvSpecialFees.DataSource = dt;
                gvSpecialFees.DataBind();
            }
        }

        protected void btnSaveFee_Click(object sender, EventArgs e)
        {
            // 1. التحقق من التاريخ
            if (string.IsNullOrWhiteSpace(txtDate.Text))
            {
                lblMessage.Text = "⚠️ Please select a date.";
                lblMessage.ForeColor = System.Drawing.Color.Yellow;
                return;
            }

            // 2. التحقق من السعر
            if (string.IsNullOrWhiteSpace(txtFee.Text))
            {
                lblMessage.Text = "⚠️ Please enter the fee amount.";
                lblMessage.ForeColor = System.Drawing.Color.Yellow;
                return;
            }

            // 3. التأكد إن السعر مو بالسالب
            decimal feeAmount;
            if (!decimal.TryParse(txtFee.Text, out feeAmount) || feeAmount < 0)
            {
                lblMessage.Text = "❌ Fee amount cannot be negative!";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = "INSERT INTO SpecialFees (Department, SpecialDate, FeeAmount) VALUES (@Dept, @Date, @Fee)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Dept", ddlDepartment.SelectedValue);
                    cmd.Parameters.AddWithValue("@Date", txtDate.Text);
                    cmd.Parameters.AddWithValue("@Fee", feeAmount);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                lblMessage.Text = "✅ Special fee saved successfully!";
                lblMessage.ForeColor = System.Drawing.Color.SpringGreen;

                txtDate.Text = "";
                txtFee.Text = "";
                LoadSpecialFees(); // تحديث الجدول فوراً
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Error saving fee: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void gvSpecialFees_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteFee")
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = "DELETE FROM SpecialFees WHERE FeeID = @ID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", e.CommandArgument.ToString());
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                lblMessage.Text = "🗑️ Fee deleted successfully.";
                lblMessage.ForeColor = System.Drawing.Color.Yellow;
                LoadSpecialFees(); // تحديث الجدول بعد الحذف
            }
        }
    }
}
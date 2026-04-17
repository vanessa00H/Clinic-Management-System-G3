using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Clinic_Management_System
{
    // 🌟 غيرنا اسم الكلاس هنا عشان يتطابق مع الصفحة الجديدة
    public partial class ConsultationFees : System.Web.UI.Page
    {
        // سطر الربط السحري عشان الفيجوال ستوديو
        protected global::System.Web.UI.WebControls.GridView gvDoctors;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // الصفحة هذي للجميع (مرضى، ريسبشن، مدراء) للعرض فقط
                // فما نحتاج أي شروط إخفاء هنا، بس نحمل البيانات!
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

                // استخدام FindControl عشان نتخطى مشاكل الديزاينر
                var gv = (System.Web.UI.WebControls.GridView)FindControl("gvDoctors");
                if (gv != null)
                {
                    gv.DataSource = dt;
                    gv.DataBind();
                }
            }
        }
    }
}
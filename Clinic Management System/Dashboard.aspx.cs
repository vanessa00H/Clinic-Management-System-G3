using System;

namespace Clinic_Management_System
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["Username"] == null || Session["Role"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            lblUsername.Text = Session["Username"].ToString();

            if (!IsPostBack)
            {
                string role = Session["Role"].ToString();

                cardBook.Visible = false;
                cardView.Visible = false;
                cardDoctors.Visible = false;
                cardSchedule.Visible = false;
                cardFees.Visible = false;
                cardNotes.Visible = false;
                cardNotifications.Visible = false;

                if (role == "Admin")
                {
                    cardBook.Visible = true;
                    cardView.Visible = true;
                    cardDoctors.Visible = true;
                    cardSchedule.Visible = true;
                    cardFees.Visible = true;
                    cardNotes.Visible = true;
                    cardNotifications.Visible = true;
                }
                else if (role == "Patient")
                {
                    cardBook.Visible = true;
                    cardView.Visible = true;
                    cardSchedule.Visible = true;
                    cardFees.Visible = true;
                }
                else if (role == "Doctor")
                {
                    cardView.Visible = true;
                    cardSchedule.Visible = true;
                    cardNotes.Visible = true;
                }
                else if (role == "Receptionist")
                {
                    cardBook.Visible = true;
                    cardView.Visible = true;
                    cardSchedule.Visible = true;
                    cardFees.Visible = true;
                    cardNotifications.Visible = true;
                }
            }
            if (Session["Role"] != null)
            {
               
                if (Session["Role"].ToString().ToLower() != "admin")
                {
                    cardSpecialFees.Visible = false;
                }
            }
        }
        
        protected void btnOpenSchedule_Click(object sender, EventArgs e)
        {
            Response.Redirect("DoctorSchedule.aspx");
        }
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
    }
}
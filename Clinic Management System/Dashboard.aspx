<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Clinic_Management_System.Dashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Clinic Dashboard</title>
    <style>
        * {
            box-sizing: border-box;
        }

        body {
            margin: 0;
            padding: 0;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #0f172a, #1d4ed8, #38bdf8);
            min-height: 100vh;
        }

        .background-circles {
            position: fixed;
            width: 100%;
            height: 100%;
            overflow: hidden;
            z-index: 0;
        }

        .circle {
            position: absolute;
            border-radius: 50%;
            background: rgba(255,255,255,0.10);
            filter: blur(8px);
        }

        .circle1 {
            width: 220px;
            height: 220px;
            top: 60px;
            left: 70px;
        }

        .circle2 {
            width: 300px;
            height: 300px;
            bottom: 40px;
            right: 60px;
        }

        .circle3 {
            width: 150px;
            height: 150px;
            top: 50%;
            left: 15%;
        }

        .main-container {
            position: relative;
            z-index: 1;
            min-height: 100vh;
            padding: 40px 20px;
        }

        .dashboard-header {
            max-width: 1200px;
            margin: 0 auto 30px auto;
            color: white;
            text-align: center;
        }

            .dashboard-header h1 {
                margin: 0;
                font-size: 34px;
            }

            .dashboard-header p {
                margin-top: 10px;
                font-size: 16px;
                color: #dbeafe;
            }

        .cards-container {
            max-width: 1200px;
            margin: 0 auto;
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
            gap: 22px;
        }

        .card {
            background: rgba(255, 255, 255, 0.18);
            backdrop-filter: blur(16px);
            -webkit-backdrop-filter: blur(16px);
            border: 1px solid rgba(255,255,255,0.22);
            border-radius: 22px;
            padding: 24px;
            color: white;
            box-shadow: 0 8px 24px rgba(0,0,0,0.20);
            transition: 0.25s ease;
        }

            .card:hover {
                transform: translateY(-4px);
            }

        .card-title {
            font-size: 20px;
            font-weight: 700;
            margin-bottom: 10px;
        }

        .card-text {
            font-size: 14px;
            color: #e0f2fe;
            line-height: 1.6;
            margin-bottom: 18px;
            min-height: 85px;
        }

        .card a {
            display: inline-block;
            text-decoration: none;
            color: white;
            background: linear-gradient(90deg, #06b6d4, #2563eb);
            padding: 10px 16px;
            border-radius: 12px;
            font-weight: 700;
        }

        .logout-box {
            max-width: 1200px;
            margin: 30px auto 0 auto;
            text-align: center;
        }

        .logout-btn {
            border: none;
            padding: 12px 22px;
            border-radius: 14px;
            background: #dc2626;
            color: white;
            font-size: 15px;
            font-weight: 700;
            cursor: pointer;
        }

            .logout-btn:hover {
                background: #b91c1c;
            }
    </style>
</head>
<body>
    <div class="background-circles">
        <div class="circle circle1"></div>
        <div class="circle circle2"></div>
        <div class="circle circle3"></div>
    </div>

    <form id="form1" runat="server">
        <div class="main-container">
            <div class="dashboard-header">
                <h1>Clinic Appointment and Patient Management System</h1>
                <p>Welcome,
                    <asp:Label ID="lblUsername" runat="server"></asp:Label></p>
            </div>

            <div class="cards-container">

                <div id="cardBook" runat="server" class="card">
                    <div class="card-title">Book Appointment</div>
                    <div class="card-text">
                        Create a new appointment with patient details, doctor, department, date, time, status, and additional services.
                    </div>
                    <a href="BookAppointment.aspx">Open</a>
                </div>

                <div id="cardView" runat="server" class="card">
                    <div class="card-title">View & Manage Appointments</div>
                    <div class="card-text">
                        View appointment details, update patient data, edit doctor/date/time, cancel, or delete appointments.
                    </div>
                    <a href="ViewAppointments.aspx">Open</a>
                </div>



                <div id="cardDoctors" runat="server" class="card">
                    <div class="card-title">Manage Doctors & Services</div>
                    <div class="card-text">
                        Add, update, or remove doctors and clinic services with specialty, consultation fee, and availability details.
                    </div>
                    <a href="ManageDoctors.aspx">Open</a>
                </div>

                <div id="cardSchedule" runat="server" class="card">
                    <div class="card-title">Doctor Schedule & Availability</div>
                    <div class="card-text">
                        Manage doctor schedules, available slots, unavailable periods, leave, emergencies, and schedule changes.
                    </div>
                    <a href="DoctorSchedule.aspx">Open</a>
                </div>

                <div id="cardFees" runat="server" class="card">
                    <div class="card-title">Consultation Fees</div>
                    <div class="card-text">
                        Manage consultation fees by specialization, appointment type, and bundled or follow-up services.
                    </div>
                    <a href="ConsultationFees.aspx">Open</a>
                </div>

                <div id="cardNotes" runat="server" class="card">
                    <div class="card-title">Appointment Notes</div>
                    <div class="card-text">
                        Add and display internal notes such as symptoms, special instructions, and patient preferences.
                    </div>
                    <a href="AppointmentNotes.aspx">Open</a>
                </div>

                <div id="cardNotifications" runat="server" class="card">
                    <div class="card-title">Notifications</div>
                    <div class="card-text">
                        Send confirmation, reminder, reschedule, and completion notifications to patients by email.
                    </div>
                    <a href="Notifications.aspx">Open</a>
                </div>
                <div id="cardSpecialFees" runat="server" class="card">
                    <div class="card-title">Special Fees</div>
                    <div class="card-text">
                        Set promotional prices and special consultation fees for specific dates or departments.
                    </div>
                    <a href="ManageFees.aspx">Open</a>
                </div>
            </div>

            <div class="logout-box">
                <asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="logout-btn" OnClick="btnLogout_Click" />
            </div>
        </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" 
    CodeBehind="Notifications.aspx.cs" 
    Inherits="Clinic_Management_System.Notifications" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Notifications</title>
    <style>
        * { box-sizing: border-box; }
        body {
            margin: 0; padding: 0;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #0f172a, #1d4ed8, #38bdf8);
            min-height: 100vh;
        }
        .main-container {
            position: relative; z-index: 1;
            min-height: 100vh;
            padding: 40px;
            display: flex;
            justify-content: center;
            align-items: flex-start;
        }
        .card {
            width: 100%;
            max-width: 700px;
            background: rgba(255,255,255,0.18);
            backdrop-filter: blur(16px);
            border: 1px solid rgba(255,255,255,0.22);
            border-radius: 24px;
            padding: 32px; color: white;
            box-shadow: 0 8px 24px rgba(0,0,0,0.20);
        }
        .title { text-align: center; font-size: 30px; font-weight: 700; margin-bottom: 8px; }
        .subtitle { text-align: center; color: #dbeafe; margin-bottom: 24px; }
        .label { display: block; font-weight: 600; margin-bottom: 8px; margin-top: 16px; }
        .dropdown, .textbox {
            width: 100%; padding: 13px;
            border-radius: 14px; border: none; outline: none;
            font-size: 15px; background: rgba(255,255,255,0.92); color: #0f172a;
        }
        .field-message {
            display: block; margin-top: 6px; min-height: 18px;
            font-size: 12px; font-weight: 600; color: #ffe4e6;
        }
        .btn {
            width: 100%; padding: 14px; margin-top: 20px;
            border: none; border-radius: 14px;
            background: linear-gradient(90deg, #06b6d4, #2563eb);
            color: white; font-size: 16px; font-weight: 700; cursor: pointer;
        }
        .message { display: block; text-align: center; margin-top: 16px; font-weight: 700; }
        .back-link {
            display: block; text-align: center; margin-top: 24px;
            color: white; text-decoration: none; font-weight: 600;
        }
        .back-link:hover { text-decoration: underline; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main-container">
            <div class="card">
                <div class="title">Notifications</div>
                <div class="subtitle">Send email notifications to patients</div>

                <label class="label">Select Appointment</label>
                <asp:DropDownList ID="ddlAppointment" runat="server" CssClass="dropdown"/>
                <asp:Label ID="lblAppointmentError" runat="server" CssClass="field-message"/>

                <label class="label">Notification Type</label>
                <asp:DropDownList ID="ddlType" runat="server" CssClass="dropdown">
                    <asp:ListItem Text="Select Type" Value=""/>
                    <asp:ListItem Text="Appointment Confirmed" Value="Confirmed"/>
                    <asp:ListItem Text="Appointment Reminder" Value="Reminder"/>
                    <asp:ListItem Text="Appointment Rescheduled" Value="Rescheduled"/>
                    <asp:ListItem Text="Appointment Completed" Value="Completed"/>
                </asp:DropDownList>
                <asp:Label ID="lblTypeError" runat="server" CssClass="field-message"/>

                <asp:Button ID="btnSend" runat="server" Text="Send Notification" 
                    CssClass="btn" OnClick="btnSend_Click"/>
                <asp:Label ID="lblMessage" runat="server" CssClass="message"/>

                <a class="back-link" href="Dashboard.aspx">Back to Dashboard</a>
            </div>
        </div>
    </form>
</body>
</html>
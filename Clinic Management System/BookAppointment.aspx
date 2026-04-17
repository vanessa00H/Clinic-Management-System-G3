<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BookAppointment.aspx.cs" Inherits="Clinic_Management_System.BookAppointment" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Book Appointment</title>
    <style>
        * { box-sizing: border-box; }
        body { margin: 0; padding: 0; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background: linear-gradient(135deg, #0f172a, #1d4ed8, #38bdf8); min-height: 100vh; }
        .background-circles { position: fixed; width: 100%; height: 100%; overflow: hidden; z-index: 0; }
        .circle { position: absolute; border-radius: 50%; background: rgba(255,255,255,0.10); filter: blur(8px); }
        .circle1 { width: 220px; height: 220px; top: 60px; left: 70px; }
        .circle2 { width: 300px; height: 300px; bottom: 40px; right: 60px; }
        .circle3 { width: 150px; height: 150px; top: 50%; left: 15%; }
        .main-container { position: relative; z-index: 1; min-height: 100vh; display: flex; justify-content: center; align-items: center; padding: 30px; }
        .form-card { width: 720px; background: rgba(255,255,255,0.18); backdrop-filter: blur(16px); -webkit-backdrop-filter: blur(16px); border: 1px solid rgba(255,255,255,0.22); border-radius: 24px; padding: 32px; color: white; box-shadow: 0 8px 24px rgba(0,0,0,0.20); }
        .title { text-align: center; font-size: 30px; font-weight: 700; margin-bottom: 8px; }
        .subtitle { text-align: center; color: #dbeafe; margin-bottom: 24px; }
        .grid { display: grid; grid-template-columns: 1fr 1fr; gap: 16px 18px; }
        .full { grid-column: 1 / -1; }
        .label { display: block; font-weight: 600; margin-bottom: 8px; }
        .textbox, .dropdown { width: 100%; padding: 13px; border-radius: 14px; border: none; outline: none; font-size: 15px; background: rgba(255,255,255,0.92); color: #0f172a; }
        .textbox:focus, .dropdown:focus { background: white; box-shadow: 0 0 0 3px rgba(255,255,255,0.25); }
        .field-message { display: block; margin-top: 6px; min-height: 18px; font-size: 12px; font-weight: 600; color: #ffe4e6; }
        .btn { width: 100%; padding: 14px; margin-top: 20px; border: none; border-radius: 14px; background: linear-gradient(90deg, #06b6d4, #2563eb); color: white; font-size: 16px; font-weight: 700; cursor: pointer; }
        .btn:hover { opacity: 0.95; }
        .message { display: block; text-align: center; margin-top: 16px; font-weight: 700; min-height: 24px; }
        .back-link { display: block; text-align: center; margin-top: 18px; color: white; text-decoration: none; font-weight: 600; }
        .back-link:hover { text-decoration: underline; }
        .hint { margin-top: 16px; text-align: center; color: #dbeafe; font-size: 12px; line-height: 1.6; }
        @media (max-width: 760px) { .form-card { width: 100%; padding: 24px; } .grid { grid-template-columns: 1fr; } }
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
            <div class="form-card">
                <div class="title">Book Appointment</div>
                <div class="subtitle">Create a new clinic appointment</div>

                <div class="grid">
                    <div>
                        <label class="label">Patient Name</label>
                        <asp:TextBox ID="txtPatientName" runat="server" CssClass="textbox"></asp:TextBox>
                        <asp:Label ID="lblPatientNameError" runat="server" CssClass="field-message"></asp:Label>
                    </div>

                    <div>
                        <label class="label">Email</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="textbox"></asp:TextBox>
                        <asp:Label ID="lblEmailError" runat="server" CssClass="field-message"></asp:Label>
                    </div>

                    <div>
                        <label class="label">Phone</label>
                        <div style="position: relative;">
                            <span style="position: absolute; left: 15px; top: 50%; transform: translateY(-50%); font-size: 15px; color: #0f172a; pointer-events: none;">05</span>
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="textbox" MaxLength="8" placeholder="XXXXXXXX" Style="padding-left: 38px;"></asp:TextBox>
                        </div>
                        <asp:Label ID="lblPhoneError" runat="server" CssClass="field-message"></asp:Label>
                    </div>

                    <div>
                        <label class="label">Appointment Date</label>
                        <asp:TextBox ID="txtDate" runat="server" TextMode="Date" CssClass="textbox"
                            Style="text-align: left; direction: ltr; color: #0f172a;"
                            placeholder="dd / mm / yyyy" 
                            AutoPostBack="true" 
                            OnTextChanged="txtDate_TextChanged">
                        </asp:TextBox>
                        <asp:Label ID="lblDateError" runat="server" CssClass="field-message"></asp:Label>
                    </div>

                    <div>
                        <label class="label">Appointment Time</label>
                        <asp:TextBox ID="txtTime" runat="server" TextMode="Time" CssClass="textbox"></asp:TextBox>
                        <asp:Label ID="lblTimeError" runat="server" CssClass="field-message"></asp:Label>
                    </div>

                    <div>
                        <label class="label">Doctor</label>
                        <asp:DropDownList ID="ddlDoctor" runat="server" CssClass="dropdown" AutoPostBack="true" OnSelectedIndexChanged="ddlDoctor_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Label ID="lblDoctorError" runat="server" CssClass="field-message"></asp:Label>
                    </div>

                    <div>
                        <label class="label">Department</label>
                        <asp:TextBox ID="txtDepartment" runat="server" CssClass="textbox" ReadOnly="true"></asp:TextBox>
                        <asp:Label ID="lblDepartmentError" runat="server" CssClass="field-message"></asp:Label>
                    </div>

                    <div>
                        <label class="label">Consultation Fee</label>
                        <asp:TextBox ID="txtConsultationFee" runat="server" CssClass="textbox" ReadOnly="true"></asp:TextBox>
                        <asp:Label ID="lblFeeError" runat="server" CssClass="field-message"></asp:Label>
                    </div>

                    <div>
                        <label class="label">Appointment Type</label>
                        <asp:DropDownList ID="ddlAppointmentType" runat="server" CssClass="dropdown" AutoPostBack="true" OnSelectedIndexChanged="CalculateTotalFee">
                            <asp:ListItem Text="Select Appointment Type" Value=""></asp:ListItem>
                            <asp:ListItem Text="Regular Visit" Value="Regular Visit"></asp:ListItem>
                            <asp:ListItem Text="Urgent Consultation" Value="Urgent Consultation"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="lblTypeError" runat="server" CssClass="field-message"></asp:Label>
                    </div>

                    <div>
                        <label class="label">Additional Service</label>
                        <asp:DropDownList ID="ddlService" runat="server" CssClass="dropdown" AutoPostBack="true" OnSelectedIndexChanged="CalculateTotalFee">
                            <asp:ListItem Text="No Additional Service" Value=""></asp:ListItem>
                            <asp:ListItem Text="Lab Test" Value="Lab Test"></asp:ListItem>
                            <asp:ListItem Text="Follow-up Consultation" Value="Follow-up Consultation"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="lblServiceError" runat="server" CssClass="field-message"></asp:Label>
                    </div>
                </div>

                <asp:Button ID="btnBook" runat="server" Text="Book Appointment" CssClass="btn" OnClick="btnBook_Click" Style="margin-top: 20px; width: 100%;" />

                <asp:Label ID="lblMessage" runat="server" CssClass="message" Style="display: block; margin-top: 10px; text-align: center;"></asp:Label>

                <div style="text-align: center; margin-top: 15px;">
                    <a class="back-link" href="Dashboard.aspx" style="color: white; text-decoration: none; font-weight: bold;">Back to Home Page</a>
                </div>

                <div class="hint" style="margin-top: 20px; font-size: 12px; color: #e2e8f0; border-top: 1px solid rgba(255,255,255,0.2); padding-top: 10px; text-align: center;">
                    Required: name, email, phone, date, time, doctor, department, appointment type.<br />
                    Status will be saved automatically as Pending.
                </div>

            </div>
        </div>
    </form>
</body>
</html>
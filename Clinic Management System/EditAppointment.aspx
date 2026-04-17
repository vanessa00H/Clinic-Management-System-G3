<%@ Page Language="C#" AutoEventWireup="true" 
    CodeBehind="EditAppointment.aspx.cs" 
    Inherits="Clinic_Management_System.EditAppointment" %>
<script>
    window.onpageshow = function (event) {
        if (event.persisted) {
            window.location.reload();
        }
    };
</script>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Appointment</title>
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
            display: flex; justify-content: center; align-items: center;
            padding: 30px;
        }
        .form-card {
            width: 720px;
            background: rgba(255,255,255,0.18);
            backdrop-filter: blur(16px);
            border: 1px solid rgba(255,255,255,0.22);
            border-radius: 24px;
            padding: 32px; color: white;
            box-shadow: 0 8px 24px rgba(0,0,0,0.20);
        }
        .title { text-align: center; font-size: 30px; font-weight: 700; margin-bottom: 8px; }
        .subtitle { text-align: center; color: #dbeafe; margin-bottom: 24px; }
        .grid { display: grid; grid-template-columns: 1fr 1fr; gap: 16px 18px; }
        .label { display: block; font-weight: 600; margin-bottom: 8px; }
        .textbox, .dropdown {
            width: 100%; padding: 13px;
            border-radius: 14px; border: none; outline: none;
            font-size: 15px; background: rgba(255,255,255,0.92); color: #0f172a;
        }
        .btn {
            width: 100%; padding: 14px; margin-top: 20px;
            border: none; border-radius: 14px;
            background: linear-gradient(90deg, #06b6d4, #2563eb);
            color: white; font-size: 16px; font-weight: 700; cursor: pointer;
        }
        .field-message {
            display: block; margin-top: 6px; min-height: 18px;
            font-size: 12px; font-weight: 600; color: #ffe4e6;
        }
        .message { display: block; text-align: center; margin-top: 16px; font-weight: 700; }
        .back-link {
            display: block; text-align: center; margin-top: 18px;
            color: white; text-decoration: none; font-weight: 600;
        }
        .back-link:hover { text-decoration: underline; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main-container">
            <div class="form-card">
                <div class="title">Edit Appointment</div>
                <div class="subtitle">Update appointment details</div>

                <div class="grid">
                    <div>
                        <label class="label">Patient Name</label>
                        <asp:TextBox ID="txtPatientName" runat="server" CssClass="textbox"/>
                        <asp:Label ID="lblPatientNameError" runat="server" CssClass="field-message"/>
                    </div>
                    <div>
                        <label class="label">Email</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="textbox"/>
                        <asp:Label ID="lblEmailError" runat="server" CssClass="field-message"/>
                    </div>
                    <div>
                        <label class="label">Phone</label>
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="textbox"/>
                        <asp:Label ID="lblPhoneError" runat="server" CssClass="field-message"/>
                    </div>
                    <div>
                        <label class="label">Appointment Date</label>
                        <asp:TextBox ID="txtDate" runat="server" TextMode="Date" CssClass="textbox"/>
                        <asp:Label ID="lblDateError" runat="server" CssClass="field-message"/>
                    </div>
                    <div>
                        <label class="label">Appointment Time</label>
                        <asp:TextBox ID="txtTime" runat="server" TextMode="Time" CssClass="textbox"/>
                        <asp:Label ID="lblTimeError" runat="server" CssClass="field-message"/>
                    </div>
                    
                    <div>
                        <label class="label">Doctor</label>
                        <asp:DropDownList ID="ddlDoctor" runat="server" CssClass="dropdown" AutoPostBack="true" OnSelectedIndexChanged="ddlDoctor_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <label class="label">Department</label>
                        <asp:TextBox ID="txtDepartment" runat="server" CssClass="textbox" ReadOnly="true" />
                    </div>
                    <div>
                        <label class="label">Additional Service</label>
                        <asp:DropDownList ID="ddlService" runat="server" CssClass="dropdown">
                            <asp:ListItem Text="No Additional Service" Value=""></asp:ListItem>
                            <asp:ListItem Text="Lab Test" Value="Lab Test"></asp:ListItem>
                            <asp:ListItem Text="Follow-up Consultation" Value="Follow-up Consultation"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <asp:PlaceHolder ID="phStatus" runat="server">
                        <div>
                            <label class="label">Status</label>
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="dropdown">
                                <asp:ListItem Text="Pending" Value="Pending"/>
                                <asp:ListItem Text="Confirmed" Value="Confirmed"/>
                                <asp:ListItem Text="In Consultation" Value="In Consultation"/>
                                <asp:ListItem Text="Completed" Value="Completed"/>
                                <asp:ListItem Text="Cancelled" Value="Cancelled"/>
                            </asp:DropDownList>
                        </div>
                    </asp:PlaceHolder>
                </div>

                <asp:Button ID="btnSave" runat="server" Text="Save Changes" CssClass="btn" OnClick="btnSave_Click"/>
                <asp:Label ID="lblMessage" runat="server" CssClass="message"/>
                <a class="back-link" href="ViewAppointments.aspx">Back to Appointments</a>
            </div>
        </div>
    </form>
</body>
</html>
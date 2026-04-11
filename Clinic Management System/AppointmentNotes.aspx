<%@ Page Language="C#" AutoEventWireup="true" 
    CodeBehind="AppointmentNotes.aspx.cs" 
    Inherits="Clinic_Management_System.AppointmentNotes" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Appointment Notes</title>
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
        }
        .card {
            width: 100%;
            background: rgba(255,255,255,0.18);
            backdrop-filter: blur(16px);
            border: 1px solid rgba(255,255,255,0.22);
            border-radius: 24px;
            padding: 32px; color: white;
            box-shadow: 0 8px 24px rgba(0,0,0,0.20);
            margin-bottom: 30px;
        }
        .title { text-align: center; font-size: 30px; font-weight: 700; margin-bottom: 8px; }
        .subtitle { text-align: center; color: #dbeafe; margin-bottom: 24px; }
        .label { display: block; font-weight: 600; margin-bottom: 8px; }
        .dropdown, .textarea {
            width: 100%; padding: 13px;
            border-radius: 14px; border: none; outline: none;
            font-size: 15px; background: rgba(255,255,255,0.92); color: #0f172a;
        }
        .textarea { height: 120px; resize: vertical; }
        .btn {
            width: 100%; padding: 14px; margin-top: 20px;
            border: none; border-radius: 14px;
            background: linear-gradient(90deg, #06b6d4, #2563eb);
            color: white; font-size: 16px; font-weight: 700; cursor: pointer;
        }
        .message { display: block; text-align: center; margin-top: 16px; font-weight: 700; }
        .field-message {
            display: block; margin-top: 6px; min-height: 18px;
            font-size: 12px; font-weight: 600; color: #ffe4e6;
        }
        table { width: 100%; border-collapse: collapse; color: white; }
        th {
            background: rgba(255,255,255,0.2); padding: 14px;
            text-align: left; font-weight: 700;
            border-bottom: 2px solid rgba(255,255,255,0.3);
        }
        td { padding: 14px; border-bottom: 1px solid rgba(255,255,255,0.1); }
        tr:hover td { background: rgba(255,255,255,0.08); }
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
                <div class="title">Appointment Notes</div>
                <div class="subtitle">Add notes to appointments</div>

                <label class="label">Select Appointment</label>
                <asp:DropDownList ID="ddlAppointment" runat="server" CssClass="dropdown"/>
                <asp:Label ID="lblAppointmentError" runat="server" CssClass="field-message"/>

                <br/><br/>

                <label class="label">Note</label>
                <asp:TextBox ID="txtNote" runat="server" CssClass="textarea" 
                    TextMode="MultiLine" placeholder="Enter note here..."/>
                <asp:Label ID="lblNoteError" runat="server" CssClass="field-message"/>

                <asp:Button ID="btnSave" runat="server" Text="Save Note" 
                    CssClass="btn" OnClick="btnSave_Click"/>
                <asp:Label ID="lblMessage" runat="server" CssClass="message"/>
            </div>

            <div class="card">
                <div class="title">Notes List</div>
                <asp:GridView ID="gvNotes" runat="server"
                    AutoGenerateColumns="False"
                    GridLines="None">
                    <Columns>
                        <asp:BoundField DataField="PatientName" HeaderText="Patient"/>
                        <asp:BoundField DataField="DoctorName" HeaderText="Doctor"/>
                        <asp:BoundField DataField="AppointmentDate" HeaderText="Date" 
                            DataFormatString="{0:dd/MM/yyyy}"/>
                        <asp:BoundField DataField="Notes" HeaderText="Note"/>
                    </Columns>
                </asp:GridView>
            </div>

            <a class="back-link" href="Dashboard.aspx">Back to Dashboard</a>
        </div>
    </form>
</body>
</html>
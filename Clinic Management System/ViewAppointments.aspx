<%@ Page Language="C#" AutoEventWireup="true" 
    CodeBehind="ViewAppointments.aspx.cs" 
    Inherits="Clinic_Management_System.ViewAppointments" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Appointments</title>
    <style>
        * { box-sizing: border-box; }

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

        .circle1 { width: 220px; height: 220px; top: 60px; left: 70px; }
        .circle2 { width: 300px; height: 300px; bottom: 40px; right: 60px; }
        .circle3 { width: 150px; height: 150px; top: 50%; left: 15%; }

        .main-container {
           
           position: relative;
           z-index: 1;
           min-height: 100vh;
           padding: 40px 40px 40px 40px;
           display: flex;
           justify-content: center;

        }

        .card {
           
          width: 100%;
          background: rgba(255,255,255,0.18);
          backdrop-filter: blur(16px);
          border: 1px solid rgba(255,255,255,0.22);
          border-radius: 24px;
          padding: 32px;
          color: white;
          box-shadow: 0 8px 24px rgba(0,0,0,0.20);
          overflow-x: auto;
          align-self: flex-start;

        }

        .title {
            text-align: center;
            font-size: 30px;
            font-weight: 700;
            margin-bottom: 8px;
        }

        .subtitle {
            text-align: center;
            color: #dbeafe;
            margin-bottom: 24px;
        }

        table {
            width: 100%;
            border-collapse: collapse;
            color: white;
        }

        th {
            background: rgba(255,255,255,0.2);
            padding: 14px;
            text-align: left;
            font-weight: 700;
            border-bottom: 2px solid rgba(255,255,255,0.3);
        }

        td {   
           padding: 14px;
        }

        tr:hover td {
            background: rgba(255,255,255,0.08);
        }

        .status-pending {
            background: rgba(234,179,8,0.3);
            padding: 4px 10px;
            border-radius: 20px;
            font-size: 12px;
            font-weight: 700;
        }

        .status-confirmed {
            background: rgba(34,197,94,0.3);
            padding: 4px 10px;
            border-radius: 20px;
            font-size: 12px;
            font-weight: 700;
        }

        .status-cancelled {
            background: rgba(239,68,68,0.3);
            padding: 4px 10px;
            border-radius: 20px;
            font-size: 12px;
            font-weight: 700;
        }

        .back-link {
            display: block;
            text-align: center;
            margin-top: 24px;
            color: white;
            text-decoration: none;
            font-weight: 600;
        }

        .back-link:hover { text-decoration: underline; }
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
            <div class="card">
                <div class="title">View Appointments</div>
                <div class="subtitle">All clinic appointments</div>

                <asp:GridView ID="gvAppointments" runat="server"
    AutoGenerateColumns="False"
    CssClass="grid"
    GridLines="None"
    DataKeyNames="AppointmentID"
    OnRowCommand="gvAppointments_RowCommand">
  <Columns>
    <asp:BoundField DataField="AppointmentID" HeaderText="ID" />
    <asp:BoundField DataField="PatientName" HeaderText="Patient Name" />
    <asp:BoundField DataField="Email" HeaderText="Email" />
    <asp:BoundField DataField="Phone" HeaderText="Phone" />
    <asp:BoundField DataField="AppointmentDate" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}" />
    <asp:BoundField DataField="AppointmentTime" HeaderText="Time" />
    <asp:BoundField DataField="DoctorName" HeaderText="Doctor" />
    <asp:BoundField DataField="Department" HeaderText="Department" />
    <asp:BoundField DataField="AppointmentType" HeaderText="Type" />
    <asp:BoundField DataField="AdditionalService" HeaderText="Additional Service" />
    <asp:BoundField DataField="Status" HeaderText="Status" />
    <asp:TemplateField HeaderText="Actions">
        <ItemTemplate>
            <asp:LinkButton CommandName="EditApp" 
                CommandArgument='<%# Eval("AppointmentID") %>'
                runat="server" 
                Style="color:#38bdf8; font-weight:700; margin-right:10px;">
                Edit
            </asp:LinkButton>
            <asp:LinkButton CommandName="CancelApp" 
                CommandArgument='<%# Eval("AppointmentID") %>'
                runat="server" 
                Style="color:#f87171; font-weight:700;"
                OnClientClick="return confirm('Are you sure you want to cancel?');">
                Cancel
            </asp:LinkButton>
        </ItemTemplate>
    </asp:TemplateField>
</Columns>
</asp:GridView>

                <a class="back-link" href="Dashboard.aspx">Back to Dashboard</a>
            </div>
        </div>
    </form>
</body>
</html>
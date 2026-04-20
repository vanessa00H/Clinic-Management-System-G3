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
            margin: 0; padding: 0;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #0f172a, #1d4ed8, #38bdf8);
            min-height: 100vh;
        }
        .background-circles { position: fixed; width: 100%; height: 100%; overflow: hidden; z-index: 0; }
        .circle { position: absolute; border-radius: 50%; background: rgba(255,255,255,0.10); filter: blur(8px); }
        .circle1 { width: 220px; height: 220px; top: 60px; left: 70px; }
        .circle2 { width: 300px; height: 300px; bottom: 40px; right: 60px; }
        .circle3 { width: 150px; height: 150px; top: 50%; left: 15%; }
        .main-container { position: relative; z-index: 1; min-height: 100vh; padding: 40px; display: flex; justify-content: center; }
        .card {
            width: 100%; background: rgba(255,255,255,0.18); backdrop-filter: blur(16px);
            border: 1px solid rgba(255,255,255,0.22); border-radius: 24px; padding: 32px;
            color: white; box-shadow: 0 8px 24px rgba(0,0,0,0.20); overflow-x: auto; align-self: flex-start;
        }
        .title { text-align: center; font-size: 30px; font-weight: 700; margin-bottom: 8px; }
        .subtitle { text-align: center; color: #dbeafe; margin-bottom: 24px; }
        table { width: 100%; border-collapse: collapse; color: white; }
        th { background: rgba(255,255,255,0.2); padding: 14px; text-align: left; font-weight: 700; border-bottom: 2px solid rgba(255,255,255,0.3); }
        td { padding: 14px; }
        tr:hover td { background: rgba(255,255,255,0.08); }
        .back-link { display: block; text-align: center; margin-top: 24px; color: white; text-decoration: none; font-weight: 600; }
        .back-link:hover { text-decoration: underline; }
        .filter-bar { display: flex; gap: 12px; margin-bottom: 20px; flex-wrap: wrap; align-items: center; }
        .filter-bar select, .filter-bar input {
            padding: 13px; border-radius: 14px; border: none;
            background: rgba(255,255,255,0.18); color: white; font-size: 14px; backdrop-filter: blur(8px);
        }
        .filter-bar select option { color: #0f172a; }
        .filter-btn {
            padding: 13px 24px; border-radius: 14px; border: none;
            background: linear-gradient(90deg, #06b6d4, #2563eb); color: white; font-weight: 700; cursor: pointer; font-size: 14px;
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
            <div class="card">
                <div class="title">View Appointments</div>
                <div class="subtitle">All clinic appointments</div>

                <div class="filter-bar">
                    <asp:DropDownList ID="ddlFilterStatus" runat="server">
                        <asp:ListItem Text="All Status" Value="" />
                        <asp:ListItem Text="Pending" Value="Pending" />
                        <asp:ListItem Text="Confirmed" Value="Confirmed" />
                        <asp:ListItem Text="In Consultation" Value="In Consultation" />
                        <asp:ListItem Text="Completed" Value="Completed" />
                        <asp:ListItem Text="Cancelled" Value="Cancelled" />
                    </asp:DropDownList>

                    <asp:DropDownList ID="ddlFilterDoctor" runat="server"></asp:DropDownList>
                    <asp:DropDownList ID="ddlFilterDepartment" runat="server"></asp:DropDownList>
                    
                    <span style="font-weight: 600;">From:</span>
                    <asp:TextBox ID="txtFromDate" runat="server" TextMode="Date" />

                    <span style="font-weight: 600;">To:</span>
                    <asp:TextBox ID="txtToDate" runat="server" TextMode="Date" />

                    <asp:DropDownList ID="ddlSort" runat="server">
                        <asp:ListItem Text="Sort by Date" Value="AppointmentDate" />
                        <asp:ListItem Text="Sort by Doctor" Value="DoctorName" />
                        <asp:ListItem Text="Sort by Status" Value="Status" />
                    </asp:DropDownList>

                    <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="filter-btn" OnClick="btnFilter_Click" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="filter-btn" OnClick="btnReset_Click" />
                </div>
                <asp:Label ID="lblMessage" runat="server" 
    Style="display:block; margin-bottom:15px; font-weight:bold;" />
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
        <asp:BoundField DataField="PaymentStatus" HeaderText="Payment" />
        <asp:BoundField DataField="Notes" HeaderText="Notes" />
        
        <asp:TemplateField HeaderText="Actions">
            <ItemTemplate>
                <asp:LinkButton ID="btnPay" runat="server" 
                    CommandName="PayApp" 
                    CommandArgument='<%# Eval("AppointmentID") %>'
                    Text="Pay"
                    Visible='<%# Eval("PaymentStatus").ToString() == "Unpaid" && Session["Role"] != null && Session["Role"].ToString().ToLower() != "patient" %>'
                    Style="color:#1d4ed8; font-weight:700; margin-right:10px; text-decoration:none;" />

                <asp:LinkButton CommandName="EditApp"
                    CommandArgument='<%# Eval("AppointmentID") %>'
                    runat="server"
                    Style="color: #45de28; font-weight: 700; margin-right: 10px; text-decoration:none;">
                    Edit
                </asp:LinkButton>
                
                <asp:LinkButton CommandName="CancelApp"
                    CommandArgument='<%# Eval("AppointmentID") %>'
                    runat="server"
                    Style="color: #690094; font-weight: 700; text-decoration:none;"
                    OnClientClick="return confirm('Are you sure you want to cancel?');">
                    Cancel
                </asp:LinkButton>
                <asp:LinkButton CommandName="DeleteApp"
                 CommandArgument='<%# Eval("AppointmentID") %>'
                  runat="server"
                 Style="color: #ff0000; font-weight: 700; text-decoration:none; margin-right:10px;"
                 OnClientClick="return confirm('Are you sure you want to delete?');">
    Delete
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
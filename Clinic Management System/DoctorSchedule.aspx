<%@ Page Language="C#" AutoEventWireup="true" 
    CodeBehind="DoctorSchedule.aspx.cs" 
    Inherits="Clinic_Management_System.DoctorSchedule" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Doctor Schedule</title>
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
        td { padding: 14px; }
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

            <%-- Add Doctor Card --%>
            <div class="card">
                <div class="title">Doctor Schedule Management</div>
                <div class="subtitle">Add or update doctors and availability</div>

                <div class="grid">
                    <div>
                        <label class="label">Doctor Name</label>
                        <asp:TextBox ID="txtDoctorName" runat="server" CssClass="textbox"/>
                        <asp:Label ID="lblDoctorNameError" runat="server" CssClass="field-message"/>
                    </div>
                    <div>
                        <label class="label">Specialization</label>
                        <asp:TextBox ID="txtSpecialization" runat="server" CssClass="textbox"/>
                        <asp:Label ID="lblSpecializationError" runat="server" CssClass="field-message"/>
                    </div>
                    <div>
                        <label class="label">Department</label>
                        <asp:TextBox ID="txtDepartment" runat="server" CssClass="textbox"/>
                        <asp:Label ID="lblDepartmentError" runat="server" CssClass="field-message"/>
                    </div>
                    <div>
                        <label class="label">Consultation Fee</label>
                        <asp:TextBox ID="txtFee" runat="server" CssClass="textbox"/>
                        <asp:Label ID="lblFeeError" runat="server" CssClass="field-message"/>
                    </div>
                    <div>
                        <label class="label">Availability</label>
                        <asp:DropDownList ID="ddlAvailable" runat="server" CssClass="dropdown">
                            <asp:ListItem Text="Available" Value="1"/>
                            <asp:ListItem Text="Unavailable" Value="0"/>
                        </asp:DropDownList>
                    </div>
                </div>

                <asp:Button ID="btnAdd" runat="server" Text="Add Doctor" 
                    CssClass="btn" OnClick="btnAdd_Click"/>
                <asp:Label ID="lblMessage" runat="server" CssClass="message"/>
            </div>

            <%-- Doctors List Card --%>
            <div class="card">
                <div class="title">Doctors List</div>
                <asp:GridView ID="gvDoctors" runat="server"
                    AutoGenerateColumns="False"
                    GridLines="None"
                    DataKeyNames="DoctorID"
                    OnRowCommand="gvDoctors_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="DoctorID" HeaderText="ID"/>
                        <asp:BoundField DataField="DoctorName" HeaderText="Name"/>
                        <asp:BoundField DataField="Specialization" HeaderText="Specialization"/>
                        <asp:BoundField DataField="Department" HeaderText="Department"/>
                        <asp:BoundField DataField="ConsultationFee" HeaderText="Fee"/>
                        <asp:BoundField DataField="IsAvailable" HeaderText="Available"/>
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:LinkButton CommandName="DeleteDoc"
                                    CommandArgument='<%# Eval("DoctorID") %>'
                                    runat="server"
                                    Style="color:#f87171; font-weight:700;"
                                    OnClientClick="return confirm('Delete this doctor?');">
                                    Delete
                                </asp:LinkButton>
                                <asp:LinkButton CommandName="ToggleDoc"
                                    CommandArgument='<%# Eval("DoctorID") %>'
                                    runat="server"
                                    Style="color:#38bdf8; font-weight:700; margin-left:10px;">
                                    Toggle
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

            <a class="back-link" href="Dashboard.aspx">Back to Dashboard</a>
        </div>
    </form>
</body>
</html>
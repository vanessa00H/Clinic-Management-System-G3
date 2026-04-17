<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageFees.aspx.cs" Inherits="Clinic_Management_System.ManageFees" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage Special Fees</title>
    <style>
        * { box-sizing: border-box; }
        body {
            margin: 0; padding: 0;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #0f172a, #1d4ed8, #38bdf8);
            min-height: 100vh; color: white;
        }
        .main-container { padding: 40px; display: flex; justify-content: center; }
        .card {
            width: 100%; max-width: 800px;
            background: rgba(255,255,255,0.18); backdrop-filter: blur(16px);
            border: 1px solid rgba(255,255,255,0.22); border-radius: 24px; padding: 32px;
            box-shadow: 0 8px 24px rgba(0,0,0,0.20);
        }
        .title { text-align: center; font-size: 30px; font-weight: 700; margin-bottom: 8px; }
        .subtitle { text-align: center; color: #dbeafe; margin-bottom: 24px; }
        .form-group { margin-bottom: 15px; }
        .label { display: block; font-weight: 600; margin-bottom: 8px; }
        .input-field {
            width: 100%; padding: 13px; border-radius: 14px; border: none;
            background: rgba(255,255,255,0.92); color: #0f172a; font-size: 15px; outline: none;
        }
        .btn-save {
            width: 100%; padding: 14px; margin-top: 10px; border: none; border-radius: 14px;
            background: linear-gradient(90deg, #22c55e, #16a34a);
            color: white; font-size: 16px; font-weight: 700; cursor: pointer;
        }
        table { width: 100%; border-collapse: collapse; margin-top: 30px; }
        th { background: rgba(255,255,255,0.2); padding: 12px; text-align: left; }
        td { padding: 12px; border-bottom: 1px solid rgba(255,255,255,0.1); }
        .back-link { display: block; text-align: center; margin-top: 24px; color: white; text-decoration: none; font-weight: 600; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main-container">
            <div class="card">
                <div class="title">Special Fees Management</div>
                <div class="subtitle">Set promotional prices for dates or departments</div>

                <div style="display:flex; gap:15px; flex-wrap:wrap;">
                    <div class="form-group" style="flex:1;">
                        <label class="label">Department</label>
                        <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="input-field">
                            <asp:ListItem Text="General" Value="General" />
                            <asp:ListItem Text="Dentistry" Value="Dentistry" />
                            <asp:ListItem Text="Cardiology" Value="Cardiology" />
                            <asp:ListItem Text="Orthopedics" Value="Orthopedics" />
                            <asp:ListItem Text="Pediatrics" Value="Pediatrics" />
                        </asp:DropDownList>
                    </div>

                    <div class="form-group" style="flex:1;">
                        <label class="label">Special Date</label>
                        <asp:TextBox ID="txtDate" runat="server" TextMode="Date" CssClass="input-field" />
                    </div>

                    <div class="form-group" style="flex:1;">
                        <label class="label">Special Fee Amount (SAR)</label>
                        <asp:TextBox ID="txtFee" runat="server" TextMode="Number" CssClass="input-field" placeholder="e.g., 99.00" min="0" step="0.01" />
                    </div>
                </div>

                <asp:Button ID="btnSaveFee" runat="server" Text="Save Special Fee" CssClass="btn-save" OnClick="btnSaveFee_Click" />
                
                <div style="text-align:center; margin-top:15px;">
                    <asp:Label ID="lblMessage" runat="server" Font-Bold="true" Font-Size="16px"></asp:Label>
                </div>

                <asp:GridView ID="gvSpecialFees" runat="server" AutoGenerateColumns="False" GridLines="None" CssClass="grid" OnRowCommand="gvSpecialFees_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="Department" HeaderText="Department" />
                        <asp:BoundField DataField="SpecialDate" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="FeeAmount" HeaderText="Fee (SAR)" DataFormatString="{0:N2} SAR" />
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton CommandName="DeleteFee" CommandArgument='<%# Eval("FeeID") %>' runat="server" Style="color: #f87171; font-weight: bold; text-decoration:none;" OnClientClick="return confirm('Delete this special fee?');">Delete</asp:LinkButton>
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
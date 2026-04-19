<%@ Page Title="Manage Doctors" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageDoctors.aspx.cs" Inherits="Clinic_Management_System.ManageDoctors" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        /* hide default */
        nav, .navbar, footer, hr, #footer { display: none !important; }
        
        /* fullscreen background  */
        .my-glass-wrapper {
            position: fixed; top: 0; left: 0; width: 100vw; height: 100vh;
            background: linear-gradient(135deg, #0f172a, #1d4ed8, #38bdf8) !important;
            display: flex; align-items: center; justify-content: center;
            z-index: 99999;
        }

        /* main glassmorphism card */
        .my-glass-card {
            background: rgba(255, 255, 255, 0.15);
            backdrop-filter: blur(25px);
            border: 1px solid rgba(255, 255, 255, 0.2);
            border-radius: 20px;
            padding: 40px 50px;
            width: 850px; 
            max-width: 95%;
            box-shadow: 0 20px 50px rgba(0,0,0,0.5);
            font-family: 'Segoe UI', Tahoma, sans-serif;
            color: white;
            box-sizing: border-box;
        }

        /* tow-column grid layout */
        .fields-grid {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 20px 30px; 
            width: 100%;
        }

        .input-group {
            display: flex;
            flex-direction: column;
            width: 100%;
        }

        .my-label {
            font-weight: 600;
            margin-bottom: 8px;
            font-size: 15px;
            margin-left: 15px; 
        }

        /* rounded capsule inputes*/
        .my-input {
            padding: 12px 25px; 
            border-radius: 50px !important; 
            border: none;
            outline: none;
            font-size: 15px;
            background: #ffffff !important;
            color: #000;
            width: 100% !important;
            max-width: 100% !important;
            box-sizing: border-box;
        }

       elect.my-input {
            -webkit-appearance: none !important;
            -moz-appearance: none !important;
            appearance: none !important;
            background-image: url("data:image/svg+xml;charset=UTF-8,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='none' stroke='black' stroke-width='3' stroke-linecap='round' stroke-linejoin='round'%3e%3cpolyline points='6 9 12 15 18 9'%3e%3c/polyline%3e%3c/svg%3e") !important;
            background-repeat: no-repeat !important;
            background-position: right 20px center !important;
            background-size: 16px !important;
            padding-right: 45px !important;
        }

        select.my-input::-ms-expand {
            display: none !important;
        }
      
        .buttons-row {
            display: grid;
            grid-template-columns: 1fr 1fr 1fr; 
            gap: 15px;
            margin-top: 30px;
            width: 100%;
        }

       
        .my-btn {
            padding: 14px;
            border-radius: 50px !important; 
            border: none;
            color: white;
            font-weight: bold;
            font-size: 16px;
            cursor: pointer;
            width: 100%;
            transition: 0.3s;
        }

        .btn-add { background: linear-gradient(90deg, #06b6d4, #2563eb); }
        .btn-upd { background: #10b981; }
        .btn-del { background: #f87171; }
        
        .my-btn:hover { opacity: 0.9; transform: translateY(-2px); box-shadow: 0 5px 15px rgba(0,0,0,0.2); }
    </style>

    <div class="my-glass-wrapper">
        <div class="my-glass-card">
            <h2 style="text-align:center; margin-top:0; font-size:32px;">Doctor Management Control</h2>
            <p style="text-align:center; color:#dbeafe; margin-bottom: 30px; font-size:15px;">Add or update doctors and clinic services</p>

            <div class="fields-grid">
                <div class="input-group">
                    <label class="my-label">Doctor Name (Type to Auto-fill)</label>
                    <asp:TextBox ID="txtDoctorName" runat="server" CssClass="my-input" AutoPostBack="true" OnTextChanged="txtDoctorName_TextChanged"></asp:TextBox>
                </div>
                <div class="input-group">
                    <label class="my-label">Specialization</label>
                    <asp:TextBox ID="txtSpecialization" runat="server" CssClass="my-input"></asp:TextBox>
                </div>

                <div class="input-group">
                    <label class="my-label">Department</label>
                    <asp:TextBox ID="txtDepartment" runat="server" CssClass="my-input"></asp:TextBox>
                </div>
                <div class="input-group">
                    <label class="my-label">Consultation Fee</label>
                    <asp:TextBox ID="txtFee" runat="server" CssClass="my-input"></asp:TextBox>
                </div>

                <div class="input-group" style="grid-column: span 2; max-width: 48%;"> 
                    <label class="my-label">Availability Status</label>
                    <asp:DropDownList ID="ddlAvailable" runat="server" CssClass="my-input">
                        <asp:ListItem Text="Available" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Unavailable" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div style="text-align:center; margin-top:15px;">
                <asp:Label ID="lblMessage" runat="server" style="color:yellow; font-weight:bold; font-size:15px;"></asp:Label>
            </div>

            <div class="buttons-row">
                <asp:Button ID="btnAdd" runat="server" Text="Add Doctor" CssClass="my-btn btn-add" OnClick="btnAdd_Click" />
                <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="my-btn btn-upd" OnClick="btnUpdate_Click" />
                <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="my-btn btn-del" OnClick="btnDelete_Click" OnClientClick="return confirm('Are you sure?');" />
            </div>

            <div style="text-align:center; margin-top:25px;">
                <a href="Dashboard.aspx" style="color:white; text-decoration:none; font-weight:bold; font-size:16px;">Back to Dashboard</a>
            </div>
        </div>
    </div>
</asp:Content>
<%@ Page Language="C#" AutoEventWireup="true" 
    CodeBehind="ConsultationFees.aspx.cs" 
    Inherits="Clinic_Management_System.ConsultationFees" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Consultation Fees</title>
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

            <%-- Fees List Card --%>
            <div class="card">
                <div class="title">Consultation Fees</div>
                <div class="subtitle">Clinic doctors and their consultation charges</div>
                
                <asp:GridView ID="gvDoctors" runat="server"
                    AutoGenerateColumns="False"
                    GridLines="None"
                    DataKeyNames="DoctorID">
                    <Columns>
                        <asp:BoundField DataField="DoctorID" HeaderText="ID"/>
                        <asp:BoundField DataField="DoctorName" HeaderText="Name"/>
                        <asp:BoundField DataField="Specialization" HeaderText="Specialization"/>
                        <asp:BoundField DataField="Department" HeaderText="Department"/>
                        <asp:BoundField DataField="ConsultationFee" HeaderText="Fee (SAR)" DataFormatString="{0:N2} SAR"/>
                    </Columns>
                </asp:GridView>
            </div>

            <a class="back-link" href="Dashboard.aspx">Back to Dashboard</a>
        </div>
    </form>
</body>
</html>
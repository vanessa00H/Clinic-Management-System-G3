<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Clinic_Management_System.Register" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Clinic System - Register</title>
    <style>
        * {
            box-sizing: border-box;
        }

        body {
            margin: 0;
            padding: 0;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #0f172a, #1d4ed8, #38bdf8);
            min-height: 100vh;
            overflow: hidden;
        }

        .background-circles {
            position: absolute;
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

        .circle1 {
            width: 220px;
            height: 220px;
            top: 60px;
            left: 70px;
        }

        .circle2 {
            width: 300px;
            height: 300px;
            bottom: 40px;
            right: 60px;
        }

        .circle3 {
            width: 150px;
            height: 150px;
            top: 50%;
            left: 15%;
        }

        .main-container {
            position: relative;
            z-index: 1;
            width: 100%;
            min-height: 100vh;
            display: flex;
            justify-content: center;
            align-items: center;
            padding: 20px;
        }

        .register-card {
            width: 420px;
            padding: 35px 30px;
            border-radius: 24px;
            background: rgba(255, 255, 255, 0.18);
            backdrop-filter: blur(16px);
            -webkit-backdrop-filter: blur(16px);
            box-shadow: 0 8px 30px rgba(0,0,0,0.25);
            border: 1px solid rgba(255,255,255,0.25);
        }

        .logo-circle {
            width: 74px;
            height: 74px;
            margin: 0 auto 18px auto;
            border-radius: 50%;
            background: rgba(255,255,255,0.20);
            display: flex;
            justify-content: center;
            align-items: center;
            font-size: 34px;
            color: white;
            box-shadow: 0 4px 14px rgba(0,0,0,0.18);
        }

        .system-title {
            text-align: center;
            color: white;
            font-size: 24px;
            font-weight: 700;
            line-height: 1.4;
            margin-bottom: 8px;
        }

        .system-subtitle {
            text-align: center;
            color: #e2e8f0;
            font-size: 14px;
            margin-bottom: 28px;
        }

        .input-label {
            display: block;
            color: white;
            font-size: 14px;
            font-weight: 600;
            margin-bottom: 8px;
            margin-top: 10px;
        }

        .textbox {
            width: 100%;
            padding: 14px 14px;
            border-radius: 14px;
            border: none;
            outline: none;
            font-size: 15px;
            background: rgba(255,255,255,0.90);
            color: #0f172a;
            margin-bottom: 12px;
            transition: 0.25s ease;
        }

        .textbox:focus {
            background: white;
            box-shadow: 0 0 0 3px rgba(255,255,255,0.25);
        }

        .btn-register {
            width: 100%;
            padding: 14px;
            margin-top: 10px;
            border: none;
            border-radius: 14px;
            background: linear-gradient(90deg, #06b6d4, #2563eb);
            color: white;
            font-size: 16px;
            font-weight: 700;
            cursor: pointer;
            transition: 0.25s ease;
        }

        .btn-register:hover {
            transform: translateY(-1px);
            box-shadow: 0 8px 18px rgba(37,99,235,0.35);
        }

        .message {
            display: block;
            text-align: center;
            margin-top: 16px;
            min-height: 22px;
            font-weight: 600;
        }

        .extra-links {
            text-align: center;
            margin-top: 18px;
        }

        .extra-links a {
            color: white;
            text-decoration: none;
            font-weight: 600;
        }

        .extra-links a:hover {
            text-decoration: underline;
        }

        .hint-text {
            text-align: center;
            color: #dbeafe;
            font-size: 12px;
            margin-top: 16px;
            line-height: 1.6;
        }

        @media (max-width: 500px) {
            .register-card {
                width: 100%;
                padding: 28px 20px;
            }

            .system-title {
                font-size: 20px;
            }
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
            <div class="register-card">
                <div class="logo-circle">+</div>

                <div class="system-title">Clinic Appointment and Patient Management System</div>
                <div class="system-subtitle">Create a new account to access clinic services</div>

                <label class="input-label">Username</label>
                <asp:TextBox ID="txtNewUsername" runat="server" CssClass="textbox"></asp:TextBox>

                <label class="input-label">Password</label>
                <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" CssClass="textbox"></asp:TextBox>

                <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="btn-register" OnClick="btnRegister_Click" />

                <asp:Label ID="lblRegisterMessage" runat="server" CssClass="message"></asp:Label>

                <div class="extra-links">
                    <a href="Login.aspx">Back to login</a>
                </div>

                <div class="hint-text">
                    Username: letters only<br />
                    Password: at least 6 characters, with letters, numbers, and underscore (_) only
                </div>
            </div>
        </div>
    </form>
</body>
</html>

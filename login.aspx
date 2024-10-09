<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="shortcut icon" href="Images/inventory.png" />
    <title>Inventory System | Login</title>
    <link rel="stylesheet" href="css/modal.css" type="text/css" />
    <link rel="stylesheet" href="css/styles_green.css" type="text/css" />
    <style type="text/css">
        .mycss
        {
            text-shadow: 1px 1px 3px rgba(99,99,99,1);
            font-weight: bold;
            color: #ffffff;
            letter-spacing: 1pt;
            word-spacing: 0pt;
            font-size: 25px;
            text-align: center;
            font-family: times new roman, times, serif;
        }
        body
        {
            background-image: url(images/BodyBG.jpg); /*You will specify your image path here.*/
            -moz-background-size: cover;
            -webkit-background-size: cover;
            background-size: cover;
            background-position: top center !important;
            background-repeat: no-repeat !important;
            background-attachment: fixed;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table border="0" cellpadding="2" cellspacing="2" width="100%">
        <tr>
            <td style="border-right: blue thin solid; border-top: blue thin solid; border-left: blue thin solid;
                border-bottom: blue thin solid; border-width: 0px" align="center">
                <table border="0">
                    <tr>
                        <td colspan="2" align="center">
                            <div align="center">
                                <img src="Images/Banner.jpg" width="1250px" height="70" alt="RSS" /></div>
                            <%--=====Title Bar=========--%>
                            <%--=====Welcome Bar=========--%>
                            <div align="left" class="panel" style="width: 1230px; height: 20px;">
                                <h2 align="center">
                                    Welcome User!</h2>
                                <div style="margin-top: -30px;">
                                    <a href="#login_form" id="login_pop">Log In</a>
                                </div>
                            </div>
                            <div style="margin-top: 7px;">
                                <h4 align="center">
                                    <asp:Label ID="lblStatus" runat="server" ForeColor="Red"></asp:Label></h4>
                            </div>
                            <%--    <div class="panel">
        <h2 align="center">align="left" style="margin-top: -30px; width: 10%; height: 20px;"
            Welcome User!</h2>
        <div style="margin-top: -30px;">
            <a href="#login_form" id="login_pop">Log In</a>
        </div>
    </div>--%>
                            <%--=====Passwrd picture=========--%>
                            <%--<div id="stickynote">
        <br />
        <div id="stickynote-cont">
            Password must
            <ul>
                <li>not contain username</li>
                <li>have min 8 chars</li>
                <li>contain atleast one alphabet, numeric and special char</li>
            </ul>
        </div>
    </div>--%>
                            <%--======popup1--%>
                            <a href="#x" class="overlay" id="login_form"></a>
                            <div class="popup">
                                <h2 align="center" style="margin-top: 34px;">
                                    Sign In Here</h2>
                                <div align="center">
                                </div>
                                <div style="margin-top: 33.5px; margin-left: 15px;">
                                    <div>
                                        <label for="login">
                                            Login ID</label>
                                        <asp:TextBox Width="300px" ID="txtLogin" BackColor="Transparent" runat="server" />
                                    </div>
                                    <div style="margin-top: 18px;">
                                        <label for="password">
                                            Password</label>
                                        <asp:TextBox Width="300px" ID="txtPassword" BackColor="Transparent" runat="server"
                                            TextMode="Password" />
                                    </div>
                                    <asp:Button ID="LoginBtn" Text="Log In" runat="server" BackColor="Transparent" ForeColor="Black"
                                        OnClick="LoginBtn_Click" Style="margin-top: 10px; margin-left: 420px; border: 0;" />
                                </div>
                                <a class="close" href="#close"></a>
                            </div>
                            <%--<a href="#x" class="overlay" id=""></a>
                            <div class="popup">
                                <h2 align="center" style="margin-top: 25px;">
                                    Sign In Here</h2>
                                <div align="center">
                                </div>
                                <div style="margin-top: 40px; margin-left: 5px;">
                                    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
                                        <Scripts>
                                            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                                            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                                            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
                                        </Scripts>
                                    </telerik:RadScriptManager>
                                    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
                                    </telerik:RadAjaxManager>
                                    <div>
                                        <label for="login">
                                            Login ID</label>
                                        <telerik:RadTextBox Width="310px" HoveredStyle-ForeColor="#ce9b55" MaxLength="10"
                                            ToolTip="Max Size 10" ID="txtLogin" runat="server" ForeColor="Blue" />
                                    </div>
                                    <div style="margin-top: 10px;">
                                        <label for="password">
                                            Password</label>
                                        <telerik:RadTextBox Width="310px" ID="txtPassword" runat="server" TextMode="Password" />
                                    </div>
                                    <asp:Button ID="LoginBtn" Text="Log In" Height="20px" Width="50px" runat="server"
                                        BackColor="Transparent" ForeColor="Black" OnClick="LoginBtn_Click" Style="margin-top: 10px;
                                        margin-left: 420px; border: 0;" />
                                </div>
                                <div style="margin-top: 10px;">
                                    <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                </div>
                                <a class="close" href="#close"></a>
                            </div>--%>
                            <%--======popup2--%>
                            <a href="#x" class="overlay" id="join_form"></a>
                            <div class="popup" style="color: White">
                                <h2>
                                    Sign Up</h2>
                                <p>
                                    Please enter your details here</p>
                                <div>
                                    <label for="email">
                                        Login (Email)</label>
                                    <input type="text" id="email" value="" />
                                </div>
                                <div>
                                    <label for="pass">
                                        Password</label>
                                    <input type="password" id="pass" value="" />
                                </div>
                                <div>
                                    <label for="firstname">
                                        First name</label>
                                    <input type="text" id="firstname" value="" />
                                </div>
                                <div>
                                    <label for="lastname">
                                        Last name</label>
                                    <input type="text" id="lastname" value="" />
                                </div>
                                <input type="button" value="Sign Up" />&nbsp;&nbsp;&nbsp;or&nbsp;&nbsp;&nbsp;<a href="#login_form"
                                    id="A1">Log In</a> <a class="close" href="#close"></a>
                            </div>
                            <br />
                            <br />
                            <br />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <%--=====back ground=========--%>
    </form>
</body>
</html>

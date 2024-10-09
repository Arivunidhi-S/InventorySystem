<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportsPPEI.aspx.cs" Inherits="ReportsPPEI" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Stimulsoft.Report.Web, Version=2011.2.1100.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a"
    Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<%--<%@ Register Assembly="Stimulsoft.Report.Web, Version=2012.1, Culture=neutral, PublicKeyToken=ebe6666cba19647a"
    Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PPEI | Reports</title>
    <link rel="shortcut icon" href="Images/inventory.png" />
    <telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server" />
    <link rel="stylesheet" href="css/menu_core.css" type="text/css" />
    <link rel="stylesheet" href="css/modal.css" type="text/css" />
    <link rel="stylesheet" href="css/styles_green.css" type="text/css" />
    <link rel="stylesheet" href="css/skins/menu_simpleAnimated.css" type="text/css" />
    <script type="text/javascript">
        function Confirm() {

            //var confirm_value = document.forms[0].getElementByName("confirm_value");
            //var confirm_value = document.createElement("INPUT");
            //            confirm_value.type = "hidden";
            //            confirm_value.name = "confirm_value";

            //if (sl == "low") {
            if (confirm("Do u want to Proceed?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            //}

            //document.forms[0].appendChild(confirm_value);
        }
    </script>
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
                <table border="0" width="100%">
                    <tr>
                        <td id="Td1" align="center" runat="server" colspan="2">
                            <div align="center">
                                <img src="Images/Banner.jpg" width="1250px" height="70" alt="RSS" />
                            </div>
                            </div>
                            <%--<div align="center" class="panel" style="width: 1230px; height: 30px;">--%>
                            <ul id="myMenu" class="nfMain nfPure">
                                <% for (int a = 0; a < dtMenuItems.Rows.Count; a++)
                                   { %>
                                <li class="nfItem"><a class="nfLink" href="#">
                                    <%= dtMenuItems.Rows[a][0].ToString()  %></a>
                                    <div class="nfSubC nfSubS">
                                        <% dtSubMenuItems = BusinessTier.getSubMenuItems(dtMenuItems.Rows[a][0].ToString(), Session["sesUserID"].ToString().Trim());
                                           int aa;
                                           for (aa = 0; aa < dtSubMenuItems.Rows.Count; aa++)
                                           { %>
                                        <div class="nfItem">
                                            <a class="nfLink" id='<%= dtSubMenuItems.Rows[aa][0].ToString() %>' href='<%= dtSubMenuItems.Rows[aa][1].ToString() %>'>
                                                <%= dtSubMenuItems.Rows[aa][2].ToString()%></a>
                                        </div>
                                        <% } %>
                                    </div>
                                </li>
                                <% } %>
                                <li class="nfItem"><a class="nfLink" href="Login.aspx" style="border-right-width: 1px;">
                                    LOGOUT</a></li>
                            </ul>
                            <%--</div>--%>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <div style="height: 20px;">
                                <asp:Label class="labelstyle" ID="lblStatus" runat="server" ForeColor="Red" Font-Bold="true" />
                            </div>
                        </td>
                    </tr>
                    <tr align="center">
                        <td valign="top" align="center">
                            <table runat="server" width="90%">
                                <tr>
                                    <td>
                                        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
                                        </telerik:RadScriptManager>
                                        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
                                            <AjaxSettings>
                                                <telerik:AjaxSetting AjaxControlID="RadGrid1">
                                                    <UpdatedControls>
                                                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                                                        <telerik:AjaxUpdatedControl ControlID="RadInputManager1" />
                                                        <telerik:AjaxUpdatedControl ControlID="lblStatus" />
                                                        <telerik:AjaxUpdatedControl ControlID="cbostaffno" />
                                                        <telerik:AjaxUpdatedControl ControlID="RadToolTipManager1" />
                                                    </UpdatedControls>
                                                </telerik:AjaxSetting>
                                            </AjaxSettings>
                                        </telerik:RadAjaxManager>
                                        <telerik:RadToolTipManager ID="RadToolTipManager1" OffsetY="-1" HideEvent="ManualClose"
                                            Width="300" Height="305" runat="server" RelativeTo="Element" Position="MiddleRight">
                                        </telerik:RadToolTipManager>
                                        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 35%">
                                        <font style="font-size: 1.0em; color: White"><span>Report</span></font><span> </span>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="txtReport" runat="server" Height="280px" Width="200px" EnableLoadOnDemand="false"
                                            AutoPostBack="true" Filter="None" AppendDataBoundItems="false" OnSelectedIndexChanged="OnSelectedCompo"
                                            EmptyMessage="--Select--">
                                            <Items>
                                                <telerik:RadComboBoxItem runat="server" Text="Departmentwise Report By Price" Value="DepartmentwiseReportByPrice" />
                                                <telerik:RadComboBoxItem runat="server" Text="Minimum StockLevel Report" Value="MinimumStockLevelReport" />
                                                <telerik:RadComboBoxItem runat="server" Text="InventoryList" Value="InventoryList" />
                                                <telerik:RadComboBoxItem runat="server" Text="InventoryList By Staff" Value="InventoryListByStaff" />
                                                <telerik:RadComboBoxItem runat="server" Text="Inventorylist by IssueDate" Value="InventorylistbyIssueDate" />
                                                <telerik:RadComboBoxItem runat="server" Text="Inventorylist by ProductCode" Value="InventorylistbyProductCode" />
                                                <telerik:RadComboBoxItem runat="server" Text="ProductList Report" Value="ProductListReport" />
                                                <telerik:RadComboBoxItem runat="server" Text="StockIncoming Report" Value="StockIncomingReport" />
                                                <telerik:RadComboBoxItem runat="server" Text="StockLevel Report" Value="StockLevelReport" />
                                                <telerik:RadComboBoxItem runat="server" Text="SupplierList Report" Value="SupplierListReport" />
                                                <telerik:RadComboBoxItem runat="server" Text="Warehousewise Incoming" Value="WarehousewiseIncoming" />
                                                <telerik:RadComboBoxItem runat="server" Text="Warehousewise Issue" Value="WarehousewiseIssue" />
                                                <telerik:RadComboBoxItem runat="server" Text="LowStock Report" Value="LowStockReport" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <font style="font-size: 1.0em; color: White"><span>Warehouse</span></font><span>
                                        </span>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="txtWarehouse" runat="server" Height="250px" Width="200px"
                                            DataSourceID="SqlDataSourceWarehouse" DataTextField="WarehouseName" AutoPostBack="false"
                                            AppendDataBoundItems="True" DataValueField="WarehouseId" Visible="true">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="---All---" Value="0" ForeColor="Silver" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <asp:SqlDataSource ID="SqlDataSourceWarehouse" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                            SelectCommand="select WarehouseId,WarehouseName from Warehouse where DELETED=0 order by WarehouseName">
                                        </asp:SqlDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <font style="font-size: 1.0em; color: White"><span>Staff Name</span></font><span>
                                        </span>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="txtStaffId" runat="server" Filter="StartsWith" Height="250px"
                                            Width="200px" DataSourceID="SqlDataSourceStaff" DataTextField="StaffName" AutoPostBack="false"
                                            AppendDataBoundItems="True" DataValueField="StaffId" Visible="true">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="---All---" Value="0" ForeColor="Silver" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <asp:SqlDataSource ID="SqlDataSourceStaff" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                            SelectCommand="select StaffId,StaffNo,StaffName from StaffMaster where DELETED=0 order by StaffNo">
                                        </asp:SqlDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <font style="font-size: 1.0em; color: White"><span>Product Code</span></font><span>
                                        </span>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="txtProdCode" runat="server" Filter="StartsWith" Height="250px"
                                            Width="200px" DataSourceID="SqlDataSourceProd" DataTextField="ProductCode" AutoPostBack="false"
                                            AppendDataBoundItems="True" DataValueField="ID" Visible="true">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="---All---" Value="0" ForeColor="Silver" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <asp:SqlDataSource ID="SqlDataSourceProd" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                            SelectCommand="select ID,ProductCode,ProductName from Products where DELETED=0 order by ProductCode">
                                        </asp:SqlDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <font style="font-size: 1.0em; color: White"><span>Supplier Name</span></font><span>
                                        </span>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="txtSuppName" runat="server" Height="250px" Width="200px"
                                            DataSourceID="SqlDataSourceSupp" DataTextField="SupplierName" AutoPostBack="false"
                                            AppendDataBoundItems="True" DataValueField="SupplierId" Visible="true">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="---All---" Value="0" ForeColor="Silver" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <asp:SqlDataSource ID="SqlDataSourceSupp" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                            SelectCommand="select SupplierId,SupplierName from Supplier where DELETED=0 order by SupplierName">
                                        </asp:SqlDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <font style="font-size: 1.0em; color: White"><span>Department</span></font><span>
                                        </span>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="txtDeptName" runat="server" Height="250px" Width="200px"
                                            DataSourceID="SqlDataSourceDept" DataTextField="DeptName" AutoPostBack="false"
                                            AppendDataBoundItems="True" DataValueField="DeptId" Visible="true">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="---All---" Value="0" ForeColor="Silver" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <asp:SqlDataSource ID="SqlDataSourceDept" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                            SelectCommand="select DeptId,DeptName from Department where DELETED=0 order by DeptName">
                                        </asp:SqlDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <font style="font-size: 1.0em; color: White"><span>From</span></font><span> </span>
                                    </td>
                                    <td>
                                        <telerik:RadDatePicker ID="txtFromDate" runat="server" AutoPostBack="true" DateInput-EmptyMessage="MinDate"
                                            MinDate="01/01/1000" MaxDate="01/01/3000" DateInput-DateFormat="dd/MMM/yyyy">
                                            <Calendar ID="Calendar1" runat="server">
                                                <SpecialDays>
                                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday" />
                                                </SpecialDays>
                                            </Calendar>
                                        </telerik:RadDatePicker>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <font style="font-size: 1.0em; color: White"><span>To</span></font><span> </span>
                                    </td>
                                    <td>
                                        <telerik:RadDatePicker ID="txtToDate" runat="server" AutoPostBack="true" DateInput-EmptyMessage="MaxDate"
                                            MinDate="01/01/1000" MaxDate="01/01/3000" DateInput-DateFormat="dd/MMM/yyyy">
                                            <Calendar ID="Calendar4" runat="server">
                                                <SpecialDays>
                                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday" />
                                                </SpecialDays>
                                            </Calendar>
                                        </telerik:RadDatePicker>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnsubmit" runat="server" Class="buttonstyle" Text="Submit" Width="70px"
                                            OnClick="btnsubmit_Click" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMsg" runat="server" Text="" Width="200px" class="style2"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="left">
                            <cc1:StiWebViewer ID="StiWebViewer1" runat="server" RenderMode="AjaxWithCache" ScrollBarsMode="true"
                                Width="920px" Height="500px" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>

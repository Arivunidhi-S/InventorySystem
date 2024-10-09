<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PPEIssue.aspx.cs" Inherits="PPEIssue" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PPE Issue</title>
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
    <form id="form1" method="post" runat="server">
    <input type="hidden" id="confirm_value" name="confirm_value" value="No" />
    <%--<cc1:msgBox ID="MsgBox1" runat="server"></cc1:msgBox>--%>
    <div>
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
                        <tr>
                            <td align="center" style="width: 100%;">
                                <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
                                    <%--                                <Scripts>
                                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
                                </Scripts>--%>
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
                                <telerik:RadInputManager ID="RadInputManager1" runat="server">
                                    <telerik:TextBoxSetting BehaviorID="TextBoxBehavior1" InitializeOnClient="false"
                                        Validation-IsRequired="true" ErrorMessage="Mandatory Fields">
                                        <TargetControls>
                                            <%--<telerik:TargetInput ControlID="txtProductCode" />
                                            <telerik:TargetInput ControlID="txtproductname" />--%>
                                        </TargetControls>
                                    </telerik:TextBoxSetting>
                                </telerik:RadInputManager>
                                <div class="mycss">
                                    OUTGOING DETAILS
                                </div>
                                <br />
                                <div id="Div10" runat="server" style="width: 1250px; overflow: auto;" align="center">
                                    <telerik:RadGrid ID="RadGrid1" runat="server" AllowMultiRowEdit="false" Skin="Hay"
                                        OnNeedDataSource="RadGrid1_NeedDataSource" GridLines="Vertical" AllowPaging="True"
                                        OnItemCreated="RadGrid1_ItemCreated" PagerStyle-AlwaysVisible="true" PagerStyle-Position="Bottom"
                                        OnDeleteCommand="RadGrid1_DeleteCommand" OnItemCommand="RadGrid1_ItemCommand"
                                        PageSize="15" AllowAutomaticUpdates="false" AllowAutomaticInserts="false" OnItemDataBound="RadGrid1_ItemDataBound"
                                        PagerStyle-Mode="NextPrevNumericAndAdvanced" AllowAutomaticDeletes="false" OnInsertCommand="RadGrid1_InsertCommand"
                                        AllowSorting="false" AllowFilteringByColumn="true" OnUpdateCommand="RadGrid1_UpdateCommand">
                                        <ClientSettings AllowDragToGroup="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                                            EnableRowHoverStyle="true" EnablePostBackOnRowClick="false">
                                            <Resizing AllowColumnResize="true" />
                                            <Selecting AllowRowSelect="true" />
                                        </ClientSettings>
                                        <MasterTableView AutoGenerateColumns="false" DataKeyNames="ID" CommandItemDisplay="Top"
                                            CommandItemSettings-AddNewRecordText="Add New Outgoing Details" InsertItemPageIndexAction="ShowItemOnFirstPage">
                                            <PagerStyle Mode="NextPrevNumericAndAdvanced" />
                                            <Columns>
                                                <telerik:GridEditCommandColumn ButtonType="ImageButton">
                                                    <HeaderStyle Width="3%" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </telerik:GridEditCommandColumn>
                                                <telerik:GridBoundColumn DataField="id" DataType="System.Int64" HeaderText="ID" ReadOnly="True"
                                                    SortExpression="ID" UniqueName="ID" AllowFiltering="false" AllowSorting="false"
                                                    Visible="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="productcode" DataType="System.String" HeaderText="Product Code"
                                                    SortExpression="productcode" UniqueName="productcode" FilterControlWidth="60px">
                                                    <HeaderStyle Width="7%" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="productname" DataType="System.String" HeaderText="Product Name"
                                                    SortExpression="productname" UniqueName="productname" FilterControlWidth="220px">
                                                    <HeaderStyle Width="16%" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="staffno" DataType="System.String" HeaderText="StaffNo"
                                                    SortExpression="staffno" UniqueName="staffno">
                                                    <HeaderStyle Width="7%" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="staffname" DataType="System.String" HeaderText="Staff Name"
                                                    SortExpression="staffname" UniqueName="staffname" FilterControlWidth="220px">
                                                    <HeaderStyle Width="20%" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="deptname" DataType="System.String" HeaderText="Department Name"
                                                    SortExpression="deptname" UniqueName="department" FilterControlWidth="120px">
                                                    <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="quantity" DataType="System.String" HeaderText="Qnty"
                                                    SortExpression="quantity" UniqueName="quantity" AllowFiltering="false">
                                                    <HeaderStyle Width="3%" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Issuedate" DataType="System.DateTime" HeaderText="Issue Date"
                                                    SortExpression="Issuedate" UniqueName="Issuedate" DataFormatString="{0:dd/MMM/yyyy}">
                                                    <HeaderStyle Width="8%" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridButtonColumn CommandName="Delete" UniqueName="DeleteColumn" ButtonType="ImageButton"
                                                    ConfirmText="Are you sure want to delete?">
                                                    <HeaderStyle Width="3%" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridButtonColumn>
                                            </Columns>
                                            <EditFormSettings EditFormType="Template">
                                                <EditColumn UniqueName="EditCommandColumn1">
                                                </EditColumn>
                                                <FormTemplate>
                                                    <table cellspacing="2" cellpadding="2" width="100%" border="0">
                                                        <tr>
                                                            <td colspan="2" align="left">
                                                                <b>ID:
                                                                    <asp:Label ID="lblID" runat="server" Visible="false" Text='<%# Bind("ID")%>' />
                                                                </b>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <b>Staff No:
                                                                    <asp:Label ID="Label11" Text="*" runat="server" Style="color: Red; font-size: smaller;
                                                                        width: 1px;" /></b>
                                                            </td>
                                                            <td align="left">
                                                                <telerik:RadComboBox ID="cbostaffno" MarkFirstMatch="true" Filter="StartsWith" runat="server"
                                                                    Height="300px" Width="200px" DataValueField="staffid" DataTextField="staffname"
                                                                    Skin="Hay" OnItemsRequested="staffcode_OnItemsRequested" OnSelectedIndexChanged="OnSelectedIndexChangedStaff"
                                                                    Text='<%# Bind("StaffName") %>' DropDownWidth="300px" EnableLoadOnDemand="true"
                                                                    AppendDataBoundItems="True" EmptyMessage="Select">
                                                                    <HeaderTemplate>
                                                                        <table style="width: 250px; font-size: small" cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td style="width: 100px;">
                                                                                    StaffNo
                                                                                </td>
                                                                                <td style="width: 150px;">
                                                                                    StaffName
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <table style="width: 250px; font-size: small" cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td style="width: 100px;" align="left">
                                                                                    <%# DataBinder.Eval(Container, "Attributes['staffno']")%>
                                                                                </td>
                                                                                <td style="width: 150px;" align="left">
                                                                                    <%# DataBinder.Eval(Container, "Text")%>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                </telerik:RadComboBox>
                                                            </td>
                                                            <td align="left">
                                                                Warehouse:
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox Width="200px" ID="txtwarehouse" Enabled="false" MaxLength="100" ToolTip="Maximum Length: 100"
                                                                    runat="server" Text='<%# Bind("warehousename") %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <b>Department:
                                                                    <asp:Label ID="Label12" Text="*" runat="server" Style="color: Red; font-size: smaller;
                                                                        width: 1px;" /></b>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox Width="200px" ID="txtdepartment" Enabled="false" MaxLength="100" ToolTip="Maximum Length: 100"
                                                                    runat="server" Text='<%# Bind("deptname") %>' />
                                                            </td>
                                                            <td align="left">
                                                                Designation:
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox Width="200px" ID="txtdesignation" Enabled="false" MaxLength="50" ToolTip="Maximum Length: 50"
                                                                    runat="server" Text='<%# Bind("designation") %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                Location:
                                                            </td>
                                                            <td align="left">
                                                               <%-- <asp:TextBox Width="200px" ID="txtstafflocation" Enabled="false" MaxLength="50" ToolTip="Maximum Length: 50"
                                                                    runat="server" Text='<%# Bind("location") %>' />--%>
                                                            </td>
                                                            <td align="left">
                                                                IC No:
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox Width="200px" ID="txttele" Enabled="false" MaxLength="50" ToolTip="Maximum Length: 50"
                                                                    runat="server" Text='<%# Bind("phone") %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <b>Products in Stock:
                                                                    <asp:Label ID="Label7" Text="*" runat="server" Style="color: Red; font-size: smaller;
                                                                        width: 1px;" /></b>
                                                            </td>
                                                            <td align="left">
                                                                <telerik:RadComboBox ID="cboprodstock" runat="server" MarkFirstMatch="true" Filter="StartsWith"
                                                                    Height="250px" Width="200px" DataValueField="productid" DataTextField="productname"
                                                                    Skin="Hay" OnItemsRequested="productcodecode_OnItemsRequested" OnSelectedIndexChanged="OnSelectedIndexChangedproductreceived"
                                                                    Text='<%# Bind("productname") %>' EnableLoadOnDemand="true" AutoPostBack="true"
                                                                    DropDownWidth="300px" AppendDataBoundItems="True" EmptyMessage="--Select--">
                                                                    <HeaderTemplate>
                                                                        <table style="width: 300px; font-size: small" cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td style="width: 100px;">
                                                                                    Productcode
                                                                                </td>
                                                                                <td style="width: 200px;">
                                                                                    Productname
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <table style="width: 300px; font-size: small" cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td style="width: 100px;" align="left">
                                                                                    <%# DataBinder.Eval(Container, "Attributes['productcode']")%>
                                                                                </td>
                                                                                <td style="width: 200px;" align="left">
                                                                                    <%# DataBinder.Eval(Container, "Text")%>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                </telerik:RadComboBox>
                                                            </td>
                                                            <td align="left">
                                                                <b>Received Products:
                                                                    <asp:Label ID="Label1" Text="*" runat="server" Style="color: Red; font-size: smaller;
                                                                        width: 1px;" /></b>
                                                            </td>
                                                            <td align="left">
                                                                <telerik:RadComboBox ID="cbproreceived" runat="server" Filter="Contains" Height="250px"
                                                                    Width="200px" DataValueField="Balqty" DropDownWidth="300px" DataTextField="incomingdate"
                                                                    AutoPostBack="true" EnableLoadOnDemand="true" Skin="Hay" OnSelectedIndexChanged="OnSelectedIndexChangedIncomeQuantity"
                                                                    AppendDataBoundItems="true" Text='<%# Bind("incomingdate") %>' EmptyMessage="--Select--">
                                                                    <HeaderTemplate>
                                                                        <table style="width: 300px; font-size: small" cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td style="width: 70px;">
                                                                                    Quantity
                                                                                </td>
                                                                                <td style="width: 230px;">
                                                                                    Incoming Date
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <table style="width: 300px; font-size: small" cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td style="width: 70px;" align="left">
                                                                                    <%# DataBinder.Eval(Container, "Attributes['Balqty']")%>
                                                                                </td>
                                                                                <td style="width: 230px;" align="left">
                                                                                    <%# DataBinder.Eval(Container, "Text")%>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                </telerik:RadComboBox>
                                                                <asp:Label ID="Lbldupdate" Text='<%# Bind("incomingdate") %>' Visible="false" runat="server" /></b>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <b>Quantity Issued:
                                                                    <asp:Label ID="Label9" Text="*" runat="server" Style="color: Red; font-size: smaller;
                                                                        width: 1px;" /></b>
                                                            </td>
                                                            <td align="left">
                                                                <telerik:RadNumericTextBox Width="200px" Type="Number" ID="txtquantityissue" MaxLength="10"
                                                                    ToolTip="Maximum Length: 10" EmptyMessage="Enter the Quantity" MinValue="0" runat="server"
                                                                    Text='<%# Bind("quantity") %>'>
                                                                    <NumberFormat GroupSeparator="" DecimalDigits="0" />
                                                                </telerik:RadNumericTextBox>
                                                            </td>
                                                            <td align="left">
                                                                <b>Issued Date:
                                                                    <asp:Label ID="Label2" Text="*" runat="server" Style="color: Red; font-size: smaller;
                                                                        width: 1px;" /></b>
                                                            </td>
                                                            <td align="left">
                                                                <telerik:RadDateTimePicker ID="txtissuedate" runat="server" Width="200px" DbSelectedDate='<%# Bind("issuedate") %>'
                                                                    PopupDirection="BottomRight" AutoPostBackControl="Both" OnSelectedDateChanged="OutgoingSelectDate_SelectedDateChanged"
                                                                    DateInput-EmptyMessage="Select Issue Date" Skin="Hay">
                                                                    <Calendar ID="Calendar1" runat="server" ShowRowHeaders="true">
                                                                    </Calendar>
                                                                    <DateInput ID="DateInput1" runat="server" Enabled="true" DateFormat="d/MMM/yyyy hh:mm tt">
                                                                    </DateInput>
                                                                </telerik:RadDateTimePicker>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="Label4" Text="Product Minimum Stock:" ForeColor="Black" Width="200px"
                                                                    runat="server" />
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label Width="200px" ID="txtminstock" MaxLength="50" ForeColor="Black" Font-Bold="true"
                                                                    runat="server" Text='<%# Bind("minimumstock") %>' />
                                                            </td>
                                                            <td align="left" style="visibility: visible">
                                                                ExpiredDate:
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="LblExpiredate" runat="server" Text='<%# Bind("expiredate")%>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" valign="top">
                                                                <asp:Label ID="Label3" Text="Reason Of Issuance:" ForeColor="Black" runat="server" />
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox Width="200px" ID="txtIssue" Enabled="true" Wrap="true" TextMode="MultiLine"
                                                                    Height="40" MaxLength="50" ToolTip="Maximum Length: 50" runat="server" />
                                                                <%--Text='<%# Bind("Issuance") %>'--%>
                                                            </td>
                                                            <td align="left" style="visibility: visible">
                                                                <asp:Label ID="Label6" Text="Safety Brief By:" ForeColor="Black" runat="server" />
                                                            </td>
                                                            <td align="left">
                                                                <telerik:RadComboBox ID="cboSafetyBy" MarkFirstMatch="true" Filter="StartsWith" runat="server"
                                                                    Height="300px" Width="200px" DataValueField="staffid" DataTextField="staffname"
                                                                    Skin="Hay" OnItemsRequested="staffcode_OnItemsRequested" OnSelectedIndexChanged="OnSelectedIndexChangedStaff"
                                                                    DropDownWidth="300px" EnableLoadOnDemand="true" AppendDataBoundItems="True" EmptyMessage="Select">
                                                                    <%--Text='<%# Bind("SafetybyName") %>'--%>
                                                                    <HeaderTemplate>
                                                                        <table style="width: 250px; font-size: small" cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td style="width: 100px;">
                                                                                    StaffNo
                                                                                </td>
                                                                                <td style="width: 150px;">
                                                                                    StaffName
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <table style="width: 250px; font-size: small" cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td style="width: 100px;" align="left">
                                                                                    <%# DataBinder.Eval(Container, "Attributes['staffno']")%>
                                                                                </td>
                                                                                <td style="width: 150px;" align="left">
                                                                                    <%# DataBinder.Eval(Container, "Text")%>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                </telerik:RadComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="Label8" Text="Safety Brief Date:" ForeColor="Black" runat="server" />
                                                            </td>
                                                            <td align="left">
                                                                <telerik:RadDateTimePicker ID="dtSafetyDate" runat="server" Width="200px" Skin="Hay"
                                                                    PopupDirection="BottomRight" AutoPostBackControl="Both" DateInput-EmptyMessage="Select Issue Date">
                                                                    <%--DbSelectedDate='<%# Bind("SafetyDate") %>'--%>
                                                                    <Calendar ID="Calendar2" runat="server" ShowRowHeaders="true">
                                                                    </Calendar>
                                                                    <DateInput ID="DateInput2" runat="server" Enabled="true" DateFormat="d/MMM/yyyy hh:mm tt">
                                                                    </DateInput>
                                                                </telerik:RadDateTimePicker>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="lblavlissue" Visible="false" Font-Bold="true" Text="Avaliable Issue Qty:"
                                                                    ForeColor="Black" Width="100px" runat="server" Style="font-size: smaller;" />
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblavlstock" Visible="false" ForeColor="Black" runat="server" Style="font-size: smaller;" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="Label10" Visible="false" Text=" Selected Date Quantity:" ForeColor="Blue"
                                                                    runat="server" Style="font-size: smaller; width: 1px;" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox Width="200px" Visible="false" ID="TxtDateqty" MaxLength="50" ToolTip="Maximum Length: 50"
                                                                    runat="server" />
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="Label5" Visible="false" Text="Total Quantity of ProductID:" ForeColor="Blue"
                                                                    runat="server" Style="font-size: smaller; width: 1px;" />
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox Width="200px" Visible="false" ID="Txtincomeqty" MaxLength="50" ToolTip="Maximum Length: 50"
                                                                    runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:Label Width="200px" ID="lblUpdateQty" Visible="false" MaxLength="50" ToolTip=" Update Quantity:"
                                                                    runat="server" Text='<%# Bind("quantity") %>' />
                                                                <asp:Label Width="200px" ID="Lblstock" Visible="false" MaxLength="50" ToolTip=" Stock:"
                                                                    runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" align="left">
                                                                <%-- <asp:Button ID="Button1" runat="server"  Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
                                                                OnClientClick = "Confirm()"   CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>'>--%>
                                                                <asp:Button ID="Button3" runat="server" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
                                                                    CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>'>
                                                                </asp:Button>
                                                                <asp:Button ID="Button2" runat="server" Text="Cancel" CausesValidation="false" CommandName="Cancel">
                                                                </asp:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </FormTemplate>
                                            </EditFormSettings>
                                        </MasterTableView>
                                        <PagerStyle Mode="NumericPages"></PagerStyle>
                                    </telerik:RadGrid>
                                    <asp:SqlDataSource ID="SqlDatastaff" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                        SelectCommand="SELECT [staffid], [staffno], [staffname] FROM [staffmaster] where deleted=0 ORDER BY [staffid]">
                                    </asp:SqlDataSource>
                                    <asp:SqlDataSource ID="SqlDataprod" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                        SelectCommand="SELECT [id], [productcode] FROM [products] where deleted=0 ORDER BY [id]">
                                    </asp:SqlDataSource>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>

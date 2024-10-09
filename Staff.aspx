<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Staff.aspx.cs" Inherits="Staff" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<link rel="shortcut icon" href="Images/inventory.png" />
    <title>Master Staff</title>
    <telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server" />
    <link rel="stylesheet" href="css/menu_core.css" type="text/css" />
    <link rel="stylesheet" href="css/modal.css" type="text/css" />
    <link rel="stylesheet" href="css/styles_green.css" type="text/css" />
    <link rel="stylesheet" href="css/skins/menu_simpleAnimated.css" type="text/css" />
    <script type="javascript">
        history.go(1); /* undo user navigation (ex: IE Back Button) */
    </script>
    <style type="text/css">
        .mycss
{
text-shadow:1px 1px 3px rgba(99,99,99,1);
font-weight:bold;
color:#ffffff;
letter-spacing:1pt;
word-spacing:0pt;
font-size:25px;
text-align:center;
font-family:times new roman, times, serif;
}
    body {
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
                                <Scripts>
                                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
                                </Scripts>
                            </telerik:RadScriptManager>
                            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
                                <AjaxSettings>
                                    <telerik:AjaxSetting AjaxControlID="RadGrid1">
                                        <UpdatedControls>
                                            <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                                            <telerik:AjaxUpdatedControl ControlID="RadInputManager1" />
                                            <telerik:AjaxUpdatedControl ControlID="lblStatus" />
                                            <telerik:AjaxUpdatedControl ControlID="cboCompany" />
                                            <telerik:AjaxUpdatedControl ControlID="cbowarehouse" />
                                            <telerik:AjaxUpdatedControl ControlID="cboDept" />
                                            <telerik:AjaxUpdatedControl ControlID="RadAjaxLoadingPanel1" />
                                            <telerik:AjaxUpdatedControl ControlID="RadToolTipManager1" />
                                        </UpdatedControls>
                                    </telerik:AjaxSetting>
                                </AjaxSettings>
                            </telerik:RadAjaxManager>
                            <telerik:RadInputManager ID="RadInputManager1" runat="server">
                                <telerik:TextBoxSetting BehaviorID="TextBoxBehavior1" InitializeOnClient="false"
                                    Validation-IsRequired="true" ErrorMessage="Mandatory Fields">
                                    <TargetControls>
                                        <telerik:TargetInput ControlID="txtCode" />
                                        <telerik:TargetInput ControlID="txtName" />
                                    </TargetControls>
                                </telerik:TextBoxSetting>
                                <%--<telerik:RegExpTextBoxSetting BehaviorID="RagExpBehavior1" Validation-IsRequired="true"
                                    ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" ErrorMessage="Invalid Email">
                                    <TargetControls>
                                        <telerik:TargetInput ControlID="txtEmail" />
                                    </TargetControls>
                                </telerik:RegExpTextBoxSetting>
                                <telerik:RegExpTextBoxSetting BehaviorID="RagExpBehavior3" ValidationExpression="[0-9]{6,}"
                                    ErrorMessage="Enter more than 6 figures" IsRequiredFields="true" Validation-IsRequired="true">
                                    <TargetControls>
                                        <telerik:TargetInput ControlID="txtPhone" />
                                    </TargetControls>
                                </telerik:RegExpTextBoxSetting>--%>
                            </telerik:RadInputManager>
                            <telerik:RadToolTipManager ID="RadToolTipManager1" OffsetY="-1" HideEvent="ManualClose"
                                Width="300" Height="305" runat="server" RelativeTo="Element" Position="MiddleRight">
                            </telerik:RadToolTipManager>
                            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
                            <div class="mycss">
                                STAFF MASTER DETAILS
                            </div>
                               <br />
                            <div id="Div10" runat="server" style="width: 1250px;
                                overflow: auto;" align="center">
                                <telerik:RadGrid ID="RadGrid1" runat="server" AllowMultiRowEdit="false" Skin="Hay"
                                    OnNeedDataSource="RadGrid1_NeedDataSource" GridLines="Vertical" AllowPaging="True" PageSize="10"
                                    OnItemCreated="RadGrid1_ItemCreated" PagerStyle-AlwaysVisible="true" PagerStyle-Position="Bottom"
                                    OnDeleteCommand="RadGrid1_DeleteCommand" AllowAutomaticUpdates="false" AllowAutomaticInserts="false"
                                    OnItemDataBound="RadGrid1_ItemDataBound" PagerStyle-Mode="NextPrevNumericAndAdvanced"
                                    AllowAutomaticDeletes="false" OnInsertCommand="RadGrid1_InsertCommand" AllowSorting="false"
                                    AllowFilteringByColumn="true" OnUpdateCommand="RadGrid1_UpdateCommand">
                                    <ClientSettings AllowDragToGroup="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                                        EnableRowHoverStyle="true" EnablePostBackOnRowClick="false">
                                        <Resizing AllowColumnResize="true" />
                                        <Selecting AllowRowSelect="true" />
                                    </ClientSettings>
                                    <MasterTableView AutoGenerateColumns="false" DataKeyNames="StaffId" CommandItemDisplay="Top"
                                        CommandItemSettings-AddNewRecordText="Add New Staff Details" InsertItemPageIndexAction="ShowItemOnFirstPage">
                                        <PagerStyle Mode="NextPrevNumericAndAdvanced" />
                                        <Columns>
                                            <telerik:GridEditCommandColumn ButtonType="ImageButton">
                                                <HeaderStyle Width="3%" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridEditCommandColumn>
                                            <telerik:GridBoundColumn DataField="StaffId" DataType="System.Int64" HeaderText="ID"
                                                ReadOnly="True" SortExpression="StaffId" UniqueName="StaffId" AllowFiltering="false"
                                                AllowSorting="false" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="StaffNo" DataType="System.String" HeaderText="Staff Code"
                                                SortExpression="StaffNo" UniqueName="StaffNo">
                                                <HeaderStyle Width="10%" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="StaffName" DataType="System.String" HeaderText="Staff Name"
                                                SortExpression="StaffName" UniqueName="StaffName">
                                                <HeaderStyle Width="15%" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Designation" DataType="System.String" HeaderText="Designation"
                                                SortExpression="Designation" UniqueName="Designation">
                                                <HeaderStyle Width="10%" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="CompanyName" DataType="System.String" HeaderText="Company Name"
                                                SortExpression="CompanyName" UniqueName="CompanyName">
                                                <HeaderStyle Width="10%" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="DeptName" DataType="System.String" HeaderText="DeptName"
                                                SortExpression="DeptName" UniqueName="DeptName">
                                                <HeaderStyle Width="10%" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="warehousename" DataType="System.String" HeaderText="Warehouse Name"
                                                SortExpression="warehousename" UniqueName="warehousename">
                                                <HeaderStyle Width="15%" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Location" DataType="System.String" HeaderText="Phone"
                                                SortExpression="Location" UniqueName="Location">
                                                <HeaderStyle Width="15%" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Phone" DataType="System.String" HeaderText="IC No"
                                                SortExpression="Phone" UniqueName="Phone">
                                                <HeaderStyle Width="10%" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridButtonColumn CommandName="Delete" UniqueName="DeleteColumn" ButtonType="ImageButton"
                                                ConfirmText="Are you sure want to delete?">
                                                <HeaderStyle Width="3%" />
                                                <ItemStyle HorizontalAlign="Left" />
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
                                                                <asp:Label ID="lblID" runat="server" Text='<%# Bind("StaffId")%>' />
                                                            </b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LblCode" Visible="false" Text='<%# Bind("StaffNo") %>' runat="server" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="LblName" Visible="false" Text='<%# Bind("StaffName") %>' runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        
                                                        <td align="left">
                                                            Staff Name:
                                                            <asp:Label ID="Label1" Text="*" runat="server" Style="color: Red; font-size: smaller;
                                                                width: 1px;" />
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox Width="200px" ID="txtName" MaxLength="50" ToolTip="Maximum Length: 50"
                                                                runat="server" Text='<%# Bind("StaffName") %>' />
                                                        </td>
                                                        <td align="left">
                                                            Staff Code:
                                                            <asp:Label ID="LblstffName" Text="*" runat="server" Style="color: Red; font-size: smaller;
                                                                width: 1px;" />
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox Width="200px" ID="txtCode" MaxLength="20" ToolTip="Maximum Length: 20"
                                                                runat="server" Text='<%# Bind("StaffNo") %>' />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            Designation:
                                                        </td>
                                                        <td align="left">
                                                            <telerik:RadTextBox Width="200px" ID="txtDesignation" MaxLength="100" ToolTip="Maximum Length: 100"
                                                                runat="server" Text='<%# Bind("Designation") %>' />
                                                        </td>
                                                        <td align="left">
                                                            Phone:
                                                        </td>
                                                        <td align="left">
                                                            <telerik:RadTextBox Width="200px" ID="txtLocation" MaxLength="30" ToolTip="Maximum Length: 30"
                                                                runat="server" Text='<%# Bind("Location") %>' />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            Email:
                                                        </td>
                                                        <td align="left">
                                                            <telerik:RadTextBox Width="200px" ID="txtEmail" MaxLength="50" ToolTip="Maximum Length: 50"
                                                                runat="server" Text='<%# Bind("Email") %>' />
                                                            <asp:RegularExpressionValidator ID="emailValidator" runat="server" Display="Dynamic"
                                                                ErrorMessage="Please, enter valid e-mail address." ValidationExpression="^[\w\.\-]+@[a-zA-Z0-9\-]+(\.[a-zA-Z0-9\-]{1,})*(\.[a-zA-Z]{2,3}){1,2}$"
                                                                ControlToValidate="txtEmail">
                                                            </asp:RegularExpressionValidator>
                                                        </td>
                                                        <td align="left">
                                                            IC NO:
                                                        </td>
                                                        <td align="left">
                                                            <telerik:RadMaskedTextBox Width="200px" ID="txtPhone" 
                                                                 Mask="######-##-########" MaxLength="25" ToolTip="Maximum Length: 15"
                                                                runat="server" Text='<%# Bind("Phone") %>' />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            Company:
                                                            <asp:Label ID="Label5" Text="*" runat="server" Style="color: Red; font-size: smaller;
                                                                width: 1px;" />
                                                        </td>
                                                        <td align="left">
                                                            <telerik:RadComboBox ID="cboCompany" runat="server" Height="300px" Width="200px" DataSourceID="SqlDataSourceCompany"
                                                                DataTextField="CompanyName" DataValueField="CompanyId" DropDownWidth="300px"
                                                                AppendDataBoundItems="True" Text='<%# Bind("CompanyId") %>' OnSelectedIndexChanged="OnSelectedIndexcompany"
                                                                AutoPostBack="true">
                                                                <Items>
                                                                    <telerik:RadComboBoxItem Text="--Select--" Value="--Select--" ForeColor="Silver" />
                                                                </Items>
                                                            </telerik:RadComboBox>
                                                            <asp:Label ID="Lblcmpy" Visible="false" Text='<%# Bind("Companyname") %>' runat="server" />
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="lbldname" Visible="false" Text="Department:" runat="server" />
                                                            <asp:Label ID="lblderr" Visible="false" Text="*" runat="server" Style="color: Red;
                                                                font-size: smaller; width: 1px;" />
                                                        </td>
                                                        <td align="left">
                                                            <telerik:RadComboBox ID="cboDept" Visible="false" runat="server" Height="60px" Width="200px"
                                                                DataSourceID="SqlDataSourceDept" DataTextField="DeptName" DataValueField="DeptId"
                                                                DropDownWidth="300px" AppendDataBoundItems="True" Text='<%# Bind("DeptId") %>'
                                                                EmptyMessage="--Select--">
                                                                <HeaderTemplate>
                                                                    <table style="width: 250px; font-weight: bold" cellspacing="0" cellpadding="0">
                                                                        <tr>
                                                                            <%--<td style="width: 30px;">
                                                                                Deptid
                                                                            </td>--%>
                                                                            <td style="width: 70px;">
                                                                                Dept code
                                                                            </td>
                                                                            <td style="width: 70px;">
                                                                                Dept Name
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <table style="width: 250px" cellspacing="0" cellpadding="0">
                                                                        <tr>
                                                                            <%-- <td style="width: 70px;" align="left">
                                                                                <%# DataBinder.Eval(Container, "Attributes['Deptid']")%>
                                                                            </td>--%>
                                                                            <td style="width: 70px;" align="left">
                                                                                <%# DataBinder.Eval(Container, "Attributes['Deptcode']")%>
                                                                            </td>
                                                                            <td style="width: 180px;" align="left">
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
                                                            <asp:Label ID="Lblwname" Visible="false" Text="Warehouse:" runat="server" />
                                                            <asp:Label ID="Lblwerr" Text="*" Visible="false" runat="server" Style="color: Red;
                                                                font-size: smaller; width: 1px;" />
                                                        </td>
                                                        <td align="left">
                                                            <telerik:RadComboBox ID="cbowarehouse" Visible="false" runat="server" Height="60px"
                                                                Width="200px" DataValueField="warehouseid" DataTextField="warehousename" Text='<%# Bind("warehousename") %>'
                                                                DropDownWidth="300px" AllowCustomText="true" Filter="StartsWith" EnableLoadOnDemand="true"
                                                                AppendDataBoundItems="True" EmptyMessage="--Select--">
                                                                <HeaderTemplate>
                                                                    <table style="width: 300px; font-weight: bold" cellspacing="0" cellpadding="0">
                                                                        <tr>
                                                                            <td style="width: 70px;">
                                                                                Warehouse ID
                                                                            </td>
                                                                            <td style="width: 180px;">
                                                                                Warehouse Name
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <table style="width: 250px" cellspacing="0" cellpadding="0">
                                                                        <tr>
                                                                            <td style="width: 70px;" align="left">
                                                                                <%# DataBinder.Eval(Container, "Attributes['warehouseid']")%>
                                                                            </td>
                                                                            <td style="width: 180px;" align="left">
                                                                                <%# DataBinder.Eval(Container, "Text")%>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                            </telerik:RadComboBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4" align="left">
                                                            <asp:Button ID="Button1" runat="server" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
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
                                <asp:SqlDataSource ID="SqlDataSourceDept" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                    SelectCommand="SELECT [DeptId], [DeptName] FROM [Department] where deleted=0 ORDER BY [DeptName]">
                                </asp:SqlDataSource>
                                <asp:SqlDataSource ID="SqlDataSourceCompany" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                    SelectCommand="SELECT [CompanyId], [CompanyName] FROM [Company] where deleted=0 ORDER BY [CompanyName]">
                                </asp:SqlDataSource>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>

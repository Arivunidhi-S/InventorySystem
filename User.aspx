<%@ Page Language="C#" AutoEventWireup="true" CodeFile="User.aspx.cs" Inherits="User" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<link rel="shortcut icon" href="Images/inventory.png" />
    <title>Master User</title>
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
                            </telerik:RadScriptManager>
                            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
                                <AjaxSettings>
                                    <telerik:AjaxSetting AjaxControlID="RadGrid1">
                                        <UpdatedControls>
                                            <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                                            <telerik:AjaxUpdatedControl ControlID="RadInputManager1" />
                                            <telerik:AjaxUpdatedControl ControlID="lblStatus" />
                                            <telerik:AjaxUpdatedControl ControlID="cbocompany" />
                                            <telerik:AjaxUpdatedControl ControlID="cbowarehouse" />
                                        </UpdatedControls>
                                    </telerik:AjaxSetting>
                                </AjaxSettings>
                            </telerik:RadAjaxManager>
                            <telerik:RadInputManager ID="RadInputManager1" runat="server">
                                <telerik:TextBoxSetting BehaviorID="TextBoxBehavior1" InitializeOnClient="false"
                                    Validation-IsRequired="true" ErrorMessage="Mandatory Fields">
                                    <TargetControls>
                                        <telerik:TargetInput ControlID="txtUserID" />
                                        <telerik:TargetInput ControlID="txtPassword" />
                                    </TargetControls>
                                </telerik:TextBoxSetting>
                            </telerik:RadInputManager>
                            <div class="mycss">
                                USER MASTER DETAILS
                            </div>
                            <br />
                            <div id="Div10" runat="server" style="width: 1250px; 
                                overflow: auto;" align="center">
                                <telerik:RadGrid ID="RadGrid1" runat="server" AllowMultiRowEdit="false" Skin="Hay"
                                    OnNeedDataSource="RadGrid1_NeedDataSource" GridLines="Vertical" AllowPaging="True"
                                    PageSize="10" OnItemCreated="RadGrid1_ItemCreated" PagerStyle-AlwaysVisible="true"
                                    PagerStyle-Position="Bottom" OnDeleteCommand="RadGrid1_DeleteCommand" AllowAutomaticUpdates="false"
                                    AllowAutomaticInserts="false" OnItemDataBound="RadGrid1_ItemDataBound" PagerStyle-Mode="NextPrevNumericAndAdvanced"
                                    AllowAutomaticDeletes="false" OnInsertCommand="RadGrid1_InsertCommand" AllowSorting="false"
                                    AllowFilteringByColumn="true" OnUpdateCommand="RadGrid1_UpdateCommand">
                                    <ClientSettings AllowDragToGroup="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                                        EnableRowHoverStyle="true" EnablePostBackOnRowClick="false">
                                        <Resizing AllowColumnResize="true" />
                                        <Selecting AllowRowSelect="true" />
                                    </ClientSettings>
                                    <MasterTableView AutoGenerateColumns="false" DataKeyNames="ID" InsertItemPageIndexAction="ShowItemOnFirstPage"
                                        CommandItemDisplay="Top" CommandItemSettings-AddNewRecordText="Add New User Details">
                                        <PagerStyle Mode="NextPrevNumericAndAdvanced" />
                                        <Columns>
                                            <telerik:GridEditCommandColumn ButtonType="ImageButton">
                                                <HeaderStyle Width="5%" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridEditCommandColumn>
                                            <telerik:GridBoundColumn DataField="ID" DataType="System.Int64" HeaderText="ID" ReadOnly="True"
                                                SortExpression="ID" UniqueName="ID" AllowFiltering="false" AllowSorting="false"
                                                Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="UserID" DataType="System.String" HeaderText="Login ID"
                                                SortExpression="UserID" UniqueName="UserID">
                                                <HeaderStyle Width="10%" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Password" DataType="System.String" HeaderText="Password"
                                                SortExpression="Password" UniqueName="Password">
                                                <HeaderStyle Width="10%" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="StaffName" DataType="System.String" HeaderText="Staff Name"
                                                SortExpression="StaffName" UniqueName="StaffName">
                                                <HeaderStyle Width="20%" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Designation" DataType="System.String" HeaderText="Designation"
                                                SortExpression="Designation" UniqueName="Designation">
                                                <HeaderStyle Width="10%" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="DeptName" DataType="System.String" HeaderText="Department Name"
                                                SortExpression="DeptName" UniqueName="DeptName">
                                                <HeaderStyle Width="10%" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="CompanyName" DataType="System.String" HeaderText="Company Name"
                                                SortExpression="CompanyName" UniqueName="CompanyName">
                                                <HeaderStyle Width="20%" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Location" DataType="System.String" HeaderText="Location"
                                                SortExpression="Location" UniqueName="Location">
                                                <HeaderStyle Width="10%" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridButtonColumn CommandName="Delete" UniqueName="DeleteColumn" ButtonType="ImageButton"
                                                ConfirmText="Are you sure want to delete?">
                                                <HeaderStyle Width="5%" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridButtonColumn>
                                        </Columns>
                                        <EditFormSettings EditFormType="Template">
                                            <EditColumn UniqueName="EditCommandColumn1">
                                            </EditColumn>
                                            <FormTemplate>
                                                <table cellspacing="4" cellpadding="4" width="100%" border="0">
                                                    <tr>
                                                        <td colspan="2" align="left">
                                                            <b>ID:
                                                                <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID")%>' />
                                                            </b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 10%" align="left">
                                                            UserID:
                                                            <asp:Label ID="Label11" Text="*" runat="server" Style="color: Red; font-size: smaller;
                                                                width: 1px;" />
                                                        </td>
                                                        <td style="width: 10%" align="left">
                                                            <asp:TextBox Width="200px" ID="txtUserID" MaxLength="15" ToolTip="Maximum Length: 15"
                                                                runat="server" Text='<%# Bind("UserID") %>' />
                                                            <asp:Label ID="lblUserId" runat="server" Visible="false" Text='<%# Bind("UserID")%>' />
                                                        </td>
                                                        <td align="left">
                                                            Password:
                                                            <asp:Label ID="Label1" Text="*" runat="server" Style="color: Red; font-size: smaller;
                                                                width: 1px;" />
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox Width="200px" ID="txtPassword" MaxLength="15" ToolTip="Maximum Length: 10"
                                                                runat="server" Text='<%# Bind("Password") %>' />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            Category:
                                                            <asp:Label ID="Label2" Text="*" runat="server" Style="color: Red; font-size: smaller;
                                                                width: 1px;" />
                                                        </td>
                                                        <td align="left">
                                                            <telerik:RadComboBox ID="cboCategory" runat="server" Height="60px" Width="200px"
                                                                AppendDataBoundItems="True" Text='<%# Bind("Category") %>'>
                                                                <Items>
                                                                    <telerik:RadComboBoxItem Text="--Select--" Value="--Select--" ForeColor="Silver" />
                                                                    <telerik:RadComboBoxItem Text="Standard" Value="Standard" />
                                                                    <telerik:RadComboBoxItem Text="Administrator" Value="Administrator" />
                                                                </Items>
                                                            </telerik:RadComboBox>
                                                        </td>
                                                        <td align="left">
                                                            Staff Name:
                                                            <asp:Label ID="Label3" Text="*" runat="server" Style="color: Red; font-size: smaller;
                                                                width: 1px;" />
                                                        </td>
                                                        <td align="left">
                                                            <telerik:RadComboBox ID="cboStaff" runat="server" Filter="StartsWith" Height="300px" DropDownWidth="250px"
                                                                Width="200px" DataSourceID="SqlDataSourceStaff" DataTextField="staffname" DataValueField="staffid"
                                                                AppendDataBoundItems="True"  OnSelectedIndexChanged="OnSelectedIndexWarehouse"
                                                                Text='<%# Bind("staffid") %>'>
                                                                <Items>
                                                                    <telerik:RadComboBoxItem Text="--Select--" Value="--Select--" ForeColor="Silver" />
                                                                </Items>
                                                            </telerik:RadComboBox>
                                                            <asp:Label ID="lblsname" Visible="false" runat="server" Text='<%# Bind("staffname") %>' />

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            Company:
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox Width="200px" ID="txtcompany" Enabled="false" runat="server" Text='<%# Bind("companyname") %>' />
                                                        </td>
                                                        <td align="left">
                                                            Warehouse:
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox Width="200px" ID="txtwarehouse" Enabled="false" runat="server" Text='<%# Bind("warehousename") %>' />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table cellspacing="4" cellpadding="4" width="100%" border="0">
                                                    <tr>
                                                        <td align="left">
                                                            Permission
                                                            <asp:Label ID="Label4" Text="*" runat="server" Style="color: Red; font-size: smaller;
                                                                width: 1px;" />
                                                        </td>
                                                        <td align="left">
                                                            <asp:CheckBox Checked="false" ID="chkDept" runat="server" Text=" Department" />
                                                            &nbsp; &nbsp;
                                                            <asp:CheckBox Checked="false" ID="chkCompany" runat="server" Text=" Company" />
                                                            &nbsp; &nbsp;
                                                            <asp:CheckBox Checked="false" ID="chkSupplier" runat="server" Text=" Supplier" />
                                                            &nbsp; &nbsp;
                                                            <asp:CheckBox Checked="false" ID="chkProduct" runat="server" Text=" Product" />
                                                            &nbsp; &nbsp;
                                                            <asp:CheckBox Checked="false" ID="chkStaff" runat="server" Text=" Staff" />
                                                            &nbsp; &nbsp;
                                                            <asp:CheckBox Checked="false" ID="chkUser" runat="server" Text=" User" />
                                                            &nbsp; &nbsp;
                                                            <asp:CheckBox Checked="false" ID="chkIncoming" runat="server" Text=" Incoming" />
                                                            &nbsp; &nbsp;
                                                            <asp:CheckBox Checked="false" ID="chkIssuing" runat="server" Text=" Issuing" />
                                                            &nbsp; &nbsp;
                                                            <asp:CheckBox Checked="false" ID="chkReport" runat="server" Text=" Report" />
                                                            &nbsp; &nbsp;
                                                            <asp:CheckBox Checked="false" ID="ChkWarehouse" runat="server" Text="Warehouse" />
                                                        </td>
                                                    </tr>
                                                    <tr style="visibility: hidden">
                                                        <td align="left">
                                                            CompanyID:
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox Width="200px" ID="txtcompanyid" MaxLength="10" ToolTip="Maximum Length: 10"
                                                                runat="server" />
                                                        </td>
                                                        <td align="left">
                                                            WarehouseID:
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox Width="200px" ID="txtwarehouseID" MaxLength="10" ToolTip="Maximum Length: 10"
                                                                runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="left">
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
                                <asp:SqlDataSource ID="SqlDataSourceStaff" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                    SelectCommand="SELECT [StaffId], [StaffName] FROM [StaffMaster] where deleted=0 ORDER BY [StaffName]">
                                </asp:SqlDataSource>
                                <asp:SqlDataSource ID="SqlDataSourceCompany" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                    SelectCommand="SELECT [companyid], [companyname] FROM [company] where deleted=0 ORDER BY [companyid]">
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
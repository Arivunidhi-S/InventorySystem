<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Warehouse.aspx.cs" Inherits="Warehouse" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<link rel="shortcut icon" href="Images/inventory.png" />
    <title>Ware House</title>
    <telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server" />
    <link rel="stylesheet" href="css/menu_core.css" type="text/css" />
    <link rel="stylesheet" href="css/modal.css" type="text/css" />
    <link rel="stylesheet" href="css/styles_green.css" type="text/css" />
    <link rel="stylesheet" href="css/skins/menu_simpleAnimated.css" type="text/css" />
    <script type="text/javascript">
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
                                            <telerik:AjaxUpdatedControl ControlID="txtPhone" />
                                        </UpdatedControls>
                                    </telerik:AjaxSetting>
                                </AjaxSettings>
                            </telerik:RadAjaxManager>
                            <telerik:RadInputManager ID="RadInputManager1" runat="server">
                                <telerik:TextBoxSetting BehaviorID="TextBoxBehavior1" InitializeOnClient="false"
                                    Validation-IsRequired="true" ErrorMessage="Mandatory Fields">
                                    <TargetControls>
                                        <telerik:TargetInput ControlID="txtName" />
                                        <telerik:TargetInput ControlID="txtAddress1" />
                                        <telerik:TargetInput ControlID="cbocompany" />
                                    </TargetControls>
                                </telerik:TextBoxSetting>
                                <telerik:RegExpTextBoxSetting BehaviorID="RagExpBehavior1" Validation-IsRequired="true"
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
                                </telerik:RegExpTextBoxSetting>
                                <telerik:RegExpTextBoxSetting BehaviorID="RagExpBehavior4" ValidationExpression="[0-9]{6,}"
                                    ErrorMessage="Enter more than 6 figures">
                                    <TargetControls>
                                        <telerik:TargetInput ControlID="txtFax" />
                                    </TargetControls>
                                </telerik:RegExpTextBoxSetting>
                            </telerik:RadInputManager>
                           <div class="mycss">
                                WAREHOUSE DETAILS
                            </div>
                             <br />
                            <div id="Div10" runat="server" style="width: 1250px;
                                overflow: auto;" align="center">
                                <telerik:RadGrid ID="RadGrid1" runat="server" AllowMultiRowEdit="false" Skin="Hay"
                                    OnNeedDataSource="RadGrid1_NeedDataSource" GridLines="Vertical" AllowPaging="True"
                                    PageSize="10" OnItemCreated="RadGrid1_ItemCreated" PagerStyle-AlwaysVisible="true"
                                    PagerStyle-Position="Bottom" OnDeleteCommand="RadGrid1_DeleteCommand" AllowAutomaticUpdates="false"
                                    AllowAutomaticInserts="false" PagerStyle-Mode="NextPrevNumericAndAdvanced" AllowAutomaticDeletes="false"
                                    OnInsertCommand="RadGrid1_InsertCommand" AllowSorting="false" AllowFilteringByColumn="true"
                                    OnItemDataBound="RadGrid1_ItemDataBound" OnUpdateCommand="RadGrid1_UpdateCommand">
                                    <ClientSettings AllowDragToGroup="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                                        EnableRowHoverStyle="true" EnablePostBackOnRowClick="false">
                                        <Resizing AllowColumnResize="true" />
                                        <Selecting AllowRowSelect="true" />
                                    </ClientSettings>
                                    <MasterTableView AutoGenerateColumns="false" InsertItemPageIndexAction="ShowItemOnFirstPage"
                                        DataKeyNames="warehouseid" CommandItemDisplay="Top" CommandItemSettings-AddNewRecordText="Add New Warehouse Details">
                                        <PagerStyle Mode="NextPrevNumericAndAdvanced" />
                                        <Columns>
                                            <telerik:GridEditCommandColumn ButtonType="ImageButton">
                                                <HeaderStyle Width="3%" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridEditCommandColumn>
                                            <telerik:GridBoundColumn DataField="warehouseid" DataType="System.Int64" HeaderText="ID"
                                                ReadOnly="True" SortExpression="warehouseid" UniqueName="warehouseid" AllowFiltering="false"
                                                AllowSorting="false" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Companyname" DataType="System.String" HeaderText="Company Name"
                                                SortExpression="CompanyName" UniqueName="CompanyName">
                                                <HeaderStyle Width="15%" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="warehouseName" DataType="System.String" HeaderText="Warehouse Name"
                                                SortExpression="warehouseName" UniqueName="warehouseName">
                                                <HeaderStyle Width="15%" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Address1" DataType="System.String" HeaderText="Address"
                                                SortExpression="Address1" UniqueName="Address1">
                                                <HeaderStyle Width="10%" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="City" DataType="System.String" HeaderText="City"
                                                SortExpression="City" UniqueName="City">
                                                <HeaderStyle Width="10%" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="State" DataType="System.String" HeaderText="State"
                                                SortExpression="State" UniqueName="State">
                                                <HeaderStyle Width="10%" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Contactno" DataType="System.String" HeaderText="Phone"
                                                SortExpression="Contactno" UniqueName="Contactno">
                                                <HeaderStyle Width="10%" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Email" DataType="System.String" HeaderText="Email"
                                                SortExpression="Email" UniqueName="Email">
                                                <HeaderStyle Width="15%" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Website" DataType="System.String" HeaderText="Website"
                                                SortExpression="Website" UniqueName="Website">
                                                <HeaderStyle Width="15%" />
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
                                                <table cellspacing="3" cellpadding="3" width="100%" border="0">
                                                    <tr>
                                                        <td colspan="2" align="left">
                                                            <b>ID:
                                                                <asp:Label ID="lblID" runat="server" Text='<%# Bind("warehouseId")%>' />
                                                            </b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <b>Company:
                                                                <asp:Label ID="Label2" Text="*" runat="server" Style="color: Red; font-size: smaller;
                                                                    width: 1px;" /></b>
                                                        </td>
                                                        <td align="left">
                                                            <telerik:RadComboBox ID="cbocompany" runat="server" Height="300px" Width="200px" DataSourceID="SqlDatacompany"
                                                                DataTextField="companyname" DataValueField="companyid" DropDownWidth="300px"
                                                                AppendDataBoundItems="True" Text='<%# Bind("companyname") %>'>
                                                                <Items>
                                                                    <telerik:RadComboBoxItem Text="--Select--" Value="--Select--" ForeColor="Silver" />
                                                                </Items>
                                                            </telerik:RadComboBox>
                                                            </b>
                                                            <asp:Label ID="Lblcmpydummy" Visible="false" Text='<%# Bind("companyname") %>' runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <b>Warehouse Name:
                                                                <asp:Label ID="Label11" Text="*" runat="server" Style="color: Red; font-size: smaller;
                                                                    width: 1px;" /></b>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox Width="200px" ID="txtName" MaxLength="100" ToolTip="Maximum Length: 100"
                                                                runat="server" Text='<%# Bind("warehousename") %>' />
                                                        </td>
                                                        <td align="left">
                                                            City:
                                                        </td>
                                                        <td align="left">
                                                            <telerik:RadTextBox Width="200px" ID="txtCity" MaxLength="30" ToolTip="Maximum Length: 30"
                                                                runat="server" Text='<%# Bind("City") %>' />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Lblwrhname" Visible="false" Text='<%# Bind("warehousename") %>' runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <b>Address 1:
                                                                <asp:Label ID="Label1" Text="*" runat="server" Style="color: Red; font-size: smaller;
                                                                    width: 1px;" /></b>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox Width="200px" ID="txtAddress1" MaxLength="150" ToolTip="Maximum Length: 150"
                                                                runat="server" Text='<%# Bind("Address1") %>' />
                                                        </td>
                                                        <td align="left">
                                                            Address 2:
                                                        </td>
                                                        <td align="left">
                                                            <telerik:RadTextBox Width="200px" ID="txtAddress2" MaxLength="150" ToolTip="Maximum Length: 150"
                                                                runat="server" Text='<%# Bind("Address2") %>' />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            State:
                                                        </td>
                                                        <td align="left">
                                                            <telerik:RadTextBox Width="200px" ID="txtState" MaxLength="30" ToolTip="Maximum Length: 30"
                                                                runat="server" Text='<%# Bind("State") %>' />
                                                        </td>
                                                        <td align="left">
                                                            Country:
                                                        </td>
                                                        <td align="left">
                                                            <telerik:RadTextBox Width="200px" ID="txtCountry" MaxLength="30" ToolTip="Maximum Length: 30"
                                                                runat="server" Text='<%# Bind("Country") %>' />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            Phone:
                                                        </td>
                                                        <td align="left">
                                                            <telerik:RadMaskedTextBox Width="200px" ID="txtPhone" SelectionOnFocus="CaretToBeginning"
                                                                PromptChar="_" Mask="###-############" MaxLength="20" ToolTip="Maximum Length: 15"
                                                                runat="server" Text='<%# Bind("Contactno") %>' />
                                                        </td>
                                                        <td align="left">
                                                            Postcode:
                                                        </td>
                                                        <td align="left">
                                                            <telerik:RadMaskedTextBox Width="200px" ID="txtPostcode" SelectionOnFocus="CaretToBeginning"
                                                                PromptChar="_" Mask="######" MaxLength="6" ToolTip="Maximum Length:6" runat="server"
                                                                Text='<%# Bind("Postcode") %>' />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            Fax:
                                                        </td>
                                                        <td align="left">
                                                            <telerik:RadMaskedTextBox Width="200px" ID="txtFax" SelectionOnFocus="CaretToBeginning"
                                                                PromptChar="_" Mask="###-############" MaxLength="20" ToolTip="Maximum Length: 15"
                                                                runat="server" Text='<%# Bind("Faxno") %>' />
                                                        </td>
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
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            Website:
                                                        </td>
                                                        <td align="left">
                                                            <telerik:RadTextBox Width="200px" ID="txtWebsite" MaxLength="50" ToolTip="Maximum Length: 50"
                                                                runat="server" Text='<%# Bind("Website") %>' />
                                                        </td>
                                                        <td align="left">
                                                            Description:
                                                        </td>
                                                        <td align="left">
                                                            <telerik:RadTextBox Width="200px" ID="txtDesc" TextMode="MultiLine" MaxLength="200"
                                                                ToolTip="Maximum Length: 200" runat="server" Text='<%# Bind("Description") %>' />
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
                                <asp:SqlDataSource ID="SqlDatacompany" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                    SelectCommand="SELECT  [companyid], [companyname] FROM [company] where deleted=0 ORDER BY [companyid]">
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

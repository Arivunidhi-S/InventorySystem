<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Incoming.aspx.cs" Inherits="Incoming" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<link rel="shortcut icon" href="Images/inventory.png" />
    <title>Incoming</title>
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
                                <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
                                <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
                                    <AjaxSettings>
                                        <telerik:AjaxSetting AjaxControlID="RadGrid1">
                                            <UpdatedControls>
                                                <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                                                <telerik:AjaxUpdatedControl ControlID="RadInputManager1" />
                                                <telerik:AjaxUpdatedControl ControlID="lblStatus" />
                                                <telerik:AjaxUpdatedControl ControlID="RadToolTipManager1" />
                                                <telerik:AjaxUpdatedControl ControlID="cboProductCode" />
                                                <telerik:AjaxUpdatedControl ControlID="RadAjaxLoadingPanel1" />
                                            </UpdatedControls>
                                        </telerik:AjaxSetting>
                                    </AjaxSettings>
                                </telerik:RadAjaxManager>
                                <telerik:RadInputManager ID="RadInputManager1" runat="server">
                                    <telerik:TextBoxSetting BehaviorID="TextBoxBehavior1" InitializeOnClient="false"
                                        Validation-IsRequired="true" ErrorMessage="Mandatory Fields">
                                        <TargetControls>
                                            <%--<telerik:TargetInput ControlID="txtquantity" />--%>
                                        </TargetControls>
                                    </telerik:TextBoxSetting>
                                </telerik:RadInputManager>
                                <telerik:RadToolTipManager ID="RadToolTipManager1" OffsetY="-1" HideEvent="ManualClose"
                                    Width="300" Height="305" runat="server" RelativeTo="Element" Position="MiddleRight">
                                </telerik:RadToolTipManager>
                                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
                                <div class="mycss">
                                    INCOMING DETAILS
                                </div>
                                <br />
                                <div id="Div10" runat="server" style="width: 1250px;
                                overflow: auto;" align="center">
                                    <telerik:RadGrid ID="RadGrid1" runat="server" Width="99.8%" AllowMultiRowEdit="false" Skin="Hay"
                                        OnNeedDataSource="RadGrid1_NeedDataSource" GridLines="Vertical" AllowPaging="True" PageSize="15"
                                        OnItemCreated="RadGrid1_ItemCreated" PagerStyle-AlwaysVisible="true" PagerStyle-Position="Bottom"
                                        OnItemDataBound="RadGrid1_ItemDataBound" OnDeleteCommand="RadGrid1_DeleteCommand"
                                        AllowAutomaticUpdates="false" AllowAutomaticInserts="false"   PagerStyle-Mode="NextPrevNumericAndAdvanced"
                                        AllowAutomaticDeletes="false" OnInsertCommand="RadGrid1_InsertCommand" AllowSorting="false"
                                        AllowFilteringByColumn="true" OnUpdateCommand="RadGrid1_UpdateCommand" OnItemCommand="RadGrid1_ItemCommand">
                                        <ClientSettings AllowDragToGroup="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                                            EnableRowHoverStyle="true" EnablePostBackOnRowClick="false">
                                            <Resizing AllowColumnResize="true" />
                                            <Selecting AllowRowSelect="true" />
                                        </ClientSettings>
                                        <MasterTableView AutoGenerateColumns="false" DataKeyNames="ID" CommandItemDisplay="Top"
                                            CommandItemSettings-AddNewRecordText="Add New Incoming Details" InsertItemPageIndexAction="ShowItemOnFirstPage">
                                            <PagerStyle Mode="NextPrevNumericAndAdvanced" />
                                            <Columns>
                                                <telerik:GridEditCommandColumn ButtonType="ImageButton">
                                                    <HeaderStyle Width="3%" />
                                                </telerik:GridEditCommandColumn>
                                                <telerik:GridBoundColumn DataField="ID" DataType="System.Int64" HeaderText="ID" ReadOnly="True"
                                                    SortExpression="ID" UniqueName="ID" AllowFiltering="false" AllowSorting="false"
                                                    Visible="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="ProductCode" DataType="System.String" HeaderText="Product Code"
                                                    SortExpression="ProductCode" UniqueName="ProductCode" FilterControlWidth="60px">
                                                    <HeaderStyle Width="7%" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="ProductName" DataType="System.String" HeaderText="Product Name" 
                                                    SortExpression="ProductName" UniqueName="ProductName" FilterControlWidth="250px">
                                                    <HeaderStyle Width="20%" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="suppliername" DataType="System.String" HeaderText="Supplier Name"
                                                    SortExpression="suppliername" UniqueName="suppliername" FilterControlWidth="170px">
                                                    <HeaderStyle Width="15%"  HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="incomingdate" DataType="System.DateTime" HeaderText="Incoming Date"
                                                    SortExpression="incomingdate" UniqueName="incomingdate" DataFormatString="{0:dd/MMM/yyyy}" FilterControlWidth="150px">
                                                    <HeaderStyle Width="15%"  HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="expireddate" DataType="System.DateTime" HeaderText="Expired Date"
                                                    SortExpression="expireddate" UniqueName="expireddate" DataFormatString="{0:dd/MMM/yyyy}" FilterControlWidth="150px">
                                                    <HeaderStyle Width="15%"  HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="quantity" DataType="System.String" HeaderText="Quantity"
                                                    SortExpression="quantity" UniqueName="quantity" FilterControlWidth="30px">
                                                    <HeaderStyle Width="5%"  HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridButtonColumn CommandName="Delete" UniqueName="DeleteColumn" ButtonType="ImageButton"
                                                    ConfirmText="Are you sure want to delete?">
                                                    <HeaderStyle Width="3%"  HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridButtonColumn>
                                            </Columns>
                                            <EditFormSettings EditFormType="Template">
                                                <EditColumn UniqueName="EditCommandColumn1">
                                                </EditColumn>
                                                <FormTemplate>
                                                    <table cellspacing="5" cellpadding="5" width="100%" border="0">
                                                        <tr>
                                                            <td colspan="2" align="left">
                                                                <b>ID:
                                                                    <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID")%>' />
                                                                </b>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 15%" align="left">
                                                                <b>Product Code:
                                                                    <asp:Label ID="Label11" Text="*" runat="server" Style="color: Red; font-size: smaller;
                                                                        width: 1px;" /></b>
                                                            </td>
                                                             <td style="width: 85%" align="left">
                                                                <telerik:RadComboBox ID="cboProductCode" runat="server" MarkFirstMatch="true" Filter="StartsWith"
                                                                    Height="300px" Width="200px" DataValueField="id" DataTextField="productname" OnItemsRequested="Productcode_OnItemsRequested"
                                                                    OnSelectedIndexChanged="OnSelectedIndexChangedHandler" Text='<%# Bind("productname") %>'
                                                                    EnableLoadOnDemand="true" AutoPostBack="true" AppendDataBoundItems="True" DropDownWidth="300px"
                                                                    EmptyMessage="--Select--">
                                                                    <HeaderTemplate>
                                                                        <table style="width: 200px; font-weight: bold" cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td style="width: 100px;">
                                                                                    Product Code
                                                                                </td>
                                                                                <td style="width: 100px;">
                                                                                    Product Name
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <table style="width: 200px" cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td style="width: 100px;" align="left">
                                                                                    <%# DataBinder.Eval(Container, "Attributes['productcode']")%>
                                                                                </td>
                                                                                <td style="width: 100px;" align="left">
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
                                                                <b>Supplier Name:
                                                                    <asp:Label ID="Label4" Text="*" runat="server" Style="color: Red; font-size: smaller;
                                                                        width: 1px;" /></b>
                                                            </td>
                                                            <td align="left">
                                                                <telerik:RadComboBox ID="cbosuppliername" runat="server" Height="300px" Width="200px"
                                                                    DataSourceID="SqlDataSourcesname" DataTextField="suppliername" DropDownWidth="300px"
                                                                    DataValueField="supplierid" AutoPostBack="true" OnSelectedIndexChanged="OnSelectedIndexSupplierdummy"
                                                                    AppendDataBoundItems="True" Text='<%# Bind("suppliername") %>'>
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Text="--Select--" Value="--Select--" ForeColor="Silver" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                                <asp:Label ID="Lblsupperr" Text="" runat="server" Visible="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                Unit Price (RM):
                                                            </td>
                                                            <td align="left">
                                                                <telerik:RadNumericTextBox ID="txtunitprice" MinValue="0" MaxValue="999999999999999999999999"
                                                                    AutoPostBack="true" OnTextChanged="txtquantity_TextChanged" Text='<%# Bind("unitprice") %>'
                                                                    MaxLength="20" Width="200px" ToolTip="Maximum Length: 10" Type="Number" runat="server">
                                                                    <NumberFormat GroupSeparator="" DecimalDigits="2" />
                                                                </telerik:RadNumericTextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <b>Quantity:
                                                                    <asp:Label ID="Label3" Text="*" runat="server" Style="color: Red; font-size: smaller;
                                                                        width: 1px;" />
                                                                </b>
                                                            </td>
                                                            <td align="left">
                                                                <telerik:RadNumericTextBox ID="txtquantity" AutoPostBack="true" OnTextChanged="txtquantity_TextChanged"
                                                                    Text='<%# Bind("quantity") %>' EmptyMessage="Enter The Quantity" MaxLength="20"
                                                                    MinValue="0" MaxValue="999999999999999999999999" Width="200px" ToolTip="Maximum Length: 10"
                                                                    Type="Number" runat="server">
                                                                    <NumberFormat GroupSeparator="" DecimalDigits="0" />
                                                                </telerik:RadNumericTextBox>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblqerr" runat="server" BackColor="Beige" Visible="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                Amount (RM):
                                                            </td>
                                                            <td align="left">
                                                                <telerik:RadNumericTextBox ID="txtamount" Text='<%# Bind("amount") %>' MaxLength="10"
                                                                    Width="200px" ToolTip="Maximum Length: 60" MinValue="0" MaxValue="999999999999999999999999"
                                                                    Type="Currency" ReadOnly="true" Enabled="false" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                Delivery Order No:
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox Width="200px" ID="txtdono" MaxLength="150" ToolTip="Maximum Length: 150"
                                                                    runat="server" Text='<%# Bind("dono") %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <b>Incoming Date:
                                                                    <asp:Label ID="Label7" Text="*" runat="server" Style="color: Red; font-size: smaller;
                                                                        width: 1px;" /></b>
                                                            </td>
                                                            <td align="left">
                                                                <telerik:RadDateTimePicker ID="txtincomingdate" runat="server" Width="200px" DateInput-EmptyMessage="Select Incoming Date"
                                                                    AutoPostBack="true" MinDate="01/01/1000" MaxDate="01/01/3000" DateInput-DateFormat="d/MMM/yyyy hh:mm tt"
                                                                    OnSelectedDateChanged="IncomingSelectDate_SelectedDateChanged" PopupDirection="BottomRight"
                                                                    EnableTheming="true" DbSelectedDate='<%# Bind("incomingdate") %>'>
                                                                    <Calendar ID="Calendar1" runat="server" ShowRowHeaders="true">
                                                                        <SpecialDays>
                                                                            <telerik:RadCalendarDay Repeatable="Today" />
                                                                        </SpecialDays>
                                                                    </Calendar>
                                                                    
                                                                </telerik:RadDateTimePicker>
                                                                <asp:Label ID="lblexprdate" Text='<%# Bind("expireddate") %>' runat="server" Visible="false" />
                                                                <asp:Label ID="Lblindateerr" Text="" runat="server" Visible="true" />
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
                                    <asp:SqlDataSource ID="SqlDataSourcepcode" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                        SelectCommand="SELECT [id], [productcode],[productname] FROM [products] where deleted=0 ORDER BY [productcode]">
                                    </asp:SqlDataSource>
                                    <asp:SqlDataSource ID="SqlDataSourcesname" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                        SelectCommand="SELECT [supplierid], [suppliername]  FROM [supplier] where deleted=0 ORDER BY [supplierid]">
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

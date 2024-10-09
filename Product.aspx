<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Product.aspx.cs" Inherits="Product" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Products</title>
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
                                                <telerik:AjaxUpdatedControl ControlID="RadGridProduct" />
                                                <telerik:AjaxUpdatedControl ControlID="RadInputManager1" />
                                                <telerik:AjaxUpdatedControl ControlID="lblStatus" />
                                            </UpdatedControls>
                                        </telerik:AjaxSetting>
                                    </AjaxSettings>
                                </telerik:RadAjaxManager>
                                <telerik:RadInputManager ID="RadInputManager1" runat="server">
                                    <telerik:TextBoxSetting BehaviorID="TextBoxBehavior1" InitializeOnClient="false"
                                        Validation-IsRequired="true" ErrorMessage="Mandatory Fields">
                                        <TargetControls>
                                            <telerik:TargetInput ControlID="txtProductCode" />
                                            <telerik:TargetInput ControlID="txtProductName" />
                                            <telerik:TargetInput ControlID="txtunitprice" />
                                            <telerik:TargetInput ControlID="txtdurationtime" />
                                            <telerik:TargetInput ControlID="txtminstock" />
                                        </TargetControls>
                                    </telerik:TextBoxSetting>
                                </telerik:RadInputManager>
                              <div class="mycss">
                                    PRODUCT MASTER DETAILS
                                </div>
                                 <br />
                                <div id="Div10" runat="server" style="width: 1250px; overflow: auto;" align="center">
                                    <telerik:RadGrid ID="RadGridProduct" runat="server" AllowMultiRowEdit="false"
                                        Skin="Hay" OnNeedDataSource="RadGrid1_NeedDataSource" GridLines="Vertical"
                                        AllowPaging="True" OnItemDataBound="RadGrid1_ItemDataBound" OnItemCreated="RadGrid1_ItemCreated"
                                        PageSize="8" PagerStyle-AlwaysVisible="true" PagerStyle-Position="Bottom" OnDeleteCommand="RadGrid1_DeleteCommand"
                                        AllowAutomaticUpdates="false" AllowAutomaticInserts="false" PagerStyle-Mode="NextPrevNumericAndAdvanced"
                                        AllowAutomaticDeletes="false" OnInsertCommand="RadGrid1_InsertCommand" AllowSorting="false"
                                        AllowFilteringByColumn="true" OnUpdateCommand="RadGrid1_UpdateCommand">
                                        <ClientSettings AllowDragToGroup="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                                            EnableRowHoverStyle="true" EnablePostBackOnRowClick="false">
                                            <Resizing AllowColumnResize="true" />
                                            <Selecting AllowRowSelect="true" />
                                        </ClientSettings>
                                        <MasterTableView AutoGenerateColumns="false" DataKeyNames="ID" CommandItemDisplay="Top"
                                            CommandItemSettings-AddNewRecordText="Add New Product Details" InsertItemPageIndexAction="ShowItemOnFirstPage">
                                            <PagerStyle Mode="NextPrevNumericAndAdvanced" />
                                            <Columns>
                                                <telerik:GridEditCommandColumn ButtonType="ImageButton">
                                                    <HeaderStyle Width="5%" />
                                                </telerik:GridEditCommandColumn>
                                                <telerik:GridBoundColumn DataField="ID" DataType="System.Int64" HeaderText="ID" ReadOnly="True"
                                                    SortExpression="ID" UniqueName="ID" AllowFiltering="false" AllowSorting="false"
                                                    Visible="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="warehousename" DataType="System.String" HeaderText="Warehouse Name">
                                                    <HeaderStyle Width="20%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="ProductCode" DataType="System.String" HeaderText="Product Code"
                                                    SortExpression="ProductCode" UniqueName="ProductCode">
                                                    <HeaderStyle Width="20%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="ProductName" DataType="System.String" HeaderText="Product Name"
                                                    SortExpression="ProductName" UniqueName="ProductName">
                                                    <HeaderStyle Width="30%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn HeaderText="Validity Period">
                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="20%" />
                                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Left" ForeColor="Black" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblmaxEu" runat="server" ForeColor="Black" Text='<%# Bind("durationtime")%>' />
                                                        <asp:Label ID="Label7" runat="server" ForeColor="Black" Text='<%# Bind("durationperiod")%>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
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
                                                    <table cellspacing="5" cellpadding="5" width="100%" border="0">
                                                        <tr>
                                                            <td colspan="2" align="left">
                                                                <b>
                                                                    <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID")%>' />
                                                                </b>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Lblpcode" Visible="false" runat="server" Text='<%# Bind("productcode")%>' />
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Lblpname" Visible="false" runat="server" Text='<%# Bind("productname")%>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 15%" align="left">
                                                                Product Code:
                                                                <asp:Label ID="Label11" Text="*" runat="server" Style="color: Red; font-size: smaller;
                                                                    width: 1px;" />
                                                            </td>
                                                            <td style="width: 85%" align="left">
                                                                <asp:TextBox Width="200px" ID="txtProductCode" MaxLength="30" ToolTip="Maximum Length: 10"
                                                                    runat="server" Text='<%# Bind("productcode") %>' />
                                                                <%--<asp:RequiredFieldValidator ID="RFpcode" runat="server" ForeColor="Red" ControlToValidate="txtProductCode"
                                                                    ErrorMessage="Mandatory" SetFocusOnError="True" />--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                Product Name:
                                                                <asp:Label ID="Label1" Text="*" runat="server" Style="color: Red; font-size: smaller;
                                                                    width: 1px;" />
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox Width="200px" ID="txtProductName" MaxLength="150" ToolTip="Maximum Length: 150"
                                                                    runat="server" Text='<%# Bind("productname") %>' />
                                                                <%-- <asp:RequiredFieldValidator ID="RFPname" runat="server" ForeColor="Red" ControlToValidate="txtProductName"
                                                                    ErrorMessage="Mandatory" SetFocusOnError="True" />--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                Unit of Measure:
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox Width="200px" ID="txtunit" MaxLength="10" ToolTip="Maximum Length: 10"
                                                                    runat="server" Text='<%# Bind("unit") %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                Unit Price:
                                                                <asp:Label ID="Label3" Text="*" runat="server" Style="color: Red; font-size: smaller;
                                                                    width: 1px;" />
                                                            </td>
                                                            <td align="left">
                                                                <telerik:RadNumericTextBox ID="txtunitprice" AutoPostBack="false" Text='<%# Bind("unitprice") %>'
                                                                    MaxLength="20" Width="200px" MinValue="0" MaxValue="9999999999999999999999999"
                                                                    ToolTip="Maximum Length: 10" Type="Number" runat="server">
                                                                    <NumberFormat GroupSeparator="" DecimalDigits="2" />
                                                                </telerik:RadNumericTextBox>
                                                                <asp:RequiredFieldValidator ID="RFup" runat="server" ForeColor="Red" ControlToValidate="txtunitprice"
                                                                    ErrorMessage="Mandatory" SetFocusOnError="True" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                Validity Period/Duration:
                                                                <asp:Label ID="Label4" Text="*" runat="server" Style="color: Red; font-size: smaller;
                                                                    width: 1px;" />
                                                            </td>
                                                            <td align="left">
                                                                <telerik:RadNumericTextBox Width="200px" MinValue="0" MaxValue="9999999999999999999999999"
                                                                    DbValue='<%# Bind("durationtime") %>' MaxLength="10" ToolTip="Maximum Length: 20"
                                                                    ID="txtdurationtime" runat="server">
                                                                    <NumberFormat GroupSeparator="" DecimalDigits="0" />
                                                                </telerik:RadNumericTextBox>
                                                                <telerik:RadComboBox ID="cboduration" runat="server" DropDownWidth="100px" Height="80px"
                                                                    Width="80px" AppendDataBoundItems="True" Text='<%# Bind("durationperiod") %>'>
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Text="--Select--" Value="--Select--" ForeColor="Silver" />
                                                                        <telerik:RadComboBoxItem Text="Years" Value="Years" />
                                                                        <telerik:RadComboBoxItem Text="Months" Value="Months" />
                                                                        <telerik:RadComboBoxItem Text="Days" Value="Days" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                                <asp:RequiredFieldValidator ID="RFdp" runat="server" ForeColor="Red" ControlToValidate="txtdurationtime"
                                                                    ErrorMessage="Mandatory" SetFocusOnError="True" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                Minimum Stock:
                                                                <asp:Label ID="Label5" Text="*" runat="server" Style="color: Red; font-size: smaller;
                                                                    width: 1px;" />
                                                            </td>
                                                            <td align="left">
                                                                <telerik:RadNumericTextBox ID="txtminstock" Text='<%# Bind("minimumstock") %>' MaxLength="10"
                                                                    Width="200px" ToolTip="Maximum Length: 10" MinValue="0" MaxValue="999999999999"
                                                                    Type="Number" runat="server">
                                                                    <NumberFormat GroupSeparator="" DecimalDigits="1" />
                                                                </telerik:RadNumericTextBox>
                                                                <asp:RequiredFieldValidator ID="RFmin" runat="server" ForeColor="Red" ControlToValidate="txtminstock"
                                                                    ErrorMessage="Mandatory" SetFocusOnError="True" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                Warehouse:
                                                                <asp:Label ID="Label2" Text="*" runat="server" Style="color: Red; font-size: smaller;
                                                                    width: 1px;" />
                                                            </td>
                                                            <td align="left">
                                                                <telerik:RadComboBox ID="cbowarehouse" runat="server" Height="250px" Width="200px"
                                                                    DataSourceID="SqlDataSourceswarehouse" DropDownWidth="200px" DataTextField="warehousename"
                                                                    DataValueField="warehouseid" AppendDataBoundItems="True" Text='<%# Bind("warehouseid") %>'>
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Text="--Select--" Value="--Select--" ForeColor="Silver" />
                                                                        <%-- <telerik:RadComboBoxItem Text="ALL" Value="000" />--%>
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                                <%--<asp:RequiredFieldValidator ID="RFwname" runat="server" ForeColor="Red" ControlToValidate="cbowarehouse"
                                                                    ErrorMessage="Mandatory" SetFocusOnError="True" />--%>
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
                                    <asp:SqlDataSource ID="SqlDataSourceswarehouse" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                        SelectCommand="SELECT [warehouseid], [warehousename]  FROM [warehouse] where deleted=0 ORDER BY [warehouseid]">
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

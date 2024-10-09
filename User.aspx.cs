using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

using System.Globalization;
using System.Collections;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.OleDb;
using System.Drawing;

public partial class User : System.Web.UI.Page
{
    public DataTable dtMenuItems = new DataTable();
    public DataTable dtSubMenuItems = new DataTable();

    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            if (!(string.IsNullOrEmpty(Session["sesUserID"].ToString())))
            {
                SqlConnection connMenu = BusinessTier.getConnection();
                connMenu.Open();
                SqlDataReader readerMenu = BusinessTier.getMenuList(connMenu, Session["sesUserID"].ToString().Trim());
                dtMenuItems.Load(readerMenu);
                BusinessTier.DisposeReader(readerMenu);
                BusinessTier.DisposeConnection(connMenu);
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("Login.aspx");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(Session["sesUserID"].ToString()))
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                lblStatus.Text = "";
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("Login.aspx");
        }
    }

    protected void RadGrid1_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditFormItem editedItem = e.Item as GridEditFormItem;
                TextBox txtUserID = editedItem.FindControl("txtUserID") as TextBox;
                TextBox txtPassword = editedItem.FindControl("txtPassword") as TextBox;
                RadComboBox cboStaff = (RadComboBox)editedItem.FindControl("cboStaff");
                RadComboBox cboCategory = (RadComboBox)editedItem.FindControl("cboCategory");
            }
        }
        catch (Exception ex)
        {
            ShowMessage(8);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterUser", "RadGrid1_ItemCreated", ex.ToString(), "Audit");
        }

    }


    protected void RadGrid1_ItemDataBound(object source, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditFormItem editedItem = e.Item as GridEditFormItem;
                Label lblID = (Label)editedItem.FindControl("lblId");

                RadComboBox cboStaff = (RadComboBox)editedItem.FindControl("cboStaff");
                cboStaff.AutoPostBack = true;
                cboStaff.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(OnSelectedIndexWarehouse);
                RadComboBox cboCategory = (RadComboBox)editedItem.FindControl("cboCategory");
                TextBox txtcompany = (TextBox)editedItem.FindControl("txtcompany");
                TextBox txtwarehouse = (TextBox)editedItem.FindControl("txtwarehouse");
                CheckBox chkDept = (CheckBox)editedItem.FindControl("chkDept");
                CheckBox chkCompany = (CheckBox)editedItem.FindControl("chkCompany");
                CheckBox chkSupplier = (CheckBox)editedItem.FindControl("chkSupplier");
                CheckBox chkProduct = (CheckBox)editedItem.FindControl("chkProduct");
                CheckBox chkStaff = (CheckBox)editedItem.FindControl("chkStaff");
                CheckBox chkUser = (CheckBox)editedItem.FindControl("chkUser");
                CheckBox chkIncoming = (CheckBox)editedItem.FindControl("chkIncoming");
                CheckBox chkIssuing = (CheckBox)editedItem.FindControl("chkIssuing");
                CheckBox chkReport = (CheckBox)editedItem.FindControl("chkReport");
                CheckBox ChkWarehouse = (CheckBox)editedItem.FindControl("ChkWarehouse");

                if (!(string.IsNullOrEmpty(lblID.Text.ToString())))
                {
                    SqlConnection conn = BusinessTier.getConnection();
                    conn.Open();
                    SqlCommand command = new SqlCommand("select * from  VW_UserDataBound where deleted=0 and id= '" + lblID.Text.ToString() + "' order by createddate", conn);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        cboCategory.SelectedValue = reader["category"].ToString();
                        cboStaff.SelectedItem.Text = reader["staffname"].ToString();
                        txtcompany.Text = reader["companyname"].ToString();
                        txtwarehouse.Text = reader["warehousename"].ToString();
                    }
                    BusinessTier.DisposeReader(reader);

                    SqlCommand comd = new SqlCommand("select menuid from UserInfo_Permission where userid='" + lblID.Text.ToString() + "'", conn);
                    SqlDataReader reader1 = comd.ExecuteReader();
                    while (reader1.Read())
                    {
                        if (reader1["menuid"].ToString().Trim() == "1")
                            chkSupplier.Checked = true;
                        if (reader1["menuid"].ToString().Trim() == "2")
                            chkUser.Checked = true;
                        if (reader1["menuid"].ToString().Trim() == "3")
                            chkCompany.Checked = true;
                        if (reader1["menuid"].ToString().Trim() == "4")
                            chkDept.Checked = true;
                        if (reader1["menuid"].ToString().Trim() == "5")
                            chkStaff.Checked = true;
                        if (reader1["menuid"].ToString().Trim() == "6")
                            chkIncoming.Checked = true;
                        if (reader1["menuid"].ToString().Trim() == "7")
                            chkIssuing.Checked = true;
                        if (reader1["menuid"].ToString().Trim() == "8")
                            chkReport.Checked = true;
                        if (reader1["menuid"].ToString().Trim() == "9")
                            chkProduct.Checked = true;
                        if (reader1["menuid"].ToString().Trim() == "17")
                            ChkWarehouse.Checked = true;
                    }
                    BusinessTier.DisposeReader(reader1);

                    BusinessTier.DisposeConnection(conn);
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage(9);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterUser", "RadGrid1_ItemDataBound", ex.ToString(), "Audit");
        }
    }


    protected void RadGrid1_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid1.DataSource = DataSourceHelper();
        }
        catch (Exception ex)
        {
            ShowMessage(9);
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterUser", "NeedDataSource", ex.ToString(), "Audit");
        }
    }

    public DataTable DataSourceHelper()
    {
        SqlConnection conn = BusinessTier.getConnection();
        conn.Close();
        string sql = "select * FROM UserInfoStaffComDeptnew where deleted=0 order by createddate desc";
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql, conn);
        DataTable g_datatable = new DataTable();
        sqlDataAdapter.Fill(g_datatable);
        BusinessTier.DisposeAdapter(sqlDataAdapter);
        BusinessTier.DisposeConnection(conn);
        return g_datatable;
    }

    protected void RadGrid1_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            string ID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString();
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            int flg = BusinessTier.SaveUser(conn, "", "", 0, "", Convert.ToInt32(Session["sesUserID"].ToString()), "D", Convert.ToInt32(ID.ToString().Trim()), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            BusinessTier.DisposeConnection(conn);
            if (flg >= 1)
            {
                ShowMessage(15);
            }
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterUser", "Delete", "Success", "Log");
        }
        catch (Exception ex)
        {
            ShowMessage(6);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterUser", "Delete", ex.ToString(), "Audit");
        }
    }

    protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            TextBox txtUserID = (TextBox)editedItem.FindControl("txtUserID");
            TextBox txtPassword = (TextBox)editedItem.FindControl("txtPassword");
            RadComboBox cboStaff = (RadComboBox)editedItem.FindControl("cboStaff");
            RadComboBox cboCategory = (RadComboBox)editedItem.FindControl("cboCategory");
            TextBox txtcompany = (TextBox)editedItem.FindControl("txtcompany");
            TextBox txtwarehouse = (TextBox)editedItem.FindControl("txtwarehouse");
            CheckBox chkDept = (CheckBox)editedItem.FindControl("chkDept");
            CheckBox chkCompany = (CheckBox)editedItem.FindControl("chkCompany");
            CheckBox chkSupplier = (CheckBox)editedItem.FindControl("chkSupplier");
            CheckBox chkProduct = (CheckBox)editedItem.FindControl("chkProduct");
            CheckBox chkStaff = (CheckBox)editedItem.FindControl("chkStaff");
            CheckBox chkUser = (CheckBox)editedItem.FindControl("chkUser");
            CheckBox chkIncoming = (CheckBox)editedItem.FindControl("chkIncoming");
            CheckBox chkIssuing = (CheckBox)editedItem.FindControl("chkIssuing");
            CheckBox chkReport = (CheckBox)editedItem.FindControl("chkReport");
            CheckBox ChkWarehouse = (CheckBox)editedItem.FindControl("ChkWarehouse");

            string strCheck = "0";
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            SqlDataReader reader = BusinessTier.checkUserLoginId(conn, txtUserID.Text.ToString(),Convert.ToInt32(cboStaff.SelectedValue.ToString()));
            if (reader.Read())
            {
                strCheck = reader["checkdup"].ToString();
            }
            BusinessTier.DisposeReader(reader);
            BusinessTier.DisposeConnection(conn);

            if (strCheck.ToString() == "1")
            {
                ShowMessage(10);
                e.Canceled = true;
                return;
            }

            if (cboCategory.SelectedValue.ToString() == "--Select--")
            {
                ShowMessage(52);
                e.Canceled = true;
                return;
            }
            if (cboStaff.SelectedItem.Text.ToString() == "--Select--")
            {
                ShowMessage(35);
                e.Canceled = true;
                return;
            }

            else
            {
                int intSupp = 0; int intUser = 0; int intComp = 0; int intDept = 0; int intStaff = 0; int intProd = 0; int intIn = 0; int intOut = 0; int intReport = 0; int intwareh = 0;

                if (chkSupplier.Checked)
                    intSupp = 1;
                if (chkUser.Checked)
                    intUser = 1;
                if (chkCompany.Checked)
                    intComp = 1;
                if (chkDept.Checked)
                    intDept = 1;
                if (chkStaff.Checked)
                    intStaff = 1;
                if (chkProduct.Checked)
                    intProd = 1;
                if (chkIncoming.Checked)
                    intIn = 1;
                if (chkIssuing.Checked)
                    intOut = 1;
                if (chkReport.Checked)
                    intReport = 1;
                if (ChkWarehouse.Checked)
                    intwareh = 1;
                if (intSupp == 0 && intComp == 0 && intDept == 0 && intStaff == 0 && intProd == 0 && intIn == 0 && intOut == 0 && intReport == 0 && intwareh == 0)
                {
                    //ShowMessage(35);
                    lblStatus.Text = "Select the user permission";
                    e.Canceled = true;
                    return;
                }
                SqlConnection con = BusinessTier.getConnection();
                con.Open();
                //string userid = "0";
                //string strqry = "select id FROM userinfo where userid='" + cboStaff.Text.ToString().Trim() + "'   and Deleted=0";
                //SqlCommand cmd = new SqlCommand(strqry, conn);
                //SqlDataReader reader1 = cmd.ExecuteReader();
                //while (reader1.Read())
                //{
                //    userid = (reader1["id"].ToString().Trim());
                //}
                //BusinessTier.DisposeReader(reader);
                //if (userid == "0")
                //{
                int flg = BusinessTier.SaveUser(con, txtUserID.Text.ToString().Trim(), txtPassword.Text.ToString().Trim(), Convert.ToInt32(cboStaff.SelectedValue.ToString().Trim()), cboCategory.SelectedItem.Text.ToString().Trim(), Convert.ToInt32(Session["sesUserID"].ToString()), "N", 0, intComp, intDept, intProd, intSupp, intStaff, intUser, intIn, intOut, intReport, intwareh);
                BusinessTier.DisposeConnection(con);
                if (flg >= 1)
                {
                    ShowMessage(13);
                }
                //}
                //else
                //{
                //    lblStatus.Text = "This userID Already have on this staff";
                //    return;
                //}

            }
            RadGrid1.Rebind();
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterUser", "Insert", "Success", "Log");
        }
        catch (Exception ex)
        {
            ShowMessage(4);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterUser", "Insert", ex.ToString(), "Audit");
        }
    }

    protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            Label lblID = (Label)editedItem.FindControl("lblID");
            Label lblUserId = (Label)editedItem.FindControl("lblUserId");
            Label lblsname = (Label)editedItem.FindControl("lblsname");
            TextBox txtUserID = (TextBox)editedItem.FindControl("txtUserID");
            TextBox txtPassword = (TextBox)editedItem.FindControl("txtPassword");
            RadComboBox cboStaff = (RadComboBox)editedItem.FindControl("cboStaff");
            RadComboBox cboCategory = (RadComboBox)editedItem.FindControl("cboCategory");

            TextBox txtcompany = (TextBox)editedItem.FindControl("txtcompany");
            TextBox txtwarehouse = (TextBox)editedItem.FindControl("txtwarehouse");

            CheckBox chkDept = (CheckBox)editedItem.FindControl("chkDept");
            CheckBox chkCompany = (CheckBox)editedItem.FindControl("chkCompany");
            CheckBox chkSupplier = (CheckBox)editedItem.FindControl("chkSupplier");
            CheckBox chkProduct = (CheckBox)editedItem.FindControl("chkProduct");
            CheckBox chkStaff = (CheckBox)editedItem.FindControl("chkStaff");
            CheckBox chkUser = (CheckBox)editedItem.FindControl("chkUser");
            CheckBox chkIncoming = (CheckBox)editedItem.FindControl("chkIncoming");
            CheckBox chkIssuing = (CheckBox)editedItem.FindControl("chkIssuing");
            CheckBox chkReport = (CheckBox)editedItem.FindControl("chkReport");
            CheckBox ChkWarehouse = (CheckBox)editedItem.FindControl("ChkWarehouse");
            if ((lblUserId.Text.ToString().Trim()) != (txtUserID.Text.ToString().Trim()) || (lblsname.Text.ToString().Trim()) != (cboStaff.Text.ToString().Trim()))
            {
                string strCheck = "0";
                SqlConnection conn = BusinessTier.getConnection();
                conn.Open();
                if (cboCategory.SelectedValue.ToString() == "--Select--")
                {
                    ShowMessage(52);
                    e.Canceled = true;
                    return;
                }
                if (cboStaff.SelectedItem.Text.ToString() == "--Select--")
                {
                    ShowMessage(35);
                    e.Canceled = true;
                    return;
                }


                SqlDataReader reader = BusinessTier.checkUserLoginId(conn, txtUserID.Text.ToString(),Convert.ToInt32(cboStaff.SelectedValue.ToString()));
                if (reader.Read())
                {
                    strCheck = reader["checkdup"].ToString();
                }
                BusinessTier.DisposeReader(reader);
                BusinessTier.DisposeConnection(conn);

                if (strCheck.ToString() == "1")
                {
                    ShowMessage(10);
                    e.Canceled = true;
                    return;
                }
            }

            else
            {
                int intSupp = 0; int intUser = 0; int intComp = 0; int intDept = 0; int intStaff = 0; int intProd = 0; int intIn = 0; int intOut = 0; int intReport = 0; int intwarh = 0;
                if (chkSupplier.Checked)
                    intSupp = 1;
                if (chkUser.Checked)
                    intUser = 1;
                if (chkCompany.Checked)
                    intComp = 1;
                if (chkDept.Checked)
                    intDept = 1;
                if (chkStaff.Checked)
                    intStaff = 1;
                if (chkProduct.Checked)
                    intProd = 1;
                if (chkIncoming.Checked)
                    intIn = 1;
                if (chkIssuing.Checked)
                    intOut = 1;
                if (chkReport.Checked)
                    intReport = 1;
                if (ChkWarehouse.Checked)
                    intwarh = 1;
                if (intSupp == 0 && intComp == 0 && intDept == 0 && intStaff == 0 && intProd == 0 && intIn == 0 && intOut == 0 && intReport == 0 && intwarh == 0)
                {
                    //ShowMessage(35);
                    lblStatus.Text = "Select the user permission";
                    e.Canceled = true;
                    return;
                }

                SqlConnection con = BusinessTier.getConnection();
                con.Open();
                string staffid = "0";
                string strqry = "select staffid,staffname,deleted from staffmaster where staffname='" + cboStaff.Text.ToString().Trim() + "'   and Deleted=0";
                SqlCommand cmd = new SqlCommand(strqry, con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    staffid = rdr["staffid"].ToString().Trim();
                }
                BusinessTier.DisposeReader(rdr);
                int flg = BusinessTier.SaveUser(con, txtUserID.Text.ToString().Trim(), txtPassword.Text.ToString().Trim(), Convert.ToInt32(staffid), cboCategory.SelectedItem.Text.ToString().Trim(), Convert.ToInt32(Session["sesUserID"].ToString()), "U", Convert.ToInt32(lblID.Text.ToString().Trim()), intComp, intDept, intProd, intSupp, intStaff, intUser, intIn, intOut, intReport, intwarh);
                BusinessTier.DisposeConnection(con);

                if (flg >= 1)
                {
                    ShowMessage(14);
                }

            }

            //InsertLogAuditTrial is used to insert the action into MasterLog table
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterUser", "Update", "Success", "Log");
        }
        catch (Exception ex)
        {
            ShowMessage(5);
            e.Canceled = true;
            //InsertLogAuditTrial is used to insert the action into MasterAuditTrail table
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterUser", "Update", ex.ToString(), "Audit");
        }
        RadGrid1.Rebind();
    }

    private void ShowMessage(int errorNo)
    {
        lblStatus.Text = BusinessTier.g_ErrorMessagesDataTable.Rows[errorNo - 1]["Message"].ToString();
        System.Drawing.ColorConverter colConvert = new ColorConverter();
        string strColor = BusinessTier.g_ErrorMessagesDataTable.Rows[errorNo - 1]["Color"].ToString();
        lblStatus.ForeColor = (System.Drawing.Color)colConvert.ConvertFromString(strColor);
    }

    private void InsertLogAuditTrail(string userid, string module, string activity, string result, string flag)
    {
        SqlConnection connLog = BusinessTier.getConnection();
        connLog.Open();
        BusinessTier.InsertLogAuditTrial(connLog, userid, module, activity, result, flag);
        BusinessTier.DisposeConnection(connLog);
    }
    //----------------------------------------------------------------------------------------------------------
    protected void OnSelectedIndexWarehouse(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        RadComboBox combobox = (RadComboBox)sender;
        GridEditFormItem editedItem = (GridEditFormItem)combobox.NamingContainer;
        Label lblID = (Label)editedItem.FindControl("lblID");
        RadComboBox cboStaff = (RadComboBox)editedItem.FindControl("cboStaff");

        TextBox txtcompany = (TextBox)editedItem.FindControl("txtcompany");
        TextBox txtwarehouse = (TextBox)editedItem.FindControl("txtwarehouse");


        if (cboStaff.SelectedValue.ToString() != "")
        {
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            SqlCommand command = new SqlCommand("SELECT dbo.Warehouse.WarehouseName, dbo.Company.CompanyName, dbo.StaffMaster.StaffId, dbo.StaffMaster.StaffName FROM dbo.StaffMaster INNER JOIN dbo.Warehouse ON dbo.StaffMaster.WarehouseId = dbo.Warehouse.WarehouseId INNER JOIN dbo.Company ON dbo.Warehouse.CompanyID = dbo.Company.CompanyId  WHERE dbo.StaffMaster.StaffId  ='" + cboStaff.SelectedValue.ToString() + "'", conn); ;
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                txtcompany.Text = reader["companyname"].ToString();
                txtwarehouse.Text = reader["warehousename"].ToString();
            }
            BusinessTier.DisposeReader(reader);
            BusinessTier.DisposeConnection(conn);
        }
    }

    protected void staff_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {

        SqlConnection conn = BusinessTier.getConnection();
        conn.Open();
        string sql1 = "select staffid,staffname from staffmaster where DELETED=0   order by staffid";
        SqlDataAdapter adapter1 = new SqlDataAdapter(sql1, conn);
        adapter1.SelectCommand.Parameters.AddWithValue("@Text", e.Text);
        DataTable dataTable1 = new DataTable();
        adapter1.Fill(dataTable1);
        RadComboBox comboBox = (RadComboBox)sender;
        comboBox.Items.Clear();
        foreach (DataRow row in dataTable1.Rows)
        {
            RadComboBoxItem item = new RadComboBoxItem();
            item.Text = row["staffname"].ToString();
            item.Value = row["staffid"].ToString();

            item.Attributes.Add("staffname", row["staffname"].ToString());
            item.Attributes.Add("staffid", row["staffid"].ToString());
            comboBox.Items.Add(item);
            item.DataBind();
        }
        BusinessTier.DisposeAdapter(adapter1);
        BusinessTier.DisposeConnection(conn);
    }
}
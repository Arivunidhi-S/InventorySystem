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

public partial class Warehouse : System.Web.UI.Page
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
                SqlDataReader readerMenu = BusinessTier.getMenuList(connMenu, Session["sesUserID"].ToString());
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
            lblStatus.Text="err:Page init"+ex.Message.ToString();
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
            lblStatus.Text = "err:Page load" + ex.Message.ToString();
        }
    }

    protected void RadGrid1_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditFormItem editedItem = e.Item as GridEditFormItem;
                TextBox txtName = editedItem.FindControl("txtName") as TextBox;
                TextBoxSetting textBoxSetting = (TextBoxSetting)RadInputManager1.GetSettingByBehaviorID("TextBoxBehavior1");
                textBoxSetting.TargetControls.Add(new TargetInput(txtName.UniqueID, true));
                TextBox txtAddress1 = editedItem.FindControl("txtAddress1") as TextBox;
                TextBoxSetting textBoxSetting1 = (TextBoxSetting)RadInputManager1.GetSettingByBehaviorID("TextBoxBehavior1");
                textBoxSetting1.TargetControls.Add(new TargetInput(txtAddress1.UniqueID, true));
                RadComboBox cbocompany = editedItem.FindControl("cbocompany") as RadComboBox;
            }
        }
        catch (Exception ex)
        {
            ShowMessage(8);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "warehouse", "RadGrid1_ItemCreated", ex.ToString(), "Audit");
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
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "warehouse", "NeedDataSource", ex.ToString(), "Audit");
        }
    }

    public DataTable DataSourceHelper()
    {
        SqlConnection conn = BusinessTier.getConnection();
        conn.Close();
        string sql = "select * FROM VW_Warehouse WHERE Deleted=0 order by createddate desc";
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
            int flg = 0;
            string ID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["warehouseid"].ToString().Trim();
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            flg = BusinessTier.Savewarehouse(conn, 0, "", "", "", "", "", "", "", "", "", "", "", "", Session["sesUserID"].ToString(), "D", Convert.ToInt32(ID.ToString().Trim()));
            BusinessTier.DisposeConnection(conn);
            if (flg >= 1)
            {
                ShowMessage(46);
            }
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "warehouse", "Delete", "Success", "Log");
        }
        catch (Exception ex)
        {
            ShowMessage(6);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "warehouse", "Delete", ex.ToString(), "Audit");
        }
    }

    protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            TextBox txtName = (TextBox)editedItem.FindControl("txtName");
            RadTextBox txtCity = (RadTextBox)editedItem.FindControl("txtCity");
            TextBox txtAddress1 = (TextBox)editedItem.FindControl("txtAddress1");
            RadTextBox txtAddress2 = (RadTextBox)editedItem.FindControl("txtAddress2");
            RadTextBox txtState = (RadTextBox)editedItem.FindControl("txtState");
            RadTextBox txtCountry = (RadTextBox)editedItem.FindControl("txtCountry");
            RadMaskedTextBox txtPhone = (RadMaskedTextBox)editedItem.FindControl("txtPhone");
            RadMaskedTextBox txtPostcode = (RadMaskedTextBox)editedItem.FindControl("txtPostcode");
            RadMaskedTextBox txtFax = (RadMaskedTextBox)editedItem.FindControl("txtFax");

            RadTextBox txtEmail = (RadTextBox)editedItem.FindControl("txtEmail");
            RadTextBox txtWebsite = (RadTextBox)editedItem.FindControl("txtWebsite");
            RadTextBox txtDesc = (RadTextBox)editedItem.FindControl("txtDesc");

            RadComboBox cbocompany = editedItem.FindControl("cbocompany") as RadComboBox;

            if (cbocompany.SelectedValue.ToString() == "--Select--")
            {
                ShowMessage(42);
                e.Canceled = true;
                return;
            }
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            int wrhouseid = 0;
            string strqry = "select warehouseid FROM warehouse where warehousename='" + txtName.Text.ToString().Trim() + "' and companyid='" + cbocompany.SelectedValue.ToString().Trim() + "'   and Deleted=0";
            SqlCommand cmd = new SqlCommand(strqry, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                wrhouseid = Convert.ToInt32(reader["warehouseid"].ToString().Trim());
            }
            BusinessTier.DisposeReader(reader);

            if (wrhouseid == 0)
            {
                int flg = BusinessTier.Savewarehouse(conn, Convert.ToInt32(cbocompany.SelectedValue.ToString()), txtName.Text.ToString().Trim(), txtAddress1.Text.ToString().Trim(), txtAddress2.Text.ToString().Trim(), txtCity.Text.ToString().Trim(), txtState.Text.ToString().Trim(), txtCountry.Text.ToString().Trim(), txtPostcode.Text.ToString().Trim(), txtDesc.Text.ToString().Trim(), txtPhone.Text.ToString().Trim(), txtFax.Text.ToString().Trim(), txtEmail.Text.ToString().Trim(), txtWebsite.Text.ToString().Trim(), Session["sesUserID"].ToString(), "N", 0);
                BusinessTier.DisposeConnection(conn);
                if (flg >= 1)
                {
                    ShowMessage(44);
                }
            }
            else
            {
                ShowMessage(43);
                e.Canceled = true;
                return;
            }
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "warehouse", "Insert", "Success", "Log");
        }
        catch (Exception ex)
        {
            ShowMessage(4);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "warehouse", "Insert", ex.ToString(), "Audit");
        }
        RadGrid1.Rebind();
    }
    protected void RadGrid1_ItemDataBound(object source, Telerik.Web.UI.GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem && e.Item.IsInEditMode)
        {
            GridEditFormItem editedItem = e.Item as GridEditFormItem;
            Label lbl = (Label)editedItem.FindControl("lblID");
            RadComboBox cbocompany = (RadComboBox)editedItem.FindControl("cbocompany");
            if (!(string.IsNullOrEmpty(lbl.Text.ToString())))
            {
                SqlConnection conn = BusinessTier.getConnection();
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT dbo.Warehouse.WarehouseId, dbo.Company.CompanyName FROM dbo.Warehouse INNER JOIN dbo.Company ON dbo.Warehouse.CompanyID = dbo.Company.CompanyId where dbo.Warehouse.WarehouseId = '" + lbl.Text.ToString() + "'", conn);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    cbocompany.SelectedItem.Text = reader["CompanyName"].ToString();
                }
                BusinessTier.DisposeReader(reader);
                BusinessTier.DisposeConnection(conn);
            }

        }

    }

    protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            Label lblID = (Label)editedItem.FindControl("lblID");
            Label Lblwrhname = (Label)editedItem.FindControl("Lblwrhname");
            Label Lblcmpydummy = (Label)editedItem.FindControl("Lblcmpydummy");
            TextBox txtName = (TextBox)editedItem.FindControl("txtName");
            RadTextBox txtCity = (RadTextBox)editedItem.FindControl("txtCity");
            TextBox txtAddress1 = (TextBox)editedItem.FindControl("txtAddress1");
            RadTextBox txtAddress2 = (RadTextBox)editedItem.FindControl("txtAddress2");
            RadTextBox txtState = (RadTextBox)editedItem.FindControl("txtState");
            RadTextBox txtCountry = (RadTextBox)editedItem.FindControl("txtCountry");
            RadMaskedTextBox txtPhone = (RadMaskedTextBox)editedItem.FindControl("txtPhone");
            RadMaskedTextBox txtPostcode = (RadMaskedTextBox)editedItem.FindControl("txtPostcode");
            RadMaskedTextBox txtFax = (RadMaskedTextBox)editedItem.FindControl("txtFax");

            RadTextBox txtEmail = (RadTextBox)editedItem.FindControl("txtEmail");
            RadTextBox txtWebsite = (RadTextBox)editedItem.FindControl("txtWebsite");
            RadTextBox txtDesc = (RadTextBox)editedItem.FindControl("txtDesc");

            RadComboBox cbocompany = editedItem.FindControl("cbocompany") as RadComboBox;

            int cmpnyid = 0;
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();

            string strqry1 = "SELECT companyid,companyname from company where companyName='" + cbocompany.Text.ToString() + "'";
            SqlCommand cmd1 = new SqlCommand(strqry1, conn);
            SqlDataReader reader1 = cmd1.ExecuteReader();
            if (reader1.Read())
            {
                cmpnyid = Convert.ToInt32(reader1["companyid"].ToString().Trim());
            }
            BusinessTier.DisposeReader(reader1);
            if (cmpnyid == 0)
            {
                ShowMessage(42);
                e.Canceled = true;
                return;
            }

            if (txtName.Text.ToString() != Lblwrhname.Text.ToString())
            {
                string strqry = "select warehouseid,warehousename,companyid FROM warehouse where warehousename='" + txtName.Text.ToString().Trim() + "' and companyid='" + cmpnyid + "'   and Deleted=0";
                SqlCommand cmd = new SqlCommand(strqry, conn);
                int wname = 0;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    wname = Convert.ToInt32(reader["warehouseid"].ToString().Trim());
                }
                BusinessTier.DisposeReader(reader);
                if (wname != 0)
                {
                    ShowMessage(43);
                    e.Canceled = true;
                    return;
                }
            }
            if (cbocompany.SelectedItem.Text.ToString() != Lblcmpydummy.Text.ToString())
            {
                string strqry11 = "select warehouseid,warehousename,companyid FROM warehouse where warehousename='" + txtName.Text.ToString().Trim() + "' and companyid='" + cmpnyid + "'   and Deleted=0";
                SqlCommand cmd11 = new SqlCommand(strqry11, conn);
                int cname = 0;
                SqlDataReader reader11 = cmd11.ExecuteReader();
                if (reader11.Read())
                {
                    cname =Convert.ToInt32(reader11["warehouseid"].ToString().Trim());
                }
                BusinessTier.DisposeReader(reader11);
                if (cname != 0)
                {
                    ShowMessage(62);
                    return;
                }
            }
            BusinessTier.DisposeConnection(conn);

            SqlConnection conn1 = BusinessTier.getConnection();
            conn1.Open();
            int flg = BusinessTier.Savewarehouse(conn1, cmpnyid, txtName.Text.ToString().Trim(), txtAddress1.Text.ToString().Trim(), txtAddress2.Text.ToString().Trim(), txtCity.Text.ToString().Trim(), txtState.Text.ToString().Trim(), txtCountry.Text.ToString().Trim(), txtPostcode.Text.ToString().Trim(), txtDesc.Text.ToString().Trim(), txtPhone.Text.ToString().Trim(), txtFax.Text.ToString().Trim(), txtEmail.Text.ToString().Trim(), txtWebsite.Text.ToString().Trim(), Session["sesUserID"].ToString(), "U", Convert.ToInt32(lblID.Text.ToString().Trim()));
            BusinessTier.DisposeConnection(conn1);
            if (flg >= 1)
            {
                ShowMessage(45);
            }
            RadGrid1.Rebind();
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "warehouse", "Update", "Success", "Log");
        }
        catch (Exception ex)
        {
            ShowMessage(5);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "warehouse", "Update", ex.ToString(), "Audit");
        }

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
}



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

public partial class Company : System.Web.UI.Page
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

                TextBox txtName = editedItem.FindControl("txtName") as TextBox;
                TextBoxSetting textBoxSetting = (TextBoxSetting)RadInputManager1.GetSettingByBehaviorID("TextBoxBehavior1");
                textBoxSetting.TargetControls.Add(new TargetInput(txtName.UniqueID, true));
                TextBox txtAddress1 = editedItem.FindControl("txtAddress1") as TextBox;
                TextBoxSetting textBoxSetting1 = (TextBoxSetting)RadInputManager1.GetSettingByBehaviorID("TextBoxBehavior1");
                textBoxSetting1.TargetControls.Add(new TargetInput(txtAddress1.UniqueID, true));
            }
        }
        catch (Exception ex)
        {
            ShowMessage(8);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterCompany", "RadGrid1_ItemCreated", ex.ToString(), "Audit");
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
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterCompany", "NeedDataSource", ex.ToString(), "Audit");
        }
    }

    public DataTable DataSourceHelper()
    {
        SqlConnection conn = BusinessTier.getConnection();
        conn.Close();
        string sql = "select CompanyId,CompanyName,Address1,Address2,City,State,Postcode,Country,Description,Contactno,Faxno,Email,Website,Createdby,createddate FROM Company WHERE Deleted=0 order by createddate desc";
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
            string ID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CompanyId"].ToString().Trim();
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            flg = BusinessTier.SaveCompany(conn, "", "", "", "", "", "", "", "", "", "", "", "", Session["sesUserID"].ToString(), "D", ID.ToString().Trim());
            BusinessTier.DisposeConnection(conn);
            if (flg >= 1)
            {
                ShowMessage(18);
            }
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterCompany", "Delete", "Success", "Log");
        }
        catch (Exception ex)
        {
            ShowMessage(6);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterCompany", "Delete", ex.ToString(), "Audit");
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

            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            string companyid = "0";
            string strqry = "select companyid FROM company where companyname='" + txtName.Text.ToString().Trim() + "'   and Deleted=0";
            SqlCommand cmd = new SqlCommand(strqry, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                companyid = (reader["companyid"].ToString().Trim());
            }
            BusinessTier.DisposeReader(reader);
            if (companyid == "0")
            {
                int flg = BusinessTier.SaveCompany(conn, txtName.Text.ToString().Trim(), txtAddress1.Text.ToString().Trim(), txtAddress2.Text.ToString().Trim(), txtCity.Text.ToString().Trim(), txtState.Text.ToString().Trim(), txtCountry.Text.ToString().Trim(), txtPostcode.Text.ToString().Trim(), txtDesc.Text.ToString().Trim(), txtPhone.Text.ToString().Trim(), txtFax.Text.ToString().Trim(), txtEmail.Text.ToString().Trim(), txtWebsite.Text.ToString().Trim(), Session["sesUserID"].ToString(), "N", "0");
                BusinessTier.DisposeConnection(conn);
                if (flg >= 1)
                {
                    ShowMessage(16);
                }
            }
            else
            {
                ShowMessage(41);
                e.Canceled = true;
                return;
            }
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterCompany", "Insert", "Success", "Log");
        }
        catch (Exception ex)
        {
            ShowMessage(4);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterCompany", "Insert", ex.ToString(), "Audit");
        }
        RadGrid1.Rebind();
    }


    protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            Label lblID = (Label)editedItem.FindControl("lblID");
            Label Lblcmpname = (Label)editedItem.FindControl("Lblcmpname");
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
            if (txtName.Text.ToString() != Lblcmpname.Text.ToString())
            {
                SqlConnection conn1 = BusinessTier.getConnection();
                conn1.Open();
                string companyid = "0";
                string strqry = "select companyid FROM company where companyname='" + txtName.Text.ToString().Trim() + "'   and Deleted=0";
                SqlCommand cmd = new SqlCommand(strqry, conn1);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    companyid = (reader["companyid"].ToString().Trim());
                }
                BusinessTier.DisposeReader(reader);
                BusinessTier.DisposeConnection(conn1);
                if (companyid.ToString() != "0")
                {
                    ShowMessage(41);
                    e.Canceled = true;
                    return;
                }
            }
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            int flg = BusinessTier.SaveCompany(conn, txtName.Text.ToString().Trim(), txtAddress1.Text.ToString().Trim(), txtAddress2.Text.ToString().Trim(), txtCity.Text.ToString().Trim(), txtState.Text.ToString().Trim(), txtCountry.Text.ToString().Trim(), txtPostcode.Text.ToString().Trim(), txtDesc.Text.ToString().Trim(), txtPhone.Text.ToString().Trim(), txtFax.Text.ToString().Trim(), txtEmail.Text.ToString().Trim(), txtWebsite.Text.ToString().Trim(), Session["sesUserID"].ToString(), "U", lblID.Text.ToString().Trim());
            BusinessTier.DisposeConnection(conn);
            if (flg >= 1)
            {
                ShowMessage(17);
            }
            RadGrid1.Rebind();
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterCompany", "Update", "Success", "Log");
        }
        catch (Exception ex)
        {
            ShowMessage(5);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterCompany", "Update", ex.ToString(), "Audit");
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
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

public partial class Product : System.Web.UI.Page
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
    //=====================================

    //Data Source  =========================================================================
    protected void RadGrid1_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGridProduct.DataSource = DataSourceHelper();
        }
        catch (Exception ex)
        {
            ShowMessage(9);
            //InsertLogAuditTrial is used to insert the action into MasterAuditTrail table
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "productmaster", "NeedDataSource", ex.ToString(), "Audit");
        }
    }

    public DataTable DataSourceHelper()
    {
        SqlConnection conn = BusinessTier.getConnection();
        conn.Close();
        string sql = "SELECT * from VW_productdatabound where deleted=0 order by createddate Desc";
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql, conn);
        DataTable g_datatable = new DataTable();
        sqlDataAdapter.Fill(g_datatable);
        BusinessTier.DisposeAdapter(sqlDataAdapter);
        BusinessTier.DisposeConnection(conn);
        return g_datatable;
    }
    //==========================================================

    protected void RadGrid1_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditFormItem editedItem = e.Item as GridEditFormItem;
                TextBox txtProductCode = editedItem.FindControl("txtProductCode") as TextBox;
                TextBox txtProductName = editedItem.FindControl("txtProductName") as TextBox;
                RadNumericTextBox txtunitprice = (RadNumericTextBox)editedItem.FindControl("txtunitprice");
                RadNumericTextBox txtdurationtime = (RadNumericTextBox)editedItem.FindControl("txtdurationtime");
                RadNumericTextBox txtminstock = (RadNumericTextBox)editedItem.FindControl("txtminstock");
                RadComboBox cboduration = (RadComboBox)editedItem.FindControl("cboduration");
                RadComboBox cbowarehouse = (RadComboBox)editedItem.FindControl("cbowarehouse");
            }
        }
        catch (Exception ex)
        {
            ShowMessage(8);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "ProductMaster", "RadGrid1_ItemCreated", ex.ToString(), "Audit");
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
                TextBox txtProductCode = (TextBox)editedItem.FindControl("txtProductCode");
                TextBox txtProductName = (TextBox)editedItem.FindControl("txtProductName");
                TextBox txtunit = (TextBox)editedItem.FindControl("txtunit");
                RadNumericTextBox txtunitprice = (RadNumericTextBox)editedItem.FindControl("txtunitprice");
                RadNumericTextBox txtdurationtime = (RadNumericTextBox)editedItem.FindControl("txtdurationtime");
                RadNumericTextBox txtminstock = (RadNumericTextBox)editedItem.FindControl("txtminstock");
                RadComboBox cboduration = (RadComboBox)editedItem.FindControl("cboduration");
                RadComboBox cbowarehouse = (RadComboBox)editedItem.FindControl("cbowarehouse");

                if (!(string.IsNullOrEmpty(lblID.Text.ToString())))
                {
                    SqlConnection conn = BusinessTier.getConnection();
                    conn.Open();
                    SqlCommand command = new SqlCommand("select id,durationperiod from products where id= '" + lblID.Text.ToString() + "'", conn);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        cboduration.SelectedItem.Text = reader["DurationPeriod"].ToString();
                    }
                    BusinessTier.DisposeReader(reader);

                    SqlCommand cmd = new SqlCommand("SELECT dbo.Products.ID, dbo.Warehouse.WarehouseName, dbo.Products.Deleted FROM dbo.Products INNER JOIN dbo.Warehouse ON dbo.Products.WarehouseId = dbo.Warehouse.WarehouseId where dbo.Products.Deleted=0 and dbo.Products.ID= '" + lblID.Text.ToString() + "'", conn);
                    SqlDataReader reader1 = cmd.ExecuteReader();
                    if (reader1.Read())
                    {
                        cbowarehouse.SelectedItem.Text = reader1["WarehouseName"].ToString();
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
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "Product Master", "RadGrid1_ItemDataBound", ex.ToString(), "Audit");
        }
    }
    //==================================================================================
    protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            TextBox txtProductCode = (TextBox)editedItem.FindControl("txtProductCode");
            TextBox txtProductName = (TextBox)editedItem.FindControl("txtProductName");
            TextBox txtunit = (TextBox)editedItem.FindControl("txtunit");
            RadNumericTextBox txtunitprice = (RadNumericTextBox)editedItem.FindControl("txtunitprice");
            RadNumericTextBox txtdurationtime = (RadNumericTextBox)editedItem.FindControl("txtdurationtime");
            RadNumericTextBox txtminstock = (RadNumericTextBox)editedItem.FindControl("txtminstock");
            RadComboBox cboduration = (RadComboBox)editedItem.FindControl("cboduration");
            RadComboBox cbowarehouse = (RadComboBox)editedItem.FindControl("cbowarehouse");
            if (cboduration.SelectedValue.ToString() == "--Select--")
            {
                ShowMessage(29);
                e.Canceled = true;
                return;
            }
            if (cbowarehouse.SelectedValue.ToString() == "--Select--")
            {
                ShowMessage(30);
                e.Canceled = true;
                return;
            }
            if (txtunitprice.Text.ToString() == "0")
            {
                ShowMessage(65);
                e.Canceled = true;
                return;
            }
            if (txtdurationtime.Text.ToString() == "0")
            {
                ShowMessage(66);
                e.Canceled = true;
                return;
            }
            if (txtminstock.Text.ToString() == "0")
            {
                ShowMessage(67);
                e.Canceled = true;
                return;
            }
            if (cbowarehouse.SelectedValue.ToString() == "000")
            {
                cbowarehouse.SelectedValue = "000";
            }
            
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();

            string prodid = "0";
            string prodname = "0";
            string strqry = "select id FROM products where  productcode='" + txtProductCode.Text.ToString().Trim() + "' and Deleted=0";
            SqlCommand cmd = new SqlCommand(strqry, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                prodid = (reader["id"].ToString().Trim());
            }
            BusinessTier.DisposeReader(reader);

            string strqry1 = "select id FROM products where productname='" + txtProductName.Text.ToString().Trim() + "'  and Deleted=0";
            SqlCommand cmd1 = new SqlCommand(strqry1, conn);
            SqlDataReader reader1 = cmd1.ExecuteReader();
            while (reader1.Read())
            {
                prodname = (reader1["id"].ToString().Trim());
            }
            BusinessTier.DisposeReader(reader1);

            if (prodid == "0" && prodname == "0")
            {
                int flg = BusinessTier.SaveProducts(conn, txtProductCode.Text.ToString().Trim(), txtProductName.Text.ToString().Trim(), txtunit.Text.ToString(), Convert.ToDecimal(txtunitprice.Text.ToString()), Convert.ToInt32(txtdurationtime.Text.ToString()), cboduration.SelectedItem.Text.ToString().Trim(), Convert.ToInt32(txtminstock.Text.ToString()), Convert.ToInt32(cbowarehouse.SelectedValue.ToString()), Convert.ToInt32(Session["sesUserID"].ToString()), "prodnew", 0);
                BusinessTier.DisposeConnection(conn);
                if (flg >= 1)
                {
                    ShowMessage(53);
                }
            }
            else
            {
                if (prodid != "0")
                {
                    ShowMessage(56);
                    e.Canceled = true;
                    return;
                }
                else if (prodname != "0")
                {
                    ShowMessage(57);
                    e.Canceled = true;
                    return;
                }
            }
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "ProductMaster", "Insert", "Success", "Log");
        }
        catch (Exception ex)
        {
            ShowMessage(4);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "ProductMaster", "Insert", ex.ToString(), "Audit");
        }
        RadGridProduct.Rebind();
    }
    //------------------------------------------------------------------------------------
    protected void RadGrid1_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            int flg = 0;
            string ID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString().Trim();
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            flg = BusinessTier.SaveProducts(conn, "", "", "", 0, 0, "", 0, 0, Convert.ToInt32(Session["sesUserID"].ToString()), "ProdDel", Convert.ToInt32(ID.ToString().Trim()));
            BusinessTier.DisposeConnection(conn);
            if (flg >= 1)
            {
                ShowMessage(55);
            }
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "Product Master", "Delete", "Success", "Log");
        }
        catch (Exception ex)
        {
            ShowMessage(6);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "Product Master", "Delete", ex.ToString(), "Audit");
        }
    }
    //------------------------------------------------------------------------------------------------------------
    protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            Label lblID = (Label)editedItem.FindControl("lblID");
            Label Lblpcode = (Label)editedItem.FindControl("Lblpcode");
            Label Lblpname = (Label)editedItem.FindControl("Lblpname");
            TextBox txtProductCode = (TextBox)editedItem.FindControl("txtProductCode");
            TextBox txtProductName = (TextBox)editedItem.FindControl("txtProductName");
            TextBox txtunit = (TextBox)editedItem.FindControl("txtunit");
            RadNumericTextBox txtunitprice = (RadNumericTextBox)editedItem.FindControl("txtunitprice");
            RadNumericTextBox txtdurationtime = (RadNumericTextBox)editedItem.FindControl("txtdurationtime");
            RadNumericTextBox txtminstock = (RadNumericTextBox)editedItem.FindControl("txtminstock");
            RadComboBox cboduration = (RadComboBox)editedItem.FindControl("cboduration");
            RadComboBox cbowarehouse = (RadComboBox)editedItem.FindControl("cbowarehouse");
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            string pcode = "0";
            string prodname = "0";
            int warehouseid = 0;

            //if (cboduration.SelectedValue.ToString() == "--Select--")
            //{
            //    ShowMessage(29);
            //    e.Canceled = true;
            //    return;
            //}
            //if (cbowarehouse.SelectedValue.ToString() == "--Select--")
            //{
            //    ShowMessage(30);
            //    e.Canceled = true;
            //    return;
            //}
            if (txtunitprice.Text.ToString() == "0")
            {
                ShowMessage(65);
                e.Canceled = true;
                return;
            }
            if (txtdurationtime.Text.ToString() == "0")
            {
                ShowMessage(66);
                e.Canceled = true;
                return;
            }
            if (txtminstock.Text.ToString() == "0")
            {
                ShowMessage(67);
                e.Canceled = true;
                return;
            }
            string strqry = "SELECT warehouseid,warehousename from warehouse where warehousename='" + cbowarehouse.Text.ToString() + "'";
            SqlCommand cmd = new SqlCommand(strqry, conn);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                warehouseid = Convert.ToInt32(reader["warehouseid"].ToString().Trim());
            }
            BusinessTier.DisposeReader(reader);


            if (txtProductCode.Text.ToString() != Lblpcode.Text.ToString())
            {
                string strqry11 = "select id FROM products where  productcode='" + txtProductCode.Text.ToString().Trim() + "' and Deleted=0";
                SqlCommand cmd11 = new SqlCommand(strqry11, conn);
                SqlDataReader reader11 = cmd11.ExecuteReader();
                while (reader11.Read())
                {
                    pcode = reader11["id"].ToString().Trim();
                }
                BusinessTier.DisposeReader(reader11);
                if (pcode != "0")
                {
                    ShowMessage(56);
                    e.Canceled = true;
                    return;
                }
            }

            if (txtProductName.Text.ToString() != Lblpname.Text.ToString())
            {
                string strqry1 = "select id FROM products where productname='" + txtProductName.Text.ToString().Trim() + "'  and Deleted=0";
                SqlCommand cmd1 = new SqlCommand(strqry1, conn);
                SqlDataReader reader1 = cmd1.ExecuteReader();
                while (reader1.Read())
                {
                    prodname = (reader1["id"].ToString().Trim());
                }
                BusinessTier.DisposeReader(reader1);
                if (prodname != "0")
                {
                    ShowMessage(57);
                    e.Canceled = true;
                    return;
                }
            }
            int flg = BusinessTier.SaveProducts(conn, txtProductCode.Text.ToString().Trim(), txtProductName.Text.ToString().Trim(), txtunit.Text.ToString(), Convert.ToDecimal(txtunitprice.Text.ToString()), Convert.ToInt32(txtdurationtime.Text.ToString()), cboduration.SelectedItem.Text.ToString().Trim(), Convert.ToInt32(txtminstock.Text.ToString()), warehouseid, Convert.ToInt32(Session["sesUserID"].ToString()), "prodUpd", Convert.ToInt32(lblID.Text.ToString().Trim()));
            BusinessTier.DisposeConnection(conn);
            if (flg >= 1)
            {
                ShowMessage(54);
            }
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "Product Master", "Update", "Success", "Log");

        }
        catch (Exception ex)
        {
            ShowMessage(5);
            e.Canceled = true;
            //InsertLogAuditTrial is used to insert the action into MasterAuditTrail table
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "Product Master", "Update", ex.ToString(), "Audit");
        }
        RadGridProduct.Rebind();
    }
    //============================================================================================
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
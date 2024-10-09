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
using Telerik.Web.UI.Calendar;
public partial class Incoming : System.Web.UI.Page
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
                TextBox txtquantity = editedItem.FindControl("txtquantity") as TextBox;
                TextBox txtunitprice = editedItem.FindControl("txtunitprice") as TextBox;
                RadComboBox cboProductCode = (RadComboBox)editedItem.FindControl("cboProductCode");
                RadComboBox cbosuppliername = (RadComboBox)editedItem.FindControl("cbosuppliername");
                if (!Page.IsPostBack)
                {
                    RadDateTimePicker txtincomedate = editedItem.FindControl("txtincomedate") as RadDateTimePicker;
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage(8);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "Incoming", "RadGrid1_ItemCreated", ex.ToString(), "Audit");
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
                Label lblexprdate = (Label)editedItem.FindControl("lblexprdate");

                RadComboBox cboProductCode = (RadComboBox)editedItem.FindControl("cboProductCode");
                cboProductCode.AutoPostBack = true;
                cboProductCode.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(OnSelectedIndexChangedHandler);

                RadComboBox cbosuppliername = (RadComboBox)editedItem.FindControl("cbosuppliername");

                RadNumericTextBox txtunitprice = (RadNumericTextBox)editedItem.FindControl("txtunitprice");
                RadNumericTextBox txtquantity = (RadNumericTextBox)editedItem.FindControl("txtquantity");
                RadNumericTextBox txtamount = (RadNumericTextBox)editedItem.FindControl("txtamount");
                TextBox txtdono = (TextBox)editedItem.FindControl("txtdono");

                RadDateTimePicker txtincomedate = editedItem.FindControl("txtincomedate") as RadDateTimePicker;

                if (!(string.IsNullOrEmpty(lblID.Text.ToString())))
                {
                    SqlConnection conn = BusinessTier.getConnection();
                    conn.Open();
                    SqlCommand command = new SqlCommand("SELECT dbo.Incoming.ID,dbo.Incoming.ProductID, dbo.Supplier.SupplierName, dbo.Products.ProductName FROM dbo.Incoming LEFT OUTER JOIN dbo.Products ON dbo.Incoming.ProductID = dbo.Products.ID LEFT OUTER JOIN dbo.Supplier ON dbo.Incoming.SupplierId = dbo.Supplier.SupplierId where dbo.Incoming.ID = '" + lblID.Text.ToString() + "'", conn);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        cboProductCode.SelectedValue = reader["ProductID"].ToString();
                        cbosuppliername.SelectedItem.Text = reader["suppliername"].ToString();
                    }
                    BusinessTier.DisposeReader(reader);
                    SqlCommand command1 = new SqlCommand("SELECT unitprice,amount,quantity from incoming where id = '" + lblID.Text.ToString() + "'", conn);
                    SqlDataReader reader1 = command1.ExecuteReader();
                    while (reader1.Read())
                    {
                        txtunitprice.Text = reader1["unitprice"].ToString();
                        txtquantity.Text = reader1["quantity"].ToString();
                        txtamount.Text = reader1["amount"].ToString();
                    }
                    BusinessTier.DisposeReader(reader);
                    BusinessTier.DisposeConnection(conn);
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage(9);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "Incoming", "RadGrid1_ItemDataBound", ex.ToString(), "Audit");
        }
    }
    //=================================================================================================
    protected void RadGrid1_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid1.DataSource = DataSourceHelper();
        }
        catch (Exception ex)
        {
            ShowMessage(9);
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "Incoming", "NeedDataSource", ex.ToString(), "Audit");
        }
    }

    public DataTable DataSourceHelper()
    {
        SqlConnection conn = BusinessTier.getConnection();
        conn.Close();
        string sql = "select * from VW_incomedatasource  where deleted=0 order by createddate DESC";
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql, conn);
        DataTable g_datatable = new DataTable();
        sqlDataAdapter.Fill(g_datatable);
        BusinessTier.DisposeAdapter(sqlDataAdapter);
        BusinessTier.DisposeConnection(conn);
        return g_datatable;
    }
    //=================================================================================
    protected void RadGrid1_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            int flg = 0;
            string ID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString().Trim();
            string dt = Convert.ToString(System.DateTime.Now);
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            int quantity = 0;
            int balqty = 0;
            string strqry11 = "SELECT quantity,balqty from incoming where id='" + Convert.ToInt32(ID.ToString().Trim()) + "'";
            SqlCommand cmd11 = new SqlCommand(strqry11, conn);
            SqlDataReader reader11 = cmd11.ExecuteReader();
            if (reader11.Read())
            {
                quantity = Convert.ToInt32(reader11["quantity"].ToString().Trim());
                balqty = Convert.ToInt32(reader11["balqty"].ToString().Trim());
            }
            BusinessTier.DisposeReader(reader11);
            if (quantity == balqty)
            {
                flg = BusinessTier.Incoming(conn, 0, 0, 0, 0, 0, "", Convert.ToDateTime(dt), Convert.ToDateTime(dt), Convert.ToInt32(Session["sesUserID"].ToString()), "D", Convert.ToInt32(ID.ToString().Trim()));

                BusinessTier.DisposeConnection(conn);
                if (flg >= 1)
                {
                    ShowMessage(28);
                }
            }
            else
            {
                ShowMessage(69);
                e.Canceled = true;
                return;
            }
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "Incoming", "Delete", "Success", "Log");

        }
        catch (Exception ex)
        {
            ShowMessage(6);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "Incoming", "Delete", ex.ToString(), "Audit");
        }
    }
    //*******************************INSERT**********************************************************************************************
    protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            Label lblqerr = (Label)editedItem.FindControl("lblqerr");
            RadNumericTextBox txtunitprice = (RadNumericTextBox)editedItem.FindControl("txtunitprice");
            RadNumericTextBox txtquantity = (RadNumericTextBox)editedItem.FindControl("txtquantity");
            RadNumericTextBox txtamount = (RadNumericTextBox)editedItem.FindControl("txtamount");
            TextBox txtdono = (TextBox)editedItem.FindControl("txtdono");

            RadComboBox cboProductCode = (RadComboBox)editedItem.FindControl("cboProductCode");
            cboProductCode.AutoPostBack = true;
            cboProductCode.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(OnSelectedIndexChangedHandler);
            RadComboBox cbosuppliername = (RadComboBox)editedItem.FindControl("cbosuppliername");

            RadDateTimePicker txtincomingdate = (RadDateTimePicker)editedItem.FindControl("txtincomingdate");

            DateTime IncomingDate;
            IncomingDate = DateTime.Now;

            if (string.IsNullOrEmpty(cboProductCode.SelectedValue.ToString()))
            {
                ShowMessage(31);
                e.Canceled = true;
                return;
            }
            if (cbosuppliername.SelectedValue.ToString() == "--Select--")
            {
                ShowMessage(32);
                e.Canceled = true;
                return;
            }
            if (txtunitprice.Text.ToString() == "0")
            {
                ShowMessage(70);
                e.Canceled = true;
                return;
            }
            if (txtquantity.Text.ToString() == "0")
            {
                ShowMessage(33);
                e.Canceled = true;
                return;
            }
            if (string.IsNullOrEmpty(txtquantity.Text.ToString()))
            {
                ShowMessage(33);
                e.Canceled = true;
                return;
            }
            if (string.IsNullOrEmpty(txtincomingdate.SelectedDate.ToString()))
            {
                ShowMessage(34);
                e.Canceled = true;
                return;
            }

            if (!(string.IsNullOrEmpty(txtincomingdate.SelectedDate.ToString())))
            {
                IncomingDate = Convert.ToDateTime(txtincomingdate.SelectedDate.Value.ToString());
            }
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            DateTime dtt = new DateTime();
            SqlCommand command = new SqlCommand("select id,durationtime,durationperiod,unitprice,minimumstock from products where id='" + cboProductCode.SelectedValue+ "'", conn);
            //((((((((((((((((((((((Duration calc))))))))))))))))))))))))))))))))))))))))))))
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                int durtime = Convert.ToInt32(reader["durationtime"].ToString());
                string durperiod = reader["durationperiod"].ToString();
                if (durperiod == "Years")   //expired date calc
                {
                    dtt = Convert.ToDateTime(txtincomingdate.SelectedDate).AddYears(durtime);
                }
                if (durperiod == "Months")
                {
                    dtt = Convert.ToDateTime(txtincomingdate.SelectedDate).AddMonths(durtime);
                }
                if (durperiod == "Days")
                {
                    dtt = Convert.ToDateTime(txtincomingdate.SelectedDate).AddDays(durtime);
                }

            }
            BusinessTier.DisposeReader(reader);


            int flg = BusinessTier.Incoming(conn, Convert.ToInt32(cboProductCode.SelectedValue.ToString().Trim()), Convert.ToInt32(cbosuppliername.SelectedValue.ToString().Trim()), Convert.ToDouble(txtunitprice.Text.ToString().Trim()), Convert.ToInt32(txtquantity.Text.ToString().Trim()), Convert.ToDouble(txtamount.Text.ToString().Trim()), txtdono.Text.ToString().Trim(), IncomingDate, Convert.ToDateTime(dtt), Convert.ToInt32(Session["sesUserID"].ToString()), "N", 0);
            BusinessTier.DisposeConnection(conn);
            if (flg >= 1)
            {
                ShowMessage(26);
            }


            InsertLogAuditTrail(Session["sesUserID"].ToString(), "Incoming", "Insert", "Success", "Log");
        }
        catch (Exception ex)
        {
            ShowMessage(4);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "Incoming", "Insert", ex.ToString(), "Audit");
        }
        RadGrid1.Rebind();
    }
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }
    //****************************UPDATE*********************************************************************************
    protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            Label lblID = (Label)editedItem.FindControl("lblID");
            RadNumericTextBox txtunitprice = (RadNumericTextBox)editedItem.FindControl("txtunitprice");
            RadNumericTextBox txtquantity = (RadNumericTextBox)editedItem.FindControl("txtquantity");
            RadNumericTextBox txtamount = (RadNumericTextBox)editedItem.FindControl("txtamount");
            TextBox txtdono = (TextBox)editedItem.FindControl("txtdono");

            RadComboBox cboProductCode = (RadComboBox)editedItem.FindControl("cboProductCode");
            RadComboBox cbosuppliername = (RadComboBox)editedItem.FindControl("cbosuppliername");

            RadDateTimePicker txtincomingdate = (RadDateTimePicker)editedItem.FindControl("txtincomingdate");
            //-----------------------------------------------------------------
            DateTime IncomingDate;
            //if (string.IsNullOrEmpty(cboProductCode.SelectedValue.ToString()))
            //{
            //    ShowMessage(31);
            //    e.Canceled = true;
            //    return;
            //}
            //if (cbosuppliername.SelectedValue.ToString() == "--Select--")
            //{
            //    ShowMessage(32);
            //    e.Canceled = true;
            //    return;
            //}
            if (txtunitprice.Text.ToString() == "0")
            {
                ShowMessage(70);
                e.Canceled = true;
                return;
            }
            if (txtquantity.Text.ToString() == "0")
            {
                ShowMessage(33);
                e.Canceled = true;
                return;
            }
            if (string.IsNullOrEmpty(txtquantity.Text.ToString()))
            {
                ShowMessage(33);
                e.Canceled = true;
                return;
            }

            if (!(string.IsNullOrEmpty(txtincomingdate.SelectedDate.ToString())))
            {
                IncomingDate = Convert.ToDateTime(txtincomingdate.SelectedDate.Value.ToString());
            }
            else
            {
                ShowMessage(34);
                e.Canceled = true;
                return;
            }

            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            int productcodeid = 0;
            int suppliernameeid = 0;
            int stock = 0;
            DateTime dtt = new DateTime();
            //(((((((((((((((((((((((Supplier id insert to table)))))))))))))))))))))))))))
            string strqry = "SELECT supplierid,suppliername from supplier where SupplierName='" + cbosuppliername.Text.ToString() + "'";
            SqlCommand cmd = new SqlCommand(strqry, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                suppliernameeid = Convert.ToInt32(reader["supplierid"].ToString().Trim());
            }
            BusinessTier.DisposeReader(reader);
            //(((((((((((((((((((((((product id insert to table)))))))))))))))))))))))))))
            string strqry1 = "SELECT id,productname from products where productname='" + cboProductCode.Text.ToString() + "'";
            SqlCommand cmd1 = new SqlCommand(strqry1, conn);
            SqlDataReader reader1 = cmd1.ExecuteReader();
            if (reader1.Read())
            {
                productcodeid = Convert.ToInt32(reader1["id"].ToString().Trim());
            }
            BusinessTier.DisposeReader(reader1);

            //(((((((((((((((((((((((Expired date update)))))))))))))))))))))))))))
            SqlCommand command = new SqlCommand("SELECT dbo.Incoming.ID, dbo.Products.DurationTime, dbo.Products.DurationPeriod FROM dbo.Products RIGHT OUTER JOIN dbo.Incoming ON dbo.Products.ID = dbo.Incoming.ProductID where  dbo.Incoming.ID ='" + lblID.Text.ToString() + "'", conn);
            SqlDataReader red = command.ExecuteReader();
            if (red.Read())
            {
                int durtime = Convert.ToInt32(red["durationtime"].ToString());
                string durperiod = red["durationperiod"].ToString();
                if (durperiod == "Years")
                {
                    dtt = Convert.ToDateTime(txtincomingdate.SelectedDate).AddYears(durtime);
                }
                if (durperiod == "Months")
                {
                    dtt = Convert.ToDateTime(txtincomingdate.SelectedDate).AddMonths(durtime);
                }
                if (durperiod == "Days")
                {
                    dtt = Convert.ToDateTime(txtincomingdate.SelectedDate).AddDays(durtime);
                }
            }
            BusinessTier.DisposeReader(red);
            int quantity = 0;
            int balqty = 0;
            string strqry11 = "SELECT quantity,balqty from incoming where id='" + lblID.Text.ToString() + "'";
            SqlCommand cmd11 = new SqlCommand(strqry11, conn);
            SqlDataReader reader11 = cmd11.ExecuteReader();
            if (reader11.Read())
            {
                quantity = Convert.ToInt32(reader11["quantity"].ToString().Trim());
                balqty = Convert.ToInt32(reader11["balqty"].ToString().Trim());
            }
            BusinessTier.DisposeReader(reader11);
            if (quantity == balqty)
            {
                int flg = BusinessTier.Incoming(conn, productcodeid, suppliernameeid, Convert.ToDouble(txtunitprice.Text.ToString().Trim()), Convert.ToInt32(txtquantity.Text.ToString()), Convert.ToDouble(txtamount.Text.ToString().Trim()), txtdono.Text.ToString().Trim(), IncomingDate, Convert.ToDateTime(dtt), Convert.ToInt32(Session["sesUserID"].ToString()), "U", Convert.ToInt32(lblID.Text.ToString().Trim()));
                if (flg >= 1)
                {
                    ShowMessage(27);
                }
            }
            else
            {
                ShowMessage(68);
                e.Canceled = true;
                return;
            }
            BusinessTier.DisposeConnection(conn);
            //InsertLogAuditTrial is used to insert the action into MasterLog table
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "Incoming", "Update", "Success", "Log");
        }
        catch (Exception ex)
        {
            ShowMessage(5);
            e.Canceled = true;
            //InsertLogAuditTrial is used to insert the action into MasterAuditTrail table
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "Incoming", "Update", ex.ToString(), "Audit");
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
    //----------------------------------------------------
    protected void Productcode_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {

        SqlConnection conn = BusinessTier.getConnection();
        conn.Open();
        try
        {
            string sql1 = "select id,productcode,productname from products where DELETED=0 order by createddate";
            SqlDataAdapter adapter1 = new SqlDataAdapter(sql1, conn);
            adapter1.SelectCommand.Parameters.AddWithValue("@Text", e.Text);
            DataTable dataTable1 = new DataTable();
            adapter1.Fill(dataTable1);
            RadComboBox comboBox = (RadComboBox)sender;
            comboBox.Items.Clear();
            foreach (DataRow row in dataTable1.Rows)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = row["productname"].ToString();
                item.Value = row["id"].ToString();
                string pcode = row["productcode"].ToString();
                item.Attributes.Add("productname", row["productname"].ToString());
                item.Attributes.Add("id", row["id"].ToString());
                item.Attributes.Add("productcode", row["productcode"].ToString());
                comboBox.Items.Add(item);
                item.DataBind();
            }
            BusinessTier.DisposeAdapter(adapter1);
            BusinessTier.DisposeConnection(conn);
        }
        catch (Exception ex)
        {
            lblStatus.Text = "err" + ex.Message.ToString();
            return;
        }
    }
    protected void RadGrid1_ItemCommand(object source, GridCommandEventArgs e)
    {
        RadToolTipManager1.TargetControls.Clear();
    }
    //------------------------------------------------------------------------------------------------------------duration calc
    protected void OnSelectedIndexChangedHandler(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        RadComboBox combobox = (RadComboBox)sender;
        GridEditFormItem editedItem = (GridEditFormItem)combobox.NamingContainer;
        RadComboBox cboProductCode = (RadComboBox)editedItem.FindControl("cboProductCode");
        RadNumericTextBox txtunitprice = (RadNumericTextBox)editedItem.FindControl("txtunitprice");
        RadDateTimePicker txtincomingdate = (RadDateTimePicker)editedItem.FindControl("txtincomingdate");

        if (cboProductCode.SelectedValue != "0")
        {
            DataTable DT = new DataTable();
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            try
            {
                SqlCommand command = new SqlCommand("select id,productcode,durationtime,durationperiod,unitprice,minimumstock from products where Deleted = 0 and id = '" + cboProductCode.SelectedValue.ToString() + "'", conn);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    int durtime = Convert.ToInt32(reader["durationtime"].ToString());
                    string durperiod = reader["durationperiod"].ToString();
                    txtunitprice.Text = reader["unitprice"].ToString();
                    cboProductCode.SelectedValue = reader["id"].ToString();
                    //Lblminstck.Visible = true;
                    //Txtminstock.Visible = true;
                    //Txtminstock.Text = reader["minimumstock"].ToString();
                }
                BusinessTier.DisposeReader(reader);
                BusinessTier.DisposeConnection(conn);
            }
            catch (Exception ex)
            {
                BusinessTier.DisposeConnection(conn);
                lblStatus.Text = "Err:Select Correct Product Code" + ex.Message.ToString();
                return;
            }
        }
    }
    protected void OnSelectedIndexSupplierdummy(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        RadComboBox combobox = (RadComboBox)sender;
        GridEditFormItem editedItem = (GridEditFormItem)combobox.NamingContainer;
        Label Lblsupperr = (Label)editedItem.FindControl("Lblsupperr");
        RadComboBox cboProductCode = (RadComboBox)editedItem.FindControl("cboProductCode");
        RadComboBox cbosuppliername = (RadComboBox)editedItem.FindControl("cbosuppliername");
        int supID = 0;
        Lblsupperr.Text = "";
        SqlConnection conn = BusinessTier.getConnection();
        conn.Open();
        try
        {
            if (string.IsNullOrEmpty(cboProductCode.SelectedValue.ToString()))
            {
                ShowMessage(31);
                return;
            }
            if (cboProductCode.SelectedValue != "0")
            {
                string strqry = "select id FROM incoming where supplierid='" + cbosuppliername.SelectedValue.ToString().Trim() + "' and productid='" + cboProductCode.SelectedValue.ToString().Trim() + "' and Deleted=0";
                SqlCommand cmd = new SqlCommand(strqry, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    supID = Convert.ToInt32(reader["id"].ToString().Trim());
                }

                //if (supID != 0)
                //{
                //    Lblsupperr.Text = "(* This Product Item Almost received from this supplier )";
                //    return;
                //}
                BusinessTier.DisposeReader(reader);
                BusinessTier.DisposeConnection(conn);
            }
        }
        catch (Exception ex)
        {
            BusinessTier.DisposeConnection(conn);
            lblStatus.Text = "Err:OnSelectedIndexSupplierdummy suppliername duplicate" + ex.Message.ToString();
            return;
        }
    }
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    protected void IncomingSelectDate_SelectedDateChanged(object sender, SelectedDateChangedEventArgs e)
    {
        RadDateTimePicker radpicker = (RadDateTimePicker)sender;
        GridEditFormItem editedItem = (GridEditFormItem)radpicker.NamingContainer;
        Label Lblindateerr = (Label)editedItem.FindControl("Lblindateerr");
        RadComboBox cboProductCode = (RadComboBox)editedItem.FindControl("cboProductCode");
        RadComboBox cbosuppliername = (RadComboBox)editedItem.FindControl("cbosuppliername");
        RadDateTimePicker txtincomingdate = (RadDateTimePicker)editedItem.FindControl("txtincomingdate");
        Lblindateerr.Text = "";
        SqlConnection conn = BusinessTier.getConnection();
        conn.Open();
        try
        {
            if (string.IsNullOrEmpty(cboProductCode.SelectedValue.ToString()))
            {
                ShowMessage(31);
                return;
            }
            int IncomID = 0;
            if (cboProductCode.SelectedValue != "0")
            {
                string strqry = "select id  FROM incoming where incomingdate='" + Convert.ToDateTime(txtincomingdate.SelectedDate.ToString().Trim()) + "' and productid='" + cboProductCode.SelectedValue.ToString() + "' and Deleted=0";
                SqlCommand cmd = new SqlCommand(strqry, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    IncomID = Convert.ToInt32(reader["id"].ToString().Trim());
                }

                if (IncomID != 0)
                {
                    Lblindateerr.Text = "(* This Selected Date Already received from Same Product)";
                    return;
                }
                BusinessTier.DisposeReader(reader);
            }
            BusinessTier.DisposeConnection(conn);
        }
        catch (Exception ex)
        {
            BusinessTier.DisposeConnection(conn);
            lblStatus.Text = "Err:Please correct Datetime format" + ex.Message.ToString();
            return;
        }
    }
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    protected void txtquantity_TextChanged(object sender, EventArgs e)
    {
        try
        {
            RadNumericTextBox radtxtbox = ((RadNumericTextBox)(sender));
            GridEditFormItem editedItem = ((GridEditFormItem)(radtxtbox.NamingContainer));
            RadNumericTextBox txtunitprice = (RadNumericTextBox)editedItem.FindControl("txtunitprice");
            RadNumericTextBox txtquantity = (RadNumericTextBox)editedItem.FindControl("txtquantity");
            RadNumericTextBox txtamount = (RadNumericTextBox)editedItem.FindControl("txtamount");
            string quantity = "0";
            double unitprice = 0;
            if (txtquantity.Text == "" || txtquantity.Text == "0")
            {
                lblStatus.Text = "Quantity is mandatory";
                return;
            }
            if (txtunitprice.Text == "" || txtunitprice.Text == "0")
            {
                lblStatus.Text = "Unit Price is mandatory";
                return;
            }
            if (txtunitprice.Text != "" || txtquantity.Text != "")
            {
                unitprice = Convert.ToDouble(txtunitprice.Text.ToString());
                quantity =  txtquantity.Text.ToString();
                txtamount.Text = Convert.ToString((Convert.ToDouble(txtunitprice.Text)) * (Convert.ToInt64(txtquantity.Text)));
                txtamount.Focus();
            }
            else
            {
                lblStatus.Text = "Please Enter Mandatory Fileds";
                return;
            }
        }
        catch (Exception ex)
        {
            lblStatus.Text = "Err:OnSelectedIndexSupplierdummy suppliername duplicate" + ex.Message.ToString();
            return;
        }
    }
}



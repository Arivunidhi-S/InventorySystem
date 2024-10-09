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

public partial class Staff : System.Web.UI.Page
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
                TextBox txtCode = editedItem.FindControl("txtCode") as TextBox;
                TextBox txtName = editedItem.FindControl("txtName") as TextBox;
                RadComboBox cbowarehouse = (RadComboBox)editedItem.FindControl("cbowarehouse");
                RadComboBox cboCompany = (RadComboBox)editedItem.FindControl("cbowarehouse");
                RadComboBox cboDept = (RadComboBox)editedItem.FindControl("cbowarehouse");
            }
        }
        catch (Exception ex)
        {
            ShowMessage(8);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterStaff", "RadGrid1_ItemCreated", ex.ToString(), "Audit");
        }
    }

    protected void RadGrid1_ItemDataBound(object source, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditFormItem editedItem = e.Item as GridEditFormItem;
                Label lbl = (Label)editedItem.FindControl("lblID");
                Label lblwname = (Label)editedItem.FindControl("lblwname");
                Label lblwerr = (Label)editedItem.FindControl("lblwerr");
                Label lbldname = (Label)editedItem.FindControl("lbldname");
                Label lblderr = (Label)editedItem.FindControl("lblderr");
                RadComboBox cboDept = (RadComboBox)editedItem.FindControl("cboDept");
                RadComboBox cboCompany = (RadComboBox)editedItem.FindControl("cboCompany");
                cboCompany.AutoPostBack = true;
                cboCompany.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(OnSelectedIndexcompany);
                RadComboBox cbowarehouse = (RadComboBox)editedItem.FindControl("cbowarehouse");

                if (!(string.IsNullOrEmpty(lbl.Text.ToString())))
                {
                    SqlConnection conn = BusinessTier.getConnection();
                    conn.Open();
                    lblwname.Visible = true;
                    lblwerr.Visible = true;
                    cbowarehouse.Visible = true;
                    cboDept.Visible = true;
                    lbldname.Visible = true;
                    lblderr.Visible = true;
                    string sql = "SELECT * from VW_staffdatabound where  StaffId = '" + lbl.Text.ToString() + "'";
                    SqlCommand command = new SqlCommand(sql, conn);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        cboCompany.SelectedItem.Text = reader["Companyname"].ToString();
                        cboDept.Text = reader["Deptname"].ToString();
                        if (reader["warehouseid"].ToString().Trim() != "0")
                            cbowarehouse.SelectedValue = reader["warehousename"].ToString();
                    }
                    BusinessTier.DisposeReader(reader);

                    BusinessTier.DisposeConnection(conn);
                }

            }
        }
        catch (Exception ex)
        {
            ShowMessage(8);
            e.Canceled = true;
            //InsertLogAuditTrial is used to insert the action into MasterAuditTrail table
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterStaff", "RadGrid1_ItemDataBound", ex.ToString(), "Audit");
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
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterStaff", "NeedDataSource", ex.ToString(), "Audit");
        }
    }

    public DataTable DataSourceHelper()
    {
        SqlConnection conn = BusinessTier.getConnection();
        conn.Close();
        string sql = "select * FROM StaffCompDept where deleted=0 order by createddate desc";
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
            string ID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["StaffId"].ToString().Trim();
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            flg = BusinessTier.SaveStaff(conn, "", "", "", "", 0, 0, "", "", Session["sesUserID"].ToString(), "D", Convert.ToInt32(ID.ToString().Trim()));
            BusinessTier.DisposeConnection(conn);
            if (flg >= 1)
            {
                ShowMessage(24);
            }
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterStaff", "Delete", "Success", "Log");
        }
        catch (Exception ex)
        {
            ShowMessage(6);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterStaff", "Delete", ex.ToString(), "Audit");
        }
    }

    protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            TextBox txtCode = (TextBox)editedItem.FindControl("txtCode");
            TextBox txtName = (TextBox)editedItem.FindControl("txtName");
            RadTextBox txtDesignation = (RadTextBox)editedItem.FindControl("txtDesignation");
            RadTextBox txtLocation = (RadTextBox)editedItem.FindControl("txtLocation");
            RadComboBox cboDept = (RadComboBox)editedItem.FindControl("cboDept");
            RadComboBox cboCompany = (RadComboBox)editedItem.FindControl("cboCompany");
            RadComboBox cbowarehouse = (RadComboBox)editedItem.FindControl("cbowarehouse");
            RadMaskedTextBox txtPhone = (RadMaskedTextBox)editedItem.FindControl("txtPhone");
            RadTextBox txtEmail = (RadTextBox)editedItem.FindControl("txtEmail");

            if (cboCompany.SelectedValue.ToString() == "--Select--")
            {
                ShowMessage(42);
                e.Canceled = true;
                return;
            }
            if (cboDept.Visible == true)
            {
                if (string.IsNullOrEmpty(cboDept.SelectedValue.ToString()))
                {
                    ShowMessage(49);
                    e.Canceled = true;
                    return;
                }
            }
            else
            {
                lblStatus.Text = "Sorry,This company not have Department means,this staff not cabable to insert";
                e.Canceled = true;
                return;
            }

            if (cbowarehouse.Visible == true)
            {
                if (string.IsNullOrEmpty(cbowarehouse.SelectedValue.ToString()))
                {
                    ShowMessage(30);
                    e.Canceled = true;
                    return;
                }
            }
            else
            {
                lblStatus.Text = "Sorry,This company not have Warehouse means,this staff not cabable to insert";
                e.Canceled = true;
                return;
            }
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            string staffid = "0";
            string strqry = "select staffid FROM VW_Staffduplicatidentify where staffno='" + txtCode.Text.ToString().Trim() + "' and companyname='" + cboCompany.SelectedItem.Text.ToString() + "' and Deleted=0";
            SqlCommand cmd = new SqlCommand(strqry, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                staffid = (reader["staffid"].ToString().Trim()); //code duplicate purpose take staffid
            }
            BusinessTier.DisposeReader(reader);

            string staffname = "0";
            string strqry11 = "select staffid FROM VW_Staffduplicatidentify where staffname='" + txtName.Text.ToString().Trim() + "' and companyname='" + cboCompany.SelectedItem.Text.ToString() + "' and Deleted=0";
            SqlCommand cmd11 = new SqlCommand(strqry11, conn);
            SqlDataReader reader11 = cmd11.ExecuteReader();
            while (reader11.Read())
            {
                staffname = (reader11["staffid"].ToString().Trim()); //name duplicate purpose take staffid
            }
            BusinessTier.DisposeReader(reader11);

            if (staffid == "0" && staffname == "0")
            {
                int flg = BusinessTier.SaveStaff(conn, txtCode.Text.ToString().Trim(), txtName.Text.ToString().Trim(), txtDesignation.Text.ToString().Trim(), txtLocation.Text.ToString().Trim(), Convert.ToInt32(cboDept.SelectedItem.Value.ToString().Trim()), Convert.ToInt32(cbowarehouse.SelectedValue.ToString()), txtPhone.Text.ToString().Trim(), txtEmail.Text.ToString().Trim(), Session["sesUserID"].ToString(), "N", 0);
                BusinessTier.DisposeConnection(conn);
                if (flg >= 1)
                {
                    ShowMessage(22);
                }
            }
            else
            {
                if (staffid != "0")
                {
                    ShowMessage(51);
                    e.Canceled = true;
                    return;
                }
                else if (staffname != "0")
                {
                    ShowMessage(50);
                    e.Canceled = true;
                    return;
                }
            }
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterStaff", "Insert", "Success", "Log");
            RadGrid1.Rebind();
        }
        catch (Exception ex)
        {
            ShowMessage(4);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterStaff", "Insert", ex.ToString(), "Audit");
        }
        RadGrid1.Rebind();
    }
    //-----------------------------------------------------------------------------------------
    protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            Label lblID = (Label)editedItem.FindControl("lblID");
            Label LblCode = (Label)editedItem.FindControl("LblCode");
            Label LblName = (Label)editedItem.FindControl("LblName");
            Label Lblcmpy = (Label)editedItem.FindControl("Lblcmpy");
            TextBox txtCode = (TextBox)editedItem.FindControl("txtCode");
            TextBox txtName = (TextBox)editedItem.FindControl("txtName");

            RadTextBox txtDesignation = (RadTextBox)editedItem.FindControl("txtDesignation");
            RadTextBox txtLocation = (RadTextBox)editedItem.FindControl("txtLocation");
            RadComboBox cboCompany = (RadComboBox)editedItem.FindControl("cboCompany");
            RadComboBox cboDept = (RadComboBox)editedItem.FindControl("cboDept");
            RadComboBox cbowarehouse = (RadComboBox)editedItem.FindControl("cbowarehouse");

            RadMaskedTextBox txtPhone = (RadMaskedTextBox)editedItem.FindControl("txtPhone");
            RadTextBox txtEmail = (RadTextBox)editedItem.FindControl("txtEmail");
            if (cboDept.SelectedValue.ToString() == "0")
            {
                ShowMessage(49);
                e.Canceled = true;
                return;
            }
            if (cbowarehouse.SelectedValue.ToString() == "0")
            {
                ShowMessage(30);
                e.Canceled = true;
                return;
            }

            if (cboDept.Visible == true)
            {
                if (string.IsNullOrEmpty(cboDept.SelectedValue.ToString()))
                {
                    ShowMessage(49);
                    e.Canceled = true;
                    return;
                }
            }
            else
            {
                lblStatus.Text = "Sorry,This company not have Department means,this staff not cabable to insert";
                e.Canceled = true;
                return;
            }
            if (cbowarehouse.Visible == true)
            {
                if (string.IsNullOrEmpty(cbowarehouse.SelectedValue.ToString()))
                {
                    ShowMessage(30);
                    e.Canceled = true;
                    return;
                }
            }
            else
            {
                lblStatus.Text = "Sorry,This company not have Warehouse means,this staff not cabable to insert";
                e.Canceled = true;
                return;
            }
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            int Deptid = 0;
            int Warehid = 0;
            string strqry1 = "SELECT deptid,deptname from department where deptname='" + cboDept.Text.ToString() + "'";
            SqlCommand cmd1 = new SqlCommand(strqry1, conn);
            SqlDataReader reader1 = cmd1.ExecuteReader();
            if (reader1.Read())
            {
                Deptid = Convert.ToInt32(reader1["deptid"].ToString().Trim());
            }
            BusinessTier.DisposeReader(reader1);

            string strqry11 = "SELECT warehouseid,warehousename from warehouse where warehousename='" + cbowarehouse.Text.ToString() + "'";
            SqlCommand cmd11 = new SqlCommand(strqry11, conn);
            SqlDataReader reader11 = cmd11.ExecuteReader();
            if (reader11.Read())
            {
                Warehid = Convert.ToInt32(reader11["warehouseid"].ToString().Trim());
            }
            BusinessTier.DisposeReader(reader11);


            if (txtCode.Text.ToString() != LblCode.Text.ToString())
            {
                string staffid = "0";
                string strqry = "select staffid FROM VW_Staffduplicatidentify where staffno='" + txtCode.Text.ToString().Trim() + "' and companyname='" + cboCompany.SelectedItem.Text.ToString() + "' and Deleted=0";
                SqlCommand cmd = new SqlCommand(strqry, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    staffid = (reader["staffid"].ToString().Trim()); //code duplicate purpose take staffid
                }
                BusinessTier.DisposeReader(reader);
                if (staffid != "0")
                {
                    ShowMessage(51);
                    e.Canceled = true;
                    return;
                }
            }
            if (txtName.Text.ToString() != LblName.Text.ToString())
            {
                string staffname = "0";
                string strqry111 = "select staffid FROM VW_Staffduplicatidentify where staffname='" + txtName.Text.ToString().Trim() + "' and companyname='" + cboCompany.SelectedItem.Text.ToString() + "' and Deleted=0";
                SqlCommand cmd111 = new SqlCommand(strqry111, conn);
                SqlDataReader reader111 = cmd111.ExecuteReader();
                while (reader111.Read())
                {
                    staffname = (reader111["staffid"].ToString().Trim()); //name duplicate purpose take staffid
                }
                BusinessTier.DisposeReader(reader111);
                if (staffname != "0")
                {
                    ShowMessage(50);
                    e.Canceled = true;
                    return;
                }
            }
            if (cboCompany.Text.ToString() != Lblcmpy.Text.ToString())
            {
                string cmpid = "0";
                string strqry1112 = "select companyid,companyname FROM VW_Staffduplicatidentify where staffname='" + txtName.Text.ToString().Trim() + "' and staffno='" + txtCode.Text.ToString().Trim() + "' and Deleted=0";
                SqlCommand cmd1112 = new SqlCommand(strqry1112, conn);
                SqlDataReader reader1112 = cmd1112.ExecuteReader();
                while (reader1112.Read())
                {
                    cmpid = (reader1112["companyname"].ToString().Trim()); //name duplicate purpose take staffid
                }
                BusinessTier.DisposeReader(reader1112);
                //if (cmpid != cboCompany.Text.ToString())
                //{
                //    ShowMessage(71);
                //    e.Canceled = true;
                //    return;
                //}
            }

            SqlConnection conn11 = BusinessTier.getConnection();
            conn11.Open();
            int flg = BusinessTier.SaveStaff(conn11, txtCode.Text.ToString().Trim(), txtName.Text.ToString().Trim(), txtDesignation.Text.ToString().Trim(), txtLocation.Text.ToString().Trim(), Deptid, Warehid, txtPhone.Text.ToString().Trim(), txtEmail.Text.ToString().Trim(), Session["sesUserID"].ToString(), "U", Convert.ToInt32(lblID.Text.ToString().Trim()));
            BusinessTier.DisposeConnection(conn11);

            if (flg >= 1)
            {
                ShowMessage(23);
            }

            RadGrid1.Rebind();
            //InsertLogAuditTrial is used to insert the action into MasterLog table
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterStaff", "Update", "Success", "Log");

        }
        catch (Exception ex)
        {
            ShowMessage(5);
            e.Canceled = true;
            //InsertLogAuditTrial is used to insert the action into MasterAuditTrail table
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterStaff", "Update", ex.ToString(), "Audit");
        }

    }
    //----------------------------------------------------------------------------------
    private void ShowMessage(int errorNo)
    {
        lblStatus.Text = BusinessTier.g_ErrorMessagesDataTable.Rows[errorNo - 1]["Message"].ToString();
        System.Drawing.ColorConverter colConvert = new ColorConverter();
        string strColor = BusinessTier.g_ErrorMessagesDataTable.Rows[errorNo - 1]["Color"].ToString();
        lblStatus.ForeColor = (System.Drawing.Color)colConvert.ConvertFromString(strColor);
    }
    //----------------------------------------------------------------------------------
    private void InsertLogAuditTrail(string userid, string module, string activity, string result, string flag)
    {
        SqlConnection connLog = BusinessTier.getConnection();
        connLog.Open();
        BusinessTier.InsertLogAuditTrial(connLog, userid, module, activity, result, flag);
        BusinessTier.DisposeConnection(connLog);
    }
    //-----------------------------------------------------------------------------------
    protected void OnSelectedIndexcompany(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        RadComboBox combobox = (RadComboBox)sender;
        GridEditFormItem editedItem = (GridEditFormItem)combobox.NamingContainer;
        Label lblID = (Label)editedItem.FindControl("lblID");
        Label lblwname = (Label)editedItem.FindControl("lblwname");
        Label lblwerr = (Label)editedItem.FindControl("lblwerr");
        Label lbldname = (Label)editedItem.FindControl("lbldname");
        Label lblderr = (Label)editedItem.FindControl("lblderr");
        RadComboBox cbocompany = (RadComboBox)editedItem.FindControl("cboCompany");
        RadComboBox cbowarehouse = (RadComboBox)editedItem.FindControl("cbowarehouse");
        RadComboBox cboDept = (RadComboBox)editedItem.FindControl("cboDept");

        SqlConnection conn = BusinessTier.getConnection();
        conn.Open();
        int wid = 0;
        int did = 0;
        cbowarehouse.Text = "";
        cboDept.Text = "";
        try
        {
            if (cbocompany.SelectedValue.ToString() != "")
            {
                string sql1 = "select warehouseid,warehousename,companyid,deleted from warehouse where companyid ='" + cbocompany.SelectedValue.ToString() + "' and deleted=0";
                SqlDataAdapter adapter1 = new SqlDataAdapter(sql1, conn);
                adapter1.SelectCommand.Parameters.AddWithValue("@Text", e.Text);
                DataTable dataTable1 = new DataTable();
                adapter1.Fill(dataTable1);
                cbowarehouse.Items.Clear();
                foreach (DataRow row in dataTable1.Rows)
                {
                    lblwname.Visible = true;
                    lblwerr.Visible = true;
                    cbowarehouse.Visible = true;
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Text = row["WarehouseName"].ToString();
                    item.Value = row["WarehouseId"].ToString();
                    wid = Convert.ToInt32(row["WarehouseId"].ToString());
                    item.Attributes.Add("WarehouseName", row["WarehouseName"].ToString());
                    item.Attributes.Add("WarehouseId", row["WarehouseId"].ToString());
                    cbowarehouse.Items.Add(item);
                    item.DataBind();
                }
                BusinessTier.DisposeAdapter(adapter1);
                if (wid == 0)
                {
                    lblwname.Visible = false;
                    lblwerr.Visible = false;
                    cbowarehouse.Visible = false;
                }
            }
            if (cbocompany.SelectedValue.ToString() != "")
            {
                cboDept.Visible = true;
                lbldname.Visible = true;
                lblderr.Visible = true;
                string sql = "select deptid,deptcode,deptname,companyid,deleted from department where companyid ='" + cbocompany.SelectedValue.ToString() + "' and deleted=0";
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@Text", e.Text);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                cboDept.Items.Clear();
                foreach (DataRow row in dataTable.Rows)
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Text = row["deptname"].ToString();
                    item.Value = row["deptid"].ToString();
                    did = Convert.ToInt32(row["deptid"].ToString());
                    string dcode = row["deptcode"].ToString();
                    item.Attributes.Add("deptname", row["deptname"].ToString());
                    item.Attributes.Add("deptcode", row["deptcode"].ToString());
                    item.Attributes.Add("dcode", row["deptid"].ToString());
                    cboDept.Items.Add(item);
                    item.DataBind();

                }
                BusinessTier.DisposeAdapter(adapter);
                if (did == 0)
                {
                    cboDept.Visible = false;
                    lbldname.Visible = false;
                    lblderr.Visible = false;
                }
                BusinessTier.DisposeConnection(conn);
            }
        }
        catch (Exception ex)
        {
            BusinessTier.DisposeConnection(conn);
            lblStatus.Text = "Err on OnSelectedIndexcompany" + ex.Message.ToString();
            return;
        }
    }

}
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
public partial class Department : System.Web.UI.Page
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
                SqlCommand command = new SqlCommand("SELECT dbo.Company.CompanyName, dbo.Department.DeptId, dbo.Department.Deleted FROM dbo.Department INNER JOIN dbo.Company ON dbo.Department.CompanyID = dbo.Company.CompanyId where dbo.Department.Deleted=0 and dbo.Department.DeptId = '" + lbl.Text.ToString() + "'", conn);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    cbocompany.SelectedItem.Text = reader["companyname"].ToString();
                }
                BusinessTier.DisposeReader(reader);
                BusinessTier.DisposeConnection(conn);
            }
        }
    }
    protected void RadGrid1_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditFormItem editedItem = e.Item as GridEditFormItem;
                TextBox txtDeptCode = editedItem.FindControl("txtDeptCode") as TextBox;
                TextBox txtDeptName = editedItem.FindControl("txtDeptName") as TextBox;
                RadComboBox cbocompany = (RadComboBox)editedItem.FindControl("cbocompany");
                cbocompany.AutoPostBack = true;
            }
        }
        catch (Exception ex)
        {
            ShowMessage(8);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterDept", "RadGrid1_ItemCreated", ex.ToString(), "Audit");
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
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterDept", "NeedDataSource", ex.ToString(), "Audit");
        }
    }

    public DataTable DataSourceHelper()
    {
        SqlConnection conn = BusinessTier.getConnection();
        conn.Close();
        string sql = "SELECT dbo.Department.DeptId, dbo.Department.DeptCode, dbo.Department.DeptName, dbo.Department.CreatedBy, dbo.Company.CompanyName,dbo.Company.CompanyId FROM dbo.Department LEFT OUTER JOIN dbo.Company ON dbo.Department.CompanyID = dbo.Company.CompanyId WHERE  (dbo.Department.Deleted = 0)ORDER BY dbo.Department.DeptName";
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
            string ID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["DeptId"].ToString().Trim();
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            flg = BusinessTier.SaveDept(conn, "", "", 0, Session["sesUserID"].ToString(), "D", Convert.ToInt32(ID.ToString().Trim()));
            BusinessTier.DisposeConnection(conn);
            if (flg >= 1)
            {
                ShowMessage(21);
            }
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterDept", "Delete", "Success", "Log");
        }
        catch (Exception ex)
        {
            ShowMessage(6);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterDept", "Delete", ex.ToString(), "Audit");
        }
    }

    protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            TextBox txtDeptCode = (TextBox)editedItem.FindControl("txtDeptCode");
            TextBox txtDeptName = (TextBox)editedItem.FindControl("txtDeptName");
            RadComboBox cbocompany = (RadComboBox)editedItem.FindControl("cbocompany");
            Label lblcpyerr = (Label)editedItem.FindControl("lblcpyerr");
            if (cbocompany.SelectedValue.ToString() == "0")
            {
                lblStatus.Text = "Company is Mandatory";
                return;
            }
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            string deptid = "0";
            string deptname = "0";
            string strqry = "select deptid FROM department where deptname='" + txtDeptName.Text.ToString().Trim() + "' and companyid='" + cbocompany.SelectedValue.ToString().Trim() + "'   and Deleted=0";
            SqlCommand cmd = new SqlCommand(strqry, conn);

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                deptid = (reader["deptid"].ToString().Trim());
            }
            BusinessTier.DisposeReader(reader);

            string strqry1 = "select deptid FROM department where deptcode='" + txtDeptCode.Text.ToString().Trim() + "' and companyid='" + cbocompany.SelectedValue.ToString().Trim() + "'   and Deleted=0";
            SqlCommand cmd1 = new SqlCommand(strqry1, conn);
            SqlDataReader reader1 = cmd1.ExecuteReader();
            while (reader1.Read())
            {
                deptname = (reader1["deptid"].ToString().Trim());
            }
            BusinessTier.DisposeReader(reader1);



            if (deptid == "0" && deptname == "0")
            {
                int flg = BusinessTier.SaveDept(conn, txtDeptCode.Text.ToString().Trim(), txtDeptName.Text.ToString().Trim(), Convert.ToInt32(cbocompany.SelectedValue.ToString()), Session["sesUserID"].ToString(), "N", 0);
                if (flg >= 1)
                {
                    ShowMessage(19);
                }
            }
            else
            {
                if (deptid != "0")
                {
                    ShowMessage(47);
                    e.Canceled = true;
                    return;
                }
                else if (deptname != "0")
                {
                    ShowMessage(48);
                    e.Canceled = true;
                    return;
                }
            }
            BusinessTier.DisposeConnection(conn);
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterDept", "Insert", "Success", "Log");
        }
        catch (Exception ex)
        {
            ShowMessage(4);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterDept", "Insert", ex.ToString(), "Audit");
        }
        RadGrid1.Rebind();
    }


    protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            Label lblID = (Label)editedItem.FindControl("lblID");
            Label Lblcmpydummy = (Label)editedItem.FindControl("Lblcmpydummy");
            Label Lbldeptcode = (Label)editedItem.FindControl("Lbldeptcode");
            Label Lbldeptname = (Label)editedItem.FindControl("Lbldeptname");
            TextBox txtDeptCode = (TextBox)editedItem.FindControl("txtDeptCode");
            TextBox txtDeptName = (TextBox)editedItem.FindControl("txtDeptName");
            RadComboBox cbocompany = (RadComboBox)editedItem.FindControl("cbocompany");
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            int cmpnyid = 0;
            int cmpydumm = 0;
            string depid = "0";
            string deptname = "0";
            string strqry = "SELECT companyid,companyname from company where companyName='" + cbocompany.Text.ToString() + "'";
            SqlCommand cmd = new SqlCommand(strqry, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                cmpnyid = Convert.ToInt32(reader["companyid"].ToString().Trim());
            }
            BusinessTier.DisposeReader(reader);

            if (txtDeptCode.Text.ToString() != Lbldeptcode.Text.ToString())
            {
                string strqry11 = "select deptid,companyid FROM department where  deptcode='" + txtDeptCode.Text.ToString().Trim() + "'  and companyid='" + cmpnyid + "' and Deleted=0";
                SqlCommand cmd11 = new SqlCommand(strqry11, conn);
                SqlDataReader reader11 = cmd11.ExecuteReader();
                if (reader11.Read())
                {
                    depid = reader11["deptid"].ToString().Trim();
                    //cmpydumm = Convert.ToInt32(reader11["companyid"].ToString().Trim());
                }
                BusinessTier.DisposeReader(reader11);
                if (depid != "0")
                {
                    ShowMessage(48);
                    e.Canceled = true;
                    return;
                }
            }
            if (txtDeptName.Text.ToString() != Lbldeptname.Text.ToString())
            {
                string strqry1 = "select deptid FROM department where deptname='" + txtDeptName.Text.ToString().Trim() + "' and companyid='" + cmpnyid + "'   and Deleted=0";
                SqlCommand cmd1 = new SqlCommand(strqry1, conn);
                SqlDataReader reader1 = cmd1.ExecuteReader();
                while (reader1.Read())
                {
                    deptname = reader1["deptid"].ToString().Trim();
                }
                BusinessTier.DisposeReader(reader1);
                if (deptname != "0")
                {
                    ShowMessage(47);
                    e.Canceled = true;
                    return;
                }
            }
            if (cbocompany.Text.ToString() != Lblcmpydummy.Text.ToString())
            {
                string strqry111 = "select deptid,companyid FROM department where  deptcode='" + txtDeptCode.Text.ToString().Trim() + "'  and deptname='" + txtDeptName.Text.ToString().Trim() + "' and Deleted=0";
                SqlCommand cmd111 = new SqlCommand(strqry111, conn);
                SqlDataReader reader111 = cmd111.ExecuteReader();
                if (reader111.Read())
                {
                    cmpydumm = Convert.ToInt32(reader111["companyid"].ToString().Trim());
                }
                BusinessTier.DisposeReader(reader111);
                if (cmpydumm != 0)
                {
                    ShowMessage(63);
                    e.Canceled = true;
                    return;
                }
            }


            int flg = BusinessTier.SaveDept(conn, txtDeptCode.Text.ToString().Trim(), txtDeptName.Text.ToString().Trim(), cmpnyid, Session["sesUserID"].ToString(), "U", Convert.ToInt32(lblID.Text.ToString().Trim()));
            BusinessTier.DisposeConnection(conn);
            if (flg >= 1)
            {
                ShowMessage(20);
            }
            

            RadGrid1.Rebind();
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterDept", "Update", "Success", "Log");
        }
        catch (Exception ex)
        {
            ShowMessage(5);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterDept", "Update", ex.ToString(), "Audit");
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
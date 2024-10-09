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
using System.ComponentModel;
using System.Globalization;
using System.Collections;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.OleDb;
using System.Drawing;
using Telerik.Web.UI.Calendar;
using System.Web.SessionState;
using System.Runtime.Remoting.Contexts;
using System.Web.Script.Serialization;
using System.Net.Mail;


public partial class PPEIssue : System.Web.UI.Page
{
    protected BunnyBear.msgBox MsgBox1;// = new BunnyBear.msgBox();
    public DataTable dtMenuItems = new DataTable();
    public DataTable dtSubMenuItems = new DataTable();
    public static string stocklevel = "High";

    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            //Session["sesUserID"] = "1";
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
            if (IsPostBack)
            {
                if (Request.Form["hid_f"] == "1")
                {
                    Request.Form["hid_f"].Replace("1", "0");
                }

            }

            Session["Incdate"] = 0;
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
    //=====================================================================================================
    protected void RadGrid1_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditFormItem editedItem = e.Item as GridEditFormItem;
                RadNumericTextBox txtquantityissue = editedItem.FindControl("txtquantityissue") as RadNumericTextBox;
                RadComboBox cbostaffno = (RadComboBox)editedItem.FindControl("cbostaffno");
                RadComboBox cboprodstock = (RadComboBox)editedItem.FindControl("cboprodstock");
                RadComboBox cbproreceived = (RadComboBox)editedItem.FindControl("cbproreceived");
                RadDateTimePicker txtissuedate = editedItem.FindControl("txtissuedate") as RadDateTimePicker;
            }
        }
        catch (Exception ex)
        {
            ShowMessage(8);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "ppeissue", "RadGrid1_ItemCreated", ex.ToString(), "Audit");
        }

    }
    //--------------------------------------------------------------------------------------------
    protected void RadGrid1_ItemDataBound(object source, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditFormItem editedItem = e.Item as GridEditFormItem;
                Label lblID = (Label)editedItem.FindControl("lblID");
                Label txtminstock = (Label)editedItem.FindControl("txtminstock");
                Label lblUpdateQty = (Label)editedItem.FindControl("lblUpdateQty");

                Label Lblstock = (Label)editedItem.FindControl("Lblstock");
                RadComboBox cboSafetyBy = (RadComboBox)editedItem.FindControl("cboSafetyBy");
                RadComboBox cbostaffno = (RadComboBox)editedItem.FindControl("cbostaffno");
                cbostaffno.AutoPostBack = true;
                cbostaffno.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(OnSelectedIndexChangedStaff);

                RadComboBox cboprodstock = (RadComboBox)editedItem.FindControl("cboprodstock");
                cboprodstock.AutoPostBack = true;
                cboprodstock.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(OnSelectedIndexChangedproductreceived);

                RadComboBox cbproreceived = (RadComboBox)editedItem.FindControl("cbproreceived");
                cboprodstock.AutoPostBack = true;
                cboprodstock.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(OnSelectedIndexChangedIncomeQuantity);

                TextBox txtstaffname = (TextBox)editedItem.FindControl("txtstaffname");
                TextBox txtwarehouse = (TextBox)editedItem.FindControl("txtwarehouse");
                TextBox txtdepartment = (TextBox)editedItem.FindControl("txtdepartment");
                TextBox txtdesignation = (TextBox)editedItem.FindControl("txtdesignation");
                TextBox txtstafflocation = (TextBox)editedItem.FindControl("txtstafflocation");
                TextBox txttele = (TextBox)editedItem.FindControl("txttele");
                RadNumericTextBox txtquantityissue = (RadNumericTextBox)editedItem.FindControl("txtquantityissue");
                TextBox Txtincomeqty = (TextBox)editedItem.FindControl("Txtincomeqty");
                TextBox TxtDateqty = (TextBox)editedItem.FindControl("TxtDateqty");
                TextBox txtIssue = (TextBox)editedItem.FindControl("txtIssue");
                RadDateTimePicker txtissuedate = editedItem.FindControl("txtissuedate") as RadDateTimePicker;
                RadDateTimePicker dtSafetyDate = editedItem.FindControl("dtSafetyDate") as RadDateTimePicker;

                if (!(string.IsNullOrEmpty(lblID.Text.ToString())))
                {
                    SqlConnection conn = BusinessTier.getConnection();
                    conn.Open();
                    string strqry1 = "SELECT dbo.Products.ProductName, dbo.PPEissue.ID,dbo.PPEissue.deleted, dbo.Incoming.IncomingDate, dbo.PPEissue.ProductID, dbo.StaffMaster.StaffName FROM dbo.PPEissue INNER JOIN dbo.Products ON dbo.PPEissue.ProductID = dbo.Products.ID INNER JOIN dbo.Incoming ON dbo.Products.ID = dbo.Incoming.ProductID INNER JOIN dbo.StaffMaster ON dbo.PPEissue.StaffId = dbo.StaffMaster.StaffId WHERE  dbo.PPEissue.ID = '" + lblID.Text.ToString() + "' and dbo.PPEissue.deleted=0 ";
                    SqlCommand cmd11 = new SqlCommand(strqry1, conn);
                    SqlDataReader rdr11 = cmd11.ExecuteReader();
                    if (rdr11.Read())
                    {
                        cbostaffno.SelectedValue = rdr11["StaffName"].ToString().Trim();
                        cboprodstock.SelectedValue = rdr11["productName"].ToString().Trim();
                    }
                    BusinessTier.DisposeReader(rdr11);

                    string strqry = "SELECT * from VW_PPEissuedatabound WHERE  Deleted = 0 and id ='" + Convert.ToInt32(lblID.Text.ToString()) + "'";
                    SqlCommand cmd = new SqlCommand(strqry, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        TxtDateqty.Text = rdr["BalQty"].ToString().Trim();
                        txtIssue.Text = rdr["Issuance"].ToString().Trim();
                        cboSafetyBy.Text = rdr["SafetybyName"].ToString().Trim();
                        dtSafetyDate.SelectedDate = Convert.ToDateTime(rdr["SafetyDate"].ToString().Trim());
                    }
                    BusinessTier.DisposeReader(rdr);

                    string strqry11 = "SELECT SUM(dbo.Incoming.BalQty) AS Ttlqty FROM dbo.PPEissue INNER JOIN dbo.Incoming ON dbo.PPEissue.ProductID = dbo.Incoming.ProductID WHERE(dbo.PPEissue.Deleted = 0) AND dbo.PPEissue.ID  =  '" + lblID.Text.ToString() + "'";
                    SqlCommand cmd1 = new SqlCommand(strqry11, conn);
                    SqlDataReader rdr1 = cmd1.ExecuteReader();
                    if (rdr1.Read())
                    {
                        Txtincomeqty.Text = rdr1["Ttlqty"].ToString().Trim();
                    }
                    BusinessTier.DisposeReader(rdr1);
                    BusinessTier.DisposeConnection(conn);
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage(8);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "ppeissue", "RadGrid1_ItemDataBound", ex.ToString(), "Audit");
        }
    }

    //============================================================================================================
    protected void RadGrid1_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid1.DataSource = DataSourceHelper();
        }
        catch (Exception ex)
        {
            ShowMessage(9);
            //InsertLogAuditTrial is used to insert the action into MasterAuditTrail table
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "ppeissue", "NeedDataSource", ex.ToString(), "Audit");
        }
    }

    public DataTable DataSourceHelper()
    {
        SqlConnection conn = BusinessTier.getConnection();
        conn.Close();
        string sql = "SELECT * from VW_PPEissue where deleted=0 order by createdDate Desc";
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql, conn);
        DataTable g_datatable = new DataTable();
        sqlDataAdapter.Fill(g_datatable);
         BusinessTier.DisposeAdapter(sqlDataAdapter);
        BusinessTier.DisposeConnection(conn);
        return g_datatable;
    }
    //====================================================================================================
    protected void RadGrid1_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            int flg = 0;
            int IncomingID = 0;
            string ID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString().Trim();
            string dt = Convert.ToString(System.DateTime.Now);
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            int Delqty = 0;
            int BalQtyIssue = 0;
            int IncomeBalQty = 0;
            string strqry11 = "SELECT dbo.PPEissue.ID, dbo.PPEissue.IncomingID, dbo.PPEissue.Quantity, dbo.Incoming.BalQty FROM dbo.PPEissue INNER JOIN dbo.Incoming ON dbo.PPEissue.IncomingID = dbo.Incoming.ID where dbo.PPEissue.ID='" + ID.ToString() + "'";
            SqlCommand cmd11 = new SqlCommand(strqry11, conn);
            SqlDataReader rdr11 = cmd11.ExecuteReader();
            if (rdr11.Read())
            {
                IncomingID = Convert.ToInt32(rdr11["incomingid"].ToString().Trim());
                Delqty = Convert.ToInt32(rdr11["quantity"].ToString().Trim());
                IncomeBalQty = Convert.ToInt32(rdr11["BalQty"].ToString().Trim());
                BalQtyIssue = IncomeBalQty + Delqty;
            }
            BusinessTier.DisposeReader(rdr11);
            flg = BusinessTier.ppeissue(conn, 0, 0, 0, 0, Convert.ToDateTime(dt).ToShortDateString(), Convert.ToInt32(Session["sesUserID"].ToString()), "D", Convert.ToInt32(ID.ToString().Trim()), BalQtyIssue, IncomingID, Convert.ToDateTime(dt), 0, "0", 0, "0", 0, Convert.ToDateTime(dt).ToString());
            BusinessTier.DisposeConnection(conn);
            if (flg >= 1)
            {
                ShowMessage(61);
            }
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "ppeissue", "Delete", "Success", "Log");
        }
        catch (Exception ex)
        {
            ShowMessage(6);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterStaff", "Delete", ex.ToString(), "Audit");
        }
    }
    //-----------------------------------------------------------------------------------------------
    protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;

            Label LblExpiredate = (Label)editedItem.FindControl("LblExpiredate");
            Label txtminstock = (Label)editedItem.FindControl("txtminstock");
            Label Lblstock = (Label)editedItem.FindControl("Lblstock");
            Label lblavlissue = (Label)editedItem.FindControl("lblavlissue");
            Label lblavlstock = (Label)editedItem.FindControl("lblavlstock");
            TextBox txtdepartment = (TextBox)editedItem.FindControl("txtdepartment");
            TextBox txtdesignation = (TextBox)editedItem.FindControl("txtdesignation");
            TextBox txtstafflocation = (TextBox)editedItem.FindControl("txtstafflocation");
            TextBox txttele = (TextBox)editedItem.FindControl("txttele");
            TextBox txtproreceived = (TextBox)editedItem.FindControl("txtproreceived");
            RadNumericTextBox txtquantityissue = (RadNumericTextBox)editedItem.FindControl("txtquantityissue");
            TextBox txtstaffname = (TextBox)editedItem.FindControl("txtstaffname");
            TextBox txtwarehouse = (TextBox)editedItem.FindControl("txtwarehouse");
            TextBox Txtincomeqty = (TextBox)editedItem.FindControl("Txtincomeqty");
            TextBox TxtDateqty = (TextBox)editedItem.FindControl("TxtDateqty");

            RadComboBox cbproreceived = (RadComboBox)editedItem.FindControl("cbproreceived");
            RadComboBox cbostaffno = (RadComboBox)editedItem.FindControl("cbostaffno");
            RadComboBox cboprodstock = (RadComboBox)editedItem.FindControl("cboprodstock");

            RadDateTimePicker txtissuedate = editedItem.FindControl("txtissuedate") as RadDateTimePicker;

            TextBox txtIssue = (TextBox)editedItem.FindControl("txtIssue");
            RadComboBox cboSafetyBy = (RadComboBox)editedItem.FindControl("cboSafetyBy");
            RadDateTimePicker dtSafetyDate = editedItem.FindControl("dtSafetyDate") as RadDateTimePicker;

            //--------------------------------------------------------
            if (string.IsNullOrEmpty(cbostaffno.SelectedValue.ToString()))
            {
                ShowMessage(35);
                e.Canceled = true;
                return;
            }
            if (string.IsNullOrEmpty(cboprodstock.SelectedValue.ToString()))
            {
                ShowMessage(36);
                e.Canceled = true;
                return;
            }
            if (string.IsNullOrEmpty(cbproreceived.SelectedValue.ToString()))
            {
                ShowMessage(37);
                e.Canceled = true;
                return;
            }
            if (string.IsNullOrEmpty(txtquantityissue.Text.ToString()) || txtquantityissue.Text.ToString() == "0")
            {
                ShowMessage(38);
                e.Canceled = true;
                return;
            }

            String IncomingDate;
            if (!(string.IsNullOrEmpty(txtissuedate.SelectedDate.ToString())))
            {
                IncomingDate = txtissuedate.SelectedDate.ToString();
                DateTime dtinsDate = DateTime.Parse(IncomingDate);
                IncomingDate = dtinsDate.Month + "/" + dtinsDate.Day + "/" + dtinsDate.Year + " 00:00:00";

                //IncomingDate = Convert.ToDateTime(txtissuedate.SelectedDate.Value.ToString());
            }
            else
            {
                ShowMessage(39);
                e.Canceled = true;
                return;
            }

            string SafetyBriefDate = string.Empty;
            if (!(string.IsNullOrEmpty(dtSafetyDate.SelectedDate.ToString())))
            {
                SafetyBriefDate = dtSafetyDate.SelectedDate.ToString();
                DateTime dtinsDate = DateTime.Parse(SafetyBriefDate);
                SafetyBriefDate = dtinsDate.Month + "/" + dtinsDate.Day + "/" + dtinsDate.Year + " 00:00:00";
                //SafetyBriefDate = Convert.ToDateTime(dtSafetyDate.SelectedDate.Value.ToString());
            }
            else
            {
                ShowMessage(73);
                e.Canceled = true;
                return;
            }

            //--------------------------------------------------------
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            int BalQtyIssue = 0;//less qty issue value
            int IncomingID = 0; //Take Incoming ID
            //(((((((((((((((((((((((((deptid))))))))))))))))))))))))
            int Deptid = 0;
            string strqry = "SELECT deptid,deptname from department where deptname='" + txtdepartment.Text.ToString() + "'";
            SqlCommand cmd = new SqlCommand(strqry, conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                Deptid = Convert.ToInt32(rdr["deptid"].ToString().Trim());
            }
            BusinessTier.DisposeReader(rdr);
            //(((((((((((((((((((((((((quantity Calculation))))))))))))))))))))))))
            SqlCommand command = new SqlCommand("SELECT dbo.Incoming.Quantity,dbo.Incoming.ID AS IncomingID,dbo.Products.MinimumStock,dbo.Incoming.BalQty, dbo.Incoming.Expireddate, dbo.Incoming.IncomingDate, dbo.Products.ID,dbo.Incoming.Deleted FROM dbo.Incoming INNER JOIN dbo.Products ON dbo.Incoming.ProductID = dbo.Products.ID where dbo.Incoming.ID='" + Convert.ToInt32(cbproreceived.SelectedValue) + "' and dbo.Products.ID='" + Convert.ToInt32(cboprodstock.SelectedValue.ToString()) + "' and dbo.Incoming.Deleted=0", conn);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                txtminstock.Text = reader["MinimumStock"].ToString().Trim();
                IncomingID = Convert.ToInt32(reader["IncomingID"].ToString().Trim());
                LblExpiredate.Text =  reader["ExpiredDate"].ToString().Trim();
            }
            int stock = (Convert.ToInt32(Txtincomeqty.Text.ToString()) - Convert.ToInt32(txtminstock.Text.ToString()));
            Lblstock.Text = stock.ToString();
            BusinessTier.DisposeReader(reader);

            if (Convert.ToInt32(TxtDateqty.Text.ToString()) >= Convert.ToInt32(txtquantityissue.Text.ToString()) && Convert.ToInt32(stock) >= Convert.ToInt32(txtquantityissue.Text.ToString()))
            {
                BalQtyIssue = (Convert.ToInt32(TxtDateqty.Text.ToString()) - Convert.ToInt32(txtquantityissue.Text.ToString()));
            }
            if (Convert.ToInt32(TxtDateqty.Text.ToString()) <= Convert.ToInt32(txtquantityissue.Text.ToString()) && Convert.ToInt32(stock) <= Convert.ToInt32(txtquantityissue.Text.ToString()))
            {
                lblavlissue.Visible = true;
                lblavlstock.Visible = true;
                lblavlstock.Text = stock.ToString();
                ShowMessage(58);
                e.Canceled = true;
                return;
                //stocklevel = "low";
                //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Confirm", "Confirm()", true);
                //string confirmValue = Request.Form["confirm_value"];

                //if (confirmValue.Substring((confirmValue.Length - 1)) == "s")
                //{
                //    BalQtyIssue = (Convert.ToInt32(TxtDateqty.Text.ToString()) - Convert.ToInt32(txtquantityissue.Text.ToString()));
                //}
                //else
                //{
                //    lblStatus.Text = "Try to another product item";
                //    e.Canceled = true;
                //    return;
                //}
            }
            int flg = BusinessTier.ppeissue(conn, Convert.ToInt32(cbostaffno.SelectedValue.ToString().Trim()), Convert.ToInt32(cboprodstock.SelectedValue.ToString().Trim()), Deptid, Convert.ToInt32(txtquantityissue.Text.ToString().Trim()), IncomingDate, Convert.ToInt32(Session["sesUserID"].ToString()), "N", 0, BalQtyIssue, Convert.ToInt32(cbproreceived.SelectedValue), Convert.ToDateTime(LblExpiredate.Text.ToString()), 0, "0", 0, txtIssue.Text.ToString(), Convert.ToInt32(cboSafetyBy.SelectedValue.ToString().Trim()),SafetyBriefDate);
            BusinessTier.DisposeConnection(conn);
            if (flg >= 1)
            {
                ShowMessage(59);
                return;
            }

            InsertLogAuditTrail(Session["sesUserID"].ToString(), "ppeissue", "Insert", "Success", "Log");
        }
        catch (Exception ex)
        {
            ShowMessage(4);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "ppeissue", "Insert", ex.ToString(), "Audit");
        }
        RadGrid1.Rebind();
    }
    //---------------------------------------------------------------------------------------------------------------------
    [System.Web.Services.WebMethod(EnableSession = false)]

    protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            Label lblID = (Label)editedItem.FindControl("lblID");
            Label LblExpiredate = (Label)editedItem.FindControl("LblExpiredate");
            Label lblUpdateQty = (Label)editedItem.FindControl("lblUpdateQty");

            TextBox txtdepartment = (TextBox)editedItem.FindControl("txtdepartment");
            TextBox txtdesignation = (TextBox)editedItem.FindControl("txtdesignation");
            TextBox txtstafflocation = (TextBox)editedItem.FindControl("txtstafflocation");
            TextBox txttele = (TextBox)editedItem.FindControl("txttele");
            TextBox txtproreceived = (TextBox)editedItem.FindControl("txtproreceived");
            RadNumericTextBox txtquantityissue = (RadNumericTextBox)editedItem.FindControl("txtquantityissue");
            TextBox txtstaffname = (TextBox)editedItem.FindControl("txtstaffname");
            TextBox txtwarehouse = (TextBox)editedItem.FindControl("txtwarehouse");
            TextBox Txtincomeqty = (TextBox)editedItem.FindControl("Txtincomeqty");
            RadComboBox cbproreceived = (RadComboBox)editedItem.FindControl("cbproreceived");
            RadComboBox cbostaffno = (RadComboBox)editedItem.FindControl("cbostaffno");
            RadComboBox cboprodstock = (RadComboBox)editedItem.FindControl("cboprodstock");
            TextBox TxtDateqty = (TextBox)editedItem.FindControl("TxtDateqty");
            Label txtminstock = (Label)editedItem.FindControl("txtminstock");
            Label Lbldupdate = (Label)editedItem.FindControl("Lbldupdate");
            RadDateTimePicker txtissuedate = editedItem.FindControl("txtissuedate") as RadDateTimePicker;
            string IssueDate = string.Empty;

            TextBox txtIssue = (TextBox)editedItem.FindControl("txtIssue");
            RadComboBox cboSafetyBy = (RadComboBox)editedItem.FindControl("cboSafetyBy");
            RadDateTimePicker dtSafetyDate = editedItem.FindControl("dtSafetyDate") as RadDateTimePicker;

            if (!(string.IsNullOrEmpty(txtissuedate.SelectedDate.ToString())))
            {

                IssueDate = txtissuedate.SelectedDate.ToString();
                DateTime dtinsDate = DateTime.Parse(IssueDate);
                IssueDate = dtinsDate.Month + "/" + dtinsDate.Day + "/" + dtinsDate.Year + " 00:00:00";

                //IssueDate = Convert.ToDateTime(txtissuedate.SelectedDate.Value.ToString());
            }
            else
            {
                lblStatus.Text = "Please Choose the Issue Date";
                return;
            }

            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            int Deptid = 0;
            int prodid = 0;
            int staffid = 0;
            int BalQtyIssue = 0;//less qty issue value
            int IncomingID = 0; //Take Incoming ID
            int IncomingIDDD = 0;// Diff date incoming id update calc
            int UpdateBalQty = 0;//Update final qty to income tbl
            string updateflag = "0";
            if (string.IsNullOrEmpty(cbostaffno.SelectedValue.ToString()))
            {
                ShowMessage(35);
                e.Canceled = true;
                return;
            }

            if (string.IsNullOrEmpty(txtquantityissue.Text.ToString()) || txtquantityissue.Text.ToString() == "0")
            {
                ShowMessage(38);
                e.Canceled = true;
                return;
            }


            DateTime IncomingDate;
            if (!(string.IsNullOrEmpty(txtissuedate.SelectedDate.ToString())))
            {
                //IncomingDate = txtissuedate.SelectedDate.ToString();
                //DateTime dtinsDate = DateTime.Parse(IncomingDate);
                //IncomingDate = dtinsDate.Month + "/" + dtinsDate.Day + "/" + dtinsDate.Year + " 00:00:00";

                IncomingDate = Convert.ToDateTime(txtissuedate.SelectedDate.Value.ToString());
            }
            else
            {
                ShowMessage(39);
                e.Canceled = true;
                return;
            }
            string SafetyBriefDate=string.Empty;
            if (!(string.IsNullOrEmpty(dtSafetyDate.SelectedDate.ToString())))
            {
                SafetyBriefDate = dtSafetyDate.SelectedDate.ToString();
                DateTime dtinsDate = DateTime.Parse(SafetyBriefDate);
                SafetyBriefDate = dtinsDate.Month + "/" + dtinsDate.Day + "/" + dtinsDate.Year + " 00:00:00";
                //SafetyBriefDate = Convert.ToDateTime(dtSafetyDate.SelectedDate.Value.ToString());
            }
            else
            {
                ShowMessage(73);
                e.Canceled = true;
                return;
            }

            if (txtquantityissue.Text.ToString() == "0")
            {
                ShowMessage(38);
                e.Canceled = true;
                return;
            }
            //(((((((((((((((((((((((((quantiy))))))))))))))))))))))))
            string strqry = " SELECT id,productname from products where productname='" + cboprodstock.Text.ToString() + "'";
            SqlCommand cmd = new SqlCommand(strqry, conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                prodid = Convert.ToInt32(rdr["id"].ToString().Trim());
            }
            BusinessTier.DisposeReader(rdr);
            //(((((((((((((((((((((((((Staff ID))))))))))))))))))))))))
            string strqry11 = " SELECT staffid,staffname from staffmaster where staffname='" + cbostaffno.Text.ToString() + "'";
            SqlCommand cmd11 = new SqlCommand(strqry11, conn);
            SqlDataReader rdr11 = cmd11.ExecuteReader();
            if (rdr11.Read())
            {
                staffid = Convert.ToInt32(rdr11["staffid"].ToString().Trim());
            }
            BusinessTier.DisposeReader(rdr11);
            //(((((((((((((((((((((((((SafetyBrief ID))))))))))))))))))))))))
            int SafetyBy = 0;
            string strqry111 = " SELECT staffid,staffname from staffmaster where staffname='" + cboSafetyBy.Text.ToString() + "'";
            SqlCommand cmd111 = new SqlCommand(strqry111, conn);
            SqlDataReader rdr111 = cmd111.ExecuteReader();
            if (rdr111.Read())
            {
                SafetyBy = Convert.ToInt32(rdr111["staffid"].ToString().Trim());
            }
            BusinessTier.DisposeReader(rdr111);

            //(((((((((((((((((((((((((Dept ID))))))))))))))))))))))))
            string strqry12 = " SELECT deptid,deptname from department where deptname='" + txtdepartment.Text.ToString() + "'";
            SqlCommand cmd21 = new SqlCommand(strqry12, conn);
            SqlDataReader rdr12 = cmd21.ExecuteReader();
            if (rdr12.Read())
            {
                Deptid = Convert.ToInt32(rdr12["deptid"].ToString().Trim());
            }
            BusinessTier.DisposeReader(rdr12);
            //(((((((((((((((((((((((((quantiy calc update))))))))))))))))))))))))
            SqlCommand command = new SqlCommand("SELECT dbo.PPEissue.ID, dbo.PPEissue.Quantity, dbo.PPEissue.IncomingID, dbo.PPEissue.Deleted, dbo.Products.MinimumStock FROM dbo.PPEissue INNER JOIN dbo.Products ON dbo.PPEissue.ProductID = dbo.Products.ID WHERE dbo.PPEissue.Deleted =0 and dbo.PPEissue.ID='" + lblID.Text.ToString() + "'", conn);
            SqlDataReader rdr13 = command.ExecuteReader();
            if (rdr13.Read())
            {
                txtminstock.Text = rdr13["MinimumStock"].ToString().Trim();
                IncomingID = Convert.ToInt32(rdr13["IncomingID"].ToString().Trim());
            }

            int stock = (Convert.ToInt32(Txtincomeqty.Text.ToString()) - Convert.ToInt32(txtminstock.Text.ToString()));
            int samedatestock = ((Convert.ToInt32(Txtincomeqty.Text.ToString()) + Convert.ToInt32(lblUpdateQty.Text.ToString())) - Convert.ToInt32(txtminstock.Text.ToString()));
            int updatestock = (Convert.ToInt32(lblUpdateQty.Text.ToString()) + Convert.ToInt32(TxtDateqty.Text.ToString()));
            BusinessTier.DisposeReader(rdr13);
            //''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            if (Convert.ToDateTime(cbproreceived.Text) == Convert.ToDateTime(Lbldupdate.Text))
            {
                updateflag = "updatef";
                if (Convert.ToInt32(updatestock) >= Convert.ToInt32(txtquantityissue.Text.ToString()) && Convert.ToInt32(samedatestock) >= Convert.ToInt32(txtquantityissue.Text.ToString())) //
                {
                    if (Convert.ToInt32(lblUpdateQty.Text.ToString()) >= Convert.ToInt32(txtquantityissue.Text.ToString()))
                    {
                        UpdateBalQty = Convert.ToInt32(lblUpdateQty.Text.ToString()) - Convert.ToInt32(txtquantityissue.Text.ToString());
                        BalQtyIssue = Convert.ToInt32(TxtDateqty.Text.ToString()) + UpdateBalQty;
                    }
                    else
                    {
                        UpdateBalQty = Convert.ToInt32(txtquantityissue.Text.ToString()) - Convert.ToInt32(lblUpdateQty.Text.ToString());
                        BalQtyIssue = Convert.ToInt32(TxtDateqty.Text.ToString()) - UpdateBalQty;
                    }
                }
                else
                {
                    ShowMessage(58);
                    e.Canceled = true;
                    return;
                }
            }
            else
            {
                IncomingIDDD = Convert.ToInt32(cbproreceived.SelectedValue);
                //SqlCommand command14 = new SqlCommand("SELECT IncomingID from ppeissue where ID='" + Convert.ToInt32(lblID.Text.ToString().Trim()) + "'",  conn);
                //SqlDataReader rdr14 = command14.ExecuteReader();
                //if (rdr14.Read())
                //{
                //    IncomingIDDD = Convert.ToInt32(rdr14["IncomingID"].ToString().Trim());
                //}
                //BusinessTier.DisposeReader(rdr14);
                updateflag = "updateflag";
                {
                    if (Convert.ToInt32(TxtDateqty.Text.ToString()) >= Convert.ToInt32(txtquantityissue.Text.ToString()) && Convert.ToInt32(stock) >= Convert.ToInt32(txtquantityissue.Text.ToString()))
                    {
                        BalQtyIssue = (Convert.ToInt32(TxtDateqty.Text.ToString()) - Convert.ToInt32(txtquantityissue.Text.ToString()));
                    }
                    else
                    {
                        ShowMessage(58);
                        e.Canceled = true;
                        return;
                    }
                }
            }
            int flg = BusinessTier.ppeissue(conn, staffid, prodid, Deptid, Convert.ToInt32(txtquantityissue.Text.ToString()), IssueDate.ToString(), Convert.ToInt32(Session["sesUserID"].ToString()), "U", Convert.ToInt32(lblID.Text.ToString()), BalQtyIssue, IncomingID, Convert.ToDateTime(LblExpiredate.Text.ToString()), IncomingIDDD, updateflag, Convert.ToInt32(lblUpdateQty.Text), txtIssue.Text.ToString(), SafetyBy, SafetyBriefDate.ToString());
            BusinessTier.DisposeConnection(conn);
            if (flg >= 1)
            {
                ShowMessage(60);
            }
            else
            {
                ShowMessage(58);
                e.Canceled = true;
                return;
            }

            RadGrid1.Rebind();
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterStaff", "Update", "Success", "Log");
        }
        catch (Exception ex)
        {
            ShowMessage(5);
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterStaff", "Update", ex.ToString(), "Audit");
        }

    }
    //====================================================================================================
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
    //==========================================Staff On ITEM REQ======================================
    protected void staffcode_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        SqlConnection conn = BusinessTier.getConnection();
        conn.Open();
        try
        {
            string sql1 = "select staffid,staffno,staffname from staffmaster where DELETED=0   order by staffno";
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
                item.Attributes.Add("staffno", row["staffno"].ToString());
                comboBox.Items.Add(item);
                item.DataBind();
            }
            BusinessTier.DisposeAdapter(adapter1);
            BusinessTier.DisposeConnection(conn);
        }
        catch (Exception ex)
        {
            BusinessTier.DisposeConnection(conn);
            lblStatus.Text = "Err:staffcode_OnItemsRequested" + ex.Message.ToString();
            return;
        }
    }

    protected void RadGrid1_ItemCommand(object source, GridCommandEventArgs e)
    {
        RadToolTipManager1.TargetControls.Clear();
    }

    //------------------------------------------------------staff DATas
    protected void OnSelectedIndexChangedStaff(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        RadComboBox combobox = (RadComboBox)sender;
        GridEditFormItem editedItem = (GridEditFormItem)combobox.NamingContainer;
        Label lblID = (Label)editedItem.FindControl("lblID");
        RadComboBox cbostaffno = (RadComboBox)editedItem.FindControl("cbostaffno");
        TextBox txtstaffname = (TextBox)editedItem.FindControl("txtstaffname");
        TextBox txtwarehouse = (TextBox)editedItem.FindControl("txtwarehouse");
        TextBox txtdepartment = (TextBox)editedItem.FindControl("txtdepartment");
        TextBox txtdesignation = (TextBox)editedItem.FindControl("txtdesignation");
        TextBox txtstafflocation = (TextBox)editedItem.FindControl("txtstafflocation");
        TextBox txttele = (TextBox)editedItem.FindControl("txttele");
        SqlConnection conn = BusinessTier.getConnection();
        conn.Open();
        try
        {
            if (cbostaffno.SelectedValue.ToString() != "")
            {
                SqlCommand command = new SqlCommand("select * from VW_PPEstaffdet where staffid='" + cbostaffno.SelectedValue.ToString() + "'", conn); ;
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    //txtstaffname.Text = reader["staffname"].ToString();
                    txtdepartment.Text = reader["deptname"].ToString();
                    txtwarehouse.Text = reader["warehousename"].ToString();
                    txtdesignation.Text = reader["designation"].ToString();
                    txtstafflocation.Text = reader["location"].ToString();
                    txttele.Text = reader["phone"].ToString();
                }
                BusinessTier.DisposeReader(reader);
            }
            BusinessTier.DisposeConnection(conn);
        }
        catch (Exception ex)
        {
            BusinessTier.DisposeConnection(conn);
            lblStatus.Text = "Err:Select Correct staff id" + ex.Message.ToString();
            return;
        }
    }

    //===============product request and onselectindexchange======================================

    protected void productcodecode_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        SqlConnection conn = BusinessTier.getConnection();
        conn.Open();
        try
        {
            string sql1 = "select distinct(productid),productname,productcode from VW_IncomeDataSource where deleted=0 and balqty<>0";
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
                item.Value = row["productid"].ToString();
                int ppid = Convert.ToInt32(row["productid"].ToString());
                string pcode = row["productcode"].ToString();
                item.Attributes.Add("productname", row["productname"].ToString());
                item.Attributes.Add("productcode", row["productcode"].ToString());
                item.Attributes.Add("productid", row["productid"].ToString());
                comboBox.Items.Add(item);
                item.DataBind();
            }
            BusinessTier.DisposeAdapter(adapter1);
            BusinessTier.DisposeConnection(conn);
        }
        catch (Exception ex)
        {
            BusinessTier.DisposeConnection(conn);
            lblStatus.Text = "Err:Select Correct Product Item" + ex.Message.ToString();
            return;
        }
    }
    //--------------------------------------

    protected void OnSelectedIndexChangedproductreceived(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        RadComboBox combobox = (RadComboBox)sender;
        GridEditFormItem editedItem = (GridEditFormItem)combobox.NamingContainer;
        Label lblID = (Label)editedItem.FindControl("lblID");
        Label txtminstock = (Label)editedItem.FindControl("txtminstock");
        RadComboBox cboprodstock = (RadComboBox)editedItem.FindControl("cboprodstock");
        RadComboBox cbproreceived = (RadComboBox)editedItem.FindControl("cbproreceived");
        TextBox Txtincomeqty = (TextBox)editedItem.FindControl("Txtincomeqty");
        Label Lbldupdate = (Label)editedItem.FindControl("Lbldupdate");
        RadNumericTextBox txtquantityissue = (RadNumericTextBox)editedItem.FindControl("txtquantityissue");
        SqlConnection conn = BusinessTier.getConnection();
        conn.Open();
        try
        {
            string sql1 = "SELECT dbo.Incoming.ID,dbo.Incoming.IncomingDate,dbo.Incoming.BalQty,dbo.Incoming.Quantity, dbo.Products.ID,dbo.Products.MinimumStock, dbo.Incoming.Deleted, dbo.Products.ProductCode FROM dbo.Incoming INNER JOIN dbo.Products ON dbo.Incoming.ProductID = dbo.Products.ID WHERE  dbo.Products.ID='" + cboprodstock.SelectedValue.ToString() + "' and dbo.Incoming.Deleted = 0 order by incomingdate";
            SqlDataAdapter adapter1 = new SqlDataAdapter(sql1, conn);
            adapter1.SelectCommand.Parameters.AddWithValue("@Text", e.Text);
            DataTable dataTable1 = new DataTable();
            adapter1.Fill(dataTable1);
            cbproreceived.Items.Clear();
            foreach (DataRow row in dataTable1.Rows)
            {
                RadComboBoxItem item = new RadComboBoxItem();

                item.Text = row["BalQty"].ToString();
                item.Text = row["IncomingDate"].ToString();
                item.Value = row["ID"].ToString();
                int balqty = Convert.ToInt32(row["BalQty"].ToString());
                if (balqty != 0)
                {
                    item.Attributes.Add("BalQty", row["BalQty"].ToString());
                    item.Attributes.Add("IncomingDate", row["IncomingDate"].ToString());
                    // Lbldupdate.Text=(row["IncomingDate"].ToString());

                    cbproreceived.Items.Add(item);
                }
                item.DataBind();
            }
            BusinessTier.DisposeAdapter(adapter1);

            SqlCommand cmd = new SqlCommand(sql1, conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                txtminstock.Text = rdr["minimumstock"].ToString().Trim();
            }
            BusinessTier.DisposeReader(rdr);

            string strqry = "select sum(balqty)as TtlQty from incoming  where productid='" + cboprodstock.SelectedValue.ToString() + "' and deleted=0 GROUP BY productid";
            SqlCommand cmd1 = new SqlCommand(strqry, conn);
            SqlDataReader rdr1 = cmd1.ExecuteReader();
            if (rdr1.Read())
            {
               Txtincomeqty.Text = rdr1["TtlQty"].ToString().Trim();
            }
            BusinessTier.DisposeReader(rdr1);
            BusinessTier.DisposeConnection(conn);
        }
        catch (Exception ex)
        {
            BusinessTier.DisposeConnection(conn);
            lblStatus.Text = "Err:Select correct Product Item" + ex.Message.ToString();
            return;
        }
    }
    //-----------------------------------------------------------------------------
    protected void OutgoingSelectDate_SelectedDateChanged(object sender, SelectedDateChangedEventArgs e)
    {
        RadDateTimePicker radpicker = (RadDateTimePicker)sender;
        GridEditFormItem editedItem = (GridEditFormItem)radpicker.NamingContainer;
        Label lblID = (Label)editedItem.FindControl("lblID");
        RadComboBox cbproreceived = (RadComboBox)editedItem.FindControl("cbproreceived");
        RadComboBox cboprodstock = (RadComboBox)editedItem.FindControl("cboprodstock");
        RadDateTimePicker txtissuedate = editedItem.FindControl("txtissuedate") as RadDateTimePicker;
        SqlConnection conn = BusinessTier.getConnection();
        conn.Open();
        try
        {
            if (string.IsNullOrEmpty(cbproreceived.Text.ToString()))
            {
                lblStatus.Text = "Select Received Product";
                return;
            }
            DateTime textdate = Convert.ToDateTime(txtissuedate.SelectedDate.ToString());
            DateTime Reproductdate = Convert.ToDateTime(cbproreceived.Text.ToString());
            string tdate = textdate.Month + "/" + textdate.Day + "/" + textdate.Year;
            string producdate = Reproductdate.Month + "/" + Reproductdate.Day + "/" + Reproductdate.Year;
            if (Convert.ToDateTime(tdate) < Convert.ToDateTime(producdate))
            {
                txtissuedate.DbSelectedDate = "";
                lblStatus.Text = "Select Max Date from Received Products";
                return;
            }
            BusinessTier.DisposeConnection(conn);
        }
        catch (Exception ex)
        {
            BusinessTier.DisposeConnection(conn);
            //lblStatus.Text = "Err : OnSelectedIndexChangedproductreceived : " + ex.Message.ToString();
            return;
        }

    }
    //-------------------------------------------------
    protected void OnSelectedIndexChangedIncomeQuantity(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try
        {
            RadComboBox combobox = (RadComboBox)sender;
            GridEditFormItem editedItem = (GridEditFormItem)combobox.NamingContainer;
            Label lblID = (Label)editedItem.FindControl("lblID");
            RadComboBox cboprodstock = (RadComboBox)editedItem.FindControl("cboprodstock");
            RadComboBox cbproreceived = (RadComboBox)editedItem.FindControl("cbproreceived");
            TextBox Txtincomeqty = (TextBox)editedItem.FindControl("Txtincomeqty");
            TextBox TxtDateqty = (TextBox)editedItem.FindControl("TxtDateqty");
            Label txtminstock = (Label)editedItem.FindControl("txtminstock");
            Label Lbldupdate = (Label)editedItem.FindControl("Lbldupdate");
            RadNumericTextBox txtquantityissue = (RadNumericTextBox)editedItem.FindControl("txtquantityissue");

            //txtquantityissue.Text = cbproreceived.SelectedValue;
            //TxtDateqty.Text = cbproreceived.SelectedValue;
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();

            // string strqry = "SELECT dbo.Incoming.Quantity,dbo.Incoming.BalQty, dbo.Incoming.IncomingDate, dbo.Products.ID,dbo.Incoming.Deleted FROM dbo.Incoming INNER JOIN dbo.Products ON dbo.Incoming.ProductID = dbo.Products.ID where dbo.Incoming.IncomingDate='" + Convert.ToDateTime(cbproreceived.SelectedItem.Text.ToString()) + "' and dbo.Products.ID='" + Convert.ToInt32(cboprodstock.SelectedValue.ToString()) + "' and dbo.Incoming.Deleted=0";
            string strqry = "SELECT BalQty FROM Incoming where ID ='" + cbproreceived.SelectedValue + "' and Deleted=0";
            SqlCommand cmd = new SqlCommand(strqry, conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                //txtquantityissue.Text = rdr["BalQty"].ToString().Trim();
                TxtDateqty.Text = rdr["BalQty"].ToString().Trim();
                //Lbldupdate.Text = rdr["IncomingDate"].ToString().Trim();
            }
            BusinessTier.DisposeReader(rdr);
            //if (Convert.ToInt32(txtminstock.Text.ToString()) == Convert.ToInt32(txtquantityissue.Text.ToString()))
            //{
            //    lblStatus.Text = "Not Issue Because Issue Quantity is minimum stock";
            //    return;
            //}
            BusinessTier.DisposeConnection(conn);
        }
        catch (Exception ex)
        {
            // BusinessTier.DisposeConnection(conn);
            lblStatus.Text = "Err:Select correct Received products" + ex.Message.ToString();
            return;
        }
    }
    //===============================================================================
    protected void RecDate_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {

    }
}

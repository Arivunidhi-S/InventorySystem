using System;
using System.Collections.Generic;
using System.Linq;
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
using Stimulsoft.Report;
using Stimulsoft.Report.Web;
using Stimulsoft.Report.Viewer;
using Stimulsoft.Report.SaveLoad;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.Print;
using Stimulsoft.Base;
using Stimulsoft.Controls;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Controls;
using System.IO;
using System.Web.SessionState;
using System.Runtime;

public partial class ReportsPPEI : System.Web.UI.Page
{
    public string query;

    public DataTable dtMenuItems = new DataTable();

    public DataTable dtSubMenuItems = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        lblStatus.Text = "";
        try
        {
            //if ((string.IsNullOrEmpty(Session["sesUserID"].ToString())))
            //{
            //    Response.Redirect("Login.aspx");
            //}
            //else if (!Page.IsPostBack)
            //{
            //    int val = int.Parse(DateTime.Now.Year.ToString()) - 1;
            //    string strDayValue = ((Convert.ToInt32(DateTime.Now.Day)) - val).ToString();
            //    //string strDate = DateTime.Now.Month.ToString() + "/" + strDayValue.ToString() + "/" + DateTime.Now.Year.ToString() + " 12:00:01 AM";
            //    string strDate = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + val.ToString() + " 12:00:01 AM";
            //    txtFromDate.SelectedDate = Convert.ToDateTime(strDate);
            //    txtToDate.SelectedDate = DateTime.Now;

            //    //Session["FromDate"] = Convert.ToDateTime(strDate);
            //    //Session["ToDate"] = DateTime.Now;

            //    Session["sesUserID"] = "1";
            //    Session["Report"] = "DepartmentwiseReportByPrice";
            //    Session["StaffId"] = "";
            //    Session["ProdId"] = "";
            //    Session["SuppId"] = "";
            //    Session["DeptId"] = "";
            //}
        }
        catch (Exception ex1)
        {
            Response.Redirect("Login.aspx");

        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {

        lblStatus.Text = "";
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
                //int val = int.Parse(DateTime.Now.Year.ToString()) - 1;
                //string strDayValue = ((Convert.ToInt32(DateTime.Now.Day)) - val).ToString();
                ////string strDate = DateTime.Now.Month.ToString() + "/" + strDayValue.ToString() + "/" + DateTime.Now.Year.ToString() + " 12:00:01 AM";
                //string strDate = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + val.ToString() + " 12:00:01 AM";
                //txtFromDate.SelectedDate = Convert.ToDateTime(strDate);
                //txtToDate.SelectedDate = DateTime.Now;
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

    protected void hidden()
    {
        Stimulsoft.Report.StiReport stiReport1;
        stiReport1 = new StiReport();
        stiReport1.Dictionary.DataStore.Clear();
        txtStaffId.Enabled = false;
        txtProdCode.Enabled = false;
        txtSuppName.Enabled = false;
        txtDeptName.Enabled = false;
        txtFromDate.Enabled = false;
        txtToDate.Enabled = false;
        txtWarehouse.Enabled = false;
    }

    protected void OnSelectedCompo(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {

        if (txtReport.SelectedValue.ToString() == "DepartmentwiseReportByPrice")
        {
            hidden();
            txtDeptName.Enabled = true;
        }
        else if (txtReport.SelectedValue.ToString() == "MinimumStockLevelReport")
        {
            hidden();
        }
        else if (txtReport.SelectedValue.ToString() == "InventoryList")
        {
            hidden();
        }
        else if (txtReport.SelectedValue.ToString() == "InventoryListByStaff")
        {
            hidden();
            txtStaffId.Enabled = true;

        }
        else if (txtReport.SelectedValue.ToString().Trim() == "InventorylistbyIssueDate")
        {
            hidden();
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
        }
        else if (txtReport.SelectedValue.ToString() == "InventorylistbyProductCode")
        {
            hidden();
            txtProdCode.Enabled = true;
        }

        else if (txtReport.SelectedValue.ToString() == "ProductListReport")
        {
            hidden();
            txtProdCode.Enabled = true;
        }

        else if (txtReport.SelectedValue.ToString() == "StockIncomingReport")
        {
            hidden();
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
        }
        else if (txtReport.SelectedValue.ToString() == "StockLevelReport")
        {
            hidden();
            txtProdCode.Enabled = true;
            txtSuppName.Enabled = false;
        }
        else if (txtReport.SelectedValue.ToString() == "SupplierListReport")
        {
            hidden();
            txtSuppName.Enabled = true;
        }
        else if (txtReport.SelectedValue.ToString() == "WarehousewiseIncoming")
        {
            hidden();
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtWarehouse.Enabled = true;
        }
        else if (txtReport.SelectedValue.ToString() == "WarehousewiseIssue")
        {
            hidden();
            txtWarehouse.Enabled = true;
        }
        else if (txtReport.SelectedValue.ToString() == "LowStockReport")
        {
            hidden();
        }
    }

    //protected void txtReport_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    //{
    //    RadComboBox combobox = (RadComboBox)sender;
    //    //Session["Report"] = combobox.Text.ToString();
    //    Session["Report"] = combobox.Text.ToString();
    //}

    //protected void txtStaffId_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    //{
    //    RadComboBox combobox = (RadComboBox)sender;
    //    //Session["StaffId"] = combobox.Text.ToString();
    //    Session["StaffId"] = combobox.SelectedValue.ToString();
    //}

    //protected void txtProdCode_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    //{
    //    RadComboBox combobox = (RadComboBox)sender;
    //    //Session["ProdCode"] = combobox.Text.ToString();
    //    Session["ProdId"] = combobox.SelectedValue.ToString();
    //}

    //protected void txtSuppName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    //{
    //    RadComboBox combobox = (RadComboBox)sender;
    //    //Session["SuppName"] = combobox.Text.ToString();
    //    Session["SuppId"] = combobox.SelectedValue.ToString();
    //}

    //protected void txtDeptName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    //{
    //    RadComboBox combobox = (RadComboBox)sender;
    //    //Session["DeptName"] = combobox.Text.ToString();
    //    Session["DeptId"] = combobox.SelectedValue.ToString();
    //}

    public void showErrorMsg()
    {
        // lblMsg.Text = "Choose The Correct values";
        ShowMessage("Choose The Correct values", "Yellow");
    }

    private void ShowMessage(string message, string color)
    {
        lblStatus.Text = message.ToString();
        System.Drawing.ColorConverter colConvert = new ColorConverter();
        string strColor = color.ToString();
        lblStatus.ForeColor = (System.Drawing.Color)colConvert.ConvertFromString(strColor);
    }

    private void InsertLogAuditTrail(string userid, string module, string activity, string result, string flag)
    {
        SqlConnection connLog = BusinessTier.getConnection();
        connLog.Open();
        BusinessTier.InsertLogAuditTrial(connLog, userid, module, activity, result, flag);
        BusinessTier.DisposeConnection(connLog);
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        //Label lblMsg 

        lblMsg.Text = "";
        //Session["FromDate"] = txtFromDate.SelectedDate.ToString();
        //Session["ToDate"] = txtToDate.SelectedDate.ToString();
        //Session["Report"] = "";
        //Session["StaffId"] = "";
        //Session["ProdId"] = "";
        //Session["SuppId"] = "";
        //Session["DeptId"] = "";

        String DeptId = "*";

        String WarehouseId = "*";

        string sql1 = "";

        //Session["Report"] = txtReport.Text.ToString();

        //String Report = Session["Report"].ToString()

        String Report = txtReport.SelectedValue.ToString();

        String fromDate = txtFromDate.SelectedDate.ToString();
        String toDate = txtToDate.SelectedDate.ToString();
       

        try
        {
            string con = @BusinessTier.getConnection1();
            if (Report.Equals("DepartmentwiseReportByPrice"))
            {
                txtStaffId.Enabled = false;
                txtProdCode.Enabled = false;
                txtSuppName.Enabled = false;

                if (txtDeptName.Text != "---All---")
                {
                    DeptId = txtDeptName.SelectedValue.ToString();
                    sql1 = "SELECT Products.ProductName,StaffMaster.StaffName,PPEissue.DeptId,Department.DeptName,PPEissue.StaffId,PPEissue.ProductID,Incoming.Quantity,Incoming.Unitprice,Incoming.Amount FROM PPEissue INNER JOIN Incoming ON PPEissue.IncomingID=Incoming.ID INNER JOIN Department on Department.DeptId=PPEissue.DeptId INNER JOIN StaffMaster on StaffMaster.StaffId=PPEissue.StaffId INNER JOIN Products Products ON PPEissue.ProductID=Products.ID where PPEIssue.Deleted=0 and Incoming.Deleted=0 and Products.Deleted=0 and StaffMaster.Deleted=0 and Department.DeptId =" + DeptId.ToString().Trim();

                }
                else
                {
                    DeptId = "";
                    sql1 = "SELECT Products.ProductName,StaffMaster.StaffName,PPEissue.DeptId,Department.DeptName,PPEissue.StaffId,PPEissue.ProductID,Incomingm.Quantity,Incoming.Unitprice,Incoming.Amount FROM PPEissue INNER JOIN Incoming ON PPEissue.IncomingID=Incoming.ID INNER JOIN Department on Department.DeptId=PPEissue.DeptId INNER JOIN StaffMaster on StaffMaster.StaffId=PPEissue.StaffId INNER JOIN Products Products ON PPEissue.ProductID=Products.ID where PPEIssue.Deleted=0 and Incoming.Deleted=0 and Products.Deleted=0 and StaffMaster.Deleted=0";
                }
                // Session["DeptId"] = DeptId.ToString();
                Stimulsoft.Report.StiReport stiReport1;

                SqlDataAdapter ad1 = new SqlDataAdapter(sql1, con);

                DataSet ds1 = new DataSet();
                ds1.DataSetName = "DynamicDataSource1";

                ds1.Tables.Add("DataSource1");
                ad1.Fill(ds1, "DataSource1");

                stiReport1 = new StiReport();
                stiReport1.Dictionary.DataStore.Clear();
                stiReport1.ClearAllStates();
                // string strpath = HttpContext.Current.Server.MapPath(string.Empty);
                //stiReport1.Load(strpath + "\\RiskRegister.mrt");
                stiReport1.Load("c:\\inetpub\\wwwroot\\InventorySystem\\Reports\\DepartmentwiseReportByPrice.mrt");
                stiReport1.Dictionary.Databases.Clear();
                stiReport1.Dictionary.Databases.Add(new StiSqlDatabase("Connection", con));
                stiReport1.Dictionary.DataSources.Clear();
                stiReport1.RegData("DataSource1", ds1);

                stiReport1.Dictionary.Synchronize();
                stiReport1.Compile();
                stiReport1.Render();
                StiWebViewer1.Report = stiReport1;
                StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                stiReport1.Dispose();
            }

            else if (Report.Equals("MinimumStockLevelReport"))
            {

                sql1 = "SELECT i1.ProductID,p1.ProductCode,p1.ProductName,p1.Unit,p1.UnitPrice,p1.DurationPeriod,sum(i1.BalQty) as BalQtySum,p1.MinimumStock FROM Products p1 inner join Incoming i1 on p1.ID = i1.ProductID WHERE p1.deleted=0 and i1.deleted=0 group by i1.ProductID,p1.MinimumStock,p1.ProductCode,p1.ProductName,p1.Unit,p1.UnitPrice,p1.DurationPeriod having sum(i1.BalQty) < p1.MinimumStock";

                Stimulsoft.Report.StiReport stiReport1;


                SqlDataAdapter ad1 = new SqlDataAdapter(sql1, con);

                DataSet ds1 = new DataSet();
                ds1.DataSetName = "DynamicDataSource1";

                ds1.Tables.Add("DataSource1");
                ad1.Fill(ds1, "DataSource1");

                stiReport1 = new StiReport();
                stiReport1.Dictionary.DataStore.Clear();
                stiReport1.ClearAllStates();
                // string strpath = HttpContext.Current.Server.MapPath(string.Empty);
                //stiReport1.Load(strpath + "\\RiskRegister.mrt");
                stiReport1.Load("c:\\inetpub\\wwwroot\\InventorySystem\\Reports\\MinimumStockLevelReport.mrt");
                stiReport1.Dictionary.Databases.Clear();
                stiReport1.Dictionary.Databases.Add(new StiSqlDatabase("Connection", con));
                stiReport1.Dictionary.DataSources.Clear();
                stiReport1.RegData("DataSource1", ds1);

                stiReport1.Dictionary.Synchronize();
                stiReport1.Compile();

                stiReport1.Render();
                StiWebViewer1.Report = stiReport1;
                StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                stiReport1.Dispose();
            }

            else if (Report.Equals("InventoryList"))
            {

                //sql1 = "SELECT dbo.StaffMaster.StaffName, PPEIssue.StaffId, Products.ProductCode, Products.ProductName, PPEIssue.ExpireDate, PPEIssue.IssueDate, PPEIssue.Issuance,PPEIssue.SafetyDate, StaffMaster_1.StaffName AS SafetyName FROM dbo.PPEissue AS PPEIssue INNER JOIN dbo.StaffMaster ON PPEIssue.StaffId = dbo.StaffMaster.StaffId INNER JOIN dbo.Products AS Products ON PPEIssue.ProductID = Products.ID INNER JOIN dbo.StaffMaster AS StaffMaster_1 ON PPEIssue.SafetyBy = StaffMaster_1.StaffId where  products.deleted=0 and PPEissue.deleted=0 and staffmaster.deleted=0";
                sql1 = "select * from VW_PPEissue";

                Stimulsoft.Report.StiReport stiReport1;

                SqlDataAdapter ad1 = new SqlDataAdapter(sql1, con);

                DataSet ds1 = new DataSet();
                ds1.DataSetName = "DynamicDataSource1";

                ds1.Tables.Add("VW_PPEissue");
                ad1.Fill(ds1, "VW_PPEissue");

                stiReport1 = new StiReport();
                stiReport1.Dictionary.DataStore.Clear();
                stiReport1.ClearAllStates();
                // string strpath = HttpContext.Current.Server.MapPath(string.Empty);
                //stiReport1.Load(strpath + "\\RiskRegister.mrt");
                stiReport1.Load("c:\\inetpub\\wwwroot\\InventorySystem\\Reports\\PPEInventoryList.mrt");
                stiReport1.Dictionary.Databases.Clear();
                stiReport1.Dictionary.Databases.Add(new StiSqlDatabase("Connection", con));
                stiReport1.Dictionary.DataSources.Clear();
                stiReport1.RegData("VW_PPEissue", ds1);

                stiReport1.Dictionary.Synchronize();
                stiReport1.Compile();
                stiReport1.Render();
                StiWebViewer1.Report = stiReport1;
                StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                stiReport1.Dispose();

            }
            else if (Report.Equals("InventoryListByStaff"))
            {
                if (txtStaffId.Text.ToString() == "---All---")
                {
                    lblStatus.Text = "Please Select Staff Name";
                    return;
                }

                sql1 = "select * from VW_PPEissue where StaffName='" + txtStaffId.Text.ToString().Trim() + "'";

                Stimulsoft.Report.StiReport stiReport1;

                SqlDataAdapter ad1 = new SqlDataAdapter(sql1, con);

                DataSet ds1 = new DataSet();
                ds1.DataSetName = "DynamicDataSource1";

                ds1.Tables.Add("VW_PPEissue");
                ad1.Fill(ds1, "VW_PPEissue");

                stiReport1 = new StiReport();
                stiReport1.Dictionary.DataStore.Clear();
                stiReport1.ClearAllStates();
                // string strpath = HttpContext.Current.Server.MapPath(string.Empty);
                //stiReport1.Load(strpath + "\\RiskRegister.mrt");
                stiReport1.Load("c:\\inetpub\\wwwroot\\InventorySystem\\Reports\\PPEInventoryList.mrt");
                stiReport1.Dictionary.Databases.Clear();
                stiReport1.Dictionary.Databases.Add(new StiSqlDatabase("Connection", con));
                stiReport1.Dictionary.DataSources.Clear();
                stiReport1.RegData("VW_PPEissue", ds1);

                stiReport1.Dictionary.Synchronize();
                stiReport1.Compile();
                stiReport1.Render();
                StiWebViewer1.Report = stiReport1;
                StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                stiReport1.Dispose();

            }

            else if (Report.Equals("InventorylistbyIssueDate"))
            {
                if (fromDate.ToString() == "")
                {
                    lblStatus.Text = "Select From Date";
                    return;
                }
                if (toDate.ToString() == "")
                {
                    lblStatus.Text = "Select To Date";
                    return;
                }
                DateTime dtFromDate = DateTime.Parse(fromDate);
                fromDate = dtFromDate.Month + "/" + dtFromDate.Day + "/" + dtFromDate.Year;

                DateTime dtToDate = DateTime.Parse(toDate);
                toDate = dtToDate.Month + "/" + dtToDate.Day + "/" + dtToDate.Year;

                sql1 = "select * from VW_PPEissue where IssueDate between convert(varchar(10),'" + fromDate.ToString().Trim() + "',101) AND convert(varchar(10),'" + toDate.ToString().Trim() + "',101) order by issuedate ";
                Stimulsoft.Report.StiReport stiReport1;

                SqlDataAdapter ad1 = new SqlDataAdapter(sql1, con);

                DataSet ds1 = new DataSet();
                ds1.DataSetName = "DynamicDataSource1";

                ds1.Tables.Add("VW_PPEissue");
                ad1.Fill(ds1, "VW_PPEissue");

                stiReport1 = new StiReport();
                stiReport1.Dictionary.DataStore.Clear();
                stiReport1.ClearAllStates();
                // string strpath = HttpContext.Current.Server.MapPath(string.Empty);
                //stiReport1.Load(strpath + "\\RiskRegister.mrt");
                stiReport1.Load("c:\\inetpub\\wwwroot\\InventorySystem\\Reports\\PPEInventoryList.mrt");
                stiReport1.Dictionary.Databases.Clear();
                stiReport1.Dictionary.Databases.Add(new StiSqlDatabase("Connection", con));
                stiReport1.Dictionary.DataSources.Clear();
                stiReport1.RegData("VW_PPEissue", ds1);

                stiReport1.Dictionary.Synchronize();
                stiReport1.Compile();
                stiReport1.Render();
                StiWebViewer1.Report = stiReport1;
                StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                stiReport1.Dispose();
            }

            else if (Report.Equals("InventorylistbyProductCode"))
            {

                if (txtProdCode.Text != "---All---")
                {
                    DeptId = txtProdCode.SelectedValue.ToString();
                    //sql1 = "SELECT Products.ProductName, Products.Unit, Products.ProductCode, Products.UnitPrice, Products.DurationTime, Products.MinimumStock, Products.DurationPeriod, Products.Quantity FROM Products Products";
                    sql1 = "select * from VW_PPEissue where ProductID='" + txtProdCode.SelectedValue.ToString().Trim() + "'";

                }
                else
                {
                    DeptId = "";
                    //sql1 = "SELECT Products.ProductName,Products.Unit,Products.ProductCode, Products.UnitPrice, Products.DurationTime, Products.MinimumStock, Products.DurationPeriod, Products.Quantity FROM Products Products WHERE Products.ID="+txtProdCode.SelectedValue.ToString().Trim();
                    sql1 = "select * from VW_PPEissue";
                }
                // Session["DeptId"] = DeptId.ToString();



                Stimulsoft.Report.StiReport stiReport1;

                SqlDataAdapter ad1 = new SqlDataAdapter(sql1, con);

                DataSet ds1 = new DataSet();
                ds1.DataSetName = "DynamicDataSource1";

                ds1.Tables.Add("VW_PPEissue");
                ad1.Fill(ds1, "VW_PPEissue");

                stiReport1 = new StiReport();
                stiReport1.Dictionary.DataStore.Clear();
                stiReport1.ClearAllStates();
                // string strpath = HttpContext.Current.Server.MapPath(string.Empty);
                //stiReport1.Load(strpath + "\\RiskRegister.mrt");
                stiReport1.Load("c:\\inetpub\\wwwroot\\InventorySystem\\Reports\\PPEInventoryList.mrt");
                stiReport1.Dictionary.Databases.Clear();
                stiReport1.Dictionary.Databases.Add(new StiSqlDatabase("Connection", con));
                stiReport1.Dictionary.DataSources.Clear();
                stiReport1.RegData("VW_PPEissue", ds1);

                stiReport1.Dictionary.Synchronize();
                stiReport1.Compile();
                stiReport1.Render();
                StiWebViewer1.Report = stiReport1;
                StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                stiReport1.Dispose();
            }
            else if (Report.Equals("ProductListReport"))
            {

                if (txtProdCode.Text != "---All---")
                {
                    DeptId = txtProdCode.SelectedValue.ToString();
                    //sql1 = "SELECT Products.ProductName, Products.Unit, Products.ProductCode, Products.UnitPrice, Products.DurationTime, Products.MinimumStock, Products.DurationPeriod, Products.Quantity FROM Products Products";
                    sql1 = "SELECT Products.ProductName, Products.Unit, Products.ProductCode, Products.UnitPrice, Products.DurationTime, Products.MinimumStock, Products.DurationPeriod, sum(Incoming.BalQty) as balance FROM Products Products join Incoming on Incoming.ProductID='" + txtProdCode.SelectedValue.ToString().Trim() + "' WHERE Products.ID='" + txtProdCode.SelectedValue.ToString().Trim() + "' group by Products.ProductName, Products.Unit, Products.ProductCode, Products.UnitPrice, Products.DurationTime, Products.MinimumStock, Products.DurationPeriod";

                }
                else
                {
                    DeptId = "";
                    //sql1 = "SELECT Products.ProductName,Products.Unit,Products.ProductCode, Products.UnitPrice, Products.DurationTime, Products.MinimumStock, Products.DurationPeriod, Products.Quantity FROM Products Products WHERE Products.ID="+txtProdCode.SelectedValue.ToString().Trim();
                    sql1 = "SELECT Products.ProductName, Products.Unit, Products.ProductCode, Products.UnitPrice, Products.DurationTime, Products.MinimumStock, Products.DurationPeriod, sum(Incoming.BalQty) as balance FROM Products Products join Incoming on Incoming.ProductID=Products.ID group by Products.ProductName, Products.Unit, Products.ProductCode, Products.UnitPrice, Products.DurationTime, Products.MinimumStock, Products.DurationPeriod";
                }
                // Session["DeptId"] = DeptId.ToString();



                Stimulsoft.Report.StiReport stiReport1;

                SqlDataAdapter ad1 = new SqlDataAdapter(sql1, con);

                DataSet ds1 = new DataSet();
                ds1.DataSetName = "DynamicDataSource1";

                ds1.Tables.Add("DataSource1");
                ad1.Fill(ds1, "DataSource1");

                stiReport1 = new StiReport();
                stiReport1.Dictionary.DataStore.Clear();
                stiReport1.ClearAllStates();
                // string strpath = HttpContext.Current.Server.MapPath(string.Empty);
                //stiReport1.Load(strpath + "\\RiskRegister.mrt");
                stiReport1.Load("c:\\inetpub\\wwwroot\\InventorySystem\\Reports\\ProductListReport.mrt");
                stiReport1.Dictionary.Databases.Clear();
                stiReport1.Dictionary.Databases.Add(new StiSqlDatabase("Connection", con));
                stiReport1.Dictionary.DataSources.Clear();
                stiReport1.RegData("DataSource1", ds1);

                stiReport1.Dictionary.Synchronize();
                stiReport1.Compile();
                stiReport1.Render();
                StiWebViewer1.Report = stiReport1;
                StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                stiReport1.Dispose();
            }


            else if (Report.Equals("StockIncomingReport"))
            {
                if (fromDate.ToString() == "")
                {
                    lblStatus.Text = "Select From Date";
                    return;
                }
                if (toDate.ToString() == "")
                {
                    lblStatus.Text = "Select To Date";
                    return;
                }
                DateTime dtFromDate = DateTime.Parse(fromDate);
                fromDate = dtFromDate.Month + "/" + dtFromDate.Day + "/" + dtFromDate.Year;

                DateTime dtToDate = DateTime.Parse(toDate);
                toDate = dtToDate.Month + "/" + dtToDate.Day + "/" + dtToDate.Year;


                sql1 = "SELECT Incoming.IncomingDate, Products.ProductCode, Products.ProductName, Supplier.SupplierName, Incoming.Quantity, Incoming.Unitprice, Incoming.Amount, Incoming.ExpiredDate FROM Incoming inner join Products on Products.ID=Incoming.ProductID inner join Supplier on Supplier.SupplierId=Incoming.SupplierId WHERE Incoming.IncomingDate between convert(varchar(10),'" + fromDate.ToString().Trim() + "',101) AND convert(varchar(10),'" + toDate.ToString().Trim() + "',101) and incoming.deleted=0 ORDER BY Incoming.IncomingDate";

                Stimulsoft.Report.StiReport stiReport1;

                SqlDataAdapter ad1 = new SqlDataAdapter(sql1, con);

                DataSet ds1 = new DataSet();
                ds1.DataSetName = "DynamicDataSource1";

                ds1.Tables.Add("DataSource1");
                ad1.Fill(ds1, "DataSource1");

                stiReport1 = new StiReport();
                stiReport1.Dictionary.DataStore.Clear();
                // string strpath = HttpContext.Current.Server.MapPath(string.Empty);
                //stiReport1.Load(strpath + "\\RiskRegister.mrt");
                stiReport1.Load("c:\\inetpub\\wwwroot\\InventorySystem\\Reports\\StockIncomingReport.mrt");
                stiReport1.Dictionary.Databases.Clear();
                stiReport1.ClearAllStates();
                stiReport1.Dictionary.Databases.Add(new StiSqlDatabase("Connection", con));
                stiReport1.Dictionary.DataSources.Clear();
                stiReport1.RegData("DataSource1", ds1);

                stiReport1.Dictionary.Synchronize();
                stiReport1.Compile();
                stiReport1.Render();
                StiWebViewer1.Report = stiReport1;
                StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                stiReport1.Dispose();
            }

            else if (Report.Equals("StockLevelReport"))
            {

                if (txtProdCode.Text != "---All---")
                {
                    sql1 = "SELECT Products.ProductCode,Supplier.SupplierName,Products.ProductName,Incoming.IncomingDate,Incoming.Quantity,ProductS.Unit,Incoming.Unitprice,Incoming.Amount,Incoming.ExpiredDate FROM Incoming INNER JOIN Products ON Incoming.ProductID=ProductS.ID inner join Supplier on Supplier.SupplierId=Incoming.SupplierId WHERE incoming.deleted=0 and products.deleted=0 and supplier.deleted=0 and Products.ID =" + txtProdCode.SelectedValue.ToString() + "";
                }
                else
                {
                    sql1 = "SELECT Products.ProductCode,Supplier.SupplierName,Products.ProductName,Incoming.IncomingDate,Incoming.Quantity,ProductS.Unit,Incoming.Unitprice,Incoming.Amount,Incoming.ExpiredDate FROM Incoming INNER JOIN Products ON Incoming.ProductID=ProductS.ID inner join Supplier on Supplier.SupplierId=Incoming.SupplierId where incoming.deleted=0 and products.deleted=0 and supplier.deleted=0";
                }

                Stimulsoft.Report.StiReport stiReport1;

                SqlDataAdapter ad1 = new SqlDataAdapter(sql1, con);

                DataSet ds1 = new DataSet();
                ds1.DataSetName = "DynamicDataSource1";

                ds1.Tables.Add("DataSource1");
                ad1.Fill(ds1, "DataSource1");

                stiReport1 = new StiReport();
                stiReport1.Dictionary.DataStore.Clear();
                stiReport1.ClearAllStates();
                // string strpath = HttpContext.Current.Server.MapPath(string.Empty);
                //stiReport1.Load(strpath + "\\RiskRegister.mrt");
                stiReport1.Load("c:\\inetpub\\wwwroot\\InventorySystem\\Reports\\StockLevelReport.mrt");
                stiReport1.Dictionary.Databases.Clear();
                stiReport1.Dictionary.Databases.Add(new StiSqlDatabase("Connection", con));
                stiReport1.Dictionary.DataSources.Clear();
                stiReport1.RegData("DataSource1", ds1);

                stiReport1.Dictionary.Synchronize();
                stiReport1.Compile();
                stiReport1.Render();
                StiWebViewer1.Report = stiReport1;
                StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                stiReport1.Dispose();
            }

            else if (Report.Equals("SupplierListReport"))
            {


                if (txtSuppName.Text.ToString().Trim() != "---All---")
                {
                    sql1 = "SELECT Supplier.SupplierName,Supplier.Address1,Supplier.Address2,Supplier.City,Supplier.postcode,Supplier.state,Supplier.email,Supplier.contactno,Supplier.Faxno,Supplier.Description,Supplier.website FROM Supplier WHERE Supplier.SupplierId='" + txtSuppName.SelectedValue.ToString().Trim() + "'";
                }
                else
                {
                    sql1 = "SELECT Supplier.SupplierName,Supplier.Address1,Supplier.Address2,Supplier.City,Supplier.postcode,Supplier.state,Supplier.email,Supplier.contactno,Supplier.Faxno,Supplier.Description,Supplier.website FROM Supplier where supplier.deleted=0 ";
                }

                Stimulsoft.Report.StiReport stiReport1;

                SqlDataAdapter ad1 = new SqlDataAdapter(sql1, con);

                DataSet ds1 = new DataSet();
                ds1.DataSetName = "DynamicDataSource1";

                ds1.Tables.Add("DataSource1");
                ad1.Fill(ds1, "DataSource1");

                stiReport1 = new StiReport();
                stiReport1.Dictionary.DataStore.Clear();
                // string strpath = HttpContext.Current.Server.MapPath(string.Empty);
                //stiReport1.Load(strpath + "\\RiskRegister.mrt");
                stiReport1.ClearAllStates();
                stiReport1.Load("c:\\inetpub\\wwwroot\\InventorySystem\\Reports\\SupplierListReport.mrt");
                stiReport1.Dictionary.Databases.Clear();
                stiReport1.Dictionary.Databases.Add(new StiSqlDatabase("Connection", con));
                stiReport1.Dictionary.DataSources.Clear();
                stiReport1.RegData("DataSource1", ds1);

                stiReport1.Dictionary.Synchronize();
                stiReport1.Compile();
                stiReport1.Render();
                StiWebViewer1.Report = stiReport1;
                StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                stiReport1.Dispose();
            }

            else if (Report.Equals("WarehousewiseIncoming"))
            {
                if (fromDate.ToString() == "")
                {
                    lblStatus.Text = "Select From Date";
                    return;
                }
                if (toDate.ToString() == "")
                {
                    lblStatus.Text = "Select To Date";
                    return;
                }
                DateTime dtFromDate = DateTime.Parse(fromDate);
                fromDate = dtFromDate.Month + "/" + dtFromDate.Day + "/" + dtFromDate.Year;

                DateTime dtToDate = DateTime.Parse(toDate);
                toDate = dtToDate.Month + "/" + dtToDate.Day + "/" + dtToDate.Year;


                if (txtWarehouse.Text != "---All---")
                {

                    sql1 = "SELECT Warehouse.WarehouseName,Incoming.IncomingDate, Products.ProductCode, Products.ProductName, Supplier.SupplierName, Incoming.Quantity, Incoming.Unitprice, Incoming.Amount, Incoming.ExpiredDate FROM Incoming inner join Products on Products.ID=Incoming.ProductID inner join Supplier on Supplier.SupplierId=Incoming.SupplierId inner join Warehouse on Warehouse.WarehouseId = Products.WarehouseId WHERE Incoming.IncomingDate between convert(varchar(10),'" + fromDate.ToString().Trim() + "',101) AND convert(varchar(10),'" + toDate.ToString().Trim() + "',101) and incoming.deleted=0 and Supplier.deleted=0 and Products.deleted=0 and Warehouse.deleted=0 and Warehouse.warehouseId =" + txtWarehouse.SelectedValue.ToString().Trim() + " group by Warehouse.WarehouseName,Incoming.IncomingDate, Products.ProductCode, Products.ProductName, Supplier.SupplierName, Incoming.Quantity, Incoming.Unitprice, Incoming.Amount, Incoming.ExpiredDate ORDER BY Incoming.IncomingDate";

                }
                else
                {

                    sql1 = "SELECT Warehouse.WarehouseName,Incoming.IncomingDate, Products.ProductCode, Products.ProductName, Supplier.SupplierName, Incoming.Quantity, Incoming.Unitprice, Incoming.Amount, Incoming.ExpiredDate FROM Incoming inner join Products on Products.ID=Incoming.ProductID inner join Supplier on Supplier.SupplierId=Incoming.SupplierId inner join Warehouse on Warehouse.WarehouseId = Products.WarehouseId WHERE Incoming.IncomingDate between convert(varchar(10),'" + fromDate.ToString().Trim() + "',101) AND convert(varchar(10),'" + toDate.ToString().Trim() + "',101) and incoming.deleted=0 and Supplier.deleted=0 and Products.deleted=0 and Warehouse.deleted=0 group by Warehouse.WarehouseName,Incoming.IncomingDate, Products.ProductCode, Products.ProductName, Supplier.SupplierName, Incoming.Quantity, Incoming.Unitprice, Incoming.Amount, Incoming.ExpiredDate ORDER BY Incoming.IncomingDate";

                }
                // Session["DeptId"] = DeptId.ToString();



                Stimulsoft.Report.StiReport stiReport1;

                SqlDataAdapter ad1 = new SqlDataAdapter(sql1, con);

                DataSet ds1 = new DataSet();
                ds1.DataSetName = "DynamicDataSource1";

                ds1.Tables.Add("DataSource1");
                ad1.Fill(ds1, "DataSource1");

                stiReport1 = new StiReport();
                stiReport1.Dictionary.DataStore.Clear();
                stiReport1.ClearAllStates();
                // string strpath = HttpContext.Current.Server.MapPath(string.Empty);
                //stiReport1.Load(strpath + "\\RiskRegister.mrt");
                stiReport1.Load("c:\\inetpub\\wwwroot\\InventorySystem\\Reports\\WarehousewiseIncoming.mrt");
                stiReport1.Dictionary.Databases.Clear();
                stiReport1.Dictionary.Databases.Add(new StiSqlDatabase("Connection", con));
                stiReport1.Dictionary.DataSources.Clear();
                stiReport1.RegData("DataSource1", ds1);

                stiReport1.Dictionary.Synchronize();
                stiReport1.Compile();
                stiReport1.Render();
                StiWebViewer1.Report = stiReport1;
                StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                stiReport1.Dispose();
            }

            else if (Report.Equals("WarehousewiseIssue"))
            {

                if (txtWarehouse.Text != "---All---")
                {

                    sql1 = "SELECT Warehouse.WarehouseName,Products.ProductCode,Products.ProductName,StaffMaster.StaffName,PPEissue.DeptId,PPEissue.StaffId,PPEissue.Quantity,Incoming.Unitprice,Incoming.Amount FROM PPEissue INNER JOIN Incoming ON PPEissue.IncomingID=Incoming.ID  INNER JOIN StaffMaster on StaffMaster.StaffId=PPEissue.StaffId INNER JOIN Products Products ON PPEissue.ProductID=Products.ID INNER JOIN Warehouse on Warehouse.WarehouseId=Products.WarehouseId where PPEIssue.Deleted=0 and Incoming.Deleted=0 and Products.Deleted=0 and StaffMaster.Deleted=0 and Warehouse.deleted=0 and Warehouse.WarehouseId =" + txtWarehouse.SelectedValue.ToString().Trim() + " group by Warehouse.WarehouseName,Products.ProductCode,Products.ProductName,StaffMaster.StaffName,PPEissue.DeptId,PPEissue.StaffId,PPEissue.Quantity,Incoming.Unitprice,Incoming.Amount";

                }
                else
                {

                    sql1 = "SELECT Warehouse.WarehouseName,Products.ProductCode,Products.ProductName,StaffMaster.StaffName,PPEissue.DeptId,PPEissue.StaffId,PPEissue.Quantity,Incoming.Unitprice,Incoming.Amount FROM PPEissue INNER JOIN Incoming ON PPEissue.IncomingID=Incoming.ID  INNER JOIN StaffMaster on StaffMaster.StaffId=PPEissue.StaffId INNER JOIN Products Products ON PPEissue.ProductID=Products.ID INNER JOIN Warehouse on Warehouse.WarehouseId=Products.WarehouseId where PPEIssue.Deleted=0 and Incoming.Deleted=0 and Products.Deleted=0 and StaffMaster.Deleted=0 and Warehouse.deleted=0 group by Warehouse.WarehouseName,Products.ProductCode,Products.ProductName,StaffMaster.StaffName,PPEissue.DeptId,PPEissue.StaffId,PPEissue.Quantity,Incoming.Unitprice,Incoming.Amount";

                }
                // Session["DeptId"] = DeptId.ToString();



                Stimulsoft.Report.StiReport stiReport1;

                SqlDataAdapter ad1 = new SqlDataAdapter(sql1, con);

                DataSet ds1 = new DataSet();
                ds1.DataSetName = "DynamicDataSource1";

                ds1.Tables.Add("DataSource1");
                ad1.Fill(ds1, "DataSource1");

                stiReport1 = new StiReport();
                stiReport1.Dictionary.DataStore.Clear();
                stiReport1.ClearAllStates();
                // string strpath = HttpContext.Current.Server.MapPath(string.Empty);
                //stiReport1.Load(strpath + "\\RiskRegister.mrt");
                stiReport1.Load("c:\\inetpub\\wwwroot\\InventorySystem\\Reports\\WarehousewiseIssue.mrt");
                stiReport1.Dictionary.Databases.Clear();
                stiReport1.Dictionary.Databases.Add(new StiSqlDatabase("Connection", con));
                stiReport1.Dictionary.DataSources.Clear();
                stiReport1.RegData("DataSource1", ds1);

                stiReport1.Dictionary.Synchronize();
                stiReport1.Compile();
                stiReport1.Render();
                StiWebViewer1.Report = stiReport1;
                StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                stiReport1.Dispose();
            }

            else if (Report.Equals("LowStockReport"))
            {

                sql1 = "SELECT i1.ProductID,p1.ProductCode,p1.ProductName,p1.Unit,p1.UnitPrice,p1.DurationPeriod,sum(i1.BalQty) as BalQtySum,p1.MinimumStock FROM Products p1 inner join Incoming i1 on p1.ID = i1.ProductID WHERE p1.deleted=0 and i1.deleted=0 group by i1.ProductID,p1.MinimumStock,p1.ProductCode,p1.ProductName,p1.Unit,p1.UnitPrice,p1.DurationPeriod having sum(i1.BalQty) < p1.MinimumStock";

                Stimulsoft.Report.StiReport stiReport1;


                SqlDataAdapter ad1 = new SqlDataAdapter(sql1, con);

                DataSet ds1 = new DataSet();
                ds1.DataSetName = "DynamicDataSource1";

                ds1.Tables.Add("DataSource1");
                ad1.Fill(ds1, "DataSource1");

                stiReport1 = new StiReport();
                stiReport1.Dictionary.DataStore.Clear();
                stiReport1.ClearAllStates();
                // string strpath = HttpContext.Current.Server.MapPath(string.Empty);
                //stiReport1.Load(strpath + "\\RiskRegister.mrt");
                stiReport1.Load("c:\\inetpub\\wwwroot\\InventorySystem\\Reports\\MinimumStockLevelReport.mrt");
                stiReport1.Dictionary.Databases.Clear();
                stiReport1.Dictionary.Databases.Add(new StiSqlDatabase("Connection", con));
                stiReport1.Dictionary.DataSources.Clear();
                stiReport1.RegData("DataSource1", ds1);

                stiReport1.Dictionary.Synchronize();
                stiReport1.Compile();

                stiReport1.Render();
                StiWebViewer1.Report = stiReport1;
                StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                stiReport1.Dispose();
            }
        }
        catch (Exception ex)
        {

            //throw;
        }
        //con.close();

    }


}
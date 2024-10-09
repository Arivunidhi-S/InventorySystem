using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Configuration;
using System.Net.Mail;
using System.IO;
using System.Net;

using System.Globalization;
using System.Collections;
using System.Data.OleDb;
using System.Drawing;

/// <summary>
/// Summary description for Class1
/// </summary>
public class BusinessTier
{
    public BusinessTier()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static DataTable g_ErrorMessagesDataTable;

    public static SqlConnection getConnection()
    {
        string conString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        SqlConnection conn = new SqlConnection(conString);
        return conn;
    }

    public static string getConnection1()
    {
        string conString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        return conString;
    }

    public static void DisposeConnection(SqlConnection conn)
    {
        conn.Close();
        conn.Dispose();
    }

    public static void DisposeReader(SqlDataReader reader)
    {
        reader.Close();
        reader.Dispose();
    }

    public static void DisposeAdapter(SqlDataAdapter adapter)
    {
        adapter.Dispose();
    }

    public static SqlDataReader VaildateUserLogin(SqlConnection connec, string Logind, string Password)
    {
        SqlCommand cmd = new SqlCommand("sp_Validate_UserLogin", connec);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Useridp", Logind);
        cmd.Parameters.AddWithValue("@Passp", Password); ;
        SqlDataReader reader1 = cmd.ExecuteReader();
        return reader1;
    }


    //---------------------------------------------------------------------------------------------
    //--------------------------< Methods For Master Module >--------------------------------------

    public static SqlDataReader getMasterModule(SqlConnection conn)
    {
        int delval = 0;
        string sql = "select * FROM MasterModule WHERE Deleted='" + delval + "' ORDER BY ModuleName";
        SqlCommand cmd = new SqlCommand(sql, conn);
        SqlDataReader reader = cmd.ExecuteReader();
        return reader;
    }

    public static SqlDataReader getMasterModuleById(SqlConnection connect, string strModuleId)
    {
        int delval = 0;
        string sql = "select * FROM MasterModule WHERE Deleted='" + delval + "' and ModuleId='" + strModuleId + "' ORDER BY ModuleName";
        SqlCommand cmd = new SqlCommand(sql, connect);
        SqlDataReader reader1 = cmd.ExecuteReader();
        return reader1;

    }

    public static int DeleteModuleGrid(SqlConnection conn, string id)
    {
        SqlCommand dCmd = new SqlCommand("sp_MasterModule_Delete", conn);
        dCmd.CommandType = CommandType.StoredProcedure;
        dCmd.Parameters.AddWithValue("@moduleidp", id);
        return dCmd.ExecuteNonQuery();
    }


    public static SqlDataReader checkModuleName(SqlConnection connCheck, string name)
    {
        SqlCommand cmd = new SqlCommand("sp_MasterModule_IsDuplicate", connCheck);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@modulenamep", name);
        SqlDataReader reader = cmd.ExecuteReader();
        return reader;
    }


    public static int SaveModuleMaster(SqlConnection conn, string name, string desc, string appflag, string userid, string saveflag, string modid)
    {
        string sp_Name;
        string RowValue = "0";
        if (saveflag.ToString() == "N")
        {
            sp_Name = "[sp_MasterModule_Insert]";
        }
        else
        {
            sp_Name = "[sp_MasterModule_Update]";
        }
        SqlCommand dCmd = new SqlCommand(sp_Name, conn);
        dCmd.CommandType = CommandType.StoredProcedure;
        if (saveflag.ToString() == "U")
        {
            dCmd.Parameters.AddWithValue("@idp", modid);
            dCmd.Parameters.AddWithValue("@Rowp", RowValue);
        }
        dCmd.Parameters.AddWithValue("@namep", name);
        dCmd.Parameters.AddWithValue("@descriptionp", desc);
        dCmd.Parameters.AddWithValue("@approvalflag", appflag);
        dCmd.Parameters.AddWithValue("@useridp", userid);
        return dCmd.ExecuteNonQuery();
    }

    //---------------------------------------------------------------------------------------------
    //--------------------------< Methods For Master User >--------------------------------------

    public static SqlDataReader getMasterUserInfo(SqlConnection conn)
    {
        int delval = 0;
        string sql = "select * FROM MasterUser WHERE Deleted='" + delval + "' ORDER BY UserName";
        SqlCommand cmd = new SqlCommand(sql, conn);
        SqlDataReader reader = cmd.ExecuteReader();
        return reader;
    }

    public static SqlDataReader getMasterUserByID(SqlConnection conn, string strID)
    {
        int delval = 0;
        string sql = "select * FROM MasterUser WHERE ID='" + strID + "' and  Deleted='" + delval + "' ORDER BY UserName";
        SqlCommand cmd = new SqlCommand(sql, conn);
        SqlDataReader reader = cmd.ExecuteReader();
        return reader;
    }

    public static SqlDataReader getUserNameByID(SqlConnection conn, string strID)
    {
        SqlCommand cmd = new SqlCommand("[sp_MasterUser_getUserName]", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@idp", strID);
        SqlDataReader reader = cmd.ExecuteReader();
        return reader;
    }

    public static string getMasterUserIDByName(SqlConnection conn, string strName)
    {
        int delval = 0;
        string sql = "select ID FROM MasterUser WHERE UserName like '%" + strName + "%' and  Deleted='" + delval + "'";
        SqlCommand cmd = new SqlCommand(sql, conn);
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Read();
        string ret = reader[0].ToString();
        BusinessTier.DisposeReader(reader);
        //BusinessTier.DisposeConnection(conn);
        return ret;
    }

    public static SqlDataReader getMasterUserByLoginId(SqlConnection conn, string strLoginId)
    {
        int delval = 0;
        string sql = "select * FROM MasterUser WHERE Deleted='" + delval + "' and LoginId='" + strLoginId + "' ORDER BY UserName";
        SqlCommand cmd = new SqlCommand(sql, conn);
        SqlDataReader reader = cmd.ExecuteReader();
        return reader;
    }


    public static void BindErrorMessageDetails(SqlConnection connError)
    {
        string sql = "select * FROM MasterErrorMessage order by OrderNo";
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql, connError);
        g_ErrorMessagesDataTable = new DataTable();
        sqlDataAdapter.Fill(g_ErrorMessagesDataTable);
        BusinessTier.DisposeAdapter(sqlDataAdapter);
    }

    public static void InsertLogAuditTrial(SqlConnection connLog, string userid, string module, string activity, string result, string flag)
    {
        string sp_Name;
        if (flag.ToString() == "Log")
        {
            sp_Name = "[sp_Master_Insert_Log]";
        }
        else
        {
            sp_Name = "[sp_Master_Insert_AuditTrail]";
        }

        SqlCommand dCmd = new SqlCommand(sp_Name, connLog);

        dCmd.CommandType = CommandType.StoredProcedure;
        dCmd.Parameters.AddWithValue("@useridp", userid);
        dCmd.Parameters.AddWithValue("@modulep", module);
        dCmd.Parameters.AddWithValue("@activityp", activity);
        dCmd.Parameters.AddWithValue("@resultp", result);
        dCmd.ExecuteNonQuery();
    }


    public static SqlDataReader getMenuList(SqlConnection conn, string Uid)
    {
        string sql = "SELECT dbo.MenuList.Category, dbo.MenuList.SeqCategory FROM dbo.UserInfo_Permission INNER JOIN dbo.MenuList ON dbo.UserInfo_Permission.MenuId = dbo.MenuList.Id WHERE dbo.UserInfo_Permission.UserId = '" + Uid.ToString().Trim() + "' GROUP BY dbo.MenuList.Category, dbo.MenuList.SeqCategory";
        SqlCommand cmd = new SqlCommand(sql, conn);
        SqlDataReader reader = cmd.ExecuteReader();
        return reader;
    }

    public static DataTable getSubMenuItems(string category, string uid)
    {

        DataTable ret = new DataTable();
        SqlConnection conn = getConnection();
        conn.Open();
        string sql = "SELECT dbo.MenuList.ModuleID, dbo.MenuList.Href, dbo.MenuList.ModuleName FROM dbo.UserInfo_Permission INNER JOIN dbo.MenuList ON dbo.UserInfo_Permission.MenuId = dbo.MenuList.Id WHERE dbo.MenuList.Category = '" + category + "' and dbo.UserInfo_Permission.UserId = '" + uid.ToString().Trim() + "'  order by SeqMenu";
        SqlCommand cmd = new SqlCommand(sql, conn);
        SqlDataReader reader = cmd.ExecuteReader();
        ret.Load(reader);
        BusinessTier.DisposeConnection(conn);
        return ret;
    }

    //--------------------------------------------------------------------------------------------
    //----------------------MISC------------------------------------------------------------------

    public static string getCCMailID(string strModule)
    {
        string strEmailFile = ConfigurationManager.AppSettings["Email_CC_FilePath"].ToString();
        string strMailCC = "Default@petronas.com.my";

        if (File.Exists(strEmailFile))
        {
            string strLine = "";
            string[] strLine1 = new string[1];
            int counter = 0;
            StreamReader reader = new StreamReader(strEmailFile);
            while ((strLine = reader.ReadLine()) != null)
            {
                if (counter == 0)
                {
                    strLine1 = strLine.Split(':');

                    if (strLine1[0].ToString().Trim() == strModule.ToString().Trim())
                    {
                        strMailCC = strLine1[1].ToString().Trim();
                        counter = 1;
                    }
                }
            }
            reader.Close();
            reader.Dispose();
        }
        return strMailCC.ToString().Trim();
    }

    public static void SendMail(string strSubject, string strBody, string strToAddress, string strApprovarMail, string strAttachmentFilename)
    {
        SmtpClient smtpClient = new SmtpClient();
        MailMessage message = new MailMessage();
        if (!(strAttachmentFilename.ToString().Trim() == "NoAttach"))
        {
            Attachment attachment = new Attachment(strAttachmentFilename.ToString().Trim());
            message.Attachments.Add(attachment);
        }
        MailAddress fromAddress = new MailAddress(ConfigurationManager.AppSettings["FromAddress"].ToString(), "Online Asset Tracking System");
        smtpClient.Host = ConfigurationManager.AppSettings["ExchangeServer"].ToString();
        smtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"].ToString());

        message.Priority = MailPriority.High;
        message.From = fromAddress;
        message.Subject = strSubject.ToString();
        message.To.Add(strToAddress.ToString());
        message.CC.Add(strApprovarMail.ToString());
        //message.CC.Add("bala@e-serbadk.com");
        message.Body = strBody;
        //smtpClient.EnableSsl = true;
        smtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["FromAddress"].ToString(), ConfigurationManager.AppSettings["Password"].ToString().Trim());
        // smtpClient.Send(message);
        message.Dispose();
        smtpClient.Dispose();
        File.Delete(strAttachmentFilename.ToString().Trim());
    }

    //---------------------------------------------------------------------------------------------
    //--------------------------< Methods For RFID Activate/Deactivate >------------------------------------------------
    public static void UpdateRFIDDB(string MaterialID, string flag, string rfid)
    {
        string conString = ConfigurationManager.ConnectionStrings["connStringRFID"].ConnectionString;

        string desc = "Unknown", name = "Unknown", expirydate = "";
        int typeID = 31, behaviourID = 23, interval = 1;
        bool isActive = false;
        if (flag == "A")
        {
            typeID = 8;
            isActive = true;

            //SqlConnection getIdConn = getConnection();
            //getIdConn.Open();
            //SqlDataReader getIdreader = getAllInfoByID_MRVDetail(getIdConn, MaterialID);
            //if (getIdreader.Read())
            //{
            //    name = (getIdreader["MaterialName"].ToString()).TrimEnd();
            //    desc = (getIdreader["MaterialNo"].ToString()).TrimEnd();
            //    expirydate = (getIdreader["ExpiryDate"].ToString()).TrimEnd();
            //}
            //BusinessTier.DisposeReader(getIdreader);
            //BusinessTier.DisposeConnection(getIdConn);
        }

        //Update fasb_Tags.... Insert into TagAlertRules......
        SqlConnection conn = new SqlConnection(conString);
        conn.Open();
        string sp_Name = "[sp_RFID_Update]";
        SqlCommand dCmd = new SqlCommand(sp_Name, conn);
        dCmd.CommandType = CommandType.StoredProcedure;
        dCmd.Parameters.AddWithValue("@ID", rfid);
        dCmd.Parameters.AddWithValue("@TypeID", typeID);
        dCmd.Parameters.AddWithValue("@Description", desc);
        dCmd.Parameters.AddWithValue("@BehaviourID", behaviourID);
        dCmd.Parameters.AddWithValue("@Interval", interval);
        dCmd.Parameters.AddWithValue("@IsActive", isActive);
        dCmd.Parameters.AddWithValue("@Name", name);
        dCmd.Parameters.AddWithValue("@ExpiryDate", expirydate);
        dCmd.Parameters.AddWithValue("@Email", ConfigurationManager.AppSettings["RFID_Email"].ToString());
        dCmd.Parameters.AddWithValue("@Flag", flag);
        dCmd.ExecuteNonQuery();
        BusinessTier.DisposeConnection(conn);
    }

    public static void ExtendTagAlertRulesEndDate(string MRVMasterID, string MRVDetailID)
    {
        string conString = ConfigurationManager.ConnectionStrings["connStringRFID"].ConnectionString;
        string rfID = getRFID(MRVMasterID, MRVDetailID);
        string expiryDate = getMRVExtExpiryDate(MRVMasterID, MRVDetailID);
        SqlConnection conn = getConnection();
        conn.Open();
        string sp_Name = "[sp_RFID_Extend]";
        SqlCommand dCmd = new SqlCommand(sp_Name, conn);
        dCmd.CommandType = CommandType.StoredProcedure;
        dCmd.Parameters.AddWithValue("@ID", rfID);
        dCmd.Parameters.AddWithValue("@ExpiryDate", expiryDate);
        dCmd.ExecuteNonQuery();
        BusinessTier.DisposeConnection(conn);
    }

    public static string getRFID(string mrvMasterID, string mrvDetID)
    {
        string ret = "";
        SqlConnection conn = getConnection();
        conn.Open();
        string sql = "select RFID from MRV_Received WHERE MRVMasterID = " + mrvMasterID + " and MRVDetailID=" + mrvDetID + " and Deleted='0'";
        SqlCommand cmd = new SqlCommand(sql, conn);
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
            ret = reader["RFID"].ToString();
        BusinessTier.DisposeConnection(conn);
        return ret;
    }
    public static string getMRVExtExpiryDate(string mrvMasterID, string mrvDetID)
    {
        string ret = "";
        SqlConnection conn = getConnection();
        conn.Open();
        string sql = "select ExpiryDate from MRV_Detail WHERE MRVMasterID = " + mrvMasterID + " and MRVDetailID=" + mrvDetID + " and Deleted='0'";
        SqlCommand cmd = new SqlCommand(sql, conn);
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
            ret = reader["ExpiryDate"].ToString();
        BusinessTier.DisposeConnection(conn);
        return ret;
    }

    public static int SaveCompany(SqlConnection conn, string name, string address1, string address2, string city, string state, string country, string postcode, string desc, string phone, string fax, string email, string website, string userid, string saveflag, string Compid)
    {
        SqlCommand dCmd = new SqlCommand("[sp_Company_Save]", conn);
        dCmd.CommandType = CommandType.StoredProcedure;
        dCmd.Parameters.AddWithValue("@idp", Compid);
        dCmd.Parameters.AddWithValue("@namep", name);
        dCmd.Parameters.AddWithValue("@address1p", address1);
        dCmd.Parameters.AddWithValue("@address2p", address2);
        dCmd.Parameters.AddWithValue("@cityp", city);
        dCmd.Parameters.AddWithValue("@statep", state);
        dCmd.Parameters.AddWithValue("@countryp", country);
        dCmd.Parameters.AddWithValue("@descriptionp", desc);
        dCmd.Parameters.AddWithValue("@contactnop", phone);
        dCmd.Parameters.AddWithValue("@faxnop", fax);
        dCmd.Parameters.AddWithValue("@emailp", email);
        dCmd.Parameters.AddWithValue("@websitep", website);
        dCmd.Parameters.AddWithValue("@postcodep", postcode);
        dCmd.Parameters.AddWithValue("@useridp", userid);
        dCmd.Parameters.AddWithValue("@saveflag", saveflag);
        return dCmd.ExecuteNonQuery();
    }

    public static int SaveSupplier(SqlConnection conn, string name, string address1, string address2, string city, string state, string country, string postcode, string desc, string phone, string fax, string email, string website, string userid, string saveflag, string suppid)
    {
        string sp_Name = "";
        string RowValue = "0";
        if (saveflag.ToString() == "N")
            sp_Name = "sp_Supplier_Ins";

        if (saveflag.ToString() == "U")
            sp_Name = "sp_Supplier_Up";

        if (saveflag.ToString() == "D")
            sp_Name = "sp_Supplier_Del";


        SqlCommand dCmd = new SqlCommand(sp_Name, conn);
        dCmd.CommandType = CommandType.StoredProcedure;
        if (saveflag.ToString() == "U")
        {
            dCmd.Parameters.AddWithValue("@idp", suppid);
            dCmd.Parameters.AddWithValue("@namep", name);
            dCmd.Parameters.AddWithValue("@Rowp", RowValue);
        }
        if (saveflag.ToString() == "D")
            dCmd.Parameters.AddWithValue("@idp", suppid);

        if (saveflag.ToString() == "N")
            dCmd.Parameters.AddWithValue("@namep", name);

        if ((saveflag.ToString() == "N") || (saveflag.ToString() == "U"))
        {
            dCmd.Parameters.AddWithValue("@address1p", address1);
            dCmd.Parameters.AddWithValue("@address2p", address2);
            dCmd.Parameters.AddWithValue("@cityp", city);
            dCmd.Parameters.AddWithValue("@statep", state);
            dCmd.Parameters.AddWithValue("@countryp", country);
            dCmd.Parameters.AddWithValue("@descriptionp", desc);
            dCmd.Parameters.AddWithValue("@contactnop", phone);
            dCmd.Parameters.AddWithValue("@faxnop", fax);
            dCmd.Parameters.AddWithValue("@emailp", email);
            dCmd.Parameters.AddWithValue("@websitep", website);
            dCmd.Parameters.AddWithValue("@postcodep", postcode);
            dCmd.Parameters.AddWithValue("@useridp", userid);
        }
        return dCmd.ExecuteNonQuery();
    }

    public static SqlDataReader checkUserLoginId(SqlConnection connCheck, string strLoginId, int strstaffid)
    {
        SqlCommand cmd = new SqlCommand("[sp_MasterUser_IsDuplicate]", connCheck);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@loginidp", strLoginId);
        cmd.Parameters.AddWithValue("@staffidp", strstaffid);
        SqlDataReader reader = cmd.ExecuteReader();
        return reader;
    }

    public static int SaveProducts(SqlConnection conn, string prodcode, string prodname, string unit, decimal unitprice, int strdurationtime, string strdurationperiod, int minstock, int warehouse, int userid, string saveflag, int intId)
    {
        SqlCommand dCmd = new SqlCommand("[sp_Product_Save]", conn);
        dCmd.CommandType = CommandType.StoredProcedure;
        dCmd.Parameters.AddWithValue("@idp", intId);
        dCmd.Parameters.AddWithValue("@prodcodep", prodcode);
        dCmd.Parameters.AddWithValue("@prodnamep", prodname);
        dCmd.Parameters.AddWithValue("@unitp", unit);
        dCmd.Parameters.AddWithValue("@unitpricep", unitprice);
        dCmd.Parameters.AddWithValue("@validperiodp", strdurationtime);
        dCmd.Parameters.AddWithValue("@durationperiodp", strdurationperiod);
        dCmd.Parameters.AddWithValue("@minstockp", minstock);
        dCmd.Parameters.AddWithValue("@warehousep", warehouse);
        dCmd.Parameters.AddWithValue("@useridp", userid);
        dCmd.Parameters.AddWithValue("@saveflag", saveflag);
        return dCmd.ExecuteNonQuery();
    }

    public static int SaveDept(SqlConnection conn, string deptcode, string deptname, int companyid,string userid, string saveflag, int deptid)
    {
        SqlCommand dCmd = new SqlCommand("[sp_Department_Save]", conn);
        dCmd.CommandType = CommandType.StoredProcedure;
        dCmd.Parameters.AddWithValue("@idp", deptid);
        dCmd.Parameters.AddWithValue("@companyidp", companyid);
        dCmd.Parameters.AddWithValue("@deptcodep", deptcode);
        dCmd.Parameters.AddWithValue("@deptnamep", deptname);
        dCmd.Parameters.AddWithValue("@useridp", userid);
        dCmd.Parameters.AddWithValue("@saveflag", saveflag);
        return dCmd.ExecuteNonQuery();
    }

    public static int SaveStaff(SqlConnection conn, string strCode, string strName, string strDesignation, string strLoc, int intDeptId, int intwarehseId, string strPhone, string strEmail, string strUserId, string strFlag, int intId)
    {
        SqlCommand dCmd = new SqlCommand("[sp_Staff_Save]", conn);
        dCmd.CommandType = CommandType.StoredProcedure;
        dCmd.Parameters.AddWithValue("@idp", intId);
        dCmd.Parameters.AddWithValue("@staffnop", strCode);
        dCmd.Parameters.AddWithValue("@namep", strName);
        dCmd.Parameters.AddWithValue("@designationp", strDesignation);
        dCmd.Parameters.AddWithValue("@deptidp", intDeptId);

        dCmd.Parameters.AddWithValue("@warehseIdp", intwarehseId);
        dCmd.Parameters.AddWithValue("@locationp", strLoc);
        dCmd.Parameters.AddWithValue("@phonep", strPhone);
        dCmd.Parameters.AddWithValue("@emailp", strEmail);
        dCmd.Parameters.AddWithValue("@useridp", strUserId);
        dCmd.Parameters.AddWithValue("@saveflag", strFlag);
        return dCmd.ExecuteNonQuery();
    }

    public static int SaveUser(SqlConnection conn, string strloginid, string strpassword, int intStaffid, string strCategory, int strUserid, string saveflag, int id, int intComp, int intDept, int intProd, int intSupp, int intStaff, int intUser, int intIn, int intOut, int intReport, int intwarh)
    {
        SqlCommand dCmd = new SqlCommand("sp_User_Save", conn);
        dCmd.CommandType = CommandType.StoredProcedure;
        dCmd.Parameters.AddWithValue("@idp", id);
        dCmd.Parameters.AddWithValue("@loginidp", strloginid);
        dCmd.Parameters.AddWithValue("@passp", strpassword);
        dCmd.Parameters.AddWithValue("@staffidp", intStaffid);
        dCmd.Parameters.AddWithValue("@categoryp", strCategory);
        dCmd.Parameters.AddWithValue("@saveflag", saveflag);
        dCmd.Parameters.AddWithValue("@useridp", strUserid);

        dCmd.Parameters.AddWithValue("@companyp", intComp);
        dCmd.Parameters.AddWithValue("@deptp", intDept);
        dCmd.Parameters.AddWithValue("@prodp", intProd);
        dCmd.Parameters.AddWithValue("@supplierp", intSupp);
        dCmd.Parameters.AddWithValue("@staffp", intStaff);
        dCmd.Parameters.AddWithValue("@userp", intUser);
        dCmd.Parameters.AddWithValue("@incomep", intIn);
        dCmd.Parameters.AddWithValue("@outg", intOut);
        dCmd.Parameters.AddWithValue("@reportp", intReport);
        dCmd.Parameters.AddWithValue("@iwarhp", intwarh);
        return dCmd.ExecuteNonQuery();
    }

    public static int Incoming(SqlConnection conn, int strproductCode, int strsuppliername, double unitprice, int quantity, double totalamt, string dono, DateTime strInsDate, DateTime exprdate, int strUserId, string strFlag, int intId)
    {
        SqlCommand dCmd = new SqlCommand("[sp_Incoming_Save]", conn);
        dCmd.CommandType = CommandType.StoredProcedure;
        dCmd.Parameters.AddWithValue("@idp", intId);
        dCmd.Parameters.AddWithValue("@productCodep", strproductCode);
        dCmd.Parameters.AddWithValue("@suppliernamep", strsuppliername);
        dCmd.Parameters.AddWithValue("@unitpricep", unitprice);
        dCmd.Parameters.AddWithValue("@quantityp", quantity);
        dCmd.Parameters.AddWithValue("@totalamtp", totalamt);
        dCmd.Parameters.AddWithValue("@donop", dono);
        dCmd.Parameters.AddWithValue("@InsDatep", strInsDate);
        
        dCmd.Parameters.AddWithValue("@exprdatep", exprdate);
        dCmd.Parameters.AddWithValue("@useridp", strUserId);
        dCmd.Parameters.AddWithValue("@saveflag", strFlag);
        return dCmd.ExecuteNonQuery();
    }

    public static int ppeissue(SqlConnection conn, int staffno, int productid, int dept, int quantityissue, string IssueDate, int strUserId, string strFlag, int intId, int Balqty, int IncomingID, DateTime ExpiredDate, int IncomingIDDD, string updateflag, int updatequantity, string Issuance, int SafetyBy, string SafetyDate)
    {
        SqlCommand dCmd = new SqlCommand("[sp_PPEIssue_Save]", conn);
        dCmd.CommandType = CommandType.StoredProcedure;
        dCmd.Parameters.AddWithValue("@idp", intId);
        dCmd.Parameters.AddWithValue("@staffnop", staffno);
        dCmd.Parameters.AddWithValue("@deptp", dept);
        dCmd.Parameters.AddWithValue("@productidp", productid);
        dCmd.Parameters.AddWithValue("@quantityissuep", quantityissue);
        dCmd.Parameters.AddWithValue("@IssueDatep", IssueDate);
        dCmd.Parameters.AddWithValue("@useridp", strUserId);
        dCmd.Parameters.AddWithValue("@saveflag", strFlag);
        dCmd.Parameters.AddWithValue("@Balqtyp", Balqty);
        dCmd.Parameters.AddWithValue("@IncomingIDp", IncomingID);
        dCmd.Parameters.AddWithValue("@ExpiredDatep", ExpiredDate);
        dCmd.Parameters.AddWithValue("@IncomingIDDDp", IncomingIDDD);
        dCmd.Parameters.AddWithValue("@updateflagp", updateflag);
        dCmd.Parameters.AddWithValue("@updatequantityp", updatequantity);

        dCmd.Parameters.AddWithValue("@Issuance", Issuance);
        dCmd.Parameters.AddWithValue("@SafetyBy", SafetyBy);
        dCmd.Parameters.AddWithValue("@SafetyDate", SafetyDate);
        return dCmd.ExecuteNonQuery();
    }

    public static int Savewarehouse(SqlConnection conn, int companyid, string warehousename, string address1, string address2, string city, string state, string country, string postcode, string desc, string phone, string fax, string email, string website, string userid, string saveflag, int warehouseid)
    {
        SqlCommand dCmd = new SqlCommand("[sp_Warehouse_Save]", conn);
        dCmd.CommandType = CommandType.StoredProcedure;
        dCmd.Parameters.AddWithValue("@idp", warehouseid);
        dCmd.Parameters.AddWithValue("@companyidp", companyid);
        dCmd.Parameters.AddWithValue("@warehousenamep", warehousename);
        dCmd.Parameters.AddWithValue("@address1p", address1);
        dCmd.Parameters.AddWithValue("@address2p", address2);
        dCmd.Parameters.AddWithValue("@cityp", city);
        dCmd.Parameters.AddWithValue("@statep", state);
        dCmd.Parameters.AddWithValue("@countryp", country);
        dCmd.Parameters.AddWithValue("@descriptionp", desc);
        dCmd.Parameters.AddWithValue("@contactnop", phone);
        dCmd.Parameters.AddWithValue("@faxnop", fax);
        dCmd.Parameters.AddWithValue("@emailp", email);
        dCmd.Parameters.AddWithValue("@websitep", website);
        dCmd.Parameters.AddWithValue("@postcodep", postcode);
        dCmd.Parameters.AddWithValue("@useridp", userid);
        dCmd.Parameters.AddWithValue("@saveflag", saveflag);
        return dCmd.ExecuteNonQuery();
    }

}
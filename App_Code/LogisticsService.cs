using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.OleDb;
using System.Text;

/// <summary>
/// This service contains logistics related WS.
/// </summary>
[WebService(Namespace = "http://www.tpifc.com/TPWebServices")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class LogisticsService : System.Web.Services.WebService
{

    public LogisticsService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    DownloadFile file = new DownloadFile();
    private string GetConectionString(string url)
    {
        Dictionary<string, string> props = new Dictionary<string, string>();
        props["Provider"] = "Microsoft.ACE.OLEDB.12.0";
        props["Extended Properties"] = "Excel 12.0 XML";
        props["Data Source"] = file.DownloadFileTemp(url); //File Source C:\Users\rhrlg\Downloads/portcode2012.xls

        StringBuilder sb = new StringBuilder();
        foreach (KeyValuePair<string, string> prop in props)
        {
            sb.Append(prop.Key);
            sb.Append('=');
            sb.Append(prop.Value);
            sb.Append(';');
        }
        return sb.ToString();
    }
    //Read Excel(.xls) file and return a DataSet that contains global sea port information.
    [WebMethod]
    public DataSet GetAllSeaPortInformation()
    {
        DataSet ds = new DataSet();
        ds = ReadExcelData(System.Web.Configuration.WebConfigurationManager.AppSettings["PortListUrl"]);
        return ds;
    }
    //Search port information based on selected category from the Excel file
    [WebMethod]
    public DataSet SearchSeaPortInformation(string searchBy, string searchText)
    {
        DataSet ds = new DataSet();
        ds = SearchExcelData(System.Web.Configuration.WebConfigurationManager.AppSettings["PortListUrl"]
            ,searchBy, searchText);
        return ds;
    }

    [WebMethod]
    public DataSet GetAllHSCodeInformation()
    {
        DataSet ds = new DataSet();
        ds = ReadExcelData(System.Web.Configuration.WebConfigurationManager.AppSettings["HSCodeListUrl"]);
        return ds;
    }

    [WebMethod]
    public PostalPrice calculateLocalPostMailPrice(string post_size, double weight)
    {
        PostalPrice postalPrice = new PostalPrice();
        postalPrice.result = true;

        double total = 0.0;
        string key = "LocalRate_" + post_size;
        int rate_key = 0;

        string[] rate_string = System.Web.Configuration.WebConfigurationManager.AppSettings[key].Split(',');

        if (weight <= 20)
            rate_key = 0;
        else if (20 < weight && weight <= 40)
            rate_key = 1;
        else if (40 < weight && weight <= 100)
            rate_key = 2;
        else if (100 < weight && weight <= 250)
            rate_key = 3;
        else if (250 < weight && weight <= 500)
            rate_key = 4;
        else if (500 < weight && weight <= 1000)
            rate_key = 5;
        else if (1000 < weight && weight <= 2000)
            rate_key = 6;

        if (rate_key + 1 <= rate_string.Length)
            total = Convert.ToDouble(rate_string[rate_key]);

        else if (rate_key + 1 > rate_string.Length)
            postalPrice.result = false;

        postalPrice.price = total;
        return postalPrice;
    }

    //Read Excel Function
    private DataSet ReadExcelData(string url)
    {
        DataSet ds = new DataSet();
        string connectionString = GetConectionString(url);
        using (OleDbConnection conn = new OleDbConnection(connectionString))
        {
            conn.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = conn;

            //Get all Sheets in Excel File
            DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            //Loop through all Sheets to get data
            foreach (DataRow dr in dtSheet.Rows)
            {
                string sheetName = dr["TABLE_NAME"].ToString();

                cmd.CommandText = "SELECT * FROM [" + sheetName + "]";
                DataTable dt = new DataTable();
                dt.TableName = sheetName;
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt);

                ds.Tables.Add(dt);
            }

            cmd = null;
            conn.Close();
        }

        return ds;
    }

    //Search from Excel fucntion
    private DataSet SearchExcelData(string url, string searchBy, string searchText)
    {
        DataSet ds = new DataSet();
        string connectionString = GetConectionString(url);
        using (OleDbConnection conn = new OleDbConnection(connectionString))
        {
            conn.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = conn;

            //Get all Sheets in Excel File
            DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            //Loop through all Sheets to get data
            foreach (DataRow dr in dtSheet.Rows)
            {
                string sheetName = dr["TABLE_NAME"].ToString();

                cmd.CommandText = "SELECT * FROM [" + sheetName + "] WHERE [" + searchBy + "]='" + searchText + "'";
                DataTable dt = new DataTable();
                dt.TableName = sheetName;
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt);

                ds.Tables.Add(dt);
            }

            cmd = null;
            conn.Close();
        }
        return ds;
    }

}

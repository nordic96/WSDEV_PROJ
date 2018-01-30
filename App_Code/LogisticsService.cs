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


    //[WebMethod]


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

    //Calculate the post mail charge in local area (SG).
    [WebMethod]
    public PostalPrice CalculatePostRateLocal(string post_size, double weight)
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
    //Calculate the post mail charge for overseas by surface transport.
    [WebMethod]
    public PostalPrice CalculatePostRateSurface(string mailType, double weight)
    {
        PostalPrice p = new PostalPrice();
        double totalSurfaceRate = 0;
        bool status = true;

        if (mailType.Equals("papers"))
        {
            if (0 < weight && weight <= 20)
                totalSurfaceRate = 0.50;
            else if (20 < weight && weight <= 50)
                totalSurfaceRate = 0.70;
            else if (50 < weight && weight <= 100)
                totalSurfaceRate = 1;
            else if (weight > 100 && weight <= 2000)
                totalSurfaceRate = 1 + (1 * (Math.Ceiling((weight - 100) / 100)));

            else
                status = false;
        }

        p.price = totalSurfaceRate;
        p.result = status;
        return p;
    }
    //Calculate the post mail charge for overseas by air transport.
    [WebMethod]
    public PostalPrice CalculatePostRateAir(string country_code, string mailType, double weight)
    {
        PostalPrice p = new PostalPrice();
        double totalAirRate = 0;
        bool status = true;

        int zone = get_zone_no(country_code);
        if (mailType.Equals("papers"))
        {
            if (zone == 1)
            {
                if (0 < weight && weight <= 20)
                    totalAirRate = 0.50;
                else if (20 < weight && weight <= 50)
                    totalAirRate = 0.70;
                else if (50 < weight && weight <= 100)
                    totalAirRate = 1.10;
                else if (weight > 100 && weight <= 2000)
                {
                    totalAirRate = 1.10 + (1.10 * (Math.Ceiling((weight - 100) / 100)));
                }
                else
                    status = false;
            }
            else if (zone == 2)
            {
                if (0 < weight && weight <= 20)
                    totalAirRate = 0.70;
                else if (weight > 20 && weight <= 2000)
                    totalAirRate = 0.70 + (0.25 * (Math.Ceiling((weight - 20) / 10)));
                else
                    status = false;
            }
            else if (zone == 3)
            {
                if (0 < weight && weight <= 20)
                    totalAirRate = 1.30;
                else if (weight > 20 && weight <= 2000)
                    totalAirRate = 1.30 + (0.35 * (Math.Ceiling((weight - 20) / 10)));
                else
                    status = false;
            }
        }
        else if (mailType.Equals("packets"))
        {
            if (zone == 1)
            {
                if (0 < weight && weight <= 100)
                    totalAirRate = 2.50;
                else if (100 < weight && weight <= 250)
                    totalAirRate = 3.90;
                else if (250 < weight && weight <= 500)
                    totalAirRate = 5.20;
                else if (weight > 500 && weight <= 2000)
                {
                    totalAirRate = 5.20 + (1.10 * (Math.Ceiling((weight - 500) / 100)));
                }
                else
                    status = false;
            }
            else if (zone == 2)
            {
                if (0 < weight && weight <= 100)
                    totalAirRate = 3.20;
                else if (100 < weight && weight <= 250)
                    totalAirRate = 6.80;
                else if (250 < weight && weight <= 500)
                    totalAirRate = 12.00;
                else if (weight > 500 && weight <= 2000)
                {
                    totalAirRate = 12.00 + (2.50 * (Math.Ceiling((weight - 500) / 100)));
                }
                else
                    status = false;
            }
            else if (zone == 3)
            {
                if (0 < weight && weight <= 100)
                    totalAirRate = 4.70;
                else if (100 < weight && weight <= 250)
                    totalAirRate = 9.85;
                else if (250 < weight && weight <= 500)
                    totalAirRate = 17.00;
                else if (weight > 500 && weight <= 2000)
                {
                    totalAirRate = 17.00 + (3.50 * (Math.Ceiling((weight - 500) / 100)));
                }
                else
                    status = false;
            }
        }

        p.price = totalAirRate;
        p.result = status;
        return p;
    }

    [WebMethod]
    public PostalPrice CalculatePostRateBulk(string transport_mode, string destination, double weight)
    {
        PostalPrice p = new PostalPrice();
        bool result = true;
        string zone_area = "";
        zone_area = get_zone_area(destination);
        List<ZoneRateList> zone_rate_lists = new List<ZoneRateList>();

        double totalRate = 0, first5Kg = 0, addlKg = 0;

        string[] zone_A_rate = "16;3;NA;NA".Split(';');
        string[] zone_B_rate = "30;5;18;2".Split(';');
        string[] zone_C_rate = "30;5;18;2".Split(';');
        string[] zone_R_rate = "40;7;20;2".Split(';');
        string[] zone_S_rate = "50;9;25;2".Split(';');
        string[] zone_T_rate = "50;9;25;2".Split(';');

        ZoneRateList a = new ZoneRateList("A", zone_A_rate);
        ZoneRateList b = new ZoneRateList("B", zone_B_rate);
        ZoneRateList c = new ZoneRateList("C", zone_C_rate);
        ZoneRateList r = new ZoneRateList("R", zone_R_rate);
        ZoneRateList s = new ZoneRateList("S", zone_S_rate);
        ZoneRateList t = new ZoneRateList("T", zone_T_rate);

        zone_rate_lists.Add(a);
        zone_rate_lists.Add(b);
        zone_rate_lists.Add(c);
        zone_rate_lists.Add(r);
        zone_rate_lists.Add(s);
        zone_rate_lists.Add(t);

        foreach (ZoneRateList list in zone_rate_lists)
        {
            if (list.zone_name.Equals(zone_area))
            {
                if (transport_mode.Equals("air"))
                {
                    first5Kg = Convert.ToDouble(list.zone_rate[0]);
                    addlKg = Convert.ToDouble(list.zone_rate[1]);
                }
                else if (transport_mode.Equals("surface"))
                {
                    if (zone_area.Equals("A"))
                    {
                        result = false;
                    }

                    else
                    {
                        first5Kg = Convert.ToDouble(list.zone_rate[2]);
                        addlKg = Convert.ToDouble(list.zone_rate[3]);
                    }
                }
                else
                    result = false;
            }
        }

        //Calculation
        if (0 < weight && weight <= 5)
        {
            totalRate = first5Kg;
        }
        else if (weight > 5 && weight <= 30)
        {
            totalRate = first5Kg + (Math.Ceiling(weight - 5) * addlKg);
        }
        else
            result = false;

        p.price = totalRate;
        p.result = result;
        return p;
    }

    [WebMethod]
    public Duty CalculateIntnProductDuty(string HSCode, double weight, double totalPrice)
    {
        ExcelRead excel = new ExcelRead();
        DataSet hrcodes = new DataSet();
        Duty duty = new Duty();
        bool result = true;
        double totPriceCalculation = 0;

        hrcodes = SearchHsCode("List of Dutiable Goods ", HSCode);
        string calculationCustomRate = hrcodes.Tables[0].Rows[0][2].ToString();
        string calculationExciseRate = hrcodes.Tables[0].Rows[0][3].ToString();
        double totalproductprice = totalPrice;


        char[] MyChar = { ' ', 'p', 'c', 'e' };
        string subC = calculationCustomRate.Substring(0, 1);
        if (subC.Equals("$")) //CHECKING FOR EXCISE DUTY THAT STARTS WITH '$'
        {
            string subC2 = calculationCustomRate.Substring(0, 7);
            string newsubC2 = subC2.Remove(0, 1);
            string NewString = newsubC2.TrimEnd(MyChar).ToString();
            double customDutiesValue = Convert.ToDouble(NewString);
            totPriceCalculation = customDutiesValue * weight;
        }

        else if (subC == "N")
        {
            result = false;
        }

        duty.totalDuties = totPriceCalculation;
        duty.result = result;
        return duty;
    }

    [WebMethod]
    public Duty CalculateDomesticProductDuty(string HSCode, double weight, double totalPrice)
    {
        ExcelRead excel = new ExcelRead();
        DataSet hrcodes = new DataSet();
        Duty duty = new Duty();
        bool result = true;
        double totPriceCalculation = 0;

        hrcodes = SearchHsCode("List of Dutiable Goods ", HSCode);
        string calculationCustomRate = hrcodes.Tables[0].Rows[0][2].ToString();
        string calculationExciseRate = hrcodes.Tables[0].Rows[0][3].ToString();
        double totalproductprice = totalPrice;


        char[] MyChar = { ' ', 'p', 'c', 'e' };
        string subC = calculationCustomRate.Substring(0, 1);
        string subE = calculationExciseRate.Substring(0, 1);
        if (subC.Equals("$")) //CHECKING FOR EXCISE DUTY THAT STARTS WITH '$'
        {
            string subC2 = calculationCustomRate.Substring(0, 7);
            string newsubC2 = subC2.Remove(0, 1);
            string NewString = newsubC2.TrimEnd(MyChar).ToString();
            double customDutiesValue = Convert.ToDouble(NewString);
            //totPriceCalculation = customDutiesValue * weight;

            if (subE.Equals("$")) //CHECKING FOR EXCISE DUTY THAT STARTS WITH '$'
            {
                string subE2 = calculationExciseRate.Substring(0, 7);
                string newsubE2 = subE2.Remove(0, 1);
                string NewStringE = newsubE2.TrimEnd(MyChar).ToString();
                double exciseDutiesValue = Convert.ToDouble(NewStringE);
                totPriceCalculation = (customDutiesValue * weight) + (exciseDutiesValue * weight);
            }

            else if (calculationExciseRate.Contains("cents")) //CHECKING FOR EXCISE DUTY THAT CONTAINS 'CENTS'
            {
                string subE2 = calculationExciseRate.Substring(0, 4);
                string NewStringE = subE2.TrimEnd(MyChar).ToString();
                double exciseDutiesValue = Convert.ToDouble(NewStringE);
                totPriceCalculation = (customDutiesValue * weight) + ((exciseDutiesValue / 100) * weight);
            }

            else if (calculationExciseRate.Contains("%")) //CHECKING FOR EXCISE DUTY THAT CONTAINS '%'
            {
                string subE2 = calculationExciseRate.Substring(0, 2);
                double exciseDutiesValue = Convert.ToDouble(subE2);
                totPriceCalculation = (customDutiesValue * weight) + (exciseDutiesValue / 100) * (totalproductprice);
            }

        }

        else if (subC == "N")
        {
            result = false;
            if (result == false)
            {
                if (subE.Equals("$")) //CHECKING FOR EXCISE DUTY THAT STARTS WITH '$'
                {
                    string subE2 = calculationExciseRate.Substring(0, 7);
                    string newsubE2 = subE2.Remove(0, 1);
                    string NewStringE = newsubE2.TrimEnd(MyChar).ToString();
                    double exciseDutiesValue = Convert.ToDouble(NewStringE);
                    totPriceCalculation = exciseDutiesValue * weight;
                }

                else if (calculationExciseRate.Contains("cents")) //CHECKING FOR EXCISE DUTY THAT CONTAINS 'CENTS'
                {
                    string subE2 = calculationExciseRate.Substring(0, 4);
                    string NewStringE = subE2.TrimEnd(MyChar).ToString();
                    double exciseDutiesValue = Convert.ToDouble(NewStringE);
                    totPriceCalculation = (exciseDutiesValue / 100) * weight;
                }

                else if (calculationExciseRate.Contains("%")) //CHECKING FOR EXCISE DUTY THAT CONTAINS '%'
                {
                    string subE2 = calculationExciseRate.Substring(0, 2);
                    double exciseDutiesValue = Convert.ToDouble(subE2);
                    totPriceCalculation = (exciseDutiesValue / 100) * (totalproductprice);
                }
            }
        }

        duty.totalDuties = totPriceCalculation;
        duty.result = result;
        return duty;
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

    [WebMethod]
    public DataSet SearchHsCode(string searchBy, string searchText)
    {
        DataSet ds = new DataSet();
        string connectionString = GetConectionString("https://www.customs.gov.sg/~/media/cus/files/business/valuation%20duties%20taxes%20and%20fees/list%20of%20dutiable%20goods20feb2017.xlsx?la=en");
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

                cmd.CommandText = "SELECT * FROM [" + sheetName + "] WHERE [" + searchBy + "]=" + Convert.ToInt32(searchText);
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
    //For Airmail Post Rate Get Country Zone Number
    private int get_zone_no(string select)
    {
        string[] zone1 = System.Web.Configuration.WebConfigurationManager.AppSettings["AirMail_Zone1"].Split(';'),
zone2 = System.Web.Configuration.WebConfigurationManager.AppSettings["AirMail_Zone2"].Split(';');

        int zoneNumber = 3;

        for (int i = 0; i < zone1.Length; i++)
        {
            if (select.Equals(zone1[i]))
                zoneNumber = 1;
        }
        for (int i = 0; i < zone2.Length; i++)
        {
            if (select.Equals(zone2[i]))
                zoneNumber = 2;
        }

        return zoneNumber;
    }

    private string get_zone_area(string select)
    {
        string zone_area = "";

        string[] zone_A = System.Web.Configuration.WebConfigurationManager.AppSettings["Bulk_Zone_A"].Split(';');
        string[] zone_B = System.Web.Configuration.WebConfigurationManager.AppSettings["Bulk_Zone_B"].Split(';');
        string[] zone_C = System.Web.Configuration.WebConfigurationManager.AppSettings["Bulk_Zone_C"].Split(';');
        string[] zone_R = System.Web.Configuration.WebConfigurationManager.AppSettings["Bulk_Zone_R"].Split(';');
        string[] zone_S = System.Web.Configuration.WebConfigurationManager.AppSettings["Bulk_Zone_S"].Split(';');
        string[] zone_T = System.Web.Configuration.WebConfigurationManager.AppSettings["Bulk_Zone_T"].Split(';');

        ZoneList a = new ZoneList("A", zone_A);
        ZoneList b = new ZoneList("B", zone_B);
        ZoneList c = new ZoneList("C", zone_C);
        ZoneList r = new ZoneList("R", zone_R);
        ZoneList s = new ZoneList("S", zone_S);
        ZoneList t = new ZoneList("T", zone_T);

        List<ZoneList> zone_lists = new List<ZoneList>();
        zone_lists.Add(a);
        zone_lists.Add(b);
        zone_lists.Add(c);
        zone_lists.Add(r);
        zone_lists.Add(s);
        zone_lists.Add(t);

        foreach (ZoneList zone_list in zone_lists)
        {
            for (int i = 0; i < zone_list.countries.Length; i++)
            {
                if (zone_list.countries[i].Equals(select))
                {
                    zone_area = zone_list.zone_name;
                }
            }
        }
        return zone_area;
    }
}

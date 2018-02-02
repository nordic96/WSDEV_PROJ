using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Data;
using System.Net;
using ExcelDataReader;

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

    //Read Excel(.xls) file and return a DataSet that contains global sea port information.
    [WebMethod 
    (Description = "Return a DataTable of global sea port information (Country Code, Country Name, Port Code, Port Name).")]
    public DataSet GetAllSeaPortInformation()
    {
        DataSet ds = new DataSet();
        ds = ExcelReadData_EP("http://www.pscoman.com/Portals/0/documents/portcode2012.xls", 8); //Using new Excel Reader library
        return ds;
    }

    /*
     * 31/1/2018
     * KoGihun
    */
    [WebMethod
    (Description = "Search port information based on selected category from the Excel file. " +
    "searchBy must be either 'Country Code', 'Port Code', 'Country Name', 'Port Name', 'Port Code'. " +
    "searchBy and searchText must be case-sensitive.")]
    public DataTable SearchSeaPortInformation(string searchBy, string searchText)
    {
        DataTable dt = new DataTable();
        //string searchText_final = "";

        dt = ExcelSearchData_EP("http://www.pscoman.com/Portals/0/documents/portcode2012.xls", 8, searchBy, searchText);

        return dt;
    }

    [WebMethod]
    public DataSet GetAllHSCodeInformation()
    {
        DataSet ds = new DataSet();
        //Replaced with Excel Data Reader library instead of oleDb.
        ds = ExcelReadData_EP("https://data.gov.in/sites/default/files/datafile/itchs2012.xls", 1);
        return ds;
    }

    //Calculate the post mail charge in local area (SG).
    [WebMethod
    (Description = "post_size must be either 'regular'/'large'/'non' (none for Non-Standard). weight is measured in (g). " +
        "Maximum weight is 2000g (for regular size, max. weight is 40g) Invalid weight amount will return result as false.")]
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
    [WebMethod
    (Description = "mailType can be only 'papers' available. weight is measaured in (g). (max. weight is 2000g)")]
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
    [WebMethod (Description = "country_code (2 characters ex: MY for Malaysia). mailType only allows 'papers'/'packets'. " +
        "weight is in (g). Any fail in validation, the result value will be set to false.")]
    public PostalPrice CalculatePostRateAir(string country_code, string mailType, double weight)
    {
        PostalPrice p = new PostalPrice();
        double totalAirRate = 0;
        bool status = true;

        if(country_code.Length.Equals(2))
        {
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
                else //if no zone number matches
                    status = false;
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
            else
            {
                status = false;
            }
        }
        //Else country code is not 2 chars
        else
        {
            status = false;
        }


        p.price = totalAirRate;
        p.result = status;
        return p;
    }

    [WebMethod (Description = "transport_mode must be either 'air'/'surface'. destination is for country code(2char; eg.MY for Malaysia). weight is measured in (kg).")]
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
        //ExcelRead excel = new ExcelRead();
        DataSet hrcodes = new DataSet();
        Duty duty = new Duty();
        bool result = true;
        double totPriceCalculation = 0;

        //hrcodes = SearchHsCode("List of Dutiable Goods ", HSCode);
        //string calculationCustomRate = hrcodes.Tables[0].Rows[0][2].ToString();
        //string calculationExciseRate = hrcodes.Tables[0].Rows[0][3].ToString();
        //double totalproductprice = totalPrice;


        //char[] MyChar = { ' ', 'p', 'c', 'e' };
        //string subC = calculationCustomRate.Substring(0, 1);
        //if (subC.Equals("$")) //CHECKING FOR EXCISE DUTY THAT STARTS WITH '$'
        //{
        //    string subC2 = calculationCustomRate.Substring(0, 7);
        //    string newsubC2 = subC2.Remove(0, 1);
        //    string NewString = newsubC2.TrimEnd(MyChar).ToString();
        //    double customDutiesValue = Convert.ToDouble(NewString);
        //    totPriceCalculation = customDutiesValue * weight;
        //}

        //else if (subC == "N")
        //{
        //    result = false;
        //}

        //duty.totalDuties = totPriceCalculation;
        //duty.result = result;
        return duty;
    }

    [WebMethod]
    public Duty CalculateDomesticProductDuty(string HSCode, double weight, double totalPrice)
    {
        //ExcelRead excel = new ExcelRead();
        DataSet hrcodes = new DataSet();
        Duty duty = new Duty();
        bool result = true;
        double totPriceCalculation = 0;

        //Change the SearchHsCode - GH
        //hrcodes = SearchHsCode("List of Dutiable Goods ", HSCode);
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

    private DataSet ExcelReadData_EP(string url, int row_to_start) //If header is located at first row, put 1
    {
        DownloadFile file = new DownloadFile();
        WebClient client = new WebClient();
        DataSet ds = new DataSet();

        using (var stream = client.OpenRead(file.DownloadFileTemp(url)))
        {

            // Auto-detect format, supports:
            //  - Binary Excel files (2.0-2003 format; *.xls)
            //  - OpenXml Excel files (2007 format; *.xlsx)
            using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
            {

                // 2. Use the AsDataSet extension method
                ds = reader.AsDataSet(new ExcelDataSetConfiguration // if 5th row is header, read 4 times. if 2nd row is header, read 1 time;
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true,
                        ReadHeaderRow = (rowReader) => {
                            //loop implemented by Gihun
                            for (int i = 0; i < row_to_start - 1; i++)
                            {
                                rowReader.Read();
                            }
                        }
                    }
                });
                // The result of each spreadsheet is in result.Tables
            }
        }
        return ds;
    }

    /*
     * Only string values are available
    */
    private DataTable ExcelSearchData_EP(string url, int row_to_start, string search_by, string search_text)
    {
        DataSet ds_whole = new DataSet();
        DataTable ds_result = new DataTable();
        DataTable dt = new DataTable();

        ds_whole = ExcelReadData_EP(url, row_to_start);
        ds_result = ds_whole.Tables[0];

        IEnumerable<DataRow> query =
            from port in ds_result.AsEnumerable()
            where port.Field<string>(search_by) == search_text
            select port;

        //If query is not empty search result will be copied into data table.
        if(query.Any())
        {
            dt = query.CopyToDataTable<DataRow>();
            dt.TableName = "SearchInfoList";
        }
        else
        {
            dt.TableName = "EmptySearchInfoList";
        }
        // Create a table from the query.

        return dt;
    }

    //Used by Jacky;
    [WebMethod]
    public DataTable GetAllDutiableTaxHSCode()
    {
        int row_to_start = 1;
        string url = "https://drive.google.com/uc?export=download&id=1bvu9u09zqzQ4oTRSHRP2CQYUkasxudJw";
        DownloadFile file = new DownloadFile();
        WebClient client = new WebClient();
        DataSet ds = new DataSet();
        DataTable ds2 = new DataTable("DutiableTaxHSCode");

        using (var stream = client.OpenRead(file.DownloadFileTemp(url)))
        {

            // Auto-detect format, supports:
            //  - Binary Excel files (2.0-2003 format; *.xls)
            //  - OpenXml Excel files (2007 format; *.xlsx)
            using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
            {

                // 2. Use the AsDataSet extension method
                ds = reader.AsDataSet(new ExcelDataSetConfiguration // if 5th row is header, read 4 times. if 2nd row is header, read 1 time;
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        EmptyColumnNamePrefix = "A",

                        UseHeaderRow = true,
                        ReadHeaderRow = (rowReader) => {
                            for (int i = 0; i < row_to_start - 1; i++)
                            {
                                rowReader.Read();
                            }
                        }
                    }
                });
                // The result of each spreadsheet is in result.Tables
            }
        }
        ds2.Columns.Add("HS Code", typeof(string));
        ds2.Columns.Add("Product Description", typeof(string));
        ds2.Columns.Add("Customs Duty", typeof(string));
        ds2.Columns.Add("Excise Duty", typeof(string));
        ds2.Load(ds.CreateDataReader(), System.Data.LoadOption.OverwriteChanges);
        return ds2;
    }

    [WebMethod]
    public DataTable SearchDutiableTaxHsCode(string searchBy, string searchText)
    {
        DataSet ds_whole = new DataSet();
        DataTable ds_result = new DataTable();
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable("final");
        IEnumerable<DataRow> query = null;

        ds_whole = ExcelReadData_EP("https://drive.google.com/uc?export=download&id=1bvu9u09zqzQ4oTRSHRP2CQYUkasxudJw", 1);
        ds_result = ds_whole.Tables[0];

        //If Else to check whether searchby is HS Code or Product Description.
        if(searchBy.Equals("HS Code"))
        {
            query =
                from port in ds_result.AsEnumerable()
                where port.Field<double>(searchBy) == Convert.ToDouble(searchText)
                select port;
        }
        else if (searchBy.Equals("Product Description"))
        {
            query =
                from port in ds_result.AsEnumerable()
                where port.Field<string>(searchBy).Contains(searchText)
                select port;
        }

        if (query.Any())
        {
            dt = query.CopyToDataTable<DataRow>();
            dt.TableName = "SearchInfoList";
        }
        else
        {
            dt.TableName = "EmptySearchInfoList";
        }

        dt2.Columns.Add("HS Code", typeof(string));
        dt2.Columns.Add("Product Description", typeof(string));
        dt2.Columns.Add("Customs Duty", typeof(string));
        dt2.Columns.Add("Excise Duty", typeof(string));
        dt2.Load(dt.CreateDataReader(), System.Data.LoadOption.OverwriteChanges);
        return dt2;
    }

    //For Airmail Post Rate Get Country Zone Number
    private int get_zone_no(string select)
    {
        string[] zone1 = "MY;BN".Split(';'),
                 zone2 = "KR;KP;CN;IN;VN;IL;TH;IR;SA;SY;HK;PK;PH;ID;MV;MM;IQ;LK;QA;BD;YE;TW;KH;AE;LB;AF;PS;NP;OM;MO;UZ;JO;AZ;MN;KW;BT;BH;AM;KG;TM;TJ;TL;CX;IO;CC".Split(';'),
                 zone3 = "NZ;AU;US;FR;GB;RU;MA;BG;ES;DE;PL;RO;UA;BY;GR".Split(';') ;

        int zoneNumber = 0;

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
        for (int i=0; i<zone3.Length; i++)
        {
            if (select.Equals(zone3[i]))
                zoneNumber = 3;
        }

        return zoneNumber;
    }

    private string get_zone_area(string select)
    {
        string zone_area = "";

        string[] zone_A = "MY".Split(';');
        string[] zone_B = "BN;HK;ID;PH;TW;TH".Split(';');
        string[] zone_C = "CN;IN;KR".Split(';');
        string[] zone_R = "AU;JP;NZ".Split(';');
        string[] zone_S = "GB;DE".Split(';');
        string[] zone_T = "CF;ZA;ZW;BW".Split(';');

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

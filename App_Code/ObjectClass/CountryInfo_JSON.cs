using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CountryInfo
/// </summary>
/// 
public class CountryInfoList
{
    public RestResponse RestResponse { get; set; }
}

public class RestResponse
{
    public string[] messages { get; set; }
    public CountryInfo[] result { get; set; }
}

public class CountryInfo
{
    public string name { get; set; }
    public string alpha2_code { get; set; }
    public string alpha3_code { get; set; }
}




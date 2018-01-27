using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PostRate
/// </summary>
public class PostalPrice
{
    public double price { get; set; }
    public bool result { get; set; }


    public PostalPrice()
    {

    }
}

public class ZoneList
{
    public string zone_name { get; set; }
    public string[] countries { get; set; }

    public ZoneList(string zone_name, string[] countries)
    {
        this.zone_name = zone_name;
        this.countries = countries;
    }
}

public class ZoneRateList
{
    public string zone_name { get; set; }
    public string[] zone_rate { get; set; }

    public ZoneRateList (string zone_name, string[] zone_rate)
    {
        this.zone_name = zone_name;
        this.zone_rate = zone_rate;
    }
}

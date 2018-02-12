using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

/// <summary>
/// Summary description for Bus
/// </summary>
public class BusList
{
    public string busNo { get; set; }
    public string arrivalTime1 { get; set; }
    public string arrivalTime2 { get; set; }
    public string arrivalTime3 { get; set; }
}

public class BusStops
{
    public string BusStopCode { get; set; }
    public string RoadName { get; set; }
    public string Description { get; set; }
}
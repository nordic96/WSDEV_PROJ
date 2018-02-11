using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Bus
/// </summary>
public class Bus
{
    public string ServiceNo { get; set; }
    public string Operator { get; set; }
    public string NextBus { get; set; }
    public string NextBus2 { get; set; }
    public string NextBus3 { get; set; }
    public Bus_Result[] bus_results { get; set; }
}

public class Bus_Result
{
    public string OriginCode { get; set; }
    public string DestinationCode { get; set; }
    public string EstimatedArrival { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public string VisitNumber { get; set; }
    public string Load { get; set; }
    public string Feature { get; set; }
    public string Type { get; set; }
}
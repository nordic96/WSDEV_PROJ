using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for AirCargoData
/// </summary>
public class AirCargoData
{
    public string help { get; set; }
    public bool success { get; set; }
    public Result result { get; set; }

    public AirCargoData()
    {

    }
}

public class Result
{
    public string resource_id { get; set; }
    public Field[] field { get; set; }
    public Record[] records { get; set; }
    public Link _links { get; set; }
    public int limit { get; set; }
    public int offset { get; set; }
    public int total { get; set; }
}

public class Field
{
    public string type { get; set; }
    public string id { get; set; }
}


public class Record
{
    public string value { get; set; }
    public DateTime month { get; set; }
    public int _id { get; set; }
    public string level_1 { get; set; }
    public string level_2 { get; set; }
    public string level_3 { get; set; }
}

public class Link
{
    public string start { get; set; }
    public string next { get; set; }
}

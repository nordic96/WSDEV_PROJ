using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for OneMap_JSON
/// </summary>
public class OneMapData
{
    public int found { get; set; }
    public int totalNumPages { get; set; }
    public int pageNum { get; set; }
    public OneMapData_Result[] results { get; set; }
}

public class OneMapData_Result
{
    public string searchval { get; set; }
    public string blk_no { get; set; }
    public string road_name { get; set; }
    public string building { get; set; }
    public string address { get; set; }
    public string postal { get; set; }
    //public string x { get; set; }
    //public string y { get; set; }
    //public string latitude { get; set; }
    //public string longitude { get; set; }
    //public string longtitude { get; set; }
}
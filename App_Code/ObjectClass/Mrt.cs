using System;
using System.Collections.Generic;
using System.Xml.Serialization;

/// <summary>
/// Summary description for Mrt
/// </summary>
[XmlRoot("root")]
public class Root
{
    [XmlElement("Line")]
    public Line[] Lines { get; set; }
}

public class Line
{
    [XmlElement("row")]
    public row[] rows { get; set; }

    [XmlAttribute("LineName")]
    public string LineName { get; set; }
}

public class row
{
    [XmlElement("stn_code")]
    public string stn_code { get; set; }

    [XmlElement("mrt_station_english")]
    public string mrt_station_english { get; set; }

    [XmlElement("mrt_station_chinese")]
    public string mrt_station_chinese { get; set; }

    [XmlElement("mrt_line_english")]
    public string mrt_line_english { get; set; }

    [XmlElement("mrt_line_chinese")]
    public string mrt_line_chinese { get; set; }
}


//This is for arrow image
public class arrow
{
    public string arrow_url { get; set; }

    public arrow(string arrow_url)
    {
        this.arrow_url = arrow_url;
    }
}



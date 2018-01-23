﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using System.Data;
using System.Text;

/// <summary>
/// Summary description for ExcelRead
/// </summary>
public class ExcelRead
{
    DownloadFile file = new DownloadFile();
    private string GetConectionString(string url)
    {
        Dictionary<string, string> props = new Dictionary<string, string>();
        props["Provider"] = "Microsoft.ACE.OLEDB.12.0";
        props["Extended Properties"] = "Excel 12.0 XML";
        props["Data Source"] = file.DownloadFileTemp(url); //File Source C:\Users\rhrlg\Downloads/portcode2012.xls

        StringBuilder sb = new StringBuilder();
        foreach(KeyValuePair<string,string> prop in props)
        {
            sb.Append(prop.Key);
            sb.Append('=');
            sb.Append(prop.Value);
            sb.Append(';');
        }
        return sb.ToString();
    }
    public DataSet ExcelReadData(string url)
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
            foreach(DataRow dr in dtSheet.Rows)
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
        //
        // TODO: Add constructor logic here
        //
    }
}
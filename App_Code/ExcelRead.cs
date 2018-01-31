using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using System.Data;
using System.Text;
using System.IO;
using System.Net;
using ExcelDataReader;

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
    public DataSet ExcelSearchData(string url, string searchBy, string searchText)
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

                cmd.CommandText = "SELECT * FROM [" + sheetName + "] WHERE [" + searchBy + "]='" + searchText+"'";
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

    public DataSet ExcelReadData_EP(string url, int row_to_start) //If header is located at first row, put 1
    {
        DownloadFile file = new DownloadFile();
        WebClient client = new WebClient();
        DataSet ds = new DataSet();

        using (var stream = client.OpenRead(file.DownloadFileTemp(url)))
        {

            // Auto-detect format, supports:
            //  - Binary Excel files (2.0-2003 format; *.xls)
            //  - OpenXml Excel files (2007 format; *.xlsx)
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                //do
                //{
                //    while (reader.Read())
                //    {
                //        // reader.GetDouble(0);
                //        if (reader.Depth.Equals(row_to_start))
                //        {
                //            reader.NextResult();
                //        }
                //        else
                //        {
                //            continue;
                //        }
                //    }
                //} while (reader.NextResult());

                // 2. Use the AsDataSet extension method
                ds = reader.AsDataSet(new ExcelDataSetConfiguration // if 5th row is header, read 4 times. if 2nd row is header, read 1 time;
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true,
                        ReadHeaderRow = (rowReader) => {
                            for(int i=0; i < row_to_start - 1; i++)
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
}
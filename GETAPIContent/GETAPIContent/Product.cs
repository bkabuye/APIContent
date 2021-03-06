﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
namespace GETAPIContent
{
    class Product
    {
        public HealthImmunizationData _healthData;
        protected static JObject jsonData = null;
        protected static DataTable dataTable = new DataTable();

        protected static Dictionary<int, ArrayList> NYSSchoolData = new Dictionary<int, ArrayList>();
        protected static Dictionary<int, ArrayList> BadData = new Dictionary<int, ArrayList>();

        public Product(JObject data)
        {
            jsonData = data;
        }

        public void ParseData()
        {
            //initial dataTable and HealthImmunization struct class to start parsing and storing data
            _healthData = new HealthImmunizationData();
            _healthData.Items = jsonData["data"].ToArray();
            int count = 0;
            HashSet<string> list;
            ArrayList elements; 
            try
            {
                foreach (var item in _healthData.Items)
                {
                    // increment count for dictionary to keep track

                    count++;
                   list = new HashSet<string>();
                   elements = new ArrayList();
                    foreach (var data in item.ToString().Split(','))
                    {
                        if (data.ToString().TrimStart().TrimEnd().Contains("\""))
                        {
                            elements.Add(data.ToString().TrimStart().TrimEnd().Replace("\"", ""));
                        }
                    
                   else if (data.Contains(""))
                        {
                            elements.Add("UNKNOWN");
                        }
                        else
                        {
                            Console.WriteLine("Bad data: " + data);
                        }
                    }
                   if(elements.Count == 36)
                    NYSSchoolData.Add(count, elements);
                   else
                   {

                       BadData.Add(count, elements);
                   }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            var datatable = addData(NYSSchoolData);
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\WEka\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                //Write(datatable, path+ "/NYSSchoolsImmunizationData.txt");
                generateCSVFile(datatable, path);
            }
            else
            {
                generateCSVFile(datatable, path);
                //Write(datatable, path + "/NYSSchoolsImmunizationData.txt");
            }
           
        }

        public DataTable addData(Dictionary<int, ArrayList> Data)
        {
            try
            {
                foreach (var data in Data)
                {
                    if (dataTable.Rows.Count == 0)
                    {
                        #region

                        dataTable.Columns.Add("School ID", typeof(long));
                        dataTable.Columns.Add("District Name", typeof(string));
                        dataTable.Columns.Add("Report Period", typeof(string));
                        dataTable.Columns.Add("Type", typeof(string));
                        dataTable.Columns.Add("School Name", typeof(string));
                        dataTable.Columns.Add("Percent Medical Exemptions", typeof(double));
                        dataTable.Columns.Add("Percent Religious Exemptions", typeof(double));
                        dataTable.Columns.Add("Percent Immunized Polio", typeof(double));
                        dataTable.Columns.Add("Percent Immunized Measles", typeof(double));
                        dataTable.Columns.Add("Percent Immunized Mumps", typeof(double));
                        dataTable.Columns.Add("Percent Immunized Rubella", typeof(double));
                        dataTable.Columns.Add("Percent Immunized Diphtheria", typeof(double));
                        dataTable.Columns.Add("Percent Immunized HepatitisB", typeof(double));
                        dataTable.Columns.Add("Percent Immunized Varicella", typeof(double));
                        dataTable.Columns.Add("Percent Completely Immunized", typeof(double));
                        dataTable.Columns.Add("Street", typeof(string));
                        dataTable.Columns.Add("City", typeof(string));
                        dataTable.Columns.Add("County", typeof(string));
                        dataTable.Columns.Add("State", typeof(string));
                        dataTable.Columns.Add("ZipCode", typeof(int));
                        dataTable.Columns.Add("Location", typeof(string));

                        #endregion
                    }

                    var row = dataTable.NewRow();

                    #region
                    int x = 0;
                    row["School ID"] = Convert.ToInt64(data.Value[8].ToString());
                    row["District Name"] = data.Value[9].ToString().ToUpper();
                    row["Report Period"] = data.Value[10];
                    row["Type"] = data.Value[11].ToString().ToUpper();
                    row["School Name"] = data.Value[12].ToString().ToUpper();
                    row["Percent Medical Exemptions"] = Convert.ToDouble(data.Value[13].ToString());
                    row["Percent Religious Exemptions"] = Convert.ToDouble(data.Value[14].ToString());
                    row["Percent Immunized Polio"] = Convert.ToDouble(data.Value[15].ToString());
                    row["Percent Immunized Measles"] = Convert.ToDouble(data.Value[16].ToString());
                    row["Percent Immunized Mumps"] = Convert.ToDouble(data.Value[17].ToString());
                    row["Percent Immunized Rubella"] = Convert.ToDouble(data.Value[18].ToString());
                    row["Percent Immunized Diphtheria"] = Convert.ToDouble(data.Value[19].ToString());
                    row["Percent Immunized HepatitisB"] = Convert.ToDouble(data.Value[20].ToString());
                    row["Percent Immunized Varicella"] = Convert.ToDouble(data.Value[21].ToString());
                    row["Percent Completely Immunized"] = Convert.ToDouble(data.Value[22].ToString());
                    row["Street"] = data.Value[23];
                    row["City"] = data.Value[24];
                    row["County"] = data.Value[25];
                    row["State"] = data.Value[26];
                    if (!data.Value[27].ToString().Equals("UNKNOWN"))
                        row["ZipCode"] = int.Parse(data.Value[27].ToString());
                    
                    else
                        row["ZipCode"] = int.TryParse(data.Value[27].ToString(), out x);
                    
                    row["Location"] = data.Value[32] + " " + data.Value[33];
                   
                    #endregion

                    try
                    {

                        dataTable.Rows.Add(row);

                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dataTable;
        }



        public static void generateCSVFile(DataTable dt, string outputFilePath)
        {
            StringBuilder sb = new StringBuilder();

            string[] columnNames = dt.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName).
                                              ToArray();
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                string[] fields = row.ItemArray.Select(field => field.ToString()).
                                                ToArray();
                sb.AppendLine(string.Join(",", fields));
            }

            File.WriteAllText(outputFilePath +"/NYSSchoolsImmunizationData.csv", sb.ToString());

        }
       //output in txt file
        public static void Write(DataTable dt, string outputFilePath)
        {
            int[] maxLengths = new int[dt.Columns.Count];

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                maxLengths[i] = dt.Columns[i].ColumnName.Length;

                foreach (DataRow row in dt.Rows)
                {
                    if (!row.IsNull(i))
                    {
                        int length = row[i].ToString().Length;

                        if (length > maxLengths[i])
                        {
                            maxLengths[i] = length;
                        }
                    }
                }
            }

            using (StreamWriter sw = new StreamWriter(outputFilePath, false))
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sw.Write(dt.Columns[i].ColumnName.PadRight(maxLengths[i] + 2));
                }

                sw.WriteLine();

                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (!row.IsNull(i))
                        {
                            sw.Write(row[i].ToString().PadRight(maxLengths[i] + 2));
                        }
                        else
                        {
                            sw.Write(new string(' ', maxLengths[i] + 2));
                        }
                    }

                    sw.WriteLine();
                }

                sw.Close();
            }
        }

        public Dictionary<int, ArrayList> getBadData()
        {
            //Console.WriteLine(BadData.Count);
            return BadData;
        }



    }

    struct HealthImmunizationData
    {
        #region

        public Array Items { get; set; }
        public long SchoolID { get; set; }
        public string DistrictName { get; set; }
        public string ReportPeriod { get; set; }
        public string Type { get; set; }
        public string SchoolName { get; set; }
        public double PercentMedicalExemptions { get; set; }
        public double PercentReligiousExemptions { get; set; }
        public double PercentImmunizedPolio { get; set; }
        public double PercentImmunizedMeasles { get; set; }
        public double PercentImmunizedMumps { get; set; }
        public double PercentImmunizedRubella { get; set; }
        public double PercentImmunizedDiphtheria { get; set; }
        public double PercentImmunizedHepatitisB { get; set; }
        public double PercentImmunizedVaricella { get; set; }
        public double PercentCompletelyImmunized { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public long ZipCode { get; set; }
        public string Location { get; set; }

        #endregion
    }
}
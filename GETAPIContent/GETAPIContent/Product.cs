using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
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
        protected static List<string> list;
        protected static Dictionary<int, List<string>> NYSSchoolData = new Dictionary<int, List<string>>(); 
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
            list =  new List<string>();
            try
            {
              foreach(var item in _healthData.Items){
                  // increment count for dictionary to keep track
                  count++;

                  foreach (var data in item.ToString().Split(','))
                  {
                      var element = new List<string>();    
                      if (data.ToString().TrimStart().TrimEnd().Contains("\""))
                      {
                          element.Add(data.ToString().TrimStart().TrimEnd().Replace("\"", ""));
                      }
                      else if (data.Contains(""))
                      {
                          element.Add("UNKNOWN");
                      }
                      list.AddRange(element);
                  }
                  NYSSchoolData.Add(count, list);
                  list.Clear();
              }              
            }
            catch(Exception ex){
               Console.WriteLine(ex.Message);
            }
            addData(NYSSchoolData);
            Console.WriteLine("Return table"+ dataTable);
        }

       public DataTable addData(Dictionary<int,List<string>> Data) {
           try
           {
               foreach(var data in Data)
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
                       dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["School ID"] };
                       #endregion
                   }

                   var row = dataTable.NewRow();
                   #region
                   row["School ID"] = Convert.ToInt64(data.Value[8].ToString());
                   row["District Name"]= data.Value[9];
                   row["Report Period"]= data.Value[10];
                   row["Type"]= data.Value[11];
                   row["School Name"]= data.Value[12];
                   row["Percent Medical Exemptions"]= Convert.ToDouble(data.Value[13].ToString());
                       row["Percent Religious Exemptions"]= Convert.ToDouble(data.Value[14].ToString());
                       row["Percent Immunized Polio"]= Convert.ToDouble(data.Value[15].ToString());
                       row["Percent Immunized Measles"]= Convert.ToDouble(data.Value[16].ToString());
                       row["Percent Immunized Mumps"]= Convert.ToDouble(data.Value[17].ToString());
                       row["Percent Immunized Rubella"]= Convert.ToDouble(data.Value[18].ToString());
                       row["Percent Immunized Diphtheria"]= Convert.ToDouble(data.Value[19].ToString());
                       row["Percent Immunized HepatitisB"]= Convert.ToDouble(data.Value[20].ToString());
                       row["Percent Immunized Varicella"]= Convert.ToDouble(data.Value[21].ToString());
                       row["Percent Completely Immunized"]= Convert.ToDouble(data.Value[22].ToString());
                       row["Street"]= data.Value[23];
                       row["City"]= data.Value[24];
                       row["County"]= data.Value[25];
                       row["State"]= data.Value[26];
                       row["ZipCode"]= Convert.ToInt32(data.Value[27].ToString());
                       row["Location"]= data.Value[32];
                   #endregion
                       try
                       {
                           dataTable.Rows.Add(row);
                       }catch{

                      }
               }
           }
          catch(Exception e){
              Console.WriteLine(e.Message);
          }
           return dataTable;
       } 
    }
    struct HealthImmunizationData
    {
        #region
        public Array Items { get; set; }
        public long SchoolID {get; set;}
	    public string DistrictName{get; set;}	
        public string ReportPeriod{get; set;}
	    public string Type{get; set;}	
        public string SchoolName{get; set;}	
        public double PercentMedicalExemptions{get; set;}	
        public double PercentReligiousExemptions {get; set;}	
        public double PercentImmunizedPolio {get; set;}	
        public double PercentImmunizedMeasles	{get; set;}
        public double PercentImmunizedMumps	 {get; set;}
        public double PercentImmunizedRubella	{get; set;}
        public double PercentImmunizedDiphtheria	{get; set;}
        public double PercentImmunizedHepatitisB	{get; set;}
        public double PercentImmunizedVaricella	{get; set;}
        public double PercentCompletelyImmunized	{get; set;}
        public string Street{get; set;}	
        public string City	{get; set;}
        public string County	{get; set;}
        public string State	{get; set;}
        public long ZipCode	{get; set;}
        public string Location{get; set; }
        #endregion

    }
}

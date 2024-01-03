using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Configuration;

namespace CsvConvertor
{
    internal class SqlToCsvConvertor
    {

        public string _connectionString { get; set; }
        public string _tableName { get; set; }
        public string[] _columns { get; set; }
        public string _dateColumn { get; set; }
        public string _startDate { get; set; }
        public string _endDate { get; set; }
        public string _outputPath { get; set; }

        public SqlToCsvConvertor()
        {
            LoadConfig();
        }

        private void LoadConfig()
        {
         

            var doc = new XmlDocument();
         
            string configFilePath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

            doc.Load(configFilePath);
            
            _connectionString = doc.SelectSingleNode("//configuration/appSettings/add[@key='ConnectionString']").Attributes["value"].Value; 

            _tableName = doc.SelectSingleNode("//configuration/appSettings/add[@key='TableName']").Attributes["value"].Value; 
            
            _columns = doc.SelectSingleNode("//configuration/appSettings/add[@key='Columns']").Attributes["value"].Value.Split(','); 

            _dateColumn = doc.SelectSingleNode("//configuration/appSettings/add[@key='Orderdate']").Attributes["value"].Value; 

            _startDate = doc.SelectSingleNode("//configuration/appSettings/add[@key='StartDate']").Attributes["value"].Value; 
            
            _endDate = doc.SelectSingleNode("//configuration/appSettings/add[@key='EndDate']").Attributes["value"].Value;

            _outputPath = doc.SelectSingleNode("//configuration/appSettings/add[@key='OutputPath']").Attributes["value"].Value;
        }




    }
}

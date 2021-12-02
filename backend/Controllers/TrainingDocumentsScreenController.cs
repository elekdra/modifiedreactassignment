using System.Collections.Generic;
using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;
using MySql.Data.MySqlClient;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]


    public class TrainingDocumentsScreenController : ControllerBase
    {
        IWebHostEnvironment environment;
        public TrainingDocumentsScreenController(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        public MySqlConnection unv()
        {
            string cs = @"server=localhost;userid=root;password=fathimaadmin;database=DOCUMENT_MANAGEMENT";
            using var con = new MySqlConnection(cs);
            con.Open();
            var cmd = new MySqlCommand();
            cmd.Connection = con;
            return con;
        }
      
        [HttpGet]
        [Route("company")]
        public string GetCompany()
        {
            string cs = @"server=localhost;userid=root;password=fathimaadmin;database=DOCUMENT_MANAGEMENT";
            using var con = new MySqlConnection(cs);
            con.Open();
            var CommandText = "SELECT * FROM DOCUMENT_MANAGEMENT.Company";
            using var cmd = new MySqlCommand(CommandText, con);
            using MySqlDataReader rdr = cmd.ExecuteReader();
            var companyNames = "";
            while (rdr.Read())
            {
                companyNames += rdr.GetString(1) + "|";
            }


            return companyNames;
        }
        [HttpGet]
        [Route("training")]
        public string GetTraining()
        {
            string cs = @"server=localhost;userid=root;password=fathimaadmin;database=DOCUMENT_MANAGEMENT";
            using var con = new MySqlConnection(cs);
            con.Open();
            var trainingNames = "";
            var CommandText = "SELECT * FROM DOCUMENT_MANAGEMENT.Training";
            using var cmd = new MySqlCommand(CommandText, con);
            using MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                trainingNames += rdr.GetString(1) + "|";
            }
            return trainingNames;
        }

        [HttpGet]
        [Route("getFilteredData")]
        public IList<FileModel> GetFilteredList(string filterParameters)
        {
          
            var userDetails = filterParameters.Split("|");
            var emptyVersion = "undefined";
            string cs = @"server=localhost;userid=root;password=fathimaadmin;database=DOCUMENT_MANAGEMENT";
            using var con = new MySqlConnection(cs);
            con.Open();
            var Company_ID = "SELECT company.COMPANY_ID FROM DOCUMENT_MANAGEMENT.company WHERE company.COMPANY_NAME=" + @"""" + userDetails[0] + @"""";
            var Training_ID = "SELECT training.Training_ID FROM DOCUMENT_MANAGEMENT.training WHERE training.TRAINING_NAME=" + @"""" + userDetails[2] + @"""";
            using var cmd = new MySqlCommand(Company_ID, con);
            using var cmd2 = new MySqlCommand(Training_ID, con);
            if (userDetails[0] != "ALL")
            {
                using MySqlDataReader rdr1 = cmd.ExecuteReader();

                while (rdr1.Read())
                {
                    userDetails[0] = rdr1.GetInt32(0).ToString();
                }
            }
            con.Close();
            con.Open();
            if (userDetails[2] != "ALL")
            {
                using MySqlDataReader rdr2 = cmd2.ExecuteReader();
                while (rdr2.Read())
                {
                    userDetails[2] = rdr2.GetInt32(0).ToString();
                }
            }
            con.Close();
            var CommandText = @"SELECT * FROM DOCUMENT_MANAGEMENT.trainingdetails_header INNER JOIN DOCUMENT_MANAGEMENT.trainingdetails_data ON trainingdetails_header.Training_index=trainingdetails_data.Training_index";
            if (userDetails[0] == "ALL")
            {
                if (userDetails[1] == emptyVersion || userDetails[1] == "")
                {
                    if (userDetails[2] == "ALL")
                    {
                        CommandText = CommandText;
                    }
                    else
                    {
                        CommandText = CommandText + " WHERE trainingdetails_header.Training_ID=" + userDetails[2];
                    }
                }
                else
                {
                    if (userDetails[1] != emptyVersion || userDetails[1] != "")
                    {
                        CommandText = CommandText + @" WHERE trainingdetails_header.Version=""" + userDetails[1] + @"""";
                    }
                    else
                    {
                        CommandText = CommandText + " WHERE trainingdetails_header.Version=" + userDetails[1] + " AND trainingdetails_header.Training_ID= " + userDetails[2];
                    }

                }
            }
            else
            {
                if (userDetails[1] == emptyVersion || userDetails[1] == "")
                {
                    if (userDetails[2] == "ALL")
                    {
                        CommandText = CommandText + " WHERE trainingdetails_header.Company_ID=" + userDetails[0];
                    }
                    else
                    {
                        CommandText = CommandText + " WHERE trainingdetails_header.Company_ID=" + userDetails[0] + " AND trainingdetails_header.Training_ID= " + userDetails[2];
                    }
                }
                else
                {
                    if (userDetails[2] == "ALL")
                    {
                        CommandText = CommandText + " WHERE trainingdetails_header.Company_ID=" + userDetails[0] + " AND trainingdetails_header.Version= " + userDetails[1];
                    }
                    else
                    {
                        CommandText = CommandText + " WHERE trainingdetails_header.Company_ID=" + userDetails[0] + " AND trainingdetails_header.Version= " + userDetails[1] + " AND trainingdetails_header.Training_ID=" + userDetails[2];
                    }
                }
            }

            IList<FileModel> filteredfileModels = new List<FileModel>();
            con.Open();
            using var cmd3 = new MySqlCommand(CommandText, con);
            using MySqlDataReader rdr = cmd3.ExecuteReader();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    FileModel filteritem = new FileModel();
                    filteritem.FileName = rdr.GetString(8);
                    filteritem.Company = rdr.GetInt32(1).ToString();
                    filteritem.Version = rdr.GetString(2);
                    filteritem.Training = rdr.GetInt32(3).ToString();
                    filteritem.FileContent = rdr.GetString(6);
                    filteritem.MinVersion = rdr.GetString(7);
                    filteritem.Mode = "";
                    filteredfileModels.Add(filteritem);
                }
            }
            IDictionary<string, string> companyMap = GetCompanyIdToNameMap(con);
            IDictionary<string, string> trainingMap = GetTrainingIdToNameMap(con);
            foreach (var model in filteredfileModels)
            {
                model.Company = companyMap[model.Company];
                model.Training = trainingMap[model.Training];
            }
            con.Close();
            return filteredfileModels;
        }

        private IDictionary<string, string> GetCompanyIdToNameMap(MySqlConnection con)
        {
            con.Close();
            con.Open();
            IDictionary<string, string> companyMap = new Dictionary<string, string>();
            using (MySqlCommand readAllCompaniesCommands = con.CreateCommand())
            {
                readAllCompaniesCommands.CommandText = "select * from DOCUMENT_MANAGEMENT.company";
                using (var reader1 = readAllCompaniesCommands.ExecuteReader())
                {
                    while (reader1.Read())
                    {
                        companyMap.Add(reader1.GetInt32(0).ToString(), reader1.GetString(1).ToString());
                    }
                }
            }
            return companyMap;
        }

        private IDictionary<string, string> GetTrainingIdToNameMap(MySqlConnection connection)
        {
            IDictionary<string, string> trainingMap = new Dictionary<string, string>();
            using (MySqlCommand readAllTrainingCommand = connection.CreateCommand())
            {
                readAllTrainingCommand.CommandText = "select * from DOCUMENT_MANAGEMENT.training";
                using (var reader2 = readAllTrainingCommand.ExecuteReader())
                {
                    while (reader2.Read())
                    {
                        trainingMap.Add(reader2.GetInt32(0).ToString(), reader2.GetString(1).ToString());
                    }
                }
            }
            return trainingMap;
        }

        private IDictionary<string, string> GetCompanyNameToIdMap(MySqlConnection connection)
        {
            IDictionary<string, string> companyMap = new Dictionary<string, string>();
            using (MySqlCommand readAllCompaniesCommand = connection.CreateCommand())
            {
                readAllCompaniesCommand.CommandText = "select * from DOCUMENT_MANAGEMENT.company";
                using (var reader = readAllCompaniesCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string id = reader.GetInt32(0).ToString();
                        string name = reader.GetString(1).ToString();
                        companyMap.Add(name, id);
                    }
                }
            }
            return companyMap;
        }

        private IDictionary<string, string> GetTrainingNameToIdMap(MySqlConnection connection)
        {
            IDictionary<string, string> trainingMap = new Dictionary<string, string>();
            using (MySqlCommand readAllTrainingCommand = connection.CreateCommand())
            {
                readAllTrainingCommand.CommandText = "select * from DOCUMENT_MANAGEMENT.training";
                using (var reader = readAllTrainingCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string id = reader.GetInt32(0).ToString();
                        string name = reader.GetString(1).ToString();
                        trainingMap.Add(name, id);
                    }
                }
            }
            return trainingMap;
        }
    }
}
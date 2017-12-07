using MySql.Data.MySqlClient;
using Selenium_g_y_proj;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
namespace DBComparator
{
    class DBConection
    {
        public enum Con
        {
            CON1 = 0,
            CON2 = 1
        }

        public class DBKeyword
        {
            public string keyword { get; set; }
            public int keyword_id { get; set; }
        }
        private MySqlConnection conn1;
        private MySqlConnection conn2;

        public DBConection()
        {
            this.Initialize();
        }

        public void Initialize()
        {
            string connStr = ConfigurationManager
                .ConnectionStrings["keywordsConnStr"]
                .ConnectionString;

            conn1 = new MySqlConnection(connStr);

            connStr = ConfigurationManager
                .ConnectionStrings["keywordsConnStr2"]
                .ConnectionString;

            conn2 = new MySqlConnection(connStr);

        }


        public Boolean isExist(DBKeyword keyword,Con con)
        {      
            
            string query = con == 0? "SELECT Count(*) FROM `keywords` WHERE `keyword`='"
                + keyword.keyword + "';": "SELECT Count(*) FROM `keywords` WHERE `name`='"
                + keyword.keyword + "';";

            int Count = -1;

            //Open Connection
            if (this.OpenConnection(con) == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, con == 0 ? conn1 : conn2);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                this.CloseConnection(con);
                if (Count > 0)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }

        }
        ////Insert statement
        //public void Insert(Keyword keyword)
        //{

        //    string replacement = "";
        //    Regex rgx = new Regex("['\"]");

        //    Console.WriteLine("Добавляем в бд " + keyword.toString());
        //    string query = "INSERT INTO `adpostions`(`url`, `description`, `positions`, `browser`, `Keywords_id`) VALUES (\""
        //        + rgx.Replace(keyword.url, replacement) + "\",\""
        //        + rgx.Replace(keyword.description, replacement) + "\",\""
        //        + keyword.getConcatPositions() + "\","
        //        + keyword.browser + ","
        //        + keyword.keyword_id + ")";

        //    //open connection
        //    if (this.OpenConnection() == true)
        //    {
        //        //create command and assign the query and connection from the constructor
        //        MySqlCommand cmd = new MySqlCommand(query, conn);

        //        //Execute command
        //        cmd.ExecuteNonQuery();

        //        //close connection
        //        this.CloseConnection();
        //    }

        //}



        ////Update statement
        //public void Update(Keyword keyword)
        //{

        //    Console.WriteLine("Обновляем в бд " + keyword.toString());

        //    string replacement = "";
        //    Regex rgx = new Regex("['\"]");

        //    string query = "UPDATE `adpostions` SET `url`=\"" + rgx.Replace(keyword.url, replacement)
        //        + "\",`description`=\"" + rgx.Replace(keyword.description, replacement)
        //        + "\",`positions`='" + keyword.getConcatPositions()
        //        + "',`browser`=\"" + keyword.browser
        //        //+ ",`Keywords_id`="+keyword.keyword_id  
        //        + "\" WHERE `Keywords_id`=" + keyword.keyword_id + " and `description`= \"" + rgx.Replace(keyword.description, replacement) + "\"; ";



        //    //Open connection
        //    if (this.OpenConnection() == true)
        //    {
        //        //create mysql command
        //        MySqlCommand cmd = new MySqlCommand();
        //        //Assign the query using CommandText
        //        cmd.CommandText = query;
        //        //Assign the connection using Connection
        //        cmd.Connection = conn;

        //        //Execute query
        //        cmd.ExecuteNonQuery();

        //        //close connection
        //        this.CloseConnection();
        //    }
        //}

        public List<DBKeyword> list(Con con)
        {
            //выбирае из бд инфу с определенным смещением, чтоб не нагружать оперативку
            string query = con==0? "SELECT `keyword` FROM `keywords`;": "SELECT `name` FROM `keywords`;";
            List<DBKeyword> list_kw = new List<DBKeyword>();

            //Open connection
            if (this.OpenConnection(con) == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, con == 0 ? conn1 : conn2);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    DBKeyword dbkw = new DBKeyword();
                    dbkw.keyword = con == 0 ? "" + dataReader["keyword"]: "" + dataReader["name"];
                    dbkw.keyword_id = 0;

                    list_kw.Add(dbkw);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection(con);

                //return list to be displayed
                return list_kw;
            }
            else
            {
                return list_kw;
            }
        }

        public int count(Con con)
        {
            string query = "SELECT Count(*) FROM `keywords`";
            int Count = -1;

            //Open Connection
            if (this.OpenConnection(con) == true)
            {
                //Create Mysql Command

                MySqlCommand cmd = new MySqlCommand(query, con==0?conn1:conn2);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                this.CloseConnection(con);

                return Count;
            }
            else
            {
                return Count;
            }

        }

        private bool CloseConnection(Con con)
        {
            try
            {
                if (con == 0)
                    conn1.Close();
                else
                    conn2.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //open connection to database
        private bool OpenConnection(Con con)
        {
            try
            {
                if (con == 0)
                    conn1.Open();
                else
                    conn2.Open();
                return true;
            }
            catch (MySqlException ex)
            {

                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }
    }
}

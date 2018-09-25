using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;
using System.Security.Cryptography;
using System.Data.SQLite;
using System.Configuration;

namespace SeniorCapstoneOfficial
{
    public class DBConnector
    {
        SQLiteConnection myConnection = new SQLiteConnection(LoadConnectionString());

        public void createUsers(string fName, string lName, string username, string password, bool admin)
        {

            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string savedPasswordHash = Convert.ToBase64String(hashBytes);

            using (myConnection)
            {
                string sql = null;
                sql = "insert into User ([uname], [password], [admin], [fName], [lName]) values(@uname,@password,@admin,@fName,@lName)";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, myConnection))
                {
                    myConnection.Open();
                    cmd.Parameters.AddWithValue("@uname", username);
                    cmd.Parameters.AddWithValue("@password", savedPasswordHash);
                    cmd.Parameters.AddWithValue("@admin", admin);
                    cmd.Parameters.AddWithValue("@fName", fName);
                    cmd.Parameters.AddWithValue("@lName", lName);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<User> userSearch(string lName)
        {
            string fName = null;
            string lName1 = null;
            string username = null;
            List<User> userList = new List<User>();

            using (SQLiteConnection conn = new SQLiteConnection(myConnection))
            {
                conn.Open();
                string sql = "SELECT uname, fName, lName FROM User WHERE lName = @lName";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@lName", lName);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            fName = reader["fName"].ToString();
                            lName1 = reader["lName"].ToString();
                            username = reader["uname"].ToString();
                            User user = new User(fName, lName, username);
                            userList.Add(user);
                        }
                    }
                }
                conn.Close();
            }
            return userList;
        }

        public void DeleteUser(string uname)
        {

        }


        public User getUser(string uname)
        {
            string fName = null;
            string lName = null;
            string username = null;
            string password = null;
            int admin = 0;

            using (SQLiteConnection conn = new SQLiteConnection(myConnection))
            {
                conn.Open();
                string sql = "SELECT uname, password, admin FROM User WHERE uname=@username";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@username", uname);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            username = reader["uname"].ToString();
                             password = reader["password"].ToString();
                            admin = Convert.ToInt32(reader["admin"]);
                        }

                    }
                }
                conn.Close();
            }

            User user = new User(fName, lName, username, password, admin);
            return user;
        }

        public void saveLogin(string uname, DateTime time)
        {
            int id = 0;

            using (SQLiteConnection conn = new SQLiteConnection(myConnection))
            {
                conn.Open();
                string sql = "SELECT id FROM Session";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            id = id + 1;
                        }

                    }
                }
                conn.Close();
            }

            using (myConnection)
            {
                string sql = null;
                sql = "insert into Session ([uname], [id], [time], [type]) values(@uname,@id,@time,@type)";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, myConnection))
                {
                    myConnection.Open();
                    cmd.Parameters.AddWithValue("@uname", uname);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@time", time);
                    cmd.Parameters.AddWithValue("@type", "Login");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void saveLogout(string uname, DateTime time)
        {
            int id = 0;

            using(SQLiteConnection conn = new SQLiteConnection(myConnection))
            {
                conn.Open();
                string sql = "SELECT id FROM Session";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            id = id + 1;
                        }

                    }
                }
                conn.Close();
            }


            using (myConnection)
            {
                string sql = null;
                sql = "insert into Session ([uname], [id], [time], [type]) values(@uname,@id,@time,@type)";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, myConnection))
                {
                    myConnection.Open();
                    cmd.Parameters.AddWithValue("@uname", uname);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@time", time);
                    cmd.Parameters.AddWithValue("@type", "Logout");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static string LoadConnectionString(string id = "Default")
         {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        public DBConnector()
        {

            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
           var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "database.sqlite3");
            myConnection = new SQLiteConnection(LoadConnectionString());
           if (!File.Exists(fileName))
            {
                SQLiteConnection.CreateFile(fileName);

            }
           
        }
    
        public void OpenConnection()
        {
            if (myConnection.State != System.Data.ConnectionState.Open)
            {
                myConnection.Open();
            }
        }

        public void CloseConnection()
        {
            if (myConnection.State != System.Data.ConnectionState.Closed)
            {
                myConnection.Close();
            }
        }

        public void createTables()
        {   
            using (myConnection)
            {
                string sql = "create table IF NOT EXISTS User (uname varchar(20) primary key, password varchar(20), admin int)";
                SQLiteCommand command = new SQLiteCommand(sql, myConnection);
                command.ExecuteNonQuery();

                sql = "create table IF NOT EXISTS Session (uname varchar(20), id int primary key, time datetime, type varchar(20))";
                command = new SQLiteCommand(sql, myConnection);
                command.ExecuteNonQuery();
            

            }
        }

    }
}
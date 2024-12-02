using System;
using System.Linq;
using Codice.Client.BaseCommands;
using LinqToDB;
using LinqToDB.Data;
using MySqlConnector;
using Objects;
using ScriptableObjectScripts;
using UnityEngine;

namespace BehaviourScripts
{
    public static class DatabaseManager
    {
        private static string Server = "localhost";
        private static string Database = "objectdata";
        private static string User = "root";
        private static string Pwd = "EMPOWERpwd";
        private static string Port = "3306";

        /// <summary>
        /// Saves the data currently in the StorageSO to the database
        /// </summary>
        /// <param name="storage">Scriptable object containing data that should be saved</param>
        public static void SaveStorageSOToDatabase(StorageSO storage)
        {
            var connection = GetDataConnection();

            var users = connection.GetTable<User>().Select(u => new User(u.Id, u.Nickname, u.Sessions)).ToArray();
            var user = users.FirstOrDefault(u => u.Nickname.Equals(storage.nickname));
            Session session = null;
            if (user != null)
            {
                var sessions = connection.GetTable<Session>().Where(s => s.UserId == user.Id)
                    .Select(s => new Session(s.Id, s.UserId)).ToArray();
                var res = sessions.LastOrDefault();
                session = res;
            }

            if (user == null)
            {
                connection.Insert(storage.User);
            }

            if (session == null || session.Id != storage.User.Sessions[^1].Id)
            {
                connection.Insert(storage.User.Sessions.LastOrDefault());
            }

            foreach (var game in storage.User.Sessions[^1].GamesList)
            {
                connection.Insert(game);
                foreach (var obj in game.Objects)
                {
                    connection.Insert(obj);
                    connection.Insert(obj.Aoi);
                    foreach (var point in obj.Points)
                    {
                        connection.Insert(point);
                    }

                    foreach (var origin in obj.Aoi.Origins)
                    {
                        connection.Insert(origin);
                    }

                    foreach (var size in obj.Aoi.Sizes)
                    {
                        connection.Insert(size);
                    }
                }
            }

            connection.Close();
            connection.Dispose();
        }

        /// <summary>
        /// Sets up the tables for object tracking in the given database
        /// </summary>
        private static void SetUpTables()
        {
            var connection = GetDataConnection();
            connection.CreateTable<User>();
            connection.CreateTable<Session>();
            connection.CreateTable<Game>();
            connection.CreateTable<ObjectInGame>();
            connection.CreateTable<Aoi>();
            connection.CreateTable<Point>();
            connection.CreateTable<AoiOrigin>();
            connection.CreateTable<AoiSize>();
            
            connection.Close();
            connection.Dispose();
            
            SetUpForeignKey("session","sessionuserfk", "UserId","User","Id");
            SetUpForeignKey("game","gamesessionfk", "SessionId","Session","Id");
            SetUpForeignKey("objectingame","objectgamefk", "GameId","Game","Id");
            SetUpForeignKey("aoi","aoiobjectfk", "ObjectName","ObjectInGame","Name");
            SetUpForeignKey("point","pointobjectfk", "ObjectName","ObjectInGame","Name");
            SetUpForeignKey("aoiorigin","originAoifk","AoiId","Aoi","Id");
            SetUpForeignKey("aoisize","sizeAoifk","AoiId","Aoi","Id");
        }

        /// <summary>
        /// Sets up a foreign key constraint for the tables given
        /// </summary>
        /// <param name="table">Table the key should be added to</param>
        /// <param name="fkName">Name of the foreign key</param>
        /// <param name="fk">Attribute in the table that should become the key</param>
        /// <param name="connectedTable">Table the key should refer to</param>
        /// <param name="connectedAttribute">Attribute the key should reference</param>
        private static void SetUpForeignKey(string table, string fkName, string fk, string connectedTable, string connectedAttribute)
        {
            var con = new MySqlConnection(
                $"Server={Server};Database={Database};Uid={User};Pwd={Pwd};Port={Port};Charset=utf8;Allow User Variables=True;");
            con.Open();
            
            var command = new MySqlCommand($"alter table {table} add constraint {fkName} foreign key ({fk}) references {connectedTable} ({connectedAttribute})");
            command.Connection = con;
            command.ExecuteNonQuery();
            con.Close();
            con.Dispose();
        }
        
        /// <summary>
        /// Gets the user by nickname from the database.
        /// Creates tables, adds and retrieves the user if it does not exist.
        /// </summary>
        /// <param name="nickname">User nickname</param>
        /// <param name="storage">Scriptable object containing data for the object tracking</param>
        /// <returns>Returns the user</returns>
        public static User GetUser(string nickname,StorageSO storage)
        {
            var connection = GetDataConnection();
            User[] users;
            try
            {
                users = connection.GetTable<User>().Where(u => u.Nickname.Equals(nickname)).Select(u => new User(u.Id,u.Nickname,u.Sessions)).ToArray();
            }
            catch (MySqlException ex)
            {
                SetUpTables();
                users = connection.GetTable<User>().Where(u => u.Nickname.Equals(nickname)).Select(u => new User(u.Id,u.Nickname,u.Sessions)).ToArray();
            }
            
            if (users.Length == 0)
            {
                users = InsertNewUser(storage);
            }
            
            connection.Close();
            connection.Dispose();
            return users.FirstOrDefault(u => u.Nickname.Equals(nickname));
        }

        /// <summary>
        /// Inserts a user into the database
        /// </summary>
        /// <param name="storage">Scriptable object containing data for the object tracking</param>
        /// <returns></returns>
        private static User[] InsertNewUser(StorageSO storage)
        {
            var connection = GetDataConnection();
            connection.Insert(new User(storage.nickname));
            var users = connection.GetTable<User>().Where(u => u.Nickname.Equals(storage.nickname)).Select(u => new User(u.Id,u.Nickname,u.Sessions)).ToArray();
            return users;
        }

        /// <summary>
        /// Gets the amount of sessions referring to the user
        /// </summary>
        /// <param name="userId">The identifier for the user</param>
        /// <returns>NUmber of sessions</returns>
        public static int GetSessionCount(int userId)
        {
            var connection = GetDataConnection();
            var sessions = connection.GetTable<Session>().Where(s => s.UserId == userId)
                .Select(s => new Session(s.Id, s.UserId)).ToArray();
            return sessions.Length;
        }

        /// <summary>
        /// Sets up a new data connection
        /// </summary>
        /// <returns>Data connection</returns>
        private static DataConnection GetDataConnection()
        {
            return new DataConnection(new DataOptions().UseMySql($"Server={Server};" +
                                                                 $"Database={Database};Uid={User};Pwd={Pwd};" +
                                                                 $"Port={Port};Charset=utf8;" +
                                                                 $"Allow User Variables=True;"));
        }
    }
}
using System;
using System.Linq;
using Codice.Client.BaseCommands;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Linq;
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

            var users = connection.GetTable<User>().Select(u => new User(u.Id, u.Nickname, u.Games)).ToArray();
            var user = users.FirstOrDefault(u => u.Nickname.Equals(storage.nickname));

            if (user == null)
            {
                connection.Insert(storage.User);
            }

            foreach (var game in storage.User.Games)
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
            connection.CreateTable<Game>();
            connection.CreateTable<ObjectInGame>();
            connection.CreateTable<Aoi>();
            connection.CreateTable<Point>();
            connection.CreateTable<AoiOrigin>();
            connection.CreateTable<AoiSize>();
            
            connection.Close();
            connection.Dispose();
            
            SetUpForeignKey("game","gameuserfk", "UserId","User","Id");
            SetUpForeignKey("objectingame","objectgamefk", "GameId","Game","Id");
            SetUpForeignKey("aoi","aoiobjectfk", "ObjectId","ObjectInGame","Id");
            SetUpForeignKey("point","pointobjectfk", "ObjectId","ObjectInGame","Id");
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
            User user;
            try
            {
                user = connection.GetTable<User>().Where(u => u.Nickname.Equals(nickname)).Select(u => new User(u.Id,u.Nickname,u.Games)).ToArray().FirstOrDefault(u => u.Nickname.Equals(nickname));
            }
            catch (MySqlException ex)
            {
                SetUpTables();
                user = connection.GetTable<User>().Where(u => u.Nickname.Equals(nickname)).Select(u => new User(u.Id,u.Nickname,u.Games)).ToArray().FirstOrDefault(u => u.Nickname.Equals(nickname));
            }
            
            if (user == null && storage.nickname != null)
            {
                user = InsertNewUser(storage);
            }
            
            connection.Close();
            connection.Dispose();
            return user;
        }

        /// <summary>
        /// Inserts a user into the database and returns it. It is added and returned to give it an id and ensure coherence
        /// </summary>
        /// <param name="storage">Scriptable object containing data for the object tracking</param>
        /// <returns>Returns the user after adding it to the database</returns>
        private static User InsertNewUser(StorageSO storage)
        {
            var connection = GetDataConnection();
            connection.Insert(new User(storage.nickname));
            var users = connection.GetTable<User>().Where(u => u.Nickname.Equals(storage.nickname)).Select(u => new User(u.Id,u.Nickname,u.Games)).ToArray();
            return users.FirstOrDefault(u => u.Nickname.Equals(storage.nickname));
        }

        public static int AmountOfTimesPlayingTheSameGame(int userId, string gameName)
        {
            var connection = GetDataConnection();
            try
            {
                return connection.GetTable<Game>().Count(g => g.UserId == userId && g.Name.Equals(gameName));
            }
            catch (Exception ex)
            {
                return -1;            
            }
        }
        
        public static int GetAvailableGameID()
        {
            var connection = GetDataConnection();
            return connection.GetTable<Game>().Count()+1;
        }

        public static int GetAvailableObjectID()
        {
            var connection = GetDataConnection();
            return connection.GetTable<ObjectInGame>().Count()+1;
        }

        public static int GetAvailableAoiID()
        {
            var connection = GetDataConnection();
            return connection.GetTable<Aoi>().Count()+1;
        }

        /// <summary>
        /// Gets the amount of sessions referring to the user
        /// </summary>
        /// <param name="userId">The identifier for the user</param>
        /// <returns>Number of sessions</returns>
        /*public static int GetSessionCount(int userId)
        {
            var connection = GetDataConnection();
            var sessions = connection.GetTable<Session>().Where(s => s.UserId == userId)
                .Select(s => new Session(s.Id,s.SessionNumber, s.UserId)).ToArray();
            return sessions.Length;
        }*/

        /// <summary>
        /// Finds the current highest session id and returns it. Used for giving ids to sessions.
        /// </summary>
        /// <returns>Number of existing sessions</returns>
        /*public static int GetCurrentHighestSessionID()
        {
            var connection = GetDataConnection();
            var sessions = connection.GetTable<Session>().Select(s => new Session(s.Id, s.SessionNumber, s.UserId))
                .ToArray();
            return sessions.Length;
        }*/

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
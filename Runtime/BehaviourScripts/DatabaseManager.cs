﻿using System.Linq;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Tools;
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
                }
            }
            
            connection.Close();
            connection.Dispose();

        }

        /// <summary>
        /// Creates and sets up a separate area of interest table and stores the given area of interest
        /// </summary>
        /// <param name="aoi">Area of interest to save in the database</param>
        public static void SaveAoiToDatabase(AreaOfInterest aoi)
        {
            var connection = GetDataConnection();
            try
            {
                connection.Insert(aoi);
                foreach (var origin in aoi.Origins)
                {
                    connection.Insert(origin);
                }
            }
            catch (MySqlException ex)
            {
                connection.CreateTable<AreaOfInterest>();
                connection.CreateTable<AoiOrigin>();
                
                var con = new MySqlConnection(
                    $"Server={Server};Database={Database};Uid={User};Pwd={Pwd};Port={Port};Charset=utf8;Allow User Variables=True;");
                con.Open();
                var command = new MySqlCommand("alter table aoiorigin add constraint originAreaOfInterestfk foreign key (AreaOfInterestId) references AreaOfInterest (Id)");
                command.Connection = con;
                command.ExecuteNonQuery();
                
                connection.Insert(aoi);
                foreach (var origo in aoi.Origins)
                {
                    connection.Insert(origo);
                }
            }
            connection.Close();
            connection.Dispose();
        }

        private static void SetUpTables()
        {
            var connection = GetDataConnection();
            connection.CreateTable<User>();
            connection.CreateTable<Session>();
            connection.CreateTable<Game>();
            connection.CreateTable<ObjectInGame>();
            connection.CreateTable<Aoi>();
            connection.CreateTable<Point>();
            
            connection.Close();
            connection.Dispose();
            
            SetUpForeignKey("session","sessionuserfk", "UserId","User","Id");
            SetUpForeignKey("game","gamesessionfk", "SessionId","Session","Id");
            SetUpForeignKey("objectingame","objectgamefk", "GameId","Game","Id");
            SetUpForeignKey("aoi","aoiobjectfk", "ObjectName","ObjectInGame","Name");
            SetUpForeignKey("point","pointobjectfk", "ObjectName","ObjectInGame","Name");
        }

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

        private static User[] InsertNewUser(StorageSO storage)
        {
            var connection = GetDataConnection();
            connection.Insert(new User(storage.nickname));
            var users = connection.GetTable<User>().Where(u => u.Nickname.Equals(storage.nickname)).Select(u => new User(u.Id,u.Nickname,u.Sessions)).ToArray();
            return users;
        }

        public static int GetSessionCount(int userId)
        {
            var connection = GetDataConnection();
            var sessions = connection.GetTable<Session>().Where(s => s.UserId == userId)
                .Select(s => new Session(s.Id, s.UserId)).ToArray();
            return sessions.Length;
        }

        private static DataConnection GetDataConnection()
        {
            return new DataConnection(new DataOptions().UseMySql($"Server={Server};" +
                                                                 $"Database={Database};Uid={User};Pwd={Pwd};" +
                                                                 $"Port={Port};Charset=utf8;" +
                                                                 $"Allow User Variables=True;"));
        }
    }
}
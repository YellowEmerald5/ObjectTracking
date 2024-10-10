using System.Linq;
using LinqToDB;
using LinqToDB.Data;
using MySqlConnector;
using Objects;
using ScriptableObjectScripts;

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
        public static void SaveToDatabase(StorageSO storage)
        {
            var connection = new DataConnection(new DataOptions().UseMySql($"Server={Server};Database={Database};Uid={User};Pwd={Pwd};Port={Port};Charset=utf8;Allow User Variables=True;"));
            
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

        public static User GetUser(string nickname,StorageSO storage)
        {
            var connection = new DataConnection(new DataOptions().UseMySql($"Server={Server};Database={Database};Uid={User};Pwd={Pwd};Port={Port};Charset=utf8;Allow User Variables=True;"));

            User[] users;
            try
            {
                users = connection.GetTable<User>().Where(u => u.Nickname.Equals(nickname)).Select(u => new User(u.Id,u.Nickname,u.Sessions)).ToArray();
            }
            catch (MySqlException ex)
            {
                connection.CreateTable<User>();
                connection.CreateTable<Session>();
                connection.CreateTable<Game>();
                connection.CreateTable<ObjectInGame>();
                connection.CreateTable<Aoi>();
                connection.CreateTable<Point>();
                
                var con = new MySqlConnection(
                    $"Server={Server};Database={Database};Uid={User};Pwd={Pwd};Port={Port};Charset=utf8;Allow User Variables=True;");
                con.Open();
                
                var command = new MySqlCommand("alter table session add constraint sessionuserfk foreign key (UserId) references User (Id)");
                command.Connection = con;
                command.ExecuteNonQuery();
                
                command = new MySqlCommand("alter table game add constraint gamesessionfk foreign key (SessionId) references Session (Id)");
                command.Connection = con;
                command.ExecuteNonQuery();
                
                command = new MySqlCommand("alter table objectingame add constraint objectgamefk foreign key (GameId) references Game (Id)");
                command.Connection = con;
                command.ExecuteNonQuery();
                
                command = new MySqlCommand("alter table aoi add constraint aoiobjectfk foreign key (ObjectName) references ObjectInGame (Name)");
                command.Connection = con;
                command.ExecuteNonQuery();
                
                command = new MySqlCommand("alter table point add constraint pointobjectfk foreign key (ObjectName) references ObjectInGame (Name)");
                command.Connection = con;
                command.ExecuteNonQuery();
                
                con.Close();
                con.Dispose();
                
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
            var connection = new DataConnection(new DataOptions().UseMySql($"Server={Server};Database={Database};Uid={User};Pwd={Pwd};Port={Port};Charset=utf8;Allow User Variables=True;"));
            connection.Insert(new User(storage.nickname));
            var users = connection.GetTable<User>().Where(u => u.Nickname.Equals(storage.nickname)).Select(u => new User(u.Id,u.Nickname,u.Sessions)).ToArray();
            return users;
        }

        public static int GetSessionCount(int userId)
        {
            var connection = new DataConnection(new DataOptions().UseMySql($"Server={Server};Database={Database};Uid={User};Pwd={Pwd};Port={Port};Charset=utf8;Allow User Variables=True;"));
            var sessions = connection.GetTable<Session>().Where(s => s.UserId == userId)
                .Select(s => new Session(s.Id, s.UserId)).ToArray();
            return sessions.Length;
        }
    }
}
using System.Collections.Generic;
using LinqToDB.Mapping;

namespace Objects
{
    public class User
    {
        [NotNull]
        public string Nickname { get; set; }
        [PrimaryKey,Identity]
        public int Id { get; set; }
        [Nullable]
        public List<Session> Sessions { get; set; }

        public User()
        {
            Sessions = new List<Session>();
        }
        public User(string nickname)
        {
            Nickname = nickname;
            Sessions = new List<Session>();
        }

        public User(int id, string nickname, List<Session> sessions)
        {
            Id = id;
            Nickname = nickname;
            Sessions = sessions;
        }

    }
}
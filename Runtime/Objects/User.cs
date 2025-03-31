using System.Collections.Generic;
using LinqToDB.Mapping;

namespace Objects
{
    public class User
    {
        [NotNull,Column(Length = 50)]
        public string Nickname { get; set; }
        [PrimaryKey,Identity]
        public int Id { get; set; }
        [Nullable]
        public List<Game> Games { get; set; }

        public User()
        {
            Games = new List<Game>();
        }
        public User(string nickname)
        {
            Nickname = nickname;
            Games = new List<Game>();
        }

        public User(int id, string nickname, List<Game> games)
        {
            Id = id;
            Nickname = nickname;
            Games = games;
        }

    }
}
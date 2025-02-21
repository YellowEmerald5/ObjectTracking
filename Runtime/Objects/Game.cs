using System.Collections.Generic;
using LinqToDB.Mapping;

namespace Objects
{
    public class Game
    {
        [PrimaryKey]
        public int Id { get; set; }
        [NotNull]
        public int TimesPlayed { get; }
        [NotNull]
        public int UserId { get; set; }
        [NotNull]
        public int SessionId { get; set; }
        [NotNull,Column(Length = 100)]
        public string Name { get; }
        
        [Nullable]
        public List<ObjectInGame> Objects { get; }
        [Nullable]
        public int AmountOfGamesPlayed { get; set; }
        [Nullable]
        public float WindowHeight { get; set; }
        [Nullable]
        public float WindowWidth { get; set; }
        
        public Game(int timesPlayed, int amountOfGamesPlayed,string name, int userId, int sessionId, float windowHeight, float windowWidth)
        {
            TimesPlayed = timesPlayed;
            UserId = userId;
            AmountOfGamesPlayed = amountOfGamesPlayed;
            Name = name;
            SessionId = sessionId;
            var idString = $"{userId}{sessionId}{amountOfGamesPlayed}{timesPlayed}";
            Id = int.Parse(idString);
            Objects = new List<ObjectInGame>();
            WindowHeight = windowHeight;
            WindowWidth = windowWidth;
        }
    }
}
﻿using System.Collections.Generic;
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
        public int SessionId { get; set; }
        [NotNull]
        public string Name { get; }
        
        [Nullable]
        public List<ObjectInGame> Objects { get; }

        public Game(int timesPlayed,string name, int userId, int sessionId)
        {
            TimesPlayed = timesPlayed;
            Name = name;
            SessionId = sessionId;
            var idString = $"{userId}{sessionId}{timesPlayed}";
            Id = int.Parse(idString);
            Objects = new List<ObjectInGame>();
        }
    }
}
using System.Collections.Generic;
using LinqToDB.Mapping;

namespace Objects
{
    public class ObjectInGame
    {
        [PrimaryKey,Column(Length = 50)]
        public string Name { get; set; }
        [NotNull]
        public int GameId { get; set; }
        [NotNull]
        public Aoi Aoi { get; set; }
        [Nullable]
        public List<Point> Points { get; set; }

        public ObjectInGame(string objectName, Aoi aoi, int gameId)
        {
            GameId = gameId;
            Name = $"{gameId} {objectName}";
            Aoi = aoi;
            Points = new List<Point>();
        }
    }
}
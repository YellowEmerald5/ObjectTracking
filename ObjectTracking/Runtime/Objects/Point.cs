using LinqToDB.Mapping;

namespace Objects
{
    public class Point
    {
        [PrimaryKey,Identity]
        public int Id { get; set; }
        [NotNull,Column(Length = 50)]
        public string ObjectName { get; set; }
        [NotNull]
        public long Time { get; }
        [NotNull]
        public float PosX { get; }
        [NotNull]
        public float PosY { get; }

        public Point(string objectName, long time, float posX, float posY)
        {
            ObjectName = objectName;
            Time = time;
            PosX = posX;
            PosY = posY;
        }
    }
}
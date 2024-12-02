using LinqToDB.Mapping;

namespace Objects
{
    public class Point
    {
        [PrimaryKey,Identity]
        public int Id { get; set; }
        [NotNull,Column(Length = 100)]
        public string ObjectName { get; set; }
        [NotNull]
        public long Time { get; }
        [NotNull]
        public float PosX { get; }
        [NotNull]
        public float PosY { get; }
        [NotNull]
        public float PosZ { get; }

        public Point(string objectName, long time, float posX, float posY, float posZ)
        {
            ObjectName = objectName;
            Time = time;
            PosX = posX;
            PosY = posY;
            PosZ = posZ;
        }
    }
}
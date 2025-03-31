using LinqToDB.Mapping;

namespace Objects
{
    public class Point
    {
        [PrimaryKey,Identity]
        public int Id { get; set; }
        [NotNull]
        public int ObjectId { get; set; }
        [NotNull]
        public long Time { get; }
        [NotNull]
        public float PosX { get; }
        [NotNull]
        public float PosY { get; }
        [NotNull]
        public float PosZ { get; }

        public Point(int objectId, long time, float posX, float posY, float posZ)
        {
            ObjectId = objectId;
            Time = time;
            PosX = posX;
            PosY = posY;
            PosZ = posZ;
        }
    }
}
using LinqToDB.Mapping;
using UnityEngine;

namespace Objects
{
    public class AoiOrigin
    {
        [PrimaryKey, Identity]
        public int Id { get; set; }
        [NotNull]
        public int AoiId { get; set; }
        [NotNull]
        public float PosX { get; set; }
        [NotNull]
        public float PosY { get; set; }
        
        public AoiOrigin(int aoiId,Vector3 origin)
        {
            AoiId = aoiId;
            PosX = origin.x;
            PosY = origin.y;
        }
    }
}
using LinqToDB.Mapping;
using UnityEngine;

namespace Objects
{
    public class AoiOrigin
    {
        [PrimaryKey, Identity]
        public int Id { get; set; }
        [NotNull, Column(Length = 255)]
        public string AreaOfInterestId { get; set; }
        [NotNull]
        public float PosX { get; set; }
        [NotNull]
        public float PosY { get; set; }
        
        public AoiOrigin(string areaOfInterestId,Vector3 origin)
        {
            AreaOfInterestId = areaOfInterestId;
            PosX = origin.x;
            PosY = origin.y;
        }
    }
}
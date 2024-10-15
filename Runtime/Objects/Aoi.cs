using System.Collections.Generic;
using LinqToDB.Mapping;
using UnityEngine;

namespace Objects
{
    public class Aoi
    {
        [PrimaryKey,Column(Length = 255)]
        public string Id { get; set; }
        [NotNull,Column(Length = 50)]
        public string ObjectName { get; set; }
        [Nullable]
        public List<AoiOrigin> Origins { get; set; }
        [NotNull]
        public long TimeSpawn { get; set; }
        [NotNull]
        public long TimeDestroy { get; set; }
        [NotNull]
        public float StartPositionX { get; set; }
        [NotNull]
        public float StartPositionY { get; set; }
        [NotNull]
        public float EndPositionX { get; set; }
        [NotNull]
        public float EndPositionY { get; set; }
        [NotNull]
        public float Height { get; set; }
        [NotNull]
        public float Width { get; set; }
        
        public Aoi(string objectName, float height, float width,long timeSpawn,Vector3 position)
        {
            Id = objectName + " aoi";
            ObjectName = objectName;
            Height = height;
            Width = width;
            TimeSpawn = timeSpawn;
            StartPositionX = position.x;
            StartPositionY = position.y;
            Origins = new List<AoiOrigin>();
        }
    }
}

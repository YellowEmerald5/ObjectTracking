using System.Collections.Generic;
using LinqToDB.Mapping;
using UnityEngine;

namespace Objects
{
    public class Aoi
    {
        [PrimaryKey,Column(Length = 100)]
        public string Id { get; set; }
        [NotNull,Column(Length = 100)]
        public string ObjectName { get; set; }
        [Nullable]
        public List<AoiOrigin> Origins { get; set; }
        [Nullable]
        public List<AoiSize> Sizes { get; set; }
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
        
        public Aoi(string objectName,long timeSpawn,Vector3 position)
        {
            Id = objectName + " aoi";
            ObjectName = objectName;
            TimeSpawn = timeSpawn;
            StartPositionX = position.x;
            StartPositionY = position.y;
            Origins = new List<AoiOrigin>();
            Sizes = new List<AoiSize>();
        }
    }
}

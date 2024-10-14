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
        [NotNull]
        public float Height { get; set; }
        [NotNull]
        public float Width { get; set; }
        
        public Aoi(string objectName, float height, float width)
        {
            Id = objectName + " aoi";
            ObjectName = objectName;
            Height = height;
            Width = width;
        }
    }
}

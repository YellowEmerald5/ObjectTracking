using System.CodeDom.Compiler;
using System.Collections.Generic;
using BehaviourScripts;
using LinqToDB.Mapping;

namespace Objects
{
    public class AreaOfInterest
    {
        [PrimaryKey,Column(Length = 50)]
        public string Id { get; set; }
        [NotNull]
        public float Height { get; set; }
        [NotNull]
        public float Width { get; set; }
        [NotNull]
        public List<AoiOrigin> Origins { get; set; }

        public AreaOfInterest(string id,float height, float width)
        {
            Id = id;
            Height = height;
            Width = width;
            Origins = new List<AoiOrigin>();
        }
    }
}
using System.CodeDom.Compiler;
using BehaviourScripts;
using LinqToDB.Mapping;

namespace Objects
{
    public class AreaOfInterest
    {
        [PrimaryKey,Identity]
        public string Id { get; set; }
        [NotNull]
        public float Height { get; set; }
        [NotNull]
        public float Width { get; set; }

        /// <summary>
        /// Creates an object of type Aoi
        /// </summary>
        /// <param name="height">Height of the area of interest</param>
        /// <param name="width">Width of the area of interest</param>
        public AreaOfInterest(float height, float width)
        {
            Height = height;
            Width = width;
            DatabaseManager.SaveObjectToDatabase(this);
        }
    }
}
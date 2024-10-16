using LinqToDB.Mapping;

namespace Objects
{
    public class AoiSize
    {
        [PrimaryKey, Identity]
        public int Id { get; set; }
        [NotNull,Column(Length = 100)]
        public string AoiId { get; set; }
        [NotNull]
        public float Height { get; set; }
        [NotNull]
        public float Width { get; set; }

        public AoiSize(string aoiId, float height, float width)
        {
            AoiId = aoiId;
            Height = height;
            Width = width;
        }
    }
}
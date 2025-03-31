using LinqToDB.Mapping;

namespace Objects
{
    public class AoiSize
    {
        [PrimaryKey, Identity]
        public int Id { get; set; }
        [NotNull,Column(Length = 100)]
        public int AoiId { get; set; }
        [NotNull]
        public float Height { get; set; }
        [NotNull]
        public float Width { get; set; }

        public AoiSize(int aoiId, float height, float width)
        {
            AoiId = aoiId;
            Height = height;
            Width = width;
        }
    }
}
namespace Explorer.API.Dtos.Tours
{
    public class MapObjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? PictureURL { get; set; }
        public string? Category { get; set; }
        public float? Longitude { get; set; }
        public float? Latitude { get; set; }
    }
}

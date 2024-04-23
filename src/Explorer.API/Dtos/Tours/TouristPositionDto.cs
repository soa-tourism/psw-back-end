namespace Explorer.API.Dtos.Tours
{
    public class TouristPositionDto
    {
        public string? Id { get; set; }
        public long CreatorId { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
    }
}

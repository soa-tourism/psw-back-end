namespace Explorer.Tours.API.Dtos
{
    public class TouristPositionDto
    {
        public long Id { get; set; }
        public long CreatorId { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
    }
}

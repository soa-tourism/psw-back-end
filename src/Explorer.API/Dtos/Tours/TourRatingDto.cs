namespace Explorer.API.Dtos.Tours
{
    public class TourRatingDto
    {
        public string? Id { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public long TouristId { get; set; }
        public string? TourId { get; set; }
        public DateTime TourDate { get; set; }
        public DateTime ReviewDate { get; set; }
        public List<byte[]>? Images { get; set; }
        public List<string>? ImageNames { get; set; }
    }
}
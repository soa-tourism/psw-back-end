namespace Explorer.API.Dtos.Tours
{
    public class PublishedTourDto
    {
        public string? Id { get; set; }
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Difficulty { get; set; }
        public double Price { get; set; }
        public List<string>? Tags { get; set; }
        public List<EquipmentDto> Equipment { get; set; }

        //     // public List<id
        // > PreviewCheckpoints { get; init; }
        //     public List<TourRatingPreviewDto> TourRating { get; set; }
        //     public List<TourTimeDto> TourTime { get; set; }
    }
}

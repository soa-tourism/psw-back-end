namespace Explorer.Tours.API.Dtos
{
    public class BasicTourDto
    {
        public long Id { get; set; }
        public long AuthorId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Difficulty { get; set; }
        public string Status { get; set; }
        public double Price { get; set; }
        public List<string>? Tags { get; set; }
        public List<EquipmentDto>? Equipment { get; set; }
        public List<CheckpointDto>? Checkpoints { get; set; }
    }
}

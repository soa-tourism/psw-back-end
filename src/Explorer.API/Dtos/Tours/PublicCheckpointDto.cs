namespace Explorer.API.Dtos.Tours
{
    public class PublicCheckpointDto
    {
        public string? Id { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<string> Pictures { get; set; }
    }
}

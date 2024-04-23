namespace Explorer.API.Dtos.Tours
{
    public class CheckpointCompletitionDto
    {
        public string? Id { get; set; }
        public string? TourExecutionId { get; set; }
        public string? CheckpointId { get; set; }
        public DateTime CompletitionTime { get; set; }
    }
}

namespace Explorer.API.Dtos.Tours
{
    public class TourExecutionDto
    {
        public string? Id { get; set; }
        public long TouristId { get; set; }
        public string? TourId { get; set; }
        public DateTime Start { get; set; }
        public DateTime LastActivity { get; set; }
        public string ExecutionStatus { get; set; }
        public List<CheckpointCompletitionDto> CompletedCheckpoints { get; set; }
    }
}

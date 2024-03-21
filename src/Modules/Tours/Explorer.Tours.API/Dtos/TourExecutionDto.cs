namespace Explorer.Tours.API.Dtos
{
    public class TourExecutionDto
    {
        public long Id { get; set; }
        public long TouristId { get; set; }
        public long TourId {  get; set; }
        public DateTime Start { get; set; }
        public DateTime LastActivity { get; set; }
        public string ExecutionStatus { get; set; }
        public List<CheckpointCompletitionDto> CompletedCheckpoints { get; set; }
    }
}

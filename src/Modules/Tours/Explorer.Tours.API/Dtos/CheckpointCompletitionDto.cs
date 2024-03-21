namespace Explorer.Tours.API.Dtos
{
    public class CheckpointCompletitionDto
    {
        public long Id { get; set; } 
        public long TourExecutionId { get; set; }
        public long CheckpointId { get; set; }
        public DateTime CompletitionTime { get; set; }
    }
}

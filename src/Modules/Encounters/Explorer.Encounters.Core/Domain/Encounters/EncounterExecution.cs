using Explorer.BuildingBlocks.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Explorer.Encounters.Core.Domain.Converters;

namespace Explorer.Encounters.Core.Domain.Encounters
{
    public class EncounterExecution : EventSourcedAggregate
    {
        public long EncounterId { get; init; }
        [ForeignKey("EncounterId")]
        public Encounter Encounter { get; set; }
        public long TouristId { get; init; }
        public EncounterExecutionStatus Status { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        [JsonConverter(typeof(SocialEncounterEventConverter))]
        public override List<DomainEvent> Changes { get; set; }
        public EncounterExecution() { }
        public EncounterExecution(long encounterId, Encounter encounter, long touristId, EncounterExecutionStatus status, DateTime startTime, DateTime endTime)
        {
            EncounterId = encounterId;
            Encounter = encounter;
            TouristId = touristId;
            Status = status;
            StartTime = startTime;
            EndTime = endTime;
            Validate();
        }

        public EncounterExecution(long encounterId, Encounter encounter, long touristId)
        {
            EncounterId = encounterId;
            Encounter = encounter;
            TouristId = touristId;
            Status = EncounterExecutionStatus.Pending;
            StartTime = DateTime.UtcNow;
            Validate();
        }
        public void Validate()
        {
            if (EncounterId == 0)
                throw new ArgumentException("Invalid encounter Id.");
            if (TouristId == 0)
                throw new ArgumentException("Invalid tourist.");
            if (string.IsNullOrWhiteSpace(Status.ToString()))
                throw new ArgumentException("Invalid execution status.");
            if (StartTime.Date > DateTime.Now.Date)
                throw new ArgumentException("Invalid StartTime.");
            if (EndTime.Date > DateTime.Now.Date)
                throw new ArgumentException("Invalid EndTime.");
        }

        public void Activate()
        {
            Status = EncounterExecutionStatus.Active;
            this.StartTime = DateTime.UtcNow;
            if(Encounter.Type == EncounterType.Social)
                Causes(new SocialEncounterActivated(this.Id, TouristId, DateTime.UtcNow));
        }
        public void Abandone()
        {
            Status = EncounterExecutionStatus.Abandoned;
        }
        public void Completed()
        {
            if (Encounter.Type == EncounterType.Social && Status == EncounterExecutionStatus.Active)
                Causes(new SocialEncounterCompleted(this.Id, DateTime.UtcNow, TouristId));
            Status = EncounterExecutionStatus.Completed;
            this.EndTime = DateTime.UtcNow;
        }

        public double CheckRangeDistance(double touristLongitude, double touristLatitude)
        {
            if (touristLatitude == Encounter.Latitude && touristLongitude == Encounter.Longitude) return 0;
            double distance = Math.Acos(Math.Sin(Math.PI / 180 * (Encounter.Latitude)) * Math.Sin(Math.PI / 180 * touristLatitude) + Math.Cos(Math.PI / 180 * Encounter.Latitude) * Math.Cos(Math.PI / 180 * touristLatitude) * Math.Cos(Math.PI / 180 * Encounter.Longitude - Math.PI / 180 * touristLongitude)) * 6371000;
            return distance;
        }

        protected void Causes(DomainEvent @event)
        {
            Changes.Add(@event);
            Apply(@event);
        }

        public override void Apply(DomainEvent @event)
        {
            Version++;
        }
    }

    public enum EncounterExecutionStatus
    {
        Pending,
        Completed,
        Active,
        Abandoned
    }
}

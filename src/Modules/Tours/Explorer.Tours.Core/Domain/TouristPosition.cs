using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Tours.Core.Domain
{
    public class TouristPosition : Entity
    {
        public long CreatorId { get; init; }
        public float? Longitude { get; init; }
        public float? Latitude { get; init; }
        public TouristPosition(long creatorId, float? longitude, float? latitude)
        {
            if (latitude < -90 || latitude > 90)
            {
                throw new ArgumentException("Invalid value for Latitude. It should be between -90 and 90.");
            }

            if (longitude < -180 || longitude > 180)
            {
                throw new ArgumentException("Invalid value for Longitude. It should be between -180 and 180.");
            }

            CreatorId = creatorId;
            Longitude = longitude;
            Latitude = latitude;
        }
    }
}

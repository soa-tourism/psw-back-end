namespace Explorer.Encounters.Core.Domain.Encounters
{
    public class SocialEncounter : Encounter
    {
        public int RequiredPeople { get; init; }
        public double Range { get; init; }
        public List<int>? ActiveTouristsIds { get; init; }
        public SocialEncounter() { }

        public SocialEncounter(SocialEncounter socialEncounter)
        {
        }

        public void CreateSocialEncounter(Encounter encounter)
        {
            if (Type == EncounterType.Social)
            {
                Causes(new SocialEncounterCreated(encounter.Id, DateTime.UtcNow, RequiredPeople, Range));
            }
        }
        public int CheckIfInRange(double touristLongitude, double touristLatitude, int touristId)
        {
            Causes(new SocialEncounterLocationUpdated(Id, touristId, DateTime.UtcNow, Longitude, Latitude));
            double distance = Math.Acos(Math.Sin(Math.PI/180*(Latitude)) * Math.Sin(Math.PI / 180 * touristLatitude) + Math.Cos(Math.PI / 180 * Latitude) * Math.Cos(Math.PI / 180 * touristLatitude) * Math.Cos(Math.PI / 180 * Longitude - Math.PI / 180 * touristLongitude)) * 6371000;
            if (distance > Range)
            {
                RemoveTourist(touristId);
                return 0;
            }
            else AddTourist(touristId);
            
            return ActiveTouristsIds.Count();
        }

        private void AddTourist(int touristId)
        {
            if(ActiveTouristsIds != null && !ActiveTouristsIds.Contains(touristId))
            {
                ActiveTouristsIds.Add(touristId);
                Causes(new SocialEncounterRangeChecked(Id, ActiveTouristsIds, DateTime.UtcNow));
            }
        }

        private void RemoveTourist(int touristId)
        {
            if (ActiveTouristsIds != null && ActiveTouristsIds.Contains(touristId))
            {
                ActiveTouristsIds.Remove(touristId);
                Causes(new SocialEncounterRangeChecked(Id, ActiveTouristsIds, DateTime.UtcNow));
            }
        }
        public bool IsRequiredPeopleNumber()
        {
            int numberOfTourists = ActiveTouristsIds.Count();
            if(numberOfTourists >= RequiredPeople) { ClearActiveTourists(); }
            return numberOfTourists >= RequiredPeople;
        }

        private void ClearActiveTourists()
        {
            ActiveTouristsIds.Clear();
        }

    }
}

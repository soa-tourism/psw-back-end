using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class CheckpointDto
    {
        public long Id { get; set; }
        public long TourId { get; set; }
        public long AuthorId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<string>? Pictures { get; set; }
        public double RequiredTimeInSeconds { get; set; }
        //public CheckpointSecretDto? CheckpointSecret { get; set; }
        public long EncounterId { get; set; }
        public bool IsSecretPrerequisite { get; set; }



    }
}

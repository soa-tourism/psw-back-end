using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Explorer.Tours.API.Dtos
{
    public class TourDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Difficulty { get; set; }
        public double Price { get; set; }
        public List<string>? Tags { get; set; }
        public int AuthorId { get; set; }
        public string Status { get; set; }
        public List<EquipmentDto>? Equipment { get; set; }
        public List<CheckpointDto>? Checkpoints { get; set; }
        public List<PublishedTourDto>? PublishedTours { get; set; }
        public List<ArchivedTourDto>? ArchivedTours { get; set; }
        public List<TourTimeDto>? TourTimes { get; set;}
        public List<TourRatingDto>? TourRatings { get; set; }
        public bool? Closed { get; set; }
    }
}



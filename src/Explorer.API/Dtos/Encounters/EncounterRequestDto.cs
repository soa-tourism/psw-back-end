using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Dtos
{
    public class EncounterRequestDto
    {
        public int? Id { get; set; }
        public long TouristId { get; set; }
        public long EncounterId { get; set; }
        public string Status { get; set; }
        public EncounterDto? EncounterDto { get; set; }
    }
}

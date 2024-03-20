using Explorer.Tours.API.Dtos;
using Explorer.Tours.Core.Domain.Tours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Mappers
{
    public class CheckpointMapper
    {

        public CheckpointMapper() { }
        public CheckpointDto CreateDto(Checkpoint checkpoint)
        {
            CheckpointDto result = new CheckpointDto();
            CheckpointSecretMapper secretMapper = new CheckpointSecretMapper(); 
            result.Pictures = checkpoint.Pictures;
            result.Longitude = checkpoint.Longitude;
            result.Latitude = checkpoint.Latitude;
            result.Name = checkpoint.Name;
            result.Description = checkpoint.Description;
            result.Id = checkpoint.Id;
            result.RequiredTimeInSeconds = checkpoint.RequiredTimeInSeconds;
            result.EncounterId= checkpoint.EncounterId;
            result.IsSecretPrerequisite = checkpoint.IsSecretPrerequisite;
            //if(checkpoint.CheckpointSecret!=null)
            //    result.CheckpointSecret = secretMapper.createDto(checkpoint.CheckpointSecret.Description, checkpoint.CheckpointSecret.Pictures);
            //else
            //    result.CheckpointSecret = null;
            return result;

        }

        public List<CheckpointDto> createListDto(List<Checkpoint> checkpointList)
        {
            List<CheckpointDto> result = new List<CheckpointDto>();
            foreach (Checkpoint cp in checkpointList)
            {
                result.Add(CreateDto(cp));
            }
            return result;
        }
    }
}

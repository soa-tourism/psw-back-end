using Explorer.Tours.API.Dtos;
using Explorer.Tours.Core.Domain;

namespace Explorer.Tours.Core.Mappers
{
    public class TourPreviewMapper
    {
        private CheckpointPreviewMapper checkpointMapper;
        private EquipmentMapper equipmentMapper;
        private TourRatingPreviewMapper ratingMapper;
        private TourTimeMapper timeMapper;
        public TourPreviewMapper() 
        {
            checkpointMapper= new CheckpointPreviewMapper();
            equipmentMapper= new EquipmentMapper();
            ratingMapper= new TourRatingPreviewMapper();
            timeMapper= new TourTimeMapper();
        }

        public TourPreviewDto createDto(TourPreview tourPreview)
        {

            TourPreviewDto result= new TourPreviewDto();

            result.Name= tourPreview.Name;
            result.Id=tourPreview.Id;
            result.AuthorId=tourPreview.AuthorId;
            result.Description=tourPreview.Description;
            result.Price=tourPreview.Price;
            result.Difficulty=tourPreview.DemandignessLevel.ToString();
            result.Checkpoint = checkpointMapper.CreateDto(tourPreview.Checkpoint);
            result.Tags=tourPreview.Tags;
            result.Equipment=equipmentMapper.createListDto(tourPreview.Equipment);          
            result.TourRating = ratingMapper.createListDto(tourPreview.TourRatings);
            result.TourTime = timeMapper.createListDto(tourPreview.TourTimes);


            return result;
        }

        public List<TourPreviewDto> createDtoList(List<TourPreview> tourPreviewList)
        {
            List<TourPreviewDto> tourPreviewDtos = new List<TourPreviewDto>();
            foreach(var  tourPreview in tourPreviewList)
            {
                tourPreviewDtos.Add(createDto(tourPreview));
            }
            return tourPreviewDtos;
        }
    }
}

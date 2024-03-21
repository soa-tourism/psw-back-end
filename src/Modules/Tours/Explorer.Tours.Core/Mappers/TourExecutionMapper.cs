using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.Core.Domain.TourExecutions;

namespace Explorer.Tours.Core.Mappers
{
    public class TourExecutionMapper
    {
        private PurchasedTourPreviewMapper _mapper;
        private CheckpointCompletitionMapper _completitionMapper;
        public TourExecutionMapper()
        {
            _mapper = new PurchasedTourPreviewMapper();
            _completitionMapper = new CheckpointCompletitionMapper();
        }

        public TourExecutionDto createDto(TourExecution tourExecution)
        {
            TourExecutionDto result = new TourExecutionDto();
            result.Id = tourExecution.Id;
            result.TouristId = tourExecution.TouristId;
            //result.Tour = _mapper.createDto(tourExecution.Tour.FilterPurchasedTour(tourExecution.Tour));
            result.ExecutionStatus = tourExecution.ExecutionStatus.ToString();
            result.Start = tourExecution.Start;
            result.LastActivity = tourExecution.LastActivity;
            result.CompletedCheckpoints = _completitionMapper.createDtos(tourExecution.CompletedCheckpoints);
            return result;
        }
   
    }
}

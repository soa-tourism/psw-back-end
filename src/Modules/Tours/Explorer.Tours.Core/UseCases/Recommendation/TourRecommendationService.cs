using Explorer.Stakeholders.Core.Domain;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.API.Public.Recommendation;
using Explorer.Payments.API.Internal;
using Explorer.Tours.Core.Domain.Tours;
using FluentResults;
using Explorer.Stakeholders.API.Internal;
using Explorer.Tours.API.Internal;
using System.Collections.Generic;

namespace Explorer.Tours.Core.UseCases.Recommendation
{
    public class TourRecommendationService : ITourRecommendationService
    {
        private readonly ITourService _tourService;
        private readonly ITourExecutionService _tourExecutionService;
        private readonly ITouristPositionService _touristPositionService;
        private readonly ITourPreferenceService _tourPreferenceService;
        private readonly IInternalTourOwnershipService _internalTourOwnershipService;
        public TourRecommendationService(ITourExecutionService tourExecutionService, ITourService tourService, ITouristPositionService touristPositionService, ITourPreferenceService tourPreferenceService, IInternalTourOwnershipService internalTourOwnershipService)
        {
            _tourExecutionService = tourExecutionService;
            _tourService = tourService;
            _touristPositionService = touristPositionService;
            _tourPreferenceService = tourPreferenceService;
            _internalTourOwnershipService = internalTourOwnershipService;
        }
        public List<PurchasedTourPreviewDto> GetActiveToursInRange(double touristLongitude, double touristLatitude)
        {
            List<PurchasedTourPreviewDto> filteredTours = new List<PurchasedTourPreviewDto>();
            foreach (TourExecutionDto execution in _tourExecutionService.GetActiveTourExecutions().Value)
            {
                if (execution.Tour.Checkpoints != null)
                {
                    bool foundCloseEnough = false;
                    for (int i = 0; i < execution.Tour.Checkpoints.Count; i++)
                    {
                        if (!foundCloseEnough && IsCloseEnough(touristLongitude, touristLatitude, execution.Tour.Checkpoints[i].Longitude, execution.Tour.Checkpoints[i].Latitude))
                        {
                            filteredTours.Add(execution.Tour);
                            foundCloseEnough = true;
                            break;
                        }
                    }
                }
            }
            return filteredTours;
        }

        public double GetTourDistanceFromTouristPosition(double touristLongitude, double touristLatitude, double tourLongitude, double tourLatitude)
        {
            return Math.Acos(Math.Sin(Math.PI / 180 * (touristLatitude)) * Math.Sin(Math.PI / 180 * tourLatitude) + Math.Cos(Math.PI / 180 * touristLatitude) * Math.Cos(Math.PI / 180 * tourLatitude) * Math.Cos(Math.PI / 180 * touristLongitude - Math.PI / 180 * tourLongitude)) * 6371000;
        }

        public List<TourDto> GetToursInRange(double touristLongitude, double touristLatitude)
        {
            List<TourDto> tours = _tourService.GetTours().Value;
            List<TourDto> filteredTours = new List<TourDto>();
            foreach (TourDto tour in tours)
            {
                if (tour.Checkpoints != null)
                {
                    bool foundCloseEnough = false;
                    for (int i = 0; i < tour.Checkpoints.Count; i++)
                    {
                        if (!foundCloseEnough && IsCloseEnough(touristLongitude, touristLatitude, tour.Checkpoints[i].Longitude, tour.Checkpoints[i].Latitude))
                        {
                            if(tour.Status.Equals(TourStatus.Published.ToString()))
                            {
                                filteredTours.Add(tour);
                            }
                            foundCloseEnough = true;
                            break;
                        }
                    }
                }
            }
            return filteredTours;
        }

        public bool IsCloseEnough(double touristLongitude, double touristLatitude, double tourLongitude, double tourLatitude)
        {
            return GetTourDistanceFromTouristPosition(touristLongitude, touristLatitude, tourLongitude, tourLatitude) <= 40000;
        }

        public Result<List<TourPreviewDto>> GetAppropriateTours(int touristId)
        {
            TouristPositionDto touristPositionDto = _touristPositionService.GetPositionByCreator(0, 0, touristId).Value;
            List<TourDto> allTours = GetToursInRange(touristPositionDto.Longitude, touristPositionDto.Latitude);
            List<int> algorithmPoints = Enumerable.Repeat(0, allTours.Count).ToList();
            TourPreferenceDto preference = _tourPreferenceService.GetPreferenceByCreator(0, 0, touristId).Value;
            int algorithmCounter = 0;
            foreach (TourDto tour in allTours)
            {
                TourPreviewDto dummyTour = _tourService.GetPublishedTour(tour.Id).Value;
                if (preference != null)
                {
                    if (tour.Tags != null && preference.Tags != null)
                    {
                        algorithmPoints[algorithmCounter] += (tour.Tags.Intersect(preference.Tags).Count() * 3);
                    }
                    if (tour.Difficulty != null && tour.Difficulty.Equals(preference.Difficulty)) { algorithmPoints[algorithmCounter] += 2; }

                    if (dummyTour != null)
                    {
                        foreach (TourTimeDto tourTime in dummyTour.TourTime)
                        {
                            if (tourTime.Transportation.Contains("driving") && preference.Car == 1)
                            {
                                algorithmPoints[algorithmCounter] += 1;
                            }
                            if (tourTime.Transportation.Contains("cycling") && preference.Bike == 1)
                            {
                                algorithmPoints[algorithmCounter] += 1;
                            }
                        }
                    }
                }
                if (tour.TourRatings != null)
                {
                    double sumOfRatings = tour.TourRatings.Sum(obj => obj.Rating);
                    double averageRating = sumOfRatings / tour.TourRatings.Count();
                    if (averageRating > 4 && tour.TourRatings.Count() > 50)
                    {
                        algorithmPoints[algorithmCounter] += 50;
                    }

                    if (preference != null)
                    {
                        if (tour.TourRatings.Count >0)
                        {
                            TourPreviewDto completedTour = _tourService.GetPublishedTour(tour.TourRatings[0].TourId).Value;
                            if (completedTour != null && completedTour.Tags.Count > 0 && preference != null  && preference.Tags != null && preference.Tags.Count > 0)
                            {
                                algorithmPoints[algorithmCounter] += (completedTour.Tags.Intersect(preference.Tags).Count() * 4);
                            }
                        }
                    }
                }
                algorithmCounter++;
            }

            var combinedList = allTours.Zip(algorithmPoints, (obj, intValue) => new { Object = obj, IntValue = intValue });
            var sortedList = combinedList.OrderBy(item => item.IntValue);
            List<TourDto> sortedObjects = sortedList.Select(item => item.Object).ToList();
            List<TourPreviewDto> frontendObjects = new List<TourPreviewDto>();
            foreach (var obj in sortedObjects)
            {
                frontendObjects.Add(_tourService.GetPublishedTour(obj.Id).Value);
            }
            if (frontendObjects.Count > 10)
            {
                frontendObjects = frontendObjects.Take(10).ToList();
            }
            //PRVI ALGORITAM
            return frontendObjects;
        }


        Result<List<TourPreviewDto>> ITourRecommendationService.GetAppropriateActiveTours(int touristId)
        {
            TouristPositionDto touristPositionDto = _touristPositionService.GetPositionByCreator(0, 0, touristId).Value;
            List<PurchasedTourPreviewDto> allActiveTours = GetActiveToursInRange(touristPositionDto.Longitude, touristPositionDto.Latitude);

            //DRUGI ALGORITAM
            var top10Tours = allActiveTours.OrderByDescending(t => GetActiveTourScore(t)).Take(10).ToList();

            List<TourPreviewDto> returnList = new List<TourPreviewDto>();

            foreach(var obj in top10Tours) { returnList.Add(MapFromPurchasedTour(obj)); }

            return returnList;
        }

        private double GetActiveTourScore(PurchasedTourPreviewDto purchasedTourPreviewDto) 
        {
            double weightNumberOfRatings = 0.4;
            double weightAverageRating = 0.4;
            double weightNumberOfPurchases = 0.2;

            double averageRating = purchasedTourPreviewDto.TourRatings.Any() ? purchasedTourPreviewDto.TourRatings.Average(tour => tour.Rating) : 0.0;
            double numberRating = purchasedTourPreviewDto.TourRatings.Any() ? Math.Log10(purchasedTourPreviewDto.TourRatings.Count()) : 0.0;
            double purchaseNumber = Math.Log10(_internalTourOwnershipService.GetPurchasesNumber(purchasedTourPreviewDto.Id));

            double score = (weightNumberOfRatings * numberRating) +
                            (weightAverageRating * averageRating) +
                            (weightNumberOfPurchases * purchaseNumber);

            return score;
        }

        private TourPreviewDto MapFromPurchasedTour(PurchasedTourPreviewDto purchasedTour)
        {
            return new TourPreviewDto
            {
                Id = purchasedTour.Id,
                Name = purchasedTour.Name,
                Description = purchasedTour.Description,
                DemandignessLevel = purchasedTour.DemandignessLevel,
                Price = purchasedTour.Price,
                Tags = purchasedTour.Tags,
                Equipment = purchasedTour.Equipment,
                Checkpoint = MapFromCheckpoint(purchasedTour.Checkpoints[0]),
                TourRating = purchasedTour.TourRatings,
                TourTime = purchasedTour.TourTimes
            };
        }

        private CheckpointPreviewDto MapFromCheckpoint(CheckpointDto checkpoint)
        {
            return new CheckpointPreviewDto
            {
                Id = checkpoint.Id,
                Longitude = checkpoint.Longitude,
                Latitude = checkpoint.Latitude,
                Name = checkpoint.Name,
                Description = checkpoint.Description,
                Pictures = checkpoint.Pictures ?? new List<string>(),
                RequiredTimeInSeconds = checkpoint.RequiredTimeInSeconds
            };
        }
    }
}

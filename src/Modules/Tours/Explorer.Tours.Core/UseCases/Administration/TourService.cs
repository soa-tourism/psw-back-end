using System.Reflection.Metadata.Ecma335;
using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Internal;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.Core.Domain.Tours;
using Explorer.Tours.Core.Mappers;
using FluentResults;

namespace Explorer.Tours.Core.UseCases.Administration
{
    public class TourService : CrudService<TourDto, Tour>, ITourService
    {
        private readonly ITourRepository _tourRepository;
        private readonly IEquipmentRepository _equipmentRepository;
        private TourPreviewMapper _tourPreviewMapper;
        private PurchasedTourPreviewMapper _purchasedTourPreviewMapper;
        private PublicTourMapper _publicTourMapper;
        private readonly IInternalTourOwnershipService _tourOwnershipService;
        private readonly IInternalItemService _tourItemService;

        public TourService(ITourRepository tourRepository, IMapper mapper, IEquipmentRepository equipmentRepository, IInternalTourOwnershipService tourOwnershipService, IInternalItemService tourItemService) : base(tourRepository, mapper)
        {
            _tourRepository = tourRepository;
            _equipmentRepository = equipmentRepository;
            _tourPreviewMapper = new TourPreviewMapper();
            _purchasedTourPreviewMapper = new PurchasedTourPreviewMapper();
            _publicTourMapper =  new PublicTourMapper();
            _tourOwnershipService = tourOwnershipService;
            _tourItemService = tourItemService;
        }

        public Result<TourDto> Update(TourDto tour, int authorId)
        {
            Tour t = MapToDomain(tour);
            if (!t.IsAuthor(authorId))
                return Result.Fail(FailureCode.InvalidArgument).WithError("Not tour author!");
            try
            {
               var result = CrudRepository.Update(t);
               if (result.Status != TourStatus.Published) return MapToDto(result);
               var tourItemDto = new ItemDto()
               {
                   SellerId = result.AuthorId,
                   ItemId = result.Id,
                   Name = result.Name,
                   Price = result.Price,
                   Type = "Tour"
               };
               _tourItemService.Update(tourItemDto);
                return MapToDto(result);
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }
        public Result Delete(int id, int authorId)
        {
            Tour t;
            try
            {
                t = _tourRepository.Get(id);
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
            if (!t.IsAuthor(authorId))
                return Result.Fail(FailureCode.InvalidArgument).WithError("Not tour author!");
            try
            {
                CrudRepository.Delete(id);
                _tourItemService.Delete(id, "Tour");
                return Result.Ok();
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }
        public Result<List<TourDto>> GetToursByAuthor(int page, int pageSize, int id) 
        { 
            try
            {
                var result = _tourRepository.GetToursByAuthor(id);
                return MapToDto(result);
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }

        }

        public Result<List<TourPreviewDto>> GetFilteredPublishedTours(int page, int pageSize) 
        {
            try
            {
                List<Tour> publishedTours= _tourRepository.GetPublishedTours();
                List<TourPreview> publishedToursPreviews= new List<TourPreview>();
                foreach(var tour in publishedTours)
                {
                    publishedToursPreviews.Add(tour.FilterView(tour));
                }
               return _tourPreviewMapper.createDtoList(publishedToursPreviews);
            }
            catch(Exception e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }

        public Result<TourPreviewDto> GetPublishedTour(long id)
        {
            try
            {
                Tour publishedTour=_tourRepository.Get(id);
                TourPreview publishedTourPreview = publishedTour.FilterView(publishedTour);
                return _tourPreviewMapper.createDto(publishedTourPreview);

            }
            catch (Exception e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        
        }

        public Result<TourDto> AddEquipment(int tourId, int equipmentId, int userId)
        {
            try
            {
                Tour tour = _tourRepository.Get(tourId);
                if (!tour.IsAuthor(userId))
                    return Result.Fail(FailureCode.InvalidArgument).WithError("Not tour author");
                tour = tour.AddEquipment(_equipmentRepository.Get(equipmentId));
                _tourRepository.Update(tour);

                return MapToDto(tour);
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }

        }

        public Result<TourDto> RemoveEquipment(int tourId, int equipmentId, int userId)
        {
            try
            {
                Tour tour = _tourRepository.Get(tourId);
                if (!tour.IsAuthor(userId))
                    return Result.Fail(FailureCode.InvalidArgument).WithError("Not tour author");
                tour = tour.RemoveEquipment(_equipmentRepository.Get(equipmentId));
                _tourRepository.Update(tour);

                return MapToDto(tour);
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }


        public Result<TourDto> Publish(int id, int userId)
        {
            Tour tour;
            try
            {
                tour = _tourRepository.Get(id);
                if(!tour.IsAuthor(userId))
                    return Result.Fail(FailureCode.InvalidArgument).WithError("Not tour author");
                tour.Publish();
                var result = _tourRepository.Update(tour);
                if (result.Status != TourStatus.Published) return MapToDto(result);
                var tourItemDto = new ItemDto()
                {
                    SellerId = result.AuthorId,
                    ItemId = result.Id,
                    Name = result.Name,
                    Price = result.Price,
                    Type = "Tour"
                };
                _tourItemService.Create(tourItemDto);
                return MapToDto(result);
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }

        public Result<TourDto> Archive(int id, int userId)
        {
            try
            {
                var tour = _tourRepository.Get(id);
                if (!tour.IsAuthor(userId))
                    return Result.Fail(FailureCode.InvalidArgument).WithError("Not tour author");
                tour.Archive();
                var result = _tourRepository.Update(tour);
                if (result.Status != TourStatus.Published)
                {
                    _tourItemService.Delete(result.Id, "Tour");
                }
                return MapToDto(result);
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }

        public Result<TourDto> AddTime(TourTimesDto tourTimesDto, int id, int userId)
        {
            try
            {
                var tour = _tourRepository.Get(id);
                if (!tour.IsAuthor(userId))
                    return Result.Fail(FailureCode.InvalidArgument).WithError("Not tour author");
                tour.ClearTourTimes();
                    foreach (var time in tourTimesDto.TourTimes)
                    {
                        tour.AddTime(time.TimeInSeconds, time.Distance, time.Transportation);
                    }
                    var result = _tourRepository.Update(tour);
                    return MapToDto(result);
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }

        public Result<List<PurchasedTourPreviewDto>> GetToursByIds(List<long> tourIds)
        {
            var foundTours = new List<PurchasedTourPreview>();

            foreach (var id in tourIds)
            {
                var tour = _tourRepository.Get(id);
                PurchasedTourPreview purchasedTourPreview = tour.FilterPurchasedTour(tour);

                foundTours.Add(purchasedTourPreview);
            }

            return _purchasedTourPreviewMapper.createDtoList(foundTours);
        }

        public Result<List<TourDto>> GetToursFromSaleByIds(List<long> tourIds)
        {
            var foundTours = new List<Tour>();

            foreach (var id in tourIds)
            {
                var tour = _tourRepository.Get(id);

                foundTours.Add(tour);
            }

            return MapToDto(foundTours);
        }

        public Result<PurchasedTourPreviewDto> GetPurchasedTourById(long purchasedTourId, int userId)
        {
            try
            {
                if (!_tourOwnershipService.IsTourPurchasedByUser(userId, purchasedTourId).Value)
                    throw new InvalidOperationException("This tour is only accessible to customers who have purchased it.");

                var foundTour = _tourRepository.Get(purchasedTourId);

                var foundPurchasedTour = foundTour.FilterPurchasedTour(foundTour);

                return _purchasedTourPreviewMapper.createDto(foundPurchasedTour);
            }
            catch (InvalidOperationException e)
            {
                return Result.Fail(FailureCode.Forbidden).WithError(e.Message);
            }

        }

        public Result<List<PublicTourDto>> GetPublicTours()
        {
            List<Tour> publishedTours = _tourRepository.GetPublishedTours(); 
            List<PublicTour> publicTours = new List<PublicTour>();
            foreach (var tour in publishedTours)
            {
                publicTours.Add(tour.CreatePublicTour(tour));
            }
            return _publicTourMapper.createDtoList(publicTours);
        }

        public double GetAverageRating(long tourId)
        {
            var tour = _tourRepository.Get(tourId);

            return tour.CalculateAverageRating();
        }

        public Result<List<TourPreviewDto>> GetTopRatedTours(int count)
        {
            var publishedTours = _tourRepository.GetPublishedTours();

            var topRatedTours = publishedTours
                .Where(tour => tour.TourRatings != null && tour.TourRatings.Any()) // Provera da li postoje ocene
                .OrderByDescending(tour => tour.TourRatings?.Average(rating => rating.Rating))
                .Take(count)
                .ToList();
            List<TourPreview> publishedToursPreviews = new List<TourPreview>();
            foreach (var tour in topRatedTours)
            {
                publishedToursPreviews.Add(tour.FilterView(tour));
            }
            return _tourPreviewMapper.createDtoList(publishedToursPreviews);
        }

        public Result<List<PublicTourDto>> GetToursByPublicCheckpoints(List<PublicCheckpointDto> checkpoints)
        {
            var tours = GetPublicTours();
            List<PublicTourDto> result= new List<PublicTourDto>();
            bool containsEveryCheckpoint;
            foreach(var tour in tours.Value)
            {
                containsEveryCheckpoint= true;
                foreach (var checkpoint in checkpoints)
                {
                    if (!tour.PreviewCheckpoints.Any(ch => ch.Longitude == checkpoint.Longitude && ch.Latitude == checkpoint.Latitude))
                    {
                        containsEveryCheckpoint = false;
                    }
                }
                if (containsEveryCheckpoint)
                {
                    result.Add(tour);
                }
            }
            return result;
        }

        public Result<List<TourDto>> GetTours()
        {
            try
            {
                var result = _tourRepository.GetTours();
                return MapToDto(result);
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }
    }
}

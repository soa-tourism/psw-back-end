using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;

namespace Explorer.Tours.API.Public.Administration
{
    public interface ITourService
    {
        Result<List<TourDto>> GetToursByAuthor(int page, int pageSize, int id);
        Result<List<TourPreviewDto>> GetFilteredPublishedTours(int page, int pageSize);
        Result<PagedResult<TourDto>> GetPaged(int page, int pageSize);
        Result<TourDto> Create(TourDto tour);
        Result<TourDto> Update(TourDto tour, int userId);
        Result Delete(int id, int userId);
        Result<TourDto> AddEquipment(int tourId, int equipmentId, int userId);
        Result<TourDto> RemoveEquipment(int tourId, int equipmentId, int userId);
        Result<TourDto> Get(int id);
        public Result<TourPreviewDto> GetPublishedTour(long id);
        Result<TourDto> Publish(int id, int userId);
        Result<TourDto> AddTime(TourTimesDto tourTimesDto, int id, int userId);
        Result<TourDto> Archive(int id, int userId);
        Result<List<PurchasedTourPreviewDto>> GetToursByIds(List<long> tourIds);
        Result<PurchasedTourPreviewDto> GetPurchasedTourById(long purchasedTourId, int userId);
        Result<List<TourDto>> GetToursFromSaleByIds(List<long> tourIds);
        Result<List<PublicTourDto>> GetPublicTours();
        double GetAverageRating(long tourId);
        Result<List<TourPreviewDto>> GetTopRatedTours(int count);
        Result<List<PublicTourDto>> GetToursByPublicCheckpoints(List<PublicCheckpointDto> checkpoints);
        Result<List<TourDto>> GetTours();
    }
}

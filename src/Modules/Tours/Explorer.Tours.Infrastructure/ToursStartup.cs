using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.API.Internal;
using Explorer.Tours.API.Public;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.API.Public.Recommendation;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.Core.Domain.TourExecutions;
using Explorer.Tours.Core.Domain.Tours;
using Explorer.Tours.Core.Mappers;
using Explorer.Tours.Core.UseCases;
using Explorer.Tours.Core.UseCases.Administration;
using Explorer.Tours.Core.UseCases.Recommendation;
using Explorer.Tours.Infrastructure.Database;
using Explorer.Tours.Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Explorer.Tours.Infrastructure;

public static class ToursStartup
{
    public static IServiceCollection ConfigureToursModule(this IServiceCollection services)
    {
        // Registers all profiles since it works on the assembly
        services.AddAutoMapper(typeof(ToursProfile).Assembly);
        SetupCore(services);
        SetupInfrastructure(services);
        return services;
    }

    private static void SetupCore(IServiceCollection services)
    {
        services.AddScoped<IEquipmentService, EquipmentService>();
        services.AddScoped<IMapObjectService, MapObjectService>();
        services.AddScoped<ITourPreferenceService, TourPreferenceService>();
        services.AddScoped<ITouristPositionService, TouristPositionService>();
        services.AddScoped<ITourExecutionService,TourExecutionService>();
        services.AddScoped<ICheckpointService, CheckpointService>();
        services.AddScoped<ITourService, TourService>();
        services.AddScoped<IReportedIssuesReviewService, ReportedIssuesReviewService>();
        services.AddScoped<IReportingIssueService, ReportingIssueService>();
        services.AddScoped<ITourRatingService, TourRatingService>();
        services.AddScoped<IPublicCheckpointService, PublicCheckpointService>();
        services.AddScoped<IPublicObjectService, PublicMapObjectService>();
        services.AddScoped<ITourExecutionService, TourExecutionService>();
        services.AddScoped<IInternalCheckpointService, InternalCheckpointService>();
        services.AddScoped<ITourBundleService, TourBundleService>();
        services.AddScoped<ITourRecommendationService, TourRecommendationService>();
    }
    private static void SetupInfrastructure(IServiceCollection services)
    {
        services.AddScoped(typeof(ICrudRepository<Equipment>), typeof(CrudDatabaseRepository<Equipment, ToursContext>));
        services.AddScoped(typeof(ICrudRepository<MapObject>), typeof(CrudDatabaseRepository<MapObject, ToursContext>));
        services.AddScoped(typeof(ICrudRepository<TourPreference>), typeof(CrudDatabaseRepository<TourPreference, ToursContext>));
        services.AddScoped(typeof(ICrudRepository<TouristPosition>), typeof(CrudDatabaseRepository<TouristPosition, ToursContext>));
        services.AddScoped(typeof(ICheckpointRepository), typeof(CheckpointDatabaseRepository));
		services.AddScoped(typeof(ITourBundleRepository), typeof(TourBundleDatabaseRepository));
		services.AddScoped(typeof(IReportedIssueRepository), typeof(ReportedIssuesDatabaseRepository));
        services.AddScoped(typeof(ICrudRepository<Tour>), typeof(CrudDatabaseRepository<Tour, ToursContext>));
        services.AddScoped(typeof(ITourEquipmentRepository), typeof(TourEquipmentDatabaseRepository));
        services.AddScoped(typeof(ITourRepository), typeof(TourDatabaseRepository));
        services.AddScoped(typeof(IEquipmentRepository), typeof(EquipmentDatabaseRepository));
        services.AddScoped(typeof(ICrudRepository<ReportedIssue>), typeof(CrudDatabaseRepository<ReportedIssue, ToursContext>));
        services.AddScoped(typeof(ICrudRepository<PublicMapObject>), typeof(CrudDatabaseRepository<PublicMapObject, ToursContext>));
        services.AddScoped(typeof(ICrudRepository<PublicCheckpoint>), typeof(CrudDatabaseRepository<PublicCheckpoint, ToursContext>));
        services.AddScoped(typeof(ITourTourBundleRepository), typeof(TourTourBundleDatabaseRepository));
        services.AddScoped(typeof(ITourExecutionRepository),typeof(TourExecutionDatabaseRepository));
		services.AddScoped<ITourRatingRepository, TourRatingDatabaseRepository>();
		services.AddScoped(typeof(ICrudRepository<TourExecution>),typeof(CrudDatabaseRepository<TourExecution, ToursContext>));
        services.AddScoped(typeof(ICrudRepository<TourRating>), typeof(CrudDatabaseRepository<TourRating, ToursContext>));

        services.AddDbContext<ToursContext>(opt =>
            opt.UseNpgsql(DbConnectionStringBuilder.Build("tours"),
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "tours")));
    }
}
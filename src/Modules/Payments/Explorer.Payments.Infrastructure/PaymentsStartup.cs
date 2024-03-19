using Explorer.BuildingBlocks.Core.UseCases;
using Microsoft.Extensions.DependencyInjection;
using Explorer.Payments.Core.Mappers;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Payments.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Explorer.Payments.API.Internal;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.UseCases;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Explorer.Payments.Infrastructure.Database.Repositories;

namespace Explorer.Payments.Infrastructure;

public static class PaymentsStartup
{
    public static IServiceCollection ConfigurePaymentsModule(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(PaymentsProfile).Assembly);
        SetupCore(services);
        SetupInfrastructure(services);
        return services;
    }

    private static void SetupCore(IServiceCollection services)
    {
        services.AddScoped<IItemOwnershipService, ItemOwnershipService>();
        services.AddScoped<IInternalTourOwnershipService, ItemOwnershipService>();
        services.AddScoped<IInternalItemService, ItemService>();
        services.AddScoped<IInternalShoppingSetupService, ShoppingSetupService>();
        services.AddScoped<IShoppingCartService, ShoppingCartService>();
        services.AddScoped<ISaleService, SaleService>();
        services.AddScoped<ICouponService, CouponService>();
        services.AddScoped<ITouristWalletService, TouristWalletService>();
    }

    private static void SetupInfrastructure(IServiceCollection services)
    {
        services.AddScoped<ITourPurchaseTokenRepository, TourPurchaseTokenDatabaseRepository>();
        services.AddScoped<IShoppingCartRepository, ShoppingCartDatabaseRepository>();
        services.AddScoped<IItemRepository, ItemDatabaseRepository>();
        services.AddScoped(typeof(ICrudRepository<Coupon>), typeof(CrudDatabaseRepository<Coupon, PaymentsContext>));
        services.AddScoped<ICouponRepository, CouponDatabaseRepository>();
        services.AddScoped(typeof(ICrudRepository<Sale>), typeof(CrudDatabaseRepository<Sale, PaymentsContext>));
        services.AddScoped<ISaleRepository, SaleDatabaseRepository>();
        services.AddScoped<ITouristWalletRepository, TouristWalletDatabaseRepository>();
        services.AddScoped(typeof(ICrudRepository<PaymentRecord>), typeof(CrudDatabaseRepository<PaymentRecord, PaymentsContext>));

        services.AddDbContext<PaymentsContext>(opt =>
            opt.UseNpgsql(DbConnectionStringBuilder.Build("payments"),
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "payments")));
    }
}
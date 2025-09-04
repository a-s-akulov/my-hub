using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketsGeneratorServices.Common.Options.Base;
using TicketsGeneratorServices.Common.Services.TicketsGeneratorStorageService;
using TicketsGeneratorServices.Common.Services.TicketsGeneratorStorageService.Repository;
using TicketsGeneratorServices.Db.Context;


namespace TicketsGeneratorServices.Common.Configuration
{
    /// <summary>
    /// Расширения для интеграции TicketsGeneratorStorageDbService в приложение
    /// </summary>
    public static class TicketsGeneratorStorageDbServiceConfigurationHostingExtensions
    {
        #region Методы

        public static IServiceCollection AddTicketsGeneratorStorageDbServiceInApp(this IServiceCollection services, DbConnectionOptions dbConnectionOptions)
        {
            services.AddSingleton<ITicketsGeneratorStorageService, TicketsGeneratorStorageDbService>();

            var connectionString = string.Format(
                dbConnectionOptions.ConnectionString,
                dbConnectionOptions.Username,
                dbConnectionOptions.Password
            );

            services.AddDbContextFactory<TicketsGeneratorDbContext>(opt =>
            {
                opt.UseNpgsql(
                        connectionString,
                        sqlOpt =>
                        {
                            sqlOpt.CommandTimeout(30);
                        }
                    )
                    .EnableDetailedErrors();
            });

            return services;
        }

        #endregion Методы
    }
}
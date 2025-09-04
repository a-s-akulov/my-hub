using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;


namespace TicketsGeneratorServices.Api.Configuration.Swagger
{
    /// <summary>
    /// Настройка поведения Swagger
    /// </summary>
    public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        #region Поля

        /// <summary>
        /// Провайдер описывающие версии контроллеров
        /// </summary>
        private readonly IApiVersionDescriptionProvider _provider;

        #endregion Поля


        #region Конструктор

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        #endregion Конструктор


        #region Методы

        /// <summary>
        /// Конфигурация Swagger
        /// </summary>
        public void Configure(SwaggerGenOptions options)
        {
            // add swagger document for every API version discovered
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
            }

            options.AddSecurityDefinition("API-Key", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.ApiKey,
                Name = "ApiKey",
                In = ParameterLocation.Header,
                Description = "Укажите ключ ApiKey в текстовом поле",
                BearerFormat = "ApiKey {your ApiKey}"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                [
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "API-Key"
                        }
                    }
                ] = new List<string>()
            });
        }

        /// <summary>
        /// Конфигурация Swagger
        /// </summary>
        public void Configure(string? name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        /// <summary>
        /// Настройка стартовой страницы Swagger
        /// </summary>
        private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "TicketsGenerator API",
                Version = description.ApiVersion.ToString(),
                Description = "TicketsGenerator API"
            };

            if (description.IsDeprecated)
            {
                info.Description += ". Внимание! Данная версия АПИ устарела и не рекомендуется к использованию";
            }

            return info;
        }

        #endregion Методы
    }
}
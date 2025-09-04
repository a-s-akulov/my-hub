using System.Reflection;
using System.Xml.Linq;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using TicketsGeneratorServices.ScheduleJobs.Configuration.Swagger;
using TicketsGeneratorServices.ScheduleJobs.Configuration.Swagger.Filters;
using TicketsGeneratorServices.ScheduleJobs.Options;


namespace TicketsGeneratorServices.ScheduleJobs.Configuration
{
    public static class SwaggerConfigurationHostingExtensions
    {
        public static IHostApplicationBuilder AddSwaggerInApp(this IHostApplicationBuilder builder, AppOptions options)
        {
            // Set the comments path for the Swagger JSON and UI.
            var xmlFilesPaths = new string[]
            {
                Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"),
                Path.Combine(AppContext.BaseDirectory, $"{typeof(Common.Configuration.OptionsConfigurationHostingExtensions).Assembly.GetName().Name}.xml"),
                Path.Combine(AppContext.BaseDirectory, $"{typeof(Db.Entities.Base.enLogOperation).Assembly.GetName().Name}.xml")
            };
            var commentsDocuments = new List<XDocument>();
            foreach (var xmlPath in xmlFilesPaths)
                if (File.Exists(xmlPath))
                    commentsDocuments.Add(XDocument.Load(xmlPath));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.AddServer(new Microsoft.OpenApi.Models.OpenApiServer()
                {
                    Description = "Current",
                    Url = options.HostPostfix
                });

                opt.UseAllOfForInheritance();
                opt.UseAllOfToExtendReferenceSchemas();
                opt.SupportNonNullableReferenceTypes();
                opt.SelectSubTypesUsing(baseType => // Необходимо чтобы Generic'и не крашили генерацию файла OpenAPI при включённом UseAllOfForInheritance()
                {
                    return typeof(Program).Assembly.GetTypes().Where(type => !type.IsGenericType && type.BaseType != typeof(Enum) && type.IsSubclassOf(baseType));
                });


                foreach (var xmlFilesPath in xmlFilesPaths)
                    opt.IncludeXmlComments(xmlFilesPath);

                // Обходной путь для лечения косяка генератора (см. в комментариях типа "NullableEnumerableElementsFixSchemaFilter")
                opt.SchemaFilter<NullableEnumerableElementsFixSchemaFilter>();
                // Обходной путь для описания полей перечислений объектов
                opt.SchemaFilter<EnumTypesSchemaFilter>(EnumTypesSchemaFilter.CreateParam(commentsDocuments));
                // Обходной путь для заполнения описания параметров перечислений из описания схемы типа
                opt.OperationFilter<OperationsParametersEnumDescriptionFromSchemaDescriptionFilter>();
            });


            builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

            return builder;
        }


        public static IApplicationBuilder UseSwaggerInApp(this IApplicationBuilder builder)
        {
            var appOptions = builder.ApplicationServices.GetRequiredService<IOptions<AppOptions>>().Value;


            builder.UseSwagger();
            builder.UseSwaggerUI(options =>
            {
                options.DocumentTitle = "TicketsGenerator ScheduleJobs";

                var provider = builder.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions.Reverse()) // .Reverse() ==> Newest API Version first
                {
                    options.SwaggerEndpoint($"{appOptions.HostPostfix}/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });

            return builder;
        }
    }
}
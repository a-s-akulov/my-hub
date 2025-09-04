using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;


namespace TicketsGeneratorServices.Api.Configuration.Swagger.Filters
{
    /// <summary>
    /// Обходной путь для заполнения описания параметров перечислений из описания схемы типа
    /// <br/>Рассчитано на использование <see cref="System.Text.Json.Serialization.JsonStringEnumConverter"/> для сериализации перечислений
    /// <br/>Основано на: <see href="https://habr.com/ru/articles/552624/"/>
    /// </summary>
    public class OperationsParametersEnumDescriptionFromSchemaDescriptionFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            foreach (var operationParameter in operation.Parameters)
            {
                var parameterDescription = context.ApiDescription.ParameterDescriptions.FirstOrDefault(x => x.Name == operationParameter.Name);
                if (parameterDescription == null)
                    continue;

                var paramMetadata = parameterDescription.ModelMetadata;
                if (!paramMetadata.IsEnum)
                    continue;

                if (string.IsNullOrEmpty(operationParameter.Schema.Description))
                    continue;

                var cutStart = operationParameter.Schema.Description.IndexOf("<ul>");
                var cutEnd = operationParameter.Schema.Description.IndexOf("</ul>") + 5;

                if (cutStart < 0 || cutEnd < 0)
                    continue;

                operationParameter.Description += "<p>Variants:</p>"
                    + operationParameter.Schema.Description
                        .Substring(cutStart, cutEnd - cutStart);
            }
        }
    }
}
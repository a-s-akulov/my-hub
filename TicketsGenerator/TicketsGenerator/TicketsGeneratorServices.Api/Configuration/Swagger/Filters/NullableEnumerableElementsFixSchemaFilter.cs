using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;


namespace TicketsGeneratorServices.Api.Configuration.Swagger.Filters
{
    /// <summary>
    /// Дополняет схему OpenApi - позволяет перечислениям содержать Nullable типы элементв
    /// <br/>Без этой штуки в схеме OpenApi все типы элементов перечислений будут обозачены как не допускающие null, даже если по факту это не так
    /// <br/>Проблема существует уже несколько лет, до сих пор не решена - это обходное решение, вдохновленное комментами к проблеме
    /// <br/><seealso href="https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/2059"/>
    /// </summary>
    public class NullableEnumerableElementsFixSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.MemberInfo is not PropertyInfo propertyInfo)
                return;

            var isEnumerableElementNullable = propertyInfo.CheckEnumerableElementIsNullable();
            if (isEnumerableElementNullable != null)
                schema.Items.Nullable = isEnumerableElementNullable.Value;
        }
    }
}
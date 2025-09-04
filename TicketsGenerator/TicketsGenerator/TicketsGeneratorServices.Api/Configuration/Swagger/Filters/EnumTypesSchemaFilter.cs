using System.Xml.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;


namespace TicketsGeneratorServices.Api.Configuration.Swagger.Filters
{
    /// <summary>
    /// Обходной путь для описания полей перечислений объектов
    /// <br/>Рассчитано на использование <see cref="System.Text.Json.Serialization.JsonStringEnumConverter"/> для сериализации перечислений
    /// <br/>Основано на: <see href="https://habr.com/ru/articles/552624/"/>
    /// </summary>
    public class EnumTypesSchemaFilter : ISchemaFilter
    {
        private readonly XDocument[] _xmlCommentsDocuments = [];


        public EnumTypesSchemaFilter(EnumTypesSchemaFilterParam param)
        {
            _xmlCommentsDocuments = param.CommentsDocuments;
        }


        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (_xmlCommentsDocuments.Length == 0)
                return;


            if (context.Type != null && context.Type.IsEnum)
            {
                schema.Description += "<p>Variants:</p><ul>";

                var fullTypeName = context.Type.FullName;

                foreach (var enumMemberName in Enum.GetNames(context.Type))
                {
                    var fullEnumMemberName = $"F:{fullTypeName}.{enumMemberName}";

                    var enumMemberComments = TryFindMemberComments(fullEnumMemberName);
                    if (enumMemberComments == null)
                        continue;

                    var summary = enumMemberComments.Descendants("summary").FirstOrDefault();
                    if (summary == null)
                        continue;

                    schema.Description += $"<li><i>{enumMemberName}</i> - {ConvertSummaryToOpenApiSchema(summary)}</li>";
                }

                schema.Description += "</ul>";
            }
        }


        private string ConvertSummaryToOpenApiSchema(XElement summary)
        {
            var result = summary
                .ToString()
                .Replace("<summary>", "")
                .Replace("</summary>", "")
                .Trim();

            return result;
        }


        private XElement? TryFindMemberComments(string fullMemberName)
        {
            foreach (var xmlCommentsDocument in _xmlCommentsDocuments)
            {
                var xmlCommentDocument = xmlCommentsDocument
                    .Descendants("member")
                    .FirstOrDefault(m =>
                        m.Attribute("name")!.Value.Equals(fullMemberName, StringComparison.OrdinalIgnoreCase)
                    );

                if (xmlCommentDocument != null)
                    return xmlCommentDocument;
            }

            return null;
        }


        public static EnumTypesSchemaFilterParam CreateParam(IEnumerable<XDocument> commentsDocuments) => EnumTypesSchemaFilterParam.Create(commentsDocuments);
    }



    public class EnumTypesSchemaFilterParam
    {
        public XDocument[] CommentsDocuments { get; set; } = [];


        public static EnumTypesSchemaFilterParam Create(IEnumerable<XDocument> commentsDocuments) => new() { CommentsDocuments = commentsDocuments.ToArray() };
    }
}
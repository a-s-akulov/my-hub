using System.ComponentModel;


namespace TicketsGeneratorServices.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string? GetDescription(this Enum source)
        {
            var sourceString = source?.ToString();
            if (sourceString == null)
                return null;

            var fieldInfo = source!.GetType().GetField(sourceString);
            if (fieldInfo == null)
                return null;

            var attributes = fieldInfo
                .GetCustomAttributes(
                    typeof(DescriptionAttribute),
                    false
                )
                as DescriptionAttribute[];

            if (attributes == null || attributes.Length == 0)
                return null;

            return attributes[0].Description;
        }


        public static string GetDescriptionStrict(this Enum source) =>
            GetDescription(source)
            ?? throw new InvalidOperationException($"No {nameof(DescriptionAttribute)} for enum");
    }
}
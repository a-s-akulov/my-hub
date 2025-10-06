using System.Reflection;
using PdfSharp.Fonts;

namespace TicketsGeneratorServices.Api.Configuration.Fonts;

/// <summary>
/// Резолвер шрифтов, читающий TTF из встроенных ресурсов сборки.
/// Ожидаемые ресурсы (имена faceName):
///  - AppSans-Regular.ttf
///  - AppSans-Bold.ttf
///  - AppSans-Italic.ttf
///  - AppSans-BoldItalic.ttf
/// Рекомендуется положить, например, DejaVuSans или NotoSans, переименовав в перечисленные имена.
/// </summary>
public sealed class EmbeddedFontResolver : IFontResolver
{
    private const string Family = "AppSans";

    private const string FaceRegular = "AppSans-Regular";
    private const string FaceBold = "AppSans-Bold";
    private const string FaceItalic = "AppSans-Italic";
    private const string FaceBoldItalic = "AppSans-BoldItalic";

    private static readonly Dictionary<string, byte[]> Cache = new(StringComparer.OrdinalIgnoreCase);
    private static readonly HashSet<string> AvailableFaces;

    static EmbeddedFontResolver()
    {
        var asm = Assembly.GetExecutingAssembly();
        var names = asm.GetManifestResourceNames();
        AvailableFaces = names
            .Where(n => n.Contains(".Assets.Fonts.", StringComparison.OrdinalIgnoreCase) && n.EndsWith(".ttf", StringComparison.OrdinalIgnoreCase))
            .Select(n => System.IO.Path.GetFileNameWithoutExtension(n)!)
            .Select(n => n.Split('.').Last())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    public byte[] GetFont(string faceName)
    {
        if (Cache.TryGetValue(faceName, out var bytes))
            return bytes;

        // Попытка прямого попадания в доступные лица, иначе откат на Regular
        var effectiveFace = AvailableFaces.Contains(faceName) ? faceName : FaceRegular;

        var asm = Assembly.GetExecutingAssembly();
        // Ресурсы должны быть помечены как EmbeddedResource, путь: TicketsGeneratorServices.Api.Assets.Fonts.
        var resourceName = asm.GetManifestResourceNames()
            .FirstOrDefault(n => n.EndsWith($"Assets.Fonts.{effectiveFace}.ttf", StringComparison.OrdinalIgnoreCase));

        if (resourceName == null)
            throw new InvalidOperationException($"Embedded font '{effectiveFace}.ttf' not found. Ensure it is added as EmbeddedResource under Assets/Fonts.");

        using var stream = asm.GetManifestResourceStream(resourceName) ?? throw new InvalidOperationException($"Resource '{resourceName}' not accessible.");
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        bytes = ms.ToArray();
        Cache[faceName] = bytes;
        return bytes;
    }

    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        // Мапим любые запросы на наше семейство
        if (!string.Equals(familyName, Family, StringComparison.OrdinalIgnoreCase))
            familyName = Family;

        // Выбор лучшего доступного варианта
        if (isBold && isItalic && AvailableFaces.Contains(FaceBoldItalic)) return new FontResolverInfo(FaceBoldItalic);
        if (isBold && AvailableFaces.Contains(FaceBold)) return new FontResolverInfo(FaceBold);
        if (isItalic && AvailableFaces.Contains(FaceItalic)) return new FontResolverInfo(FaceItalic);
        return new FontResolverInfo(FaceRegular);
    }
}



using PdfSharp.Fonts;

namespace TicketsGeneratorServices.Api.Configuration.Fonts;

public sealed class WindowsFontResolver : IFontResolver
{
    private const string ArialRegular = "Arial#R";
    private const string ArialBold = "Arial#B";
    private const string ArialItalic = "Arial#I";
    private const string ArialBoldItalic = "Arial#BI";

    public byte[] GetFont(string faceName)
    {
        var fontsDir = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
        string? path = faceName switch
        {
            ArialRegular => Path.Combine(fontsDir, "arial.ttf"),
            ArialBold => Path.Combine(fontsDir, "arialbd.ttf"),
            ArialItalic => Path.Combine(fontsDir, "ariali.ttf"),
            ArialBoldItalic => Path.Combine(fontsDir, "arialbi.ttf"),
            _ => null
        };

        if (path == null || !File.Exists(path))
            throw new InvalidOperationException($"Font file for face '{faceName}' not found.");

        return File.ReadAllBytes(path);
    }

    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        if (string.Equals(familyName, "Arial", StringComparison.OrdinalIgnoreCase))
        {
            if (isBold && isItalic) return new FontResolverInfo(ArialBoldItalic);
            if (isBold) return new FontResolverInfo(ArialBold);
            if (isItalic) return new FontResolverInfo(ArialItalic);
            return new FontResolverInfo(ArialRegular);
        }

        // Fallback на Arial Regular если запрошено неизвестное семейство
        return new FontResolverInfo(ArialRegular);
    }
}



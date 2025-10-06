using AutoMapper;
using System.Diagnostics;
using TicketsGeneratorServices.Api.DTO.V1.TicketsGenerator;
using TicketsGeneratorServices.Api.Properties;
using TicketsGeneratorServices.Api.RequestHandlers.Base;
using TicketsGeneratorServices.Common.Services.TicketsGeneratorStorageService;

using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using PdfSharp.Quality;
using ZXing;
using SkiaSharp;
using System.Runtime.InteropServices;


namespace TicketsGeneratorServices.Api.RequestHandlers.V1.MyAwesomeProducts;


public class GetTicketsV1RequestHandler : RequestHandlerBase<ITicketsGeneratorStorageService, GetTicketsRequest, GetTicketsResponse>
{
    #region Конструкторы

    public GetTicketsV1RequestHandler(ITicketsGeneratorStorageService storageService, ILogger<GetTicketsV1RequestHandler> logger, IMapper mapper, ActivitySource activitySource)
                                                : base(storageService, logger, mapper, activitySource)
    {
    }

    #endregion Конструкторы


    #region Методы

    protected override Task<GetTicketsResponse> HandleCore(GetTicketsRequest request, CancellationToken cancellationToken)
    {
        var visitDate = request.VisitDate ?? DateTimeOffset.Now;
        Stream fileStream;
        try
        {
            fileStream = GenerateDocument(request.PersonsCount <= 0 ? 1 : request.PersonsCount, visitDate);
        }
        catch (Exception)
        {
            throw;
        }

        return Task.FromResult(new GetTicketsResponse
        {
            FileStream = fileStream,
            ContentType = System.Net.Mime.MediaTypeNames.Application.Pdf,
            DownloadFileName = "Билет на посещение территории.pdf"
        });
    }

    private Stream GenerateDocument(int personsCount, DateTimeOffset visitDate)
    {
        var document = new PdfDocument();
        document.Info.Title = "Билеты на посещение территории";

        // Базовые параметры страницы и типографики (в мм → pt)
        var pageMarginPt = MmToPt(10); // поля ~10 мм
        var ticketsPerPage = 2; // два билета на страницу

        // Генерация номера заказа
        var orderId = GenerateOrderId();
        using var barcodeStream = CreateBarcodePng(orderId);
        using var barcodeImage = XImage.FromStream(barcodeStream);

        int remaining = personsCount;
        int ticketIndex = 1;
        while (remaining > 0)
        {
            var page = document.AddPage();
            page.Size = PdfSharp.PageSize.A4;
            var gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Append);

            var contentWidth = page.Width.Point - 2 * pageMarginPt;
            var contentHeight = page.Height.Point - 2 * pageMarginPt;

            // Горизонтальные купоны, приблизительно 170x60 мм, интервал 10 мм
            var ticketWidth = MmToPt(170);
            var ticketHeight = MmToPt(60);
            var verticalGap = MmToPt(10);

            var blocks = Math.Min(ticketsPerPage, remaining);

            var left = pageMarginPt + (contentWidth - ticketWidth) / 2;
            var topStart = pageMarginPt;

            for (int i = 0; i < blocks; i++)
            {
                var top = topStart + i * (ticketHeight + verticalGap);
                DrawTicketBlock(gfx, new XRect(left, top, ticketWidth, ticketHeight), orderId, visitDate, ticketIndex, barcodeImage);
                ticketIndex++;
            }

            remaining -= blocks;
        }

        var output = new MemoryStream();
        document.Save(output, false);
        output.Position = 0;
        return output;
    }

    private void DrawTicketBlock(XGraphics gfx, XRect area, string orderId, DateTimeOffset visitDate, int ticketIndex, XImage barcodeImage)
    {
        // Палитра и шрифты
        var borderPen = new XPen(XColors.Black, 0.6);
        var titleFont = new XFont("AppSans", 9.8, XFontStyleEx.Bold);
        var labelFont = new XFont("AppSans", 7.0, XFontStyleEx.Regular);
        var valueFont = new XFont("AppSans", 8.6, XFontStyleEx.Bold);
        var tinyFont  = new XFont("AppSans", 6.4, XFontStyleEx.Regular);

        // Рамка
        gfx.DrawRectangle(borderPen, area);

        // Внутренние отступы
        double padding = MmToPt(4.5);
        var inner = new XRect(area.X + padding, area.Y + padding, area.Width - 2 * padding, area.Height - 2 * padding);
        // Колонки: текст слева, справа QR + штрихкод
        var rightColWidth = MmToPt(36);
        var textArea = new XRect(inner.X, inner.Y, inner.Width - rightColWidth - MmToPt(4), inner.Height);
        var rightCol = new XRect(textArea.Right + MmToPt(4), inner.Y, rightColWidth, inner.Height);

        // Верхний служебный блок и плашка возраста
        var serviceBlockHeight = MmToPt(10);
        var serviceText = "Посещение рекреационной функциональной\nзоны и зоны хозяйственного назначения ФГБУ";
        var serviceRect = new XRect(textArea.X, textArea.Y, textArea.Width - MmToPt(26), serviceBlockHeight);
        DrawMultiline(gfx, serviceText, tinyFont, XBrushes.DarkGray, serviceRect);
        // Плашка "+0"
        var badgeSize = new XSize(MmToPt(16), MmToPt(12));
        var badgeRect = new XRect(serviceRect.Right + MmToPt(4), serviceRect.Y - MmToPt(2), badgeSize.Width, badgeSize.Height);
        gfx.DrawRectangle(new XPen(XColors.Black, 0.6), badgeRect);
        gfx.DrawString("+0", valueFont, XBrushes.Black, new XRect(badgeRect.X, badgeRect.Y, badgeRect.Width, badgeRect.Height), XStringFormats.Center);

        // Заголовок
        gfx.DrawString("ПРОПУСК", titleFont, XBrushes.Black, new XRect(textArea.X, serviceRect.Bottom + MmToPt(2), textArea.Width, MmToPt(7)), XStringFormats.TopLeft);

        // Информация
        double line = serviceRect.Bottom + MmToPt(8);
        double step = MmToPt(6);
        DrawLabelValue(gfx, labelFont, valueFont, "Дата посещения:", visitDate.ToString("dd.MM.yyyy"), textArea.X, line, textArea.Width);
        line += step * 2;
        DrawLabelValue(gfx, labelFont, valueFont, "Билет:", ticketIndex.ToString(), textArea.X, line, textArea.Width);
        line += step * 2;
        // Нижняя строка "Дата продажи" и "Заказ №" более мелким кеглем
        var bottomY = inner.Bottom - MmToPt(10);
        gfx.DrawString("Дата продажи:", tinyFont, XBrushes.DarkGray, new XRect(textArea.X, bottomY, textArea.Width/2, MmToPt(5)), XStringFormats.TopLeft);
        gfx.DrawString(DateTime.Now.ToString("dd MMMM yyyy г. HH:mm"), tinyFont, XBrushes.Black, new XRect(textArea.X + MmToPt(20), bottomY, textArea.Width/2 - MmToPt(20), MmToPt(5)), XStringFormats.TopLeft);
        gfx.DrawString("Заказ №:", tinyFont, XBrushes.DarkGray, new XRect(textArea.X + textArea.Width/2, bottomY, textArea.Width/2, MmToPt(5)), XStringFormats.TopLeft);
        gfx.DrawString(orderId, tinyFont, XBrushes.Black, new XRect(textArea.X + textArea.Width/2 + MmToPt(14), bottomY, textArea.Width/2 - MmToPt(14), MmToPt(5)), XStringFormats.TopLeft);

        // Правая колонка: QR вверху, штрихкод снизу с поворотом
        var qrSize = MmToPt(10);
        using var qrStream = CreateQrPng(orderId);
        using var qrImage = XImage.FromStream(qrStream);
        gfx.DrawImage(qrImage, rightCol.Right - qrSize, rightCol.Y, qrSize, qrSize);

        var bcWidth = MmToPt(20);
        var bcHeight = MmToPt(66);
        var bcX = rightCol.Right - bcWidth;
        var bcY = rightCol.Y + MmToPt(4);
        // Вертикальный разделитель (слегка левее)
        gfx.DrawLine(new XPen(XColors.Gray, 0.4), rightCol.Left - MmToPt(3.5), inner.Y, rightCol.Left - MmToPt(3.5), inner.Bottom);
        gfx.Save();
        gfx.TranslateTransform(bcX + bcWidth, bcY);
        gfx.RotateTransform(90);
        gfx.DrawImage(barcodeImage, 0, 0, bcHeight, bcWidth);
        gfx.Restore();
        // Вертикальная подпись у штрихкода
        var rotatedText = orderId;
        gfx.Save();
        gfx.TranslateTransform(rightCol.Left - MmToPt(4), bcY + bcHeight);
        gfx.RotateTransform(270);
        gfx.DrawString(rotatedText, tinyFont, XBrushes.Gray, new XRect(0, 0, bcHeight, MmToPt(5)), XStringFormats.TopLeft);
        gfx.Restore();
    }

    private static void DrawLabelValue(XGraphics gfx, XFont labelFont, XFont valueFont, string label, string value, double x, double y, double width)
    {
        gfx.DrawString(label, labelFont, XBrushes.Gray, new XRect(x, y, width, MmToPt(5)), XStringFormats.TopLeft);
        gfx.DrawString(value, valueFont, XBrushes.Black, new XRect(x + MmToPt(28), y, width - MmToPt(28), MmToPt(5)), XStringFormats.TopLeft);
    }

    private static double MmToPt(double mm) => mm * 72.0 / 25.4;

    private static string GenerateOrderId()
    {
        // Удобочитаемый формат: дата + случайные символы
        var rand = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            .Replace("/", "").Replace("+", "").Replace("=", "");
        return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{rand[..8].ToUpper()}";
    }

    private static MemoryStream CreateBarcodePng(string text)
    {
        var writer = new ZXing.BarcodeWriterPixelData
        {
            Format = BarcodeFormat.CODE_128,
            Options = new ZXing.Common.EncodingOptions
            {
                Height = 160,
                Width = 600,
                Margin = 10,
                PureBarcode = true
            }
        };

        var pixelData = writer.Write(text);
        // pixelData.Pixels is in BGRA32
        var info = new SKImageInfo(pixelData.Width, pixelData.Height, SKColorType.Bgra8888, SKAlphaType.Premul);
        using var bitmap = new SKBitmap(info);
        var ptr = bitmap.GetPixels();
        Marshal.Copy(pixelData.Pixels, 0, ptr, pixelData.Pixels.Length);
        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        var ms = new MemoryStream();
        data.SaveTo(ms);
        ms.Position = 0;
        return ms;
    }

    private static MemoryStream CreateQrPng(string text)
    {
        var writer = new ZXing.BarcodeWriterPixelData
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new ZXing.Common.EncodingOptions
            {
                Height = 300,
                Width = 300,
                Margin = 1
            }
        };
        var pixelData = writer.Write(text);
        var info = new SKImageInfo(pixelData.Width, pixelData.Height, SKColorType.Bgra8888, SKAlphaType.Premul);
        using var bitmap = new SKBitmap(info);
        var ptr = bitmap.GetPixels();
        Marshal.Copy(pixelData.Pixels, 0, ptr, pixelData.Pixels.Length);
        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        var ms = new MemoryStream();
        data.SaveTo(ms);
        ms.Position = 0;
        return ms;
    }

    private static void DrawMultiline(XGraphics gfx, string text, XFont font, XBrush brush, XRect rect)
    {
        var lines = text.Split('\n');
        var lineHeight = font.GetHeight();
        var y = rect.Y;
        foreach (var line in lines)
        {
            gfx.DrawString(line, font, brush, new XRect(rect.X, y, rect.Width, lineHeight), XStringFormats.TopLeft);
            y += lineHeight - 1; // небольшой сжатый интерлиньяж
            if (y > rect.Bottom) break;
        }
    }

    #endregion Методы
}

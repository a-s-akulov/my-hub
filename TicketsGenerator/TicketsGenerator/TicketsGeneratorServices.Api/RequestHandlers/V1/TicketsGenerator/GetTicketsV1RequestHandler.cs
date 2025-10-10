using AutoMapper;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using QRCoder;
using SkiaSharp;
using System.Diagnostics;
using Svg.Skia;
using TicketsGeneratorServices.Api.DTO.V1.TicketsGenerator;
using TicketsGeneratorServices.Api.RequestHandlers.Base;
using TicketsGeneratorServices.Common.Services.TicketsGeneratorStorageService;


namespace TicketsGeneratorServices.Api.RequestHandlers.V1.MyAwesomeProducts;


public class GetTicketsV1RequestHandler : RequestHandlerBase<ITicketsGeneratorStorageService, GetTicketsRequest, GetTicketsResponse>
{
    #region Поля

    private static readonly System.Globalization.CultureInfo CULTURE_RU = System.Globalization.CultureInfo.GetCultureInfo("ru-RU");

    #endregion Поля

    #region Конструкторы

    public GetTicketsV1RequestHandler(ITicketsGeneratorStorageService storageService, ILogger<GetTicketsV1RequestHandler> logger, IMapper mapper, ActivitySource activitySource)
                                                : base(storageService, logger, mapper, activitySource)
    {
    }

    #endregion Конструкторы


    #region Методы

    protected override Task<GetTicketsResponse> HandleCore(GetTicketsRequest request, CancellationToken cancellationToken)
    {
        var visitDate = request.VisitDate ?? DateOnly.FromDateTime(DateTime.Today);
        var yesterday = DateTime.Now.AddDays(-1);
        var saleDate = request.SaleDate
            ?? new DateTimeOffset(
                yesterday.Year,
                yesterday.Month,
                yesterday.Day,
                Random.Shared.Next(11, 22),
                Random.Shared.Next(1, 59),
                Random.Shared.Next(1, 59),
                TimeSpan.FromHours(3)
            );


        Stream fileStream;
        try
        {
            // Real request
            fileStream = GenerateDocument(
                    request.PersonsCount <= 0 ? 1 : request.PersonsCount,
                    visitDate,
                    saleDate
                );
            // Test template
            //fileStream = GenerateDocument(
            //    request.PersonsCount <= 0 ? 1 : request.PersonsCount,
            //    new DateOnly(2025, 08, 16),
            //    new DateTimeOffset(2025, 08, 16, 12, 36, 00, TimeSpan.FromHours(3))
            //);
            // Test not template
            //fileStream = GenerateDocument(
            //    request.PersonsCount <= 0 ? 1 : request.PersonsCount,
            //    new DateOnly(2025, 10, 25),
            //    new DateTimeOffset(2025, 10, 25, 07, 11, 00, TimeSpan.FromHours(3))
            //);
        }
        catch (Exception ex)
        {
            Log.LogError(ex, "Failed to generate tickets");
            throw;
        }

        return Task.FromResult(new GetTicketsResponse
        {
            FileStream = fileStream,
            ContentType = System.Net.Mime.MediaTypeNames.Application.Pdf,
            DownloadFileName = "Билет на посещение территории.pdf"
        });
    }


    private Stream GenerateDocument(int personsCount, DateOnly visitDate, DateTimeOffset saleDate)
    {
        // Dev
        //var orderId = "5e91f675-09ed-46";
        //Prod
        var orderId = Guid.NewGuid().ToString("D")[..16];


        // PdfSharp pdf
        using var templateStream = new MemoryStream(Properties.Resources.template);
        using var templateDocument = PdfReader.Open(templateStream, PdfDocumentOpenMode.Import);
        var templatePage = templateDocument.Pages[0];


        using var document = new PdfDocument();
        for (var personId = 0; personId < personsCount; personId++)
            document.Pages.Add(templatePage);

        foreach (var page in document.Pages)
            DrawPage(page, orderId, visitDate, saleDate);

        
        var stream = new MemoryStream();
        document.Save(stream);
        stream.Seek(0, SeekOrigin.Begin);

        return stream;
    }


    private static void DrawPage(PdfPage page, string orderId, DateOnly visitDate, DateTimeOffset saleDate)
    {
        // Dev
        //var ticketId = 81845955;
        // Prod
        var ticketId = Random.Shared.Next(12345678, 98765432);

        SetOrderId(page, orderId);
        SetTicketId(page, ticketId);
        SetTicketIdQr(page, ticketId);
        SetTicketIdBarcode(page, ticketId);
        SetVisitDate(page, visitDate);
        SetSaleDate(page, saleDate);
    }


    private static void SetOrderId(PdfPage pdfPage, string orderId)
    {
        using var gfx = XGraphics.FromPdfPage(pdfPage);

        var font = new XFont("Arial", 6, XFontStyleEx.Bold);
        var areaRect = new XRect(110.4, pdfPage.Height - 29.3, 160, 10);

        gfx.DrawRectangle(XBrushes.White, areaRect);
        gfx.DrawString(orderId, font, XBrushes.Black, areaRect, XStringFormats.CenterLeft);
    }


    private static void SetTicketId(PdfPage pdfPage, int ticketId)
    {
        using var gfx = XGraphics.FromPdfPage(pdfPage);

        var font = new XFont("Arial", 9);
        var areaRect = new XRect(-129.5, 325.8, 80, 10);

        gfx.RotateTransform(-90);
        gfx.DrawRectangle(XBrushes.White, areaRect);
        gfx.DrawString(ticketId.ToString(), font, XBrushes.Black, areaRect, XStringFormats.Center);
    }


    private static void SetTicketIdQr(PdfPage pdfPage, int ticketId)
    {
        using var gfx = XGraphics.FromPdfPage(pdfPage);

        var qrCodeSvg = SvgQRCodeHelper.GetQRCode(ticketId.ToString(), 32, "#000000", "#ffffff", eccLevel: QRCodeGenerator.ECCLevel.Q, drawQuietZones: false);
        using var svgImage = SKSvg.CreateFromSvg(qrCodeSvg);

        using var qrCodeStream = new MemoryStream();
        svgImage.Save(qrCodeStream, SKColors.White);
        qrCodeStream.Seek(0, SeekOrigin.Begin);

        using var image = XImage.FromStream(qrCodeStream);
        image.Interpolate = false;
        
        var areaRect = new XRect(242, pdfPage.Height - 147, 32, 32);

        gfx.DrawRectangle(XBrushes.White, areaRect);
        gfx.DrawImage(image, areaRect);
    }


    private static void SetTicketIdBarcode(PdfPage pdfPage, int ticketId)
    {
        using var gfx = XGraphics.FromPdfPage(pdfPage);

        var areaRect = new XRect(-150, 337, 122, 70);


        using var barcode = new BarcodeStandard.Barcode()
        {
            IncludeLabel = false
        };
        using var barcodeEncoded = barcode.Encode(BarcodeStandard.Type.Code128A, ticketId.ToString(), SKColors.Black, SKColors.White, 140, (int)areaRect.Height);
        using var imageEncoded = barcodeEncoded.Encode();

        using var qrCodeStream = new MemoryStream();
        imageEncoded.SaveTo(qrCodeStream);
        qrCodeStream.Seek(0, SeekOrigin.Begin);

        using var image = XImage.FromStream(qrCodeStream);
        image.Interpolate = false;

        gfx.RotateTransform(-90);
        gfx.DrawRectangle(XBrushes.White, areaRect);
        gfx.DrawImage(image, areaRect);
    }


    private static void SetVisitDate(PdfPage pdfPage, DateOnly visitDate)
    {
        using var gfx = XGraphics.FromPdfPage(pdfPage);

        var font = new XFont("Arial", 6);
        var areaRect = new XRect(138.2, pdfPage.Height - 99.1, 50, 10);

        gfx.DrawRectangle(XBrushes.White, areaRect);
        gfx.DrawString(visitDate.ToString("dd.MM.yyyy"), font, XBrushes.Black, areaRect, XStringFormats.CenterLeft);
    }


    private static void SetSaleDate(PdfPage pdfPage, DateTimeOffset saleDate)
    {
        using var gfx = XGraphics.FromPdfPage(pdfPage);

        var font = new XFont("Arial", 10);
        var areaRect = new XRect(107, pdfPage.Height - 48.2, 160, 15);

        gfx.DrawRectangle(XBrushes.White, areaRect);
        gfx.DrawString(saleDate.ToString("f", CULTURE_RU), font, XBrushes.Black, areaRect, XStringFormats.CenterLeft);
    }

    #endregion Методы
}

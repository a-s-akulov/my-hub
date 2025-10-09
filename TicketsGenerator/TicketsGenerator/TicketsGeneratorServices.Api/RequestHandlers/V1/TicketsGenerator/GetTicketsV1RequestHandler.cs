using AutoMapper;
using PdfSharp.Drawing;
using PdfSharp.Pdf.IO;
using System.Diagnostics;
using PdfSharp.Pdf;
using TicketsGeneratorServices.Api.DTO.V1.TicketsGenerator;
using TicketsGeneratorServices.Api.RequestHandlers.Base;
using TicketsGeneratorServices.Common.Services.TicketsGeneratorStorageService;


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
            // Real request
            //fileStream = GenerateDocument(
            //        request.PersonsCount <= 0 ? 1 : request.PersonsCount,
            //        visitDate
            //    );
            // Test template
            //fileStream = GenerateDocument(
            //    request.PersonsCount <= 0 ? 1 : request.PersonsCount,
            //    new DateOnly(2025, 08, 16),
            //    new DateTimeOffset(2025, 08, 16, 12, 36, 00, TimeSpan.FromHours(3))
            //);
            // Test not template
            fileStream = GenerateDocument(
                request.PersonsCount <= 0 ? 1 : request.PersonsCount,
                new DateOnly(2025, 10, 25),
                new DateTimeOffset(2025, 10, 25, 07, 11, 00, TimeSpan.FromHours(3))
            );
        }
        catch (Exception ex)
        {
            throw ex;
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
        // PdfSharp pdf
        using var templateStream = new MemoryStream(Properties.Resources.template);
        var document = PdfReader.Open(templateStream);
        var page = document.Pages[0];

        SetVisitDate(page, visitDate);
        SetSaleDate(page, saleDate);
        
        var stream = new MemoryStream();
        document.Save(stream);
        stream.Seek(0, SeekOrigin.Begin);

        return stream;
    }


    private static void SetVisitDate(PdfPage pdfPage, DateOnly visitDate)
    {
        using var gfx = XGraphics.FromPdfPage(pdfPage);

        var font = new XFont("Arial", 6);
        var areaRect = new XRect(138.2, pdfPage.Height - 99.1, 50, 10);

        gfx.DrawRectangle(XBrushes.White, areaRect);
        gfx.DrawString(visitDate.ToString("dd.MM.yyyy"), font, XBrushes.Red, areaRect, XStringFormats.CenterLeft);
    }


    private static void SetSaleDate(PdfPage pdfPage, DateTimeOffset saleDate)
    {
        using var gfx = XGraphics.FromPdfPage(pdfPage);

        var font = new XFont("Arial", 10);
        var areaRect = new XRect(107, pdfPage.Height - 48.2, 160, 15);

        gfx.DrawRectangle(XBrushes.White, areaRect);
        gfx.DrawString(saleDate.ToString("f"), font, XBrushes.Red, areaRect, XStringFormats.CenterLeft);
    }

    #endregion Методы
}

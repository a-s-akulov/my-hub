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

    protected override async Task<GetTicketsResponse> HandleCore(GetTicketsRequest request, CancellationToken cancellationToken)
    {
        var fileStream = new MemoryStream(Resources.template);

        return new()
        {
            FileStream = fileStream,
            ContentType = System.Net.Mime.MediaTypeNames.Application.Pdf,
            DownloadFileName = "Билет на посещение территории.pdf"
        };
    }

    private Stream GenerateDocument(int personsCount, DateTimeOffset visitDate)
    {
        var document = new PdfDocument();

        return Stream.Null;
    }

    #endregion Методы
}

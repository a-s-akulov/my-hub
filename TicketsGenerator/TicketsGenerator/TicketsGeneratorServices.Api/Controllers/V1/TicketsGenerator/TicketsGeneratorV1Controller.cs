using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TicketsGeneratorServices.Api.Controllers.Base;
using TicketsGeneratorServices.Api.DTO.V1.TicketsGenerator;


namespace TicketsGeneratorServices.Api.Controllers.V1.TicketsGenerator;


[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/tickets-generator")]
public partial class TicketsGeneratorV1Controller : ControllerInAppBase
{
    #region Конструктор

    public TicketsGeneratorV1Controller(ILogger<TicketsGeneratorV1Controller> logger, IMediator mediator) : base(logger, mediator)
    { }

    #endregion Конструктор


    #region Методы

    [HttpGet]
    [Route("")]
    public async Task<FileStreamResult> GetTickets([FromQuery] GetTicketsRequest request, CancellationToken cancellationToken = default)
    {
        var mediatrRequest = new HandlerRequest<GetTicketsRequest, ApiResponse<GetTicketsResponse>>(request);
        var result = await Mediator.Send(mediatrRequest, cancellationToken).ConfigureAwait(false);

        return File(result.Result.FileStream, result.Result.ContentType, result.Result.DownloadFileName);
    }

    #endregion Методы
}
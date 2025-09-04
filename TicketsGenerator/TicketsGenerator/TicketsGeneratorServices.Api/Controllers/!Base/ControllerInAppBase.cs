using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace TicketsGeneratorServices.Api.Controllers.Base
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public abstract class ControllerInAppBase : ControllerBase
    {
        #region Конструкторы

        protected ControllerInAppBase(ILogger logger, IMediator mediator)
        {
            Log = logger;
            Mediator = mediator;
        }

        #endregion Конструкторы


        #region Свойства

        /// <summary>
        /// Логгер
        /// </summary>
        protected ILogger Log { get; }

        /// <summary>
        /// Посредник между делегатом и его обработчиком
        /// </summary>
        protected IMediator Mediator { get; set; }

        #endregion Свойства


        #region Методы

        protected async Task<ActionResult<TResponse>> HandleRequest<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
                                                                                where TResponse : ApiResponse
        {
            var mediatrRequest = new HandlerRequest<TRequest, TResponse>(request);
            var result = await Mediator.Send(mediatrRequest, cancellationToken).ConfigureAwait(false);

            return StatusCode((int)result.StatusCode, result);
        }

        #endregion Методы
    }
}

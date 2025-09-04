using System.Diagnostics;
using AutoMapper;
using MediatR;


namespace TicketsGeneratorServices.Api.RequestHandlers.Base
{
    public abstract class RequestHandlerBasePaged<TBaseService, TRequest, TResponse> : RequestHandlerBase, IRequestHandler<HandlerRequest<TRequest, ApiPagedResponse<TResponse>>, ApiPagedResponse<TResponse>>
                                                                            where TRequest : ApiPagedRequest
                                                                            where TResponse : class
    {
        #region Конструкторы

        public RequestHandlerBasePaged(TBaseService baseService, ILogger logger, IMapper mapper, ActivitySource activitySource) : base(logger, mapper, activitySource)
        {
            BaseService = baseService;
        }

        #endregion Конструкторы


        #region Свойства

        /// <summary>
        /// Базовый сервис обработчика
        /// </summary>
        protected TBaseService BaseService { get; }

        #endregion Свойства


        #region Методы

        public async Task<ApiPagedResponse<TResponse>> Handle(HandlerRequest<TRequest, ApiPagedResponse<TResponse>> request, CancellationToken cancellationToken)
        {
            var result = await HandleProtectedApiResponse(request.First, cancellationToken)
                .ConfigureAwait(false);

            return result;
        }

        protected virtual async Task<ApiPagedResponse<TResponse>> HandleProtectedApiResponse(TRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = ValidateRequest(request);
                if (validationResult != null)
                    return validationResult;

                var result = await HandleCore(request, cancellationToken).ConfigureAwait(false);
                var response = CreateResponse(result);

                return response;
            }
            catch (Exception ex)
            {
                var handleExceptionResult = HandleException(request, ex);
                if (handleExceptionResult != null)
                    return handleExceptionResult;

                return new ApiPagedResponse<TResponse>(null, System.Net.HttpStatusCode.InternalServerError, errors: ex.ToApiErrors(statusCode: System.Net.HttpStatusCode.InternalServerError));
            }
        }

        protected abstract Task<TResponse> HandleCore(TRequest request, CancellationToken cancellationToken);


        protected virtual ApiPagedResponse<TResponse>? ValidateRequest(TRequest request)
        {
            return null;
        }

        protected virtual ApiPagedResponse<TResponse> CreateResponse(TResponse response)
        {
            return new ApiPagedResponse<TResponse>(response);
        }

        protected virtual ApiPagedResponse<TResponse>? HandleException(TRequest request, Exception exception)
        {
            return null;
        }

        #endregion Методы
    }
}
using System.Diagnostics;
using AutoMapper;
using MediatR;


namespace TicketsGeneratorServices.Api.RequestHandlers.Base
{
    public abstract class RequestHandlerBase<TBaseService, TRequest, TResponse> : RequestHandlerBase, IRequestHandler<HandlerRequest<TRequest, ApiResponse<TResponse>>, ApiResponse<TResponse>>
                                                                    where TRequest : ApiRequest
                                                                    where TResponse : class
    {
        #region Конструкторы

        public RequestHandlerBase(TBaseService baseService, ILogger logger, IMapper mapper, ActivitySource activitySource) : base(logger, mapper, activitySource)
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

        public async Task<ApiResponse<TResponse>> Handle(HandlerRequest<TRequest, ApiResponse<TResponse>> request, CancellationToken cancellationToken)
        {
            var result = await HandleProtectedApiResponse(request.First, cancellationToken)
                .ConfigureAwait(false);

            return result;
        }

        protected virtual async Task<ApiResponse<TResponse>> HandleProtectedApiResponse(TRequest request, CancellationToken cancellationToken)
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

                return new ApiResponse<TResponse>(null, System.Net.HttpStatusCode.InternalServerError, errors: ex.ToApiErrors(statusCode: System.Net.HttpStatusCode.InternalServerError));
            }
        }

        protected abstract Task<TResponse> HandleCore(TRequest request, CancellationToken cancellationToken);


        protected virtual ApiResponse<TResponse>? ValidateRequest(TRequest request)
        {
            return null;
        }

        protected virtual ApiResponse<TResponse> CreateResponse(TResponse response)
        {
            return new ApiResponse<TResponse>(response);
        }

        protected virtual ApiResponse<TResponse>? HandleException(TRequest request, Exception exception)
        {
            return null;
        }

        #endregion Методы
    }
}
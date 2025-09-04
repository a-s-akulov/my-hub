using System.Diagnostics;
using AutoMapper;
using TicketsGeneratorServices.Api.DTO.V1.MyAwesomeProducts;
using TicketsGeneratorServices.Api.RequestHandlers.Base;
using TicketsGeneratorServices.Common.Services.TicketsGeneratorStorageService;
using TicketsGeneratorServices.Db.Entities;


namespace TicketsGeneratorServices.Api.RequestHandlers.V1.MyAwesomeProducts
{
    public class AddOrUpdateProductV1RequestHandler : RequestHandlerBase<ITicketsGeneratorStorageService, AddOrUpdateProductRequest, AddOrUpdateProductResponse>
    {
        #region Конструкторы

        public AddOrUpdateProductV1RequestHandler(ITicketsGeneratorStorageService storageService, ILogger<AddOrUpdateProductV1RequestHandler> logger, IMapper mapper, ActivitySource activitySource)
                                                    : base(storageService, logger, mapper, activitySource)
        {
        }

        #endregion Конструкторы


        #region Методы

        protected override ApiResponse<AddOrUpdateProductResponse>? ValidateRequest(AddOrUpdateProductRequest request)
        {
            List<ApiError>? validationErrors = null;
            if (string.IsNullOrWhiteSpace(request.Product?.Name))
                (validationErrors ??= []).Add(new ApiError()
                {
                    Code = System.Net.HttpStatusCode.BadRequest.ToString(),
                    Title = $"{request.Product}.{nameof(request.Product.Name)} is empty",
                    Detail = $"{request.Product}.{nameof(request.Product.Name)} is required field",
                    Source = nameof(AddOrUpdateProductRequest)
                });

            if (request.Product?.ProductType == null)
                (validationErrors ??= []).Add(new ApiError()
                {
                    Code = System.Net.HttpStatusCode.BadRequest.ToString(),
                    Title = $"{request.Product}.{nameof(request.Product.ProductType)} is empty",
                    Detail = $"{request.Product}.{nameof(request.Product.ProductType)} is required field",
                    Source = nameof(AddOrUpdateProductRequest)
                });

            if (validationErrors != null && validationErrors.Count > 0)
                return new(null, System.Net.HttpStatusCode.BadRequest, errors: validationErrors);

            return null;
        }


        protected override async Task<AddOrUpdateProductResponse> HandleCore(AddOrUpdateProductRequest request, CancellationToken cancellationToken)
        {
            var recordToAddOrUpdate = Map.Map<MyAwesomeProduct>(request.Product);
            var result = await BaseService.TryAddOrUpdate(recordToAddOrUpdate, cancellationToken: cancellationToken).ConfigureAwait(false);

            if (result.IsAdded)
                Log.LogInformation("My awesome product ID '{MyAwesomeProductId}' was added", result.Entity.Id);
            else if (result.IsUpdated)
                Log.LogInformation("My awesome product ID '{MyAwesomeProductId}' was updated", result.Entity.Id);
            else
                Log.LogInformation("My awesome product ID '{MyAwesomeProductId}' has no changes", result.Entity.Id);

            return new() { Product = result.Entity };
        }


        protected override ApiResponse<AddOrUpdateProductResponse>? HandleException(AddOrUpdateProductRequest request, Exception exception)
        {
            Log.LogError(exception, "Failed to add or update my awesome product with ID '{MyAwesomeProductId}' and Name " + $"'{request.Product?.Name}'", request.Product?.Id);
            return null;
        }

        #endregion Методы
    }
}

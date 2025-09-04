using System.Diagnostics;
using AutoMapper;
using TicketsGeneratorServices.Api.DTO.V1.MyAwesomeProducts;
using TicketsGeneratorServices.Api.RequestHandlers.Base;
using TicketsGeneratorServices.Common.Services.TicketsGeneratorStorageService;


namespace TicketsGeneratorServices.Api.RequestHandlers.V1.MyAwesomeProducts
{
    public class GetProductsV1RequestHandler : RequestHandlerBase<ITicketsGeneratorStorageService, GetProductsRequest, GetProductsResponse>
    {
        #region Конструкторы

        public GetProductsV1RequestHandler(ITicketsGeneratorStorageService storageService, ILogger<GetProductsV1RequestHandler> logger, IMapper mapper, ActivitySource activitySource)
                                                    : base(storageService, logger, mapper, activitySource)
        {
        }

        #endregion Конструкторы


        #region Методы

        protected override async Task<GetProductsResponse> HandleCore(GetProductsRequest request, CancellationToken cancellationToken)
        {
            var result = await BaseService.GetMyAwesomeProducts(
                    idsFilter: request.Filters?.Ids,
                    productTypesFilter: request.Filters?.ProductTypes,
                    includeReferences: true, cancellationToken: cancellationToken
                )
                .ConfigureAwait(false);

            return new() { Products = result };
        }

        #endregion Методы
    }
}

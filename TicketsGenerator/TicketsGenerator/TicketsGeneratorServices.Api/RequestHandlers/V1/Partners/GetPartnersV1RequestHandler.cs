using System.Diagnostics;
using AutoMapper;
using TicketsGeneratorServices.Api.DTO.V1.Partners;
using TicketsGeneratorServices.Api.RequestHandlers.Base;
using TicketsGeneratorServices.Common.Services.PartnersService;


namespace TicketsGeneratorServices.Api.RequestHandlers.V1.Partners
{
    public class GetPartnersV1RequestHandler : RequestHandlerBase<IPartnersService, GetPartnersRequest, GetPartnersResponse>
    {
        #region Конструкторы

        public GetPartnersV1RequestHandler(IPartnersService partnersService, ILogger<GetPartnersV1RequestHandler> logger, IMapper mapper, ActivitySource activitySource)
                                                    : base(partnersService, logger, mapper, activitySource)
        { }

        #endregion Конструкторы


        #region Методы

        protected override async Task<GetPartnersResponse> HandleCore(GetPartnersRequest request, CancellationToken cancellationToken)
        {
            var result = await BaseService
                .GetPartnersInfo(partnersIdsFilter: request.Filters?.PartnersIds)
                .ConfigureAwait(false);

            return new GetPartnersResponse() { Partners = result };
        }

        #endregion Методы
    }
}

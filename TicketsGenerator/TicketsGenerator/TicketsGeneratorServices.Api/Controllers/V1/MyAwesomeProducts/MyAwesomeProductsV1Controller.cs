using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TicketsGeneratorServices.Api.Auth;
using TicketsGeneratorServices.Api.Controllers.Base;
using TicketsGeneratorServices.Api.DTO.V1.MyAwesomeProducts;


namespace TicketsGeneratorServices.Api.Controllers.V1.MyAwesomeProducts
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/my-awesome-products")]
    public partial class MyAwesomeProductsV1Controller : ControllerInAppBase
    {
        #region Конструктор

        public MyAwesomeProductsV1Controller(ILogger<MyAwesomeProductsV1Controller> logger, IMediator mediator) : base(logger, mediator)
        { }

        #endregion Конструктор


        #region Методы

        [HttpGet]
        [Route("")]
        [ApiAuthorize(enAppAction.MyAwesomeProductsRead)]
        [ProducesDefaultResponseType(typeof(ApiResponse<GetProductsResponse>))]
        [ProducesResponseType(typeof(ApiResponse<GetProductsResponse>), StatusCodes.Status200OK)]
        public Task<ActionResult<ApiResponse<GetProductsResponse>>> GetProducts([FromQuery] GetProductsRequest request, CancellationToken cancellationToken = default) =>
            HandleRequest<GetProductsRequest, ApiResponse<GetProductsResponse>>(request, cancellationToken);


        [HttpGet]
        [Route("{id}")]
        [ApiAuthorize(enAppAction.MyAwesomeProductsRead)]
        [ProducesDefaultResponseType(typeof(ApiResponse<GetProductResponse>))]
        [ProducesResponseType(typeof(ApiResponse<GetProductResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<GetProductResponse>), StatusCodes.Status404NotFound)]
        public Task<ActionResult<ApiResponse<GetProductResponse>>> GetProduct(Guid id, CancellationToken cancellationToken = default) =>
            HandleRequest<GetProductRequest, ApiResponse<GetProductResponse>>(new GetProductRequest() { Id = id }, cancellationToken);

        [HttpPut]
        [Route("")]
        [ApiAuthorize(enAppAction.MyAwesomeProductsAdd, enAppAction.MyAwesomeProductsUpdate)]
        [ProducesDefaultResponseType(typeof(ApiResponse<AddOrUpdateProductResponse>))]
        [ProducesResponseType(typeof(ApiResponse<AddOrUpdateProductResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<AddOrUpdateProductResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<AddOrUpdateProductResponse>), StatusCodes.Status400BadRequest)]
        public Task<ActionResult<ApiResponse<AddOrUpdateProductResponse>>> AddOrUpdateProduct([FromBody] AddOrUpdateProductRequest request, CancellationToken cancellationToken = default) =>
            HandleRequest<AddOrUpdateProductRequest, ApiResponse<AddOrUpdateProductResponse>>(request, cancellationToken);

        [HttpDelete]
        [Route("{id}")]
        [ApiAuthorize(enAppAction.MyAwesomeProductsDelete)]
        [ProducesDefaultResponseType(typeof(ApiResponse<DeleteProductResponse>))]
        [ProducesResponseType(typeof(ApiResponse<DeleteProductResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<DeleteProductResponse>), StatusCodes.Status404NotFound)]
        public Task<ActionResult<ApiResponse<DeleteProductResponse>>> DeleteProduct(Guid id, CancellationToken cancellationToken = default) =>
            HandleRequest<DeleteProductRequest, ApiResponse<DeleteProductResponse>>(new DeleteProductRequest() { Id = id }, cancellationToken);

        #endregion Методы
    }
}
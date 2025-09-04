

namespace TicketsGeneratorServices.Api.RequestHandlers.Base
{

    public class RequestHandlerException : ScopedException
    {
        public RequestHandlerException(Exception? exception) : base(exception, "RequestHandler")
        { }

        public RequestHandlerException(string exceptionScope) : base(exceptionScope)
        { }

        public RequestHandlerException(Exception? exception, string exceptionScope) : base(exception, exceptionScope)
        { }

        public RequestHandlerException(string? message, Exception? exception, string exceptionScope) : base(message, exception, exceptionScope)
        { }
    }
}

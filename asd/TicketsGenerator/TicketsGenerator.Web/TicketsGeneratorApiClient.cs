using Microsoft.AspNetCore.Http.Extensions;
using System.Net.Http;


namespace TicketsGenerator.Web;


public class TicketsGeneratorApiClient(HttpClient httpClient)
{
    public async Task<HttpContent> GetTickets(int personsCount, CancellationToken cancellationToken = default)
    {
        var uriBuilder = new UriBuilder(httpClient.BaseAddress);
        var queryBuilder = new QueryBuilder();

        uriBuilder.Path = "api/v1/tickets-generator";
        queryBuilder.Add("PersonsCount", personsCount.ToString());

        uriBuilder.Query = queryBuilder.ToString();
        var response = await httpClient.GetAsync(uriBuilder.Uri, cancellationToken).ConfigureAwait(false);
        
        return response.Content;
    }
}
using Microsoft.AspNetCore.Http.Extensions;
using System.Net.Http;
using System.Globalization;


namespace TicketsGenerator.Web;


public class TicketsGeneratorApiClient(HttpClient httpClient)
{
    public async Task<HttpContent> GetTickets(int personsCount, DateOnly? visitDate, DateTimeOffset? saleDate, CancellationToken cancellationToken = default)
    {
        var uriBuilder = new UriBuilder(httpClient.BaseAddress);
        var queryBuilder = new QueryBuilder();

        uriBuilder.Path = "api/v1/tickets-generator";
        queryBuilder.Add("PersonsCount", personsCount.ToString(CultureInfo.InvariantCulture));
        if (visitDate != null)
            queryBuilder.Add("VisitDate", visitDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        if (saleDate != null)
            queryBuilder.Add("SaleDate", saleDate.Value.ToString("o", CultureInfo.InvariantCulture));

        uriBuilder.Query = queryBuilder.ToString();
        var response = await httpClient.GetAsync(uriBuilder.Uri, cancellationToken).ConfigureAwait(false);
        
        return response.Content;
    }
}
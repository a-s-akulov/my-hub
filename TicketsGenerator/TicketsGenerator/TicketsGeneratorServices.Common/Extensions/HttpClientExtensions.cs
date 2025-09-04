using System.Net.Http.Json;
using System.Text.Json;


namespace TicketsGeneratorServices.Common.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<string> GetStringErrorsFriendlyAsync(this HttpClient httpClient, string requestUri, CancellationToken cancellationToken)
        {
            var httpResponse = await httpClient.GetAsync(requestUri, cancellationToken).ConfigureAwait(false);

            string? contentString = null;
            try
            {
                contentString = await httpResponse.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            }
            catch { }

            if (!httpResponse.IsSuccessStatusCode)
                throw new HttpRequestException($"Response status code does not indicate success: {(int)httpResponse.StatusCode}.\nErrorContent: '{contentString}'", null, httpResponse.StatusCode);

            if (contentString == null)
                throw new InvalidOperationException("Response content string is null");

            return contentString;
        }


        public static async Task<TValue?> GetFromJsonErrorsFriendlyAsync<TValue>(this HttpClient httpClient, string requestUri, JsonSerializerOptions jsonSerializerOptions, CancellationToken cancellationToken)
        {
            var contentString = await httpClient.GetStringErrorsFriendlyAsync(requestUri, cancellationToken).ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<TValue>(contentString, options: jsonSerializerOptions);

            return result;
        }


        public static async Task PatchErrorsFriendlyAsync(this HttpClient httpClient, string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            var httpResponse = await httpClient.PatchAsync(requestUri, content, cancellationToken).ConfigureAwait(false);

            if (httpResponse.IsSuccessStatusCode)
                return;

            string? contentString = null;
            try
            {
                contentString = await httpResponse.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            }
            catch { }

            throw new HttpRequestException($"Response status code does not indicate success: {(int)httpResponse.StatusCode}.\nErrorContent: '{contentString}'", null, httpResponse.StatusCode);
        }


        public static Task PatchAsJsonErrorsFriendlyAsync<TValue>(this HttpClient httpClient, string requestUri, TValue value, JsonSerializerOptions jsonSerializerOptions, CancellationToken cancellationToken)
        {
            var content = JsonContent.Create(value, options: jsonSerializerOptions);
            return httpClient.PatchErrorsFriendlyAsync(requestUri, content, cancellationToken);
        }
    }
}
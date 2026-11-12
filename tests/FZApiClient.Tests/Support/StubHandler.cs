using System.Net;
using System.Text;

namespace FZApi.Tests.Support;

internal class StubHandler(HttpStatusCode status = HttpStatusCode.OK, string body = "") : HttpMessageHandler
{
    public List<RecordedRequest> Requests { get; } = [];

    public HttpStatusCode Status { get; set; } = status;

    public string Body { get; set; } = body;

    public RecordedRequest LastRequest => Requests[^1];

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var content = request.Content is null
            ? null
            : await request.Content.ReadAsStringAsync(cancellationToken);
        Requests.Add(new RecordedRequest(
            request.Method.Method,
            request.RequestUri!,
            request.Headers.ToDictionary(h => h.Key, h => string.Join(", ", h.Value)),
            request.Content?.Headers.ContentType?.MediaType,
            content));

        return new HttpResponseMessage(Status)
        {
            Content = new StringContent(Body, Encoding.UTF8, "application/json"),
        };
    }
}

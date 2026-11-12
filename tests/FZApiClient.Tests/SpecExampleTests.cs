using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using FZApi.Tests.Support;

namespace FZApi.Tests;

public class SpecExampleTests
{
    private static readonly JsonSerializerOptions Indented = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    public static TheoryData<string> OperationIds => new(OperationCases.All.Keys.Order());

    [Fact]
    public void EveryOperationInTheSpecHasATestCase()
    {
        var missing = OpenApiSpec.Operations.Keys.Except(OperationCases.All.Keys).Order();
        Assert.Empty(missing);
    }

    [Fact]
    public void EveryTestCaseIsAnOperationInTheSpec()
    {
        var extra = OperationCases.All.Keys.Except(OpenApiSpec.Operations.Keys).Order();
        Assert.Empty(extra);
    }

    [Theory]
    [MemberData(nameof(OperationIds))]
    public async Task Operation(string operationId)
    {
        var spec = OpenApiSpec.Operations[operationId];
        var testCase = OperationCases.All[operationId];
        var response = testCase.Response ?? spec.Example?.ToJsonString() ?? "";
        var handler = new StubHandler(HttpStatusCode.OK, response);
        var client = new FZApiClient(
            new HttpClient(handler),
            new FZApiClientOptions { ApiKey = "fz_live_test", SessionCookie = "test-session" });

        var result = await testCase.Invoke(client);

        Assert.All(handler.Requests, request =>
        {
            Assert.Equal(spec.Method, request.Method);
            Assert.True(
                spec.Matches(request.Uri.AbsolutePath),
                $"{request.Uri.AbsolutePath} doesn't match {spec.PathTemplate}");
        });
        JsonShape.AssertCovers(response, testCase.DataPath, result);

        var snapshot = new JsonObject
        {
            ["requests"] = new JsonArray(handler.Requests.Select(r => (JsonNode)r.ToSnapshot()).ToArray()),
            ["result"] = JsonShape.Serialize(result),
        };
        await Verify(snapshot.ToJsonString(Indented), "json")
            .UseDirectory("Snapshots")
            .UseParameters(operationId);
    }
}

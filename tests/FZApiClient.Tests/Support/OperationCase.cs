namespace FZApi.Tests.Support;

internal record OperationCase(Func<FZApiClient, Task<object?>> Invoke, string DataPath = "$", string? Response = null);

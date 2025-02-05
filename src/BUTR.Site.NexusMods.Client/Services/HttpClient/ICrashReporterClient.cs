using BUTR.Site.NexusMods.ServerClient;

using Microsoft.Extensions.Options;

using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BUTR.Site.NexusMods.Client.Services;

public interface ICrashReporterClient
{
    Task<CrashReportModel?> GetCrashReportModelAsync(int tenant, string id, CancellationToken ct);
}

public sealed class CrashReporterClient : ICrashReporterClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public CrashReporterClient(HttpClient httpClient, IOptions<JsonSerializerOptions> jsonSerializerOptions)
    {
        _httpClient = httpClient;
        _jsonSerializerOptions = jsonSerializerOptions.Value;
    }

    public async Task<CrashReportModel?> GetCrashReportModelAsync(int tenant, string id, CancellationToken ct)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"{tenant}/{id}.json");
        using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);
        if (!response.IsSuccessStatusCode) return null;
        return await JsonSerializer.DeserializeAsync<CrashReportModel>(await response.Content.ReadAsStreamAsync(ct), _jsonSerializerOptions, ct);
    }
}
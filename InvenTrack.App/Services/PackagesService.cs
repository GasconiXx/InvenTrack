using System.Net.Http.Json;
using Inventrack.App.Models;
using Inventrack.App.Models.Dtos.Packages;
using Inventrack.App.Services.Interfaces;

namespace Inventrack.App.Services;

public sealed class PackagesService : IPackagesService // TODO AJUSTA
{
    private readonly HttpClient _http;

    public PackagesService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("ApiClient");
    }

    public Task<IReadOnlyList<PackageListItemDto>> GetMyPackagesAsync(CancellationToken ct = default)
        => GetListAsync("api/packages/mine", ct);       // TODO

    public Task<IReadOnlyList<PackageListItemDto>> GetAssignedAsync(CancellationToken ct = default)
        => GetListAsync("api/packages/assigned", ct);   // TODO

    public Task<IReadOnlyList<PackageListItemDto>> GetWarehouseAsync(CancellationToken ct = default)
        => GetListAsync("api/packages/warehouse", ct);  // TODO

    public Task<IReadOnlyList<PackageListItemDto>> GetAllAsync(CancellationToken ct = default)
        => GetListAsync("api/packages", ct);            // TODO

    private async Task<IReadOnlyList<PackageListItemDto>> GetListAsync(string url, CancellationToken ct)
    {
        var data = await _http.GetFromJsonAsync<List<PackageListItemDto>>(url, ct);
        return data;
    }
}


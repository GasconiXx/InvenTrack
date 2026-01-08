using System.Net.Http.Json;
using Inventrack.App.Models;
using Inventrack.App.Models.Dtos.Envios;
using Inventrack.App.Models.Dtos.Packages;
using Inventrack.App.Services.Interfaces;

namespace Inventrack.App.Services;

public sealed class PackagesService : IPackagesService // TODO AJUSTA
{
    private readonly HttpClient _http;
    private readonly ISessionService _session;

    public PackagesService(IHttpClientFactory factory, ISessionService session)
    {
        _http = factory.CreateClient("ApiClient");
        _session = session;
    }

    public Task<IReadOnlyList<PackageListItemDto>> GetMyPackagesAsync(CancellationToken ct = default)
    {
        var userId = _session.CurrentUser?.UsuarioId ?? 0;
        return GetListAsync($"api/paquetes/mios?userId={userId}", ct);
    }

    public Task<IReadOnlyList<PackageListItemDto>> GetAssignedAsync(CancellationToken ct = default)
        => GetListAsync("api/packages/assigned", ct);   // TODO

    public Task<IReadOnlyList<PackageListItemDto>> GetWarehouseAsync(CancellationToken ct = default)
        => GetListAsync("api/packages/warehouse", ct);  // TODO

    public Task<IReadOnlyList<PackageListItemDto>> GetAllAsync(CancellationToken ct = default)
        => GetListAsync("api/Paquetes/all", ct);            // TODO

    private async Task<IReadOnlyList<PackageListItemDto>> GetListAsync(string url, CancellationToken ct)
    {
        var data = await _http.GetFromJsonAsync<List<PackageListItemDto>>(url, ct);
        return data;
    }

    public async Task<IReadOnlyList<EnvioListItemDto>> GetMyShipmentsAsync(CancellationToken ct = default)
    {
        var userId = _session.CurrentUser?.UsuarioId ?? 0;

        var data = await _http.GetFromJsonAsync<List<EnvioListItemDto>>(
            $"api/Envios/mios?repartidorId={userId}", ct);

        return data;
    }
}


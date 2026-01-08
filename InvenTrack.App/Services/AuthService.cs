using Inventrack.App.Models.Dtos.Auth;
using Inventrack.App.Services;
using Inventrack.App.Services.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;



public sealed class AuthService : IAuthService
{
    private readonly HttpClient _http;

    public AuthService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("ApiClient");
    }

    public async Task<LoginResponseDto> LoginAsync(string email, string password, CancellationToken ct = default)
    {
        var payload = new LoginRequestDto { Email = email, Password = password };

        using var resp = await _http.PostAsJsonAsync("api/auth/login", payload, ct);

        if (!resp.IsSuccessStatusCode)
        {
            var msg = await resp.Content.ReadAsStringAsync(ct);
            throw new InvalidOperationException(string.IsNullOrWhiteSpace(msg)
                ? "Credenciales incorrectas o error de servidor."
                : msg);
        }

        var data = await resp.Content.ReadFromJsonAsync<LoginResponseDto>(cancellationToken: ct);
        if (data is null || string.IsNullOrWhiteSpace(data.Token))
            throw new InvalidOperationException("Respuesta inválida de login (token vacío).");

        return data;
    }
}


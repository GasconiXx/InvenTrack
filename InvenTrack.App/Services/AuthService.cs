using InvenTrack.Services;
using System.Net.Http.Headers;
using System.Net.Http.Json;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(string email, string password, CancellationToken ct = default);
}

public sealed class AuthService : IAuthService
{
    private readonly HttpClient _http;

    public AuthService(HttpClient http)
    {
        _http = http;
    }

    public async Task<LoginResponse> LoginAsync(string email, string password, CancellationToken ct = default)
    {
        var req = new LoginRequest { Email = email, Password = password };

        // Ajusta ruta a tu API real:
        using var resp = await _http.PostAsJsonAsync("auth/login", req, ct);

        if (!resp.IsSuccessStatusCode)
        {
            var error = await resp.Content.ReadAsStringAsync(ct);
            throw new InvalidOperationException(string.IsNullOrWhiteSpace(error)
                ? "Credenciales incorrectas o error de servidor."
                : error);
        }

        var data = await resp.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken: ct);
        if (data is null || string.IsNullOrWhiteSpace(data.Token))
            throw new InvalidOperationException("Respuesta de login inválida (token vacío).");

        return data;
    }
}


using Inventrack.App.Models.Dtos.Auth;
using Inventrack.App.Models.Dtos.Users;
using Inventrack.App.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Inventrack.App.Services
{
    public sealed class UserService : IUserService
    {
        private readonly HttpClient _http;

        public UserService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        public Task<IReadOnlyList<UserListItemDto>> GetAllUsersAsync(CancellationToken ct = default) =>
            GetListAsync("api/Usuario/all", ct);

        public async Task<UserDto> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var user = await _http.GetFromJsonAsync<UserDto>($"api/usuarios/{id}", ct);
            return user ?? throw new InvalidOperationException("Usuario no encontrado.");
        }

        public async Task CreateAsync(UserUpsertDto dto, CancellationToken ct = default)
        {
            using var resp = await _http.PostAsJsonAsync("api/usuarios", dto, ct);
            var body = await resp.Content.ReadAsStringAsync(ct);
            if (!resp.IsSuccessStatusCode)
                throw new InvalidOperationException(string.IsNullOrWhiteSpace(body) ? "No se pudo crear el usuario." : body);
        }

        public async Task UpdateAsync(int id, UserUpsertDto dto, CancellationToken ct = default)
        {
            using var resp = await _http.PutAsJsonAsync($"api/usuarios/{id}", dto, ct);
            var body = await resp.Content.ReadAsStringAsync(ct);
            if (!resp.IsSuccessStatusCode)
                throw new InvalidOperationException(string.IsNullOrWhiteSpace(body) ? "No se pudo actualizar el usuario." : body);
        }

        private async Task<IReadOnlyList<UserListItemDto>> GetListAsync(string url, CancellationToken ct)
        {
            var data = await _http.GetFromJsonAsync<List<UserListItemDto>>(url, ct);
            return data;
        }

    }
}

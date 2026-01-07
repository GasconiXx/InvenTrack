using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenTrack.Services
{
    public interface ISessionService
    {
        string? Token { get; }
        UsuarioDto? CurrentUser { get; }

        Task SetSessionAsync(string token, UsuarioDto user);
        Task ClearAsync();
    }

    public sealed class SessionService : ISessionService
    {
        private const string TokenKey = "auth_token";
        private const string UserIdKey = "auth_userid";
        private const string UserNameKey = "auth_username";
        private const string UserEmailKey = "auth_useremail";
        private const string UserRoleKey = "auth_roleid";
        private const string UserAlmacenKey = "auth_almacenid";
        private const string UserActivoKey = "auth_activo";

        public string? Token => Preferences.Get(TokenKey, null);

        public UsuarioDto? CurrentUser
        {
            get
            {
                var token = Token;
                var userId = Preferences.Get(UserIdKey, 0);
                if (string.IsNullOrWhiteSpace(token) || userId <= 0) return null;

                return new UsuarioDto
                {
                    UsuarioId = userId,
                    Nombre = Preferences.Get(UserNameKey, ""),
                    Email = Preferences.Get(UserEmailKey, ""),
                    RolId = Preferences.Get(UserRoleKey, 0),
                    AlmacenId = Preferences.ContainsKey(UserAlmacenKey) ? Preferences.Get(UserAlmacenKey, 0) : null,
                    Activo = Preferences.ContainsKey(UserActivoKey) ? Preferences.Get(UserActivoKey, true) : null
                };
            }
        }

        public Task SetSessionAsync(string token, UsuarioDto user)
        {
            Preferences.Set(TokenKey, token);
            Preferences.Set(UserIdKey, user.UsuarioId);
            Preferences.Set(UserNameKey, user.Nombre ?? "");
            Preferences.Set(UserEmailKey, user.Email ?? "");
            Preferences.Set(UserRoleKey, user.RolId);
            if (user.AlmacenId.HasValue) Preferences.Set(UserAlmacenKey, user.AlmacenId.Value);
            Preferences.Set(UserActivoKey, user.Activo ?? true);
            return Task.CompletedTask;
        }

        public Task ClearAsync()
        {
            Preferences.Remove(TokenKey);
            Preferences.Remove(UserIdKey);
            Preferences.Remove(UserNameKey);
            Preferences.Remove(UserEmailKey);
            Preferences.Remove(UserRoleKey);
            Preferences.Remove(UserAlmacenKey);
            Preferences.Remove(UserActivoKey);
            return Task.CompletedTask;
        }
    }
}

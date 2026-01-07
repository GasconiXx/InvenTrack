namespace Inventrack.API.Contracts.Auth
{
    public sealed class LoginRequest
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public sealed class LoginResponse
    {
        public string Token { get; set; } = "";
        public UsuarioDto Usuario { get; set; } = new();
    }

    public sealed class UsuarioDto
    {
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = "";
        public string Email { get; set; } = "";
        public int RolId { get; set; }
        public int? AlmacenId { get; set; }
        public bool? Activo { get; set; }
    }
}

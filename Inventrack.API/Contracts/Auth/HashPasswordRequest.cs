namespace Inventrack.API.Contracts.Auth
{
    public sealed class HashPasswordRequest
    {
        public string Password { get; set; } = "";
        public string? Key { get; set; }
    }
}

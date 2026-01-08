namespace Inventrack.API.Contracts.Users
{
    public sealed class UserListItemDto
    {
        public string Nombre {  get; set; } = "";
        public string Email { get; set; } = "";
        public string Rol { get; set; } = "";
        public string Telefono { get; set; } = "";
    }
}

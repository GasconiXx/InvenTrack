using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventrack.App.Models.Dtos.Users
{
    public sealed class UserListItemDto
    {
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = "";
        public string Email { get; set; } = "";
        public string Telefono { get; set; } = "";
        public string Rol { get; set; } = "";
        public bool Activo { get; set; }
    }
}

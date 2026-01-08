using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventrack.App.Models.Dtos.Auth
{
    public sealed class UserDto
    {
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = "";
        public string Email { get; set; } = "";
        public int RolId { get; set; }
        public int? AlmacenId { get; set; }
        public bool? Activo { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventrack.App.Models.Dtos.Users
{
    public sealed class UserUpsertDto
    {
        public string Nombre { get; set; } = "";
        public string Email { get; set; } = "";
        public string Telefono { get; set; } = "";
        public int RolId { get; set; }
        public bool Activo { get; set; } = true;
        public int? AlmacenId { get; set; }
        public string? ContrasenaHash { get; set; }
    }
}

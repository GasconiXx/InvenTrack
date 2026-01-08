using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventrack.App.Models.Dtos.Auth
{
    public sealed class LoginResponseDto
    {
        public string Token { get; set; } = "";
        public UserDto Usuario { get; set; } = new();
    }
}

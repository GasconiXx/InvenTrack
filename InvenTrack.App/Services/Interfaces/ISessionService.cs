using Inventrack.App.Models.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventrack.App.Services.Interfaces
{
    public interface ISessionService
    {
        string? Token { get; }
        UserDto? CurrentUser { get; }

        Task SetSessionAsync(string token, UserDto user);
        Task ClearAsync();
    }
}

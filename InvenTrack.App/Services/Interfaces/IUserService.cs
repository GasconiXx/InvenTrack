using Inventrack.App.Models.Dtos.Auth;
using Inventrack.App.Models.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventrack.App.Services.Interfaces
{
    public interface IUserService
    {
        Task<IReadOnlyList<UserListItemDto>> GetAllUsersAsync(CancellationToken ct = default);
        Task<UserDto> GetByIdAsync(int id, CancellationToken ct = default);
        Task CreateAsync(UserUpsertDto dto, CancellationToken ct = default);
        Task UpdateAsync(int id, UserUpsertDto dto, CancellationToken ct = default);
    }
}

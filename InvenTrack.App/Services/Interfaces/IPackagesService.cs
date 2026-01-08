using Inventrack.App.Models;
using Inventrack.App.Models.Dtos.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventrack.App.Services.Interfaces
{
    public interface IPackagesService
    {
        Task<IReadOnlyList<PackageListItemDto>> GetMyPackagesAsync(CancellationToken ct = default);
        Task<IReadOnlyList<PackageListItemDto>> GetAssignedAsync(CancellationToken ct = default);
        Task<IReadOnlyList<PackageListItemDto>> GetWarehouseAsync(CancellationToken ct = default);
        Task<IReadOnlyList<PackageListItemDto>> GetAllAsync(CancellationToken ct = default);
    }
}

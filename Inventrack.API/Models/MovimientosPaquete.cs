using System;
using System.Collections.Generic;

namespace Inventrack.API.Models;

public partial class MovimientosPaquete
{
    public int MovimientoId { get; set; }

    public int PaqueteId { get; set; }

    public int AlmacenId { get; set; }

    public int UsuarioId { get; set; }

    public string TipoMovimiento { get; set; } = null!;

    public DateTime? FechaMovimiento { get; set; }

    public string? Observaciones { get; set; }

    public virtual Almacene Almacen { get; set; } = null!;

    public virtual Paquete Paquete { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}

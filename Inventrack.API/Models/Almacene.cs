using System;
using System.Collections.Generic;

namespace Inventrack.API.Models;

public partial class Almacene
{
    public int AlmacenId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string? Telefono { get; set; }

    public virtual ICollection<MovimientosPaquete> MovimientosPaquetes { get; set; } = new List<MovimientosPaquete>();
}

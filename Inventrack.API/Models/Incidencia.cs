using System;
using System.Collections.Generic;

namespace Inventrack.API.Models;

public partial class Incidencia
{
    public int IncidenciaId { get; set; }

    public int PaqueteId { get; set; }

    public int UsuarioId { get; set; }

    public string Descripcion { get; set; } = null!;

    public DateTime? Fecha { get; set; }

    public bool? Resuelta { get; set; }

    public virtual Paquete Paquete { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}

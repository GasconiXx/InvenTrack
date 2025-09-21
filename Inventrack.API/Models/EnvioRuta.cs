using System;
using System.Collections.Generic;

namespace Inventrack.API.Models;

public partial class EnvioRuta
{
    public int EnvioRutaId { get; set; }

    public int EnvioId { get; set; }

    public int RutaId { get; set; }

    public virtual Envio Envio { get; set; } = null!;

    public virtual Ruta Ruta { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace Inventrack.API.Models;

public partial class Ruta
{
    public int RutaId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<EnvioRuta> EnvioRuta { get; set; } = new List<EnvioRuta>();
}

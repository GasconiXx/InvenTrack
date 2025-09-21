using System;
using System.Collections.Generic;

namespace Inventrack.API.Models;

public partial class Envio
{
    public int EnvioId { get; set; }

    public int PaqueteId { get; set; }

    public int RepartidorId { get; set; }

    public DateTime? FechaSalida { get; set; }

    public DateTime? FechaEntrega { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<EnvioRuta> EnvioRuta { get; set; } = new List<EnvioRuta>();

    public virtual Paquete Paquete { get; set; } = null!;

    public virtual Usuario Repartidor { get; set; } = null!;
}

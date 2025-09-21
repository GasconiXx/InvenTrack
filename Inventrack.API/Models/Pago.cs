using System;
using System.Collections.Generic;

namespace Inventrack.API.Models;

public partial class Pago
{
    public int PagoId { get; set; }

    public int ClienteId { get; set; }

    public int PaqueteId { get; set; }

    public decimal Monto { get; set; }

    public string MetodoPago { get; set; } = null!;

    public DateTime? FechaPago { get; set; }

    public virtual Usuario Cliente { get; set; } = null!;

    public virtual Paquete Paquete { get; set; } = null!;
}

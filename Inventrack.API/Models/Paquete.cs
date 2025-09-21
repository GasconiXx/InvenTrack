using System;
using System.Collections.Generic;

namespace Inventrack.API.Models;

public partial class Paquete
{
    public int PaqueteId { get; set; }

    public int ClienteId { get; set; }

    public string CodigoSeguimiento { get; set; } = null!;

    public decimal Peso { get; set; }

    public string? Dimensiones { get; set; }

    public string? Descripcion { get; set; }

    public string Estado { get; set; } = null!;

    public DateTime? FechaRegistro { get; set; }

    public virtual Usuario Cliente { get; set; } = null!;

    public virtual ICollection<Envio> Envios { get; set; } = new List<Envio>();

    public virtual ICollection<Incidencia> Incidencia { get; set; } = new List<Incidencia>();

    public virtual ICollection<MovimientosPaquete> MovimientosPaquetes { get; set; } = new List<MovimientosPaquete>();

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}

using System;
using System.Collections.Generic;

namespace Inventrack.API.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string ContraseñaHash { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<Envio> Envios { get; set; } = new List<Envio>();

    public virtual ICollection<Incidencia> Incidencia { get; set; } = new List<Incidencia>();

    public virtual ICollection<MovimientosPaquete> MovimientosPaquetes { get; set; } = new List<MovimientosPaquete>();

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual ICollection<Paquete> Paquetes { get; set; } = new List<Paquete>();
}

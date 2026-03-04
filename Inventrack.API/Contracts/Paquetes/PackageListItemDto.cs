namespace Inventrack.API.Contracts.Paquetes
{
    public sealed class PackageListItemDto
    {
        public int PaqueteId { get; set; }
        public string CodigoSeguimiento { get; set; } = "";
        public string Codigo { get; set; } = ""; // Propiedad para compatibilidad
        public string Estado { get; set; } = "";
        public string DireccionDestino { get; set; } = "";
        public string Destinatario { get; set; } = "";
        public string Remitente { get; set; } = "";
        public string Almacen { get; set; } = "";
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaEntrega { get; set; }
    }
}

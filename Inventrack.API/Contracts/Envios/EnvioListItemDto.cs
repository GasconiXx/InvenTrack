namespace Inventrack.API.Contracts.Envios
{
    public sealed class EnvioListItemDto
    {
        public int EnvioId { get; set; }
        public int PaqueteId { get; set; }
        public string CodigoSeguimiento { get; set; } = "";
        public string Estado { get; set; } = "";
        public int? IntentosEntrega { get; set; }
        public DateTime? FechaSalida { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public string DireccionDestino { get; set; } = "";
    }
}

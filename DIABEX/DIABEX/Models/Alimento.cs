namespace DIABEX.Models;

public class Alimento
{
    public string Nombre { get; set; } = string.Empty;
    public double CarbosPor100g { get; set; }
}

public class ItemPlato
{
    public string Nombre { get; set; } = string.Empty;
    public string DetalleGramaje { get; set; } = string.Empty;
    public string CarbosTotalesText { get; set; } = string.Empty;
    public double CarbosNumericos { get; set; }
}
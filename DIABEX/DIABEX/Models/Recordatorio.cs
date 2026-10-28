using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DIABEX.Models;

[Table("recordatorios")]
public class Recordatorio : BaseModel
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }

    [Column("user_id")]
    public string UserId { get; set; }

    [Column("titulo")]
    public string Titulo { get; set; }

    [Column("hora")]
    public string Hora { get; set; }

    [Column("activo")]
    public bool Activo { get; set; } = true;
}
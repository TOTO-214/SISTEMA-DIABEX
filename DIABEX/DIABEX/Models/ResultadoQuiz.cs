using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DIABEX.Models;

[Table("resultados_quiz")]
public class ResultadoQuiz : BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }

    [Column("user_id")]
    public string UserId { get; set; }

    [Column("fecha")]
    public DateTime Fecha { get; set; }

    [Column("puntaje")]
    public int Puntaje { get; set; }

    [Column("resultado_texto")]
    public string ResultadoTexto { get; set; }
}
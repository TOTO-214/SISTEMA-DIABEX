using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DIABEX.Models
{
    [Table("usuarios")]
    public class Usuario : BaseModel
    {
        [PrimaryKey("id_usuario", false)]
        [Column("id_usuario", ignoreOnInsert: true)] public long IdUsuario { get; set; }

        [Column("nombre_completo")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Column("correo")]
        public string Correo { get; set; } = string.Empty;

        [Column("contrasena")]
        public string Contrasena { get; set; } = string.Empty;

        [Column("rol")]
        public string Rol { get; set; } = "usuario";
    }
}
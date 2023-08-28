using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla_API.Modelos
{
    public class NumeroVilla
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NumVilla { get; set; }

        [Required]
        public int VillaId  { get; set; }

        [ForeignKey("VillaId")]
        public  Villa Villa { get; set; }

        public string DetalleEspecial { get; set; }

        public DateTime FechaCrecion { get; set; }= DateTime.Now;

        public DateTime FechaActualizacion{ get; set; }
    }
}

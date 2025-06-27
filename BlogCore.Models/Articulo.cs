using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class Articulo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre del articulo")]
        public string Nombre { get; set; }

        [Display(Name = "Orden de Visualizacion")]
        [Range(1, 100, ErrorMessage = "El orden debe estar entre 1 y 100")]
        public int? Orden { get; set; }
        
    }
}
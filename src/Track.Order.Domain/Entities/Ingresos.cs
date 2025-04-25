using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Track.Order.Domain.Entities
{
    public class Ingresos
    {

        [Key]
        public int IDIngreso { get; set; }
        public decimal Monto { get; set; }
        public DateTime? Fecha { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int IDPresupuesto { get; set; }
        public Presupuesto? Presupuesto { get; set; }
    }
}
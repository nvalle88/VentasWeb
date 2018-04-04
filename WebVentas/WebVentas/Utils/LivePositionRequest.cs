using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebVentas.Utils
{
   public class LivePositionRequest
    {
        public int EmpresaId { get; set; }
        public int AgenteId { get; set; }
        public string Nombre { get; set; }
        public float Lat { get; set; }
        public float Lon { get; set; }
        public DateTime fecha { get; set; }

    }
}

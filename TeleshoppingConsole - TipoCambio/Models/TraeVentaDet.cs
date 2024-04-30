using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleshoppingConsole.Models
{
    public class TraeVentaDet
    {
        public decimal NumPedido { get; set; }
        public int Renglon { get; set; }
        public string CodigoPro { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public double Precio { get; set; }
        public double PrecioSinIVA { get; set; }
        public string linea { get; set; }
        public string Talla { get; set; }
        public string Color { get; set; }
        public string Clas { get; set; }
        public string ListaPrecio { get; set; }
        public string TipoMoneda { get; set; }
    }
}

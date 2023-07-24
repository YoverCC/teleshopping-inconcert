using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleshoppingConsole.Models
{
    public class TraeVentasPago
    {
        public DateTime FechaGestion { get; set; }
        public string FechaEntrega { get; set; }
        public Decimal NumPedido { get; set; }
        public string FormaPago { get; set; }
        public double MontoTotal { get; set; }
        public string TipoTDC { get; set; }
        public string TDC { get; set; }
        public string Venc { get; set; }
        public string Autoriza { get; set; }
        public string Cuotas { get; set; }
        public string Banco { get; set; }
        public string MonedaFactura { get; set; }
        public string MonedaPag { get; set; }
        public double TipoCambio { get; set; }
        public string LineaPago { get; set; }
        public override string ToString()
        {
            return this.MonedaPag;
        }
    }
}

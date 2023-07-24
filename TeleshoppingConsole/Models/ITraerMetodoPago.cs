using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleshoppingConsole.Models
{
    public interface ITraerMetodoPago
    {
        public List<TraeVentasPago> TraeMetodoPago(Decimal numPedido);
    }
}

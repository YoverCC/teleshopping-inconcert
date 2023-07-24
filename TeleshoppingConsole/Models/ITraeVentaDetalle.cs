using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleshoppingConsole.Models
{
    public interface ITraeVentaDetalle
    {
        public List<TraeVentaDet> TraeVentaDetalles(string idCall);
    }
}

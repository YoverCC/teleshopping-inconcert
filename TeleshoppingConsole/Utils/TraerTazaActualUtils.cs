using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleshoppingConsole.Models;

namespace TeleshoppingConsole.Utils
{
    public class TraerTazaActualUtils : ITraerTazaActual
    {
        private readonly ITraerTazaActual traerTazaActual;
        public TraerTazaActualUtils()
        {
            traerTazaActual = new BitServiceSOAP();
        }

        public double ObtenerLaTazaActualDeDolaresXPesos()
        {
            var tazaActual = traerTazaActual.ObtenerLaTazaActualDeDolaresXPesos();
            if (tazaActual != 0)
            {
                ObtenerDatosDelJson.SerializarJson(new DatosUtiles(tazaActual));
                return tazaActual;
            }
            return ObtenerDatosDelJson.ObtenerLaUltimaTaza();
        }
    }
}

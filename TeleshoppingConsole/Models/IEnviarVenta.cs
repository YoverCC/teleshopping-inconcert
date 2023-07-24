using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleshoppingConsole.Models
{
    public interface IEnviarVenta
    {
        public void EnviarVentas(DateTime desde, DateTime hasta);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleshoppingConsole.Models
{
    public interface IAgregarCodigoBitClientes
    {
        public Boolean AgregarCodigoBitClientes(string IdCliente, string CodigoBit);
    }
}

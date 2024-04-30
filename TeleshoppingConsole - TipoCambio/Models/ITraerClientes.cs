using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleshoppingConsole.Models
{
    public interface ITraerClientes
    {
        public List<Cliente> TraerClientes(DateTime desde, DateTime hasta);
        public Cliente TraerClientePorId(string IDCliente);
    }
}

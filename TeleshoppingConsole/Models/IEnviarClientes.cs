using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleshoppingConsole.Models
{
    public interface IEnviarClientes
    {
        public void EnviarClientes(List<Cliente> clientes);
        public string EnviarCliente(Cliente cliente);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleshoppingConsoleArticulo.Models
{
    public interface IObtenerArticulos
    {
        public List<Articulo> GetArticulos();
    }
}

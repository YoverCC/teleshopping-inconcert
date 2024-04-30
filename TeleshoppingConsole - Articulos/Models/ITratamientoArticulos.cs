using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleshoppingConsoleArticulo.Models
{
    public interface ITratamientoArticulos
    {
        public void TratarArticulo(List<Articulo> articulos);
        public void PreTratarArticulo(string guid);

        public void PostTratarArticulo(string guid);
    }
}

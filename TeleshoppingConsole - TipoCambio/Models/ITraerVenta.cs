﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleshoppingConsole.Models
{
    public interface ITraerVenta
    {
        public List<TraeVenta> TraeVentas(DateTime desde, DateTime hasta);
    }
}

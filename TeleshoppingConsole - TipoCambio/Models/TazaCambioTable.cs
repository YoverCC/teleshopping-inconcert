using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TeleshoppingConsole.Models
{
    [XmlRoot(ElementName = "Table")]
    public class TazaCambioTable
    {
        [XmlElement(ElementName = "fecha")]
        public string Fecha { get; set; }

        [XmlElement(ElementName = "tipoCambio")]
        public double TipoCambio { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TeleshoppingConsole.Models
{
    [XmlRoot(ElementName = "PAGO")]
    public class Pago
    {
        [XmlElement(ElementName = "ENTIDAD")]
        public string Entidad { get; set; }
        [XmlElement(ElementName = "CUOTAS")]
        public string Cuotas { get; set; }
        [XmlElement(ElementName = "AUTORIZNUM")]
        public string AutorizNum { get; set; }
        [XmlElement(ElementName = "MONTO")]
        public double Monto { get; set; }
        [XmlElement(ElementName = "TARJETA")]
        public string Tarjeta { get; set; }
    }
}

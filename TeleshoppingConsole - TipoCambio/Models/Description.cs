using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TeleshoppingConsole.Models
{
    [XmlRoot(ElementName = "DOCUMENTO")]
    public class Description
    {
        [XmlIgnore]
        public string IdCliente { get; set; }
        [XmlIgnore]
        public string IdCall { get; set; }
        [XmlIgnore]
        public string NrPedido { get; set; }

        [XmlElement(ElementName = "CABEZAL")]
        public Cabezal Cabezal { get; set; }
        [XmlElement(ElementName = "DETALLE")]
        public List<Detalle> Detalles { get; set; }
        [XmlElement(ElementName = "PAGO")]
        public List<Pago> Pago { get; set; }
        public Description() { }
    }
}

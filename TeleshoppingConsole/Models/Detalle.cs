using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TeleshoppingConsole.Models
{
    [XmlRoot(ElementName = "DETALLE")]
    public class Detalle
    {
        [XmlElement(ElementName = "ARTICULO")]
        public string Articulo { get; set; }
        [XmlElement(ElementName = "LISTAPRECIO")]
        public string ListaPrecio { get; set; }
        
        [XmlElement(ElementName = "SUBART1")]
        public string Talle { get; set; }
        [XmlElement(ElementName = "SUBART2")]
        public string Color { get; set; }
        [XmlElement(ElementName = "CANTIDAD")]
        public string Cantidad { get; set; }
        [XmlElement(ElementName = "PRECIO")]
        public double Precio { get; set; }
        public Detalle() { }
    }
}

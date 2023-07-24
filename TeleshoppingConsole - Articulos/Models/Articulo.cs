using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TeleshoppingConsoleArticulo.Models
{

    [Serializable, XmlRoot(ElementName = "Table", IsNullable = true),]

    public class Articulo
    {
        [XmlElement(ElementName = "codigo")]
        public string Codigo { get; set; }
        [XmlElement(ElementName = "descripcion")]
        public string Descripcion { get; set; }
        [XmlElement(ElementName = "estado")]
        public int Estado { get; set; }
        [XmlElement(ElementName = "talle")]
        public string Talle { get; set; }
        [XmlElement(ElementName = "talleDescr")]
        public string TalleDescripcion { get; set; }
        [XmlElement(ElementName = "color")]
        public string Color { get; set; }
        [XmlElement(ElementName = "colorDescr")]
        public string ColorDescripcion { get; set; }
        [XmlElement(ElementName = "familia")]
        public string Familia { get; set; }
        [XmlElement(ElementName = "comentarios")]
        public object Comentarios { get; set; }
        [XmlElement(ElementName = "stock")]
        public sbyte Stock { get; set; }
        [XmlElement(ElementName = "PRECIO")]
        public decimal Precio { get; set; }
        [XmlElement(ElementName = "CallBackS")]
        public decimal CallBackS { get; set; }
        [XmlElement(ElementName = "ESPECIAL")]
        public decimal Especial { get; set; }
        [XmlElement(ElementName = "REGALO")]
        public decimal Regalo { get; set; }
        [XmlElement(ElementName = "ENTRANTE")]
        public decimal Entrante { get; set; }
        [XmlElement(ElementName = "SALIENTE")]
        public decimal Saliente { get; set; }
        [XmlElement(ElementName = "PRECIOUSD")]
        public decimal PrecioUSD { get; set; }
    }

}

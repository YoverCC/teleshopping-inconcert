using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TeleshoppingConsole.Models
{
    [XmlRoot(ElementName = "CABEZAL")]
    public class Cabezal
    {
        [XmlElement(ElementName = "CLIENTE")]
        public string Cliente { get; set; }

        [XmlElement(ElementName = "MODELO")]
        public string Modelo { get; set; }
        [XmlElement(ElementName = "FECHA")]
        public string Fecha { get; set; }
        //[XmlElement(ElementName = "NUMERO")]
        [XmlIgnore]
        public string Numero { get; set; }

        [XmlElement(ElementName = "LISTAPRECIO")]
        public string ListaPrecio { get; set; }

        [XmlElement(ElementName = "CLINOMBRE")]
        public string ClienteNombre { get; set; }
        [XmlElement(ElementName = "CLIRAZON")]
        public string ClienteRazon { get; set; }
        [XmlElement(ElementName = "CLIDEPTO")]
        public string ClienteDepartamento { get; set; }
        [XmlElement(ElementName = "CLICIUDAD")]
        public string ClienteCiudad { get; set; }
        [XmlElement(ElementName = "CLIDIR")]
        public string ClienteDirection { get; set; }
        [XmlElement(ElementName = "CONTACTO")]
        public string Contacto { get; set; }
        [XmlElement(ElementName = "COMENTARIO")]
        public string Comentario { get; set; }

        [XmlElement(ElementName = "FECHAREQ")]
        public string FechaReq { get; set; }

        [XmlElement(ElementName = "MENSAJE")]
        public string Mensaje { get; set; }

        [XmlElement(ElementName = "REFERENCIA")]
        public string Referencia { get; set; }

        [XmlElement(ElementName = "USUARIO")]
        public string Usuario { get; set; }

        public Cabezal() { }
    }
}

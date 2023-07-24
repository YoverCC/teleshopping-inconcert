using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TeleshoppingConsole.Models
{
    [Serializable, XmlRoot(ElementName = "params")]
    public class Cliente
    {
        [XmlIgnore]
        public string IdCliente { get; set; }
        [XmlElement]
        public string Doc { get; set; }
        [XmlElement]
        public string Nombres { get; set; }
        [XmlElement]
        public string Apellidos { get; set; }
        [XmlElement]
        public string FechaNac { get; set; }
        [XmlElement]
        public string Telefono { get; set; }
        [XmlElement]
        public string Celular { get; set; }
        [XmlIgnore]
        public string DireccionData { get; set; }
        [XmlElement]
        public System.Xml.XmlCDataSection Direccion { get { return new System.Xml.XmlDocument().CreateCDataSection(DireccionData); } set { DireccionData = value.Value; } }
        [XmlElement]
        public string NroPuerta { get; set; }
        [XmlElement]
        public string BisPiso { get; set; }
        [XmlElement]
        public string Apto { get; set; }
        [XmlElement]
        public string Entre1 { get; set; }
        [XmlElement]
        public string Entre2 { get; set; }
        [XmlElement]
        public string Ciudad { get; set; }
        [XmlElement]
        public string Barrio { get; set; }
        [XmlElement]
        public string Mail { get; set; }
        [XmlElement]
        public string Justificacion { get; set; }
        [XmlElement]
        public string RazonSocial { get; set; }
        [XmlElement]
        public string Campana { get; set; }
        [XmlElement]
        public string Agente { get; set; }
        [XmlElement]
        public string NroLlamada { get; set; }
        [XmlElement]
        public string Resultado { get; set; }

        public override string ToString()
        {
            return $"IdCliente: {this.IdCliente} {this.Resultado}";
        }
    }
}

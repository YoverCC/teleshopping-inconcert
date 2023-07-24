using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TeleshoppingConsole.Models
{
    [XmlRoot(ElementName = "ERROR")]
    public class ERROR
    {

        [XmlElement(ElementName = "CODE")]
        public int CODE { get; set; }

        [XmlElement(ElementName = "POS")]
        public string POS { get; set; }

        [XmlElement(ElementName = "DESC")]
        public string DESC { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TeleshoppingConsole.Models
{
    [XmlRoot(ElementName = "DOCUMENTO")]
    public class DOCUMENTO
    {

        [XmlElement(ElementName = "NRODOC")]
        public int NRODOC { get; set; }

        [XmlElement(ElementName = "NROORDEN")]
        public string NROORDEN { get; set; }

        [XmlElement(ElementName = "ERROR")]
        public ERROR ERROR { get; set; }
    }
}


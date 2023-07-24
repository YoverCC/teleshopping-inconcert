using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TeleshoppingConsole.Models
{
	[XmlRoot(ElementName = "Table")]
	public class ClienteTable
	{

		[XmlElement(ElementName = "ClienteCreado")]
		public string ClienteCreado { get; set; }
	}
}

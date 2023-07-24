using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TeleshoppingConsole.Models
{
	[XmlRoot(ElementName = "NewDataSet")]
	public class ClienteNewDataSet
	{

		[XmlElement(ElementName = "Table")]
		public ClienteTable Table { get; set; }
	}
}

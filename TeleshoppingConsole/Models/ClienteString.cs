using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TeleshoppingConsole.Models
{
	[XmlRoot(ElementName = "string")]
	public class ClienteString
	{

		[XmlElement(ElementName = "NewDataSet")]
		public ClienteNewDataSet NewDataSet { get; set; }

		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }

		[XmlText]
		public string Text { get; set; }
	}
}

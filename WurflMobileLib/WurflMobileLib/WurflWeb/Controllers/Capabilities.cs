using System.Collections.Generic;

namespace WurflWeb.Controllers
{
	public class Capabilities
	{
		public List<string> CapabilitiyNames { get; set; }
		public Capabilities()
		{
			CapabilitiyNames = new List<string>();
		}
	}
}
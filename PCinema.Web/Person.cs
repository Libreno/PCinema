using System;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PCinema.Web
{
	public class Person {
		public string Name { get; set; }
		public string Sex { get; set; }
		public string Bio { get; set; }
		public string Children { get; set; }
		public DateTimeOffset BirthDate { get; set; }
		public IEnumerable<string> Professions { get; set; }
	}
}

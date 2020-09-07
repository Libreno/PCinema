using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PCinema.Web
{
	[ModelBinder(BinderType = typeof(PersonEntityBinder))]
	public class Person {
		public string FullText { get; set; }
	}
}

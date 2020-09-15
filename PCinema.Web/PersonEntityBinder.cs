using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.IO;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PCinema.Web
{
	public class PersonEntityBinder : IModelBinder
	{
		public async Task BindModelAsync(ModelBindingContext bindingContext)
		{
			var request = bindingContext.HttpContext.Request;
			using var ms = new MemoryStream();
			await request.Body.CopyToAsync(ms);
			var model = new Person() { FullText = Encoding.UTF8.GetString(ms.ToArray()) };
			bindingContext.Result = ModelBindingResult.Success(model);
		}
	}
}

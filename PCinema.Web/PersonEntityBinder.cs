using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.IO;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PCinema.Web
{
	public class PersonEntityBinder : IModelBinder
	{
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			var request = bindingContext.HttpContext.Request;
			var ms = new MemoryStream();
			request.Body.CopyToAsync(ms).Wait();
			var model = new Person() { FullText = Encoding.UTF8.GetString(ms.ToArray()) };
			bindingContext.Result = ModelBindingResult.Success(model);
			return Task.CompletedTask;
		}
	}
}

using System.Threading.Tasks;
using System.Web.Http;
using DemoEfWebApi.Models.Dto;
using DemoEfWebApi.Security;
using DemoEfWebApi.Services.Interfaces;

namespace DemoEfWebApi.Controllers
{
    [RoutePrefix("api/products")]
    [TokenAuthentication] // require token for all actions in this controller
    public class ProductsController : ApiController
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            var p = await _service.GetByIdAsync(id);
            if (p == null) return NotFound();
            return Ok(p);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var ok = await _service.DeleteByIdAsync(id);
            if (!ok) return NotFound();
            return Ok(new { message = "Deleted" });
        }
    }
}

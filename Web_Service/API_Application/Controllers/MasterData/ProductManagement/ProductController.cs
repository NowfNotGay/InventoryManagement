using Base.BaseService;
using Base.ProductManagement;
using Core.ProductManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Application.Controllers.ProductManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductProvider _productProvider;
        private readonly ICRUD_Service<Product, int> _productService;

        public ProductController(IProductProvider productProvider, ICRUD_Service<Product, int> productService)
        {
            _productProvider = productProvider;
            _productService = productService;
        }

        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAll()
        {
            var rs = await _productService.GetAll();
            return rs.Code == "0" ? Ok(rs) : BadRequest(rs);
        }

        [HttpGet("GetByID/{ID}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GetByID(int ID)
        {
            var rs = await _productService.Get(ID);
            return rs.Code == "0" ? Ok(rs) : BadRequest(rs);
        }

        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Route("GetAllParam")]
        public async Task<IActionResult> GetAllParam()
        {
            var rs = await _productProvider.GetAllProductParam();
            return rs.Code == "0" ? Ok(rs) : BadRequest(rs);
        }

        [HttpGet("GetByIDParam/{ID}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        
        public async Task<IActionResult> GetByIDParam(int ID)
        {
            var rs = await _productProvider.GetByIDParam(ID);
            return rs.Code == "0" ? Ok(rs) : BadRequest(rs);
        }

        [HttpPost("Save")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Save([FromBody] Product product)
        {
            var rs = await _productProvider.SaveByDapper(product);
            return rs.Code == "0" ? Ok(rs) : BadRequest(rs);
        }

        [HttpDelete("DeleteByDapper/{code}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteByDapper(string code)
        {
            var rs = await _productProvider.DeleteByDapper(code);
            return rs.Code == "0" ? Ok(rs) : BadRequest(rs);
        }
    }
}

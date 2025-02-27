using Base.BaseService;
using Base.ProductManagement;
using Core.ProductManagement;
using Microsoft.AspNetCore.Mvc;

namespace API_Application.Controllers.ProductManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductVarientController : ControllerBase
    {
        private readonly IProductVariantProvider _productVariantProvider;
        private readonly ICRUD_Service<ProductVariant, int> _ICRUD_Service;

        public ProductVarientController(IProductVariantProvider productVariantProvider, ICRUD_Service<ProductVariant, int> iCRUD_Service)
        {
            _productVariantProvider = productVariantProvider;
            _ICRUD_Service = iCRUD_Service;
        }

        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAll()
        {
            var rs = await _ICRUD_Service.GetAll();
            return rs.Code == "0" ? Ok(rs) : BadRequest(rs);

        }

        [HttpGet("ID")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GetByID([FromQuery] int ID)
        {
            var rs = await _ICRUD_Service.Get(ID);
            return rs.Code == "0" ? Ok(rs) : BadRequest(rs);

        }


        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> Save([FromBody] ProductVariant productVariant)
        {
            var rs = await _ICRUD_Service.Create(productVariant);
            return rs.Code == "0" ? Ok(rs) : BadRequest(rs);
        }

        [HttpPut]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> Update([FromBody] ProductVariant productVariant)
        {
            var rs = await _ICRUD_Service.Update(productVariant);
            return rs.Code == "0" ? Ok(rs) : BadRequest(rs);

        }
        [HttpDelete]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> Delete(int id)
        {
            var rs = await _ICRUD_Service.Delete(id);
            return rs.Code == "0" ? Ok(rs) : BadRequest(rs);

        }
    }
}

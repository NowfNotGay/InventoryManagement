using Base.BaseService;
using Base.ProductManagement;
using Core.ProductManagement;
using Microsoft.AspNetCore.Mvc;

namespace API_Application.Controllers.ProductManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAttributeController : ControllerBase
    {
        private readonly IProductAttributeProvider _ProductAttributeProvider;
        private readonly ICRUD_Service<ProductAttribute, int> _ICRUD_Service;

        public ProductAttributeController(IProductAttributeProvider ProductAttributeProvider, ICRUD_Service<ProductAttribute, int> iCRUD_Service)
        {
            _ProductAttributeProvider = ProductAttributeProvider;
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

        public async Task<IActionResult> Save([FromBody] ProductAttribute ProductAttribute)
        {
            var rs = await _ICRUD_Service.Create(ProductAttribute);
            return rs.Code == "0" ? Ok(rs) : BadRequest(rs);
        }

        [HttpPut]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> Update([FromBody] ProductAttribute ProductAttribute)
        {
            var rs = await _ICRUD_Service.Update(ProductAttribute);
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

using Base.BaseService;
using Base.ProductManagement;
using Core.BaseClass;
using Core.ProductManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Application.Controllers.WarehouseManagement;

[Route("api/[controller]")]
[ApiController]
public class ProductUoMConversionController : ControllerBase
{
    private readonly ICRUD_Service<ProductUoMConversion, int> _ICRUD_Service;
    private readonly IProductUoMConversionProvider _productUoMConversionProvider;

    public ProductUoMConversionController(
        ICRUD_Service<ProductUoMConversion, int> iCRUD_Service,
        IProductUoMConversionProvider productUoMConversionProvider)
    {
        _ICRUD_Service = iCRUD_Service;
        _productUoMConversionProvider = productUoMConversionProvider;
    }

    #region Normal CRUD

    [HttpGet]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetAll()
    {
        var rs = await _ICRUD_Service.GetAll();
        return rs == null ? BadRequest("No ProductUoMConversions found") : Ok(rs);
    }

    [HttpGet("{id}")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetById(int id)
    {
        var rs = await _ICRUD_Service.Get(id);
        return rs == null ? NotFound($"ProductUoMConversion with ID {id} not found") : Ok(rs);
    }

    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Create([FromBody] ProductUoMConversion productUoMConversion)
    {
        var rs = await _ICRUD_Service.Create(productUoMConversion);
        return rs == null ? BadRequest("Failed to create ProductUoMConversion") : Ok(rs);
    }

    [HttpPut]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Update([FromBody] ProductUoMConversion productUoMConversion)
    {
        var rs = await _ICRUD_Service.Update(productUoMConversion);
        return rs == null ? BadRequest("Failed to update ProductUoMConversion") : Ok(rs);
    }

    [HttpDelete]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Delete(int id)
    {
        var rs = await _ICRUD_Service.Delete(id);
        return rs == null ? BadRequest("Failed to delete ProductUoMConversion") : Ok(rs);
    }

    #endregion

    #region Dapper CRUD

    [HttpGet("productUoMConversionCode/{productUoMConversionCode}")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetByCode(string productUoMConversionCode)
    {
        var rs = await _productUoMConversionProvider.GetByCode(productUoMConversionCode);
        return rs.Code == "0" ? Ok(rs) : NotFound($"ProductUoMConversion with Code {productUoMConversionCode} not found");
    }

    [HttpPost("SaveByDapper")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> SaveByDapper([FromBody] ProductUoMConversion productUoMConversion)
    {
        var rs = await _productUoMConversionProvider.SaveByDapper(productUoMConversion);
        return rs.Code == "0" ? Ok(rs.Message) : BadRequest(rs.Message);
    }

    [HttpDelete("DeleteByDapper")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> DeleteByDapper(string productUoMConversionCode)
    {
        var rs = await _productUoMConversionProvider.DeleteByDapper(productUoMConversionCode);
        return rs.Code == "0" ? Ok(rs.Message) : BadRequest(rs.Message);
    }

    #endregion
}
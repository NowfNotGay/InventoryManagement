using Base.BaseService;
using Base.ProductManagement;
using Core.ProductManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Servicer.MasterData.ProductManagement;

namespace API_Application.Controllers.MasterData.ProductManagement;
[Route("api/Conversion")]
[ApiController]
public class ProductUoMConversionController : ControllerBase
{
    private readonly IProductUoMConversionProvider _provider;
    private readonly ICRUD_Service<ProductUoMConversion, int> _ICRUD_Service;

    public ProductUoMConversionController(IProductUoMConversionProvider provider, ICRUD_Service<ProductUoMConversion, int> service)
    {
        _provider = provider;
        _ICRUD_Service = service;
    }


    [HttpGet]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetAll()
    {
        var rs = await _provider.GetAllDapper();
        return rs.Code == "0" ? Ok(rs) : BadRequest(rs);

    }

    [HttpGet("GetByCode/{code}")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetByCode(string code)
    {
        var rs = await _provider.GetByCode(code);
        return rs.Code == "0" ? Ok(rs) : BadRequest(rs);
    }

    [HttpPost("Save")]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> Save([FromBody] ProductUoMConversion productUoMConversion)
    {
        var rs = await _provider.SaveByDapper(productUoMConversion);
        return rs.Code == "0" ? Ok(rs) : BadRequest(rs);
    }

    [HttpDelete("{code}")]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> Delete(string code)
    {
        var rs = await _provider.DeleteByDapper(code);
        return rs.Code == "0" ? Ok(rs) : BadRequest(rs);

    }


}

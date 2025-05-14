using Base.BaseService;
using Base.MasterData.ProductClassification;
using Core.MasterData;
using Core.MasterData.ProductClassification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Application.Controllers.MasterData.ProductClassification;
[Route("api/[controller]")]
[ApiController]
public class ProductTypeController : ControllerBase
{
    private readonly ICRUD_Service<ProductType, int> _productTypeService;

    private readonly IProductTypeProvider _productTypeProvider;

    public ProductTypeController(ICRUD_Service<ProductType, int> productTypeService, IProductTypeProvider productTypeProvider)
    {
        _productTypeService = productTypeService;
        _productTypeProvider = productTypeProvider;
    }

    [HttpGet]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> GetAll()
    {
        var rs = await _productTypeService.GetAll();
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);

    }

    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> Save([FromBody] ProductType productType)
    {
        var rs = await _productTypeService.Create(productType);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);

    }

    [HttpPut]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> Update([FromBody] ProductType productType)
    {
        var rs = await _productTypeService.Update(productType);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);

    }

    [HttpDelete]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> Delete(int id)
    {
        var rs = await _productTypeService.Delete(id);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);

    }



    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("SaveByDapper")]
    public async Task<IActionResult> SaveByDapper([FromBody] ProductType productType)
    {
        var rs = await _productTypeProvider.SaveByDapper(productType);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);

    }

    [HttpDelete]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("DeleteByDapper/{productTypeCode}")]
    public async Task<IActionResult> DeleteByDapper(string productTypeCode)
    {
        var rs = await _productTypeProvider.DeleteByDapper(productTypeCode);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);

    }

}

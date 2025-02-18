using Base.BaseService;
using Base.ProductClassification;
using Base.ProductProperties;
using Core.MasterData;
using Core.ProductClassification;
using Core.ProductProperties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Application.Controllers.ProductProperties;
[Route("api/[controller]")]
[ApiController]
public class ProductCategoryController : ControllerBase
{
    private readonly ICRUD_Service<ProductCategory, int> _productCategoryService;

    private readonly IProductCategoryProvider _productCategoryProvider;

    public ProductCategoryController(ICRUD_Service<ProductCategory, int> productCategoryService, IProductCategoryProvider productCategoryProvider)
    {
        _productCategoryService = productCategoryService;
        _productCategoryProvider = productCategoryProvider;
    }

    [HttpGet]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> GetAll()
    {
        var rs = await _productCategoryService.GetAll();
        return rs == null ? BadRequest() : Ok(rs);

    }

    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> Save([FromBody] ProductCategory productCategory)
    {
        var rs = await _productCategoryService.Create(productCategory);
        return rs == null ? BadRequest() : Ok(rs);

    }

    [HttpPut]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> Update([FromBody] ProductCategory productCategory)
    {
        var rs = await _productCategoryService.Update(productCategory);
        return rs == null ? BadRequest() : Ok(rs);

    }

    [HttpDelete]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> Delete(int id)
    {
        var rs = await _productCategoryService.Delete(id);
        return rs == null ? BadRequest() : Ok(rs);

    }

}

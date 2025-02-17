﻿using Base.BaseService;
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
        return rs == null ? BadRequest() : Ok(rs);

    }

    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> Save([FromBody] ProductType productType)
    {
        var rs = await _productTypeService.Create(productType);
        return rs == null ? BadRequest() : Ok(rs);

    }

    [HttpPut]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> Update([FromBody] ProductType productType)
    {
        var rs = await _productTypeService.Update(productType);
        return rs == null ? BadRequest() : Ok(rs);

    }

    [HttpDelete]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> Delete(int id)
    {
        var rs = await _productTypeService.Delete(id);
        return rs == null ? BadRequest() : Ok(rs);

    }

}

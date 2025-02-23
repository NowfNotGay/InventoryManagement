using Base.BaseService;
using Base.ProductClassification;
using Base.ProductProperties;
using Core.MasterData;
using Core.ProductClassification;
using Core.ProductProperties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Application.Controllers.ProductClassification;
[Route("api/[controller]")]
[ApiController]
public class BrandController : ControllerBase
{
    private readonly ICRUD_Service<Brand, int> _brandService;
    private readonly IBrandProvider _brandProvider;

    public BrandController(ICRUD_Service<Brand, int> brandService, IBrandProvider brandProvider)
    {
        _brandService = brandService;
        _brandProvider = brandProvider;
    }

    [HttpGet]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetAll()
    {
        var rs = await _brandService.GetAll();
        return rs == null ? BadRequest("No brands found") : Ok(rs);
    }

    [HttpGet("{id}")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetById(int id)
    {
        var rs = await _brandService.Get(id);
        return rs == null ? NotFound($"Brand with ID {id} not found") : Ok(rs);
    }

    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Create([FromBody] Brand brand)
    {
        var rs = await _brandService.Create(brand);
        return rs == null ? BadRequest("Failed to create brand") : Ok(rs);
    }

    [HttpPut]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Update([FromBody] Brand brand)
    {
        var rs = await _brandService.Update(brand);
        return rs == null ? BadRequest("Failed to update brand") : Ok(rs);
    }

    [HttpDelete]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Delete(int id)
    {
        var rs = await _brandService.Delete(id);
        return rs == null ? BadRequest("Failed to delete brand") : Ok(rs);
    }
}

using Base.BaseService;
using Base.ProductProperties;
using Core.ProductProperties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Application.Controllers.ProductProperties;
[Route("api/[controller]")]
[ApiController]
public class DimensionController : ControllerBase
{
    private readonly ICRUD_Service<Dimension, int> _dimensionService;
    private readonly IDimensionProvider _dimensionProvider;

    public DimensionController(ICRUD_Service<Dimension, int> dimensionService, IDimensionProvider dimensionProvider)
    {
        _dimensionService = dimensionService;
        _dimensionProvider = dimensionProvider;
    }

    [HttpGet]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> GetAll()
    {
        var rs = await _dimensionService.GetAll();
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);

    }

    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> Save([FromBody] Dimension dimension)
    {
        var rs = await _dimensionService.Create(dimension);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);

    }

    [HttpPut]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> Update([FromBody] Dimension dimension)
    {
        var rs = await _dimensionService.Update(dimension);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);

    }

    [HttpDelete]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> Delete(int id)
    {
        var rs = await _dimensionService.Delete(id);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);

    }
}

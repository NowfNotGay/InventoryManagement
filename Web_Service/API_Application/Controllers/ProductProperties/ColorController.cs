using Base.BaseService;
using Base.ProductProperties;
using Core.MasterData;
using Core.ProductProperties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Application.Controllers.ProductProperties;
[Route("api/[controller]")]
[ApiController]
public class ColorController : ControllerBase
{
    private readonly ICRUD_Service<Color, int> _colorService;
    private readonly IColorProvider _colorProvider;

    public ColorController(ICRUD_Service<Color, int> colorService, IColorProvider colorProvider)
    {
        _colorService = colorService;
        _colorProvider = colorProvider;
    }

    [HttpGet]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> GetAll()
    {
        var rs = await _colorService.GetAll();
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);
        
    }

    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> Save([FromBody] Color color)
    {
        var rs = await _colorService.Create(color);
        return rs == null ? BadRequest(rs) : Ok(rs);

    }

    [HttpPut]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> Update([FromBody] Color color)
    {
        var rs = await _colorService.Update(color);
        return rs == null ? BadRequest() : Ok(rs);

    }

    [HttpDelete]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> Delete(int id)
    {
        var rs = await _colorService.Delete(id);
        return rs == null ? BadRequest() : Ok(rs);

    }

}

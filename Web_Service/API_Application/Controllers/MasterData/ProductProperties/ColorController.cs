using Base.BaseService;
using Base.MasterData.ProductProperties;
using Core.MasterData;
using Core.MasterData.ProductProperties;
using Core.ProductClassification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Application.Controllers.MasterData.ProductProperties;
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
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);

    }

    [HttpPut]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> Update([FromBody] Color color)
    {
        var rs = await _colorService.Update(color);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);

    }

    [HttpDelete]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> Delete(int id)
    {
        var rs = await _colorService.Delete(id);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);

    }


    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("SaveByDapper")]
    public async Task<IActionResult> SaveByDapper([FromBody] Color color)
    {
        var rs = await _colorProvider.SaveByDapper(color);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);

    }

    [HttpDelete]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("DeleteByDapper/{colorCode}")]
    public async Task<IActionResult> DeleteByDapper(string colorCode)
    {
        var rs = await _colorProvider.DeleteByDapper(colorCode);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);

    }

}

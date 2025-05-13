using Base.BaseService;
using Base.MasterData.ProductProperties;
using Core.MasterData.ProductProperties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Servicer.ProductProperties;

namespace API_Application.Controllers.MasterData.ProductProperties;
[Route("api/[controller]")]
[ApiController]
public class MaterialController : ControllerBase
{
    private readonly ICRUD_Service<Material, int> _materialService;
    private readonly IMaterialProvider _materialProvider;

    public MaterialController(ICRUD_Service<Material, int> materialService, IMaterialProvider materialProvider)
    {
        _materialService = materialService;
        _materialProvider = materialProvider;
    }

    [HttpGet]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> GetAll()
    {
        var rs = await _materialService.GetAll();
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);

    }

    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> Save([FromBody] Material material)
    {
        var rs = await _materialService.Create(material);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);

    }

    [HttpPut]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> Update([FromBody] Material material)
    {
        var rs = await _materialService.Update(material);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);

    }

    [HttpDelete]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> Delete(int id)
    {
        var rs = await _materialService.Delete(id);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);

    }



    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("SaveByDapper")]
    public async Task<IActionResult> SaveByDapper([FromBody] Material material)
    {
        var rs = await _materialProvider.SaveByDapper(material);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);

    }

    [HttpDelete]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("DeleteByDapper/{materialCode}")]
    public async Task<IActionResult> DeleteByDapper(string materialCode)
    {
        var rs = await _materialProvider.DeleteByDapper(materialCode);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);

    }
}

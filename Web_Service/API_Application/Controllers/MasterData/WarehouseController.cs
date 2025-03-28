using Base.BaseService;
using Base.MasterData;
using Core.MasterData;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API_Application.Controllers.MasterData;

[ApiController]
[Route("api/[controller]")]
public class WarehouseController : ControllerBase
{
    private readonly ICRUD_Service<Warehouse, int> _warehouseService;

    public WarehouseController(ICRUD_Service<Warehouse, int> warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpGet]
    [Produces("application/json")]
    public async Task<IActionResult> GetAll()
    {
        var rs = await _warehouseService.GetAll();
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);
    }

    [HttpGet("{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> GetById(int id)
    {
        var rs = await _warehouseService.Get(id);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);
    }

    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Create([FromBody] Warehouse warehouse)
    {
        var rs = await _warehouseService.Create(warehouse);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);
    }

    [HttpPut]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Update([FromBody] Warehouse warehouse)
    {
        var rs = await _warehouseService.Update(warehouse);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);
    }

    [HttpDelete]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Delete(int id)
    {
        var rs = await _warehouseService.Delete(id);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);
    }
}

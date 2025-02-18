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
        return rs == null ? BadRequest("No records found") : Ok(rs);
    }

    [HttpGet("{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> GetById(int id)
    {
        var warehouse = await _warehouseService.Get(id);
        return warehouse == null ? NotFound($"Warehouse with ID {id} not found") : Ok(warehouse);
    }

    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Create([FromBody] Warehouse warehouse)
    {
        var rs = await _warehouseService.Create(warehouse);
        return rs == null ? BadRequest("Failed to create warehouse") : Ok(rs);
    }

    [HttpPut]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Update([FromBody] Warehouse warehouse)
    {
        var rs = await _warehouseService.Update(warehouse);
        return rs == null ? BadRequest("Failed to update warehouse") : Ok(rs);
    }

    [HttpDelete]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Delete(int id)
    {
        var rs = await _warehouseService.Delete(id);
        return rs == null ? BadRequest("Failed to delete warehouse") : Ok(rs);
    }
}

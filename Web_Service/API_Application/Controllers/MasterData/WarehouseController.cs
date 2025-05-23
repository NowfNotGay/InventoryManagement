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
    private readonly ICRUD_Service<Warehouse, int> _ICRUD_Service;
    private readonly IWarehouseProvider _warehouseProvider;


    public WarehouseController(ICRUD_Service<Warehouse, int> iCRUD_Service, IWarehouseProvider warehouseProvider)
    {
        _warehouseProvider = warehouseProvider;
        _ICRUD_Service = iCRUD_Service;

    }
    #region Normal CRUD

    [HttpGet]
    [Produces("application/json")]
    public async Task<IActionResult> GetAll()
    {
        var rs = await _ICRUD_Service.GetAll();
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);
    }

    [HttpGet("{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> GetById(int id)
    {
        var rs = await _ICRUD_Service.Get(id);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);
    }

    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Create([FromBody] Warehouse warehouse)
    {
        var rs = await _ICRUD_Service.Create(warehouse);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);
    }

    [HttpPut]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Update([FromBody] Warehouse warehouse)
    {
        var rs = await _ICRUD_Service.Update(warehouse);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);
    }

    [HttpDelete]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Delete(int id)
    {
        var rs = await _ICRUD_Service.Delete(id);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);
    }
    #endregion
    #region Dapper CRUD

    [HttpGet("warehouseCode/{warehouseCode}")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetByCode(string warehouseCode)
    {
        var rs = await _warehouseProvider.GetByCode(warehouseCode);
        return rs.Code == "0" ? Ok(rs) : NotFound($"Brand with Code {warehouseCode} not found");
    }

    [HttpPost("SaveByDapper")]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> SaveByDapper([FromBody] Warehouse warehouse)
    {
        var rs = await _warehouseProvider.SaveByDapper(warehouse);
        return rs.Code == "0" ? Ok(rs.Message) : BadRequest(rs.Message);
    }
    [HttpDelete("DeleteByDapper")]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> DeleteByDapper(string warehouseCode)
    {
        var rs = await _warehouseProvider.DeleteByDapper(warehouseCode);
        return rs.Code == "0" ? Ok(rs.Message) : BadRequest(rs.Message);
    }

    #endregion

}

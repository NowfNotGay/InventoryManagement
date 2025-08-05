using Base.BaseService;
using Base.TransactionManagement;
using Core.BaseClass;
using Core.TransactionManagement;
using Microsoft.AspNetCore.Mvc;
using Servicer.WarehouseManagement;
using System.Threading.Tasks;

namespace API_Application.Controllers.TransactionManagement;

[ApiController]
[Route("api/[controller]")]
public class InventoryTransactionController : ControllerBase
{
    private readonly ICRUD_Service<InventoryTransaction, int> _ICRUD_Service;
    private readonly IInventoryTransactionProvider _inventoryTransactionProvider;

    public InventoryTransactionController(
        ICRUD_Service<InventoryTransaction, int> iCRUD_Service,
        IInventoryTransactionProvider inventoryTransactionProvider)
    {
        _ICRUD_Service = iCRUD_Service;
        _inventoryTransactionProvider = inventoryTransactionProvider;
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
    public async Task<IActionResult> Create([FromBody] InventoryTransaction inventoryTransaction)
    {
        var rs = await _ICRUD_Service.Create(inventoryTransaction);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);
    }

    [HttpPut]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Update([FromBody] InventoryTransaction inventoryTransaction)
    {
        var rs = await _ICRUD_Service.Update(inventoryTransaction);
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

    [HttpGet("inventoryTransactionCode/{inventoryTransactionCode}")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetByCode(string inventoryTransactionCode)
    {
        var rs = await _inventoryTransactionProvider.GetByCode(inventoryTransactionCode);
        return rs.Code == "0" ? Ok(rs) : NotFound($"InventoryTransaction with Code {inventoryTransactionCode} not found");
    }

    [HttpPost("SaveByDapper")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> SaveByDapper([FromBody] InventoryTransaction inventoryTransaction)
    {
        var rs = await _inventoryTransactionProvider.SaveByDapper(inventoryTransaction);
        return rs.Code == "0" ? Ok(rs.Message) : BadRequest(rs.Message);
    }

    [HttpDelete("DeleteByDapper")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> DeleteByDapper(string inventoryTransactionCode)
    {
        var rs = await _inventoryTransactionProvider.DeleteByDapper(inventoryTransactionCode);
        return rs.Code == "0" ? Ok(rs.Message) : BadRequest(rs.Message);
    }

    #endregion
}
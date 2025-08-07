using API_Application.Utilities;
using Base.BaseService;
using Base.TransactionManagement;
using Base.WarehouseManagement;
using Core.BaseClass;
using Core.TransactionManagement;
using Core.WarehouseManagement;
using Microsoft.AspNetCore.Mvc;
using Servicer.WarehouseManagement;
using System.Threading.Tasks;

namespace API_Application.Controllers.TransactionManagement;

[ApiController]
[Route("api/[controller]")]
public class InventoryTransactionController : ControllerBase
{
    private readonly ICRUD_Service<InventoryTransaction, int> _crudService;
    private readonly IInventoryTransactionProvider _inventoryTransactionProvider;

    public InventoryTransactionController(
        ICRUD_Service<InventoryTransaction, int> iCRUD_Service,
        IInventoryTransactionProvider inventoryTransactionProvider)
    {
        _inventoryTransactionProvider = inventoryTransactionProvider;
        _crudService = iCRUD_Service;
    }
    [HttpGet]
    [Produces("application/json")]
    public async Task<IActionResult> GetAll()
    {

        return ApiResponseHelper.HandleResult(this, await _crudService.GetAll());
    }
    [HttpGet("{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> GetById(int id)
    {

        return ApiResponseHelper.HandleResult(this, await _crudService.Get(id));
    }
    [HttpDelete("{id:int}")]
    [Produces("application/json")]
    public async Task<IActionResult> Delete(int id)
    {
        return ApiResponseHelper.HandleResult(this, await _crudService.Delete(id));
    }
    [HttpGet("inventoryTransactionCode/{code}")]
    [Produces("application/json")]
    public async Task<IActionResult> GetByCode(string code)
    {

        return ApiResponseHelper.HandleResult(this, await _inventoryTransactionProvider.GetByCode(code));
    }
    [HttpPost("Save")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Save([FromBody] InventoryTransaction inventoryTransaction)
    {

        return ApiResponseHelper.HandleResult(this, await _crudService.Save(inventoryTransaction));
    }
    [HttpDelete("DeleteByDapper")]
    [Produces("application/json")]
    public async Task<IActionResult> DeleteByDapper(string inventoryTransactionCode)
    {

        return ApiResponseHelper.HandleResult(this, await _inventoryTransactionProvider.DeleteByDapper(inventoryTransactionCode));
    }
}
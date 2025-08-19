using API_Application.Utilities;
using Base.BaseService;
using Base.TransactionManagement;
using Core.TransactionManagement;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> GetByID(int id)
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
    [HttpDelete("code/{invTranCode}")]
    [Produces("application/json")]
    public async Task<IActionResult> DeleteByDapper(string invTranCode)
    {

        return ApiResponseHelper.HandleResult(this, await _inventoryTransactionProvider.DeleteByDapper(invTranCode));
    }
}
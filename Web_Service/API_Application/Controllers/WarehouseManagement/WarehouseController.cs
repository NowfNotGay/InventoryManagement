using API_Application.Utilities;
using Base.BaseService;
using Base.WarehouseManagement;
using Core.WarehouseManagement;
using Microsoft.AspNetCore.Mvc;

namespace API_Application.Controllers.WarehouseManagement;

[ApiController]
[Route("api/[controller]")]
public class WarehouseController : ControllerBase
{
    private readonly ICRUD_Service<Warehouse, int> _crudService;
    private readonly IWarehouseProvider _warehouseProvider;

    public WarehouseController(ICRUD_Service<Warehouse, int> iCRUD_Service, IWarehouseProvider warehouseProvider)
    {
        _warehouseProvider = warehouseProvider;
        _crudService = iCRUD_Service;

    }
    #region Normal CRUD

    [HttpGet]
    [Produces("application/json")]
    public async Task<IActionResult> GetAll()
    {
        return ApiResponseHelper.HandleResult(this, await _crudService.GetAll());
    }

    [HttpGet("{id:int}")]
    [Produces("application/json")]
    public async Task<IActionResult> GetById(int id)
    {

        return ApiResponseHelper.HandleResult(this, await _crudService.Get(id));
    }

    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Create([FromBody] Warehouse warehouse)
    {

        return ApiResponseHelper.HandleResult(this, await _crudService.Create(warehouse));
    }

    [HttpPut]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Update([FromBody] Warehouse warehouse)
    {


        return ApiResponseHelper.HandleResult(this, await _crudService.Update(warehouse));
    }

    [HttpDelete("{id:int}")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Delete(int id)
    {

        return ApiResponseHelper.HandleResult(this, await _crudService.Delete(id));
    }
    #endregion
    #region Dapper CRUD

    [HttpGet("code/{warehouseCode}")]
    [Produces("application/json")]
    public async Task<IActionResult> GetByCode(string warehouseCode)
    {
        return ApiResponseHelper.HandleResult(this, await _warehouseProvider.GetByCode(warehouseCode)); 
    }

    [HttpPost("Save")]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> Save([FromBody] Warehouse warehouse)
    {
        
        return ApiResponseHelper.HandleResult(this, await _warehouseProvider.SaveByDapper(warehouse));
    }
    [HttpDelete("code/{warehouseCode}")]
    [Produces("application/json")]
    public async Task<IActionResult> DeleteByDapper(string warehouseCode)
    {

        return ApiResponseHelper.HandleResult( this, await _warehouseProvider.DeleteByDapper(warehouseCode));

    }

    #endregion

}

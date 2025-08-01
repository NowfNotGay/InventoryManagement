using API_Application.Utilities;
using Base.BaseService;
using Base.MasterData;
using Core.MasterData;
using Microsoft.AspNetCore.Mvc;

namespace API_Application.Controllers.MasterData;

[Route("api/[controller]")]
[ApiController]
public class StatusMasterController : ControllerBase
{
    private readonly ICRUD_Service<StatusMaster, int> _ICRUD_Service;
    private readonly IStatusMasterProvider _statusMasterProvider;


    public StatusMasterController(ICRUD_Service<StatusMaster, int> iCRUD_Service, IStatusMasterProvider statusMasterProvider)
    {
        _statusMasterProvider = statusMasterProvider;
        _ICRUD_Service = iCRUD_Service;
    }
    #region CRUD
    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Create([FromBody] StatusMaster statusMaster)
    {

        return ApiResponseHelper.HandleResult(this, await _ICRUD_Service.Create(statusMaster));
    }
    [HttpPut]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Update([FromBody] StatusMaster statusMaster)
    {
        return ApiResponseHelper.HandleResult( this, await _ICRUD_Service.Update(statusMaster));
    }
    [HttpDelete("{id:int}")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Delete(int id)
    {
        
        return ApiResponseHelper.HandleResult( this, await _ICRUD_Service.Delete(id));
    }
    #endregion

    #region Dapper CRUD
    [HttpGet]
    [Produces("application/json")]

    public async Task<IActionResult> GetAll()
    {
        return ApiResponseHelper.HandleResult(this, await _ICRUD_Service.GetAll());
    }
    [HttpGet("code/{statusCode}")]
    [Produces("application/json")]
    public async Task<IActionResult> GetByCode(string statusCode)
    {
        return ApiResponseHelper.HandleResult(this, await _statusMasterProvider.GetByCode(statusCode));
    }

    [HttpPost("Save")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Save([FromBody] StatusMaster statusMaster)
    {
        
        return ApiResponseHelper.HandleResult( this, await _ICRUD_Service.Save(statusMaster));
    }
    [HttpDelete("code/{statusCode}")]
    [Produces("application/json")]

    public async Task<IActionResult> DeleteByDapper(string statusCode)
    {
        
        return ApiResponseHelper.HandleResult(this, await _statusMasterProvider.DeleteByDapper(statusCode));
    }
    #endregion


}


using Base.BaseService;
using Base.MasterData;
using Core.MasterData;
using Microsoft.AspNetCore.Mvc;

namespace API_Application.Controllers.MasterData;

[ApiController]
[Route("api/[controller]")]
public class StatusMasterController :ControllerBase
{
    private readonly IStatusMasterProvider _statusMasterProvider;
    private readonly ICRUD_Service<StatusMaster, int> _statusMasterService;


    public StatusMasterController(IStatusMasterProvider statusMasterProvider, ICRUD_Service<StatusMaster, int> statusMasterService)
    {
        _statusMasterProvider = statusMasterProvider;
        _statusMasterService = statusMasterService;
    }
    [HttpGet]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> GetAll()
    {
        var rs = await _statusMasterService.GetAll();
        return rs == null ? BadRequest("No records found") : Ok(rs);
    }
  
    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Create([FromBody] StatusMaster statusMaster)
    {
        var rs = await _statusMasterService.Create(statusMaster);
        return rs == null ? BadRequest("Failed to create record") : Ok(rs);
    }
    [HttpPut]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Update([FromBody] StatusMaster statusMaster)
    {
        var rs = await _statusMasterService.Update(statusMaster);
        return rs == null ? BadRequest("Failed to update record") : Ok(rs);
    }
    [HttpDelete]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Delete(int id)
    {
        var rs = await _statusMasterService.Delete(id);
        return rs == null ? BadRequest("Failed to delete record") : Ok(rs);
    }
}


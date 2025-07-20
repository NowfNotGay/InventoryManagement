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
        var rs = await _ICRUD_Service.Create(statusMaster);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);
    }
    [HttpPut]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Update([FromBody] StatusMaster statusMaster)
    {
        var rs = await _ICRUD_Service.Update(statusMaster);
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
    [HttpGet]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> GetAll()
    {
        var rs = await _ICRUD_Service.GetAll();
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);
    }
    [HttpGet("statusCode/{statusCode}")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetByCode(string statusCode)
    {
        var rs = await _statusMasterProvider.GetByCode(statusCode);
        return rs.Code == "0" ? Ok(rs) : NotFound($"Brand with Code {statusCode} not found");
    }

    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("Save")]
    public async Task<IActionResult> Save([FromBody] StatusMaster statusMaster)
    {
        var rs = await _ICRUD_Service.Save(statusMaster);
        return rs.Code == "0" ? Ok(rs) : BadRequest(rs);
    }
    [HttpDelete("DeleteByDapper/{statusCode}")]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> DeleteByDapper(string statusCode)
    {
        var rs = await _statusMasterProvider.DeleteByDapper(statusCode);
        return rs.Code == "0" ? Ok(rs) : BadRequest(rs);
    }
    #endregion


}


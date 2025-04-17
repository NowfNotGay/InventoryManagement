using Base.BaseService;
using Base.ProductClassification;
using Base.ProductProperties;
using Core.MasterData;
using Core.ProductClassification;
using Core.ProductProperties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Servicer.ProductClassification;

namespace API_Application.Controllers.ProductClassification;
[Route("api/[controller]")]
[ApiController]
public class VehicleModelController : ControllerBase
{
    private readonly ICRUD_Service<VehicleModel, int> _ICRUD_Service;
    private readonly IVehicleModelProvider _vehicleModelProvider;

    public VehicleModelController(ICRUD_Service<VehicleModel, int> iCRUD_Service, IVehicleModelProvider vehicleModelProvider)
    {
        _vehicleModelProvider= vehicleModelProvider;
        _ICRUD_Service = iCRUD_Service;
    }

    #region Dapper CRUD
    [HttpGet]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetAll()
    {
        var rs = await _ICRUD_Service.GetAll();
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);

    }

    [HttpGet("id")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetByID([FromQuery] int id)
    {
        var rs = await _ICRUD_Service.Get(id);
        return rs.Code == "0" ? Ok(rs.Data) : BadRequest(rs.Message);
    }

    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Create([FromBody] VehicleModel vehicleModel)
    {
        var rs = await _ICRUD_Service.Create(vehicleModel);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);

    }

    [HttpPut]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Update([FromBody] VehicleModel vehicleModel)
    {

        var rs = await _ICRUD_Service.Update(vehicleModel);
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
    [HttpGet("modelCode/{modelCode}")] // Sửa thành đúng cú pháp cho route parameter
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetByCode(string modelCode)
    {
        var rs = await _vehicleModelProvider.GetByCode(modelCode);
        // Sửa lại kiểm tra dựa trên property Code của ResultService
        return rs.Code == "0" ? Ok(rs) : NotFound($"Model with Code {modelCode} not found");
    }

    [HttpPost("SaveByDapper")]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> SaveByDapper([FromBody] VehicleModel vehicleModel)
    {
        var rs = await _vehicleModelProvider.SaveByDapper(vehicleModel);
        return rs.Code == "0" ? Ok(rs.Message) : BadRequest(rs.Message);
    }
    [HttpDelete("DeleteByDapper")]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> DeleteByDapper(string modelCode)
    {
        var rs = await _vehicleModelProvider.DeleteByDapper(modelCode);
        return rs.Code == "0" ? Ok(rs.Message) : BadRequest(rs.Message);
    }

    #endregion

}

using Base.BaseService;
using Base.ProductClassification;
using Base.ProductProperties;
using Core.MasterData;
using Core.ProductClassification;
using Core.ProductProperties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Application.Controllers.ProductClassification;
[Route("api/[controller]")]
[ApiController]
public class VehicleModelController : ControllerBase
{
    private readonly ICRUD_Service<VehicleModel, int> _vehicleModelService;
    private readonly IVehicleModelProvider _vehicleModelProvider;

    public VehicleModelController(ICRUD_Service<VehicleModel, int> vehicleModelService, IVehicleModelProvider vehicleModelProvider)
    {
        _vehicleModelService = vehicleModelService;
        _vehicleModelProvider = vehicleModelProvider;
    }

    [HttpGet]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetAll()
    {
        var rs = await _vehicleModelService.GetAll();
        return rs == null ? BadRequest("No vehicle models found") : Ok(rs);
    }

    [HttpGet("{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> GetById(int id)
    {
        var vehicleModel = await _vehicleModelService.Get(id);
        return vehicleModel == null ? NotFound($"Vehicle model with ID {id} not found") : Ok(vehicleModel);
    }

    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Create([FromBody] VehicleModel vehicleModel)
    {
        var rs = await _vehicleModelService.Create(vehicleModel);
        return rs == null ? BadRequest("Failed to create vehicle model") : Ok(rs);
    }

    [HttpPut]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Update([FromBody] VehicleModel vehicleModel)
    {

        var rs = await _vehicleModelService.Update(vehicleModel);
        return rs == null ? BadRequest("Failed to update vehicle model") : Ok(rs);
    }

    [HttpDelete]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Delete(int id)
    {
        var rs = await _vehicleModelService.Delete(id);
        return rs == null ? BadRequest("Failed to delete vehicle model") : Ok(rs);
    }
}

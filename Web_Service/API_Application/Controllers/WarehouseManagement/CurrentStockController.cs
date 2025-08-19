using Base.BaseService;
using Base.WarehouseManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Application.Controllers.WarehouseManagement;
[Route("api/[controller]")]
[ApiController]
public class CurrentStockController : ControllerBase
{
    private readonly ICurrentStockProvider _currentStockProvider;

    public CurrentStockController(ICurrentStockProvider currentStockProvider)
    {
        _currentStockProvider = currentStockProvider;
    }

    [HttpGet("getAllByWarehouse/{warehouseCode}")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetAllByWarehouse( string warehouseCode)
    {
        var rs = await _currentStockProvider.GetAllCurrentStockByWarehouse(warehouseCode);
        return rs.Code == "0" ? Ok(rs) : BadRequest(rs);

    }
}

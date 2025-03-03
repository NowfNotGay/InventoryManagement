using Base.BaseService;
using Base.ProductProperties;
using Core.ProductProperties;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API_Application.Controllers.ProductProperties;
[Route("api/[controller]")]
[ApiController]
public class UnitOfMeasureController : ControllerBase
{
    private readonly ICRUD_Service<UnitOfMeasure, int> _unitOfMeasureService;
    private readonly IUnitOfMeasureProvider _unitOfMeasureProvider;

    public UnitOfMeasureController(ICRUD_Service<UnitOfMeasure, int> unitOfMeasureService, IUnitOfMeasureProvider unitOfMeasureProvider)
    {
        _unitOfMeasureService = unitOfMeasureService;
        _unitOfMeasureProvider = unitOfMeasureProvider;
    }

    [HttpGet]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetAll()
    {
        var rs = await _unitOfMeasureService.GetAll();
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);
    }

    [HttpGet("{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> GetById(int id)
    {
        var rs = await _unitOfMeasureService.Get(id);
        return !rs.Code.Equals("0") ? NotFound(rs) : Ok(rs);
    }

    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Create([FromBody] UnitOfMeasure unitOfMeasure)
    {
        var rs = await _unitOfMeasureService.Create(unitOfMeasure);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);
    }

    [HttpPut]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Update([FromBody] UnitOfMeasure unitOfMeasure)
    {
    
        var rs = await _unitOfMeasureService.Update(unitOfMeasure);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);
    }

    [HttpDelete("{id}")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Delete(int id)
    {
        var rs = await _unitOfMeasureService.Delete(id);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);
    }
}

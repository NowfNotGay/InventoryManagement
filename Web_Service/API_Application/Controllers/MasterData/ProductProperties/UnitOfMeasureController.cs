using Base.BaseService;
using Base.MasterData.ProductProperties;
using Core.MasterData.ProductProperties;
using Core.ProductClassification;
using Microsoft.AspNetCore.Mvc;
using Servicer.ProductClassification;
using System.Threading.Tasks;

namespace API_Application.Controllers.MasterData.ProductProperties;
[Route("api/[controller]")]
[ApiController]
public class UnitOfMeasureController : ControllerBase
{
    private readonly ICRUD_Service<UnitOfMeasure, int> _ICRUD_Service;
    private readonly IUnitOfMeasureProvider _unitOfMeasureProvider;

    public UnitOfMeasureController(ICRUD_Service<UnitOfMeasure, int> iCRUD_Service, IUnitOfMeasureProvider unitOfMeasureProvider)
    {
        _ICRUD_Service = iCRUD_Service;
        _unitOfMeasureProvider = unitOfMeasureProvider;
    }
    #region CRUD
    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Create([FromBody] UnitOfMeasure unitOfMeasure)
    {
        var rs = await _ICRUD_Service.Create(unitOfMeasure);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);
    }

    [HttpPut]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Update([FromBody] UnitOfMeasure unitOfMeasure)
    {

        var rs = await _ICRUD_Service.Update(unitOfMeasure);
        return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);
    }

    [HttpDelete("{id}")]
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

    [HttpGet("{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> GetById(int id)
    {
        var rs = await _ICRUD_Service.Get(id);
        return !rs.Code.Equals("0") ? NotFound(rs) : Ok(rs);
    }
    [HttpGet("uomCode/{uomCode}")] // Sửa thành đúng cú pháp cho route parameter
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetByCode(string uomCode)
    {
        var rs = await _unitOfMeasureProvider.GetByCode(uomCode);
        // Sửa lại kiểm tra dựa trên property Code của ResultService
        return rs.Code == "0" ? Ok(rs) : NotFound($"Model with Code {uomCode} not found");
    }

    [HttpPost("SaveByDapper")]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> SaveByDapper([FromBody] UnitOfMeasure unitOfMeasure)
    {
        var rs = await _unitOfMeasureProvider.SaveByDapper(unitOfMeasure);
        return rs.Code == "0" ? Ok(rs.Message) : BadRequest(rs.Message);
    }
    [HttpDelete("DeleteByDapper")]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> DeleteByDapper(string uomCode)
    {
        var rs = await _unitOfMeasureProvider.DeleteByDapper(uomCode);
        return rs.Code == "0" ? Ok(rs.Message) : BadRequest(rs.Message);
    }
    #endregion


}

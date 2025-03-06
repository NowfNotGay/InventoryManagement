using Base.BaseService;
using Base.ProductClassification;
using Base.ProductProperties;
using Core.MasterData;
using Core.ProductClassification;
using Core.ProductProperties;
using Core.WarehouseManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Application.Controllers.ProductClassification;
[Route("api/[controller]")]
[ApiController]
public class BrandController : ControllerBase
{
    private readonly ICRUD_Service<Brand, int> _ICRUD_Service;
    private readonly IBrandProvider _brandProvider;

    public BrandController(ICRUD_Service<Brand, int> iCRUD_Service, IBrandProvider brandProvider)
    {
        _ICRUD_Service = iCRUD_Service;
        _brandProvider = brandProvider;
    }
    #region Normal CRUD

    [HttpGet]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetAll()
    {
        var rs = await _ICRUD_Service.GetAll();
        return rs == null ? BadRequest("No brands found") : Ok(rs);
    }

    [HttpGet("{id}")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetById(int id)
    {
        var rs = await _ICRUD_Service.Get(id);
        return rs == null ? NotFound($"Brand with ID {id} not found") : Ok(rs);
    }

    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Create([FromBody] Brand brand)
    {
        var rs = await _ICRUD_Service.Create(brand);
        return rs == null ? BadRequest("Failed to create brand") : Ok(rs);
    }

    [HttpPut]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Update([FromBody] Brand brand)
    {
        var rs = await _ICRUD_Service.Update(brand);
        return rs == null ? BadRequest("Failed to update brand") : Ok(rs);
    }

    [HttpDelete]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Delete(int id)
    {
        var rs = await _ICRUD_Service.Delete(id);
        return rs == null ? BadRequest("Failed to delete brand") : Ok(rs);
    }
    #endregion

    #region Dapper CRUD

    [HttpPost("SaveByDapper")]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> SaveByDapper([FromBody] Brand brand)
    {
        var rs = await _brandProvider.SaveByDapper(brand);
        return rs.Code == "0" ? Ok(rs.Data) : BadRequest(rs.Message);
    }
    [HttpDelete("DeleteByDapper")]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> DeleteByDapper(string brandCode)
    {
        var rs = await _brandProvider.DeleteByDapper(brandCode);
        return rs.Code == "0" ? Ok(rs.Message) : BadRequest(rs.Message);
    }


    #endregion
}

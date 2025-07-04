﻿using Base.BaseService;
using Base.MasterData.ProductClassification;
using Core.MasterData;
using Core.MasterData.ProductClassification;
using Core.WarehouseManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Application.Controllers.MasterData.ProductClassification;
[Route("api/[controller]")]
[ApiController]
public class BrandController : ControllerBase
{
    private readonly ICRUD_Service<Brand, int> _ICRUD_Service;
    private readonly IBrandProvider _brandProvider;

    public BrandController(ICRUD_Service<Brand, int> iCRUD_Service, IBrandProvider brandProvider)
    {
        _brandProvider = brandProvider;
        _ICRUD_Service = iCRUD_Service;
    }
    #region Normal CRUD



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
    [HttpGet]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetAll()
    {
        var rs = await _ICRUD_Service.GetAll();
        return rs == null ? BadRequest("No brands found") : Ok(rs);
    }

    [HttpGet("brandCode/{brandCode}")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetByCode(string brandCode)
    {
        var rs = await _brandProvider.GetByCode(brandCode);
        return rs.Code == "0" ? Ok(rs) : NotFound($"Brand with Code {brandCode} not found");
    }

    [HttpPost("SaveByDapper")]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> SaveByDapper([FromBody] Brand brand)
    {
        var rs = await _brandProvider.SaveByDapper(brand);
        return rs.Code == "0" ? Ok(rs) : BadRequest(rs);
    }
    [HttpDelete("DeleteByDapper/{brandCode}")]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> DeleteByDapper(string brandCode)
    {
        var rs = await _brandProvider.DeleteByDapper(brandCode);
        return rs.Code == "0" ? Ok(rs) : BadRequest(rs);
    }


    #endregion
}

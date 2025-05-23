using Base.BaseService;
using Base.WarehouseManagement;
using Core.WarehouseManagement;
using Microsoft.AspNetCore.Mvc;
using Servicer.WarehouseManagement;

namespace API_Application.Controllers.WarehouseManagement;

[Route("api/[controller]")]
[ApiController]
public class GoodsIssueNoteController : ControllerBase
{
    private readonly IGoodsIssueNoteProvider _goodsIssueNoteProvider;
    private readonly ICRUD_Service<GoodsIssueNote, int> _ICRUD_Service;
    public GoodsIssueNoteController(IGoodsIssueNoteProvider goodsIssueNoteProvider, ICRUD_Service<GoodsIssueNote, int> iCRUD_Service)
    {
        _goodsIssueNoteProvider = goodsIssueNoteProvider;
        _ICRUD_Service = iCRUD_Service;
    }
    #region GoodsIssueNote
    [HttpGet]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetAll() //done
    {
        var rs = await _ICRUD_Service.GetAll();
        return rs.Code == "0" ? Ok(rs.Data) : BadRequest(rs.Message);
    }
    [HttpGet("ID")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetByID([FromQuery] int id) //done
    {
        var rs = await _ICRUD_Service.Get(id);
        return rs.Code == "0" ? Ok(rs.Data) : BadRequest(rs.Message);
    }
    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Create([FromBody] GoodsIssueNote GoodsIssueNote) //done
    {
        var rs = await _ICRUD_Service.Create(GoodsIssueNote);
        return rs.Code == "0" ? Ok(rs.Data) : BadRequest(rs.Message);
    }
    [HttpPut]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Update([FromBody] GoodsIssueNote GoodsIssueNote) //done
    {
        var rs = await _ICRUD_Service.Update(GoodsIssueNote);
        return rs.Code == "0" ? Ok(rs.Message) : BadRequest(rs.Message);
    }
    
    [HttpDelete]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Delete([FromQuery] int id) //done
    {
        var rs = await _ICRUD_Service.Delete(id);
        return rs.Code == "0" ? Ok(rs.Message) : BadRequest(rs.Message);
    }
    #endregion

    #region GoodsIssueNote Dapper

    [HttpDelete("DeleteByDapper")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> DeleteByDapper(string ginCode) //done
    {
        var rs = await _goodsIssueNoteProvider.DeleteByDapper(ginCode);
        return rs.Code == "0" ? Ok(rs.Message) : BadRequest(rs.Message);
    }

    [HttpPost("SaveByDapper")]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> SaveByDapper([FromBody] GoodsIssueNote GoodsIssueNote)
    {
        var rs = await _goodsIssueNoteProvider.SaveByDapper(GoodsIssueNote);
        return rs.Code == "0" ? Ok(rs.Message) : BadRequest(rs.Message);
    }
    #endregion

    #region GoodsIssueNoteLine
    [HttpGet("GoodsIssueNoteLine/GINCode")]
    [Consumes("application/json")]
    [Produces("application/json")]

    #endregion

    #region GoodsIssueNoteLine

    public async Task<IActionResult> GoodsIssueNoteLine_GetAll([FromQuery] string GINCode)
    {
        var rs = await _goodsIssueNoteProvider.GetLineByRefCode(GINCode);
        return rs.Code == "0" ? Ok(rs) : BadRequest(rs);
    }

    [HttpDelete("GoodsIssueNoteLine/DeleteMultiLine")]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> GoodsIssueNoteLine_Delete_Multi([FromBody] List<GoodsIssueNoteLine> param)
    {
        var rs = await _goodsIssueNoteProvider.DeleteLine(param);
        return rs.Code == "0" ? Ok(rs) : BadRequest(rs);
    }
    #endregion

    #region GoodsIssueNoteHeaderAndLine
    [HttpPost("HeaderLine/CreateBothIssue")]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> GoodsIssueNote_Create_HeaderAndLine([FromBody] GoodsIssueNote_Param param)
    {
        var rs = await _goodsIssueNoteProvider.SaveHeaderAndLine(param);
        return rs.Code == "0" ? Ok(rs) : BadRequest(rs);
    }

    [HttpDelete("HeaderLine/DeleteIssueHeaderAndLine/ginID")]
    [Consumes("application/json")]
    [Produces("application/json")]

    public async Task<IActionResult> Delete_HeaderAndDetail([FromQuery] int ginID)
    {
        var rs = await _goodsIssueNoteProvider.Delete_HeaderAndDetail(ginID);
        return rs.Code == "0" ? Ok(rs) : BadRequest(rs);
    }

    #endregion

}
using Base.BaseService;
using Base.MasterData;
using Base.WarehouseManagement;
using Core.MasterData;
using Core.WarehouseManagement;
using Helper.Method;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Application.Controllers.WarehouseManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsReceiptNoteController : ControllerBase
    {
        private readonly IGoodsReceiptNoteProvider _GoodsReceiptNoteProvider;
        private readonly ICRUD_Service<GoodsReceiptNote, string> _ICRUD_Service;

        public GoodsReceiptNoteController(IGoodsReceiptNoteProvider goodsReceiptNoteProvider, ICRUD_Service<GoodsReceiptNote, string> iCRUD_Service)
        {
            _GoodsReceiptNoteProvider = goodsReceiptNoteProvider;
            _ICRUD_Service = iCRUD_Service;
        }


        #region GoodsReceiptNote
        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAll()
        {
            var rs = await _ICRUD_Service.GetAll();
            return rs.Code == ResponseCode.Success.ToString() ? Ok(rs) : BadRequest(rs);
        }

        [HttpGet("ID")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GetByID([FromQuery] string ID)
        {
            var rs = await _ICRUD_Service.Get(ID);
            return rs.Code == ResponseCode.Success.ToString() ? Ok(rs.Data) : BadRequest(rs.Message);

        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> Save([FromBody] GoodsReceiptNote GoodsReceiptNote)
        {
            var rs = await _ICRUD_Service.Update(GoodsReceiptNote);
            return rs.Code == ResponseCode.Success.ToString() ? Ok(rs) : BadRequest(rs);
        }
        [HttpPut]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Update([FromBody] GoodsReceiptNote GoodsReceiptNote)
        {
            var rs = await _ICRUD_Service.Update(GoodsReceiptNote);
            return rs.Code == ResponseCode.Success.ToString() ? Ok(rs.Message) : BadRequest(rs.Message);

        }
        [HttpDelete]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> Delete(string id)
        {
            var rs = await _ICRUD_Service.Delete(id);
            return rs.Code == ResponseCode.Success.ToString() ? Ok(rs.Message) : BadRequest(rs.Message);
        }


        [HttpDelete("DeleteByDapper")]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> DeleteV2(string grCode)
        {
            var rs = await _GoodsReceiptNoteProvider.DeleteByDapper(grCode);
            return rs.Code == ResponseCode.Success.ToString() ? Ok(rs.Message) : BadRequest(rs.Message);
        }

        [HttpPost("SaveByDapper")]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> CreateV2([FromBody] GoodsReceiptNote GoodsReceiptNote)
        {
            var rs = await _GoodsReceiptNoteProvider.SaveByDapper(GoodsReceiptNote);
            return rs.Code == ResponseCode.Success.ToString() ? Ok(rs) : BadRequest(rs);
        }

        #endregion

        #region GoodsReceiptNoteLine
        [HttpGet("GoodsReceiptNoteLine/GRNCode")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GoodsReceiptNoteLine_GetAll([FromQuery] string GRNCode)
        {
            var rs = await _GoodsReceiptNoteProvider.GetLineByRefCode(GRNCode);
            return rs.Code == ResponseCode.Success.ToString() ? Ok(rs) : BadRequest(rs);
        }

        [HttpGet("GoodsReceiptNoteLine/AddOrUpdate")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GoodsReceiptNoteLine_Save([FromBody] GoodsReceiptNoteLine entity)
        {
            var rs = await _GoodsReceiptNoteProvider.GoodReceiptNoteLine_Save(entity);
            return rs.Code == ResponseCode.Success.ToString() ? Ok(rs) : BadRequest(rs);
        }

        [HttpDelete("GoodsReceiptNoteLine/GoodsReceiptNoteLine_Delete_SingleLine")]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> GoodsReceiptNoteLine_Delete_SingleLine([FromQuery] Guid GRNCode)
        {
            var rs = await _GoodsReceiptNoteProvider.GoodsReceiptNoteLine_Delete_SingleLine(GRNCode);
            return rs.Code == ResponseCode.Success.ToString() ? Ok(rs) : BadRequest(rs);
        }

        [HttpDelete("/GoodsReceiptNoteLine/GoodsReceiptNoteLine_Delete_Multi")]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> GoodsReceiptNoteLine_Delete_Multi([FromBody] List<GoodsReceiptNoteLine> param)
        {
            var rs = await _GoodsReceiptNoteProvider.GoodsReceiptNoteLine_Delete_Multi_Line(param);
            return rs.Code == ResponseCode.Success.ToString() ? Ok(rs.Data) : BadRequest(rs.Message);
        }
        #endregion

        #region GoodsReceiptNoteHeaderAndLine

        [HttpPost("HeaderLine/CreateBoth")]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> GoodsReceiptNote_Create_HeaderAndLine([FromBody] GoodsReceiptNote_Param param)
        {
            var rs = await _GoodsReceiptNoteProvider.Save(param);
            return rs.Code == ResponseCode.Success.ToString() ? Ok(rs.Data) : BadRequest(rs.Message);
        }

        [HttpDelete("HeaderLine/DeleteHeaderAndLine/ID")]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> Delete_HeaderAndDetail([FromQuery] string grnCode)
        {
            var rs = await _GoodsReceiptNoteProvider.Delete_HeaderAndDetail(grnCode);
            return rs.Code == ResponseCode.Success.ToString() ? Ok(rs) : BadRequest(rs);
        }

        #endregion

    }
}


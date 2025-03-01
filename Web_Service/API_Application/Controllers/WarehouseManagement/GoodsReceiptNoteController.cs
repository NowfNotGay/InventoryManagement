using Base.BaseService;
using Base.MasterData;
using Base.WarehouseManagement;
using Core.MasterData;
using Core.WarehouseManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Application.Controllers.WarehouseManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsReceiptNoteController : ControllerBase
    {
        private readonly IGoodsReceiptNoteProvider _GoodsReceiptNoteProvider;
        private readonly ICRUD_Service<GoodsReceiptNote, int> _ICRUD_Service;

        public GoodsReceiptNoteController(IGoodsReceiptNoteProvider goodsReceiptNoteProvider, ICRUD_Service<GoodsReceiptNote, int> iCRUD_Service)
        {
            _GoodsReceiptNoteProvider = goodsReceiptNoteProvider;
            _ICRUD_Service = iCRUD_Service;
        }

        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAll()
        {
            var rs = await _ICRUD_Service.GetAll();
            return rs.Code == "0" ? Ok(rs.Data) : BadRequest(rs.Message);
        }

        [HttpGet("ID")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GetByID([FromQuery] int ID)
        {
            var rs = await _ICRUD_Service.Get(ID);
            return rs.Code == "0" ? Ok(rs.Data) : BadRequest(rs.Message);

        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> Save([FromBody] GoodsReceiptNote GoodsReceiptNote)
        {
            var rs = await _ICRUD_Service.Create(GoodsReceiptNote);
            return rs.Code == "0" ? Ok(rs.Data) : BadRequest(rs.Message);
        }
        [HttpPut]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Update([FromBody] GoodsReceiptNote GoodsReceiptNote)
        {
            var rs = await _ICRUD_Service.Update(GoodsReceiptNote);
            return rs.Code == "0" ? Ok(rs.Message) : BadRequest(rs.Message);

        }
        [HttpDelete]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> Delete(int id)
        {
            var rs = await _ICRUD_Service.Delete(id);
            return rs.Code == "0" ? Ok(rs.Message) : BadRequest(rs.Message);
        }


        [HttpDelete("DeleteByDapper")]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> DeleteV2(string grCode)
        {
            var rs = await _GoodsReceiptNoteProvider.DeleteByDapper(grCode);
            return rs.Code == "0" ? Ok(rs.Message) : BadRequest(rs.Message);
        }

        [HttpPost("CreateByDapper")]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> CreateV2([FromBody] GoodsReceiptNote GoodsReceiptNote)
        {
            var rs = await _GoodsReceiptNoteProvider.CreateByDapper(GoodsReceiptNote);
            return rs.Code == "0" ? Ok(rs.Data) : BadRequest(rs.Message);
        }

        [HttpGet("/GoodsReceiptNoteLine/GRNCode")]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> GoodsReceiptNoteLine_GetAll([FromQuery]string GRNCode)
        {
            var rs = await _GoodsReceiptNoteProvider.GetLineByRefCode(GRNCode);
            return rs.Code == "0" ? Ok(rs.Data) : BadRequest(rs.Message);
        }

        [HttpPost("/HeaderLine/CreateAndCreate")]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> GoodsReceiptNote_Create_HeaderAndLine([FromBody]GoodsReceiptNote_Param param)
        {
            var rs = await _GoodsReceiptNoteProvider.CreateHeaderAndLine(param);
            return rs.Code == "0" ? Ok(rs.Data) : BadRequest(rs.Message);
        }

        [HttpDelete("/GoodsReceiptNoteLine/DeleteMultiLine")]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> GoodsReceiptNoteLine_Delete_Multi([FromBody] List<GoodsReceiptNoteLine> param)
        {
            var rs = await _GoodsReceiptNoteProvider.DeleteLine(param);
            return rs.Code == "0" ? Ok(rs.Data) : BadRequest(rs.Message);
        }

        [HttpDelete("/HeaderLine/DeleteHeaderAndLine/ID")]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> Delete_HeaderAndDetail([FromQuery]int grnID)
        {
            var rs = await _GoodsReceiptNoteProvider.Delete_HeaderAndDetail(grnID);
            return rs.Code == "0" ? Ok(rs.Message) : BadRequest(rs.Message);
        }
    }
}

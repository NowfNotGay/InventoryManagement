using Base.BaseService;
using Base.WarehouseManagement;
using Core.WarehouseManagement;
using Microsoft.AspNetCore.Mvc;

namespace API_Application.Controllers.WarehouseManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockTransferController : ControllerBase
    {
        private readonly IStockTransferProvider _StockTransferProvider;
        private readonly ICRUD_Service<StockTransfer, int> _ICRUD_Service;

        public StockTransferController(IStockTransferProvider stockTransferProvider, ICRUD_Service<StockTransfer, int> iCRUD_Service)
        {
            _StockTransferProvider = stockTransferProvider;
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

        public async Task<IActionResult> Save([FromBody] StockTransfer stockTransfer)
        {
            var rs = await _ICRUD_Service.Create(stockTransfer);
            return rs.Code == "0" ? Ok(rs.Data) : BadRequest(rs.Message);
        }
        [HttpPut]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Update([FromBody] StockTransfer stockTransfer)
        {
            var rs = await _ICRUD_Service.Update(stockTransfer);
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

        public async Task<IActionResult> DeleteV2(string stCode)
        {
            var rs = await _StockTransferProvider.DeleteByDapper(stCode);
            return rs.Code == "0" ? Ok(rs.Message) : BadRequest(rs.Message);
        }

        [HttpPost("CreateByDapper")]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> CreateV2([FromBody] StockTransfer stockTransfer)
        {
            var rs = await _StockTransferProvider.CreateByDapper(stockTransfer);
            return rs.Code == "0" ? Ok(rs.Data) : BadRequest(rs.Message);
        }

        [HttpGet("/StockTransferDetail/STCode")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> StockTransferDetail_GetAll([FromQuery] string stCode)
        {
            var rs = await _StockTransferProvider.GetDetailByStockTransferID(stCode);
            return rs.Code == "0" ? Ok(rs.Data) : BadRequest(rs.Message);
        }


        [HttpDelete("/StockTransferDetail/DeleteMultiLine")]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> StockTransferDetail_Delete_Multi([FromBody] List<StockTransferDetail> param)
        {
            var rs = await _StockTransferProvider.DeleteDetail(param);
            return rs.Code == "0" ? Ok(rs.Data) : BadRequest(rs.Message);
        }

        [HttpPost("/HeaderLine/CreateBoth")]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> StockTransfer_Create_HeaderAndLine([FromBody] StockTransfer_Param param)
        {
            var rs = await _StockTransferProvider.CreateHeaderAndDetail(param);
            return rs.Code == "0" ? Ok(rs.Data) : BadRequest(rs.Message);
        }

        [HttpDelete("/HeaderLine/DeleteHeaderAndLine/ID")]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> Delete_HeaderAndDetail([FromQuery] int stID)
        {
            var rs = await _StockTransferProvider.Delete_HeaderAndDetail(stID);
            return rs.Code == "0" ? Ok(rs.Message) : BadRequest(rs.Message);
        }
    }
}

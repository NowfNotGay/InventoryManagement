using API_Application.Utilities;
using Base.BaseService;
using Base.TransactionManagement;
using Core.TransactionManagement;
using Microsoft.AspNetCore.Mvc;
using Servicer.TransactionManagement;

namespace API_Application.Controllers.TransactionManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockTransferController : ControllerBase
    {
        private readonly IStockTransferProvider _StockTransferProvider;
        private readonly ICRUD_Service<StockTransfer, int> _crudService;

        public StockTransferController(IStockTransferProvider stockTransferProvider, ICRUD_Service<StockTransfer, int> iCRUD_Service)
        {
            _StockTransferProvider = stockTransferProvider;
            _crudService = iCRUD_Service;
        }


        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetAll()
        {
            return ApiResponseHelper.HandleResult(this, await _crudService.GetAll());
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return ApiResponseHelper.HandleResult(this, await _crudService.Get(id));

        }

        [HttpDelete("code/{stCode}")]
        [Produces("application/json")]
        public async Task<IActionResult> Delete(string stCode)
        {
            return ApiResponseHelper.HandleResult(this, await _StockTransferProvider.DeleteByDapper(stCode));
        }

        [HttpPost("Save")]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> Save([FromBody] StockTransfer stockTransfer)
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

            return ApiResponseHelper.HandleResult(this, await _StockTransferProvider.CreateHeaderAndDetail(param));
        }

        [HttpDelete("/HeaderLine/DeleteHeaderAndLine/{stID:int}")]
        [Produces("application/json")]
        public async Task<IActionResult> Delete_HeaderAndDetail([FromQuery] int stID)
        {

            return ApiResponseHelper.HandleResult(this, await _StockTransferProvider.Delete_HeaderAndDetail(stID));
        }
    }
}

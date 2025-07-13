using Base.BaseService;
using Base.MasterData;
using Core.MasterData;
using Helper.Method;
using Microsoft.AspNetCore.Mvc;

namespace API_Application.Controllers.MasterData
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionTypeController : ControllerBase
    {
        private readonly ITransactionTypeProvider _transactionTypeProvider;
        private readonly ICRUD_Service<TransactionType, string> _ICRUD_Service;

        //Tạo thêm 1 hàm GetByCode
        //Tạo thêm 1 hàm DeleteByCode
        //Chỉnh sửa hàm Save bao gồm update và create

        public TransactionTypeController(ITransactionTypeProvider transactionTypeProvider, ICRUD_Service<TransactionType, string> iCRUD_Service)
        {
            _transactionTypeProvider = transactionTypeProvider;
            _ICRUD_Service = iCRUD_Service;
        }

        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAll()
        {
            var rs = await _ICRUD_Service.GetAll();
            return rs.Code == "0" ? Ok(rs) : BadRequest(rs);

        }

        [HttpGet("ID")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GetByID([FromQuery] string TransactionTypeCode)
        {
            var rs = await _ICRUD_Service.Get(TransactionTypeCode);
            return rs.Code == "0" ? Ok(rs) : BadRequest(rs);
        }


        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> Save([FromBody] TransactionType TransactionType)
        {
            var rs = await _ICRUD_Service.Save(TransactionType);
            return rs.Code == ResponseCode.Success.ToString() ? Ok(rs) : BadRequest(rs);
        }

        [HttpPut]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> Update([FromBody] TransactionType TransactionType)
        {
            var rs = await _ICRUD_Service.Update(TransactionType);
            return rs.Code == "0" ? Ok(rs) : BadRequest(rs);

        }
        [HttpDelete("Delete")]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> Delete([FromQuery] string TransactionTypeCode)
        {
            var rs = await _ICRUD_Service.Delete(TransactionTypeCode);
            return rs.Code == "0" ? Ok(rs) : BadRequest(rs);
        }

    }
}

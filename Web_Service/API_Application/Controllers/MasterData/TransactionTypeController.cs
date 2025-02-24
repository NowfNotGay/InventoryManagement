using Base.BaseService;
using Base.MasterData;
using Core.MasterData;
using Microsoft.AspNetCore.Mvc;

namespace API_Application.Controllers.MasterData
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionTypeController : ControllerBase
    {
        private readonly ITransactionTypeProvider _transactionTypeProvider;
        private readonly ICRUD_Service_V2<TransactionType, int> _ICRUD_Service_V2;

        public TransactionTypeController(ITransactionTypeProvider transactionTypeProvider, ICRUD_Service_V2<TransactionType, int> iCRUD_Service)
        {
            _transactionTypeProvider = transactionTypeProvider;
            _ICRUD_Service_V2 = iCRUD_Service;
        }

        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAll()
        {
            var rs = await _ICRUD_Service_V2.GetAll();
            return rs.Code == "0" ? Ok(rs.Data) : BadRequest(rs.Message);

        }

        [HttpGet("ID")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GetByID([FromQuery] int ID)
        {
            var rs = await _ICRUD_Service_V2.Get(ID);
            return rs.Code == "0" ? Ok(rs.Data) : BadRequest(rs.Message);

        }


        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> Save([FromBody] TransactionType TransactionType)
        {
            var rs = await _ICRUD_Service_V2.Create(TransactionType);
            return rs.Code == "0" ? Ok(rs.Data) : BadRequest(rs.Message);
        }

        [HttpPut]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> Update([FromBody] TransactionType TransactionType)
        {
            var rs = await _ICRUD_Service_V2.Update(TransactionType);
            return rs.Code == "0" ? Ok(rs.Data) : BadRequest(rs.Message);

        }
        [HttpDelete]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> Delete(int id)
        {
            var rs = await _ICRUD_Service_V2.Delete(id);
            return rs.Code == "0" ? Ok(rs.Message) : BadRequest(rs.Message);
        }

    }
}

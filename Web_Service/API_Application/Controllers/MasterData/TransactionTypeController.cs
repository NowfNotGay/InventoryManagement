using Base.BaseService;
using Base.MasterData;
using Core.MasterData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Application.Controllers.MasterData
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionTypeController : ControllerBase
    {
        private readonly ITransactionTypeProvider _transactionTypeProvider;
        private readonly ICRUD_Service<TransactionType, int> _ICRUD_Service;

        public TransactionTypeController(ITransactionTypeProvider transactionTypeProvider, ICRUD_Service<TransactionType, int> iCRUD_Service)
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
            return rs == null ? BadRequest() : Ok(rs);

        }

        [HttpGet("ID")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GetByID([FromQuery] int ID)
        {
            var rs = await _ICRUD_Service.Get(ID);
            return rs == null ? BadRequest() : Ok(rs);

        }


        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> Save([FromBody] TransactionType TransactionType)
        {
            var rs = await _ICRUD_Service.Create(TransactionType);
            return rs == null ? BadRequest() : Ok(rs);
        }

        [HttpPut]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> Update([FromBody] TransactionType TransactionType)
        {
            var rs = await _ICRUD_Service.Update(TransactionType);
            return rs == null ? BadRequest() : Ok(rs);

        }
        [HttpDelete]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> Delete(int id)
        {
            var rs = await _ICRUD_Service.Delete(id);
            return rs == null ? BadRequest() : Ok(rs);

        }

        
    }
}

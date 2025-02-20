using Base.BaseService;
using Base.MasterData;
using Core.MasterData;
using Microsoft.AspNetCore.Mvc;

namespace API_Application.Controllers.MasterData
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageBinController : ControllerBase
    {
        private readonly IStorageBinProvider _storageBinProvider;
        private readonly ICRUD_Service<StorageBin, int> _ICRUD_Service;

        public StorageBinController(IStorageBinProvider storageBinProvider, ICRUD_Service<StorageBin, int> iCRUD_Service)
        {
            _storageBinProvider = storageBinProvider;
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

        public async Task<IActionResult> Save([FromBody] StorageBin storageBin)
        {
            var rs = await _ICRUD_Service.Create(storageBin);
            return rs == null ? BadRequest() : Ok(rs);
        }

        [HttpPut]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> Update([FromBody] StorageBin storageBin)
        {
            var rs = await _ICRUD_Service.Update(storageBin);
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

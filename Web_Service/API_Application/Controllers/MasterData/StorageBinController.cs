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
            return rs.Code == "0" ? Ok(rs) : BadRequest(rs);

        }

        [HttpGet("ID")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GetByID([FromQuery] int ID)
        {
            var rs = await _ICRUD_Service.Get(ID);
            return rs.Code == "0" ? Ok(rs) : BadRequest(rs);

        }

        [HttpGet("getAllByWarehouse/{warehouseCode}")]
        [Consumes("application/json")]


        [Produces("application/json")]
        public async Task<IActionResult> GetAllByWảehouse(string warehouseCode)
        {
            var rs = await _storageBinProvider.GetAllByWarehouseCode(warehouseCode);
            return rs.Code == "0" ? Ok(rs) : BadRequest(rs);

        }


        [HttpPost("Save")]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> Save([FromBody] StorageBin storageBin)
        {
            var rs = await _storageBinProvider.SaveByDapper(storageBin);
            return rs.Code == "0" ? Ok(rs) : BadRequest(rs);
        }

        [HttpPut]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> Update([FromBody] StorageBin storageBin)
        {
            var rs = await _ICRUD_Service.Update(storageBin);
            return rs.Code == "0" ? Ok(rs) : BadRequest(rs);

        }
        [HttpDelete("/{code}")]
        [Consumes("application/json")]
        [Produces("application/json")]
       
        public async Task<IActionResult> Delete(string code)
        {
            var rs = await _storageBinProvider.DeleteByDapper(code);
            return rs.Code == "0" ? Ok(rs) : BadRequest(rs);

        }


    }
}

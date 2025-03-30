using Base.BaseService;
using Base.WarehouseManagement;
using Core.WarehouseManagement;
using Microsoft.AspNetCore.Mvc;


namespace API_Application.Controllers.WarehouseManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrentStockController : ControllerBase
    {
        private readonly ICurrentStockProvider _CurrentStockProvider;
        private readonly ICRUD_Service<CurrentStock, int> _ICRUD_Service;
        public CurrentStockController (ICurrentStockProvider currentStockProvider , ICRUD_Service<CurrentStock,int> ICRUD_Service)
        {
            _CurrentStockProvider = currentStockProvider;
            _ICRUD_Service = ICRUD_Service;

        }
        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAll()
        {
            var rs = await _ICRUD_Service.GetAll();
            return rs.Code == "0" ? Ok(rs) : BadRequest(rs.Message);
        }

        [HttpGet("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GetByID( int ID)
        {
            var rs = await _ICRUD_Service.Get(ID);
            return rs.Code == "0" ? Ok(rs) : BadRequest(rs.Message);

        }
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> Save([FromBody] CurrentStock currentStock)
        {
            var rs = await _ICRUD_Service.Create(currentStock);
            return rs.Code == "0" ? Ok(rs.Data) : BadRequest(rs.Message);
        }

        [HttpPut]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Update([FromBody] CurrentStock currentStock)
        {
            var rs = await _ICRUD_Service.Update(currentStock);
            return rs.Code == "0" ? Ok(rs.Message) : BadRequest(rs.Message);

        }

        [HttpDelete]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Delete(int id)
        {
            var rs = await _ICRUD_Service.Delete(id);
            return rs.Code == "0" ? Ok(rs) : BadRequest(rs);
        }
        [HttpPost("Save")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> SaveBy([FromBody] CurrentStock currentStock)
        {
            var rs = await _CurrentStockProvider.Save(currentStock);            
            return rs.Code == "0" ? Ok(rs) : BadRequest(rs.Message);

        }


    }
}

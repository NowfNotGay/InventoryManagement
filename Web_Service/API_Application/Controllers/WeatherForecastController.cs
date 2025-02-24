
using Base.BaseService;
using Base.Example;
using Context.Example;
using Core.ExampleClass;
using Microsoft.AspNetCore.Mvc;
using Servicer.Example;

namespace API_Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IMessageContentProvider _messageContentProvider;
        private readonly ICRUD_Service<MessageContent, int> _messageContentService;
        //private readonly ICRUD_Service_V2<MessageContent, int> _messageContentService_V2;

        public WeatherForecastController(IMessageContentProvider messageContentProvider, ICRUD_Service<MessageContent, int> messageContentService)
        {
            _messageContentProvider = messageContentProvider;
            _messageContentService = messageContentService;
        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Save([FromBody] MessageContent data)
        {
            var result = await _messageContentService.Create(data);
            return result.Code != "0" ? BadRequest(result) : Ok(result);
        }


        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _messageContentService.GetAll();
            return result == null ? BadRequest() : Ok(result);
        }


        [HttpGet("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GetByID(int id)
        {
            var result = await _messageContentService.Get(id);
            return result == null ? BadRequest() : Ok(result);
        }
    }
}

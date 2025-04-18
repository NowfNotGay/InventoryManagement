using Base.BaseService;
using Base.Example;
using Base.MasterData;
using Context.Example;
using Context.MasterData;
using Core.ExampleClass;
using Core.MasterData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicer.Example;

namespace API_Application.Controllers.MasterData
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class BusinessPartnerController : ControllerBase
    {
        private readonly IBusinessPartnerProvider _businessPartnerProvider;
        private readonly ICRUD_Service<BusinessPartner, int> _businessPartnerService;




        public BusinessPartnerController(IBusinessPartnerProvider businessPartnerProvider, ICRUD_Service<BusinessPartner, int> businessPartnerService)
        {
            _businessPartnerProvider = businessPartnerProvider;
            _businessPartnerService = businessPartnerService;

        }



        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> Save([FromBody] BusinessPartner businessPartner)
        {
            var rs = await _businessPartnerService.Create(businessPartner);
            return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);

        }
        [HttpPut]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> Update([FromBody] BusinessPartner businessPartner)
        {
            var rs = await _businessPartnerService.Update(businessPartner);
            return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);

        }

        [HttpDelete]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> Delete(int id)
        {
            var rs = await _businessPartnerService.Delete(id);
            return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);

        }


        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> GetAll()
        {
            var rs = await _businessPartnerService.GetAll();
            return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);

        }


        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Route("SaveByDapper")]
        public async Task<IActionResult> SaveByDapper([FromBody] BusinessPartner businessPartner)
        {
            var rs = await _businessPartnerProvider.SaveByDapper(businessPartner);
            return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);

        }

        [HttpDelete]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Route("DeleteByDapper/{partnerCode}")]
        public async Task<IActionResult> DeleteByDapper(string partnerCode)
        {
            var rs = await _businessPartnerProvider.DeleteByDapper(partnerCode);
            return !rs.Code.Equals("0") ? BadRequest(rs) : Ok(rs);

        }
    }
}

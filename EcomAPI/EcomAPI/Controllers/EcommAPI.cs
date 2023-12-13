using EcomAPI.IEcomService;
using EcomAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcomAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EcommAPI : ControllerBase
    {

        private readonly IEcomOrder _IecomOrder;
        public EcommAPI(IEcomOrder IecomOrder)
        {

            _IecomOrder = IecomOrder;
        }
        [Route("GetRecentOrderofCustomer")]
        [HttpPost]
        public async Task<ActionResult> RecentOrder(CustomerInput CusInput)
        {
            try
            {
                var RecentOrder = _IecomOrder.RecentOrderResponse(CusInput);
                if (RecentOrder.customer != null)
                {
                    return Ok(RecentOrder);
                }
                return BadRequest(RecentOrder);
            }
            catch (Exception ex)
            {
                //_logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}

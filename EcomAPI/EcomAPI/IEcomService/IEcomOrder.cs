using EcomAPI.Model;

namespace EcomAPI.IEcomService
{
    public interface IEcomOrder
    {
        OrderDetailsResponse RecentOrderResponse(CustomerInput CustomerInput);
    }
}

using EcomAPI.DBCommon;
using EcomAPI.IEcomService;
using EcomAPI.Model;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace EcomAPI.EcomService
{
    public class EcomOrder : IEcomOrder
    {
        public OrderDetailsResponse RecentOrderResponse(CustomerInput CustomerInput)
        {
            OrderDetailsResponse RespOrder= new OrderDetailsResponse();
            try
            {
                DataSet OrderDetails = new DataSet();
                int nArgCnt = 0, nArgIncre = 2;
                SqlParameter[] arlParams = new SqlParameter[2];
                if (CustomerInput != null)
                {
                    //convert the class into sql params
                    foreach (PropertyInfo Param in typeof(CustomerInput).GetProperties())
                    {
                        string propertyName = Param.Name;
                        if(propertyName== "user")
                        {
                            arlParams[nArgCnt] = new SqlParameter(("@" + Param.Name), CustomerInput.user);                           
                        }
                        else
                        {
                            arlParams[nArgCnt] = new SqlParameter(("@" + Param.Name), CustomerInput.customerId);                            
                        }
                        nArgCnt = nArgCnt + 1;
                    }

                }
                //SQL helper for sql operations
                OrderDetails = SQLHelper.ExecuteDataset("GetRecentOrder", arlParams);
                if(OrderDetails != null)
                {
                    DataTable dataTable = new DataTable();
                    dataTable = OrderDetails.Tables[0];
                    if (dataTable.Rows.Count > 0)
                    {
                        Customer customer = new Customer();
                        customer.firstName = dataTable.Rows[0]["FIRSTNAME"].ToString();
                        customer.lastName = dataTable.Rows[0]["LASTNAME"].ToString();
                        RespOrder.customer= customer;

                        Order order = new Order();
                        order.deliveryAddress = dataTable.Rows[0]["DeliveryAddress"].ToString();
                        order.deliveryExpected = dataTable.Rows[0]["DeliveryExpected"].ToString();
                        order.orderNumber =int.Parse(dataTable.Rows[0]["OrderNumber"].ToString());
                        order.orderDate = dataTable.Rows[0]["ORDERDATE"].ToString();
                        RespOrder.order= order;
                        List<OrderItem> orderItems =new List<OrderItem>();
                        OrderItem LastOrder=new OrderItem();
                        LastOrder.priceEach = Convert.ToDecimal(dataTable.Rows[0]["PriceEach"].ToString());
                        LastOrder.quantity = Convert.ToInt32(dataTable.Rows[0]["QUANTITY"].ToString());
                        LastOrder.product = dataTable.Rows[0]["Product"].ToString();
                        orderItems.Add(LastOrder);
                    }
                }
                return RespOrder;
            }
            catch(Exception ex)
            {
                return RespOrder;
            }
        }
    }
}

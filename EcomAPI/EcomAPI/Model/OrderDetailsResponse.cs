namespace EcomAPI.Model
{
    public class OrderDetailsResponse
    {
        public Customer customer { get; set; }
        public Order order { get; set; }
    }
    public class Customer
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
    }

    public class Order
    {
        public int orderNumber { get; set; }
        public string orderDate { get; set; }
        public string deliveryAddress { get; set; }
        public List<OrderItem> orderItems { get; set; }
        public string deliveryExpected { get; set; }
    }

    public class OrderItem
    {
        public string product { get; set; }
        public int quantity { get; set; }
        public decimal priceEach { get; set; }
    }

}

using BusinessObject.Models;

namespace eStore.Models
{
    public class OrderViewModel
    {
        //Order = order,
        //OrderDetails = orderDetails
        public Order Order {  get; set; }
        public IEnumerable<ProductDetail> ProductDetails { get; set; }
    }
}

using BusinessObject.Models;

namespace eStore.Models
{
    public class OrderViewModel
    {
        public Order Order {  get; set; }
        public IEnumerable<ProductDetail> ProductDetails { get; set; }
    }
}

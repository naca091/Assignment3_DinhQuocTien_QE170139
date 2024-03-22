using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllProducts();
        //get product by id
        Product GetProductById(int id);
        void Create(Product product);
        void Update(Product product);
        void Delete(Product product);
        //GetProductNameById(orderDetail.ProductId)
        string GetProductNameById(int productId);
        public IEnumerable<int> GetAllCategoryId();
    }
}

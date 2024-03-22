using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class ProductDAO
    {
        
        public static ProductDAO instance = null;
        public static object instanceLock = new object();
        public static ProductDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductDAO();
                }
                return instance;
            }
        }

        public IEnumerable<Product> findAll()
        {
            IEnumerable<Product> products = new List<Product>();
            try
            {
                var context = new SalesManagementContext();
                products = context.Products;
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return products;
        }

        public void insert(Product product)
        {
            try
            {
                var context = new SalesManagementContext();
                context.Products.Add(product);
                context.SaveChanges();
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void update(Product product)
        {
            try
            {
                var context = new SalesManagementContext();
                context.Products.Update(product);
                context.SaveChanges();
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void delete(Product product)
        {
            try
            {
                var context = new SalesManagementContext();
                context.Products.Remove(product);
                context.SaveChanges();
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //GetProductNameById(orderDetail.ProductId)
        public string GetProductNameById(int productId)
        {
            string productName = "";
            try
            {
                var context = new SalesManagementContext();
                productName = context.Products.Where(p => p.ProductId.Equals(productId)).Select(p => p.ProductName).FirstOrDefault();
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return productName;
        }

        public IEnumerable<int> GetAllCategoryId()
        {
            IEnumerable<int> categories = new List<int>();
            try
            {
                var context = new SalesManagementContext();
                categories = context.Products.Select(p => p.CategoryId).Distinct();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return categories;
        }

        //ProductDAO.Instance.GetProductById(id);
        public Product GetProductById(int id)
        {
            Product product = null;
            try
            {
                var context = new SalesManagementContext();
                product = context.Products.FirstOrDefault(p => p.ProductId.Equals(id));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return product;
        }

    }



}

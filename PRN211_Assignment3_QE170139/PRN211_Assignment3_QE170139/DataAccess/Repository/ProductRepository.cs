﻿using BusinessObject.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ProductRepository : IProductRepository
    {
        public void Create(Product product) => ProductDAO.Instance.insert(product);

        public void Delete(Product product) => ProductDAO.Instance.delete(product);

        public IEnumerable<Product> GetAllProducts() => ProductDAO.Instance.findAll();

        public void Update(Product product) => ProductDAO.Instance.update(product);

        public string GetProductNameById(int productId) => ProductDAO.Instance.GetProductNameById(productId);

        public IEnumerable<int> GetAllCategoryId()
        {
            return ProductDAO.Instance.GetAllCategoryId();
        }

        public Product GetProductById(int id)
        {
            //
            return ProductDAO.Instance.GetProductById(id);

        }
    }
}

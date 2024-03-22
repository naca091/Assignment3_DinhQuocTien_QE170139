using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using DataAccess.Repository;

namespace PresentationLayer.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: Products
        // GET: Products
        public async Task<IActionResult> Index(int? categoryId)
        {
            IEnumerable<Product> products = _productRepository.GetAllProducts();

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId);
            }

            ViewBag.CategoryIds = await GetAllCategoryIds(); // Pass category IDs to the view

            return View(products);
        }
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product =  _productRepository.GetProductById(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,CategoryId,ProductName,Weight,UnitPrice,UnitsInStock")] Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepository.Create(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product =  _productRepository.GetProductById(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,CategoryId,ProductName,Weight,UnitPrice,UnitsInStock")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _productRepository.Update(product);
                }
                catch (Exception)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product =  _productRepository.GetProductById(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product =  _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            _productRepository.Delete(product);
            return RedirectToAction(nameof(Index));
        }
        private async Task<IEnumerable<int>> GetAllCategoryIds()
        {
            return  _productRepository.GetAllCategoryId();
        }

        public async Task<IEnumerable<Product>> FilterProducts(int? categoryId)
        {
            IEnumerable<Product> products = _productRepository.GetAllProducts();

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId);
            }

            foreach (var product in products)
            {
                Console.WriteLine(product.ProductName);
            }

            return products;
        }


    }
}

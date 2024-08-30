using ABC_Retailers.Models;
using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ABC_Retailers.Controllers
{
    public class ProductController : Controller
    {
         
        private readonly TableStorageService _tableStorageService;

        public ProductController(TableStorageService tableStorageService)
        {
            _tableStorageService = tableStorageService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _tableStorageService.GetAllProductsAsync();
            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            product.PartitionKey = "Product";
            product.RowKey = Guid.NewGuid().ToString();
            await _tableStorageService.AddProductAsync(product);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Create(Product product)
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string partitionKey, string rowKey)
        {
            // await _tableStorageService.DeleteProductAsync(partitionKey, rowKey);
            //return RedirectToAction(nameof(Index));
            string key = partitionKey;
            return View();
        }

        public async Task<IActionResult> Deletejj(Product product)
        {

            return View();
        }
    }
}


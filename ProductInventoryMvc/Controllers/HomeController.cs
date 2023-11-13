using Microsoft.AspNetCore.Mvc;
using ProductInventoryMvc.Models;
using ProductInventoryMvc.Models.Dto;
using ProductInventoryMvc.Services;
using System.Diagnostics;

namespace ProductInventoryMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IInventoryService _service;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IInventoryService service)
        {
            _logger = logger;
            _service = service;
        }

        public IActionResult Inventories()
        {
            return View("Inventories", _service.GetInventories());
        }

        public IActionResult CreateInventory()
        {
            return View("CreateInventory");
        }

        public IActionResult CreateProduct(InventoryDto inventory)
        {
            return View("CreateProduct", inventory);
        }

        public IActionResult EditProduct(DefineProductDto product)
        {
            return View("EditProduct", product);
        }

        public IActionResult ProductList(InventoryDto inventory)
        { 
            return View("Products", _service.GetProducts(inventory));
        }

        [HttpPost]
        public IActionResult CreateInventory([FromForm] InventoryDto inventory)
        {
            _service.AddInventory(inventory);
            return RedirectToAction("Inventories");
        }

        [HttpPost]
        public IActionResult CreateProduct([FromForm] ProductDto product)
        {
            _service.AddProduct(product.InventoryName, product);
            return RedirectToAction("ProductList", new InventoryDto { Name = product.InventoryName });
        }

        [HttpPost]
        public IActionResult EditProduct([FromForm] ProductDto product)
        {
            _service.EditProduct(product);
            return RedirectToAction("ProductList", new InventoryDto { Name = product.InventoryName });
        }

        [HttpPost]
        public IActionResult DeleteProduct([FromForm] DefineProductDto product)
        {
            _service.DeleteProduct(product.InventoryName, product);
            return RedirectToAction("ProductList", new InventoryDto { Name = product.InventoryName });
        }

        public IActionResult DeleteInventory(InventoryDto inventory)
        {
            _service.DeleteInventory(inventory);
            return RedirectToAction("Inventories");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
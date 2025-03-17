using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsWebAPI.Models;

namespace MyApi.Controllers
{
    [Route("api/[controller]")]  // API route: /api/products
    [ApiController]
    [Authorize]  // 🔹 Requires JWT authentication
    public class ProductsController : ControllerBase
    {
        // Sample products list
        private static readonly List<Product> Products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Price = 1000 },
            new Product { Id = 2, Name = "Phone", Price = 500 },
            new Product { Id = 3, Name = "Tablet", Price = 300 }
        };

        // 🔹 GET /api/products
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            return Ok(Products);
        }

        // 🔹 GET /api/products/{id}
        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            var product = Products.Find(p => p.Id == id);
            if (product == null)
                return NotFound(new { message = "Product not found" });

            return Ok(product);
        }

        // 🔹 POST /api/products (Add new product)
        [HttpPost]
        public ActionResult<Product> AddProduct(Product newProduct)
        {
            if (newProduct == null)
                return BadRequest(new { message = "Invalid product data" });

            newProduct.Id = Products.Count + 1;
            Products.Add(newProduct);
            return CreatedAtAction(nameof(GetProduct), new { id = newProduct.Id }, newProduct);
        }

        // 🔹 PUT /api/products/{id} (Update product)
        [HttpPut("{id}")]
        public ActionResult UpdateProduct(int id, Product updatedProduct)
        {
            var product = Products.Find(p => p.Id == id);
            if (product == null)
                return NotFound(new { message = "Product not found" });

            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;
            return NoContent();  // 204 No Content
        }

        // 🔹 DELETE /api/products/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteProduct(int id)
        {
            var product = Products.Find(p => p.Id == id);
            if (product == null)
                return NotFound(new { message = "Product not found" });

            Products.Remove(product);
            return NoContent();  // 204 No Content
        }
    }
}
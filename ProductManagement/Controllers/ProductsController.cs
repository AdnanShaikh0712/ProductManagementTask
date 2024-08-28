using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Data;
using ProductManagement.Models;

namespace ProductManagement.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public ProductsController(ApplicationDbContext context,IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        public IActionResult Index()
        {
            var products = context.Products.OrderByDescending(p=>p.Id).ToList();
            return View(products);
        }

        public IActionResult Create() 
        {
        
        return View();
        }

        [HttpPost]
        public IActionResult Create(ProductsDTO productsDTO)
        {
            if (productsDTO.ImageUrl == null) 
            {
                ModelState.AddModelError("ImageUrl", "The Image file is required");

            }
            if (!ModelState.IsValid)
            {
                return View(productsDTO);
            }

            //save the image
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(productsDTO.ImageUrl!.FileName);
            string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                productsDTO.ImageUrl.CopyTo(stream);
            }

            // save the new product in the database
            Product product = new Product()
            {
                Name = productsDTO.Name,
                Price = productsDTO.Price,
                Description = productsDTO.Description,
                ImageUrl = newFileName
            };

            context.Products.Add(product);
            context.SaveChanges();
                TempData["Success"] = "Product created successfully";
            return RedirectToAction("Index","Products");
        }

        public IActionResult Edit(int id) 
        {
            var product = context.Products.Find(id);

            if (product == null) {
                return RedirectToAction("Index", "Products");
            }

            //create productDTo from products

            var productDto = new ProductsDTO()
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,

            };

            ViewData["ProductId"] = product.Id;
            ViewData["ImageFileName"] = product.ImageUrl;

            return View(productDto);
        }
        [HttpPost]
        public IActionResult Edit(int id,ProductsDTO productsDTO) 
        {
            var product = context.Products.Find(id);

            if(product== null) 
            {
                return RedirectToAction("Index", "Products");
            }

            if (!ModelState.IsValid) 
            {
                ViewData["ProductId"] = product.Id;
                ViewData["ImageFileName"] = product.ImageUrl;

                return View(productsDTO);
            }

            //update the image file if we have a new file
            string newFileName = product.ImageUrl;
            if (productsDTO.ImageUrl != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(productsDTO.ImageUrl.FileName);

                string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath)) 
                { 
                  productsDTO.ImageUrl.CopyTo(stream);
                }

                //delete the old image
                string oldImagePath = environment.WebRootPath + "/products/" + product.ImageUrl;
                System.IO.File.Delete(oldImagePath);
            }

            //update the product in the database

            product.Name = productsDTO.Name;
            product.Description = productsDTO.Description;
            product.Price = productsDTO.Price;
            product.ImageUrl = newFileName;

            context.SaveChanges();

            return RedirectToAction("Index", "Products");
        }
        [HttpGet("EmployeeId")]
        public IActionResult Details(int id)
        {
            if (id <= 0)
            {
                return View("not found");
            }

            Product product = context.Products.FirstOrDefault(s => s.Id == id);

            if (id == null)
            {
                return View("notfound");
            }
            ViewData["ProductId"] = product.Id;
            ViewData["ImageFileName"] = product.ImageUrl;
            return View(product);
        }

        public IActionResult Delete(int id)
        {

            var product = context.Products.Find(id);
            if (product == null) 
            {
                return RedirectToAction("Index", "Products");
            }

            string imageFullPath = environment.WebRootPath + "/products/" + product.ImageUrl;
            System.IO.File.Delete(imageFullPath);

            context.Products.Remove(product);
            context.SaveChanges(true);

            return RedirectToAction("Index", "Products");
        }

    }
}

using DotNetCoreCrud.Web.DataAccessLayer;
using DotNetCoreCrud.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DotNetCoreCrud.Web.Controllers
{
    public class ProductController : Controller
    {
        private static string _connectionString;
        ProductData productData;

        public ProductController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            productData = new(_connectionString);
        }

        private List<SelectListItem> GetProductCategoryTypeList()
        {
            var productTypes = productData.GetProductTypes();

            List<SelectListItem> selectList = new List<SelectListItem>();
            foreach (var type in productTypes)
            {
                selectList.Add(new SelectListItem()
                {
                    Text = type.TypeName,
                    Value = type.Id.ToString()
                });
            }
            return selectList;
        }
        public IActionResult Index(string search)
        {
            var products = productData.GetAllProducts();

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p =>
                p.ProductName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                p.ProductCategory.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            return View(products);
        }
    
        [HttpGet]
        public IActionResult Create()
        {
           
            ViewBag.ProductCategories = GetProductCategoryTypeList();
            return View();
        }

        [HttpPost]
       
        public IActionResult Create(Product product)
        {
            
            if (ModelState.IsValid)
            {
                
                productData.AddProduct(product);
                return RedirectToAction("Index");
            }
            ViewBag.ProductCategories = GetProductCategoryTypeList();
            return View(product);
        }

        [HttpGet]
        public IActionResult Update(int id)
        {

            var product = productData.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.ProductCategories = GetProductCategoryTypeList();
            return View(product);
        }

        [HttpPost]
        public IActionResult Update(int id, Product product)
        {
            if (ModelState.IsValid)
            {
                product.Id = id;
                productData.UpdateProduct(product);
                return RedirectToAction("Index");
            }
            ViewBag.ProductCategories = GetProductCategoryTypeList();
            return View(product);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = productData.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                productData.DeleteProduct(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }
    }
}

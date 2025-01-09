using DotNetCoreCrud.Web.DataAccessLayer;
using DotNetCoreCrud.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DotNetCoreCrud.Web.Controllers
{
    public class FruitController : Controller
    {
        private static string _connectionString;
        FruitData fruitData;

        public FruitController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            fruitData = new FruitData(_connectionString);
        }

        private List<SelectListItem> GetFruitCategoryList()
        {
            var categories = fruitData.GetFruitCategories();
            List<SelectListItem> selectList = new List<SelectListItem>();

            foreach (var category in categories)
            {
                selectList.Add(new SelectListItem()
                {
                    Text = category.TypeName,
                    Value = category.Id.ToString()
                });
            }
            return selectList;
        }

        public IActionResult Index(string search, int pageNumber = 1, int pageSize = 10)
        {
            var fruits = fruitData.GetAllFruits(pageNumber, pageSize, search);

            var totalFruits = fruitData.GetTotalFruitCount(search);
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalFruits / pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.Search = search;

            return View(fruits);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.FruitCategories = GetFruitCategoryList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Fruit fruit)
        {
            if (ModelState.IsValid)
            {
                fruitData.AddFruit(fruit);
                return RedirectToAction("Index");
            }

            ViewBag.FruitCategories = GetFruitCategoryList();
            return View(fruit);
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var fruit = fruitData.GetFruitById(id);
            if (fruit == null)
            {
                return NotFound();
            }

            ViewBag.FruitCategories = GetFruitCategoryList();
            return View(fruit);
        }

        [HttpPost]
        public IActionResult Update(int id, Fruit fruit)
        {
            if (ModelState.IsValid)
            {
                fruit.FruitId = id;
                fruitData.UpdateFruit(fruit);
                return RedirectToAction("Index");
            }

            ViewBag.FruitCategories = GetFruitCategoryList();
            return View(fruit);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var fruit = fruitData.GetFruitById(id);
            if (fruit == null)
            {
                return NotFound();
            }

            return View(fruit);
        }

        [HttpPost]
        public IActionResult DeleteFruit(int id)
        {
            try
            {
                fruitData.DeleteFruit(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }
    }
}

using DotNetCoreCrud.Web.DataAccessLayer;
using DotNetCoreCrud.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DotNetCoreCrud.Web.Controllers
{
    public class DishController : Controller
    {
        private static string _connectionString;
        DishData dishData;

        public DishController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            dishData = new (_connectionString);
        }
        private List<SelectListItem> GetDishCategoryTypeList()
        {
            var dishCategories = dishData.GetDishTypes();

            List<SelectListItem> selectList = new List<SelectListItem>();
            foreach (var type in dishCategories)
            {
                selectList.Add(new SelectListItem()
                {
                    Text = type.TypeName,
                    Value = type.Id.ToString()
                });
            }
            return selectList;
        }
        public IActionResult Index(string search, int pageNumber = 1, int pageSize = 10)
        {
            var dishes = dishData.GetAllDishes(pageNumber, pageSize, search);

            var totalDishes = dishData.GetTotalDishCount(search);
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalDishes / pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.Search = search;

            return View(dishes);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.DishCategories = GetDishCategoryTypeList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Dish dish)
        {
            if (ModelState.IsValid)
            {
                dishData.AddDish(dish);
                return RedirectToAction("Index");
            }

            ViewBag.DishCategories = GetDishCategoryTypeList();
            return View(dish);
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            var dish = dishData.GetDishById(id);
            if (dish == null)
            {
                return NotFound();
            }

            ViewBag.DishCategories = GetDishCategoryTypeList();
            return View(dish);
        }
        [HttpPost]
        public IActionResult Update(int id, Dish dish)
        {
            if (ModelState.IsValid)
            {
                dish.Id = id;
                dishData.UpdateDish(dish);
                return RedirectToAction("Index");
            }

            ViewBag.DishCategories = GetDishCategoryTypeList();
            return View(dish);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var dish = dishData.GetDishById(id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }
        [HttpPost]
        public IActionResult DeleteDish(int id)
        {
            try
            {
                dishData.DeleteDish(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }

        }
    }
}
    


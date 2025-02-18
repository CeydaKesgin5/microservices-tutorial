using Microsoft.AspNetCore.Mvc;
using Product.Application.Models.ViewModels;

namespace Product.Application.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(CreateProductVM model)
        {
            return View();
        }



    }
}

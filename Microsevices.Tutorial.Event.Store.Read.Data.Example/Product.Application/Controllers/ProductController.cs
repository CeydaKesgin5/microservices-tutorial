using Microsoft.AspNetCore.Mvc;
using Product.Application.Models.ViewModels;
using Shared.Events;
using Shared.Services.Abstractions;

namespace Product.Application.Controllers
{
    public class ProductController(IEventStoreService eventStoreService) : Controller
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
        public async Task<IActionResult> CreateProduct(CreateProductVM model)
        {
            NewProductAddedEvent newProductAddedEvent = new()
            {
                ProductId=Guid.NewGuid().ToString(),
                ProductName=model.ProductName,
                InitialCount=model.Count,
                InitialPrice=model.Price,
                IsAvailable=model.IsAvailable
            };

            await eventStoreService.AppendToStreamAsync("product-stream", new[]//buradaki streame karşılık eventStore a göndermiş olduk
            { eventStoreService.GenerateEventData(newProductAddedEvent)}//bu eventi 
            );

            return RedirectToAction("Index");
        }



    }
}

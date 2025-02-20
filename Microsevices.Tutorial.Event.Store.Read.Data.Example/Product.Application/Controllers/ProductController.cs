using Microsoft.AspNetCore.Mvc;
using Product.Application.Models.ViewModels;
using Shared.Events;
using Shared.Services.Abstractions;

namespace Product.Application.Controllers
{
    public class ProductController(IEventStoreService eventStoreService, IMongoDBService mongoDBService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var productCollection = mongoDBService.GetCollection<Shared.Models.Product>("Products");
            var products = await(await productCollection.FindAsync(_ => true).ToListAsync());


            return View();
        }
         
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM model)
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


        public async  Task<IActionResult> Edit(string productId)
        {
            var productCollection = mongoDBService.GetCollection<Shared.Models.Product>("Products");
            var product = await(await productCollection.FindAsync(p=>p.Id ==productId)).FirstOrDefaultAsync();

            return View(product);

        }

    }
}

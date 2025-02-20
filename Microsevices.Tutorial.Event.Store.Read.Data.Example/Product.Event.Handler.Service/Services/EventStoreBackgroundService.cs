using Shared.Events;
using Shared.Services.Abstractions;
using System.Reflection;
using System.Text.Json;

namespace Product.Event.Handler.Service.Services
{
    public class EventStoreBackgroundService(IEventStoreService eventStoreService, IMongoDBService mongoDBService) : BackgroundService
    {
        //eventle ilgili tür çalışması tamamlanmış oldu.
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await eventStoreService.SubscribeToStreamAsync("products-stream", async
                (streamSubscription, resolvedEvent, cancellationToken) =>
                {
                    string eventType = resolvedEvent.Event.EventType;
                    object @event = JsonSerializer.Deserialize(resolvedEvent.Event.Data.ToArray(),
                        Assembly.Load("Shared").GetTypes().FirstOrDefault(t => t.Name == eventType));
                    //Shared içerisindeki eventType2 a karşılık olan tür hangisiyse buradaki binarydatayı o türden instance'a dönüştür.

                    var productCollection = mongoDBService.GetCollection<Shared.Models.Product>("Products");
                    //event tür kontrolü

                    switch (@event)
                    {
                        case NewProductAddedEvent e:
                            var hasProduct = await (await productCollection.FindAsync(p => p.Id.ToString() == e.ProductId)).AnyAsync();
                            if (!hasProduct)//ürün daha önce veritabanına eklenmemişse
                                await productCollection.InsertOneAsync(new()
                                {
                                    Id = e.ProductId,
                                    ProductName = e.ProductName,
                                    Count = e.InitialCount,
                                    IsAvailable = e.IsAvailable,
                                    Price = e.InitialPrice
                                });
                            break;
                    }
                }

                );
        }
    }
}

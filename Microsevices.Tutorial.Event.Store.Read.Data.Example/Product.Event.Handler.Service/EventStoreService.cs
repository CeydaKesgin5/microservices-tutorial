using EventStore.Client;
using Shared.Events;
using Shared.Services.Abstractions;
using System.Reflection;
using System.Text.Json;

namespace Product.Event.Handler.Service
{
    public class EventStoreService(IEventStoreService eventStoreService, IMongoDBService mongoDBService) : BackgroundService
    {
        //eventle ilgili tür çalışması tamamalnmış oldu.
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await eventStoreService.SubscribeToStreamAsync("products-stream", async
                (streamSubscription, resolvedEvent,cancellationToken)=>
                {
                    string eventType= resolvedEvent.Event.EventType;
                    object @event =JsonSerializer.Deserialize(resolvedEvent.Event.Data.ToArray(),
                        Assembly.Load("Shared").GetTypes().FirstOrDefault(t=>t.Name==eventType));
                    //Shared içerisindeki eventType2 a karşılık olan tür hangisiyse buradaki binarydatayı o türden instance'a dönüştür.

                    var productCollection = mongoDBService.GetCollection<Models.Product>("Products");
                    //event tür kontrolü

                    switch (@event)
                    {
                        case NewProductAddedEvent e:
                            break;
                    }
                }

                );
        }
    }
}

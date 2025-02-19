using EventStore.Client;

namespace Shared.Services.Abstractions
{
    public interface IEventStoreService
    {
        //Bir eventin EventStore a eklenebilmesi için fonksiyon tanımı
        //Subscribe işlemlerini gerçekleştirecek fonksiyonların tanımları

        Task AppendToStreamAsync(string streamName, IEnumerable<EventData> eventData);//Bu fonksiyon üzerinden evnetler EventStore a gönderilecek.
        EventData GenerateEventData(object @event);//EvenData oluşturacak method


        //EventStore'daki ilgili sunucudaki seçili streamlere subscribe olur.
        // subscribe esnasında gelen eventi yakalayacak olan method
        Task SubscribeToStreamAsync(string streamName, Func<StreamSubscription, ResolvedEvent, CancellationToken, Task>
            eventAppeared);
    }
}

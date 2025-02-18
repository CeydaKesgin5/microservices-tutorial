using EventStore.Client;
using Shared.Services.Abstractions;
using System.Text.Json;

namespace Shared.Services
{
    public class EventStoreService : IEventStoreService
    {
        EventStoreClientSettings GetEventStoreClientSettings(string connectionString = 
            "esdb://admin/changeit@localhost:2113?tls=false&tlsVerifyCert=false") =>
            EventStoreClientSettings.Create(connectionString);

        EventStoreClient Client { get => new(GetEventStoreClientSettings()); }
        public async Task AppendToStreamAsync(string streamName, IEnumerable<EventData> eventData)
            => await Client.AppendToStreamAsync(
                streamName:streamName, 
                eventData:eventData,
                expectedState: StreamState.Any
                );

        //{
        //    //EventStoreClientSettings settings = GetEventStoreClientSettings();
        //    //EventStoreClient client = new(settings);
        //}

        //data->eventdataya dönüşüm

        public EventData GenerateEventData(object @event)
        => new(
            eventId: Uuid.NewUuid(),
            type: @event.GetType().Name,
            data: JsonSerializer.SerializeToUtf8Bytes(@event)
            );
    }
}

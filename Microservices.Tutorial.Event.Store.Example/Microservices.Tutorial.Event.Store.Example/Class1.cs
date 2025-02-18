using EventStore.Client;
using System.Text.Json;
#region Giriş


//string connectionString="esdb://admin/changeit@localhost:2113?tls=false&tlsVerifyCert=false";
//var settings = EventStoreClientSettings.Create(connectionString);
//var client = new EventStoreClient(settings);

////event oluşuruldu
//OrderPlacedEvent orderPlacedEvent = new()
//{
//    OrderId = 1,
//    TotalAmount = 1000
//};

//EventData eventData = new(
//    eventId: Uuid.NewUuid(),
//    type: orderPlacedEvent.GetType().Name,
//    data: JsonSerializer.SerializeToUtf8Bytes(orderPlacedEvent)
//    );

////streame gönderildi.
//await client.AppendToStreamAsync(
//    streamName: "order-stream",
//    expectedState: StreamState.Any,
//    eventData: new[] {eventData}
//    );


//var events= client.ReadStreamAsync(
//    streamName: "order-stream",
//    direction: Direction.Forwards,//olayları baştan sona doğru okur.
//    revision: StreamPosition.Start //Streamdaki olayları kaçıncı eventten başlayarak okuycağını ifade eder.
//    );


//var datas= await events.ToListAsync();

//await client.SubscribeToStreamAsync(
//    streamName: "order-stream",
//    start: FromStream.Start,
//    //resolvedEvent: subscribe olacak eventi getirir.
//    eventAppeared: async (streamSubscription, resolvedEvent, cancellationToken) =>
//    {
//        OrderPlacedEvent @event = JsonSerializer.Deserialize<OrderPlacedEvent>(resolvedEvent.Event.Data.ToArray());
//        await Console.Out.WriteLineAsync(JsonSerializer.Serialize(@event));
//    },
//    subscriptionDropped: (streamSubscription, subscripitonDroppedReason, exception) => Console.WriteLine("Disconnection")
//    //bağlantı kopukluğu olduğunda tetklenecek fonksiyon

//    );

//Console.Read();
//class OrderPlacedEvent
//{
//    public int OrderId { get; set; }
//    public int TotalAmount { get; set; }
//}
#endregion

#region Bakiye

EventStoreService eventStoreService = new();

AccountCreatedEvent accountCreatedEvent = new()
{
    AccountId = "12345",
    CustomerId = "98765",
    StartBalance = 0,
    Date = DateTime.UtcNow.Date
};
MoneyDepositedEvent moneyDepositedEvent1 = new()
{
    AccountId="12345",
    Amount=1000,
    Date = DateTime.UtcNow.Date
};
MoneyDepositedEvent moneyDepositedEvent2= new()
{
    AccountId = "12345",
    Amount = 500,
    Date = DateTime.UtcNow.Date
};
MoneyTransferedEvent moneyTransferedEvent = new()
{
    AccountId = "12345",
    Amount = 200,
    Date = DateTime.UtcNow.Date
};
MoneyDepositedEvent moneyDepositedEvent3 = new()
{
    AccountId = "12345",
    Amount = 50,
    Date = DateTime.UtcNow.Date
};
MoneyTransferedEvent moneyTransferedEvent1 = new()
{
    AccountId = "12345",
    Amount = 250,
    Date = DateTime.UtcNow.Date
};
MoneyTransferedEvent moneyTransferedEvent2 = new()
{
    AccountId = "12345",
    Amount = 150,
    Date = DateTime.UtcNow.Date
};
MoneyDepositedEvent moneyDepositedEvent4 = new()
{
    AccountId = "12345",
    Amount = 2000,
    Date = DateTime.UtcNow.Date
};

//eventler EventStore'a gönderildi.
await eventStoreService.AsyncToStreamAsync(
    streamName: $"customer-{accountCreatedEvent.CustomerId}-stream",
    new[]
    {
        eventStoreService.GenerateEventData(accountCreatedEvent),
        eventStoreService.GenerateEventData(moneyDepositedEvent1),
        eventStoreService.GenerateEventData(moneyDepositedEvent2),
        eventStoreService.GenerateEventData(moneyTransferedEvent),
        eventStoreService.GenerateEventData(moneyDepositedEvent3),
        eventStoreService.GenerateEventData(moneyTransferedEvent1),

        eventStoreService.GenerateEventData(moneyTransferedEvent2),
        eventStoreService.GenerateEventData(moneyDepositedEvent4),
    }
    );

BalanceInfo balanceInfo = new();
//sonuç değeri eventler kümülatif olarak değerlendirilerek elde edildi.
await eventStoreService.SubscribeToStreamAsync(
    streamName: $"customer-{accountCreatedEvent.CustomerId}-stream",
    async (streamSubscription, resolvedEvent, cancellationToken )=>
    {
        string eventType= resolvedEvent.Event.EventType;
        object @event=JsonSerializer.Deserialize(resolvedEvent.Event.Data.ToArray(),Type.GetType(eventType));

        switch (@event)
        {
            case AccountCreatedEvent e:
                balanceInfo.AccountId = e.AccountId;
                balanceInfo.Balance = e.StartBalance;
                break;
            case MoneyDepositedEvent e:
                balanceInfo.Balance += e.Amount;
                break;
            case MoneyWithdrawnEvent e:
                balanceInfo.Balance -= e.Amount;
                break;
            case MoneyTransferedEvent e:
                balanceInfo.Balance -= e.Amount;
                break;
        }

        await Console.Out.WriteLineAsync("*******Balance*******");
        await Console.Out.WriteLineAsync(JsonSerializer.Serialize(balanceInfo));
        await Console.Out.WriteLineAsync("*******Balance*******");
        await Console.Out.WriteLineAsync("");
        await Console.Out.WriteLineAsync("");

    }
    
    );

Console.Read();
class EventStoreService
{
    EventStoreClientSettings GetEventStoreClientSettings(string connectionString = 
        "esdb://admin/changeit@localhost:2113?tls=false&tlsVerifyCert=false")
        => EventStoreClientSettings.Create(connectionString);

    EventStoreClient Client { get => new EventStoreClient(GetEventStoreClientSettings()); }

    public async Task AsyncToStreamAsync(string streamName, IEnumerable<EventData> eventData)
    => await Client.AppendToStreamAsync(
        streamName: streamName,
        eventData: eventData,
        expectedState: StreamState.Any
        );
    //GenerateEventData aracılığı ile eventi EventData formatına dönüştürdük.
    public EventData GenerateEventData(object @event) =>
        new(
            eventId: Uuid.NewUuid(),
            type: @event.GetType().Name,
            data: JsonSerializer.SerializeToUtf8Bytes( @event )
            );

     public async Task SubscribeToStreamAsync(string streamName, Func<StreamSubscription, ResolvedEvent, CancellationToken
        , Task> eventAppeared)
        => Client.SubscribeToStreamAsync(
            streamName: streamName,
            start: FromStream.Start,
            eventAppeared: eventAppeared,
            subscriptionDropped: (x, y, z) => Console.WriteLine("Disconnected")
            );


}

class BalanceInfo
{
    public string AccountId { get; set; }
    public int Balance { get; set; }
}
class AccountCreatedEvent
{
    public string AccountId { get; set; }
    public string CustomerId { get; set; }
    public int StartBalance { get; set; }
    public DateTime Date { get; set; }
}

class MoneyDepositedEvent
{
    public string AccountId { get; set; }
    public int Amount { get; set; }
    public DateTime Date { get; set; }
}

class MoneyWithdrawnEvent
{
    public string AccountId { get; set; }
    public int Amount { get; set; }
    public DateTime Date { get; set; }
}

class MoneyTransferedEvent
{
    public string AccountId { get; set; }
    public string TargetAccountId { get; set; }
    public int Amount { get; set; }
    public DateTime Date { get; set; }
}



#endregion
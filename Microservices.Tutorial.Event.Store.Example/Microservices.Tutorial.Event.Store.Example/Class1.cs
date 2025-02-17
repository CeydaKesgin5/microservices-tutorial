
#region Giriş
//using EventStore.Client;
//using System.Text.Json;

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

#endregion
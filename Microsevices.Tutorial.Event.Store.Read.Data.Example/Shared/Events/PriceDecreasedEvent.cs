namespace Shared.Events
{
    public class PriceDecreasedEvent
    {
        public string ProductId { get; set; }
        public int DecrementAmount { get; set; }
    }
}

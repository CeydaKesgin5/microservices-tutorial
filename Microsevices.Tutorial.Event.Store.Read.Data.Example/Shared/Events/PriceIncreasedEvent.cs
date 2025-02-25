namespace Shared.Events
{
    public class PriceIncreasedEvent
    {
        public string ProductId { get; set; }
        public int IncrementAmount { get; set; }
    }
}

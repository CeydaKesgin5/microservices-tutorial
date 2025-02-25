namespace Shared.Events
{
    public class AvailabilityChangeEvent
    {
        public string ProductId { get; set; }
        public bool IsAvailable { get; set; }
    }
}

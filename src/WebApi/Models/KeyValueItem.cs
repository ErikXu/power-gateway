namespace WebApi.Models
{
    public class KeyValueItem<TKey, TValue>
    {
        public TKey Key { get; set; }

        public TValue Value { get; set; }
    }
}

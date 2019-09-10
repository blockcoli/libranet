namespace Blockcoli.Libra.Net.Common
{
    public class EventHandle
    {
        public byte[] Key { get; private set; }
        public ulong Count { get; private set; }

        public EventHandle(byte[] key, ulong count)
        {
            this.Key = key;
            this.Count = count;
        }

        public static EventHandle GetDefault()
        {
            return new EventHandle(new byte[8], 0UL);
        }
    }
}
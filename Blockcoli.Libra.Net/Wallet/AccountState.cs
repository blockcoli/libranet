using Blockcoli.Libra.Net.Common;

namespace Blockcoli.Libra.Net.Wallet
{
    public class AccountState
    {
        public byte[] AuthenticationKey { get; private set; }
        public ulong Balance { get; private set; }
        public EventHandle ReceivedEvents { get; private set; }
        public EventHandle SentEvents { get; private set; }
        public ulong SequenceNumber { get; private set; }
        public bool DelegatedWithdrawalCapability { get; private set; }

        public static AccountState Default(string address)
        {
            return new AccountState(address.ToBytes(), 0, EventHandle.GetDefault(), EventHandle.GetDefault(), 0, true);
        }

        public static AccountState FromBytes(byte[] bytes)
        {
            var cursor = new CursorBuffer(bytes);

            var authenticationKeyLen = cursor.Read32();
            var authenticationKey = cursor.ReadXBytes((int)authenticationKeyLen);
            var balance = cursor.Read64();
            var delegatedWithdrawalCapability = cursor.ReadBool();
            var receivedEventsCount = cursor.Read64();
            var receivedEventsKeyLen = cursor.Read32();
            var receivedEventsKey = cursor.ReadXBytes((int)receivedEventsKeyLen);
            var sentEventsCount = cursor.Read64();
            var sentEventsKeyLen = cursor.Read32();
            var sentEventsKey = cursor.ReadXBytes((int)sentEventsKeyLen);
            var sequenceNumber = cursor.Read64();

            var receivedEvents = new EventHandle(receivedEventsKey,receivedEventsCount);
            var sentEvents = new EventHandle(sentEventsKey,sentEventsCount);

            return new AccountState(authenticationKey, balance, receivedEvents, sentEvents, sequenceNumber, delegatedWithdrawalCapability);
        }

        private AccountState(byte[] authenticationKey, ulong balance, EventHandle receivedEvents, EventHandle sentEvents, ulong sequenceNumber, bool delegatedWithdrawalCapability)
        {
            this.AuthenticationKey = authenticationKey;
            this.Balance = balance;
            this.ReceivedEvents = receivedEvents;
            this.SentEvents = sentEvents;
            this.SequenceNumber = sequenceNumber;
            this.DelegatedWithdrawalCapability = delegatedWithdrawalCapability;
        }
    }
}
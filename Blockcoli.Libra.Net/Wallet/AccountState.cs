using Blockcoli.Libra.Net.Common;

namespace Blockcoli.Libra.Net.Wallet
{
    public class AccountState
    {
        public byte[] AuthenticationKey { get; private set; }
        public ulong Balance { get; private set; }
        public ulong ReceivedEventsCount { get; private set; }
        public ulong SentEventsCount { get; private set; }
        public ulong SequenceNumber { get; private set; }
        public bool DelegatedWithdrawalCapability { get; private set; }

        public static AccountState Default(string address)
        {
            return new AccountState(address.ToBytes(), 0, 0, 0, 0, true);
        }

        public static AccountState FromBytes(byte[] bytes)
        {
            var cursor = new CursorBuffer(bytes);

            var authenticationKeyLen = cursor.Read32();
            var authenticationKey = cursor.ReadXBytes((int)authenticationKeyLen);
            var balance = cursor.Read64();
            var delegatedWithdrawalCapability = cursor.ReadBool();
            var receivedEventsCount = cursor.Read64();
            var sentEventsCount = cursor.Read64();
            var sequenceNumber = cursor.Read64();

            return new AccountState(authenticationKey, balance, receivedEventsCount, sentEventsCount, sequenceNumber, delegatedWithdrawalCapability);
        }

        private AccountState(byte[] authenticationKey, ulong balance, ulong receivedEventsCount, ulong sentEventsCount, ulong sequenceNumber, bool delegatedWithdrawalCapability)
        {
            this.AuthenticationKey = authenticationKey;
            this.Balance = balance;
            this.ReceivedEventsCount = receivedEventsCount;
            this.SentEventsCount = sentEventsCount;
            this.ReceivedEventsCount = receivedEventsCount;
            this.DelegatedWithdrawalCapability = delegatedWithdrawalCapability;
        }
    }
}
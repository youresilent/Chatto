using System;

namespace Chatto.DAL.Entities
{
    public class ClientPendingFriend
    {
        public int Id { get; set; }

        public Guid Id_Receiver { get; set; }
        public virtual ClientProfile ReceiverClientProfile { get; set; }

        public Guid Id_Sender { get; set; }
        public virtual ClientProfile SenderClientProfile { get; set; }
    }
}

using Core.Entities.Abstract;

namespace Entities.Concrete
{
    public class UserNotification:IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Data { get; set; }
        public string Sender { get; set; }
        public bool Readed { get; set; }

        public string IconUrl { get; set; }
        public string Header { get; set; }
        public string Message { get; set; }
    }
}
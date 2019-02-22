using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto.Notification
{
    public class NotificationSubscriptionDto
    {
        public string Endpoint { get; set; }
        public DateTime? ExpirationTime { get; set; }
        public Key Keys { get; set; } = new Key();
        public int Id { get; set; }
        public string AsString { get; set; }
    }
}

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
        public KeyDto Keys { get; set; } = new KeyDto();
        public int Id { get; set; }
        public string AsString { get; set; }
    }
}

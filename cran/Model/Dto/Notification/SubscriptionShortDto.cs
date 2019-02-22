using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto.Notification
{
    public class SubscriptionShortDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Endpoint { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto.Notification
{
    public class NotificationDto
    {
        public int SubscriptionId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }    
        public string Action {get;set;} 
        public string ActionTitle {get; set;} 
        public string ActionUrl {get;set;}                          
    }
}

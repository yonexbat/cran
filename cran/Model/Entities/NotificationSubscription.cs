using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Entities
{
    public class NotificationSubscription : CranEntity
    {
        public virtual int IdUser { get; set; }
        public virtual string Endpoint { get; set; }
        public virtual DateTime? ExpirationTime { get; set; }
        public virtual string P256DiffHell { get; set; }
        public virtual string Auth { get; set; }
        public virtual string AsString { get; set; }

        public virtual CranUser User { get; set; }
    }
}

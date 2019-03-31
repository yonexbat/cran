using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Entities
{
    public class CranUser : CranEntity
    {
        public virtual string UserId { get; set; }
        public virtual bool IsAnonymous { get; set; }
        public virtual IList<CourseInstance> CourseInstances { get; set; } = new List<CourseInstance>();
        public virtual IList<Question> Questions { get; set; } = new List<Question>();
        public virtual IList<Comment> Comments { get; set; } = new List<Comment>();
        public virtual IList<Rating> Ratings { get; set; } = new List<Rating>();
        public virtual IList<Binary> Binaries { get; set; } = new List<Binary>();
        public virtual IList<RelUserCourseFavorite> RelFavorites { get; set; } = new List<RelUserCourseFavorite>();
        public virtual IList<NotificationSubscription> Subscriptions { get; set; } = new List<NotificationSubscription>();
    }
}

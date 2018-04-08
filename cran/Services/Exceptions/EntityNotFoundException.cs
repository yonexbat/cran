using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Services.Exceptions
{
    public class EntityNotFoundException : CraniumException
    {
        public int Id { get; set; }
        public Type Type { get; set; }

        public EntityNotFoundException(int id, Type type) : base($"Entity {type?.Name} with {id} does not exist")            
        {
            Id = id;
            Type = type;
        }
    }
}

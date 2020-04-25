using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YugiohCollection.Models
{
    public class Entity
    {
        public Entity()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
    }
}

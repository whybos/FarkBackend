using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Footer : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
    }
}

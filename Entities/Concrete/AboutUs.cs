using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class AboutUs : IEntity
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public string Summary { get; set; }
        public string UpdateDate { get; set; }
    }
}
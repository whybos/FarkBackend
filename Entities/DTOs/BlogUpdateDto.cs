using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class BlogUpdateDto
    {
        public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string Speaker { get; set; }
    public string Date { get; set; }
}
}
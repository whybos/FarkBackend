using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class NewsUpdateDto
    {
        public int Id { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public string Photo { get; set; }
    public string Date { get; set; }
}
}

using Core.Entities;

namespace Entities.DTOs
{
    public class FarkindaUpdateDto : IDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public string Link { get; set; }
        public string Photo { get; set; }
    }
}

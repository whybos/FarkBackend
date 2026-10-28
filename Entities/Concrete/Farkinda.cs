using Core.Entities;

namespace Entities.Concrete
{
    public class Farkinda : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public string Link { get; set; }
        public string Photo { get; set; }
    }
}

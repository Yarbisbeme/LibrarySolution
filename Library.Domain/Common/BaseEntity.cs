
namespace Library.Domain.Common
{
    public class BaseEntity
    {
        public DateTime createdAt { get; set; } = DateTime.UtcNow;
        public DateTime updatedAt { get; set; }
    }
}
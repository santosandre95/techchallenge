namespace Core.Entities
{
    public class Contact : BaseEntity
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Ddd { get; set; }
    }
}

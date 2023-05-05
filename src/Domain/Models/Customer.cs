using Core.DomainObjects;

namespace Domain.Models
{
    public class Customer : Entity, IAggregateRoot
    {
        public string Name { get; private set; }

        public string Email { get; private set; }

        public DateTime BirthDate { get; private set; }

        // EF Relation
        public virtual ICollection<Product> Products { get; } = new List<Product>();

        // Empty constructor for EF
        protected Customer() { }

        public Customer(string name, string email, DateTime birthDate)
        {
            Name = name;
            Email = email;
            BirthDate = birthDate;
        }
        
    }
}

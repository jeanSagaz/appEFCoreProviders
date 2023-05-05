using Core.DomainObjects;

namespace Domain.Models
{
    public class Product : Entity, IAggregateRoot
    {
        public string Name { get; private set; }

        public bool Active { get; private set; }

        public Guid? CategoryId { get; private set; }

        // EF Relation

        public virtual ICollection<Customer> Customers { get; } = new List<Customer>();
        public Category Category { get; set; }

        // Empty constructor for EF
        protected Product() { }

        public Product(string name, Guid? categoryId = null)
        {
            Name = name;
            CategoryId = categoryId;
            Active = true;
        }        

        internal void Enabled() { Active = true; }

        internal void Disabled() { Active = false; }
    }
}

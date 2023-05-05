using Core.DomainObjects;

namespace Domain.Models
{
    public class Category : Entity, IAggregateRoot
    {
        public string Name { get; private set; }

        public bool Active { get; private set; }

        // EF Relation
        public virtual ICollection<Product> Products { get; } = new List<Product>();

        // Empty constructor for EF
        protected Category() { }

        public Category(string name)
        {
            Name = name;
            Active = true;
        }

        internal void Enabled() { Active = true; }

        internal void Disabled() { Active = false; }
    }
}

using Core.DomainObjects;

namespace Domain.Models
{
    public class State : Entity, IAggregateRoot
    {
        public string Name { get; private set; }

        public Governor Governor { get; private set; }

        // Empty constructor for EF
        protected State() { }

        public State(string name)
        {
            this.Name = name;
        }
    }
}

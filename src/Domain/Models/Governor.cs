using Core.DomainObjects;

namespace Domain.Models
{
    public class Governor : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public int Age { get; private set; }

        public string PoliticalParty { get; private set; }

        // EF Relation
        public Guid StateId { get; private set; }

        public State State { get; private set; }

        // Empty constructor for EF
        protected Governor() { }

        public Governor(string name,
            int age,
            string politicalParty,
            Guid stateId)
        {
            Name = name;
            Age = age;
            PoliticalParty = politicalParty;
            StateId = stateId;
        }
    }
}

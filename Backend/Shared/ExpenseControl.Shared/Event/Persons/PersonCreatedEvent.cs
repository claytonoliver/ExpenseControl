namespace ExpenseControl.Shared.Event.Persons
{
    internal class PersonCreatedEvent
    {
        public Guid Id { get; }
        public string Name { get; }
        public int Age { get; }

        public PersonCreatedEvent(Guid id, string name, int age)
        {
            Id = id;
            Name = name;
            Age = age;
        }
    }
}

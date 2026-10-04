using Lab2.EducationalPrograms.Common;

namespace Lab2.EducationalPrograms.Users;

public sealed record User : IEntity
{
    public User(Guid id, string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Id = id;
        Name = name;
    }

    public Guid Id { get; }

    public string Name { get; }
}

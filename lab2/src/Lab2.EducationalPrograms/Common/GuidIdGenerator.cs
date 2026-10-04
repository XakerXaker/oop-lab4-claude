namespace Lab2.EducationalPrograms.Common;

public sealed class GuidIdGenerator : IIdGenerator
{
    public Guid Next() => Guid.NewGuid();
}

namespace Lab2.EducationalPrograms.LabWorks;

public sealed record EvaluationCriterion
{
    public EvaluationCriterion(string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        Description = description;
    }

    public string Description { get; }
}

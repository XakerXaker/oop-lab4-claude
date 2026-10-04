namespace Lab2.EducationalPrograms.Subjects;

/// <summary>
/// Final assessment of a subject: exam or credit.
/// </summary>
public abstract record AssessmentFormat
{
    private AssessmentFormat() { }

    /// <summary>
    /// Points this format adds to the subject total.
    /// </summary>
    public abstract int Points { get; }

    /// <returns>Error message or <c>null</c> if the format is valid.</returns>
    public abstract string? Validate();

    public sealed record Exam(int ExamPoints) : AssessmentFormat
    {
        public override int Points => ExamPoints;

        public override string? Validate()
        {
            return ExamPoints > 0 ? null : $"Exam points must be positive, got {ExamPoints}";
        }
    }

    public sealed record Credit(int MinimumPoints) : AssessmentFormat
    {
        public override int Points => 0;

        public override string? Validate()
        {
            return MinimumPoints is > 0 and <= Subject.RequiredTotalPoints
                ? null
                : $"Minimum points for credit must be in range 1..{Subject.RequiredTotalPoints}, got {MinimumPoints}";
        }
    }
}

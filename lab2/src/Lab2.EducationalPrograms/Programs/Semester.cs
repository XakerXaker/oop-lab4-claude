using Lab2.EducationalPrograms.Subjects;

namespace Lab2.EducationalPrograms.Programs;

public sealed record Semester(int Number, IReadOnlyList<Subject> Subjects);

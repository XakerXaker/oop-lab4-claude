using Lab2.EducationalPrograms.Common;

namespace Lab2.EducationalPrograms.Repositories;

public interface IRepository<T>
    where T : class, IEntity
{
    /// <exception cref="InvalidOperationException">An entity with the same id is already stored.</exception>
    void Add(T entity);

    T? FindById(Guid id);

    IReadOnlyCollection<T> GetAll();
}

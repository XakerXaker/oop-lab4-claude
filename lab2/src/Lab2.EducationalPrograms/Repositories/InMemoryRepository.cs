using Lab2.EducationalPrograms.Common;

namespace Lab2.EducationalPrograms.Repositories;

public sealed class InMemoryRepository<T> : IRepository<T>
    where T : class, IEntity
{
    private readonly Dictionary<Guid, T> _entities = [];

    public void Add(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (!_entities.TryAdd(entity.Id, entity))
            throw new InvalidOperationException($"{typeof(T).Name} with id {entity.Id} already exists");
    }

    public T? FindById(Guid id) => _entities.GetValueOrDefault(id);

    public IReadOnlyCollection<T> GetAll() => _entities.Values;
}

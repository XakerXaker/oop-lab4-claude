# Лабораторная 2 — конструктор образовательных программ

C# / .NET 10. Доменная модель без UI, покрыта тестами (xUnit).

```bash
cd lab2
dotnet test
```

## Модель

| Сущность | Что хранит | Что можно менять (только автору) |
|---|---|---|
| `User` | Id, имя | — |
| `LabWork` | Id, название, описание, критерии оценивания, баллы, автор, `BasedOnId` | название, описание, критерии; баллы — нет |
| `LectureMaterial` | Id, название, краткое описание, контент, автор, `BasedOnId` | название, описание, контент |
| `Subject` | Id, название, лабораторные, лекции, формат (`Exam(баллы)` / `Credit(минимум)`), автор, `BasedOnId` | название, лекции, минимум для зачёта; лабораторные и баллы за экзамен — нет |
| `EducationalProgram` | Id, название, руководитель, семестры с предметами | — |

- Изменения не автором возвращают `EditResult.NotAuthor`, состояние сущности при этом не меняется.
- Предмет создаётся только через `SubjectBuilder` (конструктор `internal`). `Build()` возвращает ошибку,
  если сумма баллов лабораторных и экзамена не равна 100. После создания сумма поменяться не может:
  нет методов для изменения лабораторных и баллов за экзамен.
- Репозитории: `IRepository<T>` и `InMemoryRepository<T>` на словаре — добавление и поиск по Id.

## Порождающие паттерны

| Где | Паттерн | Зачем |
|---|---|---|
| Лабораторная на основе существующей | **Prototype** (`IPrototype<T>`, `LabWork.Clone`) | Копия создаётся самим объектом, без раскрытия его внутренностей; копия получает новый Id, нового автора и `BasedOnId` |
| Предмет на основе существующего | **Builder** (`SubjectBuilder.BasedOn`) | Строитель заполняется данными исходного предмета, их можно поменять перед сборкой, а `Build()` заново проверяет правило 100 баллов |
| Лекционные материалы на основе существующих, создание всех сущностей | **Abstract Factory** (`IEducationalEntityFactory`, `AuthoredEntityFactory`) | Фабрика создаёт всё семейство сущностей для одного автора, которого задают один раз при создании фабрики, а не при каждой сборке |
| Образовательная программа | **Builder** (`EducationalProgramBuilder`) | Пошаговое добавление предметов по семестрам и проверка при сборке |

```csharp
IEducationalEntityFactory factory = new AuthoredEntityFactory(author, new GuidIdGenerator());

LabWork lab = factory.CreateLabWork("Lab 1", "...", [new EvaluationCriterion("Tests pass")], points: 60);

BuildResult<Subject> result = factory.CreateSubjectBuilder()
    .WithName("OOP")
    .AddLabWork(lab)
    .WithExam(40)          // 60 + 40 = 100
    .Build();

LabWork copy = otherFactory.CreateLabWorkBasedOn(lab);   // copy.BasedOnId == lab.Id
```

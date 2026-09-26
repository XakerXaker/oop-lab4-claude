# Лабораторная 4 — менеджер файловой системы

Консольное приложение для навигации и управления файловой системой. C# / .NET 10, без сторонних библиотек
(xUnit используется только в тестах).

## Запуск

```bash
dotnet run --project src/Lab4.ConsoleApp
dotnet test
```

```
> connect /home/user
[local /home/user:/]> tree goto docs
[local /home/user:/docs]> tree list -d 2
[local /home/user:/docs]> file show ../notes.txt -m console
[local /home/user:/docs]> file copy /notes.txt .
[local /home/user:/docs]> file rename "notes.txt" todo.txt
[local /home/user:/docs]> connect D:\          # переключение на другую ФС / диск
[local D:\:/]> disconnect
```

Команды: `connect [Address] [-m Mode]`, `disconnect`, `tree goto [Path]`, `tree list [-d Depth]`,
`file show [Path] -m Mode`, `file move|copy [Source] [DestinationDir]`, `file delete [Path]`,
`file rename [Path] [Name]`, а также `help` и `exit` в оболочке. Пути с пробелами берутся в кавычки.

## Структура

| Проект | Назначение |
|---|---|
| `Lab4.Core` | Доменная логика: пути, абстракция ФС, ленивые узлы, отрисовка дерева, вывод файлов, операции над файлами, сессия |
| `Lab4.Local` | Реализация `IFileSystem` для локального диска |
| `Lab4.Commands` | Команды, парсер команд, процессор (текст → команда → результат) |
| `Lab4.ConsoleApp` | Composition root и REPL — единственное место, завязанное на консоль |
| `Lab4.Tests` | Тесты парсера, путей, операций, отрисовки, выполнения команд на in-memory ФС |

## Как выполнены нефункциональные требования

- **Парсинг расширяем по OCP.** `ICommandParserLink` — цепочка обязанностей по командам
  (`GroupParserLink` для `tree`/`file`, `CommandParserLink<TBuilder>` для конечных команд).
  Флаги обрабатываются отдельной цепочкой `IFlagHandler<TBuilder>`; новый флаг — это новый класс-обработчик,
  добавленный в цепочку в `CommandParserFactory`, существующий код парсинга не меняется.
  Команды собираются строителями (`*CommandBuilder`), которые проверяют обязательные параметры.
- **Логика не привязана к консоли.** Команды выполняются через `ICommandContext`, вывод идёт в `IOutput`;
  `CommandProcessor` возвращает `OperationResult`, а печатает его только `ConsoleShell`.
- **Логика не привязана к локальной ФС.** Всё работает через `IFileSystem`/`IReadOnlyFileSystem`;
  реализации регистрируются по режиму (`ModeRegistry<IFileSystemFactory>`, `-m local`).
  Тесты гоняют ту же логику на `InMemoryFileSystem`.
- **Локальный путь не выходит за путь подключения.** `VirtualPath` всегда нормализован и относителен корню
  подключения; `PathResolver` обрабатывает `.`/`..`, абсолютные (`/…` — от пути подключения) и относительные пути.
- **Состояние подключения** — паттерн State (`Session`, `DisconnectedState`, `ConnectedState`): в отключённом
  состоянии доступно только `connect`; повторный `connect` переключает ФС (например, диск C → D).
- **Ленивость и память.** `DirectoryNode.Children` загружает детей по одному при каждом перечислении
  (Composite + Visitor для отрисовки). `TreeRenderer` держит в памяти только текущую ветку
  (один перечислитель с просмотром на один элемент вперёд на уровень). Файлы выводятся потоково, блоками по 4 КБ.
- **Параметризуемое дерево.** `TreeRenderOptions` (+ `TreeRenderOptionsBuilder`) задаёт символы папки,
  файла, ветвей и отступов.
- **Коллизии имён** — стратегия `INameCollisionStrategy`: при копировании/перемещении по умолчанию
  подбирается имя `name (N).ext`, при переименовании — ошибка. ФС никогда не перезаписывает файлы.
- **Несуществующие пути и ошибки ввода-вывода** преобразуются в `FileSystemException` и выводятся
  как сообщение об ошибке, после чего программа ждёт следующую команду.

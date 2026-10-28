# Просмотр клиентов фитнес-клуба через Entity Framework Core

Консольное приложение .NET 8 получает клиентов из PostgreSQL через Entity Framework Core и провайдер `Npgsql.EntityFrameworkCore.PostgreSQL`.

## Требования

- .NET SDK 8.0 или новее;
- PostgreSQL с базой `Fitnes` и таблицей `clients`;
- у пользователя PostgreSQL права на чтение таблицы `clients`.

Если база еще не восстановлена, перед запуском можно восстановить предоставленный custom-дамп PostgreSQL:

```bash
createdb -U postgres Fitnes
pg_restore -U postgres --dbname=Fitnes --clean --if-exists Fitnes.backup
```

Путь к `Fitnes.backup` укажите фактический для вашей машины. Команды `createdb` и `pg_restore` входят в клиентские утилиты PostgreSQL.

## Запуск

Клонируйте репозиторий и перейдите в его каталог:

```bash
git clone <URL-репозитория>
cd github-gitlab-entity-framework-core-ef
```

Задайте строку подключения переменной окружения. В PowerShell:

```powershell
$env:ConnectionStrings__Fitness = "Host=localhost;Port=5432;Database=Fitnes;Username=postgres;Password=ВАШ_ПАРОЛЬ"
dotnet restore
dotnet run
```

В Linux/macOS:

```bash
export ConnectionStrings__Fitness='Host=localhost;Port=5432;Database=Fitnes;Username=postgres;Password=ВАШ_ПАРОЛЬ'
dotnet restore
dotnet run
```

Также поддерживается переменная `FITNESS_CONNECTION_STRING`.

## Что делает программа

1. Показывает всех клиентов.
2. Показывает активных клиентов, отсортированных по фамилии.
3. Запрашивает ID и показывает найденного клиента либо сообщение, что клиент не найден.

Все запросы выполняются через EF Core и LINQ; SQL вручную в приложении не используется. Соответствие свойств модели столбцам `client_id`, `first_name`, `last_name`, `status` задано в `FitnessContext`.

Файл `Fitnes.backup` не включается в репозиторий: это дамп базы данных, содержащий данные и настройки конкретного окружения. Его можно восстановить средствами PostgreSQL перед запуском приложения.

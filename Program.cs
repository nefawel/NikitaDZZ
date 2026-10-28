using FitnessClients.Data;
using Microsoft.EntityFrameworkCore;

var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Fitness")
    ?? Environment.GetEnvironmentVariable("FITNESS_CONNECTION_STRING");

if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.Error.WriteLine("Не задана строка подключения. Установите переменную ConnectionStrings__Fitness.");
    Console.Error.WriteLine("Пример: Host=localhost;Port=5432;Database=Fitnes;Username=postgres;Password=...;");
    return 1;
}

var options = new DbContextOptionsBuilder<FitnessContext>()
    .UseNpgsql(connectionString)
    .Options;

await using var db = new FitnessContext(options);

try
{
    Console.WriteLine("Все клиенты:");
    var clients = await db.Clients
        .AsNoTracking()
        .OrderBy(client => client.ClientId)
        .ToListAsync();

    PrintClients(clients);

    Console.WriteLine("\nАктивные клиенты (по фамилии):");
    var activeClients = await db.Clients
        .AsNoTracking()
        .Where(client => client.Status.ToLower() == "active")
        .OrderBy(client => client.LastName)
        .ThenBy(client => client.FirstName)
        .ToListAsync();

    PrintClients(activeClients);

    Console.Write("\nВведите ID клиента: ");
    var input = Console.ReadLine();
    if (!int.TryParse(input, out var clientId))
    {
        Console.WriteLine("ID должен быть целым числом.");
        return 0;
    }

    var client = await db.Clients
        .AsNoTracking()
        .SingleOrDefaultAsync(item => item.ClientId == clientId);

    if (client is null)
    {
        Console.WriteLine($"Клиент с ID {clientId} не найден.");
    }
    else
    {
        Console.WriteLine("Найденный клиент:");
        PrintClients([client]);
    }

    return 0;
}
catch (Exception exception) when (IsDatabaseException(exception))
{
    Console.Error.WriteLine($"Не удалось получить данные из PostgreSQL: {exception.Message}");
    return 1;
}

static bool IsDatabaseException(Exception exception) =>
    exception is DbUpdateException or Npgsql.NpgsqlException ||
    exception.InnerException is Npgsql.NpgsqlException;

static void PrintClients(IEnumerable<FitnessClients.Models.Client> clients)
{
    foreach (var client in clients)
    {
        Console.WriteLine($"ID: {client.ClientId}; Имя: {client.FirstName}; Фамилия: {client.LastName}; Статус: {client.Status}");
    }
}

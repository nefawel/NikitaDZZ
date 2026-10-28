namespace FitnessClients.Models;

public sealed class Client
{
    public int ClientId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;
}

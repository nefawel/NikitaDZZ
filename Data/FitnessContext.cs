using FitnessClients.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessClients.Data;

public sealed class FitnessContext(DbContextOptions<FitnessContext> options) : DbContext(options)
{
    public DbSet<Client> Clients => Set<Client>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("clients");
            entity.HasKey(client => client.ClientId);

            entity.Property(client => client.ClientId).HasColumnName("client_id");
            entity.Property(client => client.FirstName).HasColumnName("first_name");
            entity.Property(client => client.LastName).HasColumnName("last_name");
            entity.Property(client => client.Status).HasColumnName("status");
        });
    }
}

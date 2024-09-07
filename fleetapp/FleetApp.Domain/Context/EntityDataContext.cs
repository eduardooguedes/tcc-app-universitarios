using Dashdine.Domain.Entitys.Views;
using Dashdine.Domain.Entitys;
using Microsoft.EntityFrameworkCore;

namespace Dashdine.Domain.Context;

public class EntityDataContext : EntityContext
{
    public EntityDataContext(DbContextOptions<EntityContext> options) : base(options) { }

    public virtual DbSet<EstabelecimentoProximoView> EstabelecimentosProximosView { get; set; }
    public virtual DbSet<PedidoDoClienteView> PedidosDoClienteView { get; set; }
    public virtual DbSet<Timezone> Timezone { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Timezone>(entity =>
        {
            entity.HasNoKey();
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => base.OnConfiguring(optionsBuilder);
}
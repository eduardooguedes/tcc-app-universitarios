using Dashdine.Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dashdine.Domain.Context;

public class BaseContext
{
    public EntityDataContext NovaConexao(IConfiguration configuration)
    {
        string? config = configuration["ConnectionsStrings:ServerConnection"];

        DbContextOptionsBuilder<EntityContext> optionsBuilder = new();
        optionsBuilder.UseNpgsql(config);        
        return new EntityDataContext(optionsBuilder.Options);
    }
}

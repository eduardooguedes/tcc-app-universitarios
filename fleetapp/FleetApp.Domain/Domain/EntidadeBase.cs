namespace Dashdine.Domain.Domain;

public abstract class EntidadeBase
{
    public Guid Id { get; set; }

    public EntidadeBase()
    {
        Id = Guid.NewGuid();
    }
}

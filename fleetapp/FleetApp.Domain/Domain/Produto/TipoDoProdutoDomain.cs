namespace Dashdine.Domain.Domain.Produto;

public class TipoDoProdutoDomain
{
    public int Id { get; }
    public string Descricao { get; }

    public TipoDoProdutoDomain(int id, string descricao)
    {
        Id = id;
        Descricao = descricao;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj is not TipoDoProdutoDomain) return false;

        return this.Id.Equals(((TipoDoProdutoDomain)obj).Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
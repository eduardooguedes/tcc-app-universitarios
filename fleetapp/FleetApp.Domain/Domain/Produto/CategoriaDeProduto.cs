namespace Dashdine.Domain.Domain.Produto;

public class CategoriaDeProduto
{
    public int Id { get; }
    public string Descricao { get; }

    public CategoriaDeProduto(int id, string descricao)
    {
        Id = id;
        Descricao = descricao;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj is not CategoriaDeProduto) return false;

        return this.Id.Equals(((CategoriaDeProduto)obj).Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}

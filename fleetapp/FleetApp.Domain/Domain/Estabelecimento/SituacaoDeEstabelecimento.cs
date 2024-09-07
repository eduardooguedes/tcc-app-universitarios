namespace Dashdine.Domain.Domain.Estabelecimento
{
    public class SituacaoDeEstabelecimento(int id, string descricao)
    {
        public int Id { get; } = id;
        public string Descricao { get; } = descricao;

        public static List<SituacaoDeEstabelecimento> Lista
        {
            get
            {
                return [
                    Novo,
                    Ativo,
                    Inativo
                ];
            }
        }

        public static SituacaoDeEstabelecimento ObterPorIdOuInativo(int id) => Lista.Find(x => x.Id == id) ?? SituacaoDeEstabelecimento.Inativo;

        public static SituacaoDeEstabelecimento Novo { get { return new SituacaoDeEstabelecimento(1, "Novo"); } private set { } }
        public static SituacaoDeEstabelecimento Ativo { get { return new SituacaoDeEstabelecimento(2, "Ativo"); } private set { } }
        public static SituacaoDeEstabelecimento Inativo { get { return new SituacaoDeEstabelecimento(3, "Inativo"); } private set { } }
    }
}

namespace Dashdine.Domain.Domain.Estabelecimento.Gestor
{
    public class SituacaoDeGestor(int id, string descricao)
    {
        public int Id { get; } = id;
        public string Descricao { get; } = descricao;

        public static List<SituacaoDeGestor> Lista
        {
            get
            {
                return
                [
                    Novo,
                    Ativo,
                    Inativo
                ];
            }
        }

        public static SituacaoDeGestor ObterPorIdOuNovo(int id) => Lista.Find(x => x.Id == id) ?? SituacaoDeGestor.Novo;

        public static SituacaoDeGestor Novo { get { return new SituacaoDeGestor(1, "Novo"); } private set { } }
        public static SituacaoDeGestor Ativo { get { return new SituacaoDeGestor(2, "Ativo"); } private set { } }
        public static SituacaoDeGestor Inativo { get { return new SituacaoDeGestor(3, "Inativo"); } private set { } }
    }
}

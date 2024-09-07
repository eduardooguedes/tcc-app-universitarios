namespace Dashdine.Domain.Domain.Cliente
{
    public class SituacaoDeCliente
    {
        public int Id { get; private set; }
        public string Descricao { get; private set; }

        public SituacaoDeCliente(int id, string descricao)
        {
            Id = id;
            Descricao = descricao;
        }

        public static List<SituacaoDeCliente> Lista
        {
            get
            {
                List<SituacaoDeCliente> lista = new()
                {
                    Novo,
                    Ativo,
                    Inativo
                };
                return lista.ToList();
            }
        }

        public static SituacaoDeCliente ObterPorIdOuNovo(int id)
        {
            SituacaoDeCliente? situacao = Lista.FirstOrDefault(x => x.Id == id);
            if (situacao == null)
                situacao = SituacaoDeCliente.Novo;

            return situacao;
        }

        public static SituacaoDeCliente Novo { get { return new SituacaoDeCliente(1, "Novo"); } private set { } }
        public static SituacaoDeCliente Ativo { get { return new SituacaoDeCliente(2, "Ativo"); } private set { } }
        public static SituacaoDeCliente Inativo { get { return new SituacaoDeCliente(3, "Inativo"); } private set { } }
    }
}

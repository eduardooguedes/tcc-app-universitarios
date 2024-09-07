using Dashdine.CrossCutting.Senha;

namespace Dashdine.Domain.Domain.Estabelecimento.Gestor
{
    public class Gestor : EntidadeBase
    {
        public DateTime DataHoraCadastro { get; private set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string CPF { get; private set; }
        public DateOnly DataDeNascimento { get; set; }
        public string Email { get; private set; }
        public bool EmailConfirmado { get; private set; }
        public string SenhaCriptografada { get; private set; }
        public EstabelecimentoDomain? Estabelecimento { get; private set; } = null;
        public SituacaoDeGestor Situacao { get; set; }

        public Gestor(string nome, string sobrenome, string cpf, DateOnly dataDeNascimento, string email, string senha)
        {
            Nome = nome;
            Sobrenome = sobrenome;
            CPF = cpf;
            DataDeNascimento = dataDeNascimento;
            DataHoraCadastro = DateTime.Now;
            Email = email;
            Situacao = SituacaoDeGestor.Novo;
            SenhaCriptografada = SenhaUsuario.CriptografarSenhaUsuario(senha);
        }

        public Gestor(Guid id, SituacaoDeGestor situacaoGestor, DateTime dataHoraCadastro, string nome, string sobrenome, string cpf,
            DateOnly dataDeNascimento, string email, bool emailConfirmado, string senhaCriptografada)
        {
            Id = id;
            Situacao = situacaoGestor;
            DataHoraCadastro = dataHoraCadastro;
            Nome = nome;
            Sobrenome = sobrenome;
            CPF = cpf;
            DataDeNascimento = dataDeNascimento;
            Email = email;
            EmailConfirmado = emailConfirmado;
            SenhaCriptografada = senhaCriptografada;
            Situacao = situacaoGestor;
        }

        public Gestor(Guid id, EstabelecimentoDomain? estabelecimento, SituacaoDeGestor situacaoGestor, DateTime dataHoraCadastro, string nome, string sobrenome, string cpf,
            DateOnly dataDeNascimento, string email, bool emailConfirmado, string senhaCriptografada)
            : this(id, situacaoGestor, dataHoraCadastro, nome, sobrenome, cpf, dataDeNascimento, email, emailConfirmado, senhaCriptografada)
        {
            Estabelecimento = estabelecimento;
        }

        public bool SenhaEhValida(string senha)
        {
            return SenhaUsuario.SenhaDoUsuarioEhValida(senha, this.SenhaCriptografada);
        }

        public void RecuperarSenha(string novaSenha)
        {
            if (Situacao.Id == SituacaoDeGestor.Novo.Id)
                Situacao = SituacaoDeGestor.Ativo;

            this.AtualizarSenha(novaSenha);
        }

        public void AtualizarSenha(string novaSenha)
        {
            SenhaCriptografada = SenhaUsuario.CriptografarSenhaUsuario(novaSenha);
        }
    }
}

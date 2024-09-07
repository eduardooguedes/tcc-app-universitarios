using Dashdine.CrossCutting.Senha;

namespace Dashdine.Domain.Domain.Cliente
{
    public sealed class ClienteDomain : EntidadeBase
    {
        public SituacaoDeCliente Situacao { get; private set; }
        public DateTime DataHoraCadastro { get; }
        public string Nome { get; }
        public string Sobrenome { get; }
        public string CPF { get; }
        public DateOnly DataDeNascimento { get; }
        public string Email { get; }
        public bool EmailConfirmado { get; }
        public string Celular { get; }
        public bool CelularConfirmado { get; }
        public string SenhaCriptografada { get; private set; }

        public ClienteDomain(string nome, string sobrenome, string cpf, DateOnly dataDeNascimento, string email, string celular, string senha)
        {
            Situacao = SituacaoDeCliente.Novo;
            DataHoraCadastro = DateTime.Now;
            Nome = nome;
            Sobrenome = sobrenome;
            CPF = cpf;
            DataDeNascimento = dataDeNascimento;
            Email = email;
            Celular = celular;
            SenhaCriptografada = SenhaUsuario.CriptografarSenhaUsuario(senha);
        }

        public ClienteDomain(Guid id, SituacaoDeCliente situacaoDeCliente, DateTime dataHoraCadastro, string nome, string sobrenome, string cpf,
            DateOnly dataDeNascimento, string email, bool emailConfirmado, string celular, bool celularConfirmado, string senhaCriptografada)
        {
            Id = id;
            Situacao = situacaoDeCliente;
            DataHoraCadastro = dataHoraCadastro;
            Nome = nome;
            Sobrenome = sobrenome;
            CPF = cpf;
            DataDeNascimento = dataDeNascimento;
            Email = email;
            EmailConfirmado = emailConfirmado;
            Celular = celular;
            CelularConfirmado = celularConfirmado;
            SenhaCriptografada = senhaCriptografada;
        }

        public bool SenhaEhValida(string senha) => SenhaUsuario.SenhaDoUsuarioEhValida(senha, this.SenhaCriptografada);

        public void AtualizarSenha(string novaSenha) => SenhaCriptografada = SenhaUsuario.CriptografarSenhaUsuario(novaSenha);

        public void RecuperarSenha(string novaSenha)
        {
            if (Situacao.Id == SituacaoDeCliente.Novo.Id)
                Situacao = SituacaoDeCliente.Ativo;

            this.AtualizarSenha(novaSenha);
        }
    }
}

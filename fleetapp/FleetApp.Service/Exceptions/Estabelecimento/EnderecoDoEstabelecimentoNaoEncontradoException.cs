namespace Dashdine.Service.Exceptions.Estabelecimento;

internal class EnderecoDoEstabelecimentoNaoEncontradoException : ServiceException
{
    public EnderecoDoEstabelecimentoNaoEncontradoException() : base("Endereço do estabelecimento não encontrado.") { }
}

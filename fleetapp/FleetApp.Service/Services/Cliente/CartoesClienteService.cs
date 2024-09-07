using Dashdine.CrossCutting.Enums;
using Dashdine.Domain.Domain.Cliente.Cartao;
using Dashdine.Domain.Domain.Usuario.Autenticacao;
using Dashdine.Domain.Interface.Cliente;
using Dashdine.Service.Exceptions;
using Dashdine.Service.Exceptions.Cliente;
using Dashdine.Service.Extensions;
using Dashdine.Service.Interface.Cliente;
using Dashdine.Service.Models.Cliente;
using Dashdine.Service.Validacoes;

namespace Dashdine.Service.Services.Cliente;

public class CartoesClienteService(IClienteRepository clienteRepository, ICartoesClienteRepository cartoesClienteRepository, IEnderecoClienteRepository enderecoClienteRepository) : ICartoesClienteService
{
    public async Task<IEnumerable<ProjecaoDeCartaoDoCliente>> ObterLista(UsuarioAutenticado cliente)
    {
        await ValidarCliente(cliente.Email);
        return (await cartoesClienteRepository.ObterLista(cliente.Id))
                .Select(cartao => cartao.AsProjecao(ObterImagem(cartao.Bandeira)));
    }

    private async Task ValidarCliente(string emailCliente)
    {
        if (await clienteRepository.ObterPorEmail(emailCliente) is null) throw new ClienteNaoEncontradoException();
    }

    private static void ValidarTipoDoCartao(string? tipo)
    {
        if (string.IsNullOrEmpty(tipo))
            throw new TipoDoCartaoNaoAceitoException();

        if (!Enum.TryParse(typeof(EnumTipoDeCartao), tipo, out var _))
            throw new TipoDoCartaoNaoAceitoException();
    }

    private static void ValidarCpfOuCnpj(string cpfOuCnpjDoTitular)
    {
        if (ValidacaoCPF.ValidarCPF(cpfOuCnpjDoTitular))
            return;

        if (!ValidacaoCNPJ.ValidarCNPJ(cpfOuCnpjDoTitular))
            throw new CpfOuCnpjInvalidoException(cpfOuCnpjDoTitular);
    }

    private void ValidarEnderecoDeCobranca(string idEnderecoCobranca)
    {
        if (!Guid.TryParse(idEnderecoCobranca, out var idParsed))
            throw new EnderecoDeCobrancaInvalidoException();

        if (!enderecoClienteRepository.Queryable().Any(e => e.Id.Equals(idParsed) && e.IdTipoEnderecoNavigation.Cobranca))
            throw new EnderecoDeCobrancaInvalidoException();
    }

    public async Task<ProjecaoDeCartaoDoCliente> Adicionar(UsuarioAutenticado usuarioAutenticado, DtoDeCartaoDoCliente dto)
    {
        await ValidarCliente(usuarioAutenticado.Email);
        ValidarTipoDoCartao(dto.Tipo);
        ValidarCpfOuCnpj(dto.Titular.CpfOuCnpjDoTitular);
        ValidarEnderecoDeCobranca(dto.IdEnderecoCobranca);

        //TODO - SALVAR CARTÃO EM TERCEIRO
        //ARMAZENAR CODIGO DO TERCEIRO NO BANCO DA APLICACAO

        var bandeiras = new List<string> { "Mastercard", "Visa", "Hipercard", "Elo" };
        var bandeira = bandeiras[new Random().Next(bandeiras.Count)];

        CartaoDomain cartao = new(Guid.NewGuid(), usuarioAutenticado.Id, dto.Apelido, dto.Numero[^4..], (EnumTipoDeCartao)(Enum.Parse(typeof(EnumTipoDeCartao), dto.Tipo!)), dto.Validade, bandeira, Guid.Parse(dto.IdEnderecoCobranca));
        await cartoesClienteRepository.AdicionarResumoDoCartao(cartao);
        return cartao.AsProjecao(ObterImagem(cartao.Bandeira));
    }

    private static string? ObterImagem(string bandeira)
    {
        if (bandeira == "Mastercard")
            return "https://dashdine-estabelecimento-2.s3.amazonaws.com/formasdepagamento/master-card.png";
        if (bandeira == "Visa")
            return "https://dashdine-estabelecimento-2.s3.amazonaws.com/formasdepagamento/visa.png";
        return null;
    }

    public async Task Atualizar(UsuarioAutenticado usuarioAutenticado, Guid idCartao, DtoDeCartaoDoClienteEdicao dto)
    {
        await ValidarCliente(usuarioAutenticado.Email);
        var cartao = await cartoesClienteRepository.Obter(idCartao) ?? throw new CartaoNaoEncontradoException();

        if (!cartao.IdCliente.Equals(usuarioAutenticado.Id))
            throw new CartaoNaoEncontradoException();

        await cartoesClienteRepository.AtualizarCartao(cartao with { Apelido = dto.Apelido });
    }

    public async Task Excluir(UsuarioAutenticado usuarioAutenticado, Guid idCartao)
    {
        await ValidarCliente(usuarioAutenticado.Email);
        var cartao = await cartoesClienteRepository.Obter(idCartao) ?? throw new CartaoNaoEncontradoException();

        if (!cartao.IdCliente.Equals(usuarioAutenticado.Id))
            throw new CartaoNaoEncontradoException();

        //TODO - REMOVER CARTÃO EM TERCEIRO
        await cartoesClienteRepository.Excluir(idCartao);
    }
}
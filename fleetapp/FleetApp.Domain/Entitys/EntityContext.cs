using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Dashdine.Domain.Entitys;

public partial class EntityContext : DbContext
{
    public EntityContext(DbContextOptions<EntityContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Adicional> Adicionals { get; set; }

    public virtual DbSet<AdicionalProduto> AdicionalProdutos { get; set; }

    public virtual DbSet<AdicionalProdutoPedido> AdicionalProdutoPedidos { get; set; }

    public virtual DbSet<CartaoCliente> CartaoClientes { get; set; }

    public virtual DbSet<CartaoClienteGateway> CartaoClienteGateways { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<ClienteGateway> ClienteGateways { get; set; }

    public virtual DbSet<DestinoPedido> DestinoPedidos { get; set; }

    public virtual DbSet<EnderecoCliente> EnderecoClientes { get; set; }

    public virtual DbSet<EnderecoEstabelecimento> EnderecoEstabelecimentos { get; set; }

    public virtual DbSet<Estabelecimento> Estabelecimentos { get; set; }

    public virtual DbSet<EstabelecimentoGateway> EstabelecimentoGateways { get; set; }

    public virtual DbSet<Gestor> Gestors { get; set; }

    public virtual DbSet<HorariosFuncionamento> HorariosFuncionamentos { get; set; }

    public virtual DbSet<Pagamento> Pagamentos { get; set; }

    public virtual DbSet<Parametro> Parametros { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Produto> Produtos { get; set; }

    public virtual DbSet<ProdutoPedido> ProdutoPedidos { get; set; }

    public virtual DbSet<SituacaoCliente> SituacaoClientes { get; set; }

    public virtual DbSet<SituacaoEstabelecimento> SituacaoEstabelecimentos { get; set; }

    public virtual DbSet<SituacaoGestor> SituacaoGestors { get; set; }

    public virtual DbSet<SituacaoHorariosFuncionamento> SituacaoHorariosFuncionamentos { get; set; }

    public virtual DbSet<SituacaoPagamento> SituacaoPagamentos { get; set; }

    public virtual DbSet<SituacaoPedido> SituacaoPedidos { get; set; }

    public virtual DbSet<SituacaoProduto> SituacaoProdutos { get; set; }

    public virtual DbSet<TipoCartao> TipoCartaos { get; set; }

    public virtual DbSet<TipoEnderecoCliente> TipoEnderecoClientes { get; set; }

    public virtual DbSet<TipoPagamento> TipoPagamentos { get; set; }

    public virtual DbSet<TipoProduto> TipoProdutos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Adicional>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("adicional_pkey");

            entity.ToTable("adicional");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.IdEstabelecimento).HasColumnName("id_estabelecimento");
            entity.Property(e => e.IdSituacao).HasColumnName("id_situacao");
            entity.Property(e => e.Nome)
                .HasMaxLength(20)
                .HasColumnName("nome");
            entity.Property(e => e.Preco)
                .HasPrecision(4, 2)
                .HasColumnName("preco");

            entity.HasOne(d => d.IdEstabelecimentoNavigation).WithMany(p => p.Adicionals)
                .HasForeignKey(d => d.IdEstabelecimento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("adicional_id_estabelecimento_fkey");

            entity.HasOne(d => d.IdSituacaoNavigation).WithMany(p => p.Adicionals)
                .HasForeignKey(d => d.IdSituacao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("adicional_id_situacao_fkey");
        });

        modelBuilder.Entity<AdicionalProduto>(entity =>
        {
            entity.HasKey(e => new { e.IdAdicional, e.IdProduto }).HasName("adicional_produto_pkey");

            entity.ToTable("adicional_produto");

            entity.Property(e => e.IdAdicional).HasColumnName("id_adicional");
            entity.Property(e => e.IdProduto).HasColumnName("id_produto");
            entity.Property(e => e.QtdeMaxima).HasColumnName("qtde_maxima");

            entity.HasOne(d => d.IdAdicionalNavigation).WithMany(p => p.AdicionalProdutos)
                .HasForeignKey(d => d.IdAdicional)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("adicional_produto_id_adicional_fkey");

            entity.HasOne(d => d.IdProdutoNavigation).WithMany(p => p.AdicionalProdutos)
                .HasForeignKey(d => d.IdProduto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("adicional_produto_id_produto_fkey");
        });

        modelBuilder.Entity<AdicionalProdutoPedido>(entity =>
        {
            entity.HasKey(e => new { e.IdProdutoPedido, e.IdAdicionalProduto }).HasName("adicional_produto_pedido_pkey");

            entity.ToTable("adicional_produto_pedido");

            entity.Property(e => e.IdProdutoPedido).HasColumnName("id_produto_pedido");
            entity.Property(e => e.IdAdicionalProduto).HasColumnName("id_adicional_produto");
            entity.Property(e => e.PrecoTotal)
                .HasPrecision(5, 2)
                .HasColumnName("preco_total");
            entity.Property(e => e.PrecoUnitario)
                .HasPrecision(5, 2)
                .HasColumnName("preco_unitario");
            entity.Property(e => e.Quantidade).HasColumnName("quantidade");

            entity.HasOne(d => d.IdAdicionalProdutoNavigation).WithMany(p => p.AdicionalProdutoPedidos)
                .HasForeignKey(d => d.IdAdicionalProduto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("adicional_produto_pedido_id_adicional_produto_fkey");

            entity.HasOne(d => d.IdProdutoPedidoNavigation).WithMany(p => p.AdicionalProdutoPedidos)
                .HasForeignKey(d => d.IdProdutoPedido)
                .HasConstraintName("adicional_produto_pedido_id_produto_pedido_fkey");
        });

        modelBuilder.Entity<CartaoCliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cartao_cliente_pkey");

            entity.ToTable("cartao_cliente");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Apelido)
                .HasMaxLength(20)
                .HasColumnName("apelido");
            entity.Property(e => e.Bandeira)
                .HasMaxLength(20)
                .HasColumnName("bandeira");
            entity.Property(e => e.FinalDoNumero)
                .HasMaxLength(4)
                .HasColumnName("final_do_numero");
            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.IdEndereco).HasColumnName("id_endereco");
            entity.Property(e => e.IdTipo).HasColumnName("id_tipo");
            entity.Property(e => e.Validade).HasColumnName("validade");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.CartaoClientes)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cartao_cliente_id_cliente_fkey");

            entity.HasOne(d => d.IdEnderecoNavigation).WithMany(p => p.CartaoClientes)
                .HasForeignKey(d => d.IdEndereco)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cartao_cliente_id_endereco_fkey");

            entity.HasOne(d => d.IdTipoNavigation).WithMany(p => p.CartaoClientes)
                .HasForeignKey(d => d.IdTipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cartao_cliente_id_tipo_fkey");
        });

        modelBuilder.Entity<CartaoClienteGateway>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cartao_cliente_gateway_pkey");

            entity.ToTable("cartao_cliente_gateway");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.IdCartaoCliente).HasColumnName("id_cartao_cliente");
            entity.Property(e => e.IdCartaoGateway)
                .HasMaxLength(30)
                .HasColumnName("id_cartao_gateway");

            entity.HasOne(d => d.IdCartaoClienteNavigation).WithMany(p => p.CartaoClienteGateways)
                .HasForeignKey(d => d.IdCartaoCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cartao_cliente_gateway_id_cartao_cliente_fkey");
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("categoria_pkey");

            entity.ToTable("categoria");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Descricao)
                .HasMaxLength(20)
                .HasColumnName("descricao");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cliente_pkey");

            entity.ToTable("cliente");

            entity.HasIndex(e => e.Celular, "cliente_celular_key").IsUnique();

            entity.HasIndex(e => e.Cpf, "cliente_cpf_key").IsUnique();

            entity.HasIndex(e => e.Email, "cliente_email_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Celular)
                .HasMaxLength(14)
                .HasColumnName("celular");
            entity.Property(e => e.CelularConfirmado)
                .HasDefaultValue(false)
                .HasColumnName("celular_confirmado");
            entity.Property(e => e.Cpf)
                .HasMaxLength(11)
                .HasColumnName("cpf");
            entity.Property(e => e.DataHoraCadastro)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("data_hora_cadastro");
            entity.Property(e => e.DataNascimento).HasColumnName("data_nascimento");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.EmailConfirmado)
                .HasDefaultValue(false)
                .HasColumnName("email_confirmado");
            entity.Property(e => e.IdSituacao).HasColumnName("id_situacao");
            entity.Property(e => e.Nome)
                .HasMaxLength(25)
                .HasColumnName("nome");
            entity.Property(e => e.Senha)
                .HasMaxLength(128)
                .HasColumnName("senha");
            entity.Property(e => e.Sobrenome)
                .HasMaxLength(30)
                .HasColumnName("sobrenome");

            entity.HasOne(d => d.IdSituacaoNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.IdSituacao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cliente_id_situacao_fkey");
        });

        modelBuilder.Entity<ClienteGateway>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cliente_gateway_pkey");

            entity.ToTable("cliente_gateway");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.IdGateway)
                .HasMaxLength(20)
                .HasColumnName("id_gateway");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.ClienteGateways)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("cliente_gateway_id_cliente_fkey");
        });

        modelBuilder.Entity<DestinoPedido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("destino_pedido_pkey");

            entity.ToTable("destino_pedido");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Descricao)
                .HasMaxLength(14)
                .HasColumnName("descricao");
        });

        modelBuilder.Entity<EnderecoCliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("endereco_cliente_pkey");

            entity.ToTable("endereco_cliente");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Apelido)
                .HasMaxLength(20)
                .HasColumnName("apelido");
            entity.Property(e => e.Bairro)
                .HasMaxLength(15)
                .HasColumnName("bairro");
            entity.Property(e => e.Cep)
                .HasMaxLength(8)
                .HasColumnName("cep");
            entity.Property(e => e.Cidade)
                .HasMaxLength(28)
                .HasColumnName("cidade");
            entity.Property(e => e.Complemento)
                .HasMaxLength(15)
                .HasColumnName("complemento");
            entity.Property(e => e.Estado)
                .HasMaxLength(2)
                .HasColumnName("estado");
            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.IdTipoEndereco).HasColumnName("id_tipo_endereco");
            entity.Property(e => e.Latitude)
                .HasPrecision(10, 6)
                .HasColumnName("latitude");
            entity.Property(e => e.Logradouro)
                .HasMaxLength(45)
                .HasColumnName("logradouro");
            entity.Property(e => e.Longitude)
                .HasPrecision(10, 6)
                .HasColumnName("longitude");
            entity.Property(e => e.Numero).HasColumnName("numero");
            entity.Property(e => e.Principal).HasColumnName("principal");
            entity.Property(e => e.Timezone)
                .HasMaxLength(20)
                .HasColumnName("timezone");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.EnderecoClientes)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("endereco_cliente_id_cliente_fkey");

            entity.HasOne(d => d.IdTipoEnderecoNavigation).WithMany(p => p.EnderecoClientes)
                .HasForeignKey(d => d.IdTipoEndereco)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("endereco_cliente_id_tipo_endereco_fkey");
        });

        modelBuilder.Entity<EnderecoEstabelecimento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("endereco_estabelecimento_pkey");

            entity.ToTable("endereco_estabelecimento");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Bairro)
                .HasMaxLength(15)
                .HasColumnName("bairro");
            entity.Property(e => e.Cep)
                .HasMaxLength(8)
                .HasColumnName("cep");
            entity.Property(e => e.Cidade)
                .HasMaxLength(28)
                .HasColumnName("cidade");
            entity.Property(e => e.Complemento)
                .HasMaxLength(50)
                .HasColumnName("complemento");
            entity.Property(e => e.Estado)
                .HasMaxLength(2)
                .HasColumnName("estado");
            entity.Property(e => e.IdEstabelecimento).HasColumnName("id_estabelecimento");
            entity.Property(e => e.Latitude)
                .HasPrecision(10, 6)
                .HasColumnName("latitude");
            entity.Property(e => e.Logradouro)
                .HasMaxLength(45)
                .HasColumnName("logradouro");
            entity.Property(e => e.Longitude)
                .HasPrecision(10, 6)
                .HasColumnName("longitude");
            entity.Property(e => e.Numero).HasColumnName("numero");
            entity.Property(e => e.Timezone)
                .HasMaxLength(20)
                .HasColumnName("timezone");

            entity.HasOne(d => d.IdEstabelecimentoNavigation).WithMany(p => p.EnderecoEstabelecimentos)
                .HasForeignKey(d => d.IdEstabelecimento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("endereco_estabelecimento_id_estabelecimento_fkey");
        });

        modelBuilder.Entity<Estabelecimento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("estabelecimento_pkey");

            entity.ToTable("estabelecimento");

            entity.HasIndex(e => e.Cnpj, "estabelecimento_cnpj_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Cnpj)
                .HasMaxLength(14)
                .HasColumnName("cnpj");
            entity.Property(e => e.DataHoraCadastro)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("data_hora_cadastro");
            entity.Property(e => e.IdSituacao).HasColumnName("id_situacao");
            entity.Property(e => e.Logo).HasColumnName("logo");
            entity.Property(e => e.NomeFantasia)
                .HasMaxLength(35)
                .HasColumnName("nome_fantasia");
            entity.Property(e => e.RazaoSocial)
                .HasMaxLength(45)
                .HasColumnName("razao_social");
            entity.Property(e => e.Telefone)
                .HasMaxLength(13)
                .HasColumnName("telefone");

            entity.HasOne(d => d.IdSituacaoNavigation).WithMany(p => p.Estabelecimentos)
                .HasForeignKey(d => d.IdSituacao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("estabelecimento_id_situacao_fkey");
        });

        modelBuilder.Entity<EstabelecimentoGateway>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("estabelecimento_gateway_pkey");

            entity.ToTable("estabelecimento_gateway");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.IdCarteiraGateway)
                .HasMaxLength(30)
                .HasColumnName("id_carteira_gateway");
            entity.Property(e => e.IdEstabelecimento).HasColumnName("id_estabelecimento");
        });

        modelBuilder.Entity<Gestor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("gestor_pkey");

            entity.ToTable("gestor");

            entity.HasIndex(e => e.Cpf, "gestor_cpf_key").IsUnique();

            entity.HasIndex(e => e.Email, "gestor_email_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Cpf)
                .HasMaxLength(11)
                .HasColumnName("cpf");
            entity.Property(e => e.DataHoraCadastro)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("data_hora_cadastro");
            entity.Property(e => e.DataNascimento).HasColumnName("data_nascimento");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.EmailConfirmado)
                .HasDefaultValue(false)
                .HasColumnName("email_confirmado");
            entity.Property(e => e.IdEstabelecimento).HasColumnName("id_estabelecimento");
            entity.Property(e => e.IdSituacao).HasColumnName("id_situacao");
            entity.Property(e => e.Nome)
                .HasMaxLength(25)
                .HasColumnName("nome");
            entity.Property(e => e.Senha)
                .HasMaxLength(128)
                .HasColumnName("senha");
            entity.Property(e => e.Sobrenome)
                .HasMaxLength(30)
                .HasColumnName("sobrenome");

            entity.HasOne(d => d.IdEstabelecimentoNavigation).WithMany(p => p.Gestors)
                .HasForeignKey(d => d.IdEstabelecimento)
                .HasConstraintName("gestor_id_estabelecimento_fkey");

            entity.HasOne(d => d.IdSituacaoNavigation).WithMany(p => p.Gestors)
                .HasForeignKey(d => d.IdSituacao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("gestor_id_situacao_fkey");
        });

        modelBuilder.Entity<HorariosFuncionamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("horarios_funcionamento_pkey");

            entity.ToTable("horarios_funcionamento");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.DestinosDaRetirada)
                .HasMaxLength(3)
                .HasColumnName("destinos_da_retirada");
            entity.Property(e => e.Dias)
                .HasMaxLength(27)
                .HasColumnName("dias");
            entity.Property(e => e.FimHorario).HasColumnName("fim_horario");
            entity.Property(e => e.IdEstabelecimento).HasColumnName("id_estabelecimento");
            entity.Property(e => e.IdSituacao).HasColumnName("id_situacao");
            entity.Property(e => e.InicioHorario).HasColumnName("inicio_horario");
            entity.Property(e => e.IntervaloEmMinutosEntreRetiradas).HasColumnName("intervalo_em_minutos_entre_retiradas");
            entity.Property(e => e.MinutosEntrePedirERetirar).HasColumnName("minutos_entre_pedir_e_retirar");
            entity.Property(e => e.QtdePedidosPorRetirada).HasColumnName("qtde_pedidos_por_retirada");
            entity.Property(e => e.QtdePedidosPrimeiraRetirada).HasColumnName("qtde_pedidos_primeira_retirada");
            entity.Property(e => e.QtdeProdutosPorPedido).HasColumnName("qtde_produtos_por_pedido");

            entity.HasOne(d => d.IdEstabelecimentoNavigation).WithMany(p => p.HorariosFuncionamentos)
                .HasForeignKey(d => d.IdEstabelecimento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("horarios_funcionamento_id_estabelecimento_fkey");

            entity.HasOne(d => d.IdSituacaoNavigation).WithMany(p => p.HorariosFuncionamentos)
                .HasForeignKey(d => d.IdSituacao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("horarios_funcionamento_id_situacao_fkey");
        });

        modelBuilder.Entity<Pagamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pagamento_pkey");

            entity.ToTable("pagamento");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.DataHora)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("data_hora");
            entity.Property(e => e.DataHoraAtualizado)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("data_hora_atualizado");
            entity.Property(e => e.DataHoraExpiracao)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("data_hora_expiracao");
            entity.Property(e => e.IdCartao).HasColumnName("id_cartao");
            entity.Property(e => e.IdCobrancaGateway)
                .HasMaxLength(50)
                .HasColumnName("id_cobranca_gateway");
            entity.Property(e => e.IdPedido).HasColumnName("id_pedido");
            entity.Property(e => e.IdPedidoGateway)
                .HasMaxLength(50)
                .HasColumnName("id_pedido_gateway");
            entity.Property(e => e.IdQrCodeGateway)
                .HasMaxLength(50)
                .HasColumnName("id_qr_code_gateway");
            entity.Property(e => e.IdSituacao).HasColumnName("id_situacao");
            entity.Property(e => e.IdTipo).HasColumnName("id_tipo");
            entity.Property(e => e.ImagemQrCode)
                .HasMaxLength(100)
                .HasColumnName("imagem_qr_code");
            entity.Property(e => e.TextoPix)
                .HasMaxLength(280)
                .HasColumnName("texto_pix");
            entity.Property(e => e.Valor)
                .HasPrecision(5, 2)
                .HasColumnName("valor");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.Pagamentos)
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pagamento_id_pedido_fkey");

            entity.HasOne(d => d.IdSituacaoNavigation).WithMany(p => p.Pagamentos)
                .HasForeignKey(d => d.IdSituacao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pagamento_id_situacao_fkey");

            entity.HasOne(d => d.IdTipoNavigation).WithMany(p => p.Pagamentos)
                .HasForeignKey(d => d.IdTipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pagamento_id_tipo_fkey");
        });

        modelBuilder.Entity<Parametro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("parametros_pkey");

            entity.ToTable("parametros");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Descricao)
                .HasMaxLength(50)
                .HasColumnName("descricao");
            entity.Property(e => e.Valor).HasColumnName("valor");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pedido_pkey");

            entity.ToTable("pedido");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.DataHora)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("data_hora");
            entity.Property(e => e.DataHoraARetirar)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("data_hora_a_retirar");
            entity.Property(e => e.DataHoraRetirado)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("data_hora_retirado");
            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.IdDestino).HasColumnName("id_destino");
            entity.Property(e => e.IdEstabelecimento).HasColumnName("id_estabelecimento");
            entity.Property(e => e.IdGateway)
                .HasMaxLength(50)
                .HasColumnName("id_gateway");
            entity.Property(e => e.IdLocalizacaoCliente).HasColumnName("id_localizacao_cliente");
            entity.Property(e => e.IdSituacao).HasColumnName("id_situacao");
            entity.Property(e => e.Numero).HasColumnName("numero");
            entity.Property(e => e.Observacao)
                .HasMaxLength(100)
                .HasColumnName("observacao");
            entity.Property(e => e.ObservacaoEstabelecimento)
                .HasMaxLength(100)
                .HasColumnName("observacao_estabelecimento");
            entity.Property(e => e.ValorTotal)
                .HasPrecision(5, 2)
                .HasColumnName("valor_total");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pedido_id_cliente_fkey");

            entity.HasOne(d => d.IdDestinoNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdDestino)
                .HasConstraintName("pedido_id_destino_fkey");

            entity.HasOne(d => d.IdEstabelecimentoNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdEstabelecimento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pedido_id_estabelecimento_fkey");

            entity.HasOne(d => d.IdLocalizacaoClienteNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdLocalizacaoCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pedido_id_localizacao_cliente_fkey");

            entity.HasOne(d => d.IdSituacaoNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdSituacao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pedido_id_situacao_fkey");
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("produto_pkey");

            entity.ToTable("produto");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Descricao)
                .HasMaxLength(300)
                .HasColumnName("descricao");
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.IdEstabelecimento).HasColumnName("id_estabelecimento");
            entity.Property(e => e.IdSituacao).HasColumnName("id_situacao");
            entity.Property(e => e.IdTipo).HasColumnName("id_tipo");
            entity.Property(e => e.Imagem).HasColumnName("imagem");
            entity.Property(e => e.MinutosParaRetirada).HasColumnName("minutos_para_retirada");
            entity.Property(e => e.Nome)
                .HasMaxLength(25)
                .HasColumnName("nome");
            entity.Property(e => e.NotaMedia)
                .HasPrecision(3, 1)
                .HasColumnName("nota_media");
            entity.Property(e => e.Preco)
                .HasPrecision(5, 2)
                .HasColumnName("preco");
            entity.Property(e => e.QtdeVezesVendido).HasColumnName("qtde_vezes_vendido");
            entity.Property(e => e.QtdeVotos)
                .HasDefaultValue(0)
                .HasColumnName("qtde_votos");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Produtos)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("produto_id_categoria_fkey");

            entity.HasOne(d => d.IdEstabelecimentoNavigation).WithMany(p => p.Produtos)
                .HasForeignKey(d => d.IdEstabelecimento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("produto_id_estabelecimento_fkey");

            entity.HasOne(d => d.IdSituacaoNavigation).WithMany(p => p.Produtos)
                .HasForeignKey(d => d.IdSituacao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("produto_id_situacao_fkey");

            entity.HasOne(d => d.IdTipoNavigation).WithMany(p => p.Produtos)
                .HasForeignKey(d => d.IdTipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("produto_id_tipo_fkey");
        });

        modelBuilder.Entity<ProdutoPedido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("produto_pedido_pkey");

            entity.ToTable("produto_pedido");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.IdPedido).HasColumnName("id_pedido");
            entity.Property(e => e.IdProduto).HasColumnName("id_produto");
            entity.Property(e => e.Nota).HasColumnName("nota");
            entity.Property(e => e.PrecoProduto)
                .HasPrecision(5, 2)
                .HasColumnName("preco_produto");
            entity.Property(e => e.PrecoTotal)
                .HasPrecision(5, 2)
                .HasColumnName("preco_total");
            entity.Property(e => e.PrecoUnitario)
                .HasPrecision(5, 2)
                .HasColumnName("preco_unitario");
            entity.Property(e => e.Quantidade).HasColumnName("quantidade");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.ProdutoPedidos)
                .HasForeignKey(d => d.IdPedido)
                .HasConstraintName("produto_pedido_id_pedido_fkey");

            entity.HasOne(d => d.IdProdutoNavigation).WithMany(p => p.ProdutoPedidos)
                .HasForeignKey(d => d.IdProduto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("produto_pedido_id_produto_fkey");
        });

        modelBuilder.Entity<SituacaoCliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("situacao_cliente_pkey");

            entity.ToTable("situacao_cliente");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Descricao)
                .HasMaxLength(7)
                .HasColumnName("descricao");
        });

        modelBuilder.Entity<SituacaoEstabelecimento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("situacao_estabelecimento_pkey");

            entity.ToTable("situacao_estabelecimento");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Ativo).HasColumnName("ativo");
            entity.Property(e => e.Descricao)
                .HasMaxLength(7)
                .HasColumnName("descricao");
            entity.Property(e => e.Novo).HasColumnName("novo");
        });

        modelBuilder.Entity<SituacaoGestor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("situacao_gestor_pkey");

            entity.ToTable("situacao_gestor");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Descricao)
                .HasMaxLength(7)
                .HasColumnName("descricao");
        });

        modelBuilder.Entity<SituacaoHorariosFuncionamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("situacao_horarios_funcionamento_pkey");

            entity.ToTable("situacao_horarios_funcionamento");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Ativo).HasColumnName("ativo");
            entity.Property(e => e.Descricao)
                .HasMaxLength(7)
                .HasColumnName("descricao");
        });

        modelBuilder.Entity<SituacaoPagamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("situacao_pagamento_pkey");

            entity.ToTable("situacao_pagamento");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Aguardando).HasColumnName("aguardando");
            entity.Property(e => e.Cancelado).HasColumnName("cancelado");
            entity.Property(e => e.Descricao)
                .HasMaxLength(10)
                .HasColumnName("descricao");
            entity.Property(e => e.DescricaoGateway)
                .HasMaxLength(11)
                .HasColumnName("descricao_gateway");
            entity.Property(e => e.EmAnalise).HasColumnName("em_analise");
            entity.Property(e => e.EstornarAoCancelarPedido)
                .HasDefaultValue(false)
                .HasColumnName("estornar_ao_cancelar_pedido");
            entity.Property(e => e.Expirado).HasColumnName("expirado");
            entity.Property(e => e.Pago).HasColumnName("pago");
            entity.Property(e => e.Rejeitado).HasColumnName("rejeitado");
        });

        modelBuilder.Entity<SituacaoPedido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("situacao_pedido_pkey");

            entity.ToTable("situacao_pedido");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Aceito).HasColumnName("aceito");
            entity.Property(e => e.AguardandoPagamento).HasColumnName("aguardando_pagamento");
            entity.Property(e => e.ApagarPedidoAoCancelar).HasColumnName("apagar_pedido_ao_cancelar");
            entity.Property(e => e.Cancelado).HasColumnName("cancelado");
            entity.Property(e => e.CorHexadecimal)
                .HasMaxLength(6)
                .HasColumnName("cor_hexadecimal");
            entity.Property(e => e.Descricao)
                .HasMaxLength(20)
                .HasColumnName("descricao");
            entity.Property(e => e.DiminuiQuantidadeDePedidosDisponiveis).HasColumnName("diminui_quantidade_de_pedidos_disponiveis");
            entity.Property(e => e.EmAndamento).HasColumnName("em_andamento");
            entity.Property(e => e.EstornarPagamentoAoCancelar)
                .HasDefaultValue(true)
                .HasColumnName("estornar_pagamento_ao_cancelar");
            entity.Property(e => e.IntervaloMinimoEmHorasAntesDeRetirarParaCancelar).HasColumnName("intervalo_minimo_em_horas_antes_de_retirar_para_cancelar");
            entity.Property(e => e.ListadoParaCliente).HasColumnName("listado_para_cliente");
            entity.Property(e => e.ListadoParaGestor).HasColumnName("listado_para_gestor");
            entity.Property(e => e.Novo).HasColumnName("novo");
            entity.Property(e => e.PermitidoCancelar).HasColumnName("permitido_cancelar");
            entity.Property(e => e.PermitidoPagar).HasColumnName("permitido_pagar");
            entity.Property(e => e.PermitidoPedirNovamente).HasColumnName("permitido_pedir_novamente");
            entity.Property(e => e.PermitidoRejeitar).HasColumnName("permitido_rejeitar");
            entity.Property(e => e.PermitidoRetirar).HasColumnName("permitido_retirar");
            entity.Property(e => e.PermitidoSolicitarAjuda).HasColumnName("permitido_solicitar_ajuda");
            entity.Property(e => e.PossuiCategoriaPropriaParaCliente).HasColumnName("possui_categoria_propria_para_cliente");
            entity.Property(e => e.Rejeitado).HasColumnName("rejeitado");
            entity.Property(e => e.Retirado).HasColumnName("retirado");
            entity.Property(e => e.VisualizaMotivo).HasColumnName("visualiza_motivo");
        });

        modelBuilder.Entity<SituacaoProduto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("situacao_produto_pkey");

            entity.ToTable("situacao_produto");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Ativo).HasColumnName("ativo");
            entity.Property(e => e.Descricao)
                .HasMaxLength(7)
                .HasColumnName("descricao");
        });

        modelBuilder.Entity<TipoCartao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tipo_cartao_pkey");

            entity.ToTable("tipo_cartao");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Descricao)
                .HasMaxLength(7)
                .HasColumnName("descricao");
        });

        modelBuilder.Entity<TipoEnderecoCliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tipo_endereco_cliente_pkey");

            entity.ToTable("tipo_endereco_cliente");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Cobranca).HasColumnName("cobranca");
            entity.Property(e => e.Descricao)
                .HasMaxLength(10)
                .HasColumnName("descricao");
        });

        modelBuilder.Entity<TipoPagamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tipo_pagamento_pkey");

            entity.ToTable("tipo_pagamento");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CartaoCredito).HasColumnName("cartao_credito");
            entity.Property(e => e.Descricao)
                .HasMaxLength(17)
                .HasColumnName("descricao");
            entity.Property(e => e.FormaComum).HasColumnName("forma_comum");
            entity.Property(e => e.Imagem).HasColumnName("imagem");
            entity.Property(e => e.Pix).HasColumnName("pix");
        });

        modelBuilder.Entity<TipoProduto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tipo_produto_pkey");

            entity.ToTable("tipo_produto");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Descricao)
                .HasMaxLength(9)
                .HasColumnName("descricao");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

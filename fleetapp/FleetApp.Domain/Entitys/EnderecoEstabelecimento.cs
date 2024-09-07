using System;
using System.Collections.Generic;

namespace Dashdine.Domain.Entitys;

public partial class EnderecoEstabelecimento
{
    public Guid Id { get; set; }

    public Guid IdEstabelecimento { get; set; }

    public string Cep { get; set; } = null!;

    public string Logradouro { get; set; } = null!;

    public int Numero { get; set; }

    public string? Complemento { get; set; }

    public string? Bairro { get; set; }

    public string Cidade { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public string Timezone { get; set; } = null!;

    public virtual Estabelecimento IdEstabelecimentoNavigation { get; set; } = null!;
}

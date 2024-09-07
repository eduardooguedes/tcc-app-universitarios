export interface IPedido {
  id: string;
  numero: string;
  horarioDeRetirada: string;
  situacao: ISituacao;
  cliente: ICliente;
  valorTotal: number;
  produtos: IProdutoPedido[]
  observacaoDoCliente: string;
  observacaoDoEstabelecimento: string;
  permitidoAceitar: boolean;
  permitidoRejeitar: boolean;
  permitidoEntregar: boolean;
  permitidoCancelarEntrega: boolean;
}

export interface ISituacao {
  id: string;
  descricao: string;
  codigoDaCorEmHexadecimal: string;
}

export interface ICliente {
  id: string;
  nome: string;
  email: string;
  celular: string;
}

export interface IProdutoPedido {
  id: string;
  nome: string;
  quantidade: number;
  precoTotal: number;
  tipo: ITipo;
  adicionais: IProdutoAdicional[];
}

export interface ITipo {
  id: string;
  descricao: string;
}

export interface IProdutoAdicional {
  id: string;
  nome: string;
  precoTotal: number;
  quantidade: number;
}

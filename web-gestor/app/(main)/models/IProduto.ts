import { ICategoria } from "./ICategoria";
import { IProdutoSituacao } from "./IProdutoSituacao";

export interface IProduto {
  id: string;
  categoria: ICategoria;
  descricao: string;
  imagem: string;
  nome: string;
  preco: number;
  situacao: IProdutoSituacao;
  tempoEmMinutosParaRetirada: number;
  //tipo: string;
}

import { IDia } from "./IDia";

export interface IHorario {
  id: string;
  dias: IDia[];
  inicioHorario: string;
  fimHorario: string;
  intervaloEmMinutosEntreRetiradas: number;
  pedidosPorRetirada: string;
  minutosEntrePedirERetirar: number;
}

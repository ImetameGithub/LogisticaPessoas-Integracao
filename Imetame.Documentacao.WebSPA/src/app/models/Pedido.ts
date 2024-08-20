import { BaseEntity } from "./BaseEntity";
import { Credenciadora } from "./Crendenciadora";

export class Pedido extends BaseEntity{
    NumPedido: string;
    Unidade: string;
    IdCredenciadora: string;

    Credenciadora: Credenciadora
}
  
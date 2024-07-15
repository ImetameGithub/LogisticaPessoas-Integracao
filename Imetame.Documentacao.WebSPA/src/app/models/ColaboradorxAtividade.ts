import { AtividadeEspecifica } from "./AtividadeEspecifica";
import { BaseEntity } from "./BaseEntity";
import { Colaborador } from "./Colaborador";

export class ColaboradorxAtividade extends BaseEntity{    
    CXA_IDATIVIDADE_ESPECIFICA: string;
    CXA_IDCOLABORADOR: string;

    AtividadeEspecifica?: AtividadeEspecifica;
    Colaborador?: Colaborador;
}
  
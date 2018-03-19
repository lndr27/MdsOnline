using System;
using System.Collections.Generic;

namespace Lndr.MdsOnline.Web.Models.DTO.CheckList
{
    public class CheckListDTO
    {
        public CheckListDTO()
        {
            this.GruposItens = new List<CheckListGrupoItemDTO>();
        }

        public int CheckListID { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public DateTime DataCriacao { get; set; }

        public int UsuarioCriacaoID { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public int UsuarioAtualizacaoID { get; set; }

        public List<CheckListGrupoItemDTO> GruposItens { get; set; }
    }    
}
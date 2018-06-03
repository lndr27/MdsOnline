using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lndr.MdsOnline.Web.Models.ViewData.CheckList
{
    public class CheckListViewData
    {
        public CheckListViewData()
        {
            this.GruposItens = new List<CheckListGrupoItemViewData>();
        }

        public int CheckListID { get; set; }

        [Required]
        public string Nome { get; set; }

        public string Descricao { get; set; }

        public DateTime DataCriacao { get; set; }

        public int UsuarioCriacaoID { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public int UsuarioAtualizacaoID { get; set; }

        public string NomeUsuarioAtualizacao { get; set; }

        public string NomeUsuarioCriacao { get; set; }

        [Required]
        public List<CheckListGrupoItemViewData> GruposItens { get; set; }
    }    
}
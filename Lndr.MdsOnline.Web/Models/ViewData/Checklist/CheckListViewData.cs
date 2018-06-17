using System;
using System.Collections.Generic;

namespace Lndr.MdsOnline.Web.Models.ViewData.CheckList
{
    public class CheckListViewData
    {
        public CheckListViewData()
        {
            this.GruposItens = new List<CheckListGrupoItemViewData>();
        }

        public string CheckListEncryptedID { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public DateTime DataCriacao { get; set; }

        public int UsuarioCriacaoID { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public int UsuarioAtualizacaoID { get; set; }

        public List<CheckListGrupoItemViewData> GruposItens { get; set; }
    }    
}
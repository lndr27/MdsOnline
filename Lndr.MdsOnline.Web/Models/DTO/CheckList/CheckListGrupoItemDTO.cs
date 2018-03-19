using System.Collections.Generic;

namespace Lndr.MdsOnline.Web.Models.DTO.CheckList
{
    public class CheckListGrupoItemDTO
    {
        public CheckListGrupoItemDTO()
        {
            this.Itens = new List<CheckListItemDTO>();
        }

        public int CheckListGrupoItemID { get; set; }

        public int CheckListID { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public List<CheckListItemDTO> Itens { get; set; }
    }
}
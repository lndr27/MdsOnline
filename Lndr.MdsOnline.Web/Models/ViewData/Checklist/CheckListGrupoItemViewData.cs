using System.Collections.Generic;

namespace Lndr.MdsOnline.Web.Models.ViewData.CheckList
{
    public class CheckListGrupoItemViewData
    {
        public CheckListGrupoItemViewData()
        {
            this.Itens = new List<CheckListItemViewData>();
        }

        public int CheckListGrupoItemID { get; set; }

        public int CheckListID { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public List<CheckListItemViewData> Itens { get; set; }
    }
}
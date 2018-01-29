using System.Collections.Generic;

namespace Lndr.MdsOnline.Models.ViewData
{
    public class DocumentoGrupoItemCheckListViewData
    {
        public int DocumentoGrupoItemCheckListID { get; set; }

        public int DocumentoID { get; set; }

        public string Nome { get; set; }

        public IEnumerable<DocumentoItemCheckListViewData> Itens { get; set; }
    }
}
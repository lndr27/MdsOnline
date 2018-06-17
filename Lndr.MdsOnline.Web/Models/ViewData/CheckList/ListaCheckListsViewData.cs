using System.Collections.Generic;

namespace Lndr.MdsOnline.Web.Models.ViewData.CheckList
{
    public class ListaCheckListsViewData
    {
        public int TamanhoPagina { get; set; }

        public int TotalPaginas { get; set; }

        public int Pagina { get; set; }

        public bool PossuiProximaPagina { get; set; }

        public bool PossuiPaginaAnterior { get; set; }

        public int ProximaPagina { get; set; }

        public int PaginaAnterior { get; set; }

        public List<CheckListViewData> CheckLists {get; set;}
    }
}